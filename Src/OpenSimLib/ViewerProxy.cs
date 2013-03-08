using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera;
using System.Windows.Forms;
using Chimera.OpenSim.GUI;
using System.Threading;
using OpenMetaverse;
using GridProxy;
using log4net;
using System.Diagnostics;
using System.Collections;
using OpenMetaverse.Packets;
using System.ComponentModel;
using System.Net;
using GridProxyConfig = GridProxy.ProxyConfig;
using Chimera.Util;
using System.IO;

namespace Chimera.OpenSim {
    public abstract class ViewerProxy : IOutput {
        private static readonly string proxyAddress = "127.0.0.1";

        private Process mClient;
        private ILog mLogger;
        protected Proxy mProxy;
        private bool mClientLoggedIn;
        private bool mProxyStarted;
        private bool mControlCamera;
        private bool mAutoRestart;
        private UUID mSecureSessionID = UUID.Zero;
        private UUID mSessionID = UUID.Zero;
        private UUID mAgentID = UUID.Zero;
        private string mFirstName = "NotLoggedIn";
        private string mLastName = "NotLoggedIn";

        private object processLock = new object();

        private Window mWindow;
        private ProxyPanel mPanel;
        private ProxyConfig mConfig;

        /// <summary>
        /// Triggered whenever the client proxy starts up.
        /// </summary>
        public event EventHandler OnProxyStarted;

        /// <summary>
        /// Triggered whenever a client logs in to the proxy.
        /// </summary>
        public event EventHandler OnClientLoggedIn;

        /// <summary>
        /// Triggered whenever a viewer exits.
        /// </summary>
        public event EventHandler OnViewerExit;

        internal ProxyConfig ProxyConfig {
            get { return mConfig; }
        }

        public bool ProxyRunning {
            get { return mProxyStarted; }
        }

        public bool ClientLoggedIn {
            get { return mClientLoggedIn; }
        }

        public bool ControlCamera {
            get { return mControlCamera; }
            set {
                if (value != mControlCamera) {
                    mControlCamera = value;
                    if (value)
                        SetCamera();
                    else
                        ClearCamera();
                }
            }
        }

        /// <summary>
        /// The logger to use when writing to the logs.
        /// </summary>
        protected ILog Logger {
            get { return mLogger; }
            set { mLogger = value; }
        }

        internal bool StartProxy() {
            if (mConfig == null)
                throw new Exception("Unable to start proxy. No configuration specified.");
            if (mConfig.ProxyLoginURI == null)
                throw new Exception("Unable to start proxy. No login URI specified in the configuration.");
            if (mProxy != null)
                mProxy.Stop();
            string file = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;

            string portArg = "--proxy-login-port=" + mConfig.ProxyPort;
            string listenIPArg = "--proxy-proxyAddress-facing-address=" + proxyAddress;
            string loginURIArg = "--proxy-remote-login-uri=" + mConfig.ProxyLoginURI;
            string proxyCaps = "--proxy-caps=false";
            string[] args = { portArg, listenIPArg, loginURIArg, proxyCaps };
            GridProxyConfig config = new GridProxyConfig("Routing God", "jm726@st-andrews.ac.uk", args);
            try {
                mProxy = new Proxy(config);
                mProxy.AddLoginResponseDelegate(response => {
                    mClientLoggedIn = true;
                    Hashtable t = (Hashtable)response.Value;

                    if (bool.Parse(t["login"].ToString())) {
                        mSessionID = UUID.Parse(t["session_id"].ToString());
                        mSecureSessionID = UUID.Parse(t["secure_session_id"].ToString());
                        mAgentID = UUID.Parse(t["agent_id"].ToString());
                        mFirstName = t["first_name"].ToString();
                        mLastName = t["last_name"].ToString();

                        lock (processLock)
                            Monitor.PulseAll(processLock);

                        new Thread(() => {
                            if (mControlCamera)
                                SetCamera();
                            if (OnClientLoggedIn != null)
                                OnClientLoggedIn(mProxy, null);
                        }).Start();
                    } else {
                    }
                });
                foreach (PacketType pt in Enum.GetValues(typeof(PacketType))) {
                    mProxy.AddDelegate(pt, Direction.Incoming, ReceiveIncomingPacket);
                    mProxy.AddDelegate(pt, Direction.Outgoing, ReceiveOutgoingPacket);
                }

                mProxy.Start();
                mProxyStarted = true;
            } catch (NullReferenceException e) {
                Logger.Info("Unable to start proxy. " + e.Message);
                return false;
            }

            if (OnProxyStarted != null)
                OnProxyStarted(mProxy, null);

            return mProxyStarted;
        }

        protected void InjectPacket(Packet p) {
            if (ProxyRunning)
                mProxy.InjectPacket(p, Direction.Incoming);
        }

        private bool mClosing;

        public void CloseViewer() {
            if (mClientLoggedIn) {
                mClosing = true;
                ProcessWrangler.PressKey(mClient, "q", true, false, false);

                Thread shutdownThread = new Thread(() => {
                    int i = 0;
                    while (mClientLoggedIn && i++ < 10) {
                        lock (processLock)
                            Monitor.Wait(processLock, 1000);
                        if (mClientLoggedIn)
                            ProcessWrangler.PressKey(mClient, "q", true, false, false);
                    }
                });
                shutdownThread.Name = "Viewer Shutdown Thread.";
                shutdownThread.Start();
            }
        }

        private void mClient_Exited(object sender, EventArgs e) {
            if (!mClosing) {
                Console.WriteLine("Viewer shutdown unexpectedly");
                string dump = "Viewer crashed at " + DateTime.Now.ToString("u") + Environment.NewLine;
                dump += " Login: " + mFirstName + " " + mLastName + Environment.NewLine;
                dump += " Exe: " + mConfig.ViewerExecutable + Environment.NewLine;
                dump += " Dir: " + mConfig.ViewerWorkingDirectory + Environment.NewLine + Environment.NewLine;
                dump += "Log: " + Environment.NewLine;
                foreach (string line in File.ReadAllLines("C:\\Users\\Iain\\AppData\\Roaming\\Firestorm\\logs\\Firestorm.log"))
                    dump += line + Environment.NewLine;
                dump += Environment.NewLine + Environment.NewLine + "---------------- End of Viewer Crash report -------------" + Environment.NewLine + Environment.NewLine;

                File.AppendAllText(mConfig.CrashLogFile, dump);
            }
            mClosing = false;
            mClientLoggedIn = false;
            if (OnViewerExit != null)
                OnViewerExit(this, null);
            lock (processLock)
                Monitor.PulseAll(processLock);
            if (mAutoRestart)
                Launch();
        }

        /// <summary>
        /// Return control of the camera to the viewer.
        /// </summary>
        public abstract void ClearCamera();

        /// <summary>
        /// Take control of the camera and set it to the position specified by the coordinator.
        /// </summary>
        public abstract void SetCamera();

        /// <summary>
        /// Called whenever the camera position is updated.
        /// </summary>
        /// <param name="coordinator">The coordinator which triggered the camera change.</param>
        /// <param name="args">The arguments about the change that was made.</param>
        protected abstract void ProcessChange(Coordinator coordinator, CameraUpdateEventArgs args);

        /// <summary>
        /// Called whenever the eye position is updated.
        /// </summary>
        /// <param name="coordinator">The coordinator which triggered the eye change.</param>
        /// <param name="args">The arguments about the change that was made.</param>
        protected abstract void ProcessEyeUpdate(Coordinator coordinator, EventArgs args);

        /// <summary>
        /// Called whenever a packet is received from the client.
        /// </summary>
        /// <param name="p">The packet that was received.</param>
        /// <param name="ep">The end point the packet was received from.</param>
        /// <returns>The packet which is to be forwarded on to the server.</returns>
        protected virtual Packet ReceiveOutgoingPacket(Packet p, IPEndPoint ep) { return p; }

        /// <summary>
        /// Called whenever a packet is received from the server.
        /// </summary>
        /// <param name="p">The packet that was received.</param>
        /// <param name="ep">The end point the packet was received from.</param>
        /// <returns>The packet which is to be forwarded on to the client.</returns>
        protected virtual Packet ReceiveIncomingPacket(Packet p, IPEndPoint ep) { return p; }

        #region IOutput Members

        public bool AutoRestart {
            get { return mAutoRestart; }
            set { mAutoRestart = value; }
        }
        public Window Window {
            get { return mWindow; }
        }

        public UserControl ConfigPanel {
            get {
                if (mPanel == null)
                    mPanel = new ProxyPanel(this);
                return mPanel;
            }
        }

        public string State {
            get {
                string dump = "-Viewer Proxy-" + Environment.NewLine;
                if (mProxyStarted) {
                    dump += "Running:" + Environment.NewLine;
                    dump += " Proxy: localhost:" + mConfig.ProxyPort + Environment.NewLine;
                    dump += " Endpoint: " + mConfig.ProxyLoginURI + Environment.NewLine;
                } else
                    dump += "Not running" + Environment.NewLine;

                if (mClientLoggedIn) {
                    dump += "Logged In:" + Environment.NewLine;
                    dump += " Login: " + mFirstName + " " + mLastName + Environment.NewLine;
                    dump += " Exe: " + mConfig.ViewerExecutable + Environment.NewLine;
                    dump += " Dir: " + mConfig.ViewerWorkingDirectory + Environment.NewLine;
                } else
                    dump += "Not logged in" + Environment.NewLine;

                return dump;
            }
        }

        public ViewerProxy(params string[] args) {
            mConfig = new ProxyConfig(args);
        }

        public ViewerProxy(string name, params string[] args) {
            mConfig = new ProxyConfig(name, AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, args);
        }

        public void Init(Window window) {
            mLogger = LogManager.GetLogger(mConfig.Name);
            mWindow = window;
            mWindow.Coordinator.CameraUpdated += ProcessChange;
            mWindow.Coordinator.EyeUpdated += ProcessEyeUpdate;

            if (mConfig.AutoStartViewer)
                Launch();
            else if (mConfig.AutoStartProxy)
                StartProxy();

            AutoRestart = mConfig.AutoRestartViewer;
            ControlCamera = mConfig.ControlCamera;
        }

        public bool Launch() {
            if (mConfig == null)
                throw new Exception("Unable to start client. No configuration specified.");
            if (mConfig.ViewerExecutable == null)
                throw new Exception("Unable to start client. No executable specified.");
            if (!ProxyRunning) 
                StartProxy();

            mClient = new Process();
            string args = "--channel \"Firestorm-Release\" --settings settings_firestorm_release_v4.xml --set InstallLanguage en ";
            if (mConfig.UseGrid)
                args += "--grid " + mConfig.LoginGrid;
            else
                args += "--loginuri http://localhost:" + mConfig.ProxyPort;
            if (mConfig.AutoLoginClient)
                args += " --login " + mConfig.LoginFirstName + " " + mConfig.LoginLastName + " " + mConfig.LoginPassword;
            mClient = ProcessWrangler.InitProcess(mConfig.ViewerExecutable, mConfig.ViewerWorkingDirectory, args);
            mClient.EnableRaisingEvents = true;
            try {
                Logger.Info("Starting client:\n\"" + mClient.StartInfo.FileName + "\" " + mClient.StartInfo.Arguments);
                if (!mClient.Start())
                    return false;

                mClient.Exited += new EventHandler(mClient_Exited);
                return mClientLoggedIn;

            } catch (Win32Exception e) {
                Logger.Info("Unable to start client from " + mClient.StartInfo.FileName + ". " + e.Message);
                return false;
            }
        }

        public void Close() {
            mAutoRestart = false;
            if (mProxyStarted)
                mProxy.Stop();
            if (mClientLoggedIn)
                CloseViewer();
            mProxyStarted = false;
        }

        #endregion

        internal void CycleMonitor() {
            if (mClientLoggedIn)
                ProcessWrangler.SwitchWindowMonitor(mClient);
        }
    }
}

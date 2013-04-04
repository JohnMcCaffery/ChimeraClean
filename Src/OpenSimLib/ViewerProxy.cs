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
using Nwc.XmlRpc;

namespace Chimera.OpenSim {
    public abstract class ViewerProxy : IOutput, IInput {
        private static readonly string proxyAddress = "127.0.0.1";

        private Process mClient;
        private ILog mLogger;
        protected Proxy mProxy;
        private bool mClientLoggedIn;
        private bool mControlCamera;
        private bool mAutoRestart;
        private UUID mSecureSessionID = UUID.Zero;
        private UUID mSessionID = UUID.Zero;
        private UUID mAgentID = UUID.Zero;
        private string mFirstName = "NotLoggedIn";
        private string mLastName = "NotLoggedIn";
        private bool mFullscreen;
        private bool mEnabled;
        private bool mMaster;

        private object processLock = new object();

        private SetFollowCamProperties mFollowCamProperties;
        private Window mWindow;
        private OutputPanel mOutputPanel;
        private InputPanel mInputPanel;
        private ProxyConfig mConfig;

        /// <summary>
        /// Selected whenever the client proxy starts up.
        /// </summary>
        public event EventHandler OnProxyStarted;

        /// <summary>
        /// Selected whenever a client logs in to the proxy.
        /// </summary>
        public event EventHandler OnClientLoggedIn;

        /// <summary>
        /// Selected whenever a viewer exits.
        /// </summary>
        public event EventHandler OnViewerExit;

        internal ProxyConfig ProxyConfig {
            get { return mConfig; }
        }

        internal bool Fullscreen {
            get { return mFullscreen; }
            set { 
                mFullscreen = value;
                if (mClientLoggedIn && mClient != null) {
                    ProcessWrangler.SetBorder(mClient, mWindow.Monitor, !value);
                    ToggleHUD();
                }
            }
        }

        public bool ProxyRunning {
            get { return mProxy != null; }
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

        internal void ToggleHUD() {
            if (mClient != null)
                ProcessWrangler.PressKey(mClient, mConfig.ViewerToggleHUDKey);
        }

        internal bool StartProxy() {
            if (mConfig == null)
                throw new Exception("Unable to start proxy. No configuration specified.");
            if (mConfig.ProxyLoginURI == null)
                throw new Exception("Unable to start proxy. No login URI specified in the configuration.");
            if (mProxy != null)
                CloseProxy();
            string file = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;

            string portArg = "--proxy-login-port=" + mConfig.ProxyPort;
            string listenIPArg = "--proxy-proxyAddress-facing-address=" + proxyAddress;
            string loginURIArg = "--proxy-remote-login-uri=" + mConfig.ProxyLoginURI;
            string proxyCaps = "--proxy-caps=false";
            string[] args = { portArg, listenIPArg, loginURIArg, proxyCaps };
            GridProxyConfig config = new GridProxyConfig("Routing God", "jm726@st-andrews.ac.uk", args);
            try {
                mProxy = new Proxy(config);
                mProxy.AddLoginResponseDelegate(mProxy_LoginResponse);
                mProxy.AddDelegate(PacketType.AgentUpdate, Direction.Outgoing, mProxy_AgentUpdatePacketReceived);
                //foreach (PacketType pt in Enum.GetValues(typeof(PacketType))) {
                    //mProxy.AddDelegate(pt, Direction.Incoming, ReceiveIncomingPacket);
                    //mProxy.AddDelegate(pt, Direction.Outgoing, ReceiveOutgoingPacket);
                //}

                mProxy.Start();
                if (mFollowCamProperties != null)
                    mFollowCamProperties.SetProxy(mProxy);
            } catch (NullReferenceException e) {
                Logger.Info("Unable to start proxy. " + e.Message);
                mProxy = null;
                return false;
            }

            if (OnProxyStarted != null)
                OnProxyStarted(mProxy, null);

            return true;
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
                    while (mClientLoggedIn && i++ < 5) {
                        lock (processLock)
                            Monitor.Wait(processLock, 10000);
                        if (mClientLoggedIn) {
                            //ProcessWrangler.PressKey(mClient, "{ENTER}");
                            ProcessWrangler.PressKey(mClient, "q", true, false, false);
                        }
                    }
                });
                shutdownThread.Name = "Viewer Shutdown Thread.";
                shutdownThread.Start();
            }
        }

        private void mProxy_LoginResponse(XmlRpcResponse response) {
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

                //TODO - get client process if not started through GUI
                if (mClient != null) {
                    ProcessWrangler.SetMonitor(mClient, mWindow.Monitor);
                    if (mFullscreen)
                        ProcessWrangler.SetBorder(mClient, mWindow.Monitor, !mFullscreen);
                }

                new Thread(() => {
                    if (mControlCamera)
                        SetCamera();
                    if (OnClientLoggedIn != null)
                        OnClientLoggedIn(mProxy, null);

                    Thread.Sleep(5000);
                    if (mFullscreen)
                        ToggleHUD();
                    //mManager.Overlay.ForegroundOverlay();
                }).Start();
            } else {
            }
        }

        private void mClient_Exited(object sender, EventArgs e) {
            bool unexpected = !mClosing;
            if (unexpected) {
                Console.WriteLine("Viewer shutdown unexpectedly");
                string dump = "Viewer crashed at " + DateTime.Now.ToString("u") + Environment.NewLine;
                dump += " Login: " + mFirstName + " " + mLastName + Environment.NewLine;
                dump += " Exe: " + mConfig.ViewerExecutable + Environment.NewLine;
                dump += " Dir: " + mConfig.ViewerWorkingDirectory + Environment.NewLine + Environment.NewLine;
                dump += "Log: " + Environment.NewLine;
                string username = Environment.UserName;
                foreach (string line in File.ReadAllLines("C:\\Users\\" + username + "\\AppData\\Roaming\\Firestorm\\logs\\Firestorm.log"))
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
            if (mAutoRestart && unexpected)
                Launch();
        }

        private void mWindow_MonitorChanged(Window window, Screen monitor) {
            if (mClientLoggedIn && mClient != null)
                ProcessWrangler.SetMonitor(mClient, monitor);
        }

        private void ProcessChangeViewer(Coordinator coordinator, CameraUpdateEventArgs args) {
            if (coordinator.ControlMode == ControlMode.Absolute || !mMaster)
                ProcessChange(coordinator, args);
        }

        /// <summary>
        /// Return control of the camera to the viewer.
        /// </summary>
        public abstract void ClearCamera();

        /// <summary>
        /// Take control of the camera and set it to the position specified by the input.
        /// </summary>
        public abstract void SetCamera();

        /// <summary>
        /// Called whenever the camera position is updated.
        /// </summary>
        /// <param name="input">The input which triggered the camera change.</param>
        /// <param name="args">The arguments about the change that was made.</param>
        protected abstract void ProcessChange(Coordinator coordinator, CameraUpdateEventArgs args);

        /// <summary>
        /// Called whenever the eye position is updated.
        /// </summary>
        /// <param name="input">The input which triggered the eye change.</param>
        /// <param name="args">The arguments about the change that was made.</param>
        protected abstract void ProcessEyeUpdate(Coordinator coordinator, EventArgs args);

        /// <summary>
        /// Called whenever a packet is received transition the client.
        /// </summary>
        /// <param name="p">The packet that was received.</param>
        /// <param name="ep">The end point the packet was received transition.</param>
        /// <returns>The packet which is to be forwarded on to the server.</returns>
        protected virtual Packet ReceiveOutgoingPacket(Packet p, IPEndPoint ep) { return p; }

        /// <summary>
        /// Called whenever a packet is received transition the server.
        /// </summary>
        /// <param name="p">The packet that was received.</param>
        /// <param name="ep">The end point the packet was received transition.</param>
        /// <returns>The packet which is to be forwarded on to the client.</returns>
        protected virtual Packet ReceiveIncomingPacket(Packet p, IPEndPoint ep) { return p; }

        private void CloseProxy() {
            if (mProxy != null) {
                mProxy.Stop();
                mProxy = null;
                if (mFollowCamProperties != null)
                    mFollowCamProperties.SetProxy(mProxy);
            }
        }

        #region IOutput Members

        public bool Active {
            get { return mClientLoggedIn; }
        }

        public Process Process {
            get { return mClient; }
        }

        public bool AutoRestart {
            get { return mAutoRestart; }
            set { mAutoRestart = value; }
        }

        public string Type {
            get { return "Virtual World Viewer"; }
        }

        public Window Window {
            get { return mWindow; }
        }

        UserControl IOutput.ControlPanel {
            get {
                if (mOutputPanel == null)
                    mOutputPanel = new OutputPanel(this);
                return mOutputPanel;
            }
        }

        public string State {
            get {
                string dump = "-Viewer Proxy-" + Environment.NewLine;
                if (mProxy != null) {
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

        public ViewerProxy(params string[] args)
            : this(false, args) {
        }

        public ViewerProxy(bool master, params string[] args) {
            mConfig = new ProxyConfig(args);
            mMaster = master || mConfig.Master;
        }

        public ViewerProxy(string name, params string[] args)
            : this(name, false, args) {
        }

        public ViewerProxy(string name, bool master, params string[] args)
            : this(master, args) {
            mConfig = new ProxyConfig(name, AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, args);
            mMaster = master || mConfig.Master;
        }

        public void Init(Window window) {
            if (mMaster)
                mFollowCamProperties = new SetFollowCamProperties(window.Coordinator);

            mLogger = LogManager.GetLogger(mConfig.Name);
            mWindow = window;
            mWindow.Coordinator.CameraUpdated += ProcessChangeViewer;
            mWindow.Coordinator.EyeUpdated += ProcessEyeUpdate;
            mWindow.Coordinator.Tick += new Action(Coordinator_Tick);
            mWindow.Coordinator.CameraModeChanged += new Action<Coordinator,ControlMode>(Coordinator_CameraModeChanged);
            mWindow.MonitorChanged += new Action<Chimera.Window,Screen>(mWindow_MonitorChanged);
            mWindow.Changed += new Action<Chimera.Window,EventArgs>(mWindow_Changed);
            mFullscreen = mConfig.Fullscreen;

            if (mConfig.AutoStartViewer)
                Launch();
            else if (mConfig.AutoStartProxy)
                StartProxy();

            AutoRestart = mConfig.AutoRestartViewer;
            ControlCamera = mConfig.ControlCamera;
        }

        void mWindow_Changed(Window w, EventArgs args) {
            SetCamera();
        }

        void Coordinator_Tick() {
            if (mClientLoggedIn && mClient != null && DateTime.Now.Minute % 5 == 0 && DateTime.Now.Second % 60 == 0)
                ProcessWrangler.PressKey(mClient, "{ENTER}");
        }

        public bool Launch() {
            if (mConfig == null)
                throw new Exception("Unable to start client. No configuration specified.");
            if (mConfig.ViewerExecutable == null)
                throw new Exception("Unable to start client. No executable specified.");
            if (!ProxyRunning) 
                StartProxy();

            mClient = new Process();
            string args = mConfig.ViewerArguments + " ";
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
                Logger.Info("Unable to start client " + mClient.StartInfo.FileName + ". " + e.Message);
                mClient = null;
                return false;
            }
        }

        public void Close() {
            mAutoRestart = false;
            CloseProxy();
            if (mClientLoggedIn)
                CloseViewer();
        }

        public void Restart() {
            if (mClientLoggedIn) {
                /*
                ProcessWrangler.PressKey(mClient, "q", true, false, false);

                Thread shutdownThread = new Thread(() => {
                    int i = 0;
                    while (mClientLoggedIn && i++ < 5) {
                        lock (processLock)
                            Monitor.Wait(processLock, 3000);
                        if (mClientLoggedIn) {
                            ProcessWrangler.PressKey(mClient, "{ENTER}");
                            ProcessWrangler.PressKey(mClient, "q", true, false, false);
                        }
                    }
                });*/
                CloseViewer();
                Thread.Sleep(1000);
                CloseProxy();
                Thread.Sleep(1000);
                Launch();
            }
        }

        #endregion

        #region IInput Members

        public event Action<IInput, bool> EnabledChanged;

        UserControl IInput.ControlPanel {
            get {
                if (mInputPanel == null)
                    mInputPanel = new InputPanel(mFollowCamProperties);
                return mInputPanel;
            }
        }

        public bool Enabled {
            get { return mEnabled; }
            set {
                if (mEnabled != value) {
                    mEnabled = value;
                    if (EnabledChanged != null)
                        EnabledChanged(this, value);
                }
            }
        }

        public string Name {
            get { return "Master Client"; }
        }

        string IInput.State {
            get { throw new NotImplementedException(); }
        }

        public ConfigBase Config {
            get { return mConfig; }
        }

        public void Draw(Func<Vector3, System.Drawing.Point> to2D, System.Drawing.Graphics graphics, Action redraw) {
            //Do nothing
        }

        #endregion

        private Packet mProxy_AgentUpdatePacketReceived(Packet p, IPEndPoint ep) {
            if (mMaster && mWindow.Coordinator.ControlMode == ControlMode.Delta) {
                AgentUpdatePacket packet = p as AgentUpdatePacket;
                mWindow.Coordinator.Update(packet.AgentData.CameraCenter, Vector3.Zero, new Rotation(packet.AgentData.CameraAtAxis), Rotation.Zero, ControlMode.Absolute);
            }
            return p;
        }

        private void Coordinator_CameraModeChanged(Coordinator coordinator, ControlMode mode) {
            if (mMaster)
                if (mode == ControlMode.Delta)
                    ClearCamera();
                else
                    SetCamera();
        }
    }
}

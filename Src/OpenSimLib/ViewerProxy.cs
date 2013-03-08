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

namespace Chimera.OpenSim {
    public abstract class ViewerProxy : IOutput {
        private static readonly string proxyAddress = "127.0.0.1";

        private Process mClient;
        private ILog mLogger;
        protected Proxy mProxy;
        private bool mClientLoggedIn;
        private bool mProxyStarted;
        private bool mControlCamera;
        private UUID mSecureSessionID = UUID.Zero;
        private UUID mSessionID = UUID.Zero;
        private UUID mAgentID = UUID.Zero;
        private string mFirstName = "NotLoggedIn";
        private string mLastName = "NotLoggedIn";

        private object startLock = new object();

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

                        lock (startLock)
                            Monitor.PulseAll(startLock);

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

        /// <summary>
        /// Return control of the camera to the viewer.
        /// </summary>
        public abstract void ClearCamera();

        /// <summary>
        /// Take control of the camera and set it to the position specified by the coordinator.
        /// </summary>
        public abstract void SetCamera();

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
        public ViewerProxy(params string[] args) {
            mConfig = new ProxyConfig(args);
            InitFromConfig();
        }

        public ViewerProxy(string name, params string[] args) {
            mConfig = new ProxyConfig(name, AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, args);
            InitFromConfig();
        }

        private void InitFromConfig() {
            mLogger = LogManager.GetLogger(mConfig.Name);
            if (mConfig.AutoStartViewer)
                Launch();
            else if (mConfig.AutoStartProxy)
                StartProxy();
        }

        public virtual void Init(Window window) {
            mWindow = window;
        }

        public bool Launch() {
            if (mConfig == null)
                throw new Exception("Unable to start client. No configuration specified.");
            if (mConfig.ViewerExecutable == null)
                throw new Exception("Unable to start client. No executable specified.");
            if (!ProxyRunning) 
                StartProxy();

            mClient = new Process();
            mClient.StartInfo.FileName = mConfig.ViewerExecutable;
            mClient.StartInfo.WorkingDirectory = mConfig.ViewerWorkingDirectory;
            if (mConfig.UseGrid)
                mClient.StartInfo.Arguments = "--grid " + mConfig.LoginGrid;
            else
                mClient.StartInfo.Arguments = "--loginuri http://localhost:" + mConfig.ProxyPort;
            if (mConfig.AutoLoginClient)
                mClient.StartInfo.Arguments += " --login " + mConfig.LoginFirstName + " " + mConfig.LoginLastName + " " + mConfig.LoginPassword;
            try {
                Logger.Info("Starting client '" + mClient.StartInfo.FileName + "' with arguments '" + mClient.StartInfo.Arguments + "'.");
                if (!mClient.Start())
                    return false;

                return mClientLoggedIn;

            } catch (Win32Exception e) {
                Logger.Info("Unable to start client from " + mClient.StartInfo.FileName + ". " + e.Message);
                return false;
            }
        }

        public void Close() {
            if (mProxyStarted)
                mProxy.Stop();
            mProxyStarted = false;
        }

        #endregion
    }
}

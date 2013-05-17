using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GridProxy;
using System.Threading;
using Nwc.XmlRpc;
using GridProxyConfig = GridProxy.ProxyConfig;
using OpenMetaverse.Packets;
using Chimera.Util;
using OpenMetaverse;
using System.Net;
using System.Collections;

namespace Chimera.OpenSim {
    internal abstract class ProxyControllerBase {
        private readonly Window mWindow;
        private Proxy mProxy;
        private PacketDelegate mAgentUpdateListener;
        private UUID mSecureSessionID = UUID.Zero;
        private UUID mSessionID = UUID.Zero;
        private UUID mAgentID = UUID.Zero;
        private string mFirstName = "NotLoggedIn";
        private string mLastName = "NotLoggedIn";
        private string mLoginURI;
        private GridProxyConfig mConfig;

        /// <summary>
        /// Selected whenever the client proxy starts up.
        /// </summary>
        public event Action ProxyStarted;

        private event Action<Vector3, Rotation> pPositionChanged;

        internal event Action<Vector3, Rotation> PositionChanged {
            add {
                if (pPositionChanged == null && Started)
                    mProxy.AddDelegate(PacketType.AgentUpdate, Direction.Outgoing, mAgentUpdateListener);
                pPositionChanged += value;
            }
            remove {
                pPositionChanged -= value;
                if (pPositionChanged == null && Started)
                    mProxy.RemoveDelegate(PacketType.AgentUpdate, Direction.Outgoing, mAgentUpdateListener);
            }
        }

        /// <summary>
        /// Selected whenever a client logs in to the proxy.
        /// </summary>
        internal event EventHandler OnClientLoggedIn;

        public Proxy Proxy {
            get { return mProxy; }
        }

        public bool Started {
            get { return mProxy != null; }
        }

        public string LoginURI {
            get { return mLoginURI; }
        }

        protected Window Window {
            get { return mWindow; }
        }

        internal ProxyControllerBase(Window window) {
            mWindow = window;
            mAgentUpdateListener = new PacketDelegate(mProxy_AgentUpdatePacketReceived);
        }

        internal bool StartProxy(int port, string loginURI) {
            if (Started)
                Stop();
            string file = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;

            string localAddress = "127.0.0.1";
            string portArg = "--proxy-login-port=" + port;
            string listenIPArg = "--proxy-proxyAddress-facing-address=" + localAddress;
            string loginURIArg = "--proxy-remote-login-uri=" + loginURI;
            string proxyCaps = "--proxy-caps=false";
            string[] args = { portArg, listenIPArg, loginURIArg, proxyCaps };
            mConfig = new GridProxyConfig("Routing God", "jm726@st-andrews.ac.uk", args);
            mLoginURI = localAddress + ":" + port;

            return Start();
        }

        internal bool Start() {
            if (mConfig == null)
                throw new ArgumentException("Unable to start proxy. No configuration specified.");
            try {
                mProxy = new Proxy(mConfig);
                mProxy.AddLoginResponseDelegate(mProxy_LoginResponse);
                mProxy.AddDelegate(PacketType.AgentUpdate, Direction.Outgoing, mProxy_AgentUpdatePacketReceived);

                mProxy.Start();

                if (pPositionChanged != null)
                    mProxy.AddDelegate(PacketType.AgentUpdate, Direction.Outgoing, mAgentUpdateListener);

                if (ProxyStarted != null)
                    ProxyStarted();
            } catch (NullReferenceException e) {
                //Logger.Info("Unable to start proxy. " + e.Message);
                mProxy = null;
                return false;
            }

            return true;
        }

        internal void Stop() {
            if (mProxy != null) {
                mProxy.Stop();
                mProxy = null;
            }
        }

        internal void Chat(string message, int channel) {            if (Started) {
                ChatFromViewerPacket p = new ChatFromViewerPacket();
                p.ChatData.Channel = channel;
                p.ChatData.Message = Utils.StringToBytes(message);
                p.ChatData.Type = (byte)1;
                p.AgentData.AgentID = mAgentID;
                p.AgentData.SessionID = mSessionID;
                mProxy.InjectPacket(p, Direction.Outgoing);
            }
        }

        void mProxy_LoginResponse(XmlRpcResponse response) {
            new Thread(() => {
                Hashtable t = (Hashtable)response.Value;

                if (bool.Parse(t["login"].ToString())) {
                    mSessionID = UUID.Parse(t["session_id"].ToString());
                    mSecureSessionID = UUID.Parse(t["secure_session_id"].ToString());
                    mAgentID = UUID.Parse(t["agent_id"].ToString());
                    mFirstName = t["first_name"].ToString();
                    mLastName = t["last_name"].ToString();

                    Thread.Sleep(50);
                    if (OnClientLoggedIn != null)
                        OnClientLoggedIn(mProxy, null);
                } else {
                }
            }).Start();
        }
    
        Packet mProxy_AgentUpdatePacketReceived(Packet p, IPEndPoint ep) {
            AgentUpdatePacket packet = p as AgentUpdatePacket;
            if (pPositionChanged != null)
                pPositionChanged(packet.AgentData.CameraCenter, new Rotation(packet.AgentData.CameraAtAxis));
            return p;
        }

        protected void InjectPacket(Packet p) {
            if (Started)
                mProxy.InjectPacket(p, Direction.Incoming);
        }

        
        public abstract void SetCamera();
        public abstract void SetCamera(Vector3 positionDelta, Rotation orientationDelta);
        /// <summary>
        /// Set the view frustum on the viewer. Specify whether to control the position of the camer at the same time.
        /// </summary>
        /// <param name="setCamera">If true, the position of the camera will be set along with the new frustum. If false, only the frustum will be set.</param>
        public abstract void SetFrustum(bool setCamera);
        public abstract void Move(Vector3 positionDelta, Rotation orientationDelta, float scale);

        public abstract void ClearCamera();
        public abstract void ClearFrustum();
        public abstract void ClearMovement();
    }
}

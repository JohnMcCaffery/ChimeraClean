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
        private Proxy mProxy;
        private PacketDelegate mAgentUpdateListener;        private UUID mSecureSessionID = UUID.Zero;
        private UUID mSessionID = UUID.Zero;
        private UUID mAgentID = UUID.Zero;
        private string mFirstName = "NotLoggedIn";
        private string mLastName = "NotLoggedIn";

        private event Action<Vector3, Rotation> pPositionChanged;

        internal event Action<Vector3, Rotation> PositionChanged {
            add {
                if (pPositionChanged == null && mProxy != null)
                    mProxy.AddDelegate(PacketType.AgentUpdate, Direction.Outgoing, mAgentUpdateListener);
                pPositionChanged += value;
            }
            remove {
                pPositionChanged -= value;
                if (pPositionChanged == null && mProxy != null)
                    mProxy.RemoveDelegate(PacketType.AgentUpdate, Direction.Outgoing, mAgentUpdateListener);
            }
        }
        /// <summary>
        /// Selected whenever a client logs in to the proxy.
        /// </summary>
        internal event EventHandler OnClientLoggedIn;

        internal ProxyControllerBase() {
            mAgentUpdateListener = new PacketDelegate(mProxy_AgentUpdatePacketReceived);
        }

        internal bool StartProxy(int port, int address, int loginURI) {
            if (mProxy != null)
                Close();
            string file = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;

            string portArg = "--proxy-login-port=" + port;
            string listenIPArg = "--proxy-proxyAddress-facing-address=" + address;
            string loginURIArg = "--proxy-remote-login-uri=" + loginURI;
            string proxyCaps = "--proxy-caps=false";
            string[] args = { portArg, listenIPArg, loginURIArg, proxyCaps };
            GridProxyConfig config = new GridProxyConfig("Routing God", "jm726@st-andrews.ac.uk", args);
            try {
                mProxy = new Proxy(config);
                mProxy.AddLoginResponseDelegate(mProxy_LoginResponse);
                mProxy.AddDelegate(PacketType.AgentUpdate, Direction.Outgoing, mProxy_AgentUpdatePacketReceived);

                mProxy.Start();

                if (pPositionChanged != null)
                    mProxy.AddDelegate(PacketType.AgentUpdate, Direction.Outgoing, mAgentUpdateListener);

                //TODO - do this in wrapper class
                //if (mFollowCamProperties != null)
                    //mFollowCamProperties.SetProxy(mProxy);
            } catch (NullReferenceException e) {
                //Logger.Info("Unable to start proxy. " + e.Message);
                mProxy = null;
                return false;
            }

            //TODO - do this in wrapper class
            //if (OnProxyStarted != null)
                //OnProxyStarted(mProxy, null);

            return true;
        }
        internal void Close() {
            if (mProxy != null) {
                mProxy.Stop();
                mProxy = null;
            }
        }

        void mProxy_LoginResponse(XmlRpcResponse response) {
            Hashtable t = (Hashtable)response.Value;

            if (bool.Parse(t["login"].ToString())) {
                mSessionID = UUID.Parse(t["session_id"].ToString());
                mSecureSessionID = UUID.Parse(t["secure_session_id"].ToString());
                mAgentID = UUID.Parse(t["agent_id"].ToString());
                mFirstName = t["first_name"].ToString();
                mLastName = t["last_name"].ToString();

                new Thread(() => {
                    Thread.Sleep(50);
                    if (OnClientLoggedIn != null)
                        OnClientLoggedIn(mProxy, null);
                }).Start();
            } else {
            }
        }
    
        Packet mProxy_AgentUpdatePacketReceived(Packet p, IPEndPoint ep) {
            AgentUpdatePacket packet = p as AgentUpdatePacket;
            if (pPositionChanged != null)
                pPositionChanged(packet.AgentData.CameraCenter, new Rotation(packet.AgentData.CameraAtAxis));
            return p;
        }
    }
}

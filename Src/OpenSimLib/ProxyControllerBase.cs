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
using log4net;

namespace Chimera.OpenSim {
    public abstract class ProxyControllerBase {
        private static ProxyControllerPacketThread CameraThread = null;
        private readonly ILog ThisLogger;
        private readonly Frame mFrame;
        private Proxy mProxy;
        private PacketDelegate mAgentUpdateListener;
        private DateTime mLastUpdatePacket = DateTime.Now;
        private UUID mSecureSessionID = UUID.Zero;
        private UUID mSessionID = UUID.Zero;
        private UUID mAgentID = UUID.Zero;
        private string mFirstName = "NotLoggedIn";
        private string mLastName = "NotLoggedIn";
        private string mLoginURI;
        private bool mLoggedIn = false;
        private GridProxyConfig mConfig;
        private Vector3 mOffset;

        private ViewerConfig mViewerConfig;

        private Packet mCameraPacket;

        private readonly Dictionary<string, DateTime> mUnackedUpdates = new Dictionary<string, DateTime>();
        private int mUnackedCountThresh = 40;
        private long mUnackedDiscardMS = 1000;

        private Vector3 mPositionOffset;
        private Vector3 mAvatarPosition;
        private Rotation mAvatarOrientation;
        private bool mTrackingAvatarPosition;

        public DateTime LastUpdatePacket {
            get { return mLastUpdatePacket; }
            set { mLastUpdatePacket = value; }
        }
        public Vector3 Offset {
            get { return mOffset; }
            set {
                mOffset = value;
                if (Started)
                    SetCamera();
            }
        }

        public Vector3 PositionOffset {
            get { return mPositionOffset; }
        }

        public Vector3 AvatarPosition {
            get {
                if (!LoggedIn)
                    return Vector3.Zero;

                if (!mTrackingAvatarPosition)
                    InitAvatarTracking();

                return mAvatarPosition;
            }
        }

        public Rotation AvatarOrientation {
            get {
                if (!LoggedIn)
                    return Rotation.Zero;

                if (!mTrackingAvatarPosition)
                    InitAvatarTracking();

                return mAvatarOrientation;
            }
        }

        private void InitAvatarTracking() {
            mTrackingAvatarPosition = true;
            mAvatarPosition = Vector3.Zero;
            mAvatarOrientation = Rotation.Zero;
            mProxy.AddDelegate(PacketType.ImprovedTerseObjectUpdate, Direction.Incoming, mProxy_ImprovedTerseListener);
        }

        private Packet mProxy_ImprovedTerseListener(Packet p, IPEndPoint ep) {
            ImprovedTerseObjectUpdatePacket packet = p as ImprovedTerseObjectUpdatePacket;

            //if (packet.ObjectData[0].Data[0x5] == 1 && (new UUID(packet.ObjectData[0].Data, 0)) == mAgentID) {
            if (packet.ObjectData[0].Data[0x5] != 0/* && (new UUID(packet.ObjectData[0].Data, 0)) == mAgentID*/) {
                mAvatarPosition = new Vector3(packet.ObjectData[0].Data, 0x16);
                mPositionOffset = mAvatarPosition - mFrame.Core.Position;
                Quaternion rotation = Quaternion.Identity;

                // Rotation (theta)
                rotation = new Quaternion(
                    Utils.UInt16ToFloat(packet.ObjectData[0].Data, 0x2E, -1.0f, 1.0f),
                    Utils.UInt16ToFloat(packet.ObjectData[0].Data, 0x2E + 2, -1.0f, 1.0f),
                    Utils.UInt16ToFloat(packet.ObjectData[0].Data, 0x2E + 4, -1.0f, 1.0f),
                    Utils.UInt16ToFloat(packet.ObjectData[0].Data, 0x2E + 6, -1.0f, 1.0f));

                //mAvatarOrientation = new Rotation(rotation);
                mAvatarOrientation = Frame.Core.Orientation;
            }

            return p;
        }

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
        public event EventHandler OnClientLoggedIn;

        public Proxy Proxy {
            get { return mProxy; }
        }

        public bool Started {
            get { return mProxy != null; }
        }

        public string LoginURI {
            get { return mLoginURI; }
        }

        protected Frame Frame {
            get { return mFrame; }
        }

        public bool LoggedIn {
            get { return mLoggedIn; }
        }

        internal ProxyControllerBase(Frame frame) {
            mFrame = frame;

            ThisLogger = LogManager.GetLogger("OpenSim." + mFrame.Name + "Proxy");
            mViewerConfig = new ViewerConfig(frame.Name);
            if (mViewerConfig.UseThread) {
                if (CameraThread == null)
                    CameraThread = new ProxyControllerPacketThread(frame.Core, this);
                else
                    CameraThread.AddController(this);
            }

            mAgentUpdateListener = new PacketDelegate(mProxy_AgentUpdatePacketReceived);
        }

        public bool StartProxy(int port, string loginURI) {
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
            mLoginURI = " --loginuri http://" + localAddress + ":" + port;

            return Start();
        }

        public bool Start() {
            if (mConfig == null)
                throw new ArgumentException("Unable to start proxy. No configuration specified.");
            try {
                mLoggedIn = false;
                mLastUpdatePacket = DateTime.Now;
                mProxy = new Proxy(mConfig);
                mProxy.AddLoginResponseDelegate(mProxy_LoginResponse);
                mProxy.AddDelegate(PacketType.AgentUpdate, Direction.Outgoing, mProxy_AgentUpdatePacketReceived);

                ThisLogger.Info("Proxying " + mConfig.remoteLoginUri);
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

        public void Stop() {
            if (mViewerConfig.UseThread)
                CameraThread.Stop();

            if (mProxy != null) {
                ThisLogger.Debug("Closing");
                mProxy.Stop();
                mProxy = null;
            }
            ThisLogger.Info("Closed");
        }

        public void Chat(string message, int channel) {
            if (Started) {
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

                if (t.ContainsKey("login") && bool.Parse(t["login"].ToString())) {
                    mSessionID = UUID.Parse(t["session_id"].ToString());
                    mSecureSessionID = UUID.Parse(t["secure_session_id"].ToString());
                    mAgentID = UUID.Parse(t["agent_id"].ToString());
                    mFirstName = t["first_name"].ToString();
                    mLastName = t["last_name"].ToString();

                    mLastUpdatePacket = DateTime.Now;
                    mLoggedIn = true;

                    Thread.Sleep(50);
                    if (OnClientLoggedIn != null)
                        OnClientLoggedIn(mProxy, null);
                } else if (t.ContainsKey("faultstring")) {
                    ThisLogger.Warn("Unable to parse login response. " + t["faultstring"]);
                }
            }).Start();
        }

        Packet mProxy_AgentUpdatePacketReceived(Packet p, IPEndPoint ep) {
            AgentUpdatePacket packet = p as AgentUpdatePacket;
            Vector3 pos = packet.AgentData.CameraCenter;
            mLastUpdatePacket = DateTime.Now;
            //Console.WriteLine("Recieved agent update packet. " + mLastUpdatePacket);

            if (mFrame.Core.ControlMode == ControlMode.Absolute) {
                //new Thread(() => {
                if (mViewerConfig.CheckForPause) {
                    string key = MakeKey(pos);
                    lock (mUnackedUpdates) {
                        if (mUnackedUpdates.ContainsKey(key))
                            mUnackedUpdates.Remove(key);
                    }

                    CheckForPause();
                }
                //}).Start();
            }

            if (pPositionChanged != null)
                pPositionChanged(pos, new Rotation(packet.AgentData.CameraAtAxis));
            return p;
        }

        private string MakeKey(Vector3 v) {
            return String.Format("{0},{1},{2}", Math.Round(v.X), Math.Round(v.Y), Math.Round(v.Z));
        }

        private void CheckForPause() {
            Queue<KeyValuePair<string, DateTime>> q;
            lock (mUnackedUpdates)
                q = new Queue<KeyValuePair<string, DateTime>>(mUnackedUpdates.OrderBy(u => u.Value));
            while (q.Count > 0 && DateTime.Now.Subtract(q.Peek().Value).TotalMilliseconds > mUnackedDiscardMS)
                mUnackedUpdates.Remove(q.Dequeue().Key);

            if (mUnackedUpdates.Count > mUnackedCountThresh) {
                ThisLogger.Debug("Freeze detected. " + mUnackedUpdates.Count + " unacked packets.");
                mUnackedUpdates.Clear();
                ClearCamera();
                SetCamera();
            }
        }

        public void InjectPacket(Packet p) {
            if (Started)
                mProxy.InjectPacket(p, Direction.Incoming);
        }

        public void SetCamera() {
            if (mFrame.Core.ControlMode == ControlMode.Absolute)
                MarkUntracked();

            if (mViewerConfig.CheckForPause && mViewerConfig.UseThread) {
                //PrintTickInfo();
                Packet p = ActualSetCamera();
                lock (this)
                    mCameraPacket = p;
            } else if (mProxy != null)
                mProxy.InjectPacket(ActualSetCamera(), Direction.Incoming);
        }
        public void SetCamera(Vector3 positionDelta, Rotation orientationDelta) {
            if (mViewerConfig.CheckForPause && mFrame.Core.ControlMode == ControlMode.Absolute)
                MarkUntracked();

            //PrintTickInfo();
            if (mViewerConfig.UseThread) {
                Packet p = ActualSetCamera(positionDelta, orientationDelta);
                lock (this)
                    mCameraPacket = p;
            } else
                mProxy.InjectPacket(ActualSetCamera(positionDelta, orientationDelta), Direction.Incoming);
        }


        private DateTime mLastUpdate = DateTime.Now;
        private double mTotalMS;
        private double mUpdates;
        private bool mFirstSet;

        private void PrintTickInfo() {
            if (!mFirstSet) {
                if (DateTime.Now.Subtract(mLastUpdate).TotalSeconds > 30.0) {
                    mFirstSet = true;
                    mLastUpdate = DateTime.Now;
                }
                return;
            }
            if (mFirstName == "Master") {
                double diff = DateTime.Now.Subtract(mLastUpdate).TotalMilliseconds;
                mTotalMS += diff;
                mUpdates++;
                double mean = mTotalMS / mUpdates;
                if (Math.Abs(mean - diff) > 1.0)
                    ThisLogger.Debug(String.Format("Unexpected update. Mean: {0:0.#} - Tick: {1:0.#}.", mean, diff));
                mLastUpdate = DateTime.Now;
            }
        }

        private void MarkUntracked() {
            string str = MakeKey(mFrame.Core.Position);
            lock (mUnackedUpdates)
                if (mUnackedUpdates.ContainsKey(str))
                    mUnackedUpdates[str] = DateTime.Now;
                else
                    mUnackedUpdates.Add(str, DateTime.Now);
        }

        protected abstract Packet ActualSetCamera();
        protected abstract Packet ActualSetCamera(Vector3 positionDelta, Rotation orientationDelta);
        /// <summary>
        /// Set the view frustum on the viewer. Specify whether to control the position of the camer at the same time.
        /// </summary>
        /// <param name="setCamera">If true, the position of the camera will be set along with the new frustum. If false, only the frustum will be set.</param>
        public abstract void SetFrustum(bool setCamera);
        public abstract void Move(Vector3 positionDelta, Rotation orientationDelta, float scale);

        public abstract void ClearCamera();
        public abstract void ClearFrustum();
        public abstract void ClearMovement();

        internal void UpdateCamera() {
            if (mProxy != null && mCameraPacket != null)
                lock (this) {
                    mProxy.InjectPacket(mCameraPacket, Direction.Incoming);
                    mCameraPacket = null;
                }
        }
    }
}

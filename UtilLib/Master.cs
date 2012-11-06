using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GridProxy;
using OpenMetaverse.Packets;
using System.Net;
using OpenMetaverse;

namespace UtilLib {
    public abstract class Master : ProxyManager {
        private readonly Dictionary<IPEndPoint, Slave> slaves = new Dictionary<IPEndPoint,Slave>();

        public class Slave {
            /// <summary>
            /// RotationOffset Offset for the slave.
            /// </summary>
            public Rotation RotationOffset {
                get {
                    throw new System.NotImplementedException();
                }
                set {
                }
            }

            /// <summary>
            /// Position offset for the slave.
            /// </summary>
            public Vector3 PositionOffset {
                get {
                    throw new System.NotImplementedException();
                }
                set {
                }
            }

            /// <summary>
            /// Name of the slave.
            /// </summary>
            public string Name {
                get {
                    throw new System.NotImplementedException();
                }
                set {
                }
            }

            /// <summary>
            /// The address of the slave so packets can be sent to it.
            /// </summary>
            public IPEndPoint Address {
                get {
                    throw new System.NotImplementedException();
                }
                set {
                }
            }
        }

        protected readonly InterProxyServer masterServer = new InterProxyServer();

        /// <summary>
        /// Triggered whenever a slave disconnects.
        /// </summary>
        public event Action<string> OnSlaveDisconnected {
            add { masterServer.OnSlaveDisconnected += value; }
            remove { masterServer.OnSlaveDisconnected -= value; }
        }

        /// <summary>
        /// Triggered whenever a slave connects.
        /// </summary>
        public event Action<string, IPEndPoint> OnSlaveConnected {
            add { masterServer.OnSlaveConnected += value; }
            remove { masterServer.OnSlaveConnected -= value; }
        }

        /// <summary>
        /// Address that slaves should use to connect to this master.
        /// </summary>
        public string MasterAddress {
            get { return masterServer.Address; }
        }

        /// <summary>
        /// Port that slaves should use to connect to this master.
        /// </summary>
        public int MasterPort {
            get { return masterServer.Port; }
        }

        /// <summary>
        /// True if the master is ready to receive connections from slaves.
        /// </summary>
        public bool MasterRunning {
            get { return masterServer.Bound; }
        }

        /// <summary>
        /// Names of the slaves that are connected.
        /// </summary>
        public Slave[] Slaves {
            get { return slaves.Values.ToArray(); }
        }

        /// <summary>
        /// Bind the master so that slaves can connect into it.
        /// </summary>
        public bool StartMaster() {
            return masterServer.Start();
        }

        /// <summary>
        /// Bind the master so that slaves can connect into it.
        /// </summary>
        /// <param name="masterPort">The masterPort that clients should connect to this master on.</param>
        public bool StartMaster(int port) {
            return masterServer.Start(port);
        }

        /// <summary>
        /// Disconnect the master, closing any ports it had opened to listen.
        /// </summary>
        public void Stop() {
            StopProxy();
            masterServer.Stop();
        }

        /// <summary>
        /// Set whether a packet type is to be forwarded.
        /// </summary>
        /// <param name="packet">The type of packet to forward or stop forwarding.</param>
        /// <param name="forward">True if the packet is to be forwared.</param>
        /// <param name="direction">Whether to deal with ingoing or outgoing packets.</param>
        public void SetPacketForward(OpenMetaverse.Packets.PacketType packet, bool forward, Direction direction) {
            throw new System.NotImplementedException();
        }
    }
}

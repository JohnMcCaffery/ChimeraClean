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
        private readonly Dictionary<string, Slave> slaves = new Dictionary<string,Slave>();

        public class Slave {
            private readonly IPEndPoint ep;
            private string name;
            private Quaternion rotation = Quaternion.Identity;
            private Vector3 position = Vector3.Zero;

            /// <summary>
            /// Called whenever the slave changes.
            /// </summary>
            public event EventHandler OnChange;

            /// <summary>
            /// RotationOffset Offset for the slave.
            /// </summary>
            public Quaternion RotationOffset {
                get { return rotation; }
                set { 
                    rotation = value;
                    if (OnChange != null)
                        OnChange(this, null);
                }
            }

            /// <summary>
            /// Position offset for the slave.
            /// </summary>
            public Vector3 PositionOffset {
                get { return position; }
                set { 
                    position = value;
                    if (OnChange != null)
                        OnChange(this, null);
                }
            }

            /// <summary>
            /// Name of the slave.
            /// </summary>
            public string Name {
                get { return name; }
                set { name = value; }
            }

            /// <summary>
            /// The address of the slave so packets can be sent to it.
            /// </summary>
            public IPEndPoint TargetEP {
                get { return ep; }
            }

            public Slave(string name, IPEndPoint ep) {
                this.name = name;
                this.ep = ep;
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
        public event System.Action<Slave> OnSlaveConnected;

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
        public Dictionary<string, Slave> Slaves {
            get { return slaves; }
        }

        public Master() {
            masterServer.OnSlaveConnected += (name, ep) => {
                Slave slave = new Slave(name, ep);
                lock (slaves)
                    slaves.Add(name, slave);
                if (OnSlaveConnected != null)
                    OnSlaveConnected(slave);
            };
            masterServer.OnSlaveDisconnected += name => {
                lock (slaves)
                    slaves.Remove(name);
            };
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

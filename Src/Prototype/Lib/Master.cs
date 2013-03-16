/*************************************************************************
Copyright (c) 2012 John McCaffery 

This file is part of Chimera.

Chimera is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Chimera is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Chimera.  If not, see <http://www.gnu.org/licenses/>.

**************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GridProxy;
using OpenMetaverse.Packets;
using System.Net;
using OpenMetaverse;
using log4net;
using Chimera;

namespace UtilLib {
    public abstract class Master : ProxyManager {
        public ILog Logger = log4net.LogManager.GetLogger("Master Logger");
        protected readonly Dictionary<string, Slave> slaves = new Dictionary<string,Slave>();

        public class Slave {
            private readonly IPEndPoint ep;
            private string name;
            private Rotation rotation;
            private Vector3 position = Vector3.Zero;
            private Window window;

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
            public IPEndPoint EP {
                get { return ep; }
            }

            /// <summary>
            /// The input object that represents where the slave is in real space.
            /// </summary>
            public Window Window {
                get { return window; }
            }

            public Slave(string name, IPEndPoint ep, Vector3 eyePosition) {
                this.name = name;
                this.ep = ep;
                window = new Window(name);
                window.EyePosition = eyePosition;
            }
        }

        private Window window = null;
        protected readonly InterProxyServer masterServer = new InterProxyServer();

        /// <summary>
        /// Selected whenever a slave disconnects.
        /// </summary>
        public event Action<string> OnSlaveDisconnected {
            add { masterServer.OnSlaveDisconnected += value; }
            remove { masterServer.OnSlaveDisconnected -= value; }
        }

        /// <summary>
        /// Selected whenever a slave connects.
        /// </summary>
        public event Action<Slave> OnSlaveConnected;

        /// <summary>
        /// Selected whenever the master is bound to a socket.
        /// </summary>
        public event EventHandler OnMasterBound;

        /// <summary>
        /// Address that slaves should use to connect to this master.
        /// </summary>
        public string MasterAddress {
            get { return masterServer.Address; }
        }

        /// <summary>
        /// True if the master is ready to receive connections from slaves.
        /// </summary>
        public bool MasterRunning {
            get { return masterServer.Bound; }
        }

        /// <summary>
        /// All of the slaves that are connected.
        /// </summary>
        public Slave[] Slaves {
            get { return slaves.Values.ToArray(); }
        }

        public Master() : this(null) { }

        public Master(Init.Config config) : base (config, LogManager.GetLogger("Master")) {
            window = new Window("Master");
            masterServer.OnSlaveConnected += (name, ep) => {
                Slave slave = new Slave(name, ep, window.EyePosition);
                lock (slaves)
                    slaves.Add(name, slave);
                if (OnSlaveConnected != null)
                    OnSlaveConnected(slave);
            };
            masterServer.OnSlaveDisconnected += name => {
                lock (slaves)
                    slaves.Remove(name);
            };
            masterServer.OnBound += (source, args) => {
                if (OnMasterBound != null)
                    OnMasterBound(source, args);
            };
            window.OnEyeChange += (diff) => {
                if (window.LockScreenPosition)
                    foreach (var slave in Slaves)
                        //slave.Window.ScreenPosition += diff;
                        slave.Window.EyePosition = window.EyePosition;
            };
        }

        /// <summary>
        /// Bind the master so that slaves can connect into it.
        /// </summary>
        /// <returns>True if the master was successfully started.</returns>
        public bool StartMaster() {
            return masterServer.Start(ProxyConfig.MasterAddress, ProxyConfig.MasterPort);
        }

        /// <summary>
        /// Bind the master so that slaves can connect into it.
        /// </summary>
        /// <param name="masterPort">The masterPort that clients should connect to this master on.</param>
        /// <returns>True if the master was successfully started.</returns>
        public bool StartMaster(int port) {
            ProxyConfig.MasterPort = port;
            return masterServer.Start();
        }

        /// <summary>
        /// Bind the master so that slaves can connect into it.
        /// </summary>
        /// <param name="address">The address that clients should connect to this master on.</param>
        /// <param name="port">The port that clients should connect to this master on.</param>
        /// <returns>True if the master was successfully started.</returns>
        public bool StartMaster(string address, int port) {
            ProxyConfig.MasterAddress = address;
            ProxyConfig.MasterPort = port;
            return masterServer.Start();
        }

        /// <summary>
        /// Disconnect the master, closing any ports it had opened to listen.
        /// </summary>
        public void StopMaster() {
            masterServer.Stop();
            slaves.Clear();
        }

        /// <summary>
        /// Stop this master server and stop the proxy server.
        /// </summary>
        public override void Stop() {
            base.Stop();
            StopMaster();
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

        public Window Window {
            get { return window; }
        }
    }
}

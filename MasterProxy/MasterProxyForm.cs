using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using UtilLib;
using OpenMetaverse.Packets;
using GridProxy;
using System.Net;

namespace MasterProxy {
    public partial class MasterProxyForm : Form {
        private XMLRPCMaster masterServer;
        private Proxy proxy;
        private int packetCount = 0;
        private bool loggedIn = false;
        private Queue<object> newSlaves = new Queue<object>();
        
        public MasterProxyForm() {
            InitializeComponent();

            masterServer = new XMLRPCMaster();
            masterServer.SlaveConnected += (source, args) => {
                lock (newSlaves)
                    newSlaves.Enqueue(source);
            };
        }

        private void listTimer_Tick(object sender, EventArgs e) {
            lock(newSlaves)
                while (newSlaves.Count > 0)
                    slavesListBox.Items.Add(newSlaves.Dequeue());
            if (loggedIn) {
                loggedInLabel.Text = "Logged In";
                connectButton.Enabled = false;
                portBox.Enabled = false;
                listenIPBox.Enabled = false;
                loginURIBox.Enabled = false;
            }
        }

        private Packet BroadcastPacket(Packet p, IPEndPoint ep) {
            /*
            if (p.GetType() == typeof(ImprovedTerseObjectUpdatePacket)) {
                //Process terse packet
            } else if (p.GetType() == typeof(AgentUpdatePacket)) {
                //Process agent update packet
            }
            if (p != null)
                masterServer.BroadcastPacket(p);
            */
            masterServer.BroadcastPacket(p);
            packetCount++;
            packetCountLabel.Text = packetCount + "";
            return p;
        }

        private void connectButton_Click(object sender, EventArgs e) {
            string file = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;

            string portArg = "--proxy-login-port="+portBox.Text;
            string listenIPArg = "--proxy-client-facing-address="+listenIPBox.Text;
            string loginURIArg = "--proxy-remote-login-uri="+loginURIBox.Text;
            string[] args = { portArg, listenIPArg, loginURIArg };
            ProxyConfig config = new ProxyConfig("Routing God", "jm726@st-andrews.ac.uk", args);
            proxy = new Proxy(config);
            proxy.AddLoginResponseDelegate(response => {
                loggedIn = true;
            });

            foreach (PacketType packetType in Enum.GetValues(typeof(PacketType))) 
                proxy.AddDelegate(packetType, Direction.Incoming, BroadcastPacket);

            masterServer.BroadcastPacket(new ImprovedTerseObjectUpdatePacket());
        }
    }
}

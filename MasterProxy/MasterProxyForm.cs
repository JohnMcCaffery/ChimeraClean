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
using OpenMetaverse;

namespace MasterProxy {
    public partial class MasterProxyForm : Form {
        private XMLRPCMaster masterServer;
        private int packetCount = 0;
        private Queue<object> newSlaves = new Queue<object>();
        private Dictionary<string, bool> forwardedPackets = new Dictionary<string, bool>();
        private HashSet<PacketType> enabledPackets = new HashSet<PacketType>();
        
        public MasterProxyForm() {
            enabledPackets.Add(PacketType.AgentUpdate);
            enabledPackets.Add(PacketType.ObjectUpdate);
            enabledPackets.Add(PacketType.ImprovedTerseObjectUpdate);

            InitializeComponent();

            masterServer = new XMLRPCMaster();
            masterServer.OnSlaveConnected += (source, args) => {
                lock (newSlaves)
                    newSlaves.Enqueue(source);
            };

            foreach (PacketType pt in Enum.GetValues(typeof(PacketType))) {
                ListViewItem item = packetList.Items.Add(pt.ToString());
                item.Checked = enabledPackets.Contains(pt) || selectAll.Checked;
                forwardedPackets[item.Text] = item.Checked;
            }
        }

        private void listTimer_Tick(object sender, EventArgs e) {
            packetCountLabel.Text = packetCount + "";
            lock(newSlaves)
                while (newSlaves.Count > 0)
                    slavesListBox.Items.Add(newSlaves.Dequeue());
        }

        private Packet BroadcastPacket(Packet p, IPEndPoint ep) {
            if (forwardedPackets[p.Type.ToString()]) {
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
            }
            return p;
        }

        private void MasterProxyForm_FormClosing(object sender, FormClosingEventArgs e) {
            if (proxyPanel.HasStarted)
                proxyPanel.Proxy.Stop();
        }

        private void proxyPanel_Started(object sender, EventArgs e) {
            foreach (PacketType pt in Enum.GetValues(typeof(PacketType))) {
                proxyPanel.Proxy.AddDelegate(pt, Direction.Incoming, BroadcastPacket);
                proxyPanel.Proxy.AddDelegate(pt, Direction.Incoming, BroadcastPacket);
            }

            proxyPanel.Proxy.AddLoginResponseDelegate(response => {
                masterServer.SetLoginResponse(response); 
                return response;
            });
        }

        private void selectAll_CheckedChanged(object sender, EventArgs e) {
            foreach (ListViewItem item in packetList.Items) {
                item.Checked = selectAll.Checked;
            }
        }

        private void packetList_ItemChecked(object sender, ItemCheckedEventArgs e) {
            forwardedPackets[e.Item.Text] = e.Item.Checked;
        }
    }
}

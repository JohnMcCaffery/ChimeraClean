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
using System.Collections;

namespace MasterProxy {
    public partial class MasterProxyForm : Form {
        private XMLRPCMaster masterServer;
        private Queue<object> newSlaves = new Queue<object>();
        private Dictionary<string, bool> forwardedPackets = new Dictionary<string, bool>();
        private HashSet<PacketType> enabledPackets = new HashSet<PacketType>();
        private int packetCount = 0, forwardedCount = 0, createdCount = 0;
        private bool forwardLogin;
        private bool processingPacket;
        private bool slaveConnected;
        private bool leftDown, rightDown, forwardDown, backwardDown, upDown, downDown, yawRightDown, yawLeftDown, pitchUpDown, pitchDownDown;
        private Packet masterObjectPacket = null;
        private UUID masterID;
        
        public MasterProxyForm() {
            enabledPackets.Add(PacketType.AgentUpdate);
            enabledPackets.Add(PacketType.ObjectUpdate);
            //enabledPackets.Add(PacketType.ImprovedTerseObjectUpdate);

            InitializeComponent();

            foreach (PacketType pt in Enum.GetValues(typeof(PacketType))) {
                ListViewItem item = packetList.Items.Add(pt.ToString());
                item.Checked = enabledPackets.Contains(pt) || selectAll.Checked;
                forwardedPackets[item.Text] = item.Checked;
            }
        }

        private Packet ProcessReceivedPacket(Packet p, IPEndPoint ep) {
            packetCount++;
            if (proxyPanel.HasStarted && forwardedPackets[p.Type.ToString()]) {
                if (p.Type == PacketType.AgentUpdate) {
                    processingPacket = true;
                    positionPanel.Value = ((AgentUpdatePacket)p).AgentData.CameraCenter;
                    rotationPanel.Vector = ((AgentUpdatePacket)p).AgentData.CameraAtAxis;
                    processingPacket = false;
                } else if (p.Type == PacketType.ObjectUpdate && ((ObjectUpdatePacket)p).ObjectData[0].FullID == masterID) {
                    masterObjectPacket = p;
                }
                if (slaveConnected) {
                    masterServer.BroadcastPacket(p);
                    forwardedCount++;
                }
            }
            return p;
        }

        private void MasterProxyForm_FormClosing(object sender, FormClosingEventArgs e) {
            if (proxyPanel.HasStarted)
                proxyPanel.Proxy.Stop();
            if (masterServer != null)
                masterServer.Stop();
        }

        private void proxyPanel_Started(object sender, EventArgs e) {
            foreach (PacketType pt in Enum.GetValues(typeof(PacketType))) {
                proxyPanel.Proxy.AddDelegate(pt, Direction.Incoming, ProcessReceivedPacket);
                proxyPanel.Proxy.AddDelegate(pt, Direction.Outgoing, ProcessReceivedPacket);
            }

            proxyPanel.Proxy.AddLoginResponseDelegate(response => {
                masterID = UUID.Parse(((Hashtable) response.Value)["agent_id"].ToString());
                if (forwardLogin)
                    masterServer.SetLoginResponse(response); 
                return response;
            });

            udpPortBox.Enabled = false;

            masterServer = new XMLRPCMaster(proxyPanel.Proxy.proxyConfig.clientFacingAddress.ToString(), int.Parse(udpPortBox.Text));
            masterServer.OnSlaveConnected += (source, args) => {
                slaveConnected = true;
                lock (newSlaves)
                    newSlaves.Enqueue(source);
                if (masterObjectPacket != null)
                    masterServer.BroadcastPacket(masterObjectPacket);
            };
        }

        private void selectAll_CheckedChanged(object sender, EventArgs e) {
            foreach (ListViewItem item in packetList.Items) {
                item.Checked = selectAll.Checked;
            }
        }

        private void packetList_ItemChecked(object sender, ItemCheckedEventArgs e) {
            forwardedPackets[e.Item.Text] = e.Item.Checked;
        }

        private void forwardLoginCheck_CheckedChanged(object sender, EventArgs e) {
            forwardLogin = forwardLoginCheck.Checked;
        }

        private void keyDown(object sender, KeyEventArgs e) {
            switch (e.KeyData) {
                case Keys.A: leftDown = true; break;
                case Keys.D: rightDown = true; break;
                case Keys.W: forwardDown = true; break;
                case Keys.S: backwardDown = true; break;
                case Keys.PageUp: upDown = true; break;
                case Keys.PageDown: downDown = true; break;
                case Keys.Left: yawLeftDown = true; break;
                case Keys.Right: yawRightDown = true; break;
                case Keys.Up: pitchUpDown = true; break;
                case Keys.Down: pitchDownDown = true; break;
            }
        }

        private void keyUp(object sender, KeyEventArgs e) {
            switch (e.KeyData) {
                case Keys.A: leftDown = false; break;
                case Keys.D: rightDown = false; break;
                case Keys.W: forwardDown = false; break;
                case Keys.S: backwardDown = false; break;
                case Keys.PageUp: upDown = false; break;
                case Keys.PageDown: downDown = false; break;
                case Keys.D4: yawLeftDown = false; break;
                case Keys.D6: yawRightDown = false; break;
                case Keys.D8: pitchUpDown = false; break;
                case Keys.D5: pitchDownDown = false; break;
            }
        }

        private void onChange(object sender, EventArgs e) {
            if (processingPacket)
                return;
            AgentUpdatePacket p = (AgentUpdatePacket) Packet.BuildPacket(PacketType.AgentUpdate);
            p.AgentData.AgentID = UUID.Random();
            p.AgentData.BodyRotation = Quaternion.Identity;
            p.AgentData.CameraLeftAxis = Vector3.Cross(Vector3.UnitZ, rotationPanel.Vector);
            p.AgentData.CameraUpAxis = Vector3.UnitZ;
            p.AgentData.HeadRotation = Quaternion.Identity;
            p.AgentData.SessionID = UUID.Random();

            p.AgentData.CameraCenter = positionPanel.Value;
            p.AgentData.CameraAtAxis = rotationPanel.Vector;
            createdCount++;
            if (slaveConnected) {
                masterServer.BroadcastPacket(p);
                forwardedCount++;
            }
        }

        private void listTimer_Tick(object sender, EventArgs e) {
            if (yawLeftDown) rotationPanel.Yaw -= .25f;
            if (yawRightDown) rotationPanel.Yaw += .25f;
            if (pitchUpDown) rotationPanel.Pitch += .25f;
            if (pitchDownDown) rotationPanel.Pitch -= .25f;

            Vector3 move = Vector3.Zero;
            if (forwardDown) move.X += .25f;
            if (backwardDown) move.X -= .25f;
            if (leftDown) move.Y -= .25f;
            if (rightDown) move.Y += .25f;
            if (upDown) move.Z += .25f;
            if (downDown) move.Z -= .25f;

            if (move != Vector3.Zero) {
                move *= rotationPanel.Rotation;
                positionPanel.Value += move;
            }

            packetCountLabel.Text = packetCount + "";
            forwardedCountLabel.Text = forwardedCount + "";
            createdCountLabel.Text = createdCount + "";
            lock(newSlaves)
                while (newSlaves.Count > 0)
                    slavesListBox.Items.Add(newSlaves.Dequeue());
        }
    }
}

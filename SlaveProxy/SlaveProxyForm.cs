/*************************************************************************
Copyright (c) 2012 John McCaffery 

This file is part of Armadillo Proxy.

Routing Project is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Routing Project is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Routing Project.  If not, see <http://www.gnu.org/licenses/>.

**************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CookComputing.XmlRpc;
using System.Runtime.Remoting;
using System.Threading;
using UtilLib;
using GridProxy;
using OpenMetaverse.Packets;
using OpenMetaverse;
using System.Net;
using System.Collections;

namespace SlaveProxy {
    public partial class SlaveProxyForm : Form {
        public class Avatar {
            public string firstName;
            public string lastName;
            public uint localID;
            public UUID id;

            public string Name {
                get { return firstName + " " + lastName; }
                set {
                    String[] split = value.Split(' ');
                    firstName = split[0];
                    lastName = split[1];
                }
            }
            public UUID ID {
                get { return id; }
                set { id = value; }
            }

            public Avatar(string firstName, string lastName, uint localID, UUID id) {
                this.firstName = firstName;
                this.lastName = lastName;
                this.localID = localID;
                this.id = id;
            }

            public override string ToString() {
                return Name;
            }
        }

        private Queue<Packet> packetsToProcess = new Queue<Packet>();
        private Dictionary<UUID, Avatar> avatars = new Dictionary<UUID, Avatar>();
        private XMLRPCSlave slaveClient;
        private Avatar selectedAvatar;
        private int processedCount = 0;
        private bool processAgentUpdates = false;
        private bool processObjectUpdates = false;
        private bool controlCamera = false;
        private UUID slaveAvatar = UUID.Zero;
        private UUID sessionID;

        public SlaveProxyForm() {
            InitializeComponent();

            slaveClient = new XMLRPCSlave();
            slaveClient.OnPacketReceived += MasterPacketReceived;
            slaveClient.OnPing += Pinged;

            proxyPanel.OnStarted += ProxyStarted;
        }

        private void ProxyStarted(object source, EventArgs args) {
            //proxyPanel.Proxy.AddDelegate(PacketType.ObjectUpdate, Direction.Incoming, ObjectUpdatePacketReceived);
            proxyPanel.Proxy.AddDelegate(PacketType.ImprovedTerseObjectUpdate, Direction.Incoming, ImprovedTersePacketPacketReceived);
            //proxyPanel.Proxy.AddDelegate(PacketType.AgentUpdate, Direction.Outgoing, AgentUpdatePacketReceived);

            proxyPanel.Proxy.AddLoginResponseDelegate(LoginResponseReceived);
        }

        private Packet MasterPacketReceived(Packet p, IPEndPoint ep) {
            if (processAgentUpdates && p.Type == PacketType.AgentUpdate) {
                AgentUpdatePacketReceived((AgentUpdatePacket)p, ep);
            }
            if (processObjectUpdates && p.Type == PacketType.ImprovedTerseObjectUpdate) {
                ImprovedTersePacketPacketReceived(p, ep);
            }
            if (p.Type == PacketType.ObjectUpdate) {
                ObjectUpdatePacketReceived(p, ep);
            }
            
            Invoke(new Action(() => {
                countLabel.Text = processedCount + "";
                packetsToProcessLabel.Text = packetsToProcess.Count + "";
                processedPacketsLabel.Text = slaveClient.UnprocessedPackets + "";
            }));
            return p;
        }

        private Nwc.XmlRpc.XmlRpcResponse LoginResponseReceived(Nwc.XmlRpc.XmlRpcResponse response) {
            Hashtable responseData = (System.Collections.Hashtable)response.Value;
            sessionID = UUID.Parse(responseData["session_id"].ToString());
            slaveAvatar = UUID.Parse(responseData["avatar_id"].ToString());
            avatars.Add(slaveAvatar, new Avatar(responseData["first_name"].ToString(), responseData["last_name"].ToString(), uint.MaxValue, slaveAvatar));
            return response;
        }

        private Packet ObjectUpdatePacketReceived(Packet p, IPEndPoint ep) {
            ObjectUpdatePacket packet = (ObjectUpdatePacket)p;
            foreach (var block in packet.ObjectData) {
                if (block.PCode == (byte)PCode.Avatar) {
                    string name = Utils.BytesToString(block.NameValue);
                    name = name.Replace("STRING RW SV ", "");
                    string[] tokens = name.Split(' ', '\n');
                    string firstName = tokens[1];
                    string lastName = tokens[3];

                    Invoke(new Action(() => {
                        Avatar avatar = new Avatar(firstName, lastName, block.ID, block.FullID);
                        avatarList.Items.Add(avatar);
                        avatarList.SelectedItem = avatar;
                        avatars[avatar.ID] = avatar;
                    }));
                }
            }
            return packet;
        }

        private Packet AgentUpdatePacketReceived(Packet  packet, IPEndPoint ep) {
            AgentUpdatePacket p = (AgentUpdatePacket)packet;
            //if (selectedAvatar != null && selectedAvatar.id == p.AgentData.AgentID) {
                positionPanel.Value = p.AgentData.CameraCenter;
                rotationPanel.Vector = p.AgentData.CameraAtAxis;
                velocityPanel.Value = Vector3.Zero;
                accelerationPanel.Value = Vector3.Zero;
                rotationalVelocityPanel.Value = Vector3.Zero;
                sendCameraUpdate();
                processedCount++;
            //} 
            return p;
        }

        private Packet ImprovedTersePacketPacketReceived(Packet p, IPEndPoint ep) {
            if (!processObjectUpdates || selectedAvatar == null)
                return p;

            processedCount++;
            ImprovedTerseObjectUpdatePacket packet = (ImprovedTerseObjectUpdatePacket)p;
            byte[] block = packet.ObjectData[0].Data;

            if (selectedAvatar.localID == Utils.BytesToUInt(block, 0)) {
                Vector3 position = new Vector3(block, 0x16) + (Vector3.UnitZ * 2f);
                Vector3 velocity = new Vector3(
                    Utils.UInt16ToFloat(block, 0x22, -128.0f, 128.0f),
                    Utils.UInt16ToFloat(block, 0x22 + 2, -128.0f, 128.0f),
                    Utils.UInt16ToFloat(block, 0x22 + 4, -128.0f, 128.0f));
                Vector3 acceleration = new Vector3(
                    Utils.UInt16ToFloat(block, 0x28, -128.0f, 128.0f),
                    Utils.UInt16ToFloat(block, 0x28 + 2, -128.0f, 128.0f),
                    Utils.UInt16ToFloat(block, 0x28 + 4, -128.0f, 128.0f));
                Quaternion rotation = new Quaternion(
                    Utils.UInt16ToFloat(block, 0x2E, -1.0f, 1.0f),
                    Utils.UInt16ToFloat(block, 0x2E + 2, -1.0f, 1.0f),
                    Utils.UInt16ToFloat(block, 0x2E + 4, -1.0f, 1.0f),
                    Utils.UInt16ToFloat(block, 0x2E + 6, -1.0f, 1.0f));
                Vector3 rotationalVelocity = new Vector3(
                    Utils.UInt16ToFloat(block, 0x36, -64.0f, 64.0f),
                    Utils.UInt16ToFloat(block, 0x36 + 2, -64.0f, 64.0f),
                    Utils.UInt16ToFloat(block, 0x36 + 4, -64.0f, 64.0f));

                positionPanel.Value = position;
                velocityPanel.Value = velocity;
                accelerationPanel.Value = acceleration;
                rotationPanel.Vector = Vector3.UnitX * rotation;
                rotationalVelocityPanel.Value = rotationalVelocity;

                sendCameraUpdate();
            }
            return packet;
        }

        private void Pinged() {
            Invoke(new Action(() => {
                nameBox.Enabled = false;
                masterAddressBox.Enabled = false;
                connectButton.Enabled = false;
                masterXmlRpcPortBox.Enabled = false;
                listenIPBox.Enabled = false;
                portBox.Enabled = false;
                xmlRpcPortBox.Enabled = false;
            }));
        }

        private void connectButton_Click(object sender, EventArgs e) {
            slaveClient.Name = nameBox.Text;
            slaveClient.Connect(
                masterAddressBox.Text, 
                listenIPBox.Text, 
                int.Parse(masterXmlRpcPortBox.Text), 
                int.Parse(portBox.Text), 
                int.Parse(xmlRpcPortBox.Text)
            );
        }

        private void SlaveProxyForm_FormClosing(object sender, FormClosingEventArgs e) {
            controlCamera = false;
            if (proxyPanel.HasStarted)
                proxyPanel.Proxy.Stop();
            slaveClient.Stop();
        }

        private bool SendCameraPackets {
            get { return controlCamera && (processObjectUpdates || processAgentUpdates); }
        }

        private void avatarList_SelectedIndexChanged(object sender, EventArgs e) {
            selectedAvatar = (Avatar)avatarList.SelectedItem;
        }

        private void positionPanel_OnChange(object sender, EventArgs e) {
            setFollowCamPropertiesPanel.Position = positionPanel.Value;
        }

        private void rotationPanel_OnChange(object sender, EventArgs e) {
            setFollowCamPropertiesPanel.Rotation = rotationPanel.Rotation;
            Packet p = setFollowCamPropertiesPanel.Packet;
        }

        private void processAgentUpdatesCheck_CheckedChanged(object sender, EventArgs e) {
            processAgentUpdates = processAgentUpdatesCheck.Checked;
            if (!processObjectUpdates && !processAgentUpdates) StopControllingCamera();
        }

        private void processObjectUpdatesCheck_CheckedChanged(object sender, EventArgs e) {
            processObjectUpdates = processObjectUpdatesCheck.Checked;
            if (!processObjectUpdates && !processAgentUpdates) StopControllingCamera();
        }

        private void controlCameraCheck_CheckedChanged(object sender, EventArgs e) {
            controlCamera = controlCameraCheck.Checked;
            if (!controlCamera) StopControllingCamera();
        }

        private void sendCameraUpdate() {
            if (SendCameraPackets && proxyPanel.HasStarted)
                //new Thread(() => {
                    proxyPanel.Proxy.InjectPacket(setFollowCamPropertiesPanel.Packet, Direction.Incoming);
                //}).Start();
        }

        private void StopControllingCamera() {
            if (proxyPanel != null) {
                SetFollowCamPropertiesPacket packet = new SetFollowCamPropertiesPacket();
                packet.CameraProperty = new SetFollowCamPropertiesPacket.CameraPropertyBlock[1];
                packet.CameraProperty[0] = new SetFollowCamPropertiesPacket.CameraPropertyBlock();
                packet.CameraProperty[0].Type = 13;
                packet.CameraProperty[0].Value = 0;
                proxyPanel.Proxy.InjectPacket(packet, Direction.Incoming);
            }
        }
    }
}
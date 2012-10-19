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

namespace SlaveProxy {
    public partial class SlaveProxyForm : Form {
        public class Avatar {
            public string firstName;
            public string lastName;
            public uint localID;
            public UUID id;

            public string Name { get { return firstName + " " + lastName; }  
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

        private XMLRPCSlave slaveClient;
        private int packetCount = 0;
        private Queue<Packet> packetsToProcess = new Queue<Packet>();
        private bool loggedIn = false;

        public SlaveProxyForm() {
            InitializeComponent();

            slaveClient = new XMLRPCSlave();
            slaveClient.OnPacketReceived += MasterPacketReceived;
            slaveClient.OnPing += Pinged;

            proxyPanel.OnStarted += ProxyStarted;
        }

        private void ProxyStarted(object source, EventArgs args) {
            //foreach (PacketType pt in Enum.GetValues(typeof(PacketType))) {
            //proxyPanel.Proxy.AddDelegate(pt, Direction.Outgoing, (packet, ep) => { return null; });
            //proxyPanel.Proxy.AddDelegate(pt, Direction.Incoming, (packet, ep) => { return null; });
            //}

            proxyPanel.Proxy.AddDelegate(PacketType.ObjectUpdate, Direction.Incoming, ObjectUpdatePacketReceived);
            proxyPanel.Proxy.AddDelegate(PacketType.ImprovedTerseObjectUpdate, Direction.Incoming, (p, ep) => { 
                ProcessImprovedTersePacket((ImprovedTerseObjectUpdatePacket)p);
                return p;
            });

            //Replace all login responses with faked ones
            proxyPanel.Proxy.AddLoginResponseDelegate(LoginResponseReceived);

            //Kill all login requests
            //proxyPanel.Proxy.AddLoginRequestDelegate((s, a) => { return null; });
        }

        private Nwc.XmlRpc.XmlRpcResponse LoginResponseReceived(Nwc.XmlRpc.XmlRpcResponse response) {
                    Nwc.XmlRpc.XmlRpcResponse masterResponse = slaveClient.MasterResponse;
                    new Thread(() => {
                        Thread.Sleep(1000);
                        ProcessPackets();
                        loggedIn = true;
                    }).Start();
                    //return masterResponse != null ? masterResponse : response;
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
                        //avatarsListBox.DataSource = avatarsBindingSource;
                        avatarList.Items.Add(new Avatar(firstName, lastName, block.ID, block.FullID));
                    }));
                }
            }
            //"FirstName STRING RW SV Routing\nLastName STRING RW SV God\nTitle STRING RW SV "
            return packet;
        }

        private Packet MasterPacketReceived(Packet p, IPEndPoint ep) {
            //if (p.Type == PacketType.ImprovedTerseObjectUpdate)
                //ProcessImprovedTersePacket((ImprovedTerseObjectUpdatePacket)p);

            packetCount++;
            Invoke(new Action(() => {
                countLabel.Text = slaveClient.ReceivedPackets + "";
                packetsToProcessLabel.Text = packetsToProcess.Count + "";
                unprocessedPacketsLabel.Text = slaveClient.UnprocessedPackets + "";
            }));

            //if (p.Type != PacketType.ImprovedTerseObjectUpdate) {
                //lock (packetsToProcess)
                    //packetsToProcess.Enqueue(p);
            //}
            return p;
        }

        private uint selectedID = 0;
        private DateTime prev = DateTime.Now;

        private void ProcessImprovedTersePacket(ImprovedTerseObjectUpdatePacket p) {
                byte[] block = p.ObjectData[0].Data;

                if (selectedID == Utils.BytesToUInt(block, 0)) {
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


                    Console.WriteLine(position);
                    if (setFollowCamPropertiesPanel.Velocity.Length() > 0) {
                        float x = 1000;
                        double diff = DateTime.Now.Subtract(prev).TotalMilliseconds;
                        prev = DateTime.Now;

                        Vector3 posDif = (position - setFollowCamPropertiesPanel.Position) * x;
                        Vector3 scale = posDif / (setFollowCamPropertiesPanel.Velocity * (float)diff);

                        //.99
                        /*
                        ((n - o) * 1000) / v * diff = .99
                        n = (.99diff * v) + o
                         */
                    }

                    setFollowCamPropertiesPanel.Position = position;
                    setFollowCamPropertiesPanel.Velocity = velocity;
                    setFollowCamPropertiesPanel.Acceleration = acceleration;
                    setFollowCamPropertiesPanel.Rotation = rotation;
                    setFollowCamPropertiesPanel.AngularAcceleration = rotationalVelocity;

                    positionPanel.Value = position;
                    velocityPanel.Value = velocity;
                    accelerationPanel.Value = acceleration;
                    rotationPanel.Vector = Vector3.UnitX * rotation;
                    rotationalVelocityPanel.Value = rotationalVelocity;

                    /*
                    SetFollowCamPropertiesPacket packet = new SetFollowCamPropertiesPacket();
                    packet.CameraProperty = new SetFollowCamPropertiesPacket.CameraPropertyBlock[22];
                    for (int j = 0; j < 22; j++) {
                        packet.CameraProperty[j] = new SetFollowCamPropertiesPacket.CameraPropertyBlock();
                        packet.CameraProperty[j].Type = j + 1;
                    }
                    //Vector3 focus = positionVectorPanel.Value + focusRotationPanel.Value;
                    Vector3 focusVector = Vector3.UnitX * rot;
                    Vector3 focus = focusVector + pos;

                    packet.CameraProperty[0].Value = 0;
                    packet.CameraProperty[1].Value = 0;
                    packet.CameraProperty[2].Value = 0;
                    packet.CameraProperty[3].Value = 0;
                    packet.CameraProperty[4].Value = 0;
                    packet.CameraProperty[5].Value = 0;
                    packet.CameraProperty[6].Value = 0;
                    packet.CameraProperty[7].Value = 0;
                    packet.CameraProperty[8].Value = 0;
                    packet.CameraProperty[9].Value = 0;
                    packet.CameraProperty[10].Value = 0;
                    packet.CameraProperty[11].Value = 1f;
                    packet.CameraProperty[12].Value = 0f;
                    packet.CameraProperty[13].Value = pos.X;
                    packet.CameraProperty[14].Value = pos.Y;
                    packet.CameraProperty[15].Value = pos.Z;
                    packet.CameraProperty[16].Value = 0f;
                    packet.CameraProperty[17].Value = focus.X;
                    packet.CameraProperty[18].Value = focus.Y;
                    packet.CameraProperty[19].Value = focus.Z;
                    packet.CameraProperty[20].Value = 1f;
                    packet.CameraProperty[21].Value = 1f;

                    proxyPanel.Proxy.InjectPacket(packet, Direction.Incoming);
                    */
                }
        }

        private void Pinged() {
                Invoke(new Action(() => {
                    nameBox.Enabled = false;
                    masterURIBox.Enabled = false;
                    connectButton.Enabled = false;
                    portBox.Enabled = false;
                    xmlRpcPortBox.Enabled = false;
                    listenIPBox.Enabled = false;
                }));
        }

        private void connectButton_Click(object sender, EventArgs e) {
            slaveClient.Name = nameBox.Text;
            slaveClient.Connect(masterURIBox.Text, listenIPBox.Text, int.Parse(portBox.Text), int.Parse(xmlRpcPortBox.Text));
        }

        private void checkTimer_Tick(object sender, EventArgs e) {
            if (loggedIn) {
                //new Thread(ProcessPackets).Start();
                ProcessPackets();
            }
        }

        private void SlaveProxyForm_FormClosing(object sender, FormClosingEventArgs e) {
            if (proxyPanel.HasStarted)
                proxyPanel.Proxy.Stop();
        }

        private void ProcessPackets() {
            while (packetsToProcess.Count > 0) {
                Packet packet;
                lock (packetsToProcess)
                    packet = packetsToProcess.Dequeue();
                //proxyPanel.Proxy.InjectPacket(packet, Direction.Incoming);

            }
        }

        private void updateTimer_Tick(object sender, EventArgs e) {
            if (proxyPanel.HasStarted && selectedID > 0)
                //new Thread(() => {
                    proxyPanel.Proxy.InjectPacket(setFollowCamPropertiesPanel.Packet, Direction.Incoming);
                //}).Start();
        }

        private void avatarList_SelectedIndexChanged(object sender, EventArgs e) {
            if (avatarList.SelectedItem == null)
                selectedID = 0;
            else
                selectedID = ((Avatar)avatarList.SelectedItem).localID;
        }

        private void positionPanel_OnChange(object sender, EventArgs e) {
            setFollowCamPropertiesPanel.Position = positionPanel.Value;
        }

        private void rotationPanel_OnChange(object sender, EventArgs e) {
            setFollowCamPropertiesPanel.Rotation = rotationPanel.Rotation;
        }

        }
    }
    /*
                ImprovedTerseObjectUpdatePacket packet = (ImprovedTerseObjectUpdatePacket)p;
                //setCameraOffsetPropertiesPanel.Position = packet.ObjectData[0].Data; 
                
                byte[] block = packet.ObjectData[0].Data;
                int i = 4;

                StringBuilder result = new StringBuilder();
                uint localId = Utils.BytesToUInt(block, 0);
                byte attachmentPoint = block[i++];
                bool isAvatar = (block[i++] != 0);

                // Collision normal for avatar
                if (isAvatar) {
                    Vector4 collisionNormal = new Vector4(block, i);
                    i += 16;
                }

                Vector3 position = new Vector3(block, i);
                i += 12;
                Vector3 acceleration = new Vector3(
                    Utils.UInt16ToFloat(block, i, -128.0f, 128.0f),
                    Utils.UInt16ToFloat(block, i + 2, -128.0f, 128.0f),
                    Utils.UInt16ToFloat(block, i + 4, -128.0f, 128.0f));

                i += 6;
                Quaternion rotation = new Quaternion(
                    Utils.UInt16ToFloat(block, i, -1.0f, 1.0f),
                    Utils.UInt16ToFloat(block, i + 2, -1.0f, 1.0f),
                    Utils.UInt16ToFloat(block, i + 4, -1.0f, 1.0f),
                    Utils.UInt16ToFloat(block, i + 6, -1.0f, 1.0f));
                                      
                i += 8;
                // Angular velocity (omega)
                Vector3 anglularVelocity = new Vector3(
                    Utils.UInt16ToFloat(block, i, -64.0f, 64.0f),
                    Utils.UInt16ToFloat(block, i + 2, -64.0f, 64.0f),
                    Utils.UInt16ToFloat(block, i + 4, -64.0f, 64.0f));
    */
}

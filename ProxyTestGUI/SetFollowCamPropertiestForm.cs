using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GridProxy;
using OpenMetaverse;
using OpenMetaverse.Packets;
using System.Collections;
using System.Threading;


namespace ProxyTestGUI {
    public partial class SetFollowCamPropertiestForm : Form {
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
            public UUID ID { get { return id; } set { id = value; } }

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
        private Proxy mProxy;
        private UUID id;
        private bool mLoggedIn = false;

        public List<Avatar> Avatars { get; set; }

        public SetFollowCamPropertiestForm() {
            string file = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;

            string port = "8080";
            //string listenIP = "138.251.194.192";
            string listenIP = "127.0.0.1";
            string loginURI = "http://apollo.cs.st-andrews.ac.uk:8002";

            string portArg = "--proxy-login-port="+port;
            string listenIPArg = "--proxy-client-facing-address="+listenIP;
            string loginURIArg = "--proxy-remote-login-uri="+loginURI;
            string[] args = { portArg, listenIPArg, loginURIArg };
            ProxyConfig config = new ProxyConfig("Routing God", "jm726@st-andrews.ac.uk", args);
            mProxy = new Proxy(config);
            mProxy.AddLoginResponseDelegate(response => {
                mLoggedIn = true;
                return response;
            });
            mProxy.AddDelegate(PacketType.ObjectUpdate, Direction.Incoming, (p, ep) => {
                ObjectUpdatePacket packet = (ObjectUpdatePacket) p;
                foreach (var block in packet.ObjectData) {
                    if (block.PCode == (byte) PCode.Avatar) {
                        string name = Utils.BytesToString(block.NameValue);
                        name = name.Replace("STRING RW SV ", "");
                        string[] tokens = name.Split(' ', '\n');
                        string firstName = tokens[1];
                        string lastName = tokens[3];
                        string title = tokens.Length > 5 ? tokens[5] : "";

                        //avatarsListBox.DataSource = avatarsBindingSource;
                        avatarsBindingSource.Add(new Avatar(firstName, lastName, block.ID, block.FullID));

                        Console.WriteLine("\n\n Added " + firstName + " " + lastName + ".\n\n");
                    }
                }
                //"FirstName STRING RW SV Routing\nLastName STRING RW SV God\nTitle STRING RW SV "
                return packet;
            });

            InitializeComponent();

            focusRotationPanel.Vector = Vector3.UnitX;
        }

        private void updateTimer_Tick(object sender, EventArgs e) {
            avatarsBindingSource.ResetBindings(true);
            if (!mLoggedIn || !sendPacketCheckbox.Checked)
                return;

            SetFollowCamPropertiesPacket packet = new SetFollowCamPropertiesPacket();
            packet.CameraProperty = new SetFollowCamPropertiesPacket.CameraPropertyBlock[22];
            for(int i = 0; i < 22; i++) {
                packet.CameraProperty[i] = new SetFollowCamPropertiesPacket.CameraPropertyBlock();
                packet.CameraProperty[i].Type = i+1;
            }

            Vector3 focus = positionVectorPanel.Value + focusRotationPanel.Vector;

            packet.CameraProperty[0].Value = (float) type1Value.Value;
            packet.CameraProperty[1].Value = (float) type2Value.Value;
            packet.CameraProperty[2].Value = focusOffsetVectorPanel.Value.X;
            packet.CameraProperty[3].Value = focusOffsetVectorPanel.Value.Y;
            packet.CameraProperty[4].Value = focusOffsetVectorPanel.Value.Z;
            packet.CameraProperty[5].Value = (float) type6Value.Value;
            packet.CameraProperty[6].Value = (float) type7Value.Value;
            packet.CameraProperty[7].Value = (float) type8Value.Value;
            packet.CameraProperty[8].Value = (float) type9Value.Value;
            packet.CameraProperty[9].Value = (float) type10Value.Value;
            packet.CameraProperty[10].Value = (float) type11Value.Value;
            packet.CameraProperty[11].Value =  activeCheckbox.Checked ? 1f : 0f;
            packet.CameraProperty[12].Value = (float) type13Value.Value;
            packet.CameraProperty[13].Value = positionVectorPanel.Value.X;
            packet.CameraProperty[14].Value = positionVectorPanel.Value.Y;
            packet.CameraProperty[15].Value = positionVectorPanel.Value.Z;
            packet.CameraProperty[16].Value = (float) type17Value.Value;
            packet.CameraProperty[17].Value = focus.X;
            packet.CameraProperty[18].Value = focus.Y;
            packet.CameraProperty[19].Value = focus.Z;
            packet.CameraProperty[20].Value =  positionLockedCheckbox.Checked ? 1f: 0f;
            packet.CameraProperty[21].Value =  focusLockedCheckbox.Checked ? 1f : 0f;

            if (avatarsListBox.SelectedIndex != -1)
                packet.ObjectData.ObjectID = ((Avatar) avatarsListBox.SelectedValue).ID;

            mProxy.InjectPacket(packet, Direction.Incoming);
        }

        private void timerValue_ValueChanged(object sender, EventArgs e) {
            updateTimer.Interval = decimal.ToInt32(timeValue.Value);
        }

        private void startButton_Click(object sender, EventArgs e) {
            mProxy.Start();
            startButton.Enabled = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
            if (!startButton.Enabled)
                mProxy.Stop();
        }
    }
}

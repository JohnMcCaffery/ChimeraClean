using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenMetaverse.Packets;
using OpenMetaverse;
using ProxyTestGUI;

/*
1   FOLLOWCAM_PITCH = 0,
2   FOLLOWCAM_FOCUS_OFFSET,
3   FOLLOWCAM_FOCUS_OFFSET_X, //this HAS to come after FOLLOWCAM_FOCUS_OFFSET in this list
4   FOLLOWCAM_FOCUS_OFFSET_Y,
5   FOLLOWCAM_FOCUS_OFFSET_Z,
6   FOLLOWCAM_POSITION_LAG,
7   FOLLOWCAM_FOCUS_LAG,
8   FOLLOWCAM_DISTANCE,
9   FOLLOWCAM_BEHINDNESS_ANGLE,
10  FOLLOWCAM_BEHINDNESS_LAG,
11  FOLLOWCAM_POSITION_THRESHOLD,
12  FOLLOWCAM_FOCUS_THRESHOLD,
13  FOLLOWCAM_ACTIVE,
14  FOLLOWCAM_POSITION,
15  FOLLOWCAM_POSITION_X, //this HAS to come after FOLLOWCAM_POSITION in this list
16  FOLLOWCAM_POSITION_Y,
17  FOLLOWCAM_POSITION_Z,
18  FOLLOWCAM_FOCUS,
19  FOLLOWCAM_FOCUS_X, //this HAS to come after FOLLOWCAM_FOCUS in this list
20  FOLLOWCAM_FOCUS_Y,
21  FOLLOWCAM_FOCUS_Z,
22  FOLLOWCAM_POSITION_LOCKED,
23  FOLLOWCAM_FOCUS_LOCKED,
24  NUM_FOLLOWCAM_ATTRIBUTES
*/
namespace UtilLib {
    public partial class SetFollowCamPropertiesPanel : UserControl {
        private bool useVelocity;
        private Vector3 velocity;
        private DateTime lastVelocityUpdate;
        private double scale;
        private Quaternion rotation;
        private bool order;

        public SetFollowCamPropertiesPanel() {
            InitializeComponent();
            useVelocity = velocityCheckbox.Checked;
            focusPanel.Vector = new Vector3(1f, 0f, 0f);
        }

        public Vector3 Position {
            get { return rawPositionPanel.Value; }
            set { rawPositionPanel.Value = value; }
        }
        public Quaternion Rotation {
            get { return rawRotationPanel.Rotation; }
            set { rawRotationPanel.Rotation = value; }
        }
        public Vector3 Acceleration { get; set; }
        public Vector3 AngularAcceleration { get; set; }
        public Vector3 Velocity {
            get { return velocity; }
            set {
                velocity = value;
                lastVelocityUpdate = DateTime.Now;
            }
        }

        public SetFollowCamPropertiesPacket Packet {
            get {
                SetFollowCamPropertiesPacket packet = new SetFollowCamPropertiesPacket();
                packet.CameraProperty = new SetFollowCamPropertiesPacket.CameraPropertyBlock[22];
                for (int i = 0; i < 22; i++) {
                    packet.CameraProperty[i] = new SetFollowCamPropertiesPacket.CameraPropertyBlock();
                    packet.CameraProperty[i].Type = i + 1;
                }

                if (useVelocity) {
                    /*
                    (n - o) / v * diff = scale
                    n-o = scale * diff * v
                    n = (scale * diff * v) + o
                     */
                    double diff = DateTime.Now.Subtract(lastVelocityUpdate).TotalMilliseconds;
                    Position += Velocity * (float)(scale * diff);
                }
                Vector3 position = finalPositionPanel.Value;
                Vector3 focus = finalFocusPanel.Vector + position;

                packet.CameraProperty[0].Value = (float)type1Value.Value;
                packet.CameraProperty[1].Value = (float)type2Value.Value;
                packet.CameraProperty[2].Value = focusOffsetPanel.Value.X;
                packet.CameraProperty[3].Value = focusOffsetPanel.Value.Y;
                packet.CameraProperty[4].Value = focusOffsetPanel.Value.Z;
                packet.CameraProperty[5].Value = (float)type6Value.Value;
                packet.CameraProperty[6].Value = (float)type7Value.Value;
                packet.CameraProperty[7].Value = (float)type8Value.Value;
                packet.CameraProperty[8].Value = (float)type9Value.Value;
                packet.CameraProperty[9].Value = (float)type10Value.Value;
                packet.CameraProperty[10].Value = (float)type11Value.Value;
                packet.CameraProperty[11].Value = activeCheckbox.Checked ? 1f : 0f;
                packet.CameraProperty[12].Value = (float)type13Value.Value;
                packet.CameraProperty[13].Value = position.X;
                packet.CameraProperty[14].Value = position.Y;
                packet.CameraProperty[15].Value = position.Z;
                packet.CameraProperty[16].Value = (float)type17Value.Value;
                packet.CameraProperty[17].Value = focus.X;
                packet.CameraProperty[18].Value = focus.Y;
                packet.CameraProperty[19].Value = focus.Z;
                packet.CameraProperty[20].Value = positionLockedCheckbox.Checked ? 1f : 0f;
                packet.CameraProperty[21].Value = focusLockedCheckbox.Checked ? 1f : 0f;

                return packet;
            }
        }

        private void velocityCheckbox_CheckedChanged(object sender, EventArgs e) {
            useVelocity = velocityCheckbox.Checked;
        }

        private void velocityScaleValue_ValueChanged(object sender, EventArgs e) {
            scale = decimal.ToDouble(velocityScaleValue.Value);
        }

        private void orderCheckBox_CheckedChanged(object sender, EventArgs e) {
            order = orderCheckBox.Checked;
        }

        private void onChange(object sender, EventArgs e) {
            finalPositionPanel.Value = Position + (positionPanel.Value * Rotation);
            finalFocusPanel.Yaw = rawRotationPanel.Yaw + focusPanel.Yaw;
            finalFocusPanel.Pitch = rawRotationPanel.Pitch + focusPanel.Pitch;
        }
    }
}

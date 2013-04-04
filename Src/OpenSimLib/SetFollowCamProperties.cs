using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse.Packets;

namespace Chimera.OpenSim {
    class SetFollowCamProperties {
        public SetFollowCamPropertiesPacket Packet {
            set { }
                /*
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
                    double diff = DateTime.Now.Subtract(lastVelocityUpdate).TotalMilliseconds;
                    Position += Velocity * (float)(scale * diff);
                }
                Vector3 position = finalPositionPanel.Value;
                Vector3 focus = finalFocusPanel.LookAtVector + position;

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
            */
        }
    }
}

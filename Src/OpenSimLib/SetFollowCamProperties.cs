using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse.Packets;
using OpenMetaverse;
using GridProxy;

namespace Chimera.OpenSim {
    class SetFollowCamProperties {
        private float mPitch;
        private float mLookAtLag;
        private float mFocusLag;
        private float mDistance;
        private float mBehindnessAngle;
        private float mBehindnessLag = .5f;
        private float mLookAtThreshold;
        private float mFocusThreshold;
        private bool mSendPackets = true;
        private float mFocusOffset;
        private float mLookAt;
        private float mFocus;
        private Vector3 mFocusOffset3D = new Vector3(0f, 1f, 0f);

        private Proxy mProxy;
        private Coordinator mCoordinator;

        public float Pitch {
            get { return mPitch; }
            set {
                mPitch = value;
                Update();
            }
        }
        public float LookAtLag {
            get { return mLookAtLag; }
            set {
                mLookAtLag = value;
                Update();
            }
        }
        public float FocusLag {
            get { return mFocusLag; }
            set {
                mFocusLag = value;
                Update();
            }
        }
        public float Distance {
            get { return mDistance; }
            set {
                mDistance = value;
                Update();
            }
        }
        public float BehindnessAngle {
            get { return mBehindnessAngle; }
            set {
                mBehindnessAngle = value;
                Update();
            }
        }
        public float BehindnessLag {
            get { return mBehindnessLag; }
            set {
                mBehindnessLag = value;
                Update();
            }
        }
        public float LookAtThreshold {
            get { return mLookAtThreshold; }
            set {
                mLookAtThreshold = value;
                Update();
            }
        }
        public float FocusThreshold {
            get { return mFocusThreshold; }
            set {
                mFocusThreshold = value;
                Update();
            }
        }
        public bool SendPackets {
            get { return mSendPackets; }
            set {
                mSendPackets = value;
                Update();
            }
        }
        public float FocusOffset {
            get { return mFocusOffset; }
            set {
                mFocusOffset = value;
                Update();
            }
        }
        public float LookAt {
            get { return mLookAt; }
            set {
                mLookAt = value;
                Update();
            }
        }
        public float Focus {
            get { return mFocus; }
            set {
                mFocus = value;
                Update();
            }
        }
        public Vector3 FocusOffset3D {
            get { return mFocusOffset3D; }
            set {
                mFocusOffset3D = value;
                Update();
            }
        }

        public void SetProxy(Proxy proxy) {
            mProxy = proxy;
        }

        public SetFollowCamProperties(Coordinator coordinator) {
            mCoordinator = coordinator;
            mCoordinator.CameraModeChanged += new Action<Coordinator,ControlMode>(mCoordinator_CameraModeChanged);
        }

        private void mCoordinator_CameraModeChanged(Coordinator coordinator, ControlMode mode) {
            Update();
        }

        private void Update() {
            if (mProxy != null && mCoordinator.ControlMode == ControlMode.Delta)
                mProxy.InjectPacket(Packet, Direction.Incoming);
        }

        public SetFollowCamPropertiesPacket Packet {
            get {
                SetFollowCamPropertiesPacket packet = new SetFollowCamPropertiesPacket();
                packet.CameraProperty = new SetFollowCamPropertiesPacket.CameraPropertyBlock[22];
                for (int i = 0; i < 22; i++) {
                    packet.CameraProperty[i] = new SetFollowCamPropertiesPacket.CameraPropertyBlock();
                    packet.CameraProperty[i].Type = i + 1;
                }

                packet.CameraProperty[0].Value = mPitch;
                packet.CameraProperty[1].Value = mFocusOffset;
                packet.CameraProperty[2].Value = mFocusOffset3D.X;
                packet.CameraProperty[3].Value = mFocusOffset3D.Y;
                packet.CameraProperty[4].Value = mFocusOffset3D.Z;
                packet.CameraProperty[5].Value = mLookAtLag;
                packet.CameraProperty[6].Value = mFocusLag;
                packet.CameraProperty[7].Value = mDistance;
                packet.CameraProperty[8].Value = mBehindnessAngle;
                packet.CameraProperty[9].Value = mBehindnessLag;
                packet.CameraProperty[10].Value = mLookAtThreshold;
                packet.CameraProperty[11].Value = mSendPackets ? 1f : 0f;
                packet.CameraProperty[12].Value = 0f; //Position
                packet.CameraProperty[13].Value = 0f; //Position X
                packet.CameraProperty[14].Value = 0f; //Position Y
                packet.CameraProperty[15].Value = 0f; //Position Z
                packet.CameraProperty[16].Value = mFocus; //Focus
                packet.CameraProperty[17].Value = 0f; //Focus X
                packet.CameraProperty[18].Value = 0f; //Focus Y
                packet.CameraProperty[19].Value = 0f; //Focus Z
                packet.CameraProperty[20].Value = 0f; //Lock Positon
                packet.CameraProperty[21].Value = 0f; //Lock Focus
                //packet.CameraProperty[20].Value = positionLockedCheckbox.Checked ? 1f : 0f;
                //packet.CameraProperty[21].Value = focusLockedCheckbox.Checked ? 1f : 0f;

                return packet;
            }
        }
    }
}

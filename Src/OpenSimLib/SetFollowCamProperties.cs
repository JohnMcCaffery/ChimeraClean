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
using OpenMetaverse.Packets;
using OpenMetaverse;
using GridProxy;
using Chimera.Util;
using System.Threading;

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
        private Core mCoordinator;

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
        public bool ControlCamera {
            get { return mSendPackets; }
            set {
                if (mSendPackets != value) {
                    mSendPackets = value;
                    Update();
                    if (ControlCameraChanged != null)
                        ControlCameraChanged();
                }
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

        public event Action ControlCameraChanged;

        public void SetProxy(Proxy proxy) {
            mProxy = proxy;
        }

        private Action<Core, DeltaUpdateEventArgs> mDeltaListener;

        public SetFollowCamProperties(Core coordinator) {
            mCoordinator = coordinator;
            mCoordinator.CameraModeChanged += new Action<Core,ControlMode>(mCoordinator_CameraModeChanged);
            mDeltaListener = new Action<Core,DeltaUpdateEventArgs>(mCoordinator_DeltaUpdated);
            if (mCoordinator.ControlMode == ControlMode.Delta)
                mCoordinator.DeltaUpdated += mDeltaListener;
        }

        private bool mBackwards;
        private bool mFlying;

        void mCoordinator_CameraModeChanged(Core coordinator, ControlMode mode) {
            if (mCoordinator.ControlMode == ControlMode.Delta) {
                mCoordinator.DeltaUpdated += mDeltaListener;
                mCoordinator.Update(Vector3.Zero, Vector3.Zero, new Rotation(0.0, 1.0), Rotation.Zero);
                Thread.Sleep(1000);
                mCoordinator.Update(Vector3.Zero, Vector3.Zero, new Rotation(0.0, 0.0), Rotation.Zero);
            } else
                mCoordinator.DeltaUpdated -= mDeltaListener;

            Update();
        }

        void mCoordinator_DeltaUpdated(Core coordinator, DeltaUpdateEventArgs args) {
            ControlCamera = args.positionDelta.Z == 0f && args.positionDelta.X >= 0f;
            if (mFlying && args.positionDelta.Z <= 0f) {
                mFlying = false;
                if (args.rotationDelta.Yaw == 0.0) {
                    mCoordinator.Update(Vector3.Zero, args.positionDelta, Rotation.Zero, new Rotation(args.rotationDelta.Pitch, -.5));
                    Thread.Sleep(500);
                    mCoordinator.Update(Vector3.Zero, args.positionDelta, Rotation.Zero, new Rotation(args.rotationDelta.Pitch, .5));
                    Thread.Sleep(500);
                    mCoordinator.Update(Vector3.Zero, args.positionDelta, Rotation.Zero, args.rotationDelta);
                }
            } else if (!mFlying && args.positionDelta.Z > 0f)
                mFlying = true;

            if (mBackwards && args.positionDelta.X >= 0f) {
                mBackwards = false;
                if (args.positionDelta.X == 0f)
                    mCoordinator.Update(Vector3.Zero, new Vector3(1f, args.positionDelta.Y, args.positionDelta.Z), Rotation.Zero, args.rotationDelta);
                    Thread.Sleep(500);
                    mCoordinator.Update(Vector3.Zero, new Vector3(0f, args.positionDelta.Y, args.positionDelta.Z), Rotation.Zero, args.rotationDelta);
            } else if (!mBackwards && args.positionDelta.X < 0f)
                mBackwards = true;
        }

        public void Update() {
            if (mProxy != null && mCoordinator.ControlMode == ControlMode.Delta)
                mProxy.InjectPacket(Packet, Direction.Incoming);
        }

        public void Clear() {
            bool sendPackets = mSendPackets;
            mSendPackets = false;
            Update();
            mSendPackets = sendPackets;
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

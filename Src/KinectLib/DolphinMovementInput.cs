using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NuiLibDotNet;
using C = NuiLibDotNet.Condition;
using OpenMetaverse;
using System.Windows.Forms;
using Chimera.Util;
using Chimera.Kinect.GUI;

namespace Chimera.Kinect {
    public class DolphinMovementInput : IDeltaInput {
        private Scalar mWalkDiffR;
        private Scalar mWalkDiffL;
        private Scalar mWalkValR;
        private Scalar mWalkValL;
        private Scalar mWalkScale;
        private Scalar mWalkThreshold;

        private Scalar mFlyVal;
        private Scalar mFlyAngleR;
        private Scalar mFlyAngleL;
        private Scalar mFlyScale;
        private Scalar mFlyThreshold;
        private Scalar mFlyMax;
        private Scalar mFlyTimer;
        private Scalar mFlyMin;

        private Scalar mYawLean;
        private Scalar mYaw;
        private Scalar mYawScale;
        private Scalar mYawThreshold;

        private Scalar mWalkVal;
        private Vector3 mDelta;
        private double mPitchDelta, mYawDelta;

        private bool mWalkEnabled = true;
        private bool mFlyEnabled = true;
        private bool mYawEnabled = true;
        private bool mEnabled = true;

        private DateTime mFlyStart;
        private bool mFlying;

        private DolphinMovementPanel mPanel;

        public Scalar WalkVal { get { return mWalkVal; } }
        public Scalar WalkDiffR { get { return mWalkDiffR; } }
        public Scalar WalkDiffL { get { return mWalkDiffL; } }
        public Scalar WalkValR { get { return mWalkValR; } }
        public Scalar WalkValL { get { return mWalkValL; } }
        public Scalar WalkScale { get { return mWalkScale; } }
        public Scalar WalkThreshold { get { return mWalkThreshold; } }

        public Scalar FlyVal { get { return mFlyVal; } }
        public Scalar FlyAngleR { get { return mFlyAngleR; } }
        public Scalar FlyAngleL { get { return mFlyAngleL; } }
        public Scalar FlyScale { get { return mFlyScale; } }
        public Scalar FlyThreshold { get { return mFlyThreshold; } }
        public Scalar FlyMax { get { return mFlyMax; } }
        public Scalar FlyTimer { get { return mFlyTimer; } }
        public Scalar FlyMin { get { return mFlyMin; } }

        public Scalar YawLean { get { return mYawLean; } }
        public Scalar Yaw { get { return mYaw; } }
        public Scalar YawScale { get { return mYawScale; } }
        public Scalar YawThreshold { get { return mYawThreshold; } }

        public event Action<IInput, bool> EnabledChanged;

        public bool WalkEnabled {
            get { return mWalkEnabled; }
            set { 
                mWalkEnabled = value;
                if (EnabledChanged != null)
                    EnabledChanged(this, mEnabled);
            }
        }
        public bool StrafeEnabled {
            get { return false; }
            set { }
        }
        public bool FlyEnabled {
            get { return mFlyEnabled; }
            set { 
                mFlyEnabled = value;
                if (EnabledChanged != null)
                    EnabledChanged(this, mEnabled);
            }
        }
        public bool PitchEnabled {
            get { return false; }
            set { }
        }
        public bool YawEnabled {
            get { return mYawEnabled; }
            set {
                mYawEnabled = value;
                if (EnabledChanged != null)
                    EnabledChanged(this, mEnabled);
            }
        }

        public DolphinMovementInput () {
            mWalkEnabled = true;
            mFlyEnabled = true;
            mYawEnabled = true;
            //(val * scale) + min = out
            //Scalar walkThreshold = Nui.tracker("WalkStart", 40, .65f / 40f, 0f, 20);
            mWalkThreshold = Scalar.Create(.325f);
            mWalkScale = Scalar.Create(1f);

            //Scalar flyThreshold = .2f - Nui.tracker("FlyStart", 40, .2f / 40f, 0f, 20);
            //Scalar flyMax = Nui.tracker("FlyMax", 40, .2f / 40f, .2f, 20);
            mFlyThreshold = Scalar.Create(.1f);
            mFlyMax = Scalar.Create(.3f);
            mFlyScale = Scalar.Create(.25f);

            //Scalar yawD = .1f - Nui.tracker("YawStart", 40, .1f / 40f, 0f, 20);
            //Scalar yawSpeed = 10f - Nui.tracker("YawSpeed", 40, 15f / 40f, 0f, 20);
            mYawThreshold = Scalar.Create(0.05f);
            mYawScale = Scalar.Create(1f);

            Init();

            Nui.Tick += new ChangeDelegate(Nui_Tick);
        }

        void Nui_Tick() {
            mDelta = Vector3.Zero;
            mYawDelta = mYawEnabled ? -mYaw.Value : 0f;
            mPitchDelta = 0.0;

            mDelta.X = mWalkEnabled ? mWalkVal.Value : 0f;

            //Put a timer on flying so that fly input greater than x will be ignored until the user has stayed in the position for y ms.
            if (mFlyVal.Value != 0f && !mFlying) {
                mFlying = true;
                if (mFlyVal.Value < mFlyMin.Value) {
                    mFlyAllowed = true;
                    mDelta.Z = mFlyEnabled ? mFlyVal.Value : 0f;
                } else 
                    mFlyStart = DateTime.Now;
            } else if (mFlyAllowed || DateTime.Now.Subtract(mFlyStart).TotalMilliseconds > mFlyTimer.Value)
                mDelta.Z = mFlyEnabled ? mFlyVal.Value : 0f;
            else if (mFlyVal.Value == 0f) {
                mFlying = false;
                mFlyAllowed = true;
            }

            if (mEnabled && Change != null)
                Change(this);
        }
        private bool mFlyAllowed;

        private void Init() {
            //Get the primary vectors.
            Vector shoulderR = Nui.joint(Nui.Shoulder_Right);
            Vector shoulderL = Nui.joint(Nui.Shoulder_Left);
            Vector elbowR = Nui.joint(Nui.Elbow_Right);
            Vector elbowL = Nui.joint(Nui.Elbow_Left);
            Vector wristR = Nui.joint(Nui.Wrist_Right);
            Vector wristL = Nui.joint(Nui.Wrist_Left);
            Vector handR = Nui.joint(Nui.Hand_Right);
            Vector handL = Nui.joint(Nui.Hand_Left);
            Vector hipC = Nui.joint(Nui.Hip_Centre);
            Vector head = Nui.joint(Nui.Head);
            //Condition guard = closeGuard();

            Vector armR = handR - shoulderR;
            Vector armL = handL - shoulderL;

            //----------- Walk----------- 
            //Left and right
            mWalkDiffR = Nui.z(armR);
            mWalkDiffL = Nui.z(armL);
            //Active
            Condition walkActiveR = C.Or(C.And(Nui.abs(mWalkDiffR) > mWalkThreshold,  mWalkDiffR < 0f), (mWalkDiffR > mWalkThreshold / 5f));
            Condition walkActiveL = C.Or(C.And(Nui.abs(mWalkDiffL) > mWalkThreshold,  mWalkDiffL < 0f), (mWalkDiffL > mWalkThreshold / 5f));
            //Value
            Scalar moveValR = Nui.ifScalar(mWalkDiffR < 0f, mWalkDiffR + mWalkThreshold, mWalkDiffR - (mWalkThreshold / 5f));
            Scalar moveValL = Nui.ifScalar(mWalkDiffL < 0f, mWalkDiffL + mWalkThreshold, mWalkDiffL - (mWalkThreshold / 5f));
            mWalkValR = Nui.ifScalar(walkActiveR, moveValR, 0f);
            mWalkValL = Nui.ifScalar(walkActiveL, moveValL, 0f);
            mWalkVal = (mWalkValL + mWalkValR) * -1f;


            //----------- Fly----------- 
            mFlyAngleR = Nui.constrain(Nui.dot(Vector.Create(0f, 1f, 0f), armR), mFlyThreshold, mFlyMax, 0f, true);
            mFlyAngleL = Nui.constrain(Nui.dot(Vector.Create(0f, 1f, 0f), armL), mFlyThreshold, mFlyMax, 0f, true);
            Scalar flyVal = (mFlyAngleR + mFlyAngleL) * mFlyScale;
            Condition flyActiveR = C.And(mFlyAngleR != 0f,  Nui.magnitude(armR) - Nui.magnitude(Nui.limit(armR, true, true, false)) < .1f);
            Condition flyActiveL = C.And(mFlyAngleL != 0f,  Nui.magnitude(armL) - Nui.magnitude(Nui.limit(armL, true, true, false)) < .1f);
            Condition flyActive = C.And(C.Or(flyActiveR, flyActiveL),  C.And(Nui.z(armR) < 0f,  Nui.z(armR) < 0f));
            mFlyVal = Nui.ifScalar(flyActive, flyVal, 0f);
            mFlyTimer = Scalar.Create("Fly Timer", 0f);
            mFlyMin = Scalar.Create("Fly Minimum", .01f);

            //----------- Yaw----------- 
            Vector yawCore = Nui.limit(head - hipC, true, true, false);
            // Yaw is how far the user is leaning horizontally. This is calculated the angle between vertical and the vector between the hip centre and the head.
            mYawLean = Nui.dot(Nui.normalize(yawCore), Vector.Create(1f, 0f, 0f));
            // Constrain the value, deadzone is provided by a slider.
            mYaw = Nui.constrain(mYawLean, mYawThreshold, .2f, .3f, true) * mYawScale;
        }

        #region IDeltaInput Members 

        public event Action<IDeltaInput> Change;

        public Vector3 PositionDelta {
            get { return mDelta; }
        }

        public Rotation OrientationDelta {
            get { return new Rotation(mPitchDelta, mYawDelta); }
        }

        public void Init(IInputSource input) { }

        public UserControl ControlPanel {
            get {
                if (mPanel == null)
                    mPanel = new DolphinMovementPanel(this);
                return mPanel;
            }
        }

        public bool Enabled {
            get { return mEnabled; }
            set { 
                mEnabled = value;
                if (EnabledChanged != null)
                    EnabledChanged(this, mEnabled);
            }
        }

        public string Name {
            get { return "Kinect Movement - Dolphin Configuration"; }
        }

        public string State {
            get {
                string dump = "----Dolphin Config Kinect Input----";
                return ""; 
            }
        }

        public ConfigBase Config {
            get { throw new NotImplementedException(); }
        }

        public void Close() { }

        public void Draw(Perspective perspective, System.Drawing.Graphics graphics) {
            throw new NotImplementedException();
        }

        #endregion

    }
}

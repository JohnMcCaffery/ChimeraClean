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
using NuiLibDotNet;
using C = NuiLibDotNet.Condition;
using OpenMetaverse;
using System.Windows.Forms;
using Chimera.Util;
using Chimera.Kinect.GUI;
using System.Drawing;
using Chimera.Plugins;

namespace Chimera.Kinect {
    public class TimespanMovementInput : DeltaBasedPlugin {
        private Scalar mWalkDiffR;
        private Scalar mWalkDiffL;
        private Scalar mWalkValR;
        private Scalar mWalkValL;
        private Scalar mWalkScale;
        private Scalar mWalkThreshold;

        private Vector mArmR;
        private Vector mArmL;
        private Scalar mFlyVal;
        private Scalar mFlyAngleR;
        private Scalar mFlyAngleL;
        private Scalar mConstrainedFlyAngleR;
        private Scalar mConstrainedFlyAngleL;
        private Scalar mFlyScale;
        private Scalar mFlyThreshold;
        private Scalar mFlyMax;
        private Scalar mFlyTimer;
        private Scalar mFlyMin;

        private Scalar mYawLean;
        private Scalar mYawTwist;
        private Scalar mYaw;
        private Scalar mYawScale;
        private Scalar mYawThreshold;

        private Scalar mWalkVal;
        private Vector3 mDelta;
        private double mPitchDelta, mYawDelta;

        private DateTime mFlyStart;
        private bool mFlying;

        private TimespanMovementPanel mPanel;

        public Scalar WalkVal { get { return mWalkVal; } }
        public Scalar WalkDiffR { get { return mWalkDiffR; } }
        public Scalar WalkDiffL { get { return mWalkDiffL; } }
        public Scalar WalkValR { get { return mWalkValR; } }
        public Scalar WalkValL { get { return mWalkValL; } }
        public Scalar WalkScale { get { return mWalkScale; } }
        public Scalar WalkThreshold { get { return mWalkThreshold; } }

        public Vector ArmR { get { return mArmR; } }
        public Vector ArmL { get { return mArmL; } }
        public Scalar FlyVal { get { return mFlyVal; } }
        public Scalar FlyAngleR { get { return mFlyAngleR; } }
        public Scalar FlyAngleL { get { return mFlyAngleL; } }
        public Scalar ConstrainedFlyAngleR { get { return mConstrainedFlyAngleR; } }
        public Scalar ConstrainedFlyAngleL { get { return mConstrainedFlyAngleL; } }
        public Scalar FlyScale { get { return mFlyScale; } }
        public Scalar FlyThreshold { get { return mFlyThreshold; } }
        public Scalar FlyMax { get { return mFlyMax; } }
        public Scalar FlyTimer { get { return mFlyTimer; } }
        public Scalar FlyMin { get { return mFlyMin; } }

        public Scalar YawLean { get { return mYawLean; } }
        public Scalar YawTwist { get { return mYawTwist; } }
        public Scalar Yaw { get { return mYaw; } }
        public Scalar YawScale { get { return mYawScale; } }
        public Scalar YawThreshold { get { return mYawThreshold; } }


        public TimespanMovementInput () {
            //Scalar walkThreshold = Nui.tracker("WalkStart", 40, .65f / 40f, 0f, 20);
            mWalkThreshold = Scalar.Create(.325f);
            mWalkScale = Scalar.Create(2f);

            //Scalar flyThreshold = .2f - Nui.tracker("FlyStart", 40, .2f / 40f, 0f, 20);
            //Scalar flyMax = Nui.tracker("FlyMax", 40, .2f / 40f, .2f, 20);
            mFlyThreshold = Scalar.Create(.05f);
            mFlyMax = Scalar.Create(.4f);
            mFlyScale = Scalar.Create(.5f);

            //Scalar yawD = .1f - Nui.tracker("YawStart", 40, .1f / 40f, 0f, 20);
            //Scalar yawSpeed = 10f - Nui.tracker("YawSpeed", 40, 15f / 40f, 0f, 20);
            mYawThreshold = Scalar.Create(0.05f);
            mYawScale = Scalar.Create(1f);

            Init();

            Nui.Tick += new ChangeDelegate(Nui_Tick);
        }

        bool sw = false;

        void Nui_Tick() {
            if (!Nui.HasSkeleton)
                return;

            mDelta = Vector3.Zero;
            mYawDelta = -mYaw.Value;
            mPitchDelta = 0.0;

            mDelta.X = mWalkVal.Value;

            //Put a timer on flying so that fly input greater than x will be ignored until the user has stayed in the position for y ms.
            if (mFlyVal.Value != 0f && !mFlying) {
                mFlying = true;
                if (!mFlyAllowed && mFlyVal.Value < mFlyMin.Value) {
                    mFlyAllowed = true;
                    mDelta.Z = mFlyVal.Value;
                    Console.WriteLine("Below Threshold");
                } else 
                    mFlyStart = DateTime.Now;
            } else if (mFlyAllowed || DateTime.Now.Subtract(mFlyStart).TotalMilliseconds > mFlyTimer.Value) {
                mDelta.Z = mFlyVal.Value;
                //Console.WriteLine("Time trigger " + (sw ? "+" : "-"));
                sw = !sw;
            }else if (mFlyVal.Value == 0f) {
                mFlying = false;
                mFlyAllowed = true;
            }

            TriggerChange(this);
        }
        private bool mFlyAllowed;

        private void Init() {
            //Get the primary vectors.
            Vector shoulderC = Nui.joint(Nui.Shoulder_Centre);
            Vector shoulderR = Nui.joint(Nui.Shoulder_Right);
            Vector shoulderL = Nui.joint(Nui.Shoulder_Left);
            Vector elbowR = Nui.joint(Nui.Elbow_Right);
            Vector elbowL = Nui.joint(Nui.Elbow_Left);
            Vector wristR = Nui.joint(Nui.Wrist_Right);
            Vector wristL = Nui.joint(Nui.Wrist_Left);
            Vector handR = Nui.joint(Nui.Hand_Right);
            Vector handL = Nui.joint(Nui.Hand_Left);
            Vector hipC = Nui.joint(Nui.Hip_Centre);
            Vector hipR = Nui.joint(Nui.Hip_Right);
            Vector hipL = Nui.joint(Nui.Hip_Left);
            Vector head = Nui.joint(Nui.Head);
            //Condition guard = closeGuard();

            Condition heightThresholdR = Nui.y(handR) > Nui.y(hipC);
            Condition heightThresholdL = Nui.y(handL) > Nui.y(hipC);
            Condition heightThreshold = C.Or(heightThresholdL, heightThresholdR);

            Scalar dist = Nui.magnitude(shoulderC - hipC);
            Condition distanceThresholdR = Nui.x(handR - hipR) > dist;
            Condition distanceThresholdL = Nui.x(hipL - handL) > dist;
            //Condition distanceThresholdR = Nui.magnitude(handR - hipR) > dist;
            //Condition distanceThresholdL = Nui.magnitude(hipL - handL) > dist;
            Condition distanceThreshold = C.Or(distanceThresholdL, distanceThresholdR);

            Condition activeConditionR = C.Or(heightThresholdR, distanceThresholdR);
            Condition activeConditionL = C.Or(heightThresholdL, distanceThresholdL);
            Condition mActiveCondition = C.Or(heightThreshold, distanceThreshold);

            //----------- Walk----------- 
            //Left and right
            mWalkDiffR = Nui.z(handR - hipC) - .15f;
            mWalkDiffL = Nui.z(handL - hipC) - .15f;
            Scalar backwardThresh = mWalkThreshold / 5f;
            //Active
            Condition walkActiveR = C.Or(C.And(Nui.abs(mWalkDiffR) > mWalkThreshold,  mWalkDiffR < 0f), (mWalkDiffR > backwardThresh));
            Condition walkActiveL = C.Or(C.And(Nui.abs(mWalkDiffL) > mWalkThreshold,  mWalkDiffL < 0f), (mWalkDiffL > backwardThresh));
            walkActiveR = C.And(activeConditionR, walkActiveR);
            walkActiveL = C.And(activeConditionL, walkActiveL);
            //Value
            Scalar moveValR = Nui.ifScalar(mWalkDiffR < 0f, mWalkDiffR + mWalkThreshold, mWalkDiffR - backwardThresh);
            Scalar moveValL = Nui.ifScalar(mWalkDiffL < 0f, mWalkDiffL + mWalkThreshold, mWalkDiffL - backwardThresh);
            mWalkValR = Nui.ifScalar(walkActiveR, moveValR, 0f);
            mWalkValL = Nui.ifScalar(walkActiveL, moveValL, 0f);
            mWalkVal = (mWalkValL + mWalkValR) * -1f * mWalkScale;
            //mWalkVal = Nui.ifScalar(mActiveCondition, mWalkVal, 0f);


            //----------- Fly----------- 
            mArmR = handR - shoulderR;
            mArmL = handL - shoulderL;
            Vector up = Vector.Create(0f, 1f, 0f);
            mFlyAngleR = Nui.dot(up, mArmR);
            mFlyAngleL = Nui.dot(up, mArmL);
            mConstrainedFlyAngleR = Nui.constrain(mFlyAngleR, mFlyThreshold, mFlyMax, 0f, true);
            mConstrainedFlyAngleL = Nui.constrain(mFlyAngleL, mFlyThreshold, mFlyMax, 0f, true);
            Scalar flyVal = (mConstrainedFlyAngleR + mConstrainedFlyAngleL) * mFlyScale;

            //Condition flyActiveR = C.And(mFlyAngleR != 0f,  Nui.magnitude(armR) - Nui.magnitude(Nui.limit(armR, true, true, false)) < .1f);
            //Condition flyActiveL = C.And(mFlyAngleL != 0f,  Nui.magnitude(armL) - Nui.magnitude(Nui.limit(armL, true, true, false)) < .1f);
            Scalar magShoulder = Nui.magnitude(shoulderL - shoulderR);
            Scalar magR = Nui.magnitude(mArmR);
            Scalar magL = Nui.magnitude(mArmL);
            Condition flyActiveR = C.And(magR > magShoulder, Nui.abs(Nui.x(mArmR)) > Nui.abs(Nui.z(mArmR)));
            Condition flyActiveL = C.And(magL > magShoulder, Nui.abs(Nui.x(mArmL)) > Nui.abs(Nui.z(mArmL)));
            flyActiveR = C.And(mConstrainedFlyAngleR != 0f, flyActiveR);
            flyActiveL = C.And(mConstrainedFlyAngleL != 0f, flyActiveL);
            flyActiveR = C.And(activeConditionR, flyActiveR);
            flyActiveL = C.And(activeConditionL, flyActiveL);
            Condition flyActive = C.Or(flyActiveR, flyActiveL);
            //Condition flyActive = C.And(C.Or(flyActiveR, flyActiveL),  C.And(Nui.z(armR) < 0f,  Nui.z(armR) < 0f));

            mFlyVal = Nui.ifScalar(flyActive, flyVal, 0f);
            mFlyVal = Nui.ifScalar(mActiveCondition, mFlyVal, 0f);
            mFlyTimer = Scalar.Create("Fly Timer", 0f);
            mFlyMin = Scalar.Create("Fly Minimum", .01f);

            //----------- Yaw----------- 
            Vector yawCore = Nui.limit(head - hipC, true, true, false);
            // Yaw is how far the user is leaning horizontally. This is calculated the angle between vertical and the vector between the hip centre and the head.
            mYawLean = Nui.dot(Nui.normalize(yawCore), Vector.Create(1f, 0f, 0f));
            // Constrain the value, deadzone is provided by a slider.
            mYaw = Nui.constrain(mYawLean, mYawThreshold, .4f, .3f, true) * mYawScale;

            mYawTwist = Nui.z(shoulderR) - Nui.z(shoulderL);
            mYawTwist = Nui.constrain(mYawTwist, .05f, Nui.magnitude(shoulderL - shoulderR), 5f, true);

            mYaw = mYawTwist + mYawLean;
        }

        #region IDeltaInput Members 

        public override Vector3 PositionDelta {
            get { return mDelta; }
        }

        public override Rotation OrientationDelta {
            get { return new Rotation(mPitchDelta, mYawDelta); }
        }

        public override UserControl ControlPanel {
            get {
                if (mPanel == null)
                    mPanel = new TimespanMovementPanel(this);
                return mPanel;
            }
        }

        public override string Name {
            get { return "Kinect Movement - Timespan Configuration"; }
        }

        public override string State {
            get {
                string dump = "----Timespan Config Kinect Input----";
                return ""; 
            }
        }

        public override ConfigBase Config {
            get { throw new NotImplementedException(); }
        }

        public override void Close() { }

        public override void Draw(Func<Vector3, Point> to2D, Graphics graphics, Action redraw) {
            //Do nothing
        }

        #endregion

    }
}

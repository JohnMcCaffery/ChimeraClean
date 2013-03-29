using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilLib;
using Chimera.Flythrough;
using OpenMetaverse;
using NuiLibDotNet;
using C = NuiLibDotNet.Condition;
using Chimera;

namespace KinectLib {
    public enum State { Positive, Negative, Dormant }

    public class KinectManager {
        private static float R2DEG = (float) (180.0 /  Math.PI);
        private CameraMaster mMaster;
        private FlythroughManager mFlythrough;
        private Vector3 mPosition = new Vector3(0f, 0f, 0f);
        private Rotation mRotation = new Rotation(0f, 180f);
        private Vector3 mHead;
        private Vector3 mPoint;
        private int mScreen;
        private double mX, mY;
        private State mFlyState;
        private State mWalkState;
        private State mTurnState;
        private bool mControlCamera;
        private Vector mPointStart;
        private Vector mPointDir;

        private readonly List<PointSurface> mSurfaces = new List<PointSurface>();
        private readonly List<ActiveArea.Data> mActiveAreas = new List<ActiveArea.Data>();

        public State FlyState { get { return mWalkState; } }
        public State WalkState { get { return mWalkState; } }
        public State TurnState { get { return mFlyState; } }
        public Vector PointStart { get { return mPointStart; } }
        public Vector PointDir { get { return mPointDir; } }

        public PointSurface[] Surfaces {
            get { return mSurfaces.ToArray(); }
        }
        public ActiveArea.Data[] ActiveAreas {
            get { return mActiveAreas.ToArray(); }
        }

        public Vector3 KinectPosition { 
            get { return mPosition; }
            set { 
                mPosition = value;
                if (PositionChange != null)
                    PositionChange();
            }
        }
        public Rotation KinectRotation { get { return mRotation; } }

        public event Action PositionChange;
        public event Action<PointSurface> SurfaceAdded;

        public void Init(CameraMaster master, FlythroughManager flythrough) {
            mMaster = master;
            mFlythrough = flythrough;

            foreach (var window in master.Slaves.Select(s => s.Window))
                AddWindow(window);

            master.OnSlaveConnected += slave => AddWindow(slave.Window);
        }

        public void AddActiveArea(ActiveArea.Data data) {
            mActiveAreas.Add(data);
        }

        public PointSurface AddWindow(Window window) {
            PointSurface surface = new PointSurface(this, window);

            mSurfaces.Add(surface);

            if (SurfaceAdded != null)
                SurfaceAdded(surface);

            return surface;
        }

        public KinectManager() {
            Nui.Init();
            Nui.SetAutoPoll(true);
            Vector pointEnd = Nui.joint(Nui.Hand_Right);
            mPointStart = Nui.joint(Nui.Shoulder_Right);
            mPointDir = mPointStart - pointEnd;
            //mPointStart = Vector.CreateWindowState("Point Begin", 0f, 0f, 10f);
            //mPointDir = Vector.CreateWindowState("Point Dir", 0f, 0f, 1f);
        }

        #region Move
        private void InitMove() {
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

            Vector yAxis = Vector.Create("Y", 0f, 1f, 0f);
            // Normal is the direction the camera is facing.
            Vector normal = Vector.Create("Normal", 0, 0, 1);

            //Camera - If the right elbow is raised to be in line with the shoulders the camera is active.
            Vector upperArmCameraR = elbowR - shoulderR;
            Vector lowerArmCameraR = elbowR - wristR;
            Condition cameraActiveR = Nui.abs(Nui.x(upperArmCameraR)) > (Nui.abs(Nui.y(upperArmCameraR)) + Nui.abs(Nui.z(upperArmCameraR))) * 2f;

            //Camera - If the right elbow is raised to be in line with the shoulders the camera is active.
            Vector upperArmCameraL = shoulderL - elbowL;
            Vector lowerArmCameraL = elbowL - wristL;
            Condition cameraActiveL = Nui.abs(Nui.x(upperArmCameraL)) > (Nui.abs(Nui.y(upperArmCameraL)) + Nui.abs(Nui.z(upperArmCameraL))) * 2f;

            Condition cameraActive = C.Or(cameraActiveL, cameraActiveR);
            //Condition cameraActive = cameraActiveR;

            // Normalize the distance between the shoulder and the right hand against the total length of the arm (armMax).
            // Once normalized constrain the value between the input transition tracker PushD (starting at .8) and 1. 
            // So if the normalized value is .9 the constrained value is .5. Alternatively if the normalized value is .8 the constrained value is 0.
            //Scalar push = constrain(normalize(magnitude(armR), armMax), tracker("PushD", 25, .04f, 0f, 20), 1f, 0f, false);
            // If the camera is inactive and the push value is > 0 and the forward component is significantly larger than the horizontal and vertical components combined.
            //mPush = !cameraActive && push > 0 && z(armR) > ((abs(x(armR) + abs(y(armR)))) * tracker("PushActive", 9, .5f, .5f, 5));

            //Pitch
            Scalar pitchArmD = Nui.tracker("PitchArmD", 20f * 1f, 1f, 1f * 10f);
            Scalar pitchArmR = Nui.tracker("PitchArmR", 40f * 2f, 2f, 2f * 17f);
            Scalar pitchArmG = Nui.tracker("PitchArmG", 30f * 1f, 1f, 1f * 15f);
            Scalar pitchAS = Nui.tracker("PitchAS", 29f * 1f, 1f, 1f * 20f);

            Vector vPlaneCameraR = Nui.limit(lowerArmCameraR, false, true, true);
            Vector vPlaneCameraL = Nui.limit(lowerArmCameraL, false, true, true);
            // Pitch is the angle between normal and the vertical component of the vector between right shoulder and right hand.
            Scalar pitchR = Nui.acos(Nui.dot(Nui.normalize(vPlaneCameraR), normal)) * Nui.invert(Nui.x(Nui.cross(normal, vPlaneCameraR)) >= 0);
            Scalar pitchL = Nui.acos(Nui.dot(Nui.normalize(vPlaneCameraL), normal)) * Nui.invert(Nui.x(Nui.cross(normal, vPlaneCameraL)) >= 0);
            // Constrain the pitch value by 3 values input by 3 trackers.
            pitchR = Nui.constrain(pitchR * R2DEG, pitchArmD, pitchArmR, pitchArmG, true) / pitchAS;
            pitchL = Nui.constrain(pitchL * R2DEG, pitchArmD, pitchArmR, pitchArmG, true) / pitchAS;
            Scalar mPitch = Nui.ifScalar(cameraActiveR, pitchR, .0f) + Nui.ifScalar(cameraActiveL, pitchL, .0f);

            Condition mCanPitch = C.And(cameraActive, mPitch != 0f);
            //Condition mCanPitch = cameraActive && mPitch != 0f;

            //Yaw - Yaw has 3 components. The camera arm. The horizontal lean (head vs hip centre) and the twist of the shoulders.
            Scalar yawArmD = Nui.tracker("YawArmD", 20f * 1f, 1f, 1f * 10f);
            Scalar yawArmR = Nui.tracker("YawArmR", 40f * 2f, 2f, 2f * 15f);
            Scalar yawArmG = Nui.tracker("YawArmG", 20f * 1f, 1f, 1f * 10f);
            Scalar yawLeanD = Nui.tracker("YawLeanD", 20f * .5f, .5f, .5f * 10f);
            Scalar yawLeanR = Nui.tracker("YawLeanR", 20f * 1f, 1f, 1f * 15f);
            Scalar yawLeanG = Nui.tracker("YawLeanG", 50f * 1f, 1f, 1f * 30f);
            Scalar yawTwistD = Nui.tracker("YawTwistD", 10f * .025f, .025f, .025f * 6f);
            Scalar yawTwistR = Nui.tracker("YawTwistR", 20f * .05f, .05f, .05f * 9f);
            Scalar yawTwistG = Nui.tracker("YawTwistG", 20f * 1f, 1f, 1f * 10f);

            Vector hPlaneCameraR = Nui.limit(lowerArmCameraR, true, false, true);
            Vector hPlaneCameraL = Nui.limit(lowerArmCameraL, true, false, true);
            // Yaw component 1 is the angle between normal and the horizontal component of the vector between right shoulder and right hand.
            Scalar yawCameraR = Nui.acos(Nui.dot(Nui.normalize(hPlaneCameraR), normal)) * Nui.invert(Nui.y(Nui.cross(normal, hPlaneCameraR)) >= 0);
            Scalar yawCameraL = Nui.acos(Nui.dot(Nui.normalize(hPlaneCameraL), normal)) * Nui.invert(Nui.y(Nui.cross(normal, hPlaneCameraL)) >= 0);
            // Constrain the component value by 3 values input by 3 trackers.
            yawCameraR = Nui.constrain(yawCameraR * R2DEG, yawArmD, yawArmR, yawArmG, true) / Nui.tracker("YawAS", 29f * 1f, 1f, 1f * 20f);
            yawCameraL = Nui.constrain(yawCameraL * R2DEG, yawArmD, yawArmR, yawArmG, true) / Nui.tracker("YawAS", 29f * 1f, 1f, 1f * 20f);
            //Only take the value if camera is active
            yawCameraR = Nui.ifScalar(cameraActiveR, yawCameraR, .0f);
            yawCameraL = Nui.ifScalar(cameraActiveL, yawCameraL, .0f);

            Vector yawCore = Nui.limit(head - hipC, true, true, false);
            // Yaw component 2 is how far the user is leaning horizontally. This is calculated the angle between vertical and the vector between the hip centre and the head.
            Scalar yawLean = Nui.acos(Nui.dot(Nui.normalize(yawCore), yAxis)) * Nui.invert(Nui.z(Nui.cross(yawCore, yAxis)) >= 0);
            // Constrain the component value by 3 values input by 3 trackers.
            yawLean = Nui.constrain(yawLean * R2DEG, yawLeanD, yawLeanR, yawLeanG, true) / Nui.tracker("YawLS", 29f * 1f, 1f, 1f * 20f);

            Vector shoulderDiff = shoulderR - shoulderL;
            // Yaw component 3 is the twist of the shoulders. This is calculated as the difference between the two z values.
            Scalar yawTwist = Nui.z(shoulderDiff) / Nui.magnitude(shoulderDiff);
            // Constrain the component value by 3 values input by 3 trackers.
            yawTwist = Nui.constrain(yawTwist, yawTwistD, yawTwistR, yawTwistG, true) / Nui.tracker("YawTS", 29f * 1f, 1f, 1f * 20f);

            // Combine all 3 components into the final yaw value.
            Scalar mYaw = yawCameraR + yawCameraL + yawLean + yawTwist;
            Condition mCanYaw = C.Or(C.And(cameraActive, (yawCameraR + yawCameraL) != 0f), C.Or(yawLean != 0f, yawTwist != 0f));
            //Condition mCanYaw = (cameraActive && (yawCameraR + yawCameraL) != 0f) || yawLean != 0f || yawTwist != 0f;

            Scalar flyUpD = Nui.tracker("FlyUpD", 120f * 1f, 1f, 1f * 65f);
            Scalar flyUpR = Nui.tracker("FlyUpR", 120f * 1f, 1f, 1f * 50f);
            Scalar flyDownD = Nui.tracker("FlyDownD", 120f * 1f, 1f, 1f * 45f);
            Scalar flyDownR = Nui.tracker("FlyDownR", 120f * 1f, 1f, 1f * 15f);

            //Fly
            Vector armR = shoulderR - handR;
            Vector vPlaneR = Nui.limit(armR, false, true, true);
            // The angle between normal and the vector between the shoulder and the hand.
            Scalar flyR = Nui.acos(Nui.dot(Nui.normalize(vPlaneR), normal));
            // Constrain the positive angle to go up past vertical.
            Scalar upR = Nui.constrain(flyR * R2DEG, flyUpD, flyUpR, 0f, true); //Constraints if R is raised
            // Constrain the negative angle to stop before vertical so that hands lying by the side doesn't trigger flying down.
            Scalar downR = Nui.constrain(flyR * R2DEG, flyDownD, flyDownR, 0f, true); //Constraints if R is lowered
            // Whether the arm is raised or lowered.
            Condition dirR = Nui.x(Nui.cross(normal, vPlaneR)) >= 0;
            // Whether R is in range to fly
            Condition flyCondR = C.And(Nui.magnitude(vPlaneR) > 0f, C.Or(C.And(dirR, (upR > 0)), C.And((!dirR), (downR > 0))));
            //Condition flyCondR = (Nui.magnitude(vPlaneR) > 0f) && ((dirR && (upR > 0)) || ((!dirR) && (downR > 0)));

            Vector armL = shoulderL - handL;
            Vector vPlaneL = Nui.limit(armL, false, true, true);
            // The angle between normal and the vector between the shoulder and the hand.
            Scalar flyL = Nui.acos(Nui.dot(Nui.normalize(vPlaneL), normal)) * R2DEG;
            // Constrain the positive angle to go up past vertical.
            Scalar upL = Nui.constrain(flyL, flyUpD, flyUpR, 0f, true); //Constraints if L is raised
            // Constrain the negative angle to stop before vertical so that hands lying by the side doesn't trigger flying down.
            Scalar downL = Nui.constrain(flyL, flyDownD, flyDownR, 0f, true); //Constraints if L is lowered
            // Whether the arm is raised or lowered.
            Condition dirL = Nui.x(Nui.cross(normal, vPlaneL)) >= 0;
            // Whether L is in range to fly
            Condition flyCondL = C.And(Nui.magnitude(vPlaneL) > 0f, C.Or(C.And(dirL, upL > 0f), C.And(!dirL, downL > 0f)));
            //Condition flyCondL = Nui.magnitude(vPlaneL) > 0f && (dirL && upL > 0f) || (!dirL && downL > 0f);

            //Up trumps down
            Condition mFly = C.Or(C.And(dirR, flyCondR), C.And(dirL, flyCondL));
            //Condition mFly = (dirR && flyCondR) || (dirL && flyCondL);
            // Fly if camera is inactive and flying with right or left arm
            Condition mCanFly = C.Or(C.And(flyCondR, !cameraActiveR), C.And(flyCondL, !cameraActiveL));
            //Condition mCanFly = (flyCondR && !cameraActiveR) || (flyCondL && !cameraActiveL);

            Scalar pushThresh = Nui.tracker("PushThreshold", 30f * .05f, 0f, .05f * 9f);
            Condition pushR = Nui.z(shoulderR) - Nui.z(handR) > pushThresh;
            Condition pushL = Nui.z(shoulderL) - Nui.z(handL) > pushThresh;
            Condition mPush = C.Or(C.And(pushR, !cameraActiveR), C.And(pushL, !cameraActiveL));
            //Condition mPush = (pushR && !cameraActiveR) || (pushL && !cameraActiveL);

            Condition mCanMove = C.Or(C.Or(mPush, mCanYaw), C.Or(mCanPitch, mCanFly));
            //Condition mCanMove = mPush || mCanYaw || mCanPitch || mCanFly;
        }
        #endregion
    }
}
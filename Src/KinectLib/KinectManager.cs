using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilLib;
using FlythroughLib;
using OpenMetaverse;
using NuiLibDotNet;

namespace KinectLib {
    public enum State { Positive, Negative, Dormant }

    public class KinectManager {
        private static float R2DEG = (float) (180.0 /  Math.PI);
        private CameraMaster mMaster;
        private FlythroughManager mFlythrough;
        private Vector3 mPosition;
        private Rotation mRotation;
        private Vector3 mHead;
        private Vector3 mPoint;
        private int mScreen;
        private double mX, mY;
        private State mFlyState;
        private State mWalkState;
        private State mTurnState;
        private bool mControlCamera;

        public State FlyState { get { return mWalkState; } }
        public State WalkState { get { return mWalkState; } }
        public State TurnState { get { return mFlyState; } }

        public void Init(CameraMaster master, FlythroughManager flythrough) {
            mMaster = master;
            mFlythrough = flythrough;
        }

        public KinectManager() {
            Nui.Init();
            Nui.SetAutoPoll(true);

            DotNetVector shoulderR = Nui.joint(Nui.Shoulder_Right);
            DotNetVector shoulderL = Nui.joint(Nui.Shoulder_Left);
            DotNetVector elbowR = Nui.joint(Nui.Elbow_Right);
            DotNetVector elbowL = Nui.joint(Nui.Elbow_Left);
            DotNetVector wristR = Nui.joint(Nui.Wrist_Right);
            DotNetVector wristL = Nui.joint(Nui.Wrist_Left);
            DotNetVector handR = Nui.joint(Nui.Hand_Right);
            DotNetVector handL = Nui.joint(Nui.Hand_Left);
            DotNetVector hipC = Nui.joint(Nui.Hip_Centre);
            DotNetVector head = Nui.joint(Nui.Head);

            DotNetVector yAxis = DotNetVector.Create("Y", 0f, 1f, 0f);
            // Normal is the direction the camera is facing.
            DotNetVector normal = DotNetVector.Create("Normal", 0, 0, 1);

            //Camera - If the right elbow is raised to be in line with the shoulders the camera is active.
            DotNetVector upperArmCameraR = elbowR - shoulderR;
            DotNetVector lowerArmCameraR = elbowR - wristR;
            DotNetCondition cameraActiveR = Nui.abs(Nui.x(upperArmCameraR)) > (Nui.abs(Nui.y(upperArmCameraR)) + Nui.abs(Nui.z(upperArmCameraR))) * 2f;

            //Camera - If the right elbow is raised to be in line with the shoulders the camera is active.
            DotNetVector upperArmCameraL = shoulderL - elbowL;
            DotNetVector lowerArmCameraL = elbowL - wristL;
            DotNetCondition cameraActiveL = Nui.abs(Nui.x(upperArmCameraL)) > (Nui.abs(Nui.y(upperArmCameraL)) + Nui.abs(Nui.z(upperArmCameraL))) * 2f;

            DotNetCondition cameraActive = cameraActiveL || cameraActiveR;
            //Condition cameraActive = cameraActiveR;

            // Normalize the distance between the shoulder and the right hand against the total length of the arm (armMax).
            // Once normalized constrain the value between the input from tracker PushD (starting at .8) and 1. 
            // So if the normalized value is .9 the constrained value is .5. Alternatively if the normalized value is .8 the constrained value is 0.
            //Scalar push = constrain(normalize(magnitude(armR), armMax), tracker("PushD", 25, .04f, 0f, 20), 1f, 0f, false);
            // If the camera is inactive and the push value is > 0 and the forward component is significantly larger than the horizontal and vertical components combined.
            //mPush = !cameraActive && push > 0 && z(armR) > ((abs(x(armR) + abs(y(armR)))) * tracker("PushActive", 9, .5f, .5f, 5));

            //Pitch
            DotNetScalar pitchArmD = Nui.tracker("PitchArmD", 20f * 1f, 1f, 1f * 10f);
            DotNetScalar pitchArmR = Nui.tracker("PitchArmR", 40f * 2f, 2f, 2f * 17f);
            DotNetScalar pitchArmG = Nui.tracker("PitchArmG", 30f * 1f, 1f, 1f * 15f);
            DotNetScalar pitchAS = Nui.tracker("PitchAS", 29f * 1f, 1f, 1f * 20f);

            DotNetVector vPlaneCameraR = Nui.limit(lowerArmCameraR, false, true, true);
            DotNetVector vPlaneCameraL = Nui.limit(lowerArmCameraL, false, true, true);
            // Pitch is the angle between normal and the vertical component of the vector between right shoulder and right hand.
            DotNetScalar pitchR = Nui.acos(Nui.dot(Nui.normalize(vPlaneCameraR), normal)) * Nui.invert(Nui.x(Nui.cross(normal, vPlaneCameraR)) >= 0);
            DotNetScalar pitchL = Nui.acos(Nui.dot(Nui.normalize(vPlaneCameraL), normal)) * Nui.invert(Nui.x(Nui.cross(normal, vPlaneCameraL)) >= 0);
            // Constrain the pitch value by 3 values input by 3 trackers.
            pitchR = Nui.constrain(pitchR * R2DEG, pitchArmD, pitchArmR, pitchArmG, true) / pitchAS;
            pitchL = Nui.constrain(pitchL * R2DEG, pitchArmD, pitchArmR, pitchArmG, true) / pitchAS;
            DotNetScalar mPitch = Nui.ifScalar(cameraActiveR, pitchR, .0f) + Nui.ifScalar(cameraActiveL, pitchL, .0f);

            DotNetCondition mCanPitch = cameraActive && mPitch != 0f;

            //Yaw - Yaw has 3 components. The camera arm. The horizontal lean (head vs hip centre) and the twist of the shoulders.
            DotNetScalar yawArmD = Nui.tracker("YawArmD", 20f * 1f, 1f, 1f * 10f);
            DotNetScalar yawArmR = Nui.tracker("YawArmR", 40f * 2f, 2f, 2f * 15f);
            DotNetScalar yawArmG = Nui.tracker("YawArmG", 20f * 1f, 1f, 1f * 10f);
            DotNetScalar yawLeanD = Nui.tracker("YawLeanD", 20f * .5f, .5f, .5f * 10f);
            DotNetScalar yawLeanR = Nui.tracker("YawLeanR", 20f * 1f, 1f, 1f * 15f);
            DotNetScalar yawLeanG = Nui.tracker("YawLeanG", 50f * 1f, 1f, 1f * 30f);
            DotNetScalar yawTwistD = Nui.tracker("YawTwistD", 10f * .025f, .025f, .025f * 6f);
            DotNetScalar yawTwistR = Nui.tracker("YawTwistR", 20f * .05f, .05f, .05f * 9f);
            DotNetScalar yawTwistG = Nui.tracker("YawTwistG", 20f * 1f, 1f, 1f * 10f);

            DotNetVector hPlaneCameraR = Nui.limit(lowerArmCameraR, true, false, true);
            DotNetVector hPlaneCameraL = Nui.limit(lowerArmCameraL, true, false, true);
            // Yaw component 1 is the angle between normal and the horizontal component of the vector between right shoulder and right hand.
            DotNetScalar yawCameraR = Nui.acos(Nui.dot(Nui.normalize(hPlaneCameraR), normal)) * Nui.invert(Nui.y(Nui.cross(normal, hPlaneCameraR)) >= 0);
            DotNetScalar yawCameraL = Nui.acos(Nui.dot(Nui.normalize(hPlaneCameraL), normal)) * Nui.invert(Nui.y(Nui.cross(normal, hPlaneCameraL)) >= 0);
            // Constrain the component value by 3 values input by 3 trackers.
            yawCameraR = Nui.constrain(yawCameraR * R2DEG, yawArmD, yawArmR, yawArmG, true) / Nui.tracker("YawAS", 29f * 1f, 1f, 1f * 20f);
            yawCameraL = Nui.constrain(yawCameraL * R2DEG, yawArmD, yawArmR, yawArmG, true) / Nui.tracker("YawAS", 29f * 1f, 1f, 1f * 20f);
            //Only take the value if camera is active
            yawCameraR = Nui.ifScalar(cameraActiveR, yawCameraR, .0f);
            yawCameraL = Nui.ifScalar(cameraActiveL, yawCameraL, .0f);

            DotNetVector yawCore = Nui.limit(head - hipC, true, true, false);
            // Yaw component 2 is how far the user is leaning horizontally. This is calculated the angle between vertical and the vector between the hip centre and the head.
            DotNetScalar yawLean = Nui.acos(Nui.dot(Nui.normalize(yawCore), yAxis)) * Nui.invert(Nui.z(Nui.cross(yawCore, yAxis)) >= 0);
            // Constrain the component value by 3 values input by 3 trackers.
            yawLean = Nui.constrain(yawLean * R2DEG, yawLeanD, yawLeanR, yawLeanG, true) / Nui.tracker("YawLS", 29f * 1f, 1f, 1f * 20f);

            DotNetVector shoulderDiff = shoulderR - shoulderL;
            // Yaw component 3 is the twist of the shoulders. This is calculated as the difference between the two z values.
            DotNetScalar yawTwist = Nui.z(shoulderDiff) / Nui.magnitude(shoulderDiff);
            // Constrain the component value by 3 values input by 3 trackers.
            yawTwist = Nui.constrain(yawTwist, yawTwistD, yawTwistR, yawTwistG, true) / Nui.tracker("YawTS", 29f * 1f, 1f, 1f * 20f);

            // Combine all 3 components into the final yaw value.
            DotNetScalar mYaw = yawCameraR + yawCameraL + yawLean + yawTwist;
            DotNetScalar mCanYaw = (cameraActive && (yawCameraR + yawCameraL) != 0f) || yawLean != 0f || yawTwist != 0f;

            DotNetScalar flyUpD = Nui.tracker("FlyUpD", 120f * 1f, 1f, 1f * 65f);
            DotNetScalar flyUpR = Nui.tracker("FlyUpR", 120f * 1f, 1f, 1f * 50f);
            DotNetScalar flyDownD = Nui.tracker("FlyDownD", 120f * 1f, 1f, 1f * 45f);
            DotNetScalar flyDownR = Nui.tracker("FlyDownR", 120f * 1f, 1f, 1f * 15f);

            //Fly
            DotNetVector armR = shoulderR - handR;
            DotNetVector vPlaneR = Nui.limit(armR, false, true, true);
            // The angle between normal and the vector between the shoulder and the hand.
            DotNetScalar flyR = Nui.acos(Nui.dot(Nui.normalize(vPlaneR), normal));
            // Constrain the positive angle to go up past vertical.
            DotNetScalar upR = Nui.constrain(flyR * R2DEG, flyUpD, flyUpR, 0f, true); //Constraints if R is raised
            // Constrain the negative angle to stop before vertical so that hands lying by the side doesn't trigger flying down.
            DotNetScalar downR = Nui.constrain(flyR * R2DEG, flyDownD, flyDownR, 0f, true); //Constraints if R is lowered
            // Whether the arm is raised or lowered.
            DotNetCondition dirR = Nui.x(Nui.cross(normal, vPlaneR)) >= 0;
            // Whether R is in range to fly
            DotNetCondition flyCondR = (Nui.magnitude(vPlaneR) > 0f) && ((dirR && (upR > 0)) || ((!dirR) && (downR > 0)));

            DotNetVector armL = shoulderL - handL;
            DotNetVector vPlaneL = Nui.limit(armL, false, true, true);
            // The angle between normal and the vector between the shoulder and the hand.
            DotNetScalar flyL = Nui.acos(Nui.dot(Nui.normalize(vPlaneL), normal)) * R2DEG;
            // Constrain the positive angle to go up past vertical.
            DotNetScalar upL = Nui.constrain(flyL, flyUpD, flyUpR, 0f, true); //Constraints if L is raised
            // Constrain the negative angle to stop before vertical so that hands lying by the side doesn't trigger flying down.
            DotNetScalar downL = Nui.constrain(flyL, flyDownD, flyDownR, 0f, true); //Constraints if L is lowered
            // Whether the arm is raised or lowered.
            DotNetCondition dirL = Nui.x(Nui.cross(normal, vPlaneL)) >= 0;
            // Whether L is in range to fly
            DotNetCondition flyCondL = Nui.magnitude(vPlaneL) > 0f && (dirL && upL > 0f) || (!dirL && downL > 0f);

            //Up trumps down
            DotNetCondition mFly = (dirR && flyCondR) || (dirL && flyCondL);
            // Fly if camera is inactive and flying with right or left arm
            DotNetCondition mCanFly = (flyCondR && !cameraActiveR) || (flyCondL && !cameraActiveL);

            DotNetScalar pushThresh = Nui.tracker("PushThreshold", 30, .05f, .0f, 9);
            DotNetCondition pushR = Nui.z(shoulderR) - Nui.z(handR) > pushThresh;
            DotNetCondition pushL = Nui.z(shoulderL) - Nui.z(handL) > pushThresh;
            DotNetCondition mPush = (pushR && !cameraActiveR) || (pushL && !cameraActiveL);

            DotNetCondition mCanMove = mPush || mCanYaw || mCanPitch || mCanFly;
        }
    }
}

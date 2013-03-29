using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using System.Drawing;
using NuiLibDotNet;
using OpenMetaverse;

namespace Chimera.Kinect.GUI {
    public class SkeletonFeature : IDrawable {
        private static Vector sLeftHand, sRightHand;
        private static Vector sLeftElbow, sRightElbow;
        private static Vector sLeftShoulder, sRightShoulder;
        private static Vector sLeftHip, sRightHip;
        private static Vector sLeftKnee, sRightKnee;
        private static Vector sLeftAnkle, sRightAnkle;
        private static Vector sLeftFoot, sRightFoot;
        private static Vector sCentreHip;
        private static Vector sCentreShoulder;
        private static Vector sHead;        /// <summary>
        /// The colour the current skeleton will be drawn. Changes every time the skeleton changes.
        /// </summary>
        private static Color sSkeletonColour = Color.Red;
        /// <summary>
        /// The count of the skeleton currently being drawn. Used to decide which colour the skeleton should be.
        /// </summary>
        private static int sSkeletonCount = 0;
        /// <summary>
        /// Whether the skeleton has been initialised.
        /// </summary>
        private static bool sInitialised;
        /// <summary>
        /// True if the skeleton needs to be redrawn.
        /// </summary>
        private static bool sNeedsRedrawn = false;

        private float mRoomW = 3f;
        /// <summary>
        /// The clip rectangle for the window as a whole.
        /// </summary>
        private Rectangle mClip;
        /// <summary>
        /// Where the x coordinate the skeleton can be positioned at starts.
        /// </summary>
        private float mXStart;
        /// <summary>
        /// How much space the skeleton has to move left and right as the user moves across the field of view of the kinect.
        /// </summary>
        private float mXRange;
        /// <summary>
        /// Where the skeleton should be on the screen, vertically. Specified between 0 and 1. 0 at the top. 1 at the bottom.
        /// </summary>
        private float mY;
        /// <summary>
        /// The scale for the skeleton.
        /// </summary>
        private float mScale;

        public SkeletonFeature (float xStart, float xRange, float y, float scale) {
            mXStart = xStart;
            mXRange = xRange;
            mY = y;
            mScale = scale;
        }

        public bool NeedsRedrawn {
            get { return sNeedsRedrawn; }
        }

        public void RedrawStatic(Rectangle clip, Graphics graphics) {
            mClip = clip;
        }

        public void DrawDynamic(Graphics graphics) {
            sNeedsRedrawn = false;
            if (!Nui.HasSkeleton)
                return;

            if (!sInitialised)
                Init();

            float scaledPos = ((sCentreHip.X + mRoomW / 2f) / mRoomW);
            float x = mXStart + (mXRange * scaledPos);
            Point centreP = new Point((int)(mClip.Width * x), (int) (mY * mClip.Height));
            Vector3 centre = V3toVector(sCentreHip);
            float lineW = 16f;
            using (Pen p = new Pen(sSkeletonColour, lineW)) {
                int r = (int) (mScale / 5f);

                Point leftHand = Vector2Point(centre, sLeftHand, centreP, mScale);
                Point leftElbow = Vector2Point(centre, sLeftElbow, centreP, mScale);
                Point leftShoulder = Vector2Point(centre, sLeftShoulder, centreP, mScale);
                Point leftHip = Vector2Point(centre, sLeftHip, centreP, mScale);
                Point leftKnee = Vector2Point(centre, sLeftKnee, centreP, mScale);
                Point leftAnkle = Vector2Point(centre, sLeftAnkle, centreP, mScale);
                Point leftFoot = Vector2Point(centre, sLeftFoot, centreP, mScale);

                Point head = Vector2Point(centre, sHead, centreP, mScale);
                Point centreShoulder = Vector2Point(centre, sCentreShoulder, centreP, mScale);
                Point centreHip = Vector2Point(centre, sCentreHip, centreP, mScale);

                Point rightHand = Vector2Point(centre, sRightHand, centreP, mScale);
                Point rightElbow = Vector2Point(centre, sRightElbow, centreP, mScale);
                Point rightShoulder = Vector2Point(centre, sRightShoulder, centreP, mScale);
                Point rightHip = Vector2Point(centre, sRightHip, centreP, mScale);
                Point rightKnee = Vector2Point(centre, sRightKnee, centreP, mScale);
                Point rightAnkle = Vector2Point(centre, sRightAnkle, centreP, mScale);
                Point rightFoot = Vector2Point(centre, sRightFoot, centreP, mScale);

                graphics.DrawLine(p, head, Vector2Point(centre, sCentreShoulder, centreP, mScale));
                graphics.DrawLine(p, Vector2Point(centre, sCentreHip, centreP, mScale), Vector2Point(centre, sCentreShoulder, centreP, mScale));

                graphics.DrawLine(p, leftHand, leftElbow);
                graphics.DrawLine(p, leftElbow, leftShoulder);
                graphics.DrawLine(p, leftShoulder, centreShoulder);
                graphics.DrawLine(p, leftHip, centreHip);
                graphics.DrawLine(p, leftHip, leftKnee);
                graphics.DrawLine(p, leftKnee, leftAnkle);
                graphics.DrawLine(p, leftAnkle, leftFoot);

                graphics.DrawLine(p, rightHand, rightElbow);
                graphics.DrawLine(p, rightElbow, rightShoulder);
                graphics.DrawLine(p, rightShoulder, centreShoulder);
                graphics.DrawLine(p, rightHip, centreHip);
                graphics.DrawLine(p, rightHip, rightKnee);
                graphics.DrawLine(p, rightKnee, rightAnkle);
                graphics.DrawLine(p, rightAnkle, rightFoot);


                using (Brush b = new SolidBrush(sSkeletonColour)) {
                    graphics.FillEllipse(b, head.X - r, head.Y - r, r * 2, r * 2);

                    int jointR = (int) (lineW / 2f);

                    graphics.FillEllipse(b, centreShoulder.X - jointR, centreShoulder.Y - jointR, lineW, lineW);
                    graphics.FillEllipse(b, centreHip.X - jointR, centreHip.Y - jointR, lineW, lineW);

                    graphics.FillEllipse(b, leftElbow.X - jointR, leftElbow.Y - jointR, lineW, lineW);
                    graphics.FillEllipse(b, leftShoulder.X - jointR, leftShoulder.Y - jointR, lineW, lineW);
                    graphics.FillEllipse(b, leftHip.X - jointR, leftHip.Y - jointR, lineW, lineW);
                    graphics.FillEllipse(b, leftAnkle.X - jointR, leftAnkle.Y - jointR, lineW, lineW);

                    graphics.FillEllipse(b, rightElbow.X - jointR, rightElbow.Y - jointR, lineW, lineW);
                    graphics.FillEllipse(b, rightShoulder.X - jointR, rightShoulder.Y - jointR, lineW, lineW);
                    graphics.FillEllipse(b, rightHip.X - jointR, rightHip.Y - jointR, lineW, lineW);
                    graphics.FillEllipse(b, rightAnkle.X - jointR, rightAnkle.Y - jointR, lineW, lineW);

                }
            }
        }

        private Vector3 V3toVector(Vector vector) {
            return new Vector3(vector.X, vector.Y, vector.Z);
        }
        private Point Vector2Point(Vector3 skeletonCentre, Vector vector, Point centre, float scale) {
            Vector3 diff = skeletonCentre - V3toVector(vector);
            diff *= scale;
            return new Point(centre.X - (int)diff.X, centre.Y + (int)diff.Y);
        }

        private static void Init() {
            sInitialised = true;

            sLeftHand = Nui.joint(Nui.Hand_Left);
            sRightHand = Nui.joint(Nui.Hand_Right);
            sLeftElbow = Nui.joint(Nui.Elbow_Left);
            sRightElbow = Nui.joint(Nui.Elbow_Right);
            sLeftShoulder = Nui.joint(Nui.Shoulder_Left);
            sRightShoulder = Nui.joint(Nui.Shoulder_Right);
            sLeftHip = Nui.joint(Nui.Hip_Left);
            sRightHip = Nui.joint(Nui.Hip_Right);
            sLeftKnee = Nui.joint(Nui.Knee_Left);
            sRightKnee = Nui.joint(Nui.Knee_Right);
            sLeftAnkle = Nui.joint(Nui.Ankle_Left);
            sRightAnkle = Nui.joint(Nui.Ankle_Right);
            sLeftFoot = Nui.joint(Nui.Foot_Left);
            sRightFoot = Nui.joint(Nui.Foot_Right);
            sCentreHip = Nui.joint(Nui.Hip_Centre);
            sCentreShoulder = Nui.joint(Nui.Shoulder_Centre);
            sHead = Nui.joint(Nui.Head);

            Nui.SkeletonSwitched += new SkeletonTrackDelegate(Nui_SkeletonSwitched);
            Nui.SkeletonLost += new SkeletonTrackDelegate(Nui_SkeletonLost);
            Nui.Tick += new ChangeDelegate(Nui_Tick);
        }

        static void Nui_Tick() {
            if (Nui.HasSkeleton)
                sNeedsRedrawn = true;        }
        static void Nui_SkeletonLost() {
            sNeedsRedrawn = true;
            switch (sSkeletonCount++ % 3) {
                case 0: sSkeletonColour = Color.Red; break;
                case 1: sSkeletonColour = Color.Green; break;
                case 2: sSkeletonColour = Color.Blue; break;
            }
        }

        static void Nui_SkeletonSwitched() {
            sNeedsRedrawn = true;
            switch (sSkeletonCount++ % 3) {
                case 0: sSkeletonColour = Color.Red; break;
                case 1: sSkeletonColour = Color.Green; break;
                case 2: sSkeletonColour = Color.Blue; break;
            }
        }
    }
}

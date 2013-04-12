using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chimera.Interfaces;
using Chimera.Overlay;
using NuiLibDotNet;
using OpenMetaverse;
using Chimera.Util;
using Chimera.Launcher;
using Chimera.Overlay.Selectors;

namespace Chimera.GUI.Forms {
    public partial class SimpleOverlay : Form, IOverlayWindow {
        private enum State { Help, Explore, MainMenu, Flythrough }

        /// <summary>
        /// The configuration to use for the overlay.
        /// </summary>
        private SimpleOverlayConfig mConfig;

        /// <summary>
        /// The last position the mouse was at.
        /// </summary>
        private Point mLastMouse = new Point(-1, -1);
        /// <summary>
        /// The step that the current minimize/maximise step has got to.
        /// </summary>
        private int mCurrentStep;
        /// <summary>
        /// How many steps minimising/maximising should take.
        /// </summary>
        private int mSteps = 50;
        /// <summary>
        /// Whether the overlay is currently maximising.
        /// </summary>
        private bool mMaximising;
        /// <summary>
        /// Whether the overlay is currently minimizing.
        /// </summary>
        private bool mMinimizing;
        /// <summary>
        /// The controller which controls this overlay.
        /// </summary>
        private OverlayController mController;
        /// <summary>
        /// What state the system is currently in.
        /// </summary>
        private State mState = State.MainMenu;

        /// <summary>
        /// The default position to put the camera back to.
        /// </summary>
        private Vector3 mDefaultPosition;
        /// <summary>
        /// The default orientation to put the camera back to.
        /// </summary>
        private Rotation mDefaultOrientation;


        /// <summary>
        /// Selection which triggers when the user is to go in world.
        /// </summary>
        private ISelectable mGoInWorld;
        /// <summary>
        /// The renderer which will let the user know when they are hovering over go in world.
        /// </summary>
        private ISelectionRenderer mGoInWorldRender;

        /// <summary>
        /// Selection which triggers when the user is to go in world.
        /// </summary>
        private ISelectable mGoInWorldHelp;
        /// <summary>
        /// The renderer which will let the user know when they are hovering over go in world.
        /// </summary>
        private ISelectionRenderer mGoInWorldHelpRender;

        /// <summary>
        /// Selection which triggers when the user is to go in world.
        /// </summary>
        private ISelectable mGoMainMenu;
        /// <summary>
        /// The renderer which will let the user know when they are hovering over go in world.
        /// </summary>
        private ISelectionRenderer mGoMainMenuRender;
        /// <summary>
        /// The colour the current skeleton will be drawn. Changes every time the skeleton changes.
        /// </summary>
        private Color mSkeletonColour = Color.Red;
        /// <summary>
        /// The count of the skeleton currently being drawn. Used to decide which colour the skeleton should be.
        /// </summary>
        private int mSkeletonCount = 0;
        /// <summary>
        /// The time when the last skeleton was lost.
        /// </summary>
        private DateTime mSkeletonLost = DateTime.Now;
        /// <summary>
        /// The flythrough to load whilst nothing else is happening.
        /// </summary>
        //private string mIntroFlythrough = "../Caen-long.xml";
        private string mIntroFlythrough = "../CathedralFlythrough-LookAt.xml";
        /// <summary>
        /// The flythrough controller.
        /// </summary>
        private Flythrough.Flythrough mFlythrough;
        /// <summary>
        /// How many milliseconds after the skeleton is lost the system reverts to the flythrough.
        /// </summary>
        private int mSkeletonTimeoutms = 15000;

        /// <summary>
        /// The main menu image.
        /// </summary>
        //private Bitmap mMainMenuImage = new Bitmap("../Images/CaenSplashScreen.png");
        private Bitmap mMainMenuImage = new Bitmap("../Images/CathedralSplashScreen.png");
        /// <summary>
        /// The help image.
        /// </summary>
        //private Bitmap mCathedralHelpImage = new Bitmap("../Images/CaenHelp.png");
        private Bitmap mCathedralHelpImage = new Bitmap("../Images/CathedralHelp.png");

        private ISelectionRenderer mClickRenderer;

        /// <summary>
        /// Whether to go full screen.
        /// </summary>
        public bool Fullscreen {
            get { return FormBorderStyle == FormBorderStyle.None; }
            set { 
                FormBorderStyle = value ? FormBorderStyle.None : FormBorderStyle.Sizable;
                Location = mController.Window.Monitor.Bounds.Location;
                Size = mController.Window.Monitor.Bounds.Size;
            }
        }

        public SimpleOverlay() {
            InitializeComponent();
        }

        /// <param name="window">The window this overlay covers.</param>
        public SimpleOverlay(OverlayController controller)
            : this() {
            TransparencyKey = controller.TransparentColour;
            // http://www.cursor.cc/
            Cursor = new Cursor("../Cursors/cursor.cur");

            drawPanel.BackColor = controller.TransparentColour;

            CoordinatorConfig cfg = new CoordinatorConfig();
            mDefaultPosition = cfg.Position;
            mDefaultOrientation = new Rotation(cfg.Pitch, cfg.Yaw);
            mConfig = new SimpleOverlayConfig();

            Init(controller);
        }

        /// <summary>
        /// Link this form with a logical input.
        /// </summary>
        /// <param name="input">The input to link this form with.</param>
        public void Init(OverlayController controller) {
            mController = controller;
            mController.ControlPointer = true;

            StartPosition = FormStartPosition.Manual;
            Location = mController.Window.Monitor.Bounds.Location;
            Size = mController.Window.Monitor.Bounds.Size;

            Size s = mController.Window.Monitor.Bounds.Size;

            mMainMenuImage = new Bitmap(mMainMenuImage, s);
            mCathedralHelpImage = new Bitmap(mCathedralHelpImage, s);

            mGoInWorldRender = new NumberSelectionRenderer();
            //mGoInWorld = new InvisibleHoverTrigger(mGoInWorldRender, 155f / 1920, 220f / 1080, (555f - 155f) / 1920, (870f - 220f) / 1080);
            mGoInWorld = new InvisibleHoverTrigger(mGoInWorldRender, 265f / 1920, 255f / 1080, (675f - 255f) / 1920, (900f - 255f) / 1080);
            mGoInWorld.Selected += new Action<ISelectable>(mGoInWorld_Selected);

            mGoMainMenuRender = new NumberSelectionRenderer();
            //mGoMainMenu = new InvisibleHoverTrigger(mGoMainMenuRender, 70f / 1920, 65f / 1080, (490f - 70f) / 1920, (300f - 65f) / 1080);
            mGoMainMenu = new InvisibleHoverTrigger(mGoMainMenuRender, 70f / 1920, 65f / 1080, (490f - 70f) / 1920, (300f - 65f) / 1080);
            mGoMainMenu.Selected += new Action<ISelectable>(mGoMainMenu_Selected);
            mGoMainMenu.Active = false;

            mGoInWorldHelpRender = new NumberSelectionRenderer();
            //mGoInWorldHelp = new InvisibleHoverTrigger(mGoInWorldHelpRender, 55f / 1920, 515f / 1080, (330f - 55f) / 1920, (950f - 515f) / 1080);
            mGoInWorldHelp = new InvisibleHoverTrigger(mGoInWorldHelpRender, 60f / 1920, 520f / 1080, (335f - 60f) / 1920, (945f - 520f) / 1080);
            mGoInWorldHelp.Selected += new Action<ISelectable>(mGoInWorldHelp_Selected);
            mGoInWorldHelp.Active = false;

            mClickRenderer = new CursorTrigger(this, mController.Window);

            mGoInWorld.Init(mController.Window);
            mGoInWorldHelp.Init(mController.Window);
            mGoMainMenu.Init(mController.Window);

            mCoordinatorTick = new Action(Coordinator_Tick);
            mHelpTriggered = new Action(mController_HelpTriggered);
            mNuiTick = new ChangeDelegate(Nui_Tick);

            Nui.Tick += mNuiTick;
            mController.Window.Coordinator.Tick += mCoordinatorTick;
            mController.HelpTriggered += mHelpTriggered;

            Disposed += new EventHandler(SimpleOverlay_Disposed);

            mController.Window.Coordinator.EnableUpdates = false;

            mLeftHand = Nui.joint(Nui.Hand_Left);
            mRightHand = Nui.joint(Nui.Hand_Right);
            mLeftElbow = Nui.joint(Nui.Elbow_Left);
            mRightElbow = Nui.joint(Nui.Elbow_Right);
            mLeftShoulder = Nui.joint(Nui.Shoulder_Left);
            mRightShoulder = Nui.joint(Nui.Shoulder_Right);
            mLeftHip = Nui.joint(Nui.Hip_Left);
            mRightHip = Nui.joint(Nui.Hip_Right);
            mLeftKnee = Nui.joint(Nui.Knee_Left);
            mRightKnee = Nui.joint(Nui.Knee_Right);
            mLeftAnkle = Nui.joint(Nui.Ankle_Left);
            mRightAnkle = Nui.joint(Nui.Ankle_Right);
            mLeftFoot = Nui.joint(Nui.Foot_Left);
            mRightFoot = Nui.joint(Nui.Foot_Right);
            mCentreHip = Nui.joint(Nui.Hip_Centre);
            mCentreShoulder = Nui.joint(Nui.Shoulder_Centre);
            mHead = Nui.joint(Nui.Head);

            Nui.SkeletonSwitched += new SkeletonTrackDelegate(Nui_SkeletonSwitched);
            Nui.SkeletonLost += new SkeletonTrackDelegate(Nui_SkeletonLost);
            Nui.SkeletonFound += new SkeletonTrackDelegate(Nui_SkeletonFound);
            HandleCreated += new EventHandler(SimpleOverlay_HandleCreated);

            mFlythrough = mController.Window.Coordinator.GetInput<Flythrough.Flythrough>();
        }

        private Action mCoordinatorTick;
        private Action mHelpTriggered;
        private ChangeDelegate mNuiTick;

        void SimpleOverlay_HandleCreated(object sender, EventArgs e) {
            if (mConfig.EnableMenus)
                LoadMainMenu();
            else
                LoadExplore();
        }

        void Nui_SkeletonFound() {
            if (mState == State.Flythrough)
                FadeToMainMenu();
        }

        void Nui_SkeletonLost() {
            mSkeletonLost = DateTime.Now;

            switch (mSkeletonCount++ % 3) {
                case 0: mSkeletonColour = Color.Red; break;
                case 1: mSkeletonColour = Color.Green; break;
                case 2: mSkeletonColour = Color.Blue; break;
            }
        }

        void Nui_SkeletonSwitched() {
            switch (mSkeletonCount++ % 3) {
                case 0: mSkeletonColour = Color.Red; break;
                case 1: mSkeletonColour = Color.Green; break;
                case 2: mSkeletonColour = Color.Blue; break;
            }
        }

        void Nui_Tick() {
            if (mState == State.Help || mState == State.Explore)
                Redraw();

        }        

        void SimpleOverlay_Disposed(object sender, EventArgs e) {
            Nui.Tick -= mNuiTick;
            mController.Window.Coordinator.Tick -= mCoordinatorTick;
            mController.HelpTriggered -= mHelpTriggered;
        }

        public void Foreground() {
            Invoke(new Action(() => BringToFront()));
        }

        /// <summary>
        /// Redraw the input.
        /// </summary>
        public void Redraw() {
            if (!IsDisposed && Created && !Disposing)
                Invoke(new Action(() => {
                    if (!IsDisposed && Created && !Disposing)
                        drawPanel.Invalidate();
                }));
        }

        #region Draw

        private void drawPanel_Paint(object sender, PaintEventArgs e) {
            if (mState == State.MainMenu) {
                e.Graphics.DrawImage(mMainMenuImage, new Point(0, 0));
            } else if (mState == State.Help) {
                e.Graphics.DrawImage(mCathedralHelpImage, new Point(0, 0));
                float scale = 225f;
                Point centreP = new Point(1650, 800);
                DrawSkeleton(e.Graphics, e.ClipRectangle, scale, centreP);
            } else if (mState == State.Explore) {
                if (Nui.HasSkeleton) {
                    int x = (int)(e.ClipRectangle.Width * ((mCentreHip.X + mRoomW/2f) / mRoomW));
                    //Console.WriteLine("Centre: " + mCentreHip.X);
                    Point centreP = new Point(x, 150);
                    DrawSkeleton(e.Graphics, e.ClipRectangle, 100f, centreP);
                }
            }
            if (mGoInWorld.Active && mGoInWorld.CurrentlyHovering)
                mGoInWorld.DrawDynamic(e.Graphics, e.ClipRectangle);
            if (mGoInWorldHelp.Active && mGoInWorldHelp.CurrentlyHovering)
                mGoInWorldHelp.DrawDynamic(e.Graphics, e.ClipRectangle);
            if (mGoMainMenu.Active && mGoMainMenu.CurrentlyHovering)
                mGoMainMenu.DrawDynamic(e.Graphics, e.ClipRectangle);

            if (mClickRenderer != null)
                mClickRenderer.DrawHover(e.Graphics, e.ClipRectangle, DateTime.Now, 100);
        }

        private Vector mLeftHand, mRightHand;
        private Vector mLeftElbow, mRightElbow;
        private Vector mLeftShoulder, mRightShoulder;
        private Vector mLeftHip, mRightHip;
        private Vector mLeftKnee, mRightKnee;
        private Vector mLeftAnkle, mRightAnkle;
        private Vector mLeftFoot, mRightFoot;
        private Vector mCentreHip;
        private Vector mCentreShoulder;
        private Vector mHead;

        private double mExploreOverlayOpacity = .3;
        private float mRoomW = 3f;

        private void DrawSkeleton(Graphics graphics, Rectangle rectangle, float scale, Point centreP) {
            if (!Nui.HasSkeleton)
                return;

            Vector3 centre = V3toVector(mCentreHip);
            float lineW = 16f;
            using (Pen p = new Pen(mSkeletonColour, lineW)) {
                int r = (int) (scale / 5f);

                Point leftHand = Vector2Point(centre, mLeftHand, centreP, scale);
                Point leftElbow = Vector2Point(centre, mLeftElbow, centreP, scale);
                Point leftShoulder = Vector2Point(centre, mLeftShoulder, centreP, scale);
                Point leftHip = Vector2Point(centre, mLeftHip, centreP, scale);
                Point leftKnee = Vector2Point(centre, mLeftKnee, centreP, scale);
                Point leftAnkle = Vector2Point(centre, mLeftAnkle, centreP, scale);
                Point leftFoot = Vector2Point(centre, mLeftFoot, centreP, scale);

                Point head = Vector2Point(centre, mHead, centreP, scale);
                Point centreShoulder = Vector2Point(centre, mCentreShoulder, centreP, scale);
                Point centreHip = Vector2Point(centre, mCentreHip, centreP, scale);

                Point rightHand = Vector2Point(centre, mRightHand, centreP, scale);
                Point rightElbow = Vector2Point(centre, mRightElbow, centreP, scale);
                Point rightShoulder = Vector2Point(centre, mRightShoulder, centreP, scale);
                Point rightHip = Vector2Point(centre, mRightHip, centreP, scale);
                Point rightKnee = Vector2Point(centre, mRightKnee, centreP, scale);
                Point rightAnkle = Vector2Point(centre, mRightAnkle, centreP, scale);
                Point rightFoot = Vector2Point(centre, mRightFoot, centreP, scale);

                graphics.DrawLine(p, head, Vector2Point(centre, mCentreShoulder, centreP, scale));
                graphics.DrawLine(p, Vector2Point(centre, mCentreHip, centreP, scale), Vector2Point(centre, mCentreShoulder, centreP, scale));

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


                using (Brush b = new SolidBrush(mSkeletonColour)) {
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

        #endregion
         
        private void mGoInWorld_Selected(ISelectable source) {
            LoadHelp();
        }

        private void mGoInWorldHelp_Selected(ISelectable source) {
            FadeToExplore();
        }

        private void mGoMainMenu_Selected(ISelectable source) {
            LoadMainMenu();
        }

        void mController_HelpTriggered() {
            if (mMaximising || mMinimizing || !mConfig.EnableMenus)
                return;

            if (mState == State.Explore)
                FadeToHelp();
            else
                FadeToExplore();
        }

        void Coordinator_Tick() {
            if (mGoInWorld.Active && mGoInWorld.CurrentlyHovering)
                Redraw();
            if (mGoInWorldHelp.Active && mGoInWorldHelp.CurrentlyHovering)
                Redraw();
            if (mGoMainMenu.Active && mGoMainMenu.CurrentlyHovering)
                Redraw();

            if (mConfig.EnableFlythrough && mState != State.Flythrough && !Nui.HasSkeleton && DateTime.Now.Subtract(mSkeletonLost).TotalMilliseconds > mSkeletonTimeoutms)
                FadeToFlythrough();

            if ((mMinimizing || mMaximising) && mConfig.EnableMenus) {
                mCurrentStep += mMinimizing ? -1 : 1;

                if (mCurrentStep < 0) { //Fully Minimized
                    mMinimizing = false;
                    if (mState != State.Flythrough)
                        LoadExplore();
                } else if (mCurrentStep > mSteps) { //Fully maximised
                    mMaximising = false;
                    if (mState == State.Flythrough)
                        LoadMainMenu();
                    else
                        LoadHelp();
                } else { //Somewhere in between
                    Invoke(new Action(() => Opacity = (double)mCurrentStep / (double)mSteps));
                }
            } 
        }

        private void FadeToMainMenu() {
            mCurrentStep = 0;
            mMaximising = true;
        }

        private void FadeToHelp() {
            mController.Window.Coordinator.EnableUpdates = false;
            mState = State.Help;

            mCurrentStep = 0;
            mMaximising = true;
            Redraw();
        }

        private void FadeToExplore() {
            mGoInWorld.Active = false;
            mGoMainMenu.Active = false;
            mGoInWorldHelp.Active = false;
            //mManager.ControlPointer = false;

            mCurrentStep = mSteps;
            mMinimizing = true;

            Redraw();
        }

        private void FadeToFlythrough() {
            mController.Window.Coordinator.EnableUpdates = true;

            mFlythrough.Enabled = true;
            mState = State.Flythrough;
            mFlythrough.Load(mIntroFlythrough);
            mFlythrough.Loop = true;
            mFlythrough.Time = 0;
            mFlythrough.Play();

            mCurrentStep = mSteps;
            mMinimizing = true;
        }

        private void LoadMainMenu() {
            mFlythrough.Enabled = false;
            mFlythrough.Paused = true;
            mController.Window.Coordinator.EnableUpdates = false;
            mController.ControlPointer = true;
            mGoInWorldHelp.Active = false;
            mGoMainMenu.Active = false;
            mGoInWorld.Active = true;
            mState = State.MainMenu;

            mController.Window.Coordinator.EnableUpdates = true;
            mController.Window.Coordinator.Update(mDefaultPosition, Vector3.Zero, mDefaultOrientation, Rotation.Zero);
            mController.Window.Coordinator.EnableUpdates = false;

            Redraw();
        }
        private void LoadHelp() {
            mGoInWorldHelp.Active = true;
            mGoMainMenu.Active = true;
            mGoInWorld.Active = false;
            mController.ControlPointer = true;
            mState = State.Help;

            Redraw();
        }

        private void LoadExplore() {
            mGoInWorldHelp.Active = false;
            mGoMainMenu.Active = false;
            mGoInWorld.Active = false;
            //mManager.ControlPointer = false;

            Invoke(new Action(() => Opacity = mExploreOverlayOpacity));

            mState = State.Explore;
            
            mController.Window.Coordinator.EnableUpdates = true;
        }

        private void LoadFlythrough() {
        }


        private void SimpleOverlay_KeyDown(object sender, KeyEventArgs e) {
            mController.Window.Coordinator.TriggerKeyboard(true, e);
        }

        private void SimpleOverlay_KeyUp(object sender, KeyEventArgs e) {
            mController.Window.Coordinator.TriggerKeyboard(false, e);

        }

        private void drawPanel_Click(object sender, EventArgs e) {
            Console.WriteLine("Clicked");
        }

        internal void SetCursor(Cursor cursor) {
            Invoke(new Action(() => Cursor = cursor));
        }

        private void SimpleOverlay_FormClosing(object sender, FormClosingEventArgs e) {
            SimpleOverlay_Disposed(sender, e);
        }
    }
    public class SimpleOverlayWindowFactory : IOverlayWindowFactory {
        public IOverlayWindow Make(OverlayController controller) {
            return new SimpleOverlay(controller);
        }
    }
}

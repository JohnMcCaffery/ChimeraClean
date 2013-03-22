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

namespace Chimera.GUI.Forms {
    public partial class SimpleOverlay : Form, IOverlayWindow {
        private enum State { Help, Explore, MainMenu }

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
        private InvisibleSelection mGoInWorld;
        /// <summary>
        /// The renderer which will let the user know when they are hovering over go in world.
        /// </summary>
        private ISelectionRenderer mGoInWorldRender;

        /// <summary>
        /// Selection which triggers when the user is to go in world.
        /// </summary>
        private InvisibleSelection mGoInWorldHelp;
        /// <summary>
        /// The renderer which will let the user know when they are hovering over go in world.
        /// </summary>
        private ISelectionRenderer mGoInWorldHelpRender;

        /// <summary>
        /// Selection which triggers when the user is to go in world.
        /// </summary>
        private InvisibleSelection mGoMainMenu;
        /// <summary>
        /// The renderer which will let the user know when they are hovering over go in world.
        /// </summary>
        private ISelectionRenderer mGoMainMenuRender;

        /// <summary>
        /// The main menu image.
        /// </summary>
        private Bitmap mMainMenuImage = new Bitmap("../Images/CaenSplashScreen.png");
        //private Bitmap mMainMenuImage = new Bitmap("../Images/CathedralSplashScreen.png");
        /// <summary>
        /// The help image.
        /// </summary>
        private Bitmap mCathedralHelpImage = new Bitmap("../Images/CaenHelp.png");
        //private Bitmap mCathedralHelpImage = new Bitmap("../Images/CathedralHelp.png");

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

            Init(controller);
        }

        /// <summary>
        /// Link this form with a logical input.
        /// </summary>
        /// <param name="input">The input to link this form with.</param>
        public void Init(OverlayController controller) {
            mController = controller;
            mController.Window.Coordinator.EnableUpdates = false;

            TopMost = true;
            StartPosition = FormStartPosition.Manual;
            Location = mController.Window.Monitor.Bounds.Location;
            Size = mController.Window.Monitor.Bounds.Size;

            Size s = mController.Window.Monitor.Bounds.Size;

            mMainMenuImage = new Bitmap(mMainMenuImage, s);
            mCathedralHelpImage = new Bitmap(mCathedralHelpImage, s);

            mGoInWorldRender = new NumberSelectionRenderer();
            mGoInWorld = new InvisibleSelection(mGoInWorldRender, 155f / 1920, 220f / 1080, (555f-155f) / 1920, (870f - 220f) / 1080);
            mGoInWorld.Selected += new Action<ISelectable>(mGoInWorld_Selected);

            mGoMainMenuRender = new NumberSelectionRenderer();
            mGoMainMenu = new InvisibleSelection(mGoMainMenuRender, 70f / 1920, 65f / 1080, (490f-70f) / 1920, (300f - 65f) / 1080);
            mGoMainMenu.Selected += new Action<ISelectable>(mGoMainMenu_Selected);
            mGoMainMenu.Active = false;

            mGoInWorldHelpRender = new NumberSelectionRenderer();
            mGoInWorldHelp = new InvisibleSelection(mGoInWorldHelpRender, 55f / 1920, 515f / 1080, (330f-55f) / 1920, (950f - 515f) / 1080);
            mGoInWorldHelp.Selected += new Action<ISelectable>(mGoInWorldHelp_Selected);
            mGoInWorldHelp.Active = false;

            mGoInWorld.Init(mController.Window);
            mGoInWorldHelp.Init(mController.Window);
            mGoMainMenu.Init(mController.Window);

            mController.Window.Coordinator.Tick += Coordinator_Tick;
            mController.HelpTriggered += mController_HelpTriggered;
            mController.Window.Coordinator.EnableUpdates = false;

            Disposed += new EventHandler(SimpleOverlay_Disposed);
        }

        void SimpleOverlay_Disposed(object sender, EventArgs e) {
            mController.Window.Coordinator.Tick -= Coordinator_Tick;
            mController.HelpTriggered -= mController_HelpTriggered;
        }

        public void Foreground() {
            Invoke(new Action(() => BringToFront()));
        }

        /// <summary>
        /// Redraw the input.
        /// </summary>
        public void Redraw() {
            if (!IsDisposed && Created)
                Invoke(new Action(() => {
                    if (!IsDisposed && Created)
                        drawPanel.Invalidate();
                }));
        }

        private void drawPanel_Paint(object sender, PaintEventArgs e) {
            if (mState == State.MainMenu) {
                e.Graphics.DrawImage(mMainMenuImage, new Point(0, 0));
            } else if (mState == State.Help) {
                e.Graphics.DrawImage(mCathedralHelpImage, new Point(0, 0));
            }
            if (mGoInWorld.Active && mGoInWorld.CurrentlyHovering)
                mGoInWorld.DrawDynamic(e.Graphics, e.ClipRectangle);
            if (mGoInWorldHelp.Active && mGoInWorldHelp.CurrentlyHovering)
                mGoInWorldHelp.DrawDynamic(e.Graphics, e.ClipRectangle);
            if (mGoMainMenu.Active && mGoMainMenu.CurrentlyHovering)
                mGoMainMenu.DrawDynamic(e.Graphics, e.ClipRectangle);
        }
        private void mGoInWorld_Selected(ISelectable source) {
            mGoInWorld.Active = false;
            mGoInWorldHelp.Active = true;
            mGoMainMenu.Active = true;
            mState = State.Help;
            Redraw();
        }
        private void mGoInWorldHelp_Selected(ISelectable source) {
            mGoInWorld.Active = false;
            mGoInWorldHelp.Active = false;
            mCurrentStep = mSteps;
            mMinimizing = true;
            mController.ControlPointer = false;
            Redraw();
        }

        private void mGoMainMenu_Selected(ISelectable source) {
            mGoInWorldHelp.Active = false;
            mGoMainMenu.Active = false;
            mGoInWorld.Active = true;
            mState = State.MainMenu;
            mController.Window.Coordinator.Update(mDefaultPosition, Vector3.Zero, mDefaultOrientation, Rotation.Zero);
            Redraw();
        }

        void mController_HelpTriggered() {
            if (mMaximising || mMinimizing)
                return;

            if (mState == State.Explore) {
                mController.Window.Coordinator.EnableUpdates = false;
                mState = State.Help;
                mCurrentStep = 0;
                mMaximising = true;
                Redraw();
            } else {
                mCurrentStep = mSteps;
                mMinimizing = true;
                mController.ControlPointer = false;
            }
        }

        void Coordinator_Tick() {
            if (mGoInWorld.Active && mGoInWorld.CurrentlyHovering)
                Redraw();
            if (mGoInWorldHelp.Active && mGoInWorldHelp.CurrentlyHovering)
                Redraw();
            if (mGoMainMenu.Active && mGoMainMenu.CurrentlyHovering)
                Redraw();
            if (mMinimizing || mMaximising) {
                mCurrentStep += mMinimizing ? -1 : 1;
                if (mCurrentStep < 0) {
                    mMinimizing = false;
                    mState = State.Explore;
                    mController.Window.Coordinator.EnableUpdates = true;
                } else if (mCurrentStep > mSteps) {
                    mMaximising = false;
                    mGoMainMenu.Active = true;
                    mGoInWorldHelp.Active = true;
                    mController.ControlPointer = true;
                } else {
                    Invoke(new Action(() => Opacity = (double)mCurrentStep / (double)mSteps));
                }
            } 
        }

        private void SimpleOverlay_KeyDown(object sender, KeyEventArgs e)
        {
            mController.Window.Coordinator.TriggerKeyboard(true, e);
        }

        private void SimpleOverlay_KeyUp(object sender, KeyEventArgs e)
        {
            mController.Window.Coordinator.TriggerKeyboard(false, e);

        }
    }
    public class SimpleOverlayWindowFactory : IOverlayWindowFactory {
        public IOverlayWindow Make(OverlayController controller) {
            return new SimpleOverlay(controller);
        }
    }
}

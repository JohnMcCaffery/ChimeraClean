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

namespace Chimera.GUI.Forms {
    public class SimpleOverlayWindowFactory : IOverlayWindowFactory {
        public IOverlayWindow Make(OverlayController controller) {
            return new SimpleOverlay(controller);
        }
    }
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
            mGoInWorld = new InvisibleSelection(mGoInWorldRender, 265f / s.Width, 255f / s.Height, (675f-255f) / s.Width, (900f - 255f) / s.Height);
            mGoInWorld.Selected += new Action<ISelectable>(mGoInWorld_Selected);

            mGoMainMenuRender = new NumberSelectionRenderer();
            mGoMainMenu = new InvisibleSelection(mGoMainMenuRender, 70f / s.Width, 65f / s.Height, (490f-70f) / s.Width, (300f - 65f) / s.Height);
            mGoMainMenu.Selected += new Action<ISelectable>(mGoMainMenu_Selected);
            mGoMainMenu.Active = false;

            mGoInWorldHelpRender = new NumberSelectionRenderer();
            mGoInWorldHelp = new InvisibleSelection(mGoInWorldHelpRender, 60f / s.Width, 520f / s.Height, (335f-60f) / s.Width, (945f - 520f) / s.Height);
            mGoInWorldHelp.Selected += new Action<ISelectable>(mGoInWorld_Selected);
            mGoInWorldHelp.Active = false;

            mGoInWorld.Init(mController.Window);
            mGoInWorldHelp.Init(mController.Window);
            mGoMainMenu.Init(mController.Window);

            mController.Window.Coordinator.Tick += new Action(Coordinator_Tick);
            mController.HelpTriggered += new Action(mController_HelpTriggered);
            mController.Window.Coordinator.EnableUpdates = false;
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
            mGoInWorldHelp.Active = false;
            mCurrentStep = mSteps;
            mMinimizing = true;
            mController.ControlPointer = false;
        }

        private void mGoMainMenu_Selected(ISelectable source) {
            mGoInWorldHelp.Active = false;
            mGoMainMenu.Active = false;
            mGoInWorld.Active = true;
            mState = State.MainMenu;
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
    }
}

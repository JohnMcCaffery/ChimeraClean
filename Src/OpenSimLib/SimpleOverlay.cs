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
        /// The main menu image.
        /// </summary>
        private Bitmap mMainMenuImage = new Bitmap("../Images/CathedralSplashScreen.png");
        /// <summary>
        /// The help image.
        /// </summary>
        private Bitmap mCathedralHelpImage = new Bitmap("../Images/CathedralHelp.png");

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

            mGoInWorldRender = new NumberSelectionRenderer();
            mGoInWorld = new InvisibleSelection(mGoInWorldRender, 265f / 1920f, 255f / 1080f, (675f-255f) / 1920, (900f - 255f) / 1080f);
            mGoInWorld.Selected += new Action<ISelectable>(mGoInWorld_Selected);

            Init(controller);
        }

        /// <summary>
        /// Link this form with a logical input.
        /// </summary>
        /// <param name="input">The input to link this form with.</param>
        public void Init(OverlayController controller) {
            mController = controller;

            TopMost = true;
            StartPosition = FormStartPosition.Manual;
            Location = mController.Window.Monitor.Bounds.Location;
            Size = mController.Window.Monitor.Bounds.Size;
            mGoInWorld.Init(mController.Window);

            mController.Window.Coordinator.Tick += new Action(Coordinator_Tick);
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
        }

        private void mGoInWorld_Selected(ISelectable source) {
            mGoInWorld.Active = false;
            mCurrentStep = mSteps;
            mMinimizing = true;
        }

        void Coordinator_Tick() {
            if (mGoInWorld.Active && mGoInWorld.CurrentlyHovering)
                Redraw();
            if (mMinimizing || mMaximising) {
                mCurrentStep += mMinimizing ? -1 : 1;
                if (mCurrentStep < 0) {
                    mMinimizing = false;
                    mState = State.Explore;
                } else if (mCurrentStep > mSteps) {
                    mMaximising = false;
                    mState = State.MainMenu;
                }else {
                    Invoke(new Action(() => Opacity = (double)mCurrentStep / (double)mSteps));
                }
            } 
        }
    }
}

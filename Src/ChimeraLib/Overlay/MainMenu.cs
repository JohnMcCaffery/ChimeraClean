using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Chimera.Overlay {
    public class MainMenu : IOverlay {
        /// <summary>
        /// The areas which represent the choices on the main menu.
        /// </summary>
        private readonly Dictionary<string, WindowOverlay> mWindowOverlays = new Dictionary<string, WindowOverlay>();
        private Coordinator mCoordinator;
        private MainMenuItem[] mItems;
        private double mMainMenuSelectableSize = .1;

        /// <summary>
        /// The state which has been selected.
        /// </summary>
        private IOverlayState mSelectedState;
        /// <summary>
        /// True if the main menu active. False if some other state has been entered.
        /// </summary>
        private bool mMenuActive = true;
        /// <summary>
        /// True if the main menu is currently being minimised.
        /// </summary>
        private bool mMinimizing;
        /// <summary>
        /// True if the main menu is currently being maximised.
        /// </summary>
        private bool mMaximising;
        /// <summary>
        /// The current step that has been reached in terms if minimizing or maximising.
        /// </summary>
        private double mCurrentStep;
        /// <summary>
        /// The total number of steps to do whilst minimizing or maximising.
        /// </summary>
        private int mSteps = 30;

        public MainMenu(params MainMenuItem[] items) {
            mItems = items;
        }

        public double MainMenuSelectableSize {
            get { return mMainMenuSelectableSize; }
        }

        #region IOverlay Members

        public event Action<IOverlayState> StateSelected;

        public IOverlayState SelectedState {
            get { return mSelectedState; }
        }

        public void Init(Coordinator coordinator) {
            mCoordinator = coordinator;
            foreach (var window in mCoordinator.Windows) {
                WindowOverlay overlay = new WindowOverlay(mItems.Where(i => i.WindowName.Equals(window.Name)));
                mWindowOverlays.Add(window.Name, overlay);
                overlay.Init(this, window);
            }
        }

        public void Draw(Graphics graphics, Rectangle clipRectangle, Window window) {
            if (mMenuActive)
                mWindowOverlays[window.Name].DrawMenu(graphics, clipRectangle);
            else if (mMinimizing || mMaximising) {
                mCurrentStep += mMinimizing ? -1 : 1;
                double scale = (1.0 - mMainMenuSelectableSize) * (mCurrentStep / mSteps) + mMainMenuSelectableSize;
                mWindowOverlays[window.Name].DrawInBetween(mSelectedState, scale, graphics, clipRectangle);

                if (mMinimizing && mCurrentStep == 0) {
                    mMinimizing = false;
                    mSelectedState.Activate();
                }
                if (mMaximising && mCurrentStep == mSteps) {
                    mMaximising = false;
                    mMenuActive = true;
                    if (MainMenuSelected != null)
                        MainMenuSelected();
                }
            } else
                mWindowOverlays[window.Name].DrawState(mSelectedState, graphics, clipRectangle);

            mWindowOverlays[window.Name].DrawCursor(graphics, clipRectangle);
        }

        public void SelectState(IOverlayState newState) {
            mMenuActive = false;
            mMinimizing = true;
            mCurrentStep = mSteps;
            mSelectedState = newState;
            if (StateSelected != null)
                StateSelected(newState);
        }

        public void SelectMainMenu() {
            mSelectedState.Deactivated += new Action<IOverlayState>(mSelectedState_Deactivated);
            mSelectedState.Deactivate();
        }

        private void mSelectedState_Deactivated(IOverlayState state) {
            mMaximising = true;
            mCurrentStep = 0;
            mSelectedState.Deactivated -= new Action<IOverlayState>(mSelectedState_Deactivated);
        }


        #endregion

        #region OverlayState Members


        public bool Active {
            get { return mMenuActive; }
            set { mMenuActive = value; }
        }

        public void Init(IOverlay coordinator) { }

        public event Action MainMenuSelected;

        #endregion
    }
}

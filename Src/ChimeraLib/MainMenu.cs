using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Chimera {
    public class MainMenu : IOverlayState, IOverlay {
        private class WindowInfo {
            public readonly List<MainMenuItem> Items = new List<MainMenuItem>();
            public Bitmap FullSizeBG;
            public ISelectable ThumbnailSelectable;
            private Window mWindow;
            private MainMenu mMenu;

            public WindowInfo(MainMenu menu, Window window, IEnumerable<MainMenuItem> items) {
                mMenu = menu;
                mWindow = window;
                Items = new List<MainMenuItem>(items);

                mWindow.MonitorChanged += new Action<Window, Screen>(window_MonitorChanged);

                foreach (var item in Items) {
                    item.Init(mMenu);
                    FullSizeBG = new Bitmap(mWindow.Monitor.Bounds.Width, mWindow.Monitor.Bounds.Height);
                }

                window_MonitorChanged(mWindow, mWindow.Monitor);
            }

            private void window_MonitorChanged(Window window, Screen monitor) {
                Bitmap bg = new Bitmap(window.Monitor.Bounds.Width, window.Monitor.Bounds.Height);
                Graphics graphics = Graphics.FromImage(bg);
                foreach (var item in Items)
                    item.DrawBG(graphics, new Rectangle(0, 0, bg.Width, bg.Height));

                FullSizeBG = bg;
                ThumbnailSelectable = new ImageArea(mWindow, FullSizeBG, .95, .95, .05, .05);
            }
        }
        /// <summary>
        /// The areas which represent the choices on the main menu.
        /// </summary>
        private readonly Dictionary<string, WindowInfo> mMainMenuItems = new Dictionary<string, WindowInfo>();
        private Coordinator mCoordinator;
        private Bitmap mWindow;
        private bool mActive = true;
        private MainMenuItem[] mItems;

        public MainMenu(params MainMenuItem[] items) {
            mItems = items;
        }

        public void ActivateItem(MainMenuItem item) {
        }

        #region ActiveState Members

        public event Action<IOverlayState> Activated;

        public event Action<IOverlayState> Deactivated;

        public Coordinator Coordinator {
            get { return mCoordinator; }
        }

        public string State {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }

        public string Name {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }

        public string Type {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }

        public bool Active {
            get { return mActive; }
            set {
                throw new NotImplementedException();
            }
        }

        public void Init(Coordinator coordinator) {
            mCoordinator = coordinator;

            foreach (var window in mCoordinator.Windows)
                mMainMenuItems.Add(window.Name, new WindowInfo(this, window, mItems.Where(i => i.WindowName.Equals(window.Name))));
        }

        public void Draw(Graphics graphics, Rectangle clipRectangle, Window window) {
            float r = 10f;

            Bitmap bg = new Bitmap(mMainMenuItems[window.Name].FullSizeBG);
            Graphics bgGraphics = Graphics.FromImage(bg);

            if (mMainMenuItems.ContainsKey(window.Name))
                foreach (var item in mMainMenuItems[window.Name].Items)
                    item.Draw(bgGraphics, clipRectangle);

            if (window.CursorX > 0 && window.CursorX <= clipRectangle.Width && window.CursorY > 0 && window.CursorY < clipRectangle.Height)
                bgGraphics.FillEllipse(Brushes.Red, (float) window.CursorX - r, (float) window.CursorY - r, r * 2, r * 2);

            graphics.DrawImage(bg, 0, 0);
        }

        public void Deactivate() {
            throw new NotImplementedException();
        }

        public void Activate() {
            throw new NotImplementedException();
        }

        #endregion

        public event Action<IOverlayState, IOverlayState> ActiveStateChange;

        public IOverlayState ActiveState {
            get { throw new NotImplementedException(); }
        }

        public void SetActiveSet(IOverlayState newState) {
            throw new NotImplementedException();
        }


        public void DrawBG(Graphics graphics, Rectangle clipRectangle) {
            throw new NotImplementedException();
        }
    }
}

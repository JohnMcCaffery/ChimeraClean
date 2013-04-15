using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Overlay;
using System.Drawing;
using Chimera.Interfaces.Overlay;

namespace Chimera.Overlay.States {
    public class ImageBGState : State {
        private readonly Dictionary<string, Bitmap> mWindowBGs = new Dictionary<string, Bitmap>();
        private readonly Dictionary<string, ImageBGWindow> mWindows = new Dictionary<string, ImageBGWindow>();
        private Bitmap mDefaultBG;

        public ImageBGState(string name, StateManager manager, Bitmap defaultBG)
            : base(name, manager) {
            mDefaultBG = defaultBG;
            foreach (var window in mWindows.Values)
                window.BackgroundImage = defaultBG;
        }

        public ImageBGState(string name, StateManager manager, string defaultBG)
            : this(name, manager, new Bitmap(defaultBG)) {
        }

        /// <summary>
        /// CreateWindowState a window state for drawing this state to the specified window.
        /// </summary>
        /// <param name="window">The window the new window state is to draw on.</param>
        public override IWindowState CreateWindowState(Window window) {
            ImageBGWindow win = new ImageBGWindow(window.OverlayManager, mWindowBGs.ContainsKey(window.Name) ?  mWindowBGs[window.Name] : mDefaultBG);
            mWindows.Add(window.Name, win);
            return win;
        }

        /// <summary>
        /// Map a window name to a background image.
        /// </summary>
        /// <param name="window">The name of the window to map the image to.</param>
        /// <param name="image">The image to map.</param>
        public void MapWindowImage(string window, Bitmap image) {
            mWindowBGs.Add(window, image);
            if (mWindows.ContainsKey(window))
                mWindows[window].BackgroundImage = image;
        }

        protected override void TransitionToFinish() {
            Manager.Coordinator.EnableUpdates = false;
        }

        protected override void TransitionFromStart() { }

        public override void TransitionToStart() { }

        public override void TransitionFromFinish() { }
    }
}

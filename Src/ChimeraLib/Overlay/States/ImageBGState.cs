using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Overlay;
using System.Drawing;
using Chimera.Interfaces.Overlay;

namespace Chimera.Overlay.States {
    public class ImageBGState : State {
        private Bitmap mDefaultBG;

        private readonly Dictionary<string, Bitmap> mWindowBGs = new Dictionary<string, Bitmap>();

        public ImageBGState(string name, StateManager manager, Bitmap defaultBG) : base (name, manager) {
            mDefaultBG = defaultBG;
        }

        public ImageBGState(string name, StateManager manager, string defaultBG)
            : this(name, manager, new Bitmap(defaultBG)) {
        }

        /// <summary>
        /// CreateWindowState a window state for drawing this state to the specified window.
        /// </summary>
        /// <param name="window">The window the new window state is to draw on.</param>
        public override IWindowState CreateWindowState(Window window) {
            if (mWindowBGs.ContainsKey(window.Name))
                return new ImageBGWindow(window.OverlayManager, mWindowBGs[window.Name]);
            return new ImageBGWindow(window.OverlayManager, mDefaultBG);
        }
    }
}

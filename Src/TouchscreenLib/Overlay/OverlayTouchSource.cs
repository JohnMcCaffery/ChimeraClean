using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Touchscreen.Interfaces;
using Chimera.Overlay;
using System.Drawing;

namespace Touchscreen.Overlay {
    class OverlayTouchSource : ITouchSource {
        private FrameOverlayManager mManager;

        public PointF Position { get { return mManager.CursorPosition; } }

        public event Action<int> OnPress;

        public event Action<int> OnRelease;

        public OverlayTouchSource(FrameOverlayManager manager) {
            mManager = manager;
            mManager.OnPress += () => { if (OnPress != null) OnPress(0); };
            mManager.OnRelease += () => { if (OnRelease != null) OnRelease(0); };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Chimera.Overlay.Triggers {
    public class InvisibleHoverTrigger : HoverTrigger {
        public InvisibleHoverTrigger(WindowOverlayManager manager, IHoverSelectorRenderer renderer, float x, float y, float w, float h)
            : base(manager, renderer, x, y, w, h) {
        }
        public InvisibleHoverTrigger(WindowOverlayManager manager, IHoverSelectorRenderer renderer, int x, int y, int w, int h, Rectangle clip)
            : base(manager, renderer, (float) x / (float) clip.Width, (float) y / (float) clip.Height, (float) w / (float) clip.Width, (float) h / (float) clip.Height) {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Chimera.Overlay {
    public class InvisibleHoverTrigger : HoverTrigger {
        public InvisibleHoverTrigger(WindowOverlayManager manager, IHoverSelectorRenderer renderer, float x, float y, float w, float h)
            : base(manager, renderer, x, y, w, h) {
        }
    }
}

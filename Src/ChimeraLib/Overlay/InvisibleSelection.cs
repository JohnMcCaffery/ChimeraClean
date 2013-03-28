using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Chimera.Overlay {
    public class InvisibleSelection : HoverSelector {
        public InvisibleSelection(IHoverSelectorRenderer renderer, float x, float y, float w, float h)
            : base(renderer, x, y, w, h) {
        }

        public InvisibleSelection(float x, float y, float w, float h)
            : base(x, y, w, h) {
        }

        public override void Show() {
            Active = false;
        }

        public override void Hide() {
            Active = false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Chimera.Overlay.Drawables {
    public class DynamicText : Text {
        private Rectangle mClip;
        private WindowOverlayManager mManager;
        private bool mNeedsRedrawn;

        public DynamicText(string text, WindowOverlayManager manager, Font font, Color colour, PointF location)
            : base(text, manager.Window.Name, font, colour, location) {

            mManager = manager;
        }

        public override bool NeedsRedrawn {
            get { return mNeedsRedrawn; }
        }

        public override string TextString {
            get { return base.TextString; }
            set {
                base.TextString = value;
                mNeedsRedrawn = true;
            }
        }

        public override void DrawStatic(Graphics graphics) { }

        public override void DrawDynamic(Graphics graphics) {
            Draw(graphics);
        }
    }
}

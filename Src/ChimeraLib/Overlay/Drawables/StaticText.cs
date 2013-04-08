using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Chimera.Overlay.Drawables {
    public class StaticText : Text {        public StaticText(string text, string window, Font font, Color colour, PointF location)
            : base(text, window, font, colour, location) {
        }

        public override void DrawDynamic(Graphics graphics) { }

        public override void RedrawStatic(Rectangle clip, Graphics graphics) {
            Draw(graphics, clip);
        }
    }
}

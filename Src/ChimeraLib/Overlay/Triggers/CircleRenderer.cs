using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Chimera.Overlay.Triggers {
    public class CircleRenderer : IHoverSelectorRenderer {
        private Color mHoverColour = Color.Red;
        private Color mSelectionColour = Color.Blue;
        private int mR;

        public int R {
            get { return mR; }
        }

        public Color HoverColour {
            get { return mHoverColour; }
        }

        public CircleRenderer()
            : this(40, Color.Red) {
        }

        public CircleRenderer(int r)
            : this(r, Color.Blue) {
        }

        public CircleRenderer(Color colour)
            : this(40, colour) {
        }

        public CircleRenderer(int r, Color colour) {
            mR = r;
            mSelectionColour = colour;
        }

        public void DrawHover(Graphics graphics, Rectangle bounds, double hoverDone) {
            int x =  bounds.X + (bounds.Width / 2);
            int y =  bounds.Y + (bounds.Height / 2);
            using (Pen p = new Pen(mHoverColour, 10f))
                graphics.DrawArc(p, x - mR, y - mR, mR * 2, mR * 2, -90, (int)(hoverDone * 360f));
        }

        public void DrawSelected(System.Drawing.Graphics graphics, Rectangle bounds) {
            int x =  bounds.X + (bounds.Width / 2);
            int y =  bounds.Y + (bounds.Height / 2);
            using (Pen p = new Pen(mHoverColour))
                graphics.DrawEllipse(p, x - mR, y - mR, mR * 2, mR * 2);
        }

        public void Clear() { }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Chimera.Overlay.Triggers {
    public class DialRenderer : IHoverSelectorRenderer {
        private Color mHoverColour = Color.Red;
        private Color mSelectionColour = Color.Blue;
        private bool mFill;
        private int mR;

        public int R {
            get { return mR; }
        }

        public Color HoverColour {
            get { return mHoverColour; }
        }

        public DialRenderer()
            : this(40, Color.Red, true) {
        }

        public DialRenderer(int r)
            : this(r, Color.Blue, true) {
        }

        public DialRenderer(bool fill)
            : this(40, Color.Red, fill) {
        }

        public DialRenderer(Color colour)
            : this(40, colour, true) {
        }

        public DialRenderer(Color colour, bool fill)
            : this(40, colour, fill) {
        }

        public DialRenderer(int r, Color colour)
            : this(r, colour, true) {
        }

        public DialRenderer(int r, bool fill)
            : this(r, Color.Red, fill) {
        }

        public DialRenderer(int r, Color colour, bool fill) {
            mR = r;
            mSelectionColour = colour;
            mFill = fill;
        }

        public void DrawHover(Graphics graphics, Rectangle bounds, double hoverDone) {
            int x =  bounds.X + (bounds.Width / 2);
            int y =  bounds.Y + (bounds.Height / 2);
            if (mFill) {
                using (Brush b = new SolidBrush(mHoverColour))
                    graphics.FillPie(b, x - mR, y - mR, mR * 2, mR * 2, -90, (int)(hoverDone * 360f));
            } else
                using (Pen p = new Pen(mHoverColour))
                    graphics.DrawPie(p, x - mR, y - mR, mR * 2, mR * 2, -90, (int)(hoverDone * 360f));
        }

        public void DrawSelected(System.Drawing.Graphics graphics, Rectangle bounds) {
            int x =  bounds.X + (bounds.Width / 2);
            int y =  bounds.Y + (bounds.Height / 2);
            if (mFill) {
                using (Brush b = new SolidBrush(mSelectionColour))
                    graphics.FillEllipse(b, x - mR, y - mR, mR * 2, mR * 2);
            } else
                using (Pen p = new Pen(mHoverColour))
                    graphics.DrawEllipse(p, x - mR, y - mR, mR * 2, mR * 2);
        }

        public void Clear() { }
    }
}

/*************************************************************************
Copyright (c) 2012 John McCaffery 

This file is part of Chimera.

Chimera is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Chimera is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Chimera.  If not, see <http://www.gnu.org/licenses/>.

**************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Chimera.Overlay.Triggers {
    public class DialRenderer : XmlLoader, IHoverSelectorRenderer {
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

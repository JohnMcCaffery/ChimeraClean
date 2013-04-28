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
using System.Windows.Forms;

namespace Chimera.Overlay {
    public class NumberSelectionRenderer : ISelectionRenderer {
        private ISelectable mSelectable;

        public ISelectable Selectable {
            get { return mSelectable; }
        }

        public void DrawHover(Graphics graphics, Rectangle clipRectangle, DateTime mHoverStart, double mSelectTime) {
            double tickLength = mSelectTime / 3;
            double hoverTime = DateTime.Now.Subtract(mHoverStart).TotalMilliseconds;
            int tick = (int)(hoverTime / tickLength) + 1;
            double progress = hoverTime % tickLength;
            int s = (int)((progress / tickLength) * (mSelectable.ScaledBounds.Height / 2.0));
            using (Font font = new Font(FontFamily.GenericMonospace, Math.Max(1, s), FontStyle.Bold)) {
                string str = tick.ToString();
                SizeF size = graphics.MeasureString(str, font);
                //SizeF size = TextRenderer.MeasureText(str, font);

                float x = mSelectable.ScaledBounds.X + ((mSelectable.ScaledBounds.Width / 2) - (size.Width / 2));
                float y = mSelectable.ScaledBounds.Y + ((mSelectable.ScaledBounds.Height / 2) - (size.Height / 2));

                graphics.DrawString(str, font, Brushes.Red, x, y);
            }
        }

        public void DrawSelected(Graphics graphics, Rectangle clipRectangle) {
            using (Pen pen = new Pen(Brushes.Red, 20f))
                graphics.DrawRectangle(pen, mSelectable.ScaledBounds);
        }

        public void Init(ISelectable selectable) {
            mSelectable = selectable;
        }
    }
}

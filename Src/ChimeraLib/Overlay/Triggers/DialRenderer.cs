using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Chimera.Overlay.Triggers {
    public class DialRenderer : IHoverSelectorRenderer {
        private Color mHoverColour = Color.Red;
        private Color mSelectionColour = Color.Blue;

        public void DrawHover(Graphics graphics, Rectangle bounds, DateTime hoverStart, float hoverLength) {
            int x =  bounds.X + (bounds.Width / 2);
            int y =  bounds.Y + (bounds.Height / 2);
            int r = 40;
            using (Brush b = new SolidBrush(mHoverColour))
                graphics.FillPie(b, x - r, y - r, r*2, r*2, -90, (int) ((DateTime.Now.Subtract(hoverStart).TotalMilliseconds / hoverLength) * 360f));
        }

        public void DrawSelected(System.Drawing.Graphics graphics, Rectangle bounds) {
            int x =  bounds.X + (bounds.Width / 2);
            int y =  bounds.Y + (bounds.Height / 2);
            int r = 40;
            using (Brush b = new SolidBrush(mSelectionColour))
                graphics.FillEllipse(b, x - r, y - r, r*2, r*2);
        }
    }
}

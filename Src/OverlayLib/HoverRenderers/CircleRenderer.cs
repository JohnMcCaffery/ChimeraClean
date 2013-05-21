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
using System.Xml;
using Chimera.Interfaces.Overlay;

namespace Chimera.Overlay.SelectionRenderers {
    public class CircleRendererFactory : XmlLoader, ISelectionRendererFactory {
        #region IFactory<ISelectionRenderer> Members

        public ISelectionRenderer Create(OverlayPlugin manager, XmlNode node) {
            return new CircleRenderer(manager, node);
        }

        public ISelectionRenderer Create(OverlayPlugin manager, System.Xml.XmlNode node, Rectangle clip) {
            return Create(manager, node);
        }

        #endregion

        #region IFactory Members

        public string Name {
            get { return "Circle"; }
        }

        #endregion
    }

    public class CircleRenderer : XmlLoader, ISelectionRenderer {
        private Color mHoverColour = Color.Red;
        private Color mSelectionColour = Color.Blue;
        private int mR;
        private float mW;

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

        public CircleRenderer(OverlayPlugin manager, XmlNode node) {
            mW = GetFloat(node, 10f, "Width");
            mSelectionColour = GetColour(node, "dial renderer", Color.Red);
            mR = GetInt(node, 100, "Radius");
        }

        public void DrawHover(Graphics graphics, Rectangle bounds, double hoverDone) {
            int x =  bounds.X + (bounds.Width / 2);
            int y =  bounds.Y + (bounds.Height / 2);
            using (Pen p = new Pen(mHoverColour, mW))
                graphics.DrawArc(p, x - mR, y - mR, mR * 2, mR * 2, -90, (int)(hoverDone * 360f));
        }

        public void DrawSelected(System.Drawing.Graphics graphics, Rectangle bounds) {
            int x =  bounds.X + (bounds.Width / 2);
            int y =  bounds.Y + (bounds.Height / 2);
            using (Pen p = new Pen(mHoverColour, mW))
                graphics.DrawEllipse(p, x - mR, y - mR, mR * 2, mR * 2);
        }

        public void Clear() { }
    }
}

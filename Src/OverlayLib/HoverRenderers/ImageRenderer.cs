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
using Chimera.Interfaces.Overlay;
using System.Xml;

namespace Chimera.Overlay.SelectionRenderers {
    public class ImageRendererFactory : OverlayXmlLoader, ISelectionRendererFactory {
        #region IFactory<ISelectionRenderer> Members

        public ISelectionRenderer Create(OverlayPlugin manager, XmlNode node) {
            return new ImageRenderer(manager, node);
        }

        public ISelectionRenderer Create(OverlayPlugin manager, System.Xml.XmlNode node, Rectangle clip) {
            return Create(manager, node);
        }

        #endregion

        #region IFactory Members

        public string Name {
            get { return "Image"; }
        }

        #endregion
    }

    public class ImageRenderer : OverlayXmlLoader, ISelectionRenderer {
        private Bitmap mImage;
        private Bitmap mBackgroundImage;

        public ImageRenderer(OverlayPlugin manager, XmlNode node) {
            mImage = GetImage(node, "Image cursor partial image", "PartialFile");
            mBackgroundImage = GetImage(node, "Image cursor completed image", "BackgroundFile");
        }

        public Size Size {
            get { return mImage.Size; }
        }

        public void DrawHover(Graphics graphics, Rectangle bounds, double hoverDone) {
            using (Bitmap b = new Bitmap(mBackgroundImage, bounds.Size))
                graphics.DrawImage(b, bounds.Location);

            var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddPie(bounds.X, bounds.Y, bounds.Width, bounds.Height, -90f, (float) (360 * hoverDone));
            graphics.SetClip(path);
            using (Bitmap b = new Bitmap(mImage, bounds.Size))
                graphics.DrawImage(b, bounds.Location);
            graphics.ResetClip();
        }

        public void DrawSelected(System.Drawing.Graphics graphics, Rectangle bounds) {
            graphics.DrawImage(mBackgroundImage, 0, 0);
        }

        public void Clear() { }
    }
}

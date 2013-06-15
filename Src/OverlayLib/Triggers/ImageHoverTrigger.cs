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
using System.Threading;
using Chimera.Overlay.Features;
using Chimera.Interfaces.Overlay;
using System.Xml;

namespace Chimera.Overlay.Triggers {
    public class ImageHoverTriggerFactory : ITriggerFactory {
        public SpecialTrigger Special {
            get { return SpecialTrigger.Image; }
        }

        public string Mode {
            get { return OverlayPlugin.HOVER_MODE; }
        }

        public string Name {
            get { return "ImageHoverTrigger"; }
        }

        public ITrigger Create(OverlayPlugin manager, XmlNode node) {
            return new ImageHoverTrigger(manager, node);
        }

        public ITrigger Create(OverlayPlugin manager, XmlNode node, Rectangle clip) {
            return new ImageHoverTrigger(manager, node, clip);
        }
    }

    public class ImageHoverTrigger : HoverTrigger {
        private OverlayImage mImage;

        protected override RectangleF Bounds {
            get { return mImage.Bounds; }
            set { base.Bounds = value; }
        }

        public OverlayImage Image {
            get { return mImage; }
            set {
                mImage = value;
                Bounds = value.Bounds;
                Manager.ForceRedrawStatic();
            }
        }

        public override bool Active {
            get { return base.Active; }
            set {
                base.Active = value;
                mImage.Active = value;
            }
        }

        public ImageHoverTrigger(WindowOverlayManager manager, ISelectionRenderer render, OverlayImage image)
            : base(manager, render, image.Bounds) {
            mImage = image;
        }

        public ImageHoverTrigger(OverlayPlugin manager, XmlNode node)
            : base(manager, node) {
            mImage = new OverlayImage(manager, node, "image trigger");
            Bounds = mImage.Bounds;
        }

        public ImageHoverTrigger(OverlayPlugin manager, XmlNode node, Rectangle clip)
            : base(manager, node) {
            mImage = new OverlayImage(manager, node, clip, "image trigger");
            Bounds = mImage.Bounds;
        }

        #region IDrawable Members

        public override Rectangle Clip {
            get { return base.Clip; }
            set {
                base.Clip = value;
                mImage.Clip = value;
                Bounds = mImage.Bounds;
            }
        }

        public override void DrawStatic(Graphics graphics) {
            base.DrawStatic(graphics);
            mImage.DrawStatic(graphics);
        }

        #endregion
    }
}

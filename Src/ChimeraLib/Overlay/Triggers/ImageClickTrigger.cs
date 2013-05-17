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
using Chimera.Overlay.Drawables;
using Chimera.Interfaces.Overlay;

namespace Chimera.Overlay.Triggers {
    public class ImageClickTriggerFactory : ITriggerFactory {
        public SpecialTrigger Special {
            get { return SpecialTrigger.Image; }
        }

        public string Mode {
            get { return StateManager.CLICK_MODE; }
        }

        public string Name {
            get { return "ImageClickTrigger"; }
        }

        public ITrigger Create(System.Xml.XmlNode node, Coordinator coordinator) {
        }
    }

    public class ImageClickTrigger : ClickTrigger, IDrawable {
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

        public ImageClickTrigger(WindowOverlayManager manager, OverlayImage image)
            : base(manager, image.Bounds) {
            mImage = image;
        }

        #region IDrawable Members

        public Rectangle Clip {
            get { return mImage.Clip; }
            set {
                mImage.Clip = value;
                Bounds = mImage.Bounds;
            }
        }

        public void DrawStatic(Graphics graphics) {
            mImage.DrawStatic(graphics);
        }

        public bool NeedsRedrawn {
            get { return mImage.NeedsRedrawn; }
        }

        public void DrawDynamic(Graphics graphics) {
            mImage.DrawDynamic(graphics);
        }

        #endregion
    }
}

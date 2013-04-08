using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using Chimera.Overlay.Drawables;

namespace Chimera.Overlay.Triggers {
    public class ImageHoverTrigger : HoverTrigger {
        private OverlayImage mImage;
        private Rectangle mClip;

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

        public ImageHoverTrigger(WindowOverlayManager manager, IHoverSelectorRenderer render, OverlayImage image)
            : base(manager, render, image.Bounds) {
            mImage = image;
        }

        #region IDrawable Members

        public override void RedrawStatic(Rectangle clip, Graphics graphics) {
            base.RedrawStatic(clip, graphics);
            mImage.RedrawStatic(clip, graphics);
            if (!clip.Equals(mClip)) {
                mClip = clip;
                Bounds = mImage.Bounds;
            }
        }

        #endregion
    }
}

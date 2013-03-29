using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace Chimera.Overlay.Triggers {
    public class ImageHoverTrigger : HoverTrigger {
        private Bitmap mImage;
        private Bitmap mScaledImage;

        public Bitmap ScaledImage {
            get { return mScaledImage; }
        }

        public Bitmap Image {
            get { return mImage; }
            set {
                mImage = value;
                Manager.ForceRedrawStatic();
            }
        }

        public ImageHoverTrigger(Bitmap image, WindowOverlayManager manager, IHoverSelectorRenderer render, float x, float y, float w, float h)
            : base(manager, render, x, y, w, h) {
            mImage = image;
        }

        public ImageHoverTrigger(string imageFile, WindowOverlayManager manager, IHoverSelectorRenderer render, float x, float y, float w, float h)
            : this(new Bitmap(imageFile), manager, render, x, y, w, h) {
        }

        #region ISelectable Members

        public override void RedrawStatic(Rectangle clip, Graphics graphics) {
            base.RedrawStatic(clip, graphics);
            mScaledImage = new Bitmap(mImage, ScaledBounds.Size);
            graphics.DrawImage(mScaledImage, Bounds.Location);
        }

        #endregion
    }
}

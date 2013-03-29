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
        /// <summary>
        /// Create the trigger leaving the image at its default size and positioning it top left.
        /// </summary>
        /// <param name="image">The image this trigger draws.</param>
        /// <param name="manager">The manager which manages the window this trigger is to draw to.</param>
        /// <param name="render">The renderer used to draw this trigger being selected.</param>
        /// <param name="clip">The clip rectangle for the area the image is to be drawn onto.</param>
        public ImageHoverTrigger(Bitmap image, WindowOverlayManager manager, IHoverSelectorRenderer render, Rectangle clip)
            : this(new Bitmap(image), manager, render, clip, 0f, 0f){
        }

        /// <summary>
        /// Create the trigger, leaving the image its default size but specifiying a position for it.
        /// </summary>
        /// <param name="image">The image this trigger draws.</param>
        /// <param name="manager">The manager which manages the window this trigger is to draw to.</param>
        /// <param name="render">The renderer used to draw this trigger being selected.</param>
        /// <param name="clip">The clip rectangle for the area the image is to be drawn onto.</param>
        /// <param name="x">The x coordinate for where the image is to be positioned, specified between 0 and 1. 0 is flush to the left, 1 flush to the right.</param>
        /// <param name="y">The y coordinate for where the image is to be positioned, specified between 0 and 1. 0 is flush to the top, 1 flush to the bottom.</param>
        public ImageHoverTrigger(Bitmap image, WindowOverlayManager manager, IHoverSelectorRenderer render, Rectangle clip, float x, float y)
            : this(new Bitmap(image), manager, render, x, y, (float) image.Width / (float) clip.Width, (float) image.Height / (float) clip.Height) {
        }
        #region IDrawable Members

        public override void RedrawStatic(Rectangle clip, Graphics graphics) {
            base.RedrawStatic(clip, graphics);
            mScaledImage = new Bitmap(mImage, ScaledBounds.Size);
            graphics.DrawImage(mScaledImage, ScaledBounds);
        }

        #endregion
    }
}

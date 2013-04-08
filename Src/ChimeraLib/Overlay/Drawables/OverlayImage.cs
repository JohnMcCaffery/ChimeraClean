using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using System.Drawing;
using System.Windows.Forms;

namespace Chimera.Overlay.Drawables {
    public class OverlayImage : IDrawable {
        private RectangleF mBounds;
        private Bitmap mImage;
        private float mAspectRatio;
        private string mWindow;
        private bool mActive = true;

        /// <summary>
        /// The position / size for the image. Specified as a relative values.
        /// For location: 0,0 = top left, 1,1 = bottom right.
        /// For size: 0,0 = invisible, 1,1 = same size as the screen
        /// </summary>
        public RectangleF Bounds {
            get { return mBounds; }
        }

        public bool NeedsRedrawn {
            get { return false; }
        }

        public string Window {
            get { return mWindow; }
        }

        public bool Active {
            get { return mActive; }
            set { mActive = value; }
        }

        public void RedrawStatic(Rectangle clip, Graphics graphics) {
            int x = (int) (clip.Width * mBounds.X);
            int y = (int) (clip.Height * mBounds.Y);
            if (mBounds.Width > 0) {
                int w = (int)(clip.Width * mBounds.Width);
                int h = (int)(mBounds.Height > 0 ? clip.Height * mBounds.Height : w * mAspectRatio);
                graphics.DrawImage(mImage, new Rectangle(x, y, w, h));
            } else
                graphics.DrawImage(mImage, x, y);
        }

        public void DrawDynamic(Graphics graphics) { }

        private OverlayImage(Bitmap image, string window) {
            mImage = image;
            mWindow = window;
        }

        /// <summary>
        /// Create the image, specifying x and y as relative values and determining the size by the size of the image vs the size of the area it is to be drawn on.
        /// </summary>
        /// <param name="image">The image to draw.</param>
        /// <param name="x">How far across the screen to draw the image (0: left, 1: right).</param>
        /// <param name="y">How far down the screen to draw the image (0: top, 1: bottom).</param>
        /// <param name="bounds">The bounding rectangle for the area the image is to be drawn on. Width and Height will be calculated using the relative size of this and the size of the image.</param>
        public OverlayImage(Bitmap image, float x, float y, Rectangle bounds, string window)
            : this(image, window) {
            mBounds = new RectangleF(x, y, (float)image.Width / (float) bounds.Width, (float)image.Height / (float) bounds.Height);
        }

        /// <summary>
        /// Create the image, specifying x and y as relative values and determining the size by the size of the image vs the size of the screen it is to be drawn on.
        /// </summary>
        /// <param name="image">The image to draw.</param>
        /// <param name="x">How far across the screen to draw the image (0: left, 1: right).</param>
        /// <param name="y">How far down the screen to draw the image (0: top, 1: bottom).</param>
        /// <param name="screen">The screen the image is to be drawn on. Width and Height will be calculated using the relative size of this and the size of the image.</param>
        public OverlayImage(Bitmap image, float x, float y, Screen screen, string window)
            : this(image, x, y, screen.Bounds, window) {
        }

        /// <summary>
        /// Create the image, specifying x and y as absolute coordinates on the area defined by the bounds rectangle. All value will be calculated from the ratio between the bounds rectangle and x, y, and image height/width.
        /// </summary>
        /// <param name="image">The image to draw.</param>
        /// <param name="x">Where across the bounding rectangle the image should be drawn.</param>
        /// <param name="y">Where down the bounding rectangle the image should be drawn.</param>
        /// <param name="bounds">The bounding rectangle for the area the image will be drawn on.</param>
        public OverlayImage(Bitmap image, int x, int y, Rectangle bounds, string window)
            : this(image, window) {
            mBounds = new RectangleF(
                (float) x / (float) bounds.Width, 
                (float) y / (float) bounds.Height, 
                (float)image.Width / (float) bounds.Width, 
                (float)image.Height / (float) bounds.Height);
        }

        /// <summary>
        /// Create the image, specifying only a position. The image will always stay the natural size of the image provided.
        /// </summary>
        /// <param name="image">The image to draw.</param>
        /// <param name="x">How far across the screen to draw the image (0: left, 1: right).</param>
        /// <param name="y">How far down the screen to draw the image (0: top, 1: bottom).</param>
        public OverlayImage(Bitmap image, float x, float y, string window)
            : this(image, window) {
            mBounds = new RectangleF(x, y, -1f, -1f);
        }

        /// <summary>
        /// Create the image image, specifying a position and width, height will be calculated to keep the aspect ratio the same as image supplied originally has.
        /// </summary>
        /// <param name="image">The image to draw.</param>
        /// <param name="x">How far across the screen to draw the image (0: left, 1: right).</param>
        /// <param name="y">How far down the screen to draw the image (0: top, 1: bottom).</param>
        /// <param name="w">The width to draw the image, specified as fraction of the width of the surface the image will be drawn on. (0: no width, 1: covers the entire screen).</param>
        public OverlayImage(Bitmap image, float x, float y, float w, string window)
            : this(image, window) {
            mBounds = new RectangleF(x, y, -1f, -1f);
            mAspectRatio = (float) image.Height / (float) image.Width;
        }

        /// <summary>
        /// Create the image image, specifying a position, width and height as fractional values, relative to whatever surface the image will be drawn on.
        /// </summary>
        /// <param name="image">The image to draw.</param>
        /// <param name="x">How far across the screen to draw the image (0: left, 1: right).</param>
        /// <param name="y">How far down the screen to draw the image (0: top, 1: bottom).</param>
        /// <param name="w">The width to draw the image, specified as fraction of the width of the surface the image will be drawn on. (0: no width, 1: covers the entire screen).</param>
        /// <param name="h">The width to draw the image, specified as fraction of the height of the surface the image will be drawn on. (0: no height, 1: covers the entire screen).</param>
        public OverlayImage(Bitmap image, float x, float y, float w, float h, string window)
            : this(image, window) {
            mBounds = new RectangleF(x, y, w, h);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace Chimera.Overlay {
    public class ImageSelection : HoverTrigger, ISelectable {
        private readonly double mSelectTime = 1500;

        private Window mWindow;
        private Bitmap mImage;
        private Bitmap mScaledImage;

        public Bitmap ScaledImage {
            get { return mScaledImage; }
            set {
            }
        }

        public Bitmap Image {
            get { return mImage; }
            set {
                mImage = value;
                window_MonitorChanged(mWindow, mWindow.Monitor);
                if (ImageChanged != null)
                    ImageChanged(this, null);
            }
        }

        public ImageHoverTrigger(Bitmap image, float x, float y, float w, float h)
            : base(x, y, w, h) {
            mImage = image;
        }


        public ImageHoverTrigger(string imageFile, float x, float y, float w, float h)
            : this(new Bitmap(imageFile), x, y, w, h) {
        }

        public ImageHoverTrigger(Window window, Bitmap image, float x, float y, float w, float h)
            : this(image, x, y, w, h) {
            Init(window);
        }

        public ImageHoverTrigger(Window window, string imageFile, float x, float y, float w, float h)
            : this(window, new Bitmap(imageFile), x, y, w, h) {
        }

        private void window_MonitorChanged(Window window, Screen screen) {
            mScaledImage = new Bitmap(mImage, (int) (Bounds.Width * screen.Bounds.Width), (int) (Bounds.Height * screen.Bounds.Height));
        }

        #region ISelectable Members

        public override event Action<ISelectable> StaticChanged;

        public override string DebugState {
            get { 
                return base.DebugState;
            }
        }

        public override void Init(Window window) {
            base.Init(window);

            mWindow = window;

            mWindow.MonitorChanged += new Action<Window, Screen>(window_MonitorChanged);

            window_MonitorChanged(mWindow, mWindow.Monitor);
        }

        public override void DrawStatic(Graphics graphics, Rectangle clipRectangle) {
            graphics.DrawImage(mScaledImage, Bounds.Location);
        }

        public override void Show() {
            Visible = true;
        }

        public override void Hide() {
            Visible = false;
        }

        #endregion

        /// <summary>
        /// Triggered when the image this area renders as its selectable area changes.
        /// </summary>
        public event EventHandler ImageChanged;
    }
}

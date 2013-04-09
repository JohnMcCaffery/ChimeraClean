using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using System.Drawing;

namespace Chimera.Overlay.States {
    public class ImageBGWindow : WindowState {
        private Bitmap mBG;

        public Bitmap BackgroundImage {
            get { return mBG; }
            set {
                mBG = value;
                Manager.ForceRedrawStatic();
            }
        }

        public ImageBGWindow(WindowOverlayManager manager, Bitmap BG)
            : base(manager) {
            mBG = BG;
        }

        public override void DrawStatic(Graphics graphics) {
            graphics.DrawImage(mBG, Clip);
            base.DrawStatic(graphics);
        }

        protected override void OnActivated() {
            Manager.ControlPointer = true;
            Manager.Opacity = 1.0; ;
        }
    }
}

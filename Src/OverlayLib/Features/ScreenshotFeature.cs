using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using System.Drawing;

namespace Chimera.Overlay.Features {
    public class ScreenshotFeature : IFeature {
        private WindowOverlayManager mManager;
        private Bitmap mScreenshot;
        private Rectangle mClip;
        private bool mActive;

        #region IFeature Members

        public System.Drawing.Rectangle Clip {
            get { return mClip; }
            set { mClip = value; }
        }

        public bool Active {
            get { return mActive; }
            set {
                mActive = value;
                if (value) {
                    mScreenshot = new Bitmap(mManager.Window.Monitor.Bounds.Width, mManager.Window.Monitor.Bounds.Height);
                    using (Graphics g = Graphics.FromImage(mScreenshot)) {
                        g.CopyFromScreen(mManager.Window.Monitor.Bounds.Location, Point.Empty, mManager.Window.Monitor.Bounds.Size);
                    }
                }
            }
        }

        public bool NeedsRedrawn {
            get { return false; }
        }

        public string Window {
            get { return mManager.Window.Name; }
        }

        public void DrawStatic(System.Drawing.Graphics graphics) {
            graphics.DrawImage(mScreenshot, mClip.Location);
        }

        public void DrawDynamic(System.Drawing.Graphics graphics) { }

        #endregion

        #region IControllable Members

        public System.Windows.Forms.Control ControlPanel {
            get { throw new NotImplementedException(); }
        }

        public string Name {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}

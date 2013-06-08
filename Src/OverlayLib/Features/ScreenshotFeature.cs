using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using System.Drawing;
using System.Xml;

namespace Chimera.Overlay.Features {
    public class ScreenshotFeature : XmlLoader, IFeature {
        private WindowOverlayManager mManager;
        private Bitmap mScreenshot;
        private Rectangle mClip;
        private bool mActive;
        private string mName;

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

        public string Frame {
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
            get { return mName; }
            set { mName = value; }
        }

        #endregion

        public ScreenshotFeature(OverlayPlugin plugin, XmlNode node) {
            mName = GetName(node, "Screenshot Feature");
            mManager = GetManager(plugin, node, "Screenshot Feature");
        }
    }
}

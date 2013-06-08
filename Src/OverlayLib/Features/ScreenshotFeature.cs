using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using System.Drawing;
using System.Xml;

namespace Chimera.Overlay.Features {    public class ScreenshotFeatureFactory : IFeatureFactory {
        #region IFactory<IFeature> Members

        public IFeature Create(OverlayPlugin manager, XmlNode node) {
            return new ScreenshotFeature(manager, node);
        }

        public IFeature Create(OverlayPlugin manager, XmlNode node, Rectangle clip) {
            return Create(manager, node);
        }

        #endregion

        #region IFactory Members

        public string Name {
            get { return "Screenshot"; }
        }

        #endregion
    }

    public class ScreenshotFeature : XmlLoader, IFeature {
        private WindowOverlayManager mManager;
        private Bitmap mScreenshot;
        private Rectangle mClip;
        private bool mActive;
        private bool mIncludeOverlay;

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
                    //bool launched = mManager.Visible;
                    //mManager.Close();
                    using (Graphics g = Graphics.FromImage(mScreenshot)) {
                        g.CopyFromScreen(mManager.Window.Monitor.Bounds.Location, Point.Empty, mManager.Window.Monitor.Bounds.Size);
                        if (mIncludeOverlay && mManager != null && mManager.CurrentDisplay != null) {
                            mManager.CurrentDisplay.DrawStatic(g);
                        }
                    }
                    mScreenshot.Save("Images/screenshot.jpg");

                    //if (launched)
                        //mManager.Launch();
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

        public ScreenshotFeature(OverlayPlugin plugin, XmlNode node) {
            mManager = GetManager(plugin, node, "Screenshot Feature");
            mIncludeOverlay = GetBool(node, false, "IncludeOverlay");
        }
    }
}

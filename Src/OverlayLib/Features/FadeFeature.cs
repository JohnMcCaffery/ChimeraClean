using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using System.Drawing;
using System.Xml;
using Chimera.Overlay.Transitions;

namespace Chimera.Overlay.Features {
    public class FadeFeature : XmlLoader, IFeature {
        private IFeature mStart;
        private IFeature mFinish;
        private IImageTransition mTransition;

        private Rectangle mClip;
        private bool mActive;
        private string mName;
        private string mFrame;

        #region IFeature Members

        public System.Drawing.Rectangle Clip {
            get { return mClip; }
            set { mClip = value; }
        }

        public bool Active {
            get {
                return mActive;
            }
            set {
                if (value == mActive)
                    return;

                if (value) {
                    Bitmap from = new Bitmap(Clip.Width, Clip.Height);
                    Bitmap to = new Bitmap(Clip.Width, Clip.Height);
                    using (Graphics g = Graphics.FromImage(from))
                        mStart.DrawStatic(g);
                    using (Graphics g = Graphics.FromImage(to))
                        mFinish.DrawStatic(g);

                    mTransition.Init(from, to);
                    mTransition.Begin();
                } else
                    mTransition.Cancel();
                mActive = value;
            }
        }

        public bool NeedsRedrawn {
            get { return mTransition.NeedsRedrawn; }
        }

        public string Frame {
            get { return mFrame; }
        }

        public void DrawStatic(System.Drawing.Graphics graphics) {
            mFinish.DrawStatic(graphics);
        }

        public void DrawDynamic(System.Drawing.Graphics graphics) {
            mTransition.DrawDynamic(graphics);
        }

        #endregion

        public FadeFeature(OverlayPlugin plugin, XmlNode node) {
            mName = GetName(node, "Fade Feature");
            mFrame = GetManager(plugin, node, "Fade Feature").Name;
            mStart = plugin.GetFeature(node.SelectSingleNode("child::Start"), "Fade Feature", null);
            mFinish = plugin.GetFeature(node.SelectSingleNode("child::Finish"), "Fade Feature", null);
            mTransition = plugin.GetImageTransition(node.SelectSingleNode("child::Transition"), "Fade Feature", null);
        }
    }
}

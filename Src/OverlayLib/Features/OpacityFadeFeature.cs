using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using System.Drawing;
using System.Xml;

namespace Chimera.Overlay.Features {
    public class OpacityFadeFeatureFactory : IFeatureFactory {
        public IFeature Create(OverlayPlugin manager, XmlNode node) {
            return new OpacityFadeFeature(manager, node);
        }

        public IFeature Create(OverlayPlugin manager, XmlNode node, Rectangle clip) {
            return Create(manager, node);
        }

        public string Name {
            get { return "OpacityFade"; }
        }
    }

    public class OpacityFadeFeature : OverlayXmlLoader, IFeature {
        private FrameOverlayManager mManager;
        private Rectangle mClip;
        private DateTime mActivated;
        private double mStart, mFinish;
        private double mWait;
        private double mLength = 1000;
        private bool mActive;
        private bool mFinished;

        public OpacityFadeFeature(OverlayPlugin manager, XmlNode node) {
            mManager = GetManager(manager, node, "Opacity Fade");

            if (node.Attributes["FadeIn"] != null) {
                mStart = GetBool(node, true, "FadeIn") ? 0.0 : 1.0;
                mFinish = GetBool(node, true, "FadeIn") ? 1.0 : 0.0;
            }

            mStart = GetDouble(node, mStart, "Start");
            mFinish = GetDouble(node, mFinish, "Finish");
            mWait = GetDouble(node, 0.0, "Wait");
            mLength = GetDouble(node, mLength, "Length");
        }

        public System.Drawing.Rectangle Clip {
            get { return mClip; }
            set { mClip = value; }
        }

        public bool Active {
            get { return mActive; }
            set {
                if (mActive != value) {
                    mActive = value;
                    if (value) {
                        mFinished = false;
                        mActivated = DateTime.Now;
                        mManager.Opacity = mStart;
                    }
                }
            }
        }

        public bool NeedsRedrawn {
            get { return mActive && (Time <= 1.0 || !mFinished); }
        }

        public string Frame {
            get { return mManager.Name; }
        }

        public void DrawStatic(System.Drawing.Graphics graphics) { }

        public void DrawDynamic(System.Drawing.Graphics graphics) {
            if (Time >= 0.0 && Time < 1.0)
                mManager.Opacity = ((mFinish - mStart) * Time) + mStart;
            else if (Time >= 1.0) {
                mManager.Opacity = mFinish;
                mFinished = true;
            }
        }

        private double Time {
            get {
                double ellapsed = DateTime.Now.Subtract(mActivated).TotalMilliseconds - Math.Min(0.0, mWait);
                return ellapsed / mLength;
            }
        }
    }
}

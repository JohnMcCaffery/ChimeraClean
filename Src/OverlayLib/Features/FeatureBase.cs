using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace Chimera.Overlay.Features {
    public abstract class FeatureBase : OverlayXmlLoader, IFeature {
        private Rectangle mClip;
        private bool mActive;
        private FrameOverlayManager mManager;
        private Control mControlPanel;
        private string mName;

        protected FeatureBase(OverlayPlugin plugin, XmlNode node) {
            mManager = GetManager(plugin, node, "FeatureBase");
        }

        protected FeatureBase(OverlayPlugin plugin, XmlNode node, Rectangle clip) : this(plugin, node) {
            mClip = clip;
        }

        protected FrameOverlayManager Manager { get { return mManager; } }

        public Rectangle Clip {
            get { return mClip; ; }
            set { mClip = value; }
        }

        public virtual bool Active {
            get { return mActive; }
            set { mActive = value; }
        }

        public virtual bool NeedsRedrawn {
            get { return false; }
        }

        public string Frame {
            get { return mManager.Frame.Name; }
        }

        public virtual void DrawStatic(System.Drawing.Graphics graphics) { }

        public virtual void DrawDynamic(System.Drawing.Graphics graphics) { }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace Chimera.Overlay.Features {
    public abstract class ControlFeature<TControl> : OverlayXmlLoader, IFeature where  TControl : System.Windows.Forms.Control {
        private string mFrame;        /// <summary>
        /// The clip rectangle bounding the area this item will be drawn to.
        /// </summary>
        private Rectangle mClip;
 
        private FrameOverlayManager mManager;
        private OverlayPlugin mPlugin;

        private RectangleF mBounds;
        private bool mSingletonControl;

        private static TControl sControl;
        private TControl mControl;
        private Control mControlPanel;

        private bool mActive;

        protected abstract Func<TControl> MakeControl { get; }
        protected abstract Func<Control> MakeControlPanel { get; }

        protected ControlFeature(OverlayPlugin manager, XmlNode node, bool singletonControl) {
            mPlugin = manager;
            mManager = GetManager(manager, node, "control feature");
            mFrame = mManager.Frame.Name;
            mSingletonControl = singletonControl;

            float x = GetFloat(node, 0f, "X");
            float y = GetFloat(node, 0f, "Y");
            float w = GetFloat(node, 1f, "W", "Width");
            float h = GetFloat(node, 1f, "H", "Height");
            mBounds = new RectangleF(x, y, w, h);
        }

        protected ControlFeature(OverlayPlugin manager, XmlNode node, bool SingletonControl, Rectangle clip)
            : this(manager, node, SingletonControl) {
            mClip = clip;

            mBounds.X = mBounds.X / clip.Width;
            mBounds.Y = mBounds.Y / clip.Height;
            mBounds.Width = GetFloat(node, mClip.Width, "W", "Width") / clip.Width;
            mBounds.Height = GetFloat(node, mClip.Height, "H", "Height") / clip.Height;
        }

        protected TControl Control {
            get {
                if (mSingletonControl && sControl == null)
                    sControl = MakeControl();
                else if (!mSingletonControl && mControl == null)
                    mControl = MakeControl();
                TControl ctrl = mSingletonControl ? sControl : mControl;
                ctrl.Dock = DockStyle.None;
                return ctrl;
            }
        }

        protected FrameOverlayManager Manager {
            get { return mManager; }
        }

        /// <summary>
        /// The position / size for the image. Specified as a relative values.
        /// For location: 0,0 = top left, 1,1 = bottom right.
        /// For size: 0,0 = invisible, 1,1 = same size as the screen
        /// </summary>
        public RectangleF Bounds {
            get {
                //RectangleF bounds = new RectangleF(mBounds.X, mBounds.Y, mBounds.Width, mBounds.Height);
                return mBounds;
            }
        }

        /** IFeature properties */

        public virtual Rectangle Clip {
            get { return mClip; }
            set { mClip = value; }
        }

        public virtual bool Active {
            get { return mActive; }
            set {
                if (value != mActive) {
                    if (value)
                        mManager.AddControl(Control, mBounds);
                    else
                        mManager.RemoveControl(Control);
                    mActive = value;
                }
            }
        }

        public bool NeedsRedrawn {
            get { return false; }
        }

        public string Frame {
            get { return mFrame; }
        }

        public void DrawStatic(System.Drawing.Graphics graphics) { }

        public void DrawDynamic(System.Drawing.Graphics graphics) { }

        public override Control ControlPanel {
            get {
                if (MakeControlPanel == null)
                    return base.ControlPanel;

                if (mControlPanel == null)
                    mControlPanel = MakeControlPanel();
                return mControlPanel;
            }
        }
    }
}

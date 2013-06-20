using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using System.Drawing;
using System.Xml;

namespace Chimera.Overlay.Triggers {
    public abstract class AreaTrigger : XmlLoader, ITrigger {
        /// <summary>
        /// The manager which will supply the cursor position.
        /// </summary>
        private readonly FrameOverlayManager mManager;
        private Action<FrameOverlayManager, EventArgs> mMoveListener;

        /// <summary>
        /// The bounds defining the area which the cursor can hover over to trigger this selector. The bounds are specified as scaled values between 0,0 and 1,1. 0,0 is top left. 1,1 bottom right.
        /// </summary>
        private RectangleF mBounds;

        /// <summary>
        /// The bounds defining the area which the cursor can hover over to trigger this selector. The bounds are specified as scaled values between 0,0 and 1,1. 0,0 is top left. 1,1 bottom right.
        /// </summary>
        protected virtual RectangleF Bounds {
            get { return mBounds; }
            set { mBounds = value; }
        }

        /// <summary>
        /// Create the trigger. Specifies the position and size of the area the cursor must hover in to trigger this trigger as values between 0 and 1.
        /// 0,0 is top left, 1,1 is bottom right.
        /// </summary>
        /// <param name="manager">The manager which manages the window this trigger is to draw to.</param>
        /// <param name="render">The renderer used to draw this trigger being selected.</param>
        /// <param name="x">The x coordinate for where the image is to be positioned, specified between 0 and 1. 0 is flush to the left, 1 flush to the right.</param>
        /// <param name="y">The y coordinate for where the image is to be positioned, specified between 0 and 1. 0 is flush to the top, 1 flush to the bottom.</param>
        /// <param name="x">The width of the image, specified between 0 and 1. 1 will fill the entire width, 0 will be invisible.</param>
        /// <param name="y">The width of the image, specified between 0 and 1. 1 will fill the entire height, 0 will be invisible.</param>
        public AreaTrigger(FrameOverlayManager manager, float x, float y, float w, float h)
            : this(manager, new RectangleF(x, y, w, h)) {
        }

        public AreaTrigger(FrameOverlayManager manager, int x, int y, int w, int h, Rectangle clip)
            : this(manager, (float) x / (float) clip.Width, (float) y / (float) clip.Height, (float) w / (float) clip.Width, (float) h / (float) clip.Height) {
        }

        public AreaTrigger(FrameOverlayManager manager, RectangleF bounds) {
            mManager = manager;
            mBounds = bounds;
            mMoveListener = new Action<FrameOverlayManager,EventArgs>(mManager_CursorMoved);
        }

        public AreaTrigger(OverlayPlugin manager, XmlNode node) 
            : this(GetManager(manager, node, "trigger"), GetBounds(node, "trigger")) { 
        }

        public AreaTrigger(OverlayPlugin manager, XmlNode node, Rectangle clip) 
            : this(GetManager(manager, node, "trigger"), GetBounds(node, "trigger", clip)) { 
        }

        /// <summary>
        /// The manager which controls the window this trigger renders on.
        /// </summary>
        protected FrameOverlayManager Manager {
            get { return mManager; }
        }

        public string Frame {
            get { return Manager.Frame.Name; }
        }

        public override string ToString() {
            return "Trigger: " + mBounds.X + "," + mBounds.Height;
        }

        private bool mInside;

        protected abstract void Entered();
        protected abstract void Exited();

        protected void Trigger() {
            if (Triggered != null) {
                Triggered();
            }
        }

        public bool Inside {
            get { return Bounds.Contains(Manager.CursorPosition); }
        }

        void mManager_CursorMoved(FrameOverlayManager manager, EventArgs args) {
            if (Inside) {
                if (!mInside) {
                    mInside = true;
                    Entered();
                }
            } else if (mInside) {
                mInside = false;
                Exited();
            }
        }

        private bool mActive;

        public event Action Triggered;

        public virtual bool Active {
            get { return mActive; }
            set {
                if (mActive != value) {
                    mActive = value;
                    if (value) {
                        if (Inside)
                            mManager_CursorMoved(Manager, null);
                        mManager.CursorMoved += mMoveListener;
                    } else
                        mManager.CursorMoved -= mMoveListener;
                }
            }
        }
    }
}

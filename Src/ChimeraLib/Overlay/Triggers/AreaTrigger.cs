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
        private readonly WindowOverlayManager mManager;

        /// <summary>
        /// The bounds defining the area which the cursor can hover over to trigger this selector. The bounds are specified as scaled values between 0,0 and 1,1. 0,0 is top left. 1,1 bottom right.
        /// </summary>
        private RectangleF mBounds;
        /// <summary>
        /// Whether the area is active. If false it will not draw or trigger.
        /// </summary>
        private bool mActive = true;


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
        public AreaTrigger(WindowOverlayManager manager, float x, float y, float w, float h)
            : this(manager, new RectangleF(x, y, w, h)) {
        }

        public AreaTrigger(WindowOverlayManager manager, int x, int y, int w, int h, Rectangle clip)
            : this(manager, (float) x / (float) clip.Width, (float) y / (float) clip.Height, (float) w / (float) clip.Width, (float) h / (float) clip.Height) {
        }

        public AreaTrigger(WindowOverlayManager manager, RectangleF bounds) {
            mManager = manager;
            mBounds = bounds;
        }

        public AreaTrigger(Coordinator coordinator, XmlNode node) {
            mManager = GetManager(coordinator, node);
            mBounds = new RectangleF(GetFloat(node, 0f, "X"), GetFloat(node, 0f, "Y"), GetFloat(node, .1f, "W", "Width"), GetFloat(node, .1f, "H", "Height"));
        }

        public AreaTrigger(Coordinator coordinator, XmlNode node, Rectangle clip) {
            mManager = GetManager(coordinator, node);
            if (node.Attributes["L"] != null) {
                float l = GetFloat(node, 0, "L", "Left");
                float r = GetFloat(node, clip.Width / 10, "R", "Right");
                float t = GetFloat(node, 0, "T", "Top");
                float b = GetFloat(node, clip.Height / 10, "B", "Bottom");
                mBounds = new RectangleF(l / clip.Width, t / clip.Height, (r - l) / clip.Width, (b - t) / clip.Height);
            } else {
                mBounds = new RectangleF(GetFloat(node, 0f, "X"), GetFloat(node, 0f, "Y"), GetFloat(node, .1f, "W", "Width"), GetFloat(node, .1f, "H", "Height"));
            }
        }

        /// <summary>
        /// The manager which controls the window this trigger renders on.
        /// </summary>
        protected WindowOverlayManager Manager {
            get { return mManager; }
        }
        public string Window {
            get { return Manager.Window.Name; }
        }

        #region ITrigger Members

        public abstract event Action Triggered;

        public virtual bool Active {
            get { return mActive; }
            set { mActive = value; }
        }

        #endregion
    }
}

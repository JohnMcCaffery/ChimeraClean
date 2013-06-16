using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using System.Drawing;
using System.Xml;

namespace Chimera.Overlay.Triggers {
    public abstract class AreaTrigger : ConditionTrigger, ITrigger {
        /// <summary>
        /// The manager which will supply the cursor position.
        /// </summary>
        private readonly WindowOverlayManager mManager;

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
        public AreaTrigger(WindowOverlayManager manager, float x, float y, float w, float h, double time)
            : this(manager, new RectangleF(x, y, w, h), time) {
        }

        public AreaTrigger(WindowOverlayManager manager, int x, int y, int w, int h, Rectangle clip, double time)
            : this(manager, (float) x / (float) clip.Width, (float) y / (float) clip.Height, (float) w / (float) clip.Width, (float) h / (float) clip.Height, time) {
        }

        public AreaTrigger(WindowOverlayManager manager, RectangleF bounds, double time)
            : base(manager.Frame.Core, time) {
            mManager = manager;
            mBounds = bounds;
        }

        public AreaTrigger(OverlayPlugin manager, XmlNode node, double time)
            : base(manager.Core, time) {
            mManager = GetManager(manager, node, "trigger");
            mBounds = GetBounds(node, "trigger");
        }

        public AreaTrigger(OverlayPlugin manager, XmlNode node, Rectangle clip, double time)
            : base(manager.Core, time) {
            mManager = GetManager(manager, node, "trigger");
            mBounds = GetBounds(node, "trigger", clip);
        }

        /// <summary>
        /// The manager which controls the window this trigger renders on.
        /// </summary>
        protected WindowOverlayManager Manager {
            get { return mManager; }
        }

        public string Frame {
            get { return Manager.Frame.Name; }
        }

        public override string ToString() {
            return "Trigger: " + mBounds.X + "," + mBounds.Height;
        }

        public override bool Condition {
            get { return Bounds.Contains(Manager.CursorPosition); }
        }
    }
}

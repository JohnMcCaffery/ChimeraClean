using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace FlythroughLib {
    public class LookAtEvent : FlythroughEvent {
        /// <summary>
        /// How many LookAtEvents have been created.
        /// </summary>
        private static int COUNT = 0;
        /// <summary>
        /// The target the event will keep the camera pointing at.
        /// </summary>
        private Vector3 mTarget;
        /// <summary>
        /// The name of the event.
        /// </summary>
        private readonly string mName;

        /// <summary>
        /// Create the event specifying the bare minimum.
        /// </summary>
        /// <param name="container">The container which this event is part of.</param>
        /// <param name="length">The length of time the event will run (ms).</param>
        public LookAtEvent(FlythroughLib.FlythroughManager container, int length)
            : base(container, length) {

            mName = "Look At " + (++COUNT);
        }

        /// <summary>
        /// The target the event will keep the camera pointing at.
        /// </summary>
        public Vector3 Target {
            get { return mTarget; }
            set { mTarget = value; }
        }

        public override string Name {
            get { return mName; }
        }
    
        public override bool Step() {
            if (CurrentStep == 0) {
                Container.OnPositionChange += Container_OnPositionChange;
            }

            if (!DoStep()) {
                Container.OnPositionChange -= Container_OnPositionChange;
                return false;
            }
            return true;
        }

        void Container_OnPositionChange(object sender, EventArgs e) {
            Container.Rotation.LookAtVector = mTarget - Container.Position;
        }

        protected override void LengthChanged() {
            //Do Nothing
        }
    }
}

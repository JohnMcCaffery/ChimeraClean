using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilLib;

namespace FlythroughLib {
    public class CircleEvent : FlythroughEvent {
        /// <summary>
        /// The vector which describes the 0 point. This defines where the arc will start and the plane on which it will rotate.
        /// </summary>
        private Rotation mStart;
        /// <summary>
        /// The length of the arc in degrees.
        /// </summary>
        private float mLength;

        public CircleEvent() : base (null, 0) {
            throw new System.NotImplementedException();
        }
        /// <summary>
        /// The vector which describes the 0 point. This defines where the arc will start and the plane on which it will rotate.
        /// </summary>
        public Rotation Start {
            get {
                throw new System.NotImplementedException();
            }
            set {
            }
        }
    
        public override bool Step() {
            throw new NotImplementedException();
        }

        protected override void LengthChanged() {
            throw new NotImplementedException();
        }
    }
}

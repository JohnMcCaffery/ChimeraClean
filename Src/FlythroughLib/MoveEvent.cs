﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace FlythroughLib {
    public class MoveToEvent : FlythroughEvent {
        /// <summary>
        /// The position the camera will end in.
        /// </summary>
        private Vector3 mTargetPosition;
        /// <summary>
        /// The shift to be applied each step.
        /// </summary>
        private Vector3 mShift;
        /// <summary>
        /// The position the camera started at.
        /// </summary>
        private Vector3 mStartPosition;

        /// <summary>
        /// Initialise the event, specificying where the camera will end up.
        /// </summary>
        /// <param name="target">The position the camera will end up in.</param>
        public MoveToEvent(FlythroughManager container, int length, Vector3 target)
            : base(container, length) {
            mTargetPosition = target;
        }

        /// <summary>
        /// The position the camera will end up at.
        /// </summary>
        public Vector3 Target {
            get { return mTargetPosition;  }
            set { 
                mTargetPosition = value; 
                mShift = (mTargetPosition - mStartPosition) / TotalSteps;
            }
        }
   
        /// <summary>
        /// The position the camera starts at.
        /// </summary>
        public Vector3 Start {
            get { return mStartPosition;  }
            set { 
                mStartPosition = value;
                mShift = (mTargetPosition - mStartPosition) / TotalSteps;
            }
        }
    
        public override bool Step() {
            if (CurrentStep == 0)
                Start= Container.Position;

            Container.SetPosition(mStartPosition + (mShift * CurrentStep));

            return DoStep();
        }

        protected override void LengthChanged() {
            mShift = (mTargetPosition - mStartPosition) / TotalSteps;
        }
    }
}

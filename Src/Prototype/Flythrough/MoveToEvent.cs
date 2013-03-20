using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using System.Xml;

namespace Chimera.Flythrough {
    public class MoveToEvent : FlythroughEvent {
        /// <summary>
        /// How many MoveToEvents have been created.
        /// </summary>
        private static int COUNT = 0;
        /// <summary>
        /// The position the camera will end in.
        /// </summary>
        private Vector3 mTarget;
        /// <summary>
        /// The shift to be applied each step.
        /// </summary>
        private Vector3 mShift;
        /// <summary>
        /// The position the camera started at.
        /// </summary>
        private Vector3 mStartPosition;
        /// <summary>
        /// The name of the event.
        /// </summary>
        private readonly string mName;

        /// <summary>
        /// Initialise the event, specificying where the camera will end up.
        /// </summary>
        /// <param name="target">The position the camera will end up in.</param>
        public MoveToEvent(FlythroughManager container, int length, Vector3 target)
            : base(container, length) {
            mTarget = target;
            mName = "Move To " + (++COUNT);
        }

        /// <summary>
        /// The position the camera will end up at.
        /// </summary>
        public Vector3 Target {
            get { return mTarget;  }
            set { 
                mTarget = value; 
                mShift = (mTarget - mStartPosition) / TotalSteps;
            }
        }
   
        /// <summary>
        /// The position the camera starts at.
        /// </summary>
        public Vector3 Start {
            get { return mStartPosition;  }
            set { 
                mStartPosition = value;
                mShift = (mTarget - mStartPosition) / TotalSteps;
            }
        }

        public override string Name {
            get { return mName; }
        }
    
        public override bool Step() {
            if (CurrentStep == 0)
                Start = Container.Position;

            Container.SetPosition(mStartPosition + (mShift * (CurrentStep + 1)));

            return DoStep();
        }

        protected override void LengthChanged() {
            mShift = (mTarget - mStartPosition) / TotalSteps;
        }

        public override void Load(XmlNode node) {
            Target = Vector3.Parse(node.Attributes["Target"].Value);
            Length = int.Parse(node.Attributes["Length"].Value);
        }

        public override XmlNode Save(XmlDocument doc) {
            XmlNode node = doc.CreateElement("MoveToEvent");

            XmlAttribute target = doc.CreateAttribute("Target");
            XmlAttribute length = doc.CreateAttribute("Length");

            target.Value = mTarget.ToString();
            length.Value = Length.ToString();

            node.Attributes.Append(target);
            node.Attributes.Append(length);

            return node;
        }
    }
}

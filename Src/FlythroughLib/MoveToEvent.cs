using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using System.Xml;
using System.Windows.Forms;
using Chimera.FlythroughLib.GUI;

namespace Chimera.FlythroughLib {
    public class MoveToEvent : FlythroughEvent<Vector3> {
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
        /// The panel used to control the event.
        /// </summary>
        private MoveToPanel mControl;

        /// <summary>
        /// Initialise the event, specificying where the camera will end up.
        /// </summary>
        /// <param name="target">The position the camera will end up in.</param>
        public MoveToEvent(Flythrough container, int length, Vector3 target)
            : base(container, length) {
            Target = target;
            Name = "Move To " + (++COUNT);
        }

        /// <summary>
        /// The position the camera will end up at.
        /// </summary>
        public Vector3 Target {
            get { return mTarget;  }
            set { 
                mTarget = value; 
                mShift = (mTarget - StartValue) / Length;
                TriggerFinishChange(value);
            }
        }

        public override UserControl ControlPanel {
            get { 
                if (mControl == null)
                    mControl = new MoveToPanel(this);
                return mControl; 
            }
        }
        public override Vector3 this[int time] {
            get { return StartValue + (mShift * time); }
        }
        public override Vector3 FinishValue {
            get { return mTarget; }
        }
        public override Vector3 Value {
            get { return StartValue + (mShift * Time); }
        }

        protected override void StartChanged(Vector3 value) {
            mShift = (mTarget - value) / Length;
        }

        protected override void LengthChanged(int time) {
            mShift = (StartValue - mTarget) / Length;
        }

        protected override void StartTimeChanged(int startTime) { }

        protected override void TimeChanged(int value) { }

        public override void Load(XmlNode node) {
            Name = node.Attributes["Name"].Value;
            Length = int.Parse(node.Attributes["Length"].Value);
            Target = Vector3.Parse(node.Attributes["Target"].Value);
        }

        public override XmlNode Save(XmlDocument doc) {
            XmlNode node = doc.CreateElement("MoveToEvent");

            XmlAttribute name = doc.CreateAttribute("Name");
            XmlAttribute target = doc.CreateAttribute("Target");
            XmlAttribute length = doc.CreateAttribute("Length");

            name.Value = Name;
            target.Value = mTarget.ToString();
            length.Value = Length.ToString();

            node.Attributes.Append(name);
            node.Attributes.Append(target);
            node.Attributes.Append(length);

            return node;
        }
    }
}

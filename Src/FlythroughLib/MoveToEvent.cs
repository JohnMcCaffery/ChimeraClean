/*************************************************************************
Copyright (c) 2012 John McCaffery 

This file is part of Chimera.

Chimera is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Chimera is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Chimera.  If not, see <http://www.gnu.org/licenses/>.

**************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using System.Xml;
using System.Windows.Forms;
using Chimera.Flythrough.GUI;

namespace Chimera.Flythrough {
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

        protected override string GetSpecificState() {
            string dump = "";
            dump += String.Format("  {1:-30} {2}{0}", Environment.NewLine, "Target:", mTarget.ToString());
            dump += String.Format("  {1:-30} {2}{0}", Environment.NewLine, "Shift/ms:", mShift.ToString());
            return dump;

        }
    }
}

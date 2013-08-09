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
using System.Xml;
using Chimera.Util;
using System.Windows.Forms;
using Chimera.Flythrough.GUI;
using OpenMetaverse;

namespace Chimera.Flythrough {
    public class LookAtEvent : FlythroughEvent<Rotation>, IPositionListener {
        /// <summary>
        /// How many RotateToEvents have been created.
        /// </summary>
        private static int COUNT = 0;
        /// <summary>
        /// The orientation the camera should end up at.
        /// </summary>
        private Vector3 mTarget = Vector3.Zero;
        /// <summary>
        /// The panel used to control the event.
        /// </summary>
        private LookAtPanel mControl;
        /// <summary>
        /// The sequence which governs the position of the camera parallel to this.
        /// </summary>
        private EventSequence<Vector3>  mPositions;

        /// <summary>
        /// CreateWindowState the event specifying pitch and pitch.
        /// </summary>
        /// <param name="container">The container which this event is part of.</param>
        /// <param name="length">The length of time the event will run (ms).</param>
        /// <param name="pitch">How far the event should rotate the camera around the pitch axis (degrees).</param>
        /// <param name="yaw">How far the event should rotate the camera around the yaw axis (degrees).</param>
        public LookAtEvent(FlythroughPlugin container, int length, Vector3 target)
            : this(container, length) {

            Target = target;
        }

        /// <summary>
        /// 
        /// CreateWindowState the event specifying the bare minimum.
        /// </summary>
        /// <param name="container">The container which this event is part of.</param>
        /// <param name="length">The length of time the event will run (ms).</param>
        public LookAtEvent(FlythroughPlugin container, int length)
            : base(container, length) {

            Name = "Look At " + (++COUNT);
            StartValue = Rotation.Zero;
            Target = Vector3.Zero;
        }

        /// <summary>
        /// How many degrees around the pitch axis to end at.
        /// </summary>
        public Vector3 Target {
            get { return mTarget; }
            set {
                mTarget = value;
                TriggerFinishChange(FinishValue);
            }
        }

        public override UserControl ControlPanel {
            get {
                if (mControl == null)
                    mControl = new LookAtPanel(this);
                return mControl;
            }
        }
        public override Rotation this[int time] {
            get {
                if (mPositions == null)
                    return Rotation.Zero;

                Vector3 pos = mPositions.FinishValue;
                time += SequenceStartTime;
                if (mPositions.Length >= time && mPositions.Length > 0) {
                    FlythroughEvent<Vector3> evt = mPositions[time];
                    pos = evt[time - evt.SequenceStartTime];
                }
                Vector3 lookAt = mTarget - pos;
                Rotation ret = new Rotation(lookAt);
                return ret;
                //return new Rotation(mTarget - pos); 
            }
        }
        public override Rotation FinishValue {
            get { return this[SequenceFinishTime]; }
        }
        public override Rotation Value {
            get { return this[Time]; }
        }

        protected override void StartChanged(Rotation value) { }

        protected override void LengthChanged(int length) { TriggerFinishChange(FinishValue); }

        protected override void StartTimeChanged(int startTime) { TriggerFinishChange(FinishValue); }

        protected override void TimeChanged(int time) { }

        public override void Load(XmlNode node) {
            Name = node.Attributes["Name"].Value;
            Length = int.Parse(node.Attributes["Length"].Value);
            Target = Vector3.Parse(node.Attributes["Target"].Value);
        }

        public override XmlNode Save(XmlDocument doc) {
            XmlNode node = doc.CreateElement("LookAtEvent");

            XmlAttribute name = doc.CreateAttribute("Name");
            XmlAttribute target = doc.CreateAttribute("Target");
            XmlAttribute length = doc.CreateAttribute("Length");
            
            name.Value = Name;
            length.Value = Length.ToString();
            target.Value = mTarget.ToString();

            node.Attributes.Append(name);
            node.Attributes.Append(target);
            node.Attributes.Append(length);

            return node;
        }

        void value_OnChange(object sender, EventArgs e) { TriggerFinishChange(FinishValue); }

        public void Init(EventSequence<Vector3> positions) {
            mPositions = positions;
        }

        protected override string GetSpecificState() {
            string dump = "";
            dump += String.Format("  {1:-30} {2}{0}", Environment.NewLine, "Target:", mTarget.ToString());
            if (mPositions != null) {
                dump += String.Format("  {1:-30} {2}{0}", Environment.NewLine, "Positions Sequence length:", mPositions.Length);
                dump += String.Format("  {1:-30} {2}{0}", Environment.NewLine, "# Parallel Positions:", mPositions.Count);
            } else
                dump += String.Format("  No position sequence set{0}", Environment.NewLine);

            return dump;
        }
    }
}

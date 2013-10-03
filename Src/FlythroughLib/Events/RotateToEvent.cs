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

namespace Chimera.Flythrough {
    public class RotateToEvent : Chimera.Flythrough.FlythroughEvent<Rotation> {
        /// <summary>
        /// How many RotateToEvents have been created.
        /// </summary>
        private static int COUNT = 0;
        /// <summary>
        /// How much the orientation should change every ms.
        /// </summary>
        private Rotation mShift;
        /// <summary>
        /// The orientation the camera should end up at.
        /// </summary>
        private Rotation mTarget = Rotation.Zero;
        /// <summary>
        /// The panel used to control the event.
        /// </summary>
        private RotateToPanel mControl;

        /// <summary>
        /// CreateWindowState the event specifying pitch and pitch.
        /// </summary>
        /// <param name="container">The container which this event is part of.</param>
        /// <param name="length">The length of time the event will run (ms).</param>
        /// <param name="pitch">How far the event should rotate the camera around the pitch axis (degrees).</param>
        /// <param name="yaw">How far the event should rotate the camera around the yaw axis (degrees).</param>
        public RotateToEvent(FlythroughPlugin container, int length, Rotation target)
            : this(container, length) {

            Target = target;
        }

        /// <summary>
        /// CreateWindowState the event specifying the bare minimum.
        /// </summary>
        /// <param name="container">The container which this event is part of.</param>
        /// <param name="length">The length of time the event will run (ms).</param>
        public RotateToEvent(FlythroughPlugin container, int length)
            : base(container, length) {

            Name = "Rotate To " + (++COUNT);
            StartValue = Rotation.Zero;
            Target = Rotation.Zero;
        }

        /// <summary>
        /// How many degrees around the pitch axis to end at.
        /// </summary>
        public Rotation Target {
            get { return mTarget; }
            set {
                mTarget = value;
                Recalculate();
                TriggerFinishChange(value);
                value.Changed += new EventHandler(value_OnChange);
            }
        }

        public override UserControl ControlPanel {
            get {
                if (mControl == null)
                    mControl = new RotateToPanel(this);
                return mControl;
            }
        }
        public override Rotation this[int time] {
            get { return StartValue + (mShift * time); }
        }
        public override Rotation FinishValue {
            get { return mTarget; }
        }
        public override Rotation Value {
            get { return this[Time]; }
        }

        protected override void StartChanged(Rotation value) { Recalculate(); }

        protected override void LengthChanged(int length) { Recalculate(); }

        protected override void StartTimeChanged(int startTime) { }

        protected override void TimeChanged(int time) { }

        public override void Load(XmlNode node) {
            Name = node.Attributes["Name"].Value;
            double PitchTarget = double.Parse(node.Attributes["Pitch"].Value);
            double YawTarget = double.Parse(node.Attributes["Yaw"].Value);
            Length = int.Parse(node.Attributes["Length"].Value);
            Target = new Rotation(PitchTarget, YawTarget);
        }

        public override XmlNode Save(XmlDocument doc) {
            XmlNode node = doc.CreateElement("RotateToEvent");

            XmlAttribute name = doc.CreateAttribute("Name");
            XmlAttribute pitch = doc.CreateAttribute("Pitch");
            XmlAttribute yaw = doc.CreateAttribute("Yaw");
            XmlAttribute length = doc.CreateAttribute("Length");
            
            name.Value = Name;
            pitch.Value = mTarget.Pitch.ToString();
            yaw.Value = mTarget.Yaw.ToString();
            length.Value = Length.ToString();

            node.Attributes.Append(name);
            node.Attributes.Append(pitch);
            node.Attributes.Append(yaw);
            node.Attributes.Append(length);

            return node;
        }

        void value_OnChange(object sender, EventArgs e) {
            Recalculate();
            TriggerFinishChange(FinishValue);
        }
        private void Recalculate() {
            mShift = (FinishValue - StartValue) / Length;
        }

        protected override string GetSpecificState() {
            string dump = "";
            if (mTarget != null)
                dump += String.Format("  {1:-30}  Pitch: {2} - Yaw: {3}{0}", Environment.NewLine, "Target Orientation:", mTarget.Pitch, mTarget.Yaw);
            else
                dump += String.Format("  No target set{0}", Environment.NewLine);

            if (mShift != null)
                dump += String.Format("  {1:-30}  Pitch: {2} - Yaw: {3}{0}", Environment.NewLine, "Shift/ms:", mShift.Pitch, mShift.Yaw);
            else
                dump += String.Format("  No shift calculated{0}", Environment.NewLine);

            return dump;
        }
    }
}

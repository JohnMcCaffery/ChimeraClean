using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Chimera.Util;
using System.Windows.Forms;
using Chimera.FlythroughLib.GUI;

namespace Chimera.FlythroughLib {
    public class RotateToEvent : FlythroughEvent<Rotation> {
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
        private Rotation mTarget = new Rotation();
        /// <summary>
        /// The panel used to control the event.
        /// </summary>
        private RotateToPanel mControl;

        /// <summary>
        /// Create the event specifying pitch and pitch.
        /// </summary>
        /// <param name="container">The container which this event is part of.</param>
        /// <param name="length">The length of time the event will run (ms).</param>
        /// <param name="pitch">How far the event should rotate the camera around the pitch axis (degrees).</param>
        /// <param name="yaw">How far the event should rotate the camera around the yaw axis (degrees).</param>
        public RotateToEvent(Flythrough container, int length, Rotation target)
            : this(container, length) {

            Target = target;
        }

        /// <summary>
        /// Create the event specifying the bare minimum.
        /// </summary>
        /// <param name="container">The container which this event is part of.</param>
        /// <param name="length">The length of time the event will run (ms).</param>
        public RotateToEvent(Flythrough container, int length)
            : base(container, length) {

            Name = "Rotate To " + (++COUNT);
            StartValue = new Rotation();
            Target = new Rotation();
        }

        /// <summary>
        /// How many degrees around the pitch axis to end at.
        /// </summary>
        public Rotation Target {
            get { return mTarget; }
            set {
                mTarget = value;
                mShift = (value - StartValue) / Length;
                TriggerFinishChange(value);
                value.OnChange += new EventHandler(value_OnChange);
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
    }
}
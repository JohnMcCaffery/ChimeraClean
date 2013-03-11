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
        /// How much to change pitch by every step.
        /// </summary>
        private double mPitchShift;
        /// <summary>
        /// How much to change pitch by ever step.
        /// </summary>
        private double mYawShift;
        /// <summary>
        /// How many degrees around the pitch axis to start at.
        /// </summary>
        private double mPitchStart;
        /// <summary>
        /// How many degrees around the yaw axis to start at.
        /// </summary>
        private double mYawStart;
        /// <summary>
        /// How many degrees around the yaw axis to finish at
        /// </summary>
        private double mYawTarget;
        /// <summary>
        /// The panel used to control the event.
        /// </summary>
        private RotateToPanel mControl;
        /// <summary>
        /// How many degrees around the pitch axis to end at.
        /// </summary>
        private double mPitchTarget;

        /// <summary>
        /// Create the event specifying pitch and pitch.
        /// </summary>
        /// <param name="container">The container which this event is part of.</param>
        /// <param name="length">The length of time the event will run (ms).</param>
        /// <param name="pitch">How far the event should rotate the camera around the pitch axis (degrees).</param>
        /// <param name="yaw">How far the event should rotate the camera around the yaw axis (degrees).</param>
        public RotateToEvent(Flythrough container, int length, double pitch, double yaw)
            : this(container, length) {

            PitchTarget = pitch;
            YawTarget = yaw;
        }

        /// <summary>
        /// Create the event specifying the bare minimum.
        /// </summary>
        /// <param name="container">The container which this event is part of.</param>
        /// <param name="length">The length of time the event will run (ms).</param>
        public RotateToEvent(Flythrough container, int length)
            : base(container, length) {

            Name = "Rotate To " + (++COUNT);
        }

        /// <summary>
        /// How many degrees around the pitch axis to start at.
        /// </summary>
        public double PitchStart {
            get { return mPitchStart; }
            set {
                mPitchStart = value;
                mPitchShift = (mPitchTarget - value) / Length;
            }
        }

        /// <summary>
        /// How many degrees around the yaw axis to start at.
        /// </summary>
        public double YawStart {
            get { return mYawStart; }
            set {
                mYawStart = value;
                mYawShift = (mYawTarget - value) / Length;
            }
        }

        /// <summary>
        /// How many degrees around the pitch axis to end at.
        /// </summary>
        public double PitchTarget {
            get { return mPitchTarget; }
            set {
                mPitchTarget = value;
                mPitchShift = (value - mPitchStart) / Length;
            }
        }

        /// <summary>
        /// How many degrees around the yaw axis to finish at
        /// </summary>
        public double YawTarget {
            get { return mYawTarget; }
            set {
                mYawTarget = value;
                mYawShift = (value - mYawStart) / Length;
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
            get { return new Rotation(mPitchStart + (mPitchShift * time), mYawStart + (mYawShift * time)); }
        }
        public override Rotation Finish {
            get { return this[Length]; }
        }
        public override Rotation Value {
            get { return this[Time]; }
        }

        protected override void TimeChanged(int time) { }

        protected override void StartChanged(Rotation value) { }

        protected override void LengthChanged(int length) {
            mPitchShift = (mPitchTarget - mPitchStart) / Length;
            mYawShift = (mYawTarget - mYawStart) / Length;
        }

        public override void Load(XmlNode node) {
            Name = node.Attributes["Name"].Value;
            PitchTarget = double.Parse(node.Attributes["Pitch"].Value);
            YawTarget = double.Parse(node.Attributes["Yaw"].Value);
            Length = int.Parse(node.Attributes["Length"].Value);
        }

        public override XmlNode Save(XmlDocument doc) {
            XmlNode node = doc.CreateElement("RotateToEvent");

            XmlAttribute name = doc.CreateAttribute("Name");
            XmlAttribute pitch = doc.CreateAttribute("Pitch");
            XmlAttribute yaw = doc.CreateAttribute("Yaw");
            XmlAttribute length = doc.CreateAttribute("Length");
            
            name.Value = Name;
            pitch.Value = mPitchTarget.ToString();
            yaw.Value = mYawTarget.ToString();
            length.Value = Length.ToString();

            node.Attributes.Append(name);
            node.Attributes.Append(pitch);
            node.Attributes.Append(yaw);
            node.Attributes.Append(length);

            return node;
        }
    }
}
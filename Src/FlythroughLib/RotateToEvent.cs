using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace FlythroughLib {
    public class RotateToEvent : FlythroughEvent {
        /// <summary>
        /// How many RotateToEvents have been created.
        /// </summary>
        private static int COUNT = 0;
        /// <summary>
        /// How much to change pitch by every step.
        /// </summary>
        private float mPitchShift;
        /// <summary>
        /// How much to change pitch by ever step.
        /// </summary>
        private float mYawShift;
        /// <summary>
        /// How many degrees around the pitch axis to start at.
        /// </summary>
        private float mPitchStart;
        /// <summary>
        /// How many degrees around the yaw axis to start at.
        /// </summary>
        private float mYawStart;
        /// <summary>
        /// How many degrees around the yaw axis to finish at
        /// </summary>
        private float mYawTarget;
        /// <summary>
        /// How many degrees around the pitch axis to end at.
        /// </summary>
        private float mPitchTarget;
        /// <summary>
        /// The name of the event.
        /// </summary>
        private readonly string mName;

        /// <summary>
        /// Create the event specifying pitch and pitch.
        /// </summary>
        /// <param name="container">The container which this event is part of.</param>
        /// <param name="length">The length of time the event will run (ms).</param>
        /// <param name="pitch">How far the event should rotate the camera around the pitch axis (degrees).</param>
        /// <param name="yaw">How far the event should rotate the camera around the yaw axis (degrees).</param>
        public RotateToEvent(FlythroughManager container, int length, float pitch, float yaw)
            : this(container, length) {

            PitchTarget = pitch;
            YawTarget = yaw;
        }

        /// <summary>
        /// Create the event specifying the bare minimum.
        /// </summary>
        /// <param name="container">The container which this event is part of.</param>
        /// <param name="length">The length of time the event will run (ms).</param>
        public RotateToEvent(FlythroughManager container, int length)
            : base(container, length) {

            mName = "Rotate To " + (++COUNT);
        }


        /// <summary>
        /// How many degrees around the pitch axis to start at.
        /// </summary>
        public float PitchStart {
            get { return mPitchStart; }
            set {
                mPitchStart = value;
                mPitchShift = (mPitchTarget - value) / TotalSteps;
            }
        }

        /// <summary>
        /// How many degrees around the yaw axis to start at.
        /// </summary>
        public float YawStart {
            get { return mYawStart; }
            set {
                mYawStart = value;
                mYawShift = (mYawTarget - value) / TotalSteps;
            }
        }

        /// <summary>
        /// How many degrees around the pitch axis to end at.
        /// </summary>
        public float PitchTarget {
            get { return mPitchTarget; }
            set {
                mPitchTarget = value;
                mPitchShift = (value - mPitchStart) / TotalSteps;
            }
        }

        /// <summary>
        /// How many degrees around the yaw axis to finish at
        /// </summary>
        public float YawTarget {
            get { return mYawTarget; }
            set {
                mYawTarget = value;
                mYawShift = (value - mYawStart) / TotalSteps;
            }
        }

        public override string Name {
            get { return mName; }
        }
    
        public override bool Step() {
            if (CurrentStep == 0) {
                PitchStart = Container.Rotation.Pitch;
                YawStart = Container.Rotation.Yaw;
            }

            Container.Rotation.Pitch += mPitchShift;
            Container.Rotation.Yaw += mYawShift;

            return DoStep();
        }

        protected override void LengthChanged() {
            mPitchShift = (mPitchTarget - mPitchStart) / TotalSteps;
            mYawShift = (mYawTarget - mYawStart) / TotalSteps;
        }

        public override void Load(XmlNode node) {
            PitchTarget = float.Parse(node.Attributes["Pitch"].Value);
            YawTarget = float.Parse(node.Attributes["Yaw"].Value);
            Length = int.Parse(node.Attributes["Length"].Value);
        }

        public override XmlNode Save(XmlDocument doc) {
            XmlNode node = doc.CreateElement("RotateToEvent");

            XmlAttribute pitch = doc.CreateAttribute("Pitch");
            XmlAttribute yaw = doc.CreateAttribute("Yaw");
            XmlAttribute length = doc.CreateAttribute("Length");
            
            pitch.Value = mPitchTarget.ToString();
            yaw.Value = mYawTarget.ToString();
            length.Value = Length.ToString();

            node.Attributes.Append(pitch);
            node.Attributes.Append(yaw);
            node.Attributes.Append(length);

            return node;
        }
    }
}
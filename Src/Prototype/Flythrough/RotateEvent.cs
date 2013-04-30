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

namespace Chimera.Flythrough {
    public class RotateEvent : FlythroughEvent {
        /// <summary>
        /// How many LookAtEvents have been created.
        /// </summary>
        private static int COUNT = 0;
        /// <summary>
        /// How much to change pitch by every step.
        /// </summary>
        private float mPitchShift;
        /// <summary>
        /// How much to change pitch by every step.
        /// </summary>
        private float mYawShift;
        /// <summary>
        /// How many degrees to rotate around the pitch axis.
        /// </summary>
        public float mPitchDelta;
        /// <summary>
        /// How many degrees to rotate around the pitch axis.
        /// </summary>
        public float mYawDelta;
        /// <summary>
        /// The name of the event.
        /// </summary>
        private readonly string mName;

        /// <summary>
        /// CreateWindowState the event specifying pitch and pitch.
        /// </summary>
        /// <param name="container">The container which this event is part of.</param>
        /// <param name="length">The length of time the event will run (ms).</param>
        /// <param name="pitch">How far the event should rotate the camera around the pitch axis (degrees).</param>
        /// <param name="yaw">How far the event should rotate the camera around the yaw axis (degrees).</param>
        public RotateEvent(FlythroughManager container, int length, float pitch, float yaw)
            : this(container, length) {

            mPitchDelta = pitch;
            mYawDelta = yaw;
        }

        /// <summary>
        /// CreateWindowState the event specifying the bare minimum.
        /// </summary>
        /// <param name="container">The container which this event is part of.</param>
        /// <param name="length">The length of time the event will run (ms).</param>
        public RotateEvent(FlythroughManager container, int length)
            : base(container, length) {

            mName = "Rotate " + (++COUNT);
        }
   

        /// <summary>
        /// How many degrees to rotate around the pitch axis.
        /// </summary>
        public float PitchDelta {
            get { return mPitchDelta; }
            set { 
                mPitchDelta = value;
                mPitchShift = value / TotalSteps;
            }
        }

        /// <summary>
        /// How many degrees to rotate around the pitch axis.
        /// </summary>
        public float YawDelta {
            get { return mYawDelta; }
            set {
                mYawDelta = value;
                mYawShift = value / TotalSteps;
            }
        }

        public override string Name {
            get { return mName; }
        }
 
        public override bool Step() {
            Container.Rotation.Yaw += mYawShift;
            Container.Rotation.Pitch += mYawShift;
            return DoStep();
        }

        protected override void LengthChanged() {
            mPitchShift = mPitchDelta / TotalSteps;
            mYawShift = mYawDelta / TotalSteps;
        }

        public override void Load(XmlNode node) {
            PitchDelta = float.Parse(node.Attributes["Pitch"].Value);
            YawDelta = float.Parse(node.Attributes["Yaw"].Value);
        }

        public override XmlNode Save(XmlDocument doc) {
            XmlNode node = doc.CreateElement("RotateEvent");

            XmlAttribute pitch = doc.CreateAttribute("Pitch");
            XmlAttribute yaw = doc.CreateAttribute("Yaw");
            
            pitch.Value = mPitchDelta.ToString();
            yaw.Value = mYawDelta.ToString();

            node.Attributes.Append(pitch);
            node.Attributes.Append(yaw);

            return node;
        }
    }
}

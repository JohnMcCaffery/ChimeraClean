using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.FlythroughLib;
using OpenMetaverse;
using Chimera.Util;
using System.Xml;

namespace FlythroughLib {
    public class ComboEvent : FlythroughEvent<Camera> {
        private readonly EventSequence<Vector3> mPositionSequence = new EventSequence<Vector3>();
        private readonly EventSequence<Rotation> mOrientationSequence = new EventSequence<Rotation>();

        /// <summary>
        /// Triggered whenever the current position event changes.
        /// Supplies the old current event and the new one.
        /// </summary>
        public event Action<FlythroughEvent<Vector3>, FlythroughEvent<Vector3>> CurrentPositionEventChange {
            add { mPositionSequence.CurrentEventChange += value; }
            remove { mPositionSequence.CurrentEventChange -= value; }
        }
        /// <summary>
        /// Triggered whenever the current position event changes.
        /// Supplies the old current event and the new one.
        /// </summary>
        public event Action<FlythroughEvent<Rotation>, FlythroughEvent<Rotation>> CurrentOrientationEventChange {
            add { mOrientationSequence.CurrentEventChange += value; }
            remove { mOrientationSequence.CurrentEventChange -= value; }
        }

        /// <summary>
        /// </summary>
        /// <param name="flythrough">The flythrough this event is part of.</param>
        /// <param name="length">The length of time the event will run (ms).</param>
        public ComboEvent(Flythrough flythrough, int length)
            : base(flythrough, length) {

            mPositionSequence.LengthChange += new Action<EventSequence<Vector3>,int>(mPositionSequence_LengthChange);
            mPositionSequence.FinishChange += new EventHandler(FinishChanged);

            mOrientationSequence.LengthChange += new Action<EventSequence<Rotation>,int>(mOrientationSequence_LengthChange);
            mOrientationSequence.FinishChange += new EventHandler(FinishChanged);
        }

        public override Camera Start {
            get { return new Camera(mPositionSequence.Start, mOrientationSequence.Start); }
            set {
                mPositionSequence.Start = value.Position;
                mOrientationSequence.Start = value.Orientation;
            }
        }

        public override Camera Finish {
            get { return new Camera(mPositionSequence.Finish, mOrientationSequence.Finish); }
            set { /*Do Nothing*/ }
        }

        public override Camera Value {
            get { return new Camera(mPositionSequence[Time], mOrientationSequence[Time]); }
        }

        public override void TimeChanged(int time) {
            mPositionSequence.Time = Math.Min(mPositionSequence.Length, time);
            mOrientationSequence.Time = Math.Min(mOrientationSequence.Length, time);
        }

        public override void Load(XmlNode node) {
            throw new NotImplementedException();
        }

        public override System.Xml.XmlNode Save(System.Xml.XmlDocument doc) {
            throw new NotImplementedException();
        }

        public void AddEvent(FlythroughEvent<Vector3> evt) {
            mPositionSequence.AddEvent(evt);
            if (evt is IOrientationListener)
                ((IOrientationListener)evt).Init(mOrientationSequence);
        }

        public void AddEvent(FlythroughEvent<Rotation> evt) {
            mOrientationSequence.AddEvent(evt);
            if (evt is IPositionListener)
                ((IPositionListener)evt).Init(mPositionSequence);
        }

        public void RemoveEvent(FlythroughEvent<Vector3> evt) {
            mPositionSequence.RemoveEvent(evt);
        }

        public void RemoveEvent(FlythroughEvent<Rotation> evt) {
            mOrientationSequence.RemoveEvent(evt);
        }

        private void FinishChanged(object source, EventArgs args) {
            TriggerFinishChange(Finish);
        }

        private void mPositionSequence_LengthChange(EventSequence<Vector3> evt, int length) {
            Length = Math.Max(mPositionSequence.Length, mOrientationSequence.Length);
        }

        private void mOrientationSequence_LengthChange(EventSequence<Rotation> evt, int length) {
            Length = Math.Max(mPositionSequence.Length, mOrientationSequence.Length);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using Chimera.Util;
using System.Xml;
using Chimera.FlythroughLib.GUI;
using System.Windows.Forms;

namespace Chimera.FlythroughLib {
    public class ComboEvent : FlythroughEvent<Camera> {
        private static int COUNT = 0;
        private readonly EventSequence<Vector3> mPositionSequence = new EventSequence<Vector3>();
        private readonly EventSequence<Rotation> mOrientationSequence = new EventSequence<Rotation>();
        private ComboPanel mControl;

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

        public EventSequence<Vector3> Positions {
            get { return mPositionSequence; }
        }
        public EventSequence<Rotation> Orientations {
            get { return mOrientationSequence; }
        }

        /// <summary>
        /// </summary>
        /// <param name="flythrough">The flythrough this event is part of.</param>
        /// <param name="length">The length of value the event will run (ms).</param>
        public ComboEvent(Flythrough flythrough)
            : base(flythrough, 1) {

            mPositionSequence.LengthChange += new Action<EventSequence<Vector3>,int>(mPositionSequence_LengthChange);
            mPositionSequence.FinishChange += new EventHandler(FinishChanged);

            mOrientationSequence.LengthChange += new Action<EventSequence<Rotation>,int>(mOrientationSequence_LengthChange);
            mOrientationSequence.FinishChange += new EventHandler(FinishChanged);

            mPositionSequence.Start = StartValue.Position;
            mOrientationSequence.Start = StartValue.Orientation;

            Name = "Step " + COUNT++;
        }

        public override Camera this[int time] {
            get { return new Camera(mPositionSequence[time].Value, mOrientationSequence[time].Value); }
        }

        public override UserControl ControlPanel {
            get {
                if (mControl == null)
                    mControl = new ComboPanel(this);
                return mControl;
            }
        }

        public override Camera FinishValue {
            get { return new Camera(mPositionSequence.FinishValue, mOrientationSequence.FinishValue); }
        }

        public override Camera Value {
            get {
                Vector3 pos = mPositionSequence.Count == 0 ? 
                    StartValue.Position : 
                    mPositionSequence[Math.Min(Time, mPositionSequence.Length)].Value;
                Rotation rot = mOrientationSequence.Count == 0 ? 
                    StartValue.Orientation : 
                    mOrientationSequence[Math.Min(Time, mOrientationSequence.Length)].Value;
                return new Camera(pos, rot); 
            }
        }

        protected override void StartChanged(Camera value) {
            mPositionSequence.Start = value.Position;
            mOrientationSequence.Start = value.Orientation;
        }

        protected override void TimeChanged(int time) {
            mPositionSequence.Time = Math.Min(mPositionSequence.Length, time);
            mOrientationSequence.Time = Math.Min(mOrientationSequence.Length, time);
        }

        protected override void StartTimeChanged(int startTime) {
            mPositionSequence.StartTime = startTime;
            mOrientationSequence.StartTime = startTime;
        }

        protected override void LengthChanged(int length) { }

        public override void Load(XmlNode node) {
            if (node.FirstChild != null) {
                LoadNode<Vector3>(node.FirstChild);
                if (node.FirstChild.NextSibling != null)
                    LoadNode<Rotation>(node.FirstChild.NextSibling);
            }
        }

        private void LoadNode<T>(XmlNode root) {
            foreach (XmlNode node in root.ChildNodes) {
                if (typeof(T) == typeof(Vector3)) {
                    FlythroughEvent<Vector3> evt = null;
                    switch (node.Name) {
                        case "MoveToEvent": evt = new MoveToEvent(Container, 0, Vector3.Zero); break;
                        case "BlankEvent": evt = new BlankEvent<Vector3>(Container, 0); break;
                    }
                    evt.Load(node);
                    mPositionSequence.AddEvent(evt);
                } else {
                    FlythroughEvent<Rotation> evt = null;
                    switch (node.Name) {
                        case "RotateToEvent": evt = new RotateToEvent(Container, 0, new Rotation()); break;
                        case "BlankEvent": evt = new BlankEvent<Rotation>(Container, 0); break;
                    }
                    evt.Load(node);
                    mOrientationSequence.AddEvent(evt);
                }
            }
        }

        public override XmlNode Save(XmlDocument doc) {
            XmlNode root = doc.CreateElement("ComboEvent");
            XmlNode stream1 = doc.CreateElement("PositionSequence");
            XmlNode stream2 = doc.CreateElement("OrientationSequence");

            foreach(var evt in mPositionSequence)
                root.AppendChild(evt.Save(root.OwnerDocument));

            foreach(var evt in mOrientationSequence)
                root.AppendChild(evt.Save(root.OwnerDocument));

            root.AppendChild(stream1);
            root.AppendChild(stream2);
            return root;
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
            TriggerFinishChange(FinishValue);
        }

        private void mPositionSequence_LengthChange(EventSequence<Vector3> evt, int length) {
            Length = Math.Max(mPositionSequence.Length, mOrientationSequence.Length);
        }

        private void mOrientationSequence_LengthChange(EventSequence<Rotation> evt, int length) {
            Length = Math.Max(mPositionSequence.Length, mOrientationSequence.Length);
        }
    }
}

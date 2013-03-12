using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using OpenMetaverse;
using Chimera.Util;
using System.Threading;
using Chimera.FlythroughLib.GUI;

namespace Chimera.FlythroughLib {
    public struct Camera {
        private Vector3 mPosition;
        private Rotation mOrientation;

        public Vector3 Position { get { return mPosition; } }
        public Rotation Orientation { get { return mOrientation; } }

        public Camera(Vector3 vector3, Rotation rotation) {
            mPosition = vector3;
            mOrientation = rotation;
        }
    }

    public class Flythrough : IInput {
        private EventSequence<Camera> mEvents = new EventSequence<Camera>();
        private Coordinator mCoordinator;
        private FlythroughPanel mPanel;
        private bool mEnabled = true;
        private bool mPlaying = false;
        private bool mLoop = false;
        private bool mAutoStep = true;

        /// <summary>
        /// Triggered whenever the sequence gets to the end, even if looping.
        /// </summary>
        public event EventHandler SequenceFinished;
        /// <summary>
        /// Triggered whenever playback moves value forward one tick.
        /// Not triggered when value is set directly.
        /// </summary>
        public event Action<int> TimeChange;
        /// <summary>
        /// Triggered every value the length of the event is changed.
        /// </summary>
        public event Action<int> LengthChange;
        /// <summary>
        /// Triggered whenever the currently executing event changes.
        /// </summary>
        public event Action<FlythroughEvent<Camera>, FlythroughEvent<Camera>> CurrentEventChange {
            add { mEvents.CurrentEventChange += value; }
            remove { mEvents.CurrentEventChange -= value; }
        }

        /// <summary>
        /// All the events queued up to play.
        /// </summary>
        public FlythroughEvent<Camera>[] Events {
            get { return mEvents.ToArray(); }
        }
        /// <summary>
        /// Where in the current sequence playback has reached.
        /// Between 0 and Length.
        /// </summary>
        public int Time {
            get { return mEvents.Time; }
            set { 
                mEvents.Time = value;
                FlythroughEvent<Camera> evt = mEvents[value];
                if (value == 0)
                    mCoordinator.Update(mEvents.Start.Position, Vector3.Zero, mEvents.Start.Orientation, new Rotation());
                if (mEnabled && evt != null)
                    mCoordinator.Update(evt.Value.Position, Vector3.Zero, evt.Value.Orientation, new Rotation());
                if (TimeChange != null)
                    TimeChange(value);
            }
        }
        /// <summary>
        /// How long the entire sequence is.
        /// </summary>
        public int Length {
            get { return mEvents.Length; }
        }
        /// <summary>
        /// Where the camera should start at the beginning of the sequence.
        /// </summary>
        public Camera Start {
            get { return mEvents.Start; }
            set { mEvents.Start = value; }
        }
        public bool Paused {
            get { return !mPlaying; }
            set {
                if (value)
                    mPlaying = false;
                else
                    Play();
            }
        }
        /// <summary>
        /// If true then, whilst playing, whenever one event finishes in the main sequence the next will start to play.
        /// If false then playback will stop after each event and a new call to Play will be required to start it up.
        /// </summary>
        public bool AutoStep {
            get { return mAutoStep; }
            set { mAutoStep = value; }
        }
        /// <summary>
        /// If true then once the sequence reaches the end it will start again.
        /// </summary>
        public bool Loop {
            get { return mLoop; }
            set { mLoop = value; }
        }

        public Flythrough() {
            Start = new Camera(new Vector3(128f, 128f, 60f), new Rotation());
            mEvents.Start = Start;
            mEvents.CurrentEventChange += new Action<FlythroughEvent<Camera>,FlythroughEvent<Camera>>(mEvents_CurrentEventChange);
            mEvents.LengthChange += new Action<EventSequence<Camera>,int>(mEvents_LengthChange);
        }

        public void Play() {
            if (mPlaying)
                return;

            Thread t = new Thread(PlayThread);
            t.Name = "Flythrough Playback Thread";
            t.Start();
        }

        internal void AddEvent(FlythroughEvent<Camera> evt) {
            mEvents.AddEvent(evt);
            if (mEvents.Count == 1)
                evt.StartValue = Start;
        }

        internal void RemoveEvent(ComboEvent evt) {
            mEvents.RemoveEvent(evt);
            mEvents[0].StartValue = Start;
        }

        public void MoveUp(ComboEvent evt) {
            mEvents.MoveUp(evt);
            if (evt.SequenceStartTime == 0)
                evt.StartValue = Start;
        }

        /// <summary>
        /// Initialise the flythrough from an xml file.
        /// </summary>
        /// <param name="file">The file to load as a flythrough.</param>
        public void Load(string file) {
            if (!File.Exists(file)) {
                Console.WriteLine("Unable to load " + file + ". Ignoring load request.");
                return;
            }
            mEvents = new EventSequence<Camera>();
            XmlDocument doc = new XmlDocument();
            doc.Load(file);
            int start = 0;
            XmlNode root = doc.GetElementsByTagName("Events")[0];
            
            XmlAttribute startPositionAttr = root.Attributes["StartPosition"];
            XmlAttribute startPitchAttr = root.Attributes["StartPitch"];
            XmlAttribute startYawAttr = root.Attributes["StartYaw"];
            Vector3 startPos = mCoordinator.Position;
            double startPitch = mCoordinator.Orientation.Pitch;
            double startYaw = mCoordinator.Orientation.Yaw;
            if (startPositionAttr != null) Vector3.TryParse(startPositionAttr.Value, out startPos);
            if (startPitchAttr != null) double.TryParse(startPitchAttr.Value, out startPitch);
            if (startYawAttr != null) double.TryParse(startYawAttr.Value, out startYaw);
            Start = new Camera(startPos, new Rotation(startPitch, startYaw));

            foreach (XmlNode node in root.ChildNodes) {
                ComboEvent evt = new ComboEvent(this);
                evt.Load(node);
                mEvents.AddEvent(evt);
                start = evt.SequenceStartTime + evt.Length;
            }
        }

        /// <summary>
        /// Save the flythrough as an XML file.
        /// </summary>
        /// <param name="file">The file to save the XML serialization to.</param>
        public void Save(string file) {
            XmlDocument doc = new XmlDocument();
            XmlNode root = doc.CreateElement("Events");

            XmlAttribute startPositionAttr = doc.CreateAttribute("StartPosition");
            XmlAttribute startPitchAttr = doc.CreateAttribute("StartPitch");
            XmlAttribute startYawAttr = doc.CreateAttribute("StartYaw");
            startPositionAttr.Value = Start.Position.ToString();
            startPitchAttr.Value = Start.Orientation.Pitch.ToString();
            startYawAttr.Value = Start.Orientation.Yaw.ToString();
            root.Attributes.Append(startPositionAttr);
            root.Attributes.Append(startPitchAttr);
            root.Attributes.Append(startYawAttr);

            foreach (var evt in mEvents) {
                root.AppendChild(evt.Save(doc));
            }

            doc.AppendChild(root);
            doc.Save(file);
        }

        private void mEvents_CurrentEventChange(FlythroughEvent<Camera> oldEvent, FlythroughEvent<Camera> newEvent) {
            if (!mAutoStep) {
                mPlaying = false;
                if (SequenceFinished != null)
                    SequenceFinished(this, null);
            }
        }

        private void mEvents_LengthChange(EventSequence<Camera> sequence, int length) {
            if (LengthChange != null)
                LengthChange(length);
        }

        private void PlayThread() {
            mPlaying = true;

            if (mEnabled) {
                Camera n = mEvents.CurrentEvent.Value;
                mCoordinator.Update(n.Position, Vector3.Zero, n.Orientation, new Rotation());
            }

            while (mPlaying) {
                Thread.Sleep(mCoordinator.TickLength);
                if (mPlaying) {
                    if (mEvents.Time + mCoordinator.TickLength < mEvents.Length)
                        DoTick(mEvents.Time + mCoordinator.TickLength, mEvents.CurrentEvent.Value);
                    else {
                        if (mLoop)
                            DoTick(0, mEvents.Start);
                        else {
                            DoTick(mEvents.Length, mEvents.CurrentEvent.Value);
                            mPlaying = false;
                        }

                        if (SequenceFinished != null)
                            SequenceFinished(this, null);
                    }
                }
            }
         }

        private void DoTick(int time, Camera o) {
            mEvents.Time = time;
            Camera n = mEvents.CurrentEvent.Value;
            if (mEnabled && mPlaying)
                mCoordinator.Update(n.Position, n.Position - o.Position, n.Orientation, n.Orientation - o.Orientation);
            if (TimeChange != null)
                TimeChange(time);
        }

        #region IInput Members

        public UserControl ControlPanel {
            get {
                if (mPanel == null)
                    mPanel = new FlythroughPanel(this);
                return mPanel;
            }
        }

        public string Name {
            get { return "Flythrough"; }
        }

        public bool Enabled {
            get { return mEnabled; }
            set { mEnabled = value; }
        }

        public string[] ConfigSwitches {
            get { throw new NotImplementedException(); }
        }

        public string State {
            get { return "-Flythrough-" + Environment.NewLine; }
        }

        public Coordinator Coordinator {
            get { return mCoordinator; }
        }

        public void Init(Coordinator coordinator) {
            mCoordinator = coordinator;
        }

        public void Close() { }

        public void Draw(Perspective perspective, System.Drawing.Graphics graphics) {
            throw new NotImplementedException();
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilLib;
using OpenMetaverse;
using System.Threading;

namespace FlythroughLib {
    public class FlythroughManager {
        /// <summary>
        /// The length of a tick.
        /// </summary>
        internal static readonly int TICK_LENGTH = 20;

        /// <summary>
        /// The events that make up the sequence.
        /// </summary>
        private FlythroughLib.FlythroughEvent[] mEvents;
        /// <summary>
        /// The event currently playing.
        /// </summary>
        private FlythroughEvent mCurrentEvent;
        /// <summary>
        /// Whether the flythrough is currently playing
        /// </summary>
        private bool mPlaying;
        /// <summary>
        /// The current position of the flythrough.
        /// </summary>
        private Vector3 mPosition;
        /// <summary>
        /// The current orientation of the flythrough.
        /// </summary>
        private Rotation mRotation;
        /// <summary>
        /// The index of the currently playing event.
        /// </summary>
        private int mCurrentEventIndex;
        /// <summary>
        /// Triggered whenever the rotation changes.
        /// </summary>
        public event EventHandler OnRotationChange;

        /// <summary>
        /// Triggered whenever the rotation changes.
        /// </summary>
        public event EventHandler OnPositionChange;

        /// <summary>
        /// Triggered whenever the flythrough completes.
        /// </summary>
        public event EventHandler OnComplete;

        /// <summary>
        /// Triggered whenever the flythrough starts.
        /// </summary>
        public event EventHandler OnStart;
        /// <summary>
        /// The current rotation dictated by the flythrough.
        /// </summary>
        public Rotation Rotation {
            get { return mRotation; }
        }

        /// <summary>
        /// The current rotation dictated by the flythrough.
        /// </summary>
        public Vector3 Position {
            get { return mPosition; }
        }

        /// <summary>
        /// Whether the flythrough is currently playing.
        /// </summary>
        public bool Playing {
            get { return mCurrentEvent != null && mCurrentEvent.Playing; }
        }

        /// <summary>
        /// The events which make up the flythrough.
        /// </summary>
        public FlythroughEvent[] Events {
            get { return mEvents; }
        }
        /// <summary>
        /// Start the flythrough running. Will continue on from wherever it was before.
        /// </summary>
        public void Play() {
            if (mEvents.Length == 0)
                return;

            if (mCurrentEvent == null) {
                mCurrentEventIndex = 0;
                mCurrentEvent = mEvents[mCurrentEventIndex];
            }
            Start();
        }

        /// <summary>
        /// Reset the flythrough so if started it will start from the beginning.
        /// </summary>
        public void Reset() {
            mPlaying = false;
            mCurrentEvent = null;
        }

        /// <summary>
        /// Pause the playthrough as its running.
        /// </summary>
        public void Pause() {
            mPlaying = false;
        }

        /// <summary>
        /// Go to a specific time through the playthrough.
        /// </summary>
        /// <param name="time">The time to jump to in ms.</param>
        public void JumpTo(int time) {
            lock (this) {
                int total = 0;
                for (int i = 0; total < time && i < mEvents.Length; i++) {
                    mCurrentEvent = mEvents[i];
                    total += mCurrentEvent.Length;
                }
                mCurrentEvent.SetTime(time - (total - mCurrentEvent.Length));
            }
        }

        /// <summary>
        /// Initialise the flythrough from an xml file.
        /// </summary>
        /// <param name="file">The file to load as a flythrough.</param>
        public void Load(string file) {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Body of the timer loop.
        /// </summary>
        private void TimerMethod() {
            while (mPlaying && mCurrentEvent != null) {
                lock (this) {
                    if (!mCurrentEvent.Step()) {
                        if (mEvents.Length < mCurrentEventIndex + 1) {
                            mPlaying = false;
                            mCurrentEvent = null;
                            OnComplete(this, null);
                        } else {
                            mCurrentEvent = mEvents[++mCurrentEventIndex];
                        }
                    }
                }
                Thread.Sleep(TICK_LENGTH);
            }
        }

        /// <summary>
        /// Set the rotation for the event, via a new Rotation object.
        /// </summary>
        /// <param name="rotation">The rotation to set.</param>
        internal void SetRotation(Rotation rotation) {
            mRotation = rotation;
        }

        /// <summary>
        /// Set the rotation for the event, via pitch and yaw.
        /// </summary>
        /// <param name="pitch">The pitch to set.</param>
        /// <param name="yaw">The yaw to set.</param>
        internal void SetRotation(float pitch, float yaw) {
            mRotation.Pitch = pitch;
            mRotation.Yaw = yaw;
        }

        /// <summary>
        /// Set the rotation for the event, via a quaternion.
        /// </summary>
        /// <param name="rotation">The quaternion to set rotation equal to.</param>
        internal void SetRotation(Quaternion rotation) {
            mRotation.Quaternion = rotation;
        }

        /// <summary>
        /// Set the rotation for the event.
        /// </summary>
        /// <param name="rotation">The rotation to set.</param>
        internal void SetPosition(Vector3 position) {
            mPosition = position;
            if (OnPositionChange != null)
                OnPositionChange(this, null);
        }

        /// <summary>
        /// Start the flythrough loop running.
        /// </summary>
        private void Start() {
            if (mPlaying)
                return;

            mPlaying = true;
            Thread t = new Thread(TimerMethod);
            t.Name = "FLythroughTimer";
            t.Start();
        }
    }
}

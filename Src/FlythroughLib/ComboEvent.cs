using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlythroughLib {
    public class ComboEvent : FlythroughEvent {
        /// <summary>
        /// The event currently playing.
        /// </summary>
        private FlythroughEvent mStream1Current;

        /// <summary>
        /// The first event in the sequence.
        /// </summary>
        private FlythroughEvent mStream1First;

        /// <summary>
        /// The last event in the sequence.
        /// </summary>
        private FlythroughEvent mStream1Last;

        /// <summary>
        /// The event currently playing.
        /// </summary>
        private FlythroughEvent mStream2Current;

        /// <summary>
        /// The first event in the sequence.
        /// </summary>
        private FlythroughEvent mStream2First;

        /// <summary>
        /// The last event in the sequence.
        /// </summary>
        private FlythroughEvent mStream2Last;

        /// <summary>
        /// True if stream 1 is currently playing.
        /// </summary>
        private bool mStream1Playing;
        /// <summary>
        /// True if stream 2 is currently playing.
        /// </summary>
        private bool mStream2Playing;

        /// <summary>
        /// Add a new event to the sequence.
        /// </summary>
        /// <param name="evt">The event to add.</param>
        public void AddStream1Event(FlythroughEvent evt) {
            if (mStream1First == null)
                mStream1First = evt;
            else
                mStream1Last.NextEvent = evt;

            mStream1Last = evt;
        }

        /// <summary>
        /// Add a new event to the sequence.
        /// </summary>
        /// <param name="evt">The event to add.</param>
        public void AddStream2Event(FlythroughEvent evt) {
            if (mStream2First == null)
                mStream2First = evt;
            else
                mStream2Last.NextEvent = evt;

            mStream2Last = evt;
        }

        public override int Length {
            get {
                int stream1L = 0, stream2L = 0;
                FlythroughEvent s1 = mStream1First, s2 = mStream2First;
                while (s1 != null) { stream1L = s1.Length; s1 = s1.NextEvent; }
                while (s2 != null) { stream2L = s2.Length; s2 = s2.NextEvent; }
                return Math.Max(stream1L, stream2L);
            }
            set {
                base.Length = value;
            }
        }

        public override bool Step() {
            if (CurrentStep == 0) {
                mStream1Playing = mStream1First != null;
                mStream2Playing = mStream2First != null;
                mStream1Current = mStream1First;
                mStream2Current = mStream2First;

                DoStep();
            }

            if (mStream1Playing && !mStream1Current.Step()) {
                mStream1Current = mStream1Current.NextEvent;
                if (mStream1Current == null) {
                    mStream1Playing = false;
                    mStream1Current = null;
                }
            }

            if (mStream2Playing && !mStream2Current.Step()) {
                mStream2Current = mStream2Current.NextEvent;
                if (mStream2Current == null) {
                    mStream2Playing = false;
                    mStream2Current = null;
                }
            }

            if (!mStream1Playing && !mStream2Playing) {
                DoStep();
                return false;
            }

            return true;
        }

        protected override void LengthChanged() {
            //Do nothing
        }

        /// <summary>
        /// Initialise with no events in the streams.
        /// </summary>
        /// <param name="container">The container this event is part of.</param>
        public ComboEvent(FlythroughManager container)
            : base(container, (FlythroughManager.TICK_LENGTH * 2) + 1) {
        }
    }
}

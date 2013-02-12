using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using UtilLib;

namespace FlythroughLib {
    public abstract class FlythroughEvent {
        /// <summary>
        /// The flythroughmanager which contains all the events.
        /// </summary>
        private readonly FlythroughManager mContainer;
        /// <summary>
        /// The event that this event is part of. May be null.
        /// </summary>
        private readonly FlythroughEvent mParentEvent;
        /// <summary>
        /// The next event in the sequence. May be null.
        /// </summary>
        private readonly FlythroughEvent mNextEvent;
        /// <summary>
        /// The previous event in the sequence. May be null.
        /// </summary>
        private readonly FlythroughEvent mPrevEvent;
        /// <summary>
        /// The current step being processed.
        /// </summary>
        private int mCurrentStep = 0;
        /// <summary>
        /// The total number of steps in the event.
        /// </summary>
        private int mSteps = 1;
        /// <summary>
        /// How long the event will last.
        /// </summary>
        private int mLength;

        /// <summary>
        /// Triggered whenever the event completes.
        /// </summary>
        public event EventHandler OnComplete;

        /// <summary>
        /// Triggered whenever the event starts.
        /// </summary>
        public event EventHandler OnStart;

        /// <summary>
        /// Initialise this event with no parent, next or previous events.
        /// </summary>
        /// <param name="container">The container which manages all events.</param>
        /// <param name="length">The length of time the event will run (ms).</param>
        public FlythroughEvent(FlythroughManager container, int length) {
            mContainer = container;
            Length = length;
        }

        /// <summary>
        /// Initialise this event with a parent event. No next or previous events.
        /// </summary>
        /// <param name="container">The container which manages all events.</param>
        /// <param name="length">The length of time the event will run (ms).</param>
        /// <param name="parent">The event this event is part of.</param>
        public FlythroughEvent(FlythroughManager container, int length, FlythroughEvent parent)
            : this(container, length) {
            mParentEvent = parent;
        }

        /// <summary>
        /// Initialise this event with a previous and a next event. No parent.
        /// </summary>
        /// <param name="container">The container which manages all events.</param>
        /// <param name="length">The length of time the event will run (ms).</param>
        /// <param name="prev">The previous event in the sequence. May be null.</param>
        /// <param name="next">The next event in the sequence. May be null.</param>
        public FlythroughEvent(FlythroughManager container, int length, FlythroughEvent prev, FlythroughEvent next)
            : this(container, length) {
            mPrevEvent = prev;
            mNextEvent = next;
        }

        /// <summary>
        /// Initialise this event with a parent event and previous and next events.
        /// </summary>
        /// <param name="container">The container which manages all events.</param>
        /// <param name="length">The length of time the event will run (ms).</param>
        /// <param name="parent">The event this event is part of.</param>
        /// <param name="prev">The previous event in the sequence. May be null.</param>
        /// <param name="next">The next event in the sequence. May be null.</param>
        public FlythroughEvent(FlythroughManager container, int length, FlythroughEvent parent, FlythroughEvent prev, FlythroughEvent next)
            : this(container, length, prev, next) {
            mParentEvent = parent;
        }

        /// <summary>
        /// How long the event will run for.
        /// </summary>
        public int Length {
            get { return mLength; }
            set {
                if (mCurrentStep != 0)
                    throw new Exception("Unable to set length. Event is currently running.");
                mLength = value;
                if (value == 0)
                    mSteps = 1;
                else
                    mSteps = value / FlythroughManager.TICK_LENGTH;
            }
        }

        /// <summary>
        /// How many steps it will take to complete the event.
        /// </summary> public int Steps } 
        /// </summary>
        public int CurrentStep {
            get { return mCurrentStep; }
        }

        /// <summary>
        /// The event this event is part of. May be null.
        /// </summary>
        public FlythroughEvent ParentEvent {
            get { return mParentEvent; }
        }

        /// <summary>
        /// The next event in the sequence. May be null.
        /// </summary>
        public FlythroughEvent NextEvent {
            get { return mNextEvent; }
        }

        /// <summary>
        /// The previous event in the sequence. May be null.
        /// </summary>
        private FlythroughEvent PrevEvent {
            get { return mPrevEvent; }
        }

        /// <summary>
        /// The container which manages all the events.
        /// </summary>
        public FlythroughManager Container {
            get { return mContainer; }
        }

        /// <summary>
        /// How many steps the event will take.
        /// </summary>
        public int TotalSteps {
            get { return mSteps; }
        }

        /// <summary>
        /// Trigger the OnStart event.
        /// </summary>
        private void TriggerOnStart() {
            if (OnStart != null)
                OnStart(this, null);
        }

        /// <summary>
        /// Trigger the OnComplete event.
        /// </summary>
        private void TriggerOnComplete() {
            if (OnComplete != null)
                OnComplete(this, null);
        }

        /// <summary>
        /// Trigger the next step to happen. Returns true if there is another event to come. False otherwise.
        /// </summary>
        public virtual bool Step() {
            mCurrentStep++;
            return mCurrentStep < mSteps;
        }

        /// <summary>
        /// Set how far through the event playback has reached.
        /// </summary>
        /// <param name="time">The time through the event to play from.</param>
        public void SetTime(int time) {
            if (time < mLength)
                mCurrentStep = time / FlythroughManager.TICK_LENGTH;
            else
                mCurrentStep = mSteps;
        }
    }
}

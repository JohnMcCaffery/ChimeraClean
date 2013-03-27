using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using UtilLib;
using System.Xml;

namespace Chimera.Flythrough {
    public abstract class FlythroughEvent {
        /// <summary>
        /// The flythroughmanager which contains all the events.
        /// </summary>
        private readonly FlythroughManager mContainer;
        /// <summary>
        /// The event that this event is part of. May be null.
        /// </summary>
        private FlythroughEvent mParentEvent;
        /// <summary>
        /// The next event in the sequence. May be null.
        /// </summary>
        private FlythroughEvent mNextEvent;
        /// <summary>
        /// The previous event in the sequence. May be null.
        /// </summary>
        private FlythroughEvent mPrevEvent;
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
        /// Selected whenever the event completes.
        /// </summary>
        public event EventHandler OnComplete;

        /// <summary>
        /// Selected whenever the event starts.
        /// </summary>
        public event EventHandler OnStart;

        /// <summary>
        /// Selected every time the next step is triggered.
        /// </summary>
        public event EventHandler OnStep;

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
        public virtual int Length {
            get { return mLength; }
            set {
                if (mCurrentStep != 0)
                    throw new Exception("Unable to set length. Event is currently running.");
                mLength = value;
                if (value < FlythroughManager.TICK_LENGTH)
                    mSteps = 1;
                else
                    mSteps = value / FlythroughManager.TICK_LENGTH;
                LengthChanged();
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
            set { mParentEvent = value; }
        }

        /// <summary>
        /// The next event in the sequence. May be null.
        /// </summary>
        public FlythroughEvent NextEvent {
            get { return mNextEvent; }
            set { mNextEvent = value; }
        }

        /// <summary>
        /// The previous event in the sequence. May be null.
        /// </summary>
        public FlythroughEvent PrevEvent {
            get { return mPrevEvent; }
            set { mPrevEvent = value; }
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
        /// Unique name for the event.
        /// </summary>
        public abstract string Name {
            get;
        }

        /// <summary>
        /// CustomTrigger the OnStart event.
        /// </summary>
        private void TriggerOnStart() {
            if (OnStart != null)
                OnStart(this, null);
        }

        /// <summary>
        /// CustomTrigger the OnComplete event.
        /// </summary>
        private void TriggerOnComplete() {
            if (OnComplete != null)
                OnComplete(this, null);
        }

        /// <summary>
        /// CustomTrigger the next step to happen. Returns true if there is another event to come. False otherwise.
        /// </summary>
        public abstract bool Step();

        /// <summary>
        /// Called whenever length (and therefore # steps) changes.
        /// </summary>
        protected abstract void LengthChanged();


        /// <summary>
        /// Move the step counter forward and calculate whether the event is finished.
        /// </summary>
        protected bool DoStep() {
            mCurrentStep++;
            bool running = mCurrentStep < mSteps;
            if (OnStep != null)
                OnStep(this, null);
            if (!running)
                mCurrentStep = 0;
            return running;
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

        /// <summary>
        /// Reset the step counter.
        /// </summary>
        public virtual void Reset() {
            mCurrentStep = 0;
        }

        /// <summary>
        /// Load parameters from an XML node.
        /// </summary>
        /// <param name="node">The noad to load parameters from.</param>
        public abstract void Load(XmlNode node);

        /// <summary>
        /// Save the window of the event to an XML node which can later be written to file.
        /// </summary>
        /// <param name="doc">The document that the node is to be part of.</param>
        /// <returns>An XML node storing all relevant information about the event.</returns>
        public abstract XmlNode Save(XmlDocument doc);
    }
}

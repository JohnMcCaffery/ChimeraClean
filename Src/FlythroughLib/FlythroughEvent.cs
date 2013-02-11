using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using UtilLib;

namespace FlythroughLib {
    public abstract class FlythroughEvent {
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
        private int mCurrentStep;
        /// <summary>
        /// The total number of steps in the event.
        /// </summary>
        private int mSteps;
        /// <summary>
        /// The flythroughmanager which contains all the events.
        /// </summary>
        private FlythroughManager mContainer;
        /// <summary>
        /// How long the event will last.
        /// </summary>
        private int mLength;

        /// <summary>
        /// Whether the event is currently playing
        /// </summary>
        private bool mPlaying;
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
        public FlythroughEvent(FlythroughManager container) {
            mContainer = container;
        }

        /// <summary>
        /// Initialise this event with a parent event. No next or previous events.
        /// </summary>
        /// <param name="container">The container which manages all events.</param>
        /// <param name="parent">The event this event is part of.</param>
        public FlythroughEvent(FlythroughManager container, FlythroughEvent parent) {
            mContainer = container;
            mParentEvent = parent;
        }

        /// <summary>
        /// Initialise this event with a previous and a next event. No parent.
        /// </summary>
        /// <param name="container">The container which manages all events.</param>
        /// <param name="prev">The previous event in the sequence. May be null.</param>
        /// <param name="next">The next event in the sequence. May be null.</param>
        public FlythroughEvent(FlythroughManager container, FlythroughEvent prev, FlythroughEvent next) {
            mContainer = container;
            mPrevEvent = prev;
            mNextEvent = next;
        }

        /// <param name="container">The container which manages all events.</param>
        /// <param name="parent">The event this event is part of.</param>
        /// <param name="prev">The previous event in the sequence. May be null.</param>
        /// <param name="next">The next event in the sequence. May be null.</param>
        public FlythroughEvent(FlythroughManager container, FlythroughEvent parent, FlythroughEvent prev, FlythroughEvent next) {
            mContainer = container;
            mParentEvent = parent;
            mPrevEvent = prev;
            mNextEvent = next;
        }
        /// <summary>
        /// The name of the event. Not unique.
        /// </summary> public string Name } 
        /// </summary>
        public int Length {
            get { return mLength; }
        }

        /// <summary>
        /// How many steps it will take to complete the event.
        /// </summary> public int Steps } 
        /// </summary>
        public int CurrentStep {
            get { return mCurrentStep; }
        }

        /// <summary>
        /// How far through the event currently is.
        /// </summary> public int CurrentTime } 
        /// </summary>
        public bool Playing {
            get { return mPlaying; }
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
            get {
                throw new System.NotImplementedException();
            }
            set {
            }
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
        /// Trigger the OnComplete event.
        /// </summary>
        private void TriggerOnComplete() {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Trigger the OnStart event.
        /// </summary>
        private void TriggerOnStart() {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Start the event.
        /// </summary>
        public void Play() {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Pause the event.
        /// </summary>
        public void Pause() {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Trigger the next step to happen. Returns true if there is another event to come. False otherwise.
        /// </summary>
        public abstract bool Step() {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Set how far through the event playback has reached.
        /// </summary>
        /// <param name="time">The time through the event to play from.</param>
        public void SetTime(int time) {
            throw new System.NotImplementedException();
        }
    }
}

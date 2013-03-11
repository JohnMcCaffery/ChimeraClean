using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using System.Xml;
using System.Threading;

namespace Chimera.FlythroughLib {
    public abstract class FlythroughEvent<T> : IComparable<FlythroughEvent<T>> {
        /// <summary>
        /// The flythrough this event is part of.
        /// </summary>
        private readonly Flythrough mFlythrough;
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
        /// When this event starts, used for sorting the sequence of events.
        /// </summary>
        private int mStartTime;
        /// <summary>
        /// The name this event is known by.
        /// </summary>
        private string mName;
        
        /// <summary>
        /// Triggered every time the next step is triggered.
        /// </summary>
        public event EventHandler TimeChange;
        /// <summary>
        /// Triggered every time the length of the event is changed.
        /// </summary>
        public event EventHandler LengthChange;
        /// <summary>
        /// Triggered whenever the position of the camera when the event finishes changes.
        /// </summary>
        public event Action<FlythroughEvent<T>, T> FinishChange;

        /// <summary>
        /// Initialise this event with no parent, next or previous events.
        /// </summary>
        /// <param name="flythrough">The flythrough this event is part of.</param>
        /// 
        /// <param name="length">The length of time the event will run (ms).</param>
        public FlythroughEvent(Flythrough flythrough, int length) {
            mFlythrough = flythrough;
            mStartTime = 0;
            Length = length;
        }

        /// <summary>
        /// When this event starts, used for sorting the sequence of events.
        /// </summary>
        public virtual int StartTime {
            get { return mStartTime; }
            set { mStartTime = value; }
        }

        /// <summary>
        /// How far through the event playback has got. Should be between 0 and Length.
        /// </summary>
        public int Time {
            get { return mCurrentStep * mFlythrough.Coordinator.TickLength; }
            set {
                if (value > Length || value < 0)
                    throw new ArgumentException("Time must be between 0 and Length.");
                CurrentStep = value / mFlythrough.Coordinator.TickLength;
                TimeChanged(value);
            }
        }

        /// <summary>
        /// How long the event will run for.
        /// </summary>
        public virtual int Length {
            get { return mLength; }
            set {
                mLength = value;
                if (value < mFlythrough.Coordinator.TickLength)
                    mSteps = 1;
                else
                    mSteps = value / mFlythrough.Coordinator.TickLength;
                if (LengthChange != null)
                    LengthChange(this, null);
            }
        }

        /// <summary>
        /// How many steps it will take to complete the event.
        /// </summary> public int Steps } 
        /// </summary>
        public int CurrentStep {
            get { return mCurrentStep; }
            set {
                if (value != mCurrentStep) {
                    mCurrentStep = value;
                    if (TimeChange != null)
                        TimeChange(this, null);
                }
            }
        }

        /// <summary>
        /// How many steps the event will take.
        /// </summary>
        public int TotalSteps {
            get { return mSteps; }
        }

        /// <summary>
        /// The container which manages all the events.
        /// </summary>
        public Flythrough Container {
            get { return mFlythrough; }
        }

        /// <summary>
        /// Unique name for the event.
        /// </summary>
        public string Name {
            get { return mName; }
            set { mName = value; }
        }

        /// <summary>
        /// Where the camera is at the end of the sequence.
        /// </summary>
        public abstract T Finish {
            get;
            set;
        }

        /// <summary>
        /// What the camera is doing at the start of the sequence.
        /// </summary>
        public abstract T Start {
            get;
            set;
        }

        /// <summary>
        /// The value this event is currently at.
        /// </summary>
        public abstract T Value {
            get;
        }

        /// <summary>
        /// Called whenver the time is updated for this event.
        /// CurrentEvent will already have been updated before this is called.
        /// </summary>
        /// <param name="time"></param>
        public abstract void TimeChanged(int time);

        /// <summary>
        /// Save this event as an XML node from the specified document.
        /// </summary>
        /// <param name="doc">The document the new node should be part of.</param>
        /// <returns>A node which can be used to instantiate a new instance of this event.</returns>
        public abstract XmlNode Save(XmlDocument doc);

        /// <summary>
        /// Instantiate an instance of this event from an XML node.
        /// </summary>
        /// <param name="doc">The node to load details from.</param>
        public abstract void Load(XmlNode node);

        #region IComparable<FlythroughEvent> Members

        public int CompareTo(FlythroughEvent<T> other) {
            if (other == null)
                return 1;
            return StartTime.CompareTo(other.StartTime);
        }

        protected void TriggerFinishChange(T finish) {
            if (FinishChange != null)
                FinishChange(this, finish);
        }

        #endregion
    }
}

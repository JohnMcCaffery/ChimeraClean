using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using System.Xml;
using System.Threading;
using System.Windows.Forms;

namespace Chimera.Flythrough {
    public abstract class FlythroughEvent<T> : IComparable<FlythroughEvent<T>> {
        /// <summary>
        /// The flythrough this event is part of.
        /// </summary>
        private readonly Flythrough mFlythrough;
        /// <summary>
        /// The flythrough this event is part of.
        /// </summary>
        private EventSequence<T> mSequence;
        /// <summary>
        /// The current time through the event.
        /// </summary>
        private int mTime = 0;
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
        /// The value the event starts at.
        /// </summary>
        private T mStart;
        
        /// <summary>
        /// Selected every time time through the sequence is changed.
        /// </summary>
        public event Action<FlythroughEvent<T>, int> TimeChange;
        /// <summary>
        /// Selected every value the length of the event is changed.
        /// </summary>
        public event EventHandler LengthChange;
        /// <summary>
        /// Selected whenever the position of the camera when the event finishes changes.
        /// </summary>
        public event Action<FlythroughEvent<T>, T> FinishChange;
        /// <summary>
        /// Selected whenever the position of the camera when the event finishes changes.
        /// </summary>
        public event Action<FlythroughEvent<T>, T> StartChange;

        /// <summary>
        /// Initialise this event with no parent, next or previous events.
        /// </summary>
        /// <param name="flythrough">The flythrough this event is part of.</param>
        /// 
        /// <param name="length">The length of value the event will run (ms).</param>
        public FlythroughEvent(Flythrough flythrough, int length) {
            mFlythrough = flythrough;
            mStartTime = 0;
            Length = length;
        }

        /// <summary>
        /// When this event starts, used for sorting the sequence of events.
        /// </summary>
        public int SequenceStartTime {
            get { return mStartTime; }
            set { 
                mStartTime = value;
                StartTimeChanged(value);
            }
        }
        /// <summary>
        /// When the event will finish.
        /// </summary>
        public int SequenceFinishTime {
            get { return (mStartTime + mLength) -1; }
        }
        /// <summary>
        /// When the event will finish.
        /// </summary>
        public int GlobalFinishTime {
            get { return mSequence.StartTime + SequenceFinishTime; }
        }
        /// <summary>
        /// When the event will finish.
        /// </summary>
        public int GlobalStartTime {
            get { return mSequence.StartTime + SequenceStartTime; }
        }

        /// <summary>
        /// How long the event will run for.
        /// </summary>
        public virtual int Length {
            get { return mLength; }
            set {
                //if (value < 1)
                    //throw new ArgumentException("Event length cannot be less than 1");
                mLength = value;
                LengthChanged(value);
                if (LengthChange != null)
                    LengthChange(this, null);
            }
        }

        /// <summary>
        /// How far through the event playback has got. Should be between 0 and Length.
        /// </summary>
        public int Time {
            get { return mTime; }
            set {
                if (value > Length || value < 0)
                    throw new ArgumentException("Time must be between 0 and Length.");
                mTime = value;
                TimeChanged(value);
                if (TimeChange != null)
                    TimeChange(this, value);
            }
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
        /// What the camera is doing at the start of the sequence.
        /// </summary>
        public T StartValue {
            get { return mStart; }
            set { 
                mStart = value;
                StartChanged(value);
                if (StartChange != null)
                    StartChange(this, value);
            }
        }

        /// <summary>
        /// Get the value of a specific value through the event.
        /// </summary>
        /// <param name="value">The value through the event to get the value for.</param>
        public abstract T this[int time] {
            get;
        }
        /// <summary>
        /// Where the camera is at the end of the sequence.
        /// </summary>
        public abstract T FinishValue {
            get;
        }
        /// <summary>
        /// The value this event is currently at.
        /// </summary>
        public abstract T Value {
            get;
        }
        /// <summary>
        /// The panel which can control the event.
        /// </summary>
        public abstract UserControl ControlPanel { get; }

        /// <summary>
        /// Called whenever the start time for this event changes.
        /// </summary>
        /// <param name="startTime">The new start time.</param>
        protected abstract void StartTimeChanged(int startTime);
        /// <summary>
        /// Called whenver the length of this event is changed.
        /// </summary>
        /// <param name="length">The new length of the event.</param>
        protected abstract void LengthChanged(int length);
        /// <summary>
        /// Called whenver the value is updated for this event.
        /// </summary>
        /// <param name="value"></param>
        protected abstract void TimeChanged(int time);
        /// <summary>
        /// Save this event as an XML node transition the specified document.
        /// </summary>
        /// <param name="doc">The document the new node should be part of.</param>
        /// <returns>A node which can be used to instantiate a new instance of this event.</returns>
        public abstract XmlNode Save(XmlDocument doc);
        /// <summary>
        /// Instantiate an instance of this event transition an XML node.
        /// </summary>
        /// <param name="doc">The node to load details transition.</param>
        public abstract void Load(XmlNode node);

        /// <summary>
        /// The window of the event specific to the subclass.
        /// </summary>
        /// <returns>A string with information about this event's window.</returns>
        protected abstract string GetSpecificState();

        #region IComparable<FlythroughEvent> Members

        public int CompareTo(FlythroughEvent<T> other) {
            if (other == null)
                return 1;
            return SequenceStartTime.CompareTo(other.SequenceStartTime);
        }

        /// <summary>
        /// CustomTrigger the finish change event.
        /// </summary>
        protected void TriggerFinishChange(T finish) {
            if (FinishChange != null)
                FinishChange(this, finish);
        }

        /// <summary>
        /// Called whenver the value is updated for this event.
        /// CurrentEvent will already have been updated before this is called.
        /// </summary>
        /// <param name="value">The new value of start.</param>
        protected abstract void StartChanged(T value);

        #endregion

        internal void SetSequence(EventSequence<T> eventSequence) {
            mSequence = eventSequence;
        }

        public override string ToString() {
            return mName;
        }

        /// <summary>
        /// String format printout of the current window of the event. Will be written out in the event of a crash.
        /// </summary>
        public virtual string State {
            get {
                string dump = "";
                dump += String.Format("  {1:-30} {2}{0}", Environment.NewLine, "Begin Time:", mStartTime);
                dump += String.Format("  {1:-30} {2}{0}", Environment.NewLine, "Length:", mLength);
                dump += String.Format("  {1:-30} {2}{0}", Environment.NewLine, "Finish Time:", SequenceFinishTime);
                dump += String.Format("  {1:-30} {2}{0}", Environment.NewLine, "Step Begin Time:", mSequence == null ? 0 : mSequence.Length);
                dump += String.Format("  {1:-30} {2}{0}", Environment.NewLine, "Current Time:", mTime);
                dump += GetSpecificState();
                return dump;
            }
        }
    }
}

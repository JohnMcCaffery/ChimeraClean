using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Chimera.Flythrough {
    /// <summary>
    /// Collection of events. Can be iterated over to play the sequence.
    /// Iteration can involve the whole sequence of only events starting transition Begin.
    /// </summary>
    public class EventSequence<T> : IEnumerable<FlythroughEvent<T>> {
        /// <summary>
        /// All the events.
        /// </summary>
        private List<FlythroughEvent<T>> mEvents = new List<FlythroughEvent<T>>();
        /// <summary>
        /// The event for the current time. May be null if mEvents is empty.
        /// </summary>
        private FlythroughEvent<T> mCurrentEvent;
        /// <summary>
        /// The final event in the sequence. Helpful when adding new events.
        /// </summary>
        private FlythroughEvent<T> mLastEvent;
        /// <summary>
        /// Where the camera is at the start of the sequence.
        /// </summary>
        private T mStartValue;
        /// <summary>
        /// The time when the first event in the sequence should start.
        /// </summary>
        private int mStartTime;

        /// <summary>
        /// Selected every value the length of the event is changed.
        /// </summary>
        public event Action<EventSequence<T>, int> LengthChange;
        /// <summary>
        /// Selected whenever the current event changes.
        /// Supplies the old current event and the new one.
        /// </summary>
        public event Action<FlythroughEvent<T>, FlythroughEvent<T>> CurrentEventChange;
        /// <summary>
        /// Selected whenever the position of the camera when the sequence finishes changes.
        /// </summary>
        public event EventHandler FinishChange;

        /// <summary>
        /// Get the value of a specific value through the sequence.
        /// </summary>
        /// <param name="value">The value through the sequence to get the position for.</param>
        public FlythroughEvent<T> this[int time] {
            get {
                if (time < 0 || time > Length)
                    throw new ArgumentException("Unable to fetch event. Time must be between 0 and Length");
                if (mEvents.Count == 0)
                    return null;
                return time == Length ? mLastEvent : mEvents.First(evt => evt.SequenceFinishTime >= time);
            }
        }
        /// <summary>
        /// The first element to play transition. 
        /// If this is set to null or First then iterating through the sequence will start transition the beginning. Otherwise it will start transition this event.
        /// </summary>
        /// <returns></returns>
        public FlythroughEvent<T> CurrentEvent {
            get { return mCurrentEvent; }
            set {
                if (!mEvents.Contains(value))
                    throw new ArgumentException("Unable to set start to event that is not in the sequence.");
                if (mCurrentEvent != value) {
                    FlythroughEvent<T> old = mCurrentEvent;
                    mCurrentEvent = value;
                    mCurrentEvent.Time = 0;
                    if (CurrentEventChange != null)
                        CurrentEventChange(old, value);
                }
            }
        }
        /// <summary>
        /// How far through the sequence playback has reached.
        /// </summary>
        public int Time {
            get { return mCurrentEvent == null ? 0 : mCurrentEvent.SequenceStartTime + mCurrentEvent.Time; }
            set {
                if (value < 0 || value > Length)
                    throw new ArgumentException("Unable to set value. Value must be between 0 and Length");
                if (mEvents.Count == 0)
                    return;

                CurrentEvent = value == Length ? mLastEvent : mEvents.First(e => e.SequenceFinishTime >= value);
                mCurrentEvent.Time = value - mCurrentEvent.SequenceStartTime;
            }
        }
        /// <summary>
        /// How long the full sequence is, in ms.
        /// </summary>
        public int Length {
            get { return mEvents.Aggregate(0, (current, evt) => current + evt.Length); }
        }
        /// <summary>
        /// How many elemts there are in the full sequence.
        /// </summary>
        public int Count {
            get { return mEvents.Count; }
        }
        /// <summary>
        /// Where the camera is at the end of the sequence.
        /// </summary>
        public T FinishValue {
            get { return mEvents.Count == 0 ? mStartValue : mEvents[mEvents.Count-1].FinishValue; }
        }
        /// <summary>
        /// Where the camera is at the start of the sequence.
        /// </summary>
        public T Start {
            get { return mStartValue; }
            set {
                mStartValue = value;
                T start = value;
                foreach (var evt in mEvents) {
                    evt.StartValue = start;
                    start = evt.FinishValue;
                }
            }
        }

        /// <summary>
        /// The time when the first event in the sequence should start.
        /// </summary>
        public int StartTime {
            get { return mStartTime; }
            set { mStartTime = value; }
        }

        private int NextStart(FlythroughEvent<T> prev) {
            return prev == null ? 0 : prev.SequenceFinishTime + 1;
        }

        public virtual void AddEvent(FlythroughEvent<T> evt) {
            evt.SetSequence(this);
            evt.SequenceStartTime = NextStart(mLastEvent); ;
            evt.StartValue = mLastEvent == null ? mStartValue : mLastEvent.FinishValue;
            mLastEvent = evt;
            mEvents.Add(evt);

            //if (mCurrentEvent == null)
                //mCurrentEvent = evt;

            evt.LengthChange += (source, args) => {
                FlythroughEvent<T> prev = evt;
                foreach (var after in mEvents.Where(e => e.SequenceStartTime > evt.SequenceStartTime)) {
                    after.SequenceStartTime = NextStart(prev);
                    prev = after;
                }
                if (LengthChange != null)
                    LengthChange(this, Length);
            };
            evt.FinishChange += evt_FinishChange;

            if (LengthChange != null)
                LengthChange(this, Length);
        }

        public virtual void RemoveEvent(FlythroughEvent<T> evt) {
            int time = Time;
            mEvents.Remove(evt);
            //Shift all events back by the length of the removed event.
            foreach (var after in mEvents.Where(e => e.SequenceStartTime > evt.SequenceStartTime))
                after.SequenceStartTime -= evt.Length;

            mLastEvent = mEvents.Count > 0 ? mEvents[mEvents.Count - 1] : null;

            //Set value to take account the change to the chronology.
            Time = time < Length ? time : Length;
            evt.FinishChange -= evt_FinishChange;
            if (LengthChange != null)
                LengthChange(this, Length);
        }

        public void MoveUp(FlythroughEvent<T> evt) {
            if (!mEvents.Contains(evt))
                throw new ArgumentException("Unable to modify the position of an event that is not in the sequence.");

            FlythroughEvent<T> prev = null;
            foreach (var current in mEvents) {
                if (current == evt)
                    break;
                prev = current;
            }

            evt.SequenceStartTime = NextStart(prev);
            if (prev != null)
                prev.SequenceStartTime = NextStart(evt);

            if (evt == mLastEvent && prev != null)
                mLastEvent = prev;

            mEvents.Sort();

            T finish = evt.FinishValue;
            foreach (var e in mEvents.Where(e => e.SequenceStartTime > evt.SequenceStartTime)) {
                e.StartValue = finish;
                finish = e.FinishValue;
            }
        }

        private void evt_FinishChange(FlythroughEvent<T> modifiedEvent, T finish) {
            foreach (var evt in mEvents.Where(e => e.SequenceStartTime > modifiedEvent.SequenceStartTime)) {
                evt.StartValue = finish;
                finish = evt.FinishValue;
            }
            if (FinishChange != null)
                FinishChange(this, null);
        }

        public IEnumerator<FlythroughEvent<T>> GetEnumerator() {
            return mEvents.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return mEvents.GetEnumerator();
        }
    }
}

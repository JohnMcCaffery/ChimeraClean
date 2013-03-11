using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Chimera.FlythroughLib {
    /// <summary>
    /// Collection of events. Can be iterated over to play the sequence.
    /// Iteration can involve the whole sequence of only events starting from Start.
    /// </summary>
    public class EventSequence<T> {
        private List<FlythroughEvent<T>> mEvents = new List<FlythroughEvent<T>>();
        private FlythroughEvent<T> mCurrentEvent;
        private FlythroughEvent<T> mLastEvent;
        /// <summary>
        /// Where the camera is at the start of the sequence.
        /// </summary>
        private T mStart;

        /// <summary>
        /// Triggered every time the length of the event is changed.
        /// </summary>
        public event Action<EventSequence<T>, int> LengthChange;
        /// <summary>
        /// Triggered whenever the current event changes.
        /// Supplies the old current event and the new one.
        /// </summary>
        public event Action<FlythroughEvent<T>, FlythroughEvent<T>> CurrentEventChange;
        /// <summary>
        /// Triggered whenever the position of the camera when the sequence finishes changes.
        /// </summary>
        public event EventHandler FinishChange;

        /// <summary>
        /// Get the value of a specific time through the sequence.
        /// </summary>
        /// <param name="time">The time through the sequence to get the position for.</param>
        public T this[int time] {
            get {
                if (time < 0 || time > Length)
                    throw new ArgumentException("Unable to fetch event. Time must be between 0 and Length");
                if (mEvents.Count == 0)
                    return default(T);
                return mEvents.First(evt => evt.StartTime + evt.Length > time).Value;
            }
        }

        /// <summary>
        /// The first element to play from. 
        /// If this is set to null or First then iterating through the sequence will start from the beginning. Otherwise it will start from this event.
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
                    CurrentEventChange(old, value);
                }
            }
        }

        /// <summary>
        /// How far through the sequence playback has reached.
        /// </summary>
        public int Time {
            get { return mEvents.Count == 0 ? 0 : mCurrentEvent.StartTime + mCurrentEvent.Time; }
            set {
                if (value < 0 || value > Length)
                    throw new ArgumentException("Unable to set time. Value must be between 0 and Length");
                if (mEvents.Count == 0)
                    return;

                CurrentEvent = mEvents.First(e => e.StartTime + e.Length > value);
                mCurrentEvent.Time = value - mCurrentEvent.StartTime;
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
        /// All the events in the sequence.
        /// </summary>
        public FlythroughEvent<T>[] Events {
            get { return mEvents.ToArray(); }
        }

        /// <summary>
        /// Where the camera is at the end of the sequence.
        /// </summary>
        public T Finish {
            get { return mEvents.Count == 0 ? default(T) : mEvents[mEvents.Count-1].Finish; }
        }

        /// <summary>
        /// Where the camera is at the start of the sequence.
        /// </summary>
        public T Start {
            get { return mStart; }
            set {
                mStart = value;
                T start = value;
                foreach (var evt in Events) {
                    evt.Start = start;
                    start = evt.Finish;
                }
            }
        }

        public virtual void AddEvent(FlythroughEvent<T> evt) {
            evt.StartTime = mLastEvent == null ? 0 : mLastEvent.StartTime + mLastEvent.Length;
            mLastEvent = evt;
            mEvents.Add(evt);

            if (mCurrentEvent == null)
                mCurrentEvent = evt;

            evt.LengthChange += (source, args) => {
                int time = evt.StartTime + evt.Length;
                foreach (var after in mEvents.Where(e => e.StartTime > evt.StartTime)) {
                    after.StartTime = time;
                    time += after.Length;
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
            foreach (var after in mEvents.Where(e => e.StartTime > evt.StartTime))
                after.StartTime -= evt.Length;

            //Set time to take account the change to the chronology.
            Time = time < Length ? time : Length;
            evt.FinishChange -= evt_FinishChange;
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

            evt.StartTime = prev == null ? 0 : prev.StartTime;
            if (prev != null)
                prev.StartTime = evt.StartTime + evt.Length;

            if (evt == mLastEvent && prev != null)
                mLastEvent = prev;

            mEvents.Sort();
        }
        private void evt_FinishChange(FlythroughEvent<T> modifiedEvent, T finish) {
            foreach (var evt in Events.Where(e => e.StartTime > modifiedEvent.StartTime)) {
                modifiedEvent.Start = finish;
                finish = evt.Finish;
            }
            if (FinishChange != null)
                FinishChange(this, null);
        }
    }
}

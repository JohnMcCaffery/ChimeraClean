using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using UtilLib;

namespace FlythroughLib
{
    public abstract class FlythroughEvent
    {
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
        /// Triggered whenever the event completes.
        /// </summary>
        public event EventHandler OnComplete;

        /// <summary>
        /// Triggered whenever the event starts.
        /// </summary>
        public event EventHandler OnStart;

        /// <summary>
        /// Triggered whenever the rotation changes.
        /// </summary>
        public event EventHandler OnPositionChange;

        /// <summary>
        /// Triggered whenever the rotation changes.
        /// </summary>
        public event EventHandler OnRotationChange;

        /// <summary>
        /// Initialise this event with a parent event. No next or previous events.
        /// </summary>
        /// <param name="parentEvent">The event this event is part of.</param>
        public FlythroughEvent(FlythroughEvent parentEvent)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Initialise this event with no parent, next or previous events.
        /// </summary>
        public FlythroughEvent()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Initialise this event with a previous and a next event. No parent.
        /// </summary>
        /// <param name="prev">The previous event in the sequence. May be null.</param>
        /// <param name="next">The next event in the sequence. May be null.</param>
        public FlythroughEvent(FlythroughEvent prev, FlythroughEvent next)
        {
            throw new System.NotImplementedException();
        }

        /// <param name="parent">The event this event is part of.</param>
        /// <param name="prev">The previous event in the sequence. May be null.</param>
        /// <param name="next">The next event in the sequence. May be null.</param>
        public FlythroughEvent(FlythroughEvent parent, FlythroughEvent prev, FlythroughEvent next)
        {
            throw new System.NotImplementedException();
        }
        /// <summary>
        /// The name of the event. Not unique.
        /// </summary>
        public string Name
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        /// <summary>
        /// How long (ms) the event takes.
        /// </summary>
        public int Length
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        /// <summary>
        /// How many steps it will take to complete the event.
        /// </summary>
        public int Steps
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        /// <summary>
        /// The current step the event is on.
        /// </summary>
        public int CurrentStep
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        /// <summary>
        /// How far through the event currently is.
        /// </summary>
        public int CurrentTime
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        /// <summary>
        /// The current rotation dictated by the event.
        /// </summary>
        public Vector3 Position
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        /// <summary>
        /// The current rotation dictated by the event.
        /// </summary>
        public Rotation Rotation
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        /// <summary>
        /// Whether this event is currently playing.
        /// </summary>
        public bool Playing
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        /// <summary>
        /// The event this event is part of. May be null.
        /// </summary>
        public FlythroughEvent ParentEvent
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        /// <summary>
        /// The next event in the sequence. May be null.
        /// </summary>
        public FlythroughEvent NextEvent
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        /// <summary>
        /// The previous event in the sequence. May be null.
        /// </summary>
        private FlythroughEvent PrevEvent
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        /// <summary>
        /// Trigger the OnComplete event.
        /// </summary>
        private void TriggerOnComplete()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Trigger the OnStart event.
        /// </summary>
        private void TriggerOnStart()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Start the event.
        /// </summary>
        public void Play()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Pause the event.
        /// </summary>
        public void Pause()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Set the rotation for the event.
        /// </summary>
        /// <param name="rotation">The rotation to set.</param>
        private void SetPosition(Vector3 position)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Set the rotation for the event, via a new Rotation object.
        /// </summary>
        /// <param name="rotation">The rotation to set.</param>
        private void SetRotation(Random rotation)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Set the rotation for the event, via pitch and yaw.
        /// </summary>
        /// <param name="pitch">The pitch to set.</param>
        /// <param name="yaw">The yaw to set.</param>
        private void SetRotation(float pitch, float yaw)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Set the rotation for the event, via a quaternion.
        /// </summary>
        /// <param name="rotation">The quaternion to set rotation equal to.</param>
        private void SetRotation(Queryable rotation)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Trigger the next step to happen.
        /// </summary>
        public abstract void Step()
        {
            throw new System.NotImplementedException();
        }
    }
}

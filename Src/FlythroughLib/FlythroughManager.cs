using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlythroughLib
{
    public class FlythroughManager
    {
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
        /// The current rotation dictated by the flythrough.
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
        /// Whether the flythrough is currently playing.
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
        /// The events which make up the flythrough.
        /// </summary>
        public FlythroughEvent[] Events
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
        /// Start the flythrough running. Will continue on from wherever it was before.
        /// </summary>
        public void Play()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Reset the flythrough so if started it will start from the beginning.
        /// </summary>
        public void Reset()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Pause the playthrough as its running.
        /// </summary>
        public void Pause()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Go to a specific time through the playthrough.
        /// </summary>
        /// <param name="time">The time to jump to in ms.</param>
        public void JumpTo(int time)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Initialise the flythrough from an xml file.
        /// </summary>
        /// <param name="file">The file to load as a flythrough.</param>
        public void Load(string file)
        {
            throw new System.NotImplementedException();
        }
    }
}

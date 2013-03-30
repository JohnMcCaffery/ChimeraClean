﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using NuiLibDotNet;

namespace Chimera.Kinect.Overlay {
    public class SkeletonLostTrigger : ITrigger {
        private bool mActive;
        private DateTime mLost;
        private bool mTriggered = false;
        private double mTimeout;

        #region ITrigger Members

        public event Action Triggered;

        public bool Active {
            get { return mActive; }
            set { mActive = value; }
        }

        #endregion

        /// <summary>
        /// How long after losing a skeleton the trigger fires.
        /// </summary>
        public double Timeout {
            get { return mTimeout; }
            set { mTimeout = value; }
        }

        public SkeletonLostTrigger(Coordinator coordinator) {
            Nui.SkeletonLost += new SkeletonTrackDelegate(Nui_SkeletonLost);
            coordinator.Tick += new Action(coordinator_Tick);
            mLost = DateTime.Now;
        }

        public SkeletonLostTrigger(Coordinator coordinator, double timeout)
            : this(coordinator) {
            mTimeout = timeout;
        }

        void coordinator_Tick() {
            if (mActive && Triggered != null && !mTriggered && mTimeout != 0.0 && !Nui.HasSkeleton && DateTime.Now.Subtract(mLost).TotalMilliseconds > mTimeout) {
                mTriggered = true;
                Triggered();
            }
        }

        void Nui_SkeletonLost() {
            if (mActive) {
                mTriggered = false;
                if (mTimeout == 0.0 && Triggered != null)
                    Triggered();
                else
                    mLost = DateTime.Now;
            }
        }
    }
}
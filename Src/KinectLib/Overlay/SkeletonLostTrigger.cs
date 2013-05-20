/*************************************************************************
Copyright (c) 2012 John McCaffery 

This file is part of Chimera.

Chimera is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Chimera is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Chimera.  If not, see <http://www.gnu.org/licenses/>.

**************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using NuiLibDotNet;
using Chimera.Overlay;
using System.Xml;
using System.Drawing;

namespace Chimera.Kinect.Overlay {
    public class SkeletonLostFactory : XmlLoader, ITriggerFactory {
        public SpecialTrigger Special {
            get { return SpecialTrigger.None; }
        }

        public string Mode {
            get { return OverlayPlugin.HOVER_MODE; }
        }

        public string Name {
            get { return "SkeletonLost"; }
        }

        public ITrigger Create(OverlayPlugin manager, XmlNode node) {
            double timeout = GetDouble(node, 30000, "Timeout");
            return new SkeletonLostTrigger(manager.Coordinator, timeout);
        }

        public ITrigger Create(OverlayPlugin manager, XmlNode node, Rectangle clip) {
            return Create(manager, node);
        }
    }
    public class SkeletonLostTrigger : XmlLoader, ITrigger {
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

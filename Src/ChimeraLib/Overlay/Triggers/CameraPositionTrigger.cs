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
using System.Drawing;
using System.Xml;

namespace Chimera.Overlay.Triggers {
    public class CameraPositionTrigger : ITrigger {
        private bool mActive;
        private bool mInArea;
        private Rectangle mActiveArea;

        public event Action Triggered;
        public event Action Left;

        public CameraPositionTrigger(Coordinator coordinator) {
            coordinator.CameraUpdated += new Action<Coordinator,CameraUpdateEventArgs>(coordinator_CameraUpdated);
        }

        public CameraPositionTrigger(XmlNode node) {
            //TODO add logic for initialisation
        }

        private void coordinator_CameraUpdated(Coordinator coordinator, CameraUpdateEventArgs args) {
            if (mActive) {
                if (mActiveArea.Contains(new Point((int)args.position.X, (int)args.position.Y))) {
                    mInArea = true;
                    if (Triggered != null)
                        Triggered();
                } else if (mInArea) {
                    mInArea = false;
                    if (Left != null)
                        Left();
                }
            }
        }

        public bool Active {
            get { return mActive; }
            set { 
                mActive = value;
                if (!value)
                    mInArea = false;
            }
        }
    }
}

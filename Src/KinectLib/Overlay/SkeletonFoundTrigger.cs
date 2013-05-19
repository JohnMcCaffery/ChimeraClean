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

namespace Chimera.Kinect.Overlay {    public class SkeletonFoundFactory : ITriggerFactory {
        public SpecialTrigger Special {
            get { return SpecialTrigger.None; }
        }

        public string Mode {
            get { return OverlayPlugin.HOVER_MODE; }
        }

        public string Name {
            get { return "SkeletonFound"; }
        }

        public ITrigger Create(OverlayPlugin manager, XmlNode node) {
            return new SkeletonFoundTrigger();
        }

        public ITrigger Create(OverlayPlugin manager, XmlNode node, Rectangle clip) {
            return Create(manager, node);
        }
    }
    public class SkeletonFoundTrigger : ITrigger {
        private bool mActive;

        public event Action Triggered;

        public bool Active {
            get { return mActive; }
            set { mActive = value; }
        }

        public SkeletonFoundTrigger() {
            Nui.SkeletonFound += new SkeletonTrackDelegate(Nui_SkeletonFound);
        }

        void Nui_SkeletonFound() {
            if (mActive && Triggered != null)
                Triggered();
        }
    }
}

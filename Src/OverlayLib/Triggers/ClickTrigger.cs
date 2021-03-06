﻿/*************************************************************************
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
using System.Drawing;
using System.Windows.Forms;
using Chimera.Interfaces.Overlay;
using System.Xml;
using System.Threading;

namespace Chimera.Overlay.Triggers {
    public class ClickTriggerFactory : ITriggerFactory {
        public SpecialTrigger Special {
            get { return SpecialTrigger.Invisible; }
        }

        public string Mode {
            get { return OverlayPlugin.CLICK_MODE; }
        }

        public string Name {
            get { return "ClickTrigger"; }
        }

        public ITrigger Create(OverlayPlugin manager, XmlNode node) {
            return new ClickTrigger(manager, node);
        }

        public ITrigger Create(OverlayPlugin manager, XmlNode node, Rectangle clip) {
            return new ClickTrigger(manager, node, clip);
        }
    }

    public class ClickTrigger : AreaTrigger {
        /// <summary>
        /// Create the trigger. Specifies the position and size of the area the cursor must hover in to trigger this trigger as values between 0 and 1.
        /// 0,0 is top left, 1,1 is bottom right.
        /// </summary>
        /// <param name="manager">The manager which manages the window this trigger is to draw to.</param>
        /// <param name="render">The renderer used to draw this trigger being selected.</param>
        /// <param name="x">The x coordinate for where the image is to be positioned, specified between 0 and 1. 0 is flush to the left, 1 flush to the right.</param>
        /// <param name="y">The y coordinate for where the image is to be positioned, specified between 0 and 1. 0 is flush to the top, 1 flush to the bottom.</param>
        /// <param name="x">The width of the image, specified between 0 and 1. 1 will fill the entire width, 0 will be invisible.</param>
        /// <param name="y">The width of the image, specified between 0 and 1. 1 will fill the entire height, 0 will be invisible.</param>
        public ClickTrigger(FrameOverlayManager manager, float x, float y, float w, float h)
            : this(manager, new RectangleF(x, y, w, h)) {
        }

        public ClickTrigger(FrameOverlayManager manager, int x, int y, int w, int h, Rectangle clip)
            : this(manager, (float) x / (float) clip.Width, (float) y / (float) clip.Height, (float) w / (float) clip.Width, (float) h / (float) clip.Height) {
        }

        public ClickTrigger(FrameOverlayManager manager, RectangleF bounds)
            : base(manager, bounds) {
            Manager.OnRelease += new Action(mManager_OnRelease);
        }

        public ClickTrigger(OverlayPlugin manager, XmlNode node)
            : base(manager, node) {
            Manager.OnRelease += new Action(mManager_OnRelease);
        }

        public ClickTrigger(OverlayPlugin manager, XmlNode node, Rectangle clip)
            : base(manager, node, clip) {
            Manager.OnRelease += new Action(mManager_OnRelease);
        }

        void mManager_OnRelease() {
            if (Active && Inside)
                Trigger();
        }

        protected override void Entered() { }

        protected override void Exited() { }
    }
}

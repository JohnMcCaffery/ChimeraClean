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
using Chimera.Overlay;
using System.Drawing;

namespace Chimera.Overlay.Transitions {
    public class CutTransitionFactory : ITransitionStyleFactory {
        public ITransitionStyle Create(OverlayPlugin manager, System.Xml.XmlNode node) {
            return new CutTransition();
        }

        public ITransitionStyle Create(OverlayPlugin manager, System.Xml.XmlNode node, Rectangle clip) {
            return Create(manager, node);
        }

        public string Name {
            get { return "Cut"; }
        }
    }

    public class CutTransition : XmlLoader, ITransitionStyle {
        public IWindowTransition Create(StateTransition transition, FrameOverlayManager manager) {
            return new CutWindowTransition(transition, manager);
        }
    }
    public class CutWindowTransition : FrameTransition {
        public override event Action<IWindowTransition> Finished;

        public CutWindowTransition(StateTransition transition, FrameOverlayManager manager)
            : base(transition, manager) {
        }

        public override void Begin() {
            base.Begin();
            if (Finished != null)
                Finished(this);
        }

        public override void Cancel() { }

        public override bool NeedsRedrawn {
            get { return false; }
        }

        public override void DrawStatic(Graphics graphics) { }

        public override void DrawDynamic(Graphics graphics) { }
    }
}

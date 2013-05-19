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
    public class CutWindowTransitionFactory : IWindowTransitionFactory {
        public IWindowTransition Create(StateTransition transition, WindowOverlayManager manager) {
            return new CutWindowTransition(transition, manager);
        }
    }
    public class CutWindowTransition : WindowTransition {
        public override event Action<IWindowTransition> Finished;

        public CutWindowTransition(StateTransition transition, WindowOverlayManager manager)
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

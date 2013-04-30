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
using System.Drawing;

namespace Chimera.Overlay {
    public class InvisibleSelection : HoverTrigger {
        public override event Action<ISelectable> StaticChanged;

        public InvisibleTrigger(ISelectionRenderer renderer, float x, float y, float w, float h)
            : base(renderer, x, y, w, h) {
        }

        public InvisibleTrigger(float x, float y, float w, float h)
            : base(x, y, w, h) {
        }

        public override void DrawStatic(Graphics graphics, Rectangle clipRectangle) { }

        public override void Show() {
            Active = false;
        }

        public override void Hide() {
            Active = false;
        }
    }
}

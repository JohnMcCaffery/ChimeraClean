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
    public class BoxSelection : HoverTrigger {
        private Color mColour = Color.Red;        private float mWidth = 10f;        public override event Action<ISelectable> StaticChanged;
        public BoxSelection(ISelectionRenderer renderer, float x, float y, float w, float h)
            : base(renderer, x, y, w, h) {
        }

        public BoxSelection(float x, float y, float w, float h)
            : base(x, y, w, h) {
        }

        public BoxSelection(Color colour, ISelectionRenderer renderer, float x, float y, float w, float h)
            : this(renderer, x, y, w, h) {
            mColour = colour;
        }

        public BoxSelection(Color colour, float x, float y, float w, float h)
            : this(x, y, w, h) {
            mColour = colour;
        }

        public BoxSelection(Color colour, float width, float x, float y, float w, float h)
            : this(colour, x, y, w, h) {
            mWidth = width;
        }

        public BoxSelection(Color colour, float width, ISelectionRenderer renderer, float x, float y, float w, float h)
            : this(renderer, x, y, w, h) {
            mWidth = width;
        }

        public override void DrawStatic(Graphics graphics, Rectangle clipRectangle) {
            using (Pen pen = new Pen(mColour, mWidth)) {
                graphics.DrawRectangle(pen, ScaledBounds);
            }
        }

        public override void Show() {
            Visible = true;
        }

        public override void Hide() {
            Visible = false;
        }
    }
}

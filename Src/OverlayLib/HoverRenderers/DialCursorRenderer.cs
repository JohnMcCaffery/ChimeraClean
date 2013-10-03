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
using Chimera.Interfaces.Overlay;

namespace Chimera.Overlay.SelectionRenderers {
    public class DialCursorRendererFactory : ISelectionRendererFactory {
        #region IFactory<ISelectionRenderer> Members

        public ISelectionRenderer Create(OverlayPlugin manager, System.Xml.XmlNode node) {
            return new DialCursorRenderer(manager, new DialRenderer(manager, node));
        }

        public ISelectionRenderer Create(OverlayPlugin manager, System.Xml.XmlNode node, Rectangle clip) {
            return Create(manager, node);
        }

        #endregion

        #region IFactory Members

        public string Name {
            get { return "DialCursor"; }
        }

        #endregion
    }
    public class DialCursorRenderer : CursorRenderer {
        private static readonly int DEFAULT_R = 40;
        private static readonly Color DEFAULT_COLOUR = Color.Red;


        public DialCursorRenderer(OverlayPlugin manager)
            : base(new DialRenderer(DEFAULT_R, DEFAULT_COLOUR).DrawHover, new Size(DEFAULT_R * 2, DEFAULT_R * 2), manager[0]) {
        }
        public DialCursorRenderer(OverlayPlugin manager, int r)
            : base(new DialRenderer(r, DEFAULT_COLOUR).DrawHover, new Size(r * 2, r * 2), manager[0]) {
        }
        public DialCursorRenderer(OverlayPlugin manager, Color colour)
            : base(new DialRenderer(DEFAULT_R, colour).DrawHover, new Size(DEFAULT_R * 2, DEFAULT_R * 2), manager[0]) {
        }
        public DialCursorRenderer(OverlayPlugin manager, int r, Color colour)
            : base(new DialRenderer(DEFAULT_R, colour).DrawHover, new Size(r * 2, r * 2), manager[0]) {
        }
        public DialCursorRenderer(OverlayPlugin manager, DialRenderer r)
            : base(r.DrawHover, new Size(r.R * 2, r.R * 2), manager[0]) {
        }
    }
}

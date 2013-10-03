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
using System.Xml;
using Chimera.Interfaces.Overlay;

namespace Chimera.Overlay.Features {
    public class StaticTextFactory : IFeatureFactory {
        #region IFactory<IFeature> Members

        public IFeature Create(OverlayPlugin manager, XmlNode node) {
            return new StaticText(manager, node);
        }

        public IFeature Create(OverlayPlugin manager, XmlNode node, Rectangle clip) {
            return new StaticText(manager, node, clip);
        }

        #endregion

        #region IFactory Members

        public string Name {
            get { return "StaticText"; }
        }

        #endregion
    }

    public class StaticText : Text {
        private FrameOverlayManager mManager;
        
        public StaticText(string text, FrameOverlayManager manager, Font font, Color colour, PointF location)
            : base(text, manager.Frame.Name, font, colour, location) {

            mManager = manager;
        }

        public StaticText(OverlayPlugin manager, XmlNode node)
            : base(manager, node) {

            mManager = GetManager(manager, node, "text");
        }

        public StaticText(OverlayPlugin manager, XmlNode node, Rectangle clip)
            : base(manager, node, clip) {

            mManager = GetManager(manager, node, "text");
        }

        public override string TextString {
            get { return base.TextString; }
            set {
                base.TextString = value;
                mManager.ForceRedrawStatic();
            }
        }

        public override void DrawDynamic(Graphics graphics) { }

        public override void DrawStatic(Graphics graphics) {
            Draw(graphics);
        }
    }
}

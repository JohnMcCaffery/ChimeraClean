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
using Chimera.Overlay.Drawables;
using System.Drawing;
using Chimera.Interfaces.Overlay;
using System.Xml;

namespace Chimera.Overlay.Triggers {
    public class TextHoverTriggerFactory : ITriggerFactory {
        public SpecialTrigger Special {
            get { return SpecialTrigger.Text; }
        }

        public string Mode {
            get { return StateManager.HOVER_MODE; }
        }

        public string Name {
            get { return "TextHoverTrigger"; }
        }

        public ITrigger Create(XmlNode node, StateManager manager) {
            return new TextHoverTrigger(manager, node);
        }

        public ITrigger Create(XmlNode node, StateManager manager, Rectangle clip) {
            return new TextHoverTrigger(manager, node, clip);
        }
    }

    public class TextHoverTrigger : HoverTrigger, IDrawable {
        private Text mText;

        public TextHoverTrigger(WindowOverlayManager manager, IHoverSelectorRenderer renderer, Text text, Rectangle clip)
            : base(manager, renderer, Text.GetBounds(text, clip)) {
                mText = text;
                Clip = clip;
        }

        public TextHoverTrigger(StateManager manager, XmlNode node)
            : base(manager, node) {
            mText = new StaticText(manager, node);
            throw new NotImplementedException("What happens if Clip is not set?");
        }

        public TextHoverTrigger(StateManager manager, XmlNode node, Rectangle clip)
            : base(manager, node, clip) {
            mText = new StaticText(manager, node, clip);
            Clip = clip;
        }

        protected override RectangleF Bounds {
            get { return Text.GetBounds(mText, Clip); }
            set { }
        }

        #region IDrawable Members

        public override Rectangle Clip {
            get { return base.Clip; }
            set {
                base.Clip = value;
                mText.Clip = value;
            }
        }

        public override bool Active {
            get { return base.Active; }
            set { 
                base.Active = value;
                mText.Active = value;
            }
        }

        public override bool NeedsRedrawn {
            get { return mText.NeedsRedrawn || base.NeedsRedrawn; }
        }

        string IDrawable.Window {
            get { return mText.Window; }
        }

        void IDrawable.DrawStatic(Graphics graphics) {
            mText.DrawStatic(graphics);
            base.DrawStatic(graphics);
        }

        void IDrawable.DrawDynamic(Graphics graphics) {
            mText.DrawDynamic(graphics);
            base.DrawDynamic(graphics);
        }

        #endregion

        public override string ToString() {
            return mText.ToString();
        }
    }
}

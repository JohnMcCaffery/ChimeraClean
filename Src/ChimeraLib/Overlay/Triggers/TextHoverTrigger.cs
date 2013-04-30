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

namespace Chimera.Overlay.Triggers {
    public class TextHoverTrigger : HoverTrigger, IDrawable {
        private Text mText;
        private bool mActive;

        private static RectangleF GetBounds(Text text, Rectangle clip) {
            using (Bitmap b = new Bitmap(1, 1)) {
                using (Graphics g = Graphics.FromImage(b)) {
                    SizeF size = g.MeasureString(text.TextString, text.Font);
                    return new RectangleF(text.Position.X, text.Position.Y, size.Width / clip.Width, size.Height / clip.Height);
                }
            }
        }

        public TextHoverTrigger(WindowOverlayManager manager, IHoverSelectorRenderer renderer, Text text, Rectangle clip)
            : base(manager, renderer, GetBounds(text, clip)) {
                mText = text;
                Clip = clip;
        }

        protected override RectangleF Bounds {
            get { return GetBounds(mText, Clip); }
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
            get { return mActive; }
            set { 
                mActive = value;
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
    }
}

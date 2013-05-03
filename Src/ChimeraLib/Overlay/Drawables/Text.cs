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
using System.Drawing;

namespace Chimera.Overlay.Drawables {
    public abstract class Text : IDrawable {
        public static RectangleF GetBounds(Text text, Rectangle clip) {
            using (Bitmap b = new Bitmap(1, 1)) {
                using (Graphics g = Graphics.FromImage(b)) {
                    SizeF size = g.MeasureString(text.TextString, text.Font);
                    return new RectangleF(text.Position.X, text.Position.Y, size.Width / clip.Width, size.Height / clip.Height);
                }
            }
        }

        private string mText;
        private bool mActive;
        private string mWindow;
        /// <summary>
        /// The clip rectangle bounding the area this item will be drawn to.
        /// </summary>
        private Rectangle mClip;
 
        private Font mFont;
        private PointF mPosition;
        protected readonly Color mColour;

        public virtual Rectangle Clip {
            get { return mClip; }
            set { mClip = value; }
        }


        public virtual String TextString {
            get { return mText; }
            set { mText = value; }
        }

        public Text(string text, string window, Font font, Color colour, PointF location) {
            mText = text;
            mWindow = window;
            mFont = font;
            mColour = colour;
            mPosition = location;
        }

        public Text(string text, string window, Font font, Color colour, Point location, Rectangle clip)
            : this(text, window, font, colour, new PointF((float)location.X / (float)clip.Width, (float)location.Y / (float)clip.Height)) {
        }

        #region IDrawable Members

        public virtual bool Active {
            get { return mActive; }
            set { mActive = value; }
        }

        public virtual bool NeedsRedrawn {
            get { return false; }
        }

        public virtual string Window {
            get { return mWindow; }
        }

        public abstract void DrawStatic(Graphics graphics);

        public abstract void DrawDynamic(System.Drawing.Graphics graphics);

        #endregion

        protected void Draw(Graphics g) {
            using (Brush b = new SolidBrush(mColour))
                Draw(g, b);
        }
        protected void Draw(Graphics g, Brush b) {
                g.DrawString(mText, mFont, b, GetPoint(Clip));
        }

        protected PointF GetPoint(Rectangle clip) {
            return new PointF(clip.Width * mPosition.X, clip.Height * mPosition.Y);
        }

        public PointF Position {
            get { return mPosition; }
        }

        public Font Font {
            get { return mFont; }
        }
    }
}

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
using System.Xml;

namespace Chimera.Overlay.Drawables {
    public abstract class Text : XmlLoader, IFeature {
        public static RectangleF GetBounds(Text text, Rectangle clip) {
            using (Bitmap b = new Bitmap(1, 1)) {
                using (Graphics g = Graphics.FromImage(b)) {
                    SizeF size = g.MeasureString(text.TextString, text.Font);
                    return new RectangleF(text.Position.X, text.Position.Y, size.Width / clip.Width, size.Height / clip.Height);
                }
            }
        }

        private string mText;
        private bool mActive = true;
        private string mWindow;
        private SizeF mSize = new SizeF(10f, 10f);
        private ContentAlignment mAlignment = ContentAlignment.TopLeft;
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

        public virtual ContentAlignment Alignment {
            get { return mAlignment; }
            set { mAlignment = value; }
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

        public Text(OverlayPlugin manager, XmlNode node) {
            mText = node != null ? node.InnerText : "";
            mWindow = GetManager(manager, node, "text").Window.Name;
            mFont = GetFont(node, "text");
            mColour = GetColour(node, "text", DEFAULT_FONT_COLOUR);
            mPosition = GetBounds(node, "text").Location;

            if (node != null && node.Attributes["Alignment"] != null) {
                ContentAlignment alignment = ContentAlignment.TopLeft;
                if (Enum.TryParse<ContentAlignment>(node.Attributes["Alignment"].Value, out alignment))
                    mAlignment = alignment;
            }
        }

        public Text(OverlayPlugin manager, XmlNode node, Rectangle clip)
            : this(manager, node) {
            mPosition = GetBounds(node, "text", clip).Location;
        }

        #region IDrawable Members

        public virtual bool Active {
            get { return mActive; }
            set { mActive = value; }
        }

        public virtual bool NeedsRedrawn {
            get { return false; }
        }

        public virtual string Frame {
            get { return mWindow; }
        }

        public abstract void DrawStatic(Graphics graphics);

        public abstract void DrawDynamic(Graphics graphics);

        #endregion

        protected void Draw(Graphics g) {
            using (Brush b = new SolidBrush(mColour))
                Draw(g, b);
        }
        protected void Draw(Graphics g, Brush b) {
            mSize = g.MeasureString(mText, mFont);
            g.DrawString(mText, mFont, b, GetPoint(Clip));
        }

        protected PointF GetPoint(Rectangle clip) {
            PointF p = new PointF(clip.Width * mPosition.X, clip.Height * mPosition.Y);
            switch (mAlignment) {
                case ContentAlignment.BottomCenter: p.X -= mSize.Width / 2f; p.Y -= mSize.Height; break;
                case ContentAlignment.BottomLeft:   p.Y -= mSize.Height; break;
                case ContentAlignment.BottomRight:  p.X -= mSize.Width; p.Y -= mSize.Height; break;

                case ContentAlignment.MiddleCenter: p.X -= mSize.Width / 2f; p.Y -= mSize.Height / 2f; break;
                case ContentAlignment.MiddleLeft:   p.Y -= mSize.Height / 2f; break;
                case ContentAlignment.MiddleRight:  p.X -= mSize.Width; p.Y -= mSize.Height / 2f; break;

                case ContentAlignment.TopCenter: p.X -= mSize.Width / 2f;; break;
                case ContentAlignment.TopRight:  p.X -= mSize.Width; break;
            }
            return p;
        }

        public PointF Position {
            get { return mPosition; }
        }

        public Font Font {
            get { return mFont; }
        }

        public override string ToString() {
            return mText != null ? mText : "";
        }
    }
}

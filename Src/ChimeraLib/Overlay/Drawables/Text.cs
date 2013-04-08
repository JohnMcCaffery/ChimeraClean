using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using System.Drawing;

namespace Chimera.Overlay.Drawables {
    public abstract class Text : IDrawable {
        private string mText;
        private bool mActive;
        private string mWindow;

        private Font mFont;
        private PointF mPosition;
        protected readonly Color mColour;

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

        public abstract void RedrawStatic(Rectangle clip, Graphics graphics);

        public abstract void DrawDynamic(System.Drawing.Graphics graphics);

        #endregion

        protected void Draw(Graphics g, Rectangle clip) {
            using (Brush b = new SolidBrush(mColour))
                Draw(g, clip, b);
        }
        protected void Draw(Graphics g, Rectangle clip, Brush b) {
                g.DrawString(mText, mFont, b, GetPoint(clip));
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

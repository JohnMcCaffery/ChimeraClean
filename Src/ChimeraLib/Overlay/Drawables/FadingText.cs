using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using System.Drawing;

namespace Chimera.Overlay.Drawables {
    public class FadingText : IDrawable {
        private DateTime mActivated;
        private double mFadeTime;
        private string mText;
        private string mWindow;
        private Color mColour;
        private Font mFont;
        private PointF mPosition;

        /// <summary>
        /// Create a fading text object, specifying position as relative values.
        /// </summary>
        /// <param name="text">The text to draw.</param>
        /// <param name="window">The window to draw the text on.</param>
        /// <param name="colour">The colour to draw the text.</param>
        /// <param name="font">The font to draw the text with.</param>
        /// <param name="x">Where the text should be positioned, as relative values (0: left, 1: right)</param>
        /// <param name="y">Where the text should be positioned, as relative values (0: top, 1: bottom)</param>
        public FadingText(string text, string window, Color colour, Font font, float x, float y) {
            mWindow = window;
            mText = text;
            mColour = colour;
            mFont = font;
            mPosition = new PointF(x, y);
        }

        public FadingText(string text, string window, Color colour, Font font, int x, int y, Rectangle bounds)
            : this(text, window, colour, font, (float)x / (float)bounds.Width, (float)y / (float)bounds.Height) {
        }

        public bool NeedsRedrawn {
            get { return DateTime.Now.Subtract(mActivated).TotalMilliseconds < mFadeTime; }
        }

        public string Window {
            get { return mWindow; }
        }

        public void RedrawStatic(Rectangle clip, Graphics graphics) {
            mActivated = DateTime.Now;
        }

        public void DrawDynamic(Graphics graphics) {
            double done = DateTime.Now.Subtract(mActivated).TotalMilliseconds / mFadeTime;
            if (done < 1.0) {
                using (Brush b = new SolidBrush(Color.FromArgb((int) (255.0 * (1.0 - done)), mColour))) {
                    graphics.DrawString(mText, mFont, b, mPosition);
                }
            }
        }
    }
}

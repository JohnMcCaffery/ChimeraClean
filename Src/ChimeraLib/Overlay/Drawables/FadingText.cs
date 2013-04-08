﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using System.Drawing;

namespace Chimera.Overlay.Drawables {
    public class FadingText : IDrawable {
        private DateTime mActivated;
        private double mSolidTime;
        private double mFadeTime;
        private string mText;
        private string mWindow;
        private Color mColour;
        private Font mFont;
        private PointF mPosition;
        private bool mFirstDrawn;

        /// <summary>
        /// Create a fading text object, specifying position as relative values.
        /// Fading texts will draw the same text on screen, getting fainter and fainter, for the specified length of time.
        /// The stay fully visible for 'solidTimeMS' then fade away over the length of 'fadeTimeMS'. Both specified in milliseconds.
        /// </summary>
        /// <param name="text">The text to draw.</param>
        /// <param name="fadeTimeMS">How long the text should remain on screen before fading away completely.</param>
        /// <param name="window">The window to draw the text on.</param>
        /// <param name="colour">The colour to draw the text.</param>
        /// <param name="font">The font to draw the text with.</param>
        /// <param name="x">Where the text should be positioned, as relative values (0: left, 1: right)</param>
        /// <param name="y">Where the text should be positioned, as relative values (0: top, 1: bottom)</param>
        public FadingText(string text, double solidTimeMS, double fadeTimeMS, string window, Color colour, Font font, float x, float y) {
            mWindow = window;
            mSolidTime = solidTimeMS;
            mFadeTime = fadeTimeMS;
            mText = text;
            mColour = colour;
            mFont = font;
            mPosition = new PointF(x, y);
        }

        /// <summary>
        /// Create a fading text object, specifying position as absolute values and a bounding area they are contained within.
        /// Fading texts will draw the same text on screen, getting fainter and fainter, for the specified length of time.
        /// The stay fully visible for 'solidTimeMS' then fade away over the length of 'fadeTimeMS'. Both specified in milliseconds.
        /// </summary>
        /// <param name="text">The text to draw.</param>
        /// <param name="fadeTimeMS">How long the text should remain on screen before fading away completely.</param>
        /// <param name="window">The window to draw the text on.</param>
        /// <param name="colour">The colour to draw the text.</param>
        /// <param name="font">The font to draw the text with.</param>
        /// <param name="x">Where the text should be positioned, as an absolute value on bounds.</param>
        /// <param name="y">Where the text should be positioned, as an absolute value on bounds.</param>
        /// <param name="bounds">The bounding area on which the text is to be drawn.</param>
        public FadingText(string text, double solidTimeMS, double fadeTimeMS, string window, Color colour, Font font, int x, int y, Rectangle bounds)
            : this(text, solidTimeMS, fadeTimeMS, window, colour, font, (float)x / (float)bounds.Width, (float)y / (float)bounds.Height) {
        }

        private double Time {
            get { return DateTime.Now.Subtract(mActivated).TotalMilliseconds; }
        }

        public bool NeedsRedrawn {
            get { return !mFirstDrawn || Time > mSolidTime && Time < mSolidTime + mFadeTime; }
        }

        public string Window {
            get { return mWindow; }
        }

        public void RedrawStatic(Rectangle clip, Graphics graphics) {
            mActivated = DateTime.Now;
            mFirstDrawn = false;
        }

        public void DrawDynamic(Graphics graphics) {
            double done = (Time - mSolidTime) / mFadeTime;
            if (done < 0.0) {
                mFirstDrawn = true;
                using (Brush b = new SolidBrush(mColour)) {
                    graphics.DrawString(mText, mFont, b, mPosition);
                }
            }
            if (done < 1.0) {
                using (Brush b = new SolidBrush(Color.FromArgb((int) (255.0 * (1.0 - done)), mColour))) {
                    graphics.DrawString(mText, mFont, b, mPosition);
                }
            }
        }
    }
}

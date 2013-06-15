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

namespace Chimera.Overlay.Features {
    public class FadingText : Text, IFeature {
        private DateTime mActivated;
        private double mSolidTime;
        private double mFadeTime;
        private bool mFirstDrawn;
        private bool mActive = true;
        private PointF mLocation;

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
        public FadingText(string text, double solidTimeMS, double fadeTimeMS, string window, Color colour, Font font, float x, float y)
            : base(text, window, font, colour, new PointF(x, y)) {
            mSolidTime = solidTimeMS;
            mFadeTime = fadeTimeMS;
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

        public override bool NeedsRedrawn {
            //get { return !mFirstDrawn || Time > mSolidTime && Time < mSolidTime + mFadeTime; }
            get { return true; }
        }

        public override Rectangle Clip {
            get { return base.Clip; }
            set {
                base.Clip = value;
                mLocation = GetPoint(value);
            }
        }

        public override void DrawStatic(Graphics graphics) {
            mActivated = DateTime.Now;
            mFirstDrawn = false;
        }

        public override void DrawDynamic(Graphics graphics) {
            double done = (Time - mSolidTime) / mFadeTime;
            mFirstDrawn = true;
            if (done < 0.0) {
                using (Brush b = new SolidBrush(mColour)) {
                    graphics.DrawString(TextString, Font, b, mLocation);
                }
            }
            else if (done < 1.0) {
                using (Brush b = new SolidBrush(Color.FromArgb((int) (255.0 * (1.0 - done)), mColour))) {
                    graphics.DrawString(TextString, Font, b, mLocation);
                }
            }
        }
    }
}

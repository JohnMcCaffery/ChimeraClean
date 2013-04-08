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
        private Point mPosition;



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

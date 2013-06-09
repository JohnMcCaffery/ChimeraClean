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
using Chimera.Interfaces.Overlay;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Xml;

namespace Chimera.Overlay.Transitions {
    public class BitmapFadeFactory : XmlLoader, IImageTransitionFactory {
        #region IImageTransitionFactory Members

        public string Name { get { return "Fade"; } }

        public IImageTransition Create(OverlayPlugin manager, XmlNode node) {
            double length = GetDouble(node, 2000, "Length");
            return new FadeTransition(length);
        }

        public IImageTransition Create(OverlayPlugin manager, XmlNode node, Rectangle clip) {
            return Create(manager, node);
        }

        public IImageTransition Create(double length) {
            return new FadeTransition(length);
        }

        #endregion

    }
    public class FadeTransition : XmlLoader, IImageTransition {
        /// <summary>
        /// When the transition began.
        /// </summary>
        private DateTime mTransitionStart;
        /// <summary>
        /// How long the transition should last.
        /// </summary>
        private double mLengthMS;
        /// <summary>
        /// The image for the background of the state that is fading out.
        /// </summary>
        private Bitmap mFrom;
        /// <summary>
        /// The image for the background of the state that is fading out.
        /// </summary>
        private Bitmap mTo;
        /// <summary>
        /// Whether this transition is active as a drawable.
        /// </summary>
        private bool mActive = true;
        /// <summary>
        /// Whether the transition has completed.
        /// </summary>
        private bool mCompleted;
        /// <summary>
        /// The clip rectangle bounding the area this item will be drawn to.
        /// </summary>
        private Rectangle mClip;
 


        public event Action Finished;

        public FadeTransition(double lengthMS) {
            mLengthMS = lengthMS;
        }

        public FadeTransition(double lengthMS, Bitmap from, Bitmap to) {
            mLengthMS = lengthMS;
            Init(from, to);
        }

        public void Begin() {
            mCompleted = false;
            mTransitionStart = DateTime.Now;
        }

        public void Init(Bitmap from, Bitmap to) {
            mFrom = from;
            mTo = to;
        }

        #region IImageTransition Members

        public void Cancel() {
            mCompleted = true;
        }

        #endregion

        public virtual Rectangle Clip {
            get { return mClip; }
            set { mClip = value; }
        }


        #region IDrawable Members

        public bool Active {
            get { return mActive; }
            set { mActive = value; }
        }

        public bool NeedsRedrawn {
            get { return !mCompleted; }
        }

        public string Frame {
            get { throw new NotImplementedException(); }
        }

        public void DrawStatic(Graphics graphics) {
            graphics.DrawImage(mTo, 0, 0);
        }

        public void DrawDynamic(System.Drawing.Graphics graphics) {
            double time = DateTime.Now.Subtract(mTransitionStart).TotalMilliseconds;
            if (time > mLengthMS && !mCompleted) {
                mCompleted = true;
                if (Finished != null)
                    Finished();
            }

            else if (!mCompleted) {
                int i = (int) (time / 20.0);
                using (Bitmap step = CreateStep(time))
                    graphics.DrawImage(step, 0, 0);
            }
        }

        #endregion

        private Bitmap CreateStep(double time) {
            Bitmap image = new Bitmap(mFrom);
            Rectangle affectedRect = new Rectangle(0, 0, image.Width, image.Height);
            BitmapData dat = image.LockBits(affectedRect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            byte[] argbValues = new byte[image.Height * dat.Stride];

            Marshal.Copy(dat.Scan0, argbValues, 0, argbValues.Length);

            byte a = (byte)((double)byte.MaxValue * (1.0 - (time / mLengthMS)));

            for (int i = 3; i < argbValues.Length; i += 4)
                argbValues[i] = a;

            Marshal.Copy(argbValues, 0, dat.Scan0, argbValues.Length);
            image.UnlockBits(dat);
            return image;
        }
    }
}

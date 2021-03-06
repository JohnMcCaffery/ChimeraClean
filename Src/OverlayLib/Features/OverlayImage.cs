﻿/*************************************************************************
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
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace Chimera.Overlay.Features {
    public class OverlayImageFactory : IFeatureFactory {
        #region IFactory<IFeature> Members

        public IFeature Create(OverlayPlugin manager, XmlNode node) {
            return new OverlayImage(manager, node, "image factory");
        }

        public IFeature Create(OverlayPlugin manager, XmlNode node, Rectangle clip) {
            return new OverlayImage(manager, node, clip, "image factory");
        }

        #endregion

        #region IFactory Members

        public string Name {
            get { return "Image"; }
        }

        #endregion
    }

    public class OverlayImage : OverlayXmlLoader, IFeature {
        private RectangleF mBounds;
        private Bitmap mImage;
        private string mFile;
        private float mAspectRatio;
        private string mWindow;
        private bool mActive = true;
        private int mW;
        private int mH;
        
        /// <summary>
        /// The clip rectangle bounding the area this item will be drawn to.
        /// </summary>
        private Rectangle mClip;
 

        public virtual Rectangle Clip {
            get { return mClip; }
            set { mClip = value; }
        }

        /// <summary>
        /// The position / size for the image. Specified as a relative values.
        /// For location: 0,0 = top left, 1,1 = bottom right.
        /// For size: 0,0 = invisible, 1,1 = same size as the screen
        /// </summary>
        public RectangleF Bounds {
            get {
                RectangleF bounds = new RectangleF(mBounds.X, mBounds.Y, mBounds.Width, mBounds.Height);

                if (mBounds.Width < 0) {
                    bounds.Width = mW / (float)Clip.Width;
                    bounds.Height = mH / (float)Clip.Height;
                } else if (mBounds.Height < 0f)
                    bounds.Height = mBounds.Width * mAspectRatio * ((float) Clip.Width / (float) Clip.Height);

                return bounds; 
            }
        }

        public bool NeedsRedrawn {
            get { return false; }
        }

        public string Frame {
            get { return mWindow; }
        }

        public bool Active {
            get { return mActive; }
            set {
                if (mActive != value) {
                    mActive = value;
                    if (value && mImage == null)
                        mImage = new Bitmap(mFile);
                    else if (mImage != null) {
                        mImage.Dispose();
                        mImage = null;
                    }
                }
            }
        }

        private OverlayImage(string image, string window) {
            mFile = image;
            using (mImage = new Bitmap(image)) {
                mWindow = window;
                mW = mImage.Width;
                mH = mImage.Height;
            }
            mImage = null;
        }

        /// <summary>
        /// Create the image, specifying x and y as relative values and determining the size by the size of the image vs the size of the area it is to be drawn on.
        /// </summary>
        /// <param name="image">The image to draw.</param>
        /// <param name="x">How far across the screen to draw the image (0: left, 1: right).</param>
        /// <param name="y">How far down the screen to draw the image (0: top, 1: bottom).</param>
        /// <param name="bounds">The bounding rectangle for the area the image is to be drawn on. Width and Height will be calculated using the relative size of this and the size of the image.</param>
        public OverlayImage(string image, float x, float y, Rectangle bounds, string window)
            : this(image, window) {

            using (mImage = new Bitmap(image)) {
                mBounds = new RectangleF(x, y, (float)mImage.Width / (float)bounds.Width, (float)mImage.Height / (float)bounds.Height);
            }
            mImage = null;
        }

        /// <summary>
        /// Create the image, specifying x and y as relative values and determining the size by the size of the image vs the size of the screen it is to be drawn on.
        /// </summary>
        /// <param name="image">The image to draw.</param>
        /// <param name="x">How far across the screen to draw the image (0: left, 1: right).</param>
        /// <param name="y">How far down the screen to draw the image (0: top, 1: bottom).</param>
        /// <param name="screen">The screen the image is to be drawn on. Width and Height will be calculated using the relative size of this and the size of the image.</param>
        public OverlayImage(string image, float x, float y, Screen screen, string window)
            : this(image, x, y, screen.Bounds, window) {
        }

        /// <summary>
        /// Create the image, specifying x and y as absolute coordinates on the area defined by the bounds rectangle. All value will be calculated from the ratio between the bounds rectangle and x, y, and image height/width.
        /// </summary>
        /// <param name="image">The image to draw.</param>
        /// <param name="x">Where across the bounding rectangle the image should be drawn.</param>
        /// <param name="y">Where down the bounding rectangle the image should be drawn.</param>
        /// <param name="bounds">The bounding rectangle for the area the image will be drawn on.</param>
        public OverlayImage(string image, int x, int y, Rectangle bounds, string window)
            : this(image, window) {

                using (mImage = new Bitmap(image)) {
                    mBounds = new RectangleF(
                        (float)x / (float)bounds.Width,
                        (float)y / (float)bounds.Height,
                        (float)mImage.Width / (float)bounds.Width,
                        (float)mImage.Height / (float)bounds.Height);
                }
                mImage = null;
        }

        /// <summary>
        /// Create the image, specifying only a position. The image will always stay the natural size of the image provided.
        /// </summary>
        /// <param name="image">The image to draw.</param>
        /// <param name="x">How far across the screen to draw the image (0: left, 1: right).</param>
        /// <param name="y">How far down the screen to draw the image (0: top, 1: bottom).</param>
        public OverlayImage(string image, float x, float y, string window)
            : this(image, window) {
            mBounds = new RectangleF(x, y, -1f, -1f);
        }

        /// <summary>
        /// Create the image image, specifying a position and width, height will be calculated to keep the aspect ratio the same as image supplied originally has.
        /// </summary>
        /// <param name="image">The image to draw.</param>
        /// <param name="x">How far across the screen to draw the image (0: left, 1: right).</param>
        /// <param name="y">How far down the screen to draw the image (0: top, 1: bottom).</param>
        /// <param name="w">The width to draw the image, specified as fraction of the width of the surface the image will be drawn on. (0: no width, 1: covers the entire screen).</param>
        public OverlayImage(string image, float x, float y, float w, string window)
            : this(image, window) {
            mBounds = new RectangleF(x, y, w, -1f);
            using (mImage = new Bitmap(image)) {
                mAspectRatio = (float)mImage.Height / (float)mImage.Width;
            }
                mImage = null;
        }

        /// <summary>
        /// Create the image image, specifying a position, width and height as fractional values, relative to whatever surface the image will be drawn on.
        /// </summary>
        /// <param name="image">The image to draw.</param>
        /// <param name="x">How far across the screen to draw the image (0: left, 1: right).</param>
        /// <param name="y">How far down the screen to draw the image (0: top, 1: bottom).</param>
        /// <param name="w">The width to draw the image, specified as fraction of the width of the surface the image will be drawn on. (0: no width, 1: covers the entire screen).</param>
        /// <param name="h">The width to draw the image, specified as fraction of the height of the surface the image will be drawn on. (0: no height, 1: covers the entire screen).</param>
        public OverlayImage(string image, float x, float y, float w, float h, string window)
            : this(image, window) {
            mBounds = new RectangleF(x, y, w, h);
        }

        public OverlayImage(OverlayPlugin manager, XmlNode node, string reason) {
            mFile = node.InnerText;
            using (mImage = GetImage(node, reason)) {
                if (mImage == null)
                    throw new ArgumentException("Problem loading image for " + reason + (node == null ? "." : " from " + node.Name + "."));

                mW = mImage.Width;
                mH = mImage.Height;

                float x = GetFloat(node, 0f, "X");
                float y = GetFloat(node, 0f, "Y");
                float w = GetFloat(node, -1f, "W", "Width");
                float h = GetFloat(node, -1f, "H", "Height");
                mBounds = new RectangleF(x, y, w, h);
                mAspectRatio = (float)mImage.Height / (float)mImage.Width;
                mWindow = GetManager(manager, node, "overlay image").Frame.Name;
            }
                mImage = null;
        }

        public OverlayImage(OverlayPlugin manager, XmlNode node, Rectangle clip, string reason)
            : this(manager, node, reason) {

            mBounds.X = mBounds.X / clip.Width;
            mBounds.Y = mBounds.Y / clip.Height;
            if (mBounds.Width > 0f) mBounds.Width = mBounds.Width / clip.Width;
            if (mBounds.Height > 0f) mBounds.Height = mBounds.Height / clip.Height;
        }

        public void DrawStatic(Graphics graphics) {
            if (mImage == null)

            lock (mImage) {
                int x = (int)(Clip.Width * mBounds.X);
                int y = (int)(Clip.Height * mBounds.Y);
                if (mBounds.Width > 0)
                {
                    int w = (int)(Clip.Width * mBounds.Width);
                    int h = (int)(mBounds.Height > 0 ? Clip.Height * mBounds.Height : w * mAspectRatio);
                    graphics.DrawImage(mImage, new Rectangle(x, y, w, h));
                }
                else
                    graphics.DrawImage(mImage, x, y, mW, mH);
            }
        }

        public void DrawDynamic(Graphics graphics) { }

        public override string ToString() {
            return mFile != null && mFile.Length > 0 ? mFile : mW + "x" + mH + " Image";
        }
    }
}


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
using System.Windows.Forms;

namespace KinectLib {
    public class ActiveArea {
        public static readonly int CURSOR_R = 15;
        public class Data {
            public RectangleF shape;
            public Action<Form, PointSurface> evt;
            public Bitmap img;

            public Data(string imgName, RectangleF shape, Action<Form, PointSurface> action) {
                this.shape = shape;
                this.evt = action;
                img = new Bitmap(imgName);
            }
        }
        private Bitmap mImage;
        private Bitmap mImageResized;
        private Panel mContainer;
        private Action<Form, PointSurface> mEvent;
        private Form mForm;
        private PointSurface mSurface;
        private PictureBox mPictureBox;
        private RectangleF mShape;
        private static readonly int SELECT_TIME = 3000;
        private bool mSelected;
        private bool mTriggered;
        private DateTime mFirstSelected;
        private Font mFont;

        public event Action Triggered;

        public RectangleF Shape {
            get { return mShape; }
        }

        public PictureBox ImageBox {
            get { return mPictureBox; }
        }

        public ActiveArea(PointSurface surface, Data data, Form form) {
            mEvent = data.evt;
            mShape = data.shape;
            mImage = data.img;
            mSurface = surface;
            mForm = form;

            mFont = new Font(FontFamily.GenericMonospace, 30f, FontStyle.Bold);

            mSurface.OnChange += SurfaceChanged;
        }

        private void SurfaceChanged(PointSurface surface) {
            if (mSurface.X > mShape.Left && mSurface.X < mShape.Right && mSurface.Y > mShape.Top && mSurface.Y < mShape.Bottom) {
                if (!mSelected) {
                    mSelected = true;
                    mFirstSelected = DateTime.Now;
                }

                if (!mTriggered && DateTime.Now.Subtract(mFirstSelected).TotalMilliseconds > SELECT_TIME) {
                    mEvent(mForm, mSurface);
                    if (Triggered != null)
                        Triggered();
                    mTriggered = true;
                }
            } else {
                mSelected = false;
                mTriggered = false;
            }
        }

        public PictureBox MakePictureBox(Panel container) {
            mContainer = container;

            mPictureBox = new PictureBox();
            mPictureBox.Location = new System.Drawing.Point((int) (mShape.Left * container.Width), (int) (mShape.Top * container.Height));
            mPictureBox.Name = "pictureBox";
            mPictureBox.Size = new System.Drawing.Size((int) (mShape.Width * container.Width), (int) (mShape.Height * container.Height));
            mPictureBox.Paint += new PaintEventHandler(mPictureBox_Paint);
            mPictureBox.MouseMove += new MouseEventHandler(mPictureBox_MouseMove);
            mImageResized = new Bitmap(mImage, mPictureBox.Size);
            mPictureBox.Image = mImageResized;

            container.Resize += new EventHandler(container_Resize);

            return mPictureBox;
        }

        private void container_Resize(object sender, EventArgs e) {
            mPictureBox.Location = new System.Drawing.Point((int) (mShape.Left * mContainer.Width), (int) (mShape.Top * mContainer.Height));
            mPictureBox.Size = new System.Drawing.Size((int) (mShape.Width * mContainer.Width), (int) (mShape.Height * mContainer.Height));
            mImageResized = new Bitmap(mImage, mPictureBox.Size);
            mPictureBox.Image = mImageResized;
        }

        private void mPictureBox_Paint(object sender, PaintEventArgs e) {
            if (mSelected) {
                float xScale = (mSurface.X - Shape.Left) / Shape.Width;
                float yScale = (mSurface.Y - Shape.Top) / Shape.Height;

                int x = (int) (e.ClipRectangle.Width * xScale) - CURSOR_R;
                int y = (int) (e.ClipRectangle.Height * yScale) - CURSOR_R;

                int r = CURSOR_R * 2;
                e.Graphics.FillEllipse(Brushes.Red, x, y, r, r);
                if (!mTriggered) {
                    string seconds = ((int) (DateTime.Now.Subtract(mFirstSelected).TotalSeconds)).ToString();
                    e.Graphics.DrawString(seconds, mFont, Brushes.Black, (e.ClipRectangle.Width - r) / 2f, (e.ClipRectangle.Height - r) / 2f);
                }
            }
        }

        private void mPictureBox_MouseMove(object sender, MouseEventArgs e) {
            if (mSurface != null) {
                float localX = (float)e.X / (float)mPictureBox.Width;
                float localY = (float)e.Y / (float)mPictureBox.Height;
                //mSurface.OverridePosition(mShape.Left + (localX * mShape.Width), mShape.Top + (localY * mShape.Height));
            }
        }
    }
}

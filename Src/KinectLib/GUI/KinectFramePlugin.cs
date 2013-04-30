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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NuiLibDotNet;
using OpenMetaverse;

namespace Chimera.Kinect.GUI {
    public partial class KinectFramePlugin : UserControl, ISystemPlugin {
        private static readonly int R = 5;

        private Vector mHead;
        private Vector mHandR;
        private Vector mHandL;
        private Vector mWristR;
        private Vector mWristL;
        private Vector mElbowR;
        private Vector mElbowL;
        private Vector mShoulderR;
        private Vector mShoulderL;
        private Vector mShoulderC;
        private Vector mHipC;
        private Vector mHipR;
        private Vector mHipL;
        private Vector mKneeR;
        private Vector mKneeL;
        private Vector mAnkleR;
        private Vector mAnkleL;
        private Vector mFootR;
        private Vector mFootL;

        private Size mSize = new Size(R * 2, R * 2);
        private ChangeDelegate mKinectTick;
        private bool mEnabled;

        public KinectFramePlugin() {
            InitializeComponent();

            mKinectTick = new ChangeDelegate(Nui_Tick);
            Disposed += new EventHandler(KinectPanel_Disposed);
            HandleCreated += new EventHandler(KinectPanel_HandleCreated);
        }

        void KinectPanel_HandleCreated(object sender, EventArgs e) {
            mHead = Nui.joint(Nui.Head);
            mHandR = Nui.joint(Nui.Hand_Right);
            mHandL = Nui.joint(Nui.Hand_Left);
            mWristR = Nui.joint(Nui.Wrist_Right);
            mWristL = Nui.joint(Nui.Wrist_Left);
            mElbowR = Nui.joint(Nui.Elbow_Right);
            mElbowL = Nui.joint(Nui.Elbow_Left);
            mShoulderR = Nui.joint(Nui.Shoulder_Right);
            mShoulderL = Nui.joint(Nui.Shoulder_Left);
            mShoulderC = Nui.joint(Nui.Shoulder_Centre);
            mHipC = Nui.joint(Nui.Hip_Centre);
            mHipR = Nui.joint(Nui.Hip_Right);
            mHipL = Nui.joint(Nui.Hip_Left);
            mKneeR = Nui.joint(Nui.Knee_Right);
            mKneeL = Nui.joint(Nui.Knee_Left);
            mAnkleR = Nui.joint(Nui.Ankle_Right);
            mAnkleL = Nui.joint(Nui.Ankle_Left);
            mFootR = Nui.joint(Nui.Foot_Right);
            mFootL = Nui.joint(Nui.Foot_Left);
            Nui.Tick += mKinectTick;
        }

        void KinectPanel_Disposed(object sender, EventArgs e) {
            Close();
        }

        private void Nui_Tick() {
            bool depth = true;
            Image oldFrame = null;
            Invoke(new Action(() => {
                depth = depthFrameButton.Checked;
                oldFrame = frameImage.Image;
            }));
            if (mEnabled) {
                Bitmap frame = depth ? Nui.DepthFrame : Nui.ColourFrame;
                Func<Vector, Point> toP = v => {
                    Point p = depth ? Nui.SkeletonToDepth(v) : Nui.SkeletonToColour(v);
                    return new Point(p.X - R, p.Y - R);
                };

                if (Nui.HasSkeleton) {
                    using (Graphics g = Graphics.FromImage(frame)) {
                        using (Pen p = new Pen(Color.Red, R / 2)) {
                            g.DrawEllipse(p, new Rectangle(toP(mHead), mSize));
                            g.DrawEllipse(p, new Rectangle(toP(mHandL), mSize));
                            g.DrawEllipse(p, new Rectangle(toP(mHandR), mSize));
                            g.DrawEllipse(p, new Rectangle(toP(mWristL), mSize));
                            g.DrawEllipse(p, new Rectangle(toP(mWristR), mSize));
                            g.DrawEllipse(p, new Rectangle(toP(mElbowL), mSize));
                            g.DrawEllipse(p, new Rectangle(toP(mElbowL), mSize));
                            g.DrawEllipse(p, new Rectangle(toP(mShoulderL), mSize));
                            g.DrawEllipse(p, new Rectangle(toP(mShoulderR), mSize));
                            g.DrawEllipse(p, new Rectangle(toP(mShoulderC), mSize));
                            g.DrawEllipse(p, new Rectangle(toP(mHipC), mSize));
                            g.DrawEllipse(p, new Rectangle(toP(mHipL), mSize));
                            g.DrawEllipse(p, new Rectangle(toP(mHipR), mSize));
                            g.DrawEllipse(p, new Rectangle(toP(mKneeL), mSize));
                            g.DrawEllipse(p, new Rectangle(toP(mKneeR), mSize));
                            g.DrawEllipse(p, new Rectangle(toP(mAnkleL), mSize));
                            g.DrawEllipse(p, new Rectangle(toP(mAnkleR), mSize));
                            g.DrawEllipse(p, new Rectangle(toP(mFootL), mSize));
                            g.DrawEllipse(p, new Rectangle(toP(mFootR), mSize));
                        }
                    }
                }

                BeginInvoke(new Action(() => {
                    frameImage.Image = frame;
                    if (oldFrame != null)
                        oldFrame.Dispose();
                }));
            }
        }

        #region ISystemPlugin Members

        public void Init(Coordinator coordinator) { }

        #endregion

        #region IPlugin Members

        public new bool Enabled {
            get { return mEnabled; }
            set { mEnabled = value; }
        }

        public new event Action<IPlugin, bool> EnabledChanged;

        public UserControl ControlPanel {
            get { return this; }
        }

        public string State {
            get { throw new NotImplementedException(); }
        }

        public Util.ConfigBase Config {
            get { throw new NotImplementedException(); }
        }

        public void Close() {
            Nui.Tick -= mKinectTick;
        }

        public void Draw(Func<Vector3, Point> to2D, Graphics graphics, Action redraw) { }

        #endregion
    }
}

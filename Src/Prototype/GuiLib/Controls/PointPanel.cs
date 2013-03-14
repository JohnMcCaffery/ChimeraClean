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
using UtilLib;

namespace KinectLib {
    public partial class PointPanel : UserControl {
        private PointSurface mSurface;
        private KinectManager mManager;

        private bool guiChange, nuiChange;
        private bool initialised;

        public PointPanel() {
            InitializeComponent();
        }

        public void Init(KinectManager manager, PointSurface surface) {
            mManager = manager;

            windowPanel.Window = surface.Window;
            ////windowPanel.Window.Width = 20f;
            //windowPanel.Window.Height = 20f;
            //windowPanel.Window.ScreenPosition = Vector3.Zero;
            //windowPanel.Window.RotationOffset.LookAtVector = Vector3.UnitX;
            //mManager.KinectPosition = kinectPositionPanel.Value;
            //mManager.KinectRotation.LookAtVector = kinectRotationPanel.LookAtVector;
            kinectPositionPanel.Value = mManager.KinectPosition; 
            kinectRotationPanel.LookAtVector = mManager.KinectRotation.LookAtVector ;

            Vector3 lookat = pointDirPanel.LookAtVector;
            Vector3 start = pointStartPanel.Value;

            pointStartPanel.OnChange += GuiChange;
            pointDirPanel.OnChange += GuiChange;
            kinectPositionPanel.OnChange += GuiChange;
            kinectRotationPanel.OnChange += GuiChange;
            manager.PositionChange += () => GuiChange(manager, null);
            manager.KinectRotation.OnChange += GuiChange;

            mSurface = surface;

            guiChange = true;
            Nui.Tick += () => {
                if (!guiChange && !Disposing && !IsDisposed && Created) {
                    nuiChange = true;
                    Console.WriteLine(mManager.PointStart.X + ", " + mManager.PointStart.Y + "," + mManager.PointStart.Z + " --- " + "X: " + mSurface.X + " Y: " + mSurface.Y);
                    Invoke(new Action(() => {
                        pointStartPanel.Value = new Vector3(mManager.PointStart.X, mManager.PointStart.Y, mManager.PointStart.Z);
                        pointDirPanel.LookAtVector = new Vector3(mManager.PointDir.X, mManager.PointDir.Y, mManager.PointDir.Z);
                        Change(mSurface);
                    }));
                    nuiChange = false;
                }
            };

            Nui.Poll();
            mSurface.OnChange += Change;

            initialised = true;
            Change(mSurface);
            guiChange = false;
        }

        private void GuiChange(object source, EventArgs args) {
            if (!nuiChange) {
                guiChange = true;
                mManager.PointStart.Set(pointStartPanel.Value.X, pointStartPanel.Value.Y, pointStartPanel.Value.Z);
                mManager.PointDir.Set(pointDirPanel.LookAtVector.X, pointDirPanel.LookAtVector.Y, pointDirPanel.LookAtVector.Z);
                mManager.KinectRotation.LookAtVector = kinectRotationPanel.LookAtVector;
                Nui.Poll();
                Change(mSurface);
                guiChange = false;
            }
        }

        private void Change(PointSurface surface) {
            Action a = () => {
                xLabel.Text = "X: " + surface.X.ToString(".000");
                yLabel.Text = "Y: " + surface.Y.ToString(".000");
                topLeftXLabel.Text = "TopLeft X: " + surface.TopLeft.X.ToString(".000");
                topLeftYLabel.Text = "TopLeft Y: " + surface.TopLeft.Y.ToString(".000");
                topLeftZLabel.Text = "TopLeft Z: " + surface.TopLeft.Z.ToString(".000");
                planeNormalXLabel.Text = "Normal X: " + surface.Normal.X.ToString(".000");
                planeNormalYLabel.Text = "Normal Y: " + surface.Normal.Y.ToString(".000");
                planeNormalZLabel.Text = "Normal Z: " + surface.Normal.Z.ToString(".000");
                topXLabel.Text = "Top X: " + surface.Top.X.ToString(".000");
                topYLabel.Text = "Top Y: " + surface.Top.Y.ToString(".000");
                topZLabel.Text = "Top Z: " + surface.Top.Z.ToString(".000");
                sideXLabel.Text = "Side X: " + surface.Side.X.ToString(".000");
                sideYLabel.Text = "Side Y: " + surface.Side.Y.ToString(".000");
                sideZLabel.Text = "Side Z: " + surface.Side.Z.ToString(".000");
                intersectionXLabel.Text = "Intersection: " + surface.Intersection.X.ToString(".000");
                intersectionYLabel.Text = "Intersection: " + surface.Intersection.Y.ToString(".000");
                intersectionZLabel.Text = "Intersection: " + surface.Intersection.Z.ToString(".000");
                graphicBox.Refresh();
            };
            if (InvokeRequired)
                Invoke(a);
            else
                a();
        }

        private void graphicBox_Paint(object sender, PaintEventArgs e) {
            if (!initialised)
                return;

            Point centreP = new Point(e.ClipRectangle.Width / 2, e.ClipRectangle.Height / 2);
            float scaleX = e.ClipRectangle.Width / 6000f;
            float scaleY = e.ClipRectangle.Height / 6000f;

            //Window
            Point windowLeftP = new Point((int) (mSurface.TopLeft.X * scaleX) + centreP.X, centreP.Y - (int) (mSurface.TopLeft.Z * scaleY));
            Rotation windowRot = new Rotation(mSurface.Top);
            Vector3 windowRightV = mSurface.TopLeft + ((Vector3.UnitX * windowRot.Quaternion) * mSurface.W);
            Point windowRightP = new Point((int) (windowRightV.X * scaleX) + centreP.X, centreP.Y - (int) (windowRightV.Z * scaleY));
            e.Graphics.DrawLine(new Pen(Brushes.Green, 5f), windowLeftP, windowRightP);

            //Top
            Point topEndP = new Point((int) (windowLeftP.X + (mSurface.Top.X * scaleX)), (int) (windowLeftP.Y - (mSurface.Top.Z * scaleY)));
            e.Graphics.DrawLine(new Pen(Brushes.Red), windowLeftP, topEndP);

            //Kinect / Centre
            e.Graphics.FillEllipse(Brushes.Black, new Rectangle(new Point(centreP.X - 2, centreP.Y - 2), new Size(4, 4)));

            //Point start
            Point pointStartP = new Point((int) (mManager.PointStart.X * scaleX) + centreP.X, centreP.Y - (int) (mManager.PointStart.Z * scaleY));
            e.Graphics.FillEllipse(Brushes.Black, new Rectangle(new Point(pointStartP.X - 2, pointStartP.Y - 2), new Size(4, 4)));

            //Point line
            Vector3 pointLineEndV = (pointDirPanel.LookAtVector * Math.Max(e.ClipRectangle.Height, e.ClipRectangle.Width)) + pointStartPanel.Value;
            Point pointLineEndP = new Point((int) (pointLineEndV.X * scaleX) + centreP.X, centreP.Y - (int) (pointLineEndV.Z * scaleY));
            e.Graphics.DrawLine(Pens.Red, pointStartP, pointLineEndP);

            if (!float.IsNaN(mSurface.Intersection.X) && !float.IsNaN(mSurface.Intersection.Y) && !float.IsNaN(mSurface.Intersection.Z)) {
                //Intersection point
                Point intersectionP = new Point((int)(mSurface.Intersection.X * scaleX) + centreP.X, centreP.Y - (int)(mSurface.Intersection.Z * scaleY));
                e.Graphics.FillEllipse(Brushes.Red, new Rectangle(new Point(intersectionP.X - 5, intersectionP.Y - 5), new Size(10, 10)));
            }
        }
    }
}

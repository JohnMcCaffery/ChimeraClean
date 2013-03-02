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
        private Vector pointStart;
        private Vector pointDir;
        private KinectManager mManager;

        private bool guiChange, nuiChange;
        private bool initialised;

        public PointPanel() {
            InitializeComponent();
        }

        public void Init(KinectManager manager) {
            mManager = manager;

            windowPanel.Window.Width = 20f;
            windowPanel.Window.Height = 20f;
            windowPanel.Window.ScreenPosition = Vector3.Zero;
            windowPanel.Window.RotationOffset.LookAtVector = Vector3.UnitX;
            mManager.KinectPosition = kinectPositionPanel.Value;
            mManager.KinectRotation.LookAtVector = kinectRotationPanel.LookAtVector;

            Vector3 lookat = pointDirPanel.LookAtVector;
            Vector3 start = pointStartPanel.Value;
            pointDir = Vector.Create("LinePoint", lookat.X, lookat.Y, lookat.Z);
            pointStart = Vector.Create("LineDir", start.X, start.Y, start.Z);

            pointStartPanel.OnChange += GuiChange;
            pointDirPanel.OnChange += GuiChange;
            kinectPositionPanel.OnChange += GuiChange;
            kinectRotationPanel.OnChange += GuiChange;

            mSurface = new PointSurface(mManager, windowPanel.Window, pointDir, pointStart);

            guiChange = true;
            Nui.OnChange += () => {
                if (!guiChange && !Disposing && !IsDisposed) {
                    nuiChange = true;
                    Console.WriteLine(pointStart.X + ", " + pointStart.Y + "," + pointStart.Z + " --- " + "X: " + mSurface.X + " Y: " + mSurface.Y);
                    Invoke(new Action(() => {
                        pointStartPanel.Value = new Vector3(pointStart.X, pointStart.Y, pointStart.Z);
                        pointDirPanel.LookAtVector = new Vector3(pointStart.X, pointStart.Y, pointStart.Z);
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
                pointStart.Set(pointStartPanel.Value.X, pointStartPanel.Value.Y, pointStartPanel.Value.Z);
                pointDir.Set(pointDirPanel.LookAtVector.X, pointDirPanel.LookAtVector.Y, pointDirPanel.LookAtVector.Z);
                mManager.KinectRotation.LookAtVector = kinectRotationPanel.LookAtVector;
                mSurface = new PointSurface(mManager, windowPanel.Window, pointDir, pointStart);
                Nui.Poll();
                Change(mSurface);
                guiChange = false;
            }
        }

        private void Change(PointSurface surface) {
            Action a = () => {
                xLabel.Text = "X: " + surface.X;
                yLabel.Text = "Y: " + surface.Y;
                topLeftXLabel.Text = "TopLeft X: " + surface.TopLeft.X;
                topLeftYLabel.Text = "TopLeft Y: " + surface.TopLeft.Y;
                topLeftZLabel.Text = "TopLeft Z: " + surface.TopLeft.Z;
                planeNormalXLabel.Text = "Normal X: " + surface.Normal.X;
                planeNormalYLabel.Text = "Normal Y: " + surface.Normal.Y;
                planeNormalZLabel.Text = "Normal Z: " + surface.Normal.Z;
                topXLabel.Text = "Top X: " + surface.Top.X;
                topYLabel.Text = "Top Y: " + surface.Top.Y;
                topZLabel.Text = "Top Z: " + surface.Top.Z;
                sideXLabel.Text = "Side X: " + surface.Side.X;
                sideYLabel.Text = "Side Y: " + surface.Side.Y;
                sideZLabel.Text = "Side Z: " + surface.Side.Z;
                intersectionXLabel.Text = "Intersection: " + surface.Intersection.X;
                intersectionYLabel.Text = "Intersection: " + surface.Intersection.Y;
                intersectionZLabel.Text = "Intersection: " + surface.Intersection.Z;
                graphicBox.Refresh();
            };
            if (InvokeRequired)
                Invoke(a);
            else
                a();
        }

        private void initButton_Click(object sender, EventArgs e) {
                //Nui.Init();
            //Nui.SetAutoPoll(true);
        }

        private void graphicBox_Paint(object sender, PaintEventArgs e) {
            if (!initialised)
                return;

            Point centreP = new Point(e.ClipRectangle.Width / 2, e.ClipRectangle.Height / 2);
            float scaleX = e.ClipRectangle.Width / 60f;
            float scaleY = e.ClipRectangle.Height / 60f;

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
            Point pointStartP = new Point((int) (pointStart.X * scaleX) + centreP.X, centreP.Y - (int) (pointStart.Z * scaleY));
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

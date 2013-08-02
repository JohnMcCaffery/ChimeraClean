using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Experimental.GUI;
using System.Drawing;
using OpenMetaverse;
using System.Xml;
using Chimera.Overlay;
using System.IO;
using Chimera.Util;

namespace Chimera.Experimental.Plugins {
    public enum State { Ready, Running, Finished, Nothing, Prepped }
    public class MovementTracker : XmlLoader, ISystemPlugin {
        private readonly HashSet<Action> mRedraws = new HashSet<Action>();

        private List<Vector3> mStarts;
        private List<Vector3> mTargets;
        private Vector2[] mStartVectors;
        private Vector2[] mTargetVectors;

        private MovementTrackerControl mControl;
        private Core mCore;
        private Bitmap mMap;
        private bool mEnabled;

        private Vector3 mFarCorner = new Vector3(0f, 256f, 100f);
        private float mScaleX = 2.5f;
        private float mScaleY = 3f;
        private float mLeftScaleX;
        private float mLeftScaleY;
        private float mR = 2.5f;

        private bool mPrep;
        private State mState = Plugins.State.Nothing;
        private DateTime mStart;
        private DateTime mFinish;

        private int mUpdate = 0;
        private const int REDRAW = 5;

        private Action mTickListener;

        public event Action TimeChanged;

        public event Action StateChanged;

        private List<Vector3> mRoute = new List<Vector3>();

        public TimeSpan Time {
            get { return mState == Plugins.State.Running ? DateTime.Now.Subtract(mStart) : mFinish.Subtract(mStart); }
        }

        public bool Prep {
            get { return mPrep; }
            set { 
                mPrep = value;
                if (mState == Plugins.State.Ready)
                    mState = Plugins.State.Prepped;
            }
        }

        public State ExperimentState {
            get { return mState; }
        }

        void mCore_CameraUpdated(Core core, CameraUpdateEventArgs args) {
            mUpdate++;

            if (Algorithms.PolygonContains(new Vector2(mCore.Position.X, mCore.Position.Y), mStartVectors)) {
                mState = mPrep ? Plugins.State.Prepped : Plugins.State.Ready;
                if (StateChanged != null)
                    StateChanged();
            } else {
                if (mState == Plugins.State.Prepped)
                    OnStart();
                else if (mState == Plugins.State.Ready) {
                    mState = Plugins.State.Nothing;
                    if (StateChanged != null)
                        StateChanged();
                }

                if (mState == Plugins.State.Running) {
                    if (Algorithms.PolygonContains(new Vector2(mCore.Position.X, mCore.Position.Y), mTargetVectors))
                        OnFinish();
                    else if (mRoute.Count < 1 || mRoute[mRoute.Count - 1] != mCore.Position)
                        mRoute.Add(mCore.Position);
                }
            }

            if (mUpdate == REDRAW) {
                mUpdate = 0;
                foreach (var redraw in mRedraws)
                    redraw();
            }
        }

        void mCore_Tick() {
            if (TimeChanged != null)
                TimeChanged();
        }

        public void Init(Core core) {
            mCore = core;
            mCore.CameraUpdated += new Action<Core, CameraUpdateEventArgs>(mCore_CameraUpdated);
            mTickListener = new Action(mCore_Tick);

            ExperimentalConfig cfg = new ExperimentalConfig();
            if (cfg.ExperimentFile != null && File.Exists(cfg.ExperimentFile))
                Load(cfg.ExperimentFile);
            else {
                mMap = new Bitmap("Images/Maps/OrthogonalMap.png");
                mLeftScaleX = (mScaleX - 1f) / 2f;
                mLeftScaleY = (mScaleY - 1f) / 2f;
            }
        }

        public void Load(string file) {
            XmlDocument doc = new XmlDocument();
            doc.Load(file);

            XmlNode root = doc.GetElementsByTagName("Experiment")[0];

            XmlNode mapNode = root.SelectNodes("child::Map").OfType<XmlElement>().FirstOrDefault();
            if (mapNode != null) {
                mMap = new Bitmap(GetText(mapNode, "Images/Maps/OrthogonalMap.png"));
                mFarCorner = GetVector(mapNode, mFarCorner, "FarCorner");
                mScaleX = GetFloat(mapNode, mScaleX, "ScaleX");
                mScaleY = GetFloat(mapNode, mScaleY, "ScaleY");
            } else {
                mMap = new Bitmap("Images/Maps/OrthogonalMap.png");
            }

            mLeftScaleX = (mScaleX - 1f) / 2f;
            mLeftScaleY = (mScaleY - 1f) / 2f;

            mStarts = new List<Vector3>();
            foreach (var node in GetChildrenOfChild(root, "Start"))
                mStarts.Add(GetVector(node, Vector3.Zero));

            mTargets = new List<Vector3>();
            foreach (var node in GetChildrenOfChild(root, "Target"))
                mTargets.Add(GetVector(node, Vector3.Zero));

            mStartVectors = mStarts.Select(v => new Vector2(v.X, v.Y)).ToArray();
            mTargetVectors = mTargets.Select(v => new Vector2(v.X, v.Y)).ToArray();
        }

        public void SetForm(System.Windows.Forms.Form form) { }

        public event Action<IPlugin, bool> EnabledChanged;

        public System.Windows.Forms.Control ControlPanel {
            get {
                if (mControl == null)
                    mControl = new MovementTrackerControl(this);
                return mControl;
            }
        }

        public bool Enabled {
            get {
                return mEnabled;
            }
            set {
                if (mEnabled != value) {
                    mEnabled = value;
                    if (EnabledChanged != null)
                        EnabledChanged(this, value);
                }
            }
        }

        public string Name {
            get { return "MovementTracker"; }
        }

        public string State {
            get { throw new NotImplementedException(); }
        }

        public Config.ConfigBase Config {
            get { throw new NotImplementedException(); }
        }

        public void Close() {
            mMap.Dispose();
        }

        public void Draw(Graphics graphics, Func<Vector3, Point> to2D, Action redraw, Perspective perspective) {
            if (perspective == Perspective.Map) {
                if (!mRedraws.Contains(redraw))
                    mRedraws.Add(redraw);

                Point bottomRight = to2D(mFarCorner);
                graphics.DrawImage(mMap, -bottomRight.X * mLeftScaleX, -bottomRight.Y * mLeftScaleY, bottomRight.X * mScaleX, bottomRight.Y * mScaleY);

                if (mState == Plugins.State.Ready || mState == Plugins.State.Prepped) {
                    graphics.FillPolygon(Brushes.Blue, mStarts.Select(v => GetPoint(v, to2D, bottomRight)).ToArray());
                    graphics.DrawPolygon(Pens.Red, mTargets.Select(v => GetPoint(v, to2D, bottomRight)).ToArray());
                } else {
                    graphics.DrawPolygon(Pens.Red, mStarts.Select(v => GetPoint(v, to2D, bottomRight)).ToArray());
                    if (mState == Plugins.State.Finished)
                        graphics.FillPolygon(Brushes.Blue, mTargets.Select(v => GetPoint(v, to2D, bottomRight)).ToArray());
                    else
                        graphics.DrawPolygon(Pens.Red, mTargets.Select(v => GetPoint(v, to2D, bottomRight)).ToArray());

                    if (mRoute.Count > 1)
                        graphics.DrawLines(Pens.Red, mRoute.Select(v => GetPoint(v, to2D, bottomRight)).ToArray());
                }


                Point location = GetPoint(mCore.Position, to2D, bottomRight);
                graphics.FillEllipse(Brushes.Red, location.X - mR, location.Y - mR, mR * 2, mR * 2);

                Point lookAt = GetPoint(mCore.Position + mCore.Orientation.LookAtVector, to2D, bottomRight);
                graphics.DrawLine(Pens.Green, location, lookAt);
            }
        }

        private void OnStart() {
            mState = Plugins.State.Running;
            mRoute.Clear();
            mStart = DateTime.Now;
            mCore.Tick += mTickListener;
            mPrep = false;
            if (StateChanged != null)
                StateChanged();
        }

        private void OnFinish() {
            mCore.Tick -= mTickListener;
            mFinish = DateTime.Now;
            mState = Plugins.State.Finished;
            if (StateChanged != null)
                StateChanged();
            if (TimeChanged != null)
                TimeChanged();
        }

        private Point GetPoint(Vector3 original, Func<Vector3, Point> to2D, Point bottomRight) {
            Point point = to2D(original);
            point.X = (int)(point.X * mScaleX);
            point.Y = (int)(point.Y * mScaleY);
            point.X -= (int)(bottomRight.X * mLeftScaleX);
            point.Y -= (int)(bottomRight.Y * mLeftScaleY);

            return point;
        }
    }
}

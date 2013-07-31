using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Experimental.GUI;
using System.Drawing;
using OpenMetaverse;

namespace Chimera.Experimental.Plugins {
    public class MovementTracker : ISystemPlugin {
        private readonly HashSet<Action> mRedraws = new HashSet<Action>();

        private MovementTrackerControl mControl;
        private Core mCore;
        private Bitmap mMap;
        private bool mEnabled;

        void mCore_CameraUpdated(Core core, CameraUpdateEventArgs args) {
            foreach (var redraw in mRedraws)
                redraw();
        }

        public void Init(Core core) {
            mCore = core;
            mCore.CameraUpdated += new Action<Core,CameraUpdateEventArgs>(mCore_CameraUpdated);

            mMap = new Bitmap("Images/Maps/OrthogonalMap.png");
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

                Point bottomRight = to2D(new Vector3(0f, 256f, 100f));
                graphics.DrawImage(mMap, -bottomRight.X * .75f, -bottomRight.Y, bottomRight.X * 2.5f, bottomRight.Y * 3f);
                //graphics.DrawImage(mMap, 0f, 0f, bottomRight.X * 2, bottomRight.Y * 2);
                float r = 2.5f;
                Point location = to2D(mCore.Position);
                location.X = (int) (location.X * 2.5);
                location.Y *= 3;
                location.X -= (int) (bottomRight.X * .75);
                location.Y -= bottomRight.Y;
                //graphics.FillEllipse(Brushes.Red, location.X - r, location.Y - r, r * 2, r * 2);
                graphics.FillEllipse(Brushes.Red, location.X - r, location.Y - r, r * 2, r * 2);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using Chimera.Config;
using System.Xml;
using System.Drawing;
using Chimera.Plugins;
using System.Windows.Forms;
using Chimera.GUI.Controls.Plugins;

namespace Chimera.Plugins {
    public class ProjectorPlugin : ISystemPlugin {
        private readonly HashSet<Action> mRedraws = new HashSet<Action>();
        private readonly List<Vector3> sCorners = new List<Vector3>();

        private Vector3 mAnchor;
        private Vector3 mBig = new Vector3(float.MinValue, float.MinValue, float.MinValue);
        private Vector3 mSmall = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        private bool mEnabled;
        private bool mDrawLabels;
        private bool mDrawRoom;
        private ProjectorPluginPanel mPanel;

        public Vector3 Big {
            get { return mBig; }
        }

        public Vector3 Small {
            get { return mSmall; }
        }

        public IEnumerable<Vector3> Corners {
            get { return sCorners; }
        }

        public IEnumerable<Vector3> InternalCorners {
            get { return sCorners; }
        }

        public Vector3 RoomPosition {
            get { return mAnchor; }
            set { 
                mAnchor = value;
                if (RoomChanged != null)
                    RoomChanged();
                Redraw();
            }
        }

        public bool DrawLabels {
            get { return mDrawLabels; }
            set {
                mDrawLabels = value;
                Redraw();
            }
        }

        public bool DrawRoom {
            get { return mDrawRoom; }
            set {
                mDrawRoom = value;
                Redraw();
            }
        }

        internal void Redraw() {
            foreach (var redraw in mRedraws)
                redraw();

            if (Change != null)
                Change();
        }

        void coordinator_WindowAdded(Window window, EventArgs args) {
            Projector p = new Projector(window, this);
            mProjectors.Add(p);
            if (ProjectorAdded != null)
                ProjectorAdded(p);
        }

        private readonly List<Projector> mProjectors = new List<Projector>();

        public Projector[] Projectors {
            get { return mProjectors.ToArray(); }
        }

        public event Action RoomChanged;

        public event Action<Projector> ProjectorAdded;

        public event Action Change;

        #region ISystemPlugin Members

        public void Init(Coordinator coordinator) {
            ProjectorConfig config = new ProjectorConfig();
            mDrawLabels = config.DrawGlobalLabels;
            mDrawRoom = config.DrawRoom;

            string roomFile = config.RoomFile;
            if (roomFile != null) {
                XmlDocument doc = new XmlDocument();
                doc.Load(roomFile);

                foreach (XmlElement cornerNode in doc.GetElementsByTagName("Room")[0].ChildNodes.OfType<XmlElement>()) {
                    XmlAttribute xAttr = cornerNode.Attributes["X"];
                    XmlAttribute yAttr = cornerNode.Attributes["Y"];
                    XmlAttribute zAttr = cornerNode.Attributes["Z"];

                    float x, y, z;
                    x = y = z = 0f;

                    if (xAttr != null)
                        float.TryParse(xAttr.Value, out x);
                    if (yAttr != null)
                        float.TryParse(yAttr.Value, out y);
                    if (yAttr != null)
                        float.TryParse(zAttr.Value, out z);

                    sCorners.Add(new Vector3(x, y, z));

                    if (mBig.X < x) mBig.X = x;
                    if (mBig.Y < y) mBig.Y = y;
                    if (mBig.Z < z) mBig.Z = z;

                    if (mSmall.X > x) mSmall.X = x;
                    if (mSmall.Y > y) mSmall.Y = y;
                    if (mSmall.Z > z) mSmall.Z = z;
                }

                mAnchor = config.RoomAnchor;
            }

            coordinator.WindowAdded += new Action<Window,EventArgs>(coordinator_WindowAdded);
            foreach (var window in coordinator.Windows)
                coordinator_WindowAdded(window, null);
        }

        public event Action<IPlugin, bool> EnabledChanged;

        public UserControl ControlPanel {
            get {
                if (mPanel == null)
                    mPanel = new ProjectorPluginPanel(this);
                return mPanel; 
            }
        }

        public bool Enabled {
            get { return mEnabled; }
            set {
                if (mEnabled != value) {
                    mEnabled = value;
                    if (EnabledChanged != null)
                        EnabledChanged(this, value);
                }
            }
        }

        public string Name {
            get { return "Projectors"; }
        }

        public string State {
            get { throw new NotImplementedException(); }
        }

        public Config.ConfigBase Config {
            get { throw new NotImplementedException(); }
        }

        public void Close() { }

        public void Draw(Graphics g, Func<Vector3, Point> to2D, Action redraw, Perspective perspective) {
            if (!mRedraws.Contains(redraw))
                mRedraws.Add(redraw);
            
            if (!mEnabled)
                return;

            if (mDrawRoom) {
                g.DrawPolygon(Pens.Black, sCorners.Select(v => to2D(v + mAnchor)).ToArray());
                if (mDrawLabels) {
                    Font font = SystemFonts.DefaultFont;

                    Vector3 edgeB = Big + RoomPosition;
                    Vector3 edgeS = Small + RoomPosition;
                    Vector3 centre = edgeS + (Size / 2f);

                    string w = String.Format("Width: {0:.#}", Size.Y);
                    string h = String.Format("Height: {0:.#}", Size.Z);
                    string d = String.Format("Depth: {0:.#}", Size.X);

                    if (perspective == Perspective.X) {
                        PH(g, new Vector3(0f, edgeB.Y, centre.Z), h, to2D, font);
                        PW(g, new Vector3(0f, centre.Y, edgeS.Z), w, to2D, font);
                    }
                    if (perspective == Perspective.Y) {
                        PH(g, new Vector3(edgeS.X, 0f, centre.Z), h, to2D, font);
                        PW(g, new Vector3(centre.X, 0f, edgeS.Z), d, to2D, font);
                    }
                    if (perspective == Perspective.Z) {
                        PH(g, new Vector3(centre.X, edgeB.Y, 0f), d, to2D, font);
                        PW(g, new Vector3(edgeS.X, centre.Y, 0f), w, to2D, font);
                    }
                }
            }

            foreach (var projector in mProjectors.Where(p => p.DrawDiagram))
                projector.Draw(g, to2D, redraw, perspective);
        }

        #endregion


        private void PH(Graphics g, Vector3 v, string txt, Func<Vector3, Point> to2D, Font font) {
            Point p = to2D(v);
            SizeF s = g.MeasureString(txt, font);
            p.Y -= (int)(s.Height / 2f);
            g.DrawString(txt, font, Brushes.Black, p);
        }
        private void PW(Graphics g, Vector3 v, string txt, Func<Vector3, Point> to2D, Font font) {
            Point p = to2D(v);
            SizeF s = g.MeasureString(txt, font);
            p.X -= (int)(s.Width / 2f);
            g.DrawString(txt, font, Brushes.Black, p);
        }

        public Vector3 Size { get { return mBig - mSmall; } }
    }
}

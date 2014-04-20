using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Overlay;
using Chimera.OpenSim;
using Chimera.Experimental.GUI;
using OpenMetaverse;
using System.Xml;
using System.IO;
using log4net;

namespace Chimera.Experimental.Plugins {
    public class AvatarMovementPlugin : XmlLoader, ISystemPlugin {
        private ILog Logger = LogManager.GetLogger("AvatarMovementPlugin");
        private bool mEnabled;
        private Dictionary<string, OpenSimController> mController = new Dictionary<string,OpenSimController>();
        private OpenSimController mMainController;
        private Core mCore;
        private ExperimentalConfig mConfig = new ExperimentalConfig();
        private AvatarMovementControl mPanel;
        private string mTargetsFile, mNodesFile;

        private List<Vector3> mTargets = new List<Vector3>();

        private double mYawRate = .2;
        private double mPitchRate = .2;

        public string NodesFile {
            get { return mNodesFile; }
            set {
                mNodesFile = value;
                LoadTargets();
            }
        }

        public string TargetsFile {
            get { return mTargetsFile; }
            set {
                mTargetsFile = value;
                LoadTargets();
            }
        }

        private void LoadTargets() {
            if (mTargetsFile == null || mNodesFile == null)
                return;

            if (!File.Exists(mTargetsFile) && !File.Exists(mNodesFile)) {
                Logger.Warn("Unable to load targets. Neither targets file or nodes file exists.");
                return;
            } else if (!File.Exists(mTargetsFile)) {
                Logger.Warn("Unable to load targets. Targets file does not exist.");
                return;
            } else if (!File.Exists(mNodesFile)) {
                Logger.Warn("Unable to load targets. Nodes file does not exist.");
                return;
            }

            XmlDocument nodesDoc = new XmlDocument();
            nodesDoc.Load(mNodesFile);

            XmlDocument targetsDoc = new XmlDocument();
            nodesDoc.Load(mTargetsFile);

            mTargets.Clear();
            foreach (var targetNode in targetsDoc.GetElementsByTagName("Target").OfType<XmlElement>()) {
                var nameStr = targetNode.Attributes["name"].Value;
                Vector3 target = Vector3.Zero;

                foreach (var node in nodesDoc.GetElementsByTagName("node").OfType<XmlElement>()) {
                    if (node.InnerText == nameStr) {
                        XmlNode x = node.NextSibling;
                        XmlNode y = node.NextSibling;
                        XmlNode z = node.NextSibling;

                        target.X = float.Parse(x.InnerXml);
                        target.Y = float.Parse(y.InnerXml);
                        target.Z = float.Parse(z.InnerXml);

                        break;
                    }
                }

                mTargets.Add(target);
            }
        }

        public void Start() {
            foreach (var target in mTargets) {
            }
        }

        private void GoToTarget(Vector3 target) {
        }

        #region ISystemPlugin Members

        public void Init(Core core) {
            mCore = core;
            mMainController = mCore.GetPlugin<OpenSimController>();
            foreach (var controller in core.Frames.Select(f => f.Output as OpenSimController))
                mController.Add(controller.Frame.Name, controller);
        }

        public void SetForm(System.Windows.Forms.Form form) { }

        #endregion

        #region IPlugin Members

        public event Action<IPlugin, bool> EnabledChanged;

        public System.Windows.Forms.Control ControlPanel {
            get {
                if (mPanel == null)
                    mPanel = new AvatarMovementControl(this);
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
            get { return "AvatarMovementPlugin"; }
        }

        public string State {
            get { throw new NotImplementedException(); }
        }

        public Config.ConfigBase Config {
            get { return mConfig; }
        }

        public void Close() { }

        public void Draw(System.Drawing.Graphics graphics, Func<OpenMetaverse.Vector3, System.Drawing.Point> to2D, Action redraw, Perspective perspective) { }

        #endregion
    }
}

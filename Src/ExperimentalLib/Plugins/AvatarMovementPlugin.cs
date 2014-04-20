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
using Chimera.Util;

namespace Chimera.Experimental.Plugins {
    public class AvatarMovementPlugin : XmlLoader, ISystemPlugin {
        private ILog Logger = LogManager.GetLogger("AvatarMovementPlugin");
        private bool mEnabled;
        private Dictionary<string, OpenSimController> mController = new Dictionary<string,OpenSimController>();
        private OpenSimController mMainController;
        private Core mCore;
        private ExperimentalConfig mConfig = new ExperimentalConfig();
        private AvatarMovementControl mPanel;
        private Action mTickListener;

        private List<KeyValuePair<string, Vector3>> mTargets = new List<KeyValuePair<string, Vector3>>();
        private KeyValuePair<string, Vector3> mTarget;
        private int mTargetIndex = 0;

        public string NodesFile {
            get { return mConfig.NodesFile; }
            set {
                mConfig.NodesFile = value;
                LoadTargets();
            }
        }

        public string TargetsFile {
            get { return mConfig.TargetsFile; }
            set {
                mConfig.TargetsFile = value;
                LoadTargets();
            }
        }

        public AvatarMovementPlugin() {
            mTickListener = new Action(mCore_Tick);
        }

        private void LoadTargets() {
            if (mConfig.TargetsFile == null || mConfig.NodesFile == null)
                return;

            if (!File.Exists(mConfig.TargetsFile) && !File.Exists(mConfig.NodesFile)) {
                Logger.Warn("Unable to load targets. Neither targets file or nodes file exists.");
                return;
            } else if (!File.Exists(mConfig.TargetsFile)) {
                Logger.Warn("Unable to load targets. Targets file does not exist.");
                return;
            } else if (!File.Exists(mConfig.NodesFile)) {
                Logger.Warn("Unable to load targets. Nodes file does not exist.");
                return;
            }

            XmlDocument nodesDoc = new XmlDocument();
            nodesDoc.Load(mConfig.NodesFile);

            XmlDocument targetsDoc = new XmlDocument();
            targetsDoc.Load(mConfig.TargetsFile);

            mTargets.Clear();
            foreach (var targetNode in targetsDoc.GetElementsByTagName("Target").OfType<XmlElement>()) {
                var nameStr = targetNode.Attributes["name"].Value;

                foreach (var node in nodesDoc.GetElementsByTagName("name").OfType<XmlElement>()) {
                    if (node.InnerText == nameStr) {
                        XmlNode x = node.NextSibling;
                        XmlNode y = x.NextSibling;
                        XmlNode z = y.NextSibling;

                        Vector3 target = Vector3.Zero;

                        target.X = float.Parse(x.InnerXml);
                        target.Y = float.Parse(y.InnerXml);
                        target.Z = float.Parse(z.InnerXml);

                        mTargets.Add(new KeyValuePair<string, Vector3>(nameStr, target));
                        break;
                    }
                }

            }
        }

        public event Action<string, Vector3> TargetChanged;

        public void Start() {
            if (mTargets.Count > 0) {
                mTargetIndex = 0;
                mTarget = mTargets[mTargetIndex];
                mTurn = new Rotation(mTarget.Value - mCore.Position);
                if (TargetChanged != null)
                    TargetChanged(mTarget.Key, mTarget.Value);
                mCore.Tick += new Action(mCore_Tick);
            }

        }

        private Rotation mTurn;

        private bool AreClose(double a, double b) {
            return Math.Abs(a - b) < .0001;
        }

        void mCore_Tick() {
            if (!AreClose(mCore.Orientation.Yaw, mTurn.Yaw) || !AreClose(mCore.Orientation.Pitch, mTurn.Pitch)) {
                Rotation delta = mTurn - mCore.Orientation;
                delta.Pitch = delta.Pitch >= 0 ? 1 : -1 * Math.Min(mConfig.PitchRate, Math.Abs(delta.Pitch));
                delta.Yaw = delta.Yaw >= 0 ? 1 : -1 * Math.Min(mConfig.YawRate, Math.Abs(delta.Yaw));

                mCore.Update(mCore.Position, Vector3.Zero, mCore.Orientation + delta, delta);
                return;
            } else if ((mTarget.Value - mCore.Position).Length() > .5f) {
                Vector3 delta = mTarget.Value - mCore.Position;
                if (delta.Length() > mConfig.MoveRate)
                    delta *= mConfig.MoveRate / delta.Length();

                mCore.Update(mCore.Position + delta, delta, mCore.Orientation, Rotation.Zero);
            } else {
                mCore.Update(mCore.Position, Vector3.Zero, mCore.Orientation, Rotation.Zero);
                if (++ mTargetIndex< mTargets.Count) {
                    mTarget = mTargets[mTargetIndex];
                    mTurn = new Rotation(mTarget.Value - mCore.Position);
                    if (TargetChanged != null)
                        TargetChanged(mTarget.Key, mTarget.Value);
                } else {
                    Logger.Info("Finished walking route.");
                    mCore.Tick -= mTickListener;
                }
            }
        }


        #region ISystemPlugin Members

        public void Init(Core core) {
            mCore = core;
            mMainController = mCore.GetPlugin<OpenSimController>();
            foreach (var controller in core.Frames.Select(f => f.Output as OpenSimController))
                mController.Add(controller.Frame.Name, controller);

            LoadTargets();
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
                    if (!value)
                        mCore.Tick -= mTickListener;
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

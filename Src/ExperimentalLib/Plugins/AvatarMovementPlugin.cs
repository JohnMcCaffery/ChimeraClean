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
                //Turn = new Rotation(Target - Position);
                if (TargetChanged != null)
                    TargetChanged(mTarget.Key, Target);
                mCore.Tick += new Action(mCore_Tick);
            }

        }

        private Vector3 Target {
            get { return mTarget.Value + new Vector3(0f, 0f, mConfig.HeightOffset); }
        }

        private Vector3 Position {
            get { return mCore.ControlMode == ControlMode.Absolute ? mCore.Position : mMainController.AvatarPosition; }
        }

        private Rotation Orientation {
            get { return mCore.ControlMode == ControlMode.Absolute ? mCore.Orientation : mMainController.AvatarOrientation; }
        }

        private Rotation Turn {
            get { return new Rotation(Target - Position); }
        }

        private bool mRotationComplete = false;

        void mCore_Tick() {
            if (!AtTarget()) {
                if (!mRotationComplete && !RotatedToTarget()) {
                    Rotation delta = Turn - Orientation;
                    if (Math.Abs(delta.Pitch) > mConfig.PitchRate)
                        delta.Pitch = (delta.Pitch >= 0 ? 1 : -1) * mConfig.PitchRate;
                    if (Math.Abs(delta.Yaw) > mConfig.YawRate)
                        delta.Yaw = (delta.Yaw >= 0 ? 1 : -1) * mConfig.YawRate;

                    mCore.Update(mCore.Position, Vector3.Zero, mCore.Orientation + delta, delta);

                    //mCore.Update(mCore.Position, Vector3.Zero, mTurn, delta);
                    return;
                } else {
                    mRotationComplete = true;
                    Vector3 delta = Target - Position;
                    if (delta.Length() > mConfig.MoveRate)
                        delta *= mConfig.MoveRate / delta.Length();

                    if (mCore.ControlMode == ControlMode.Absolute)
                        mCore.Update(mCore.Position + delta, delta, mCore.Orientation, Rotation.Zero);
                    else
                        mCore.Update(mCore.Position + delta, new Vector3(mConfig.MoveRate, 0f, 0f), mCore.Orientation, Rotation.Zero);
                }
            } else {
                mCore.Update(mCore.Position, Vector3.Zero, mCore.Orientation, Rotation.Zero);
                mRotationComplete = false;
                if (++mTargetIndex < mTargets.Count) {
                    mTarget = mTargets[mTargetIndex];
                    //Turn = new Rotation(Target - mCore.Position);
                    if (TargetChanged != null)
                        TargetChanged(mTarget.Key, mTarget.Value);
                } else {
                    Logger.Info("Finished walking route.");
                    mCore.Tick -= mTickListener;
                }
            }
        }

        private bool RotatedToTarget() {
            Console.Write("Yaw: " + (Orientation.Yaw - Turn.Yaw) + " - Pitch: " + (Orientation.Pitch - Turn.Pitch));
            bool ret = AreClose(Orientation.Yaw, Turn.Yaw) && AreClose(Orientation.Pitch, Turn.Pitch);
            Console.WriteLine();
            return ret;
        }

        private bool AtTarget() {
            //Vector3 delta = (Target - Position);
            //Console.WriteLine(delta + "    " + delta.Length());
            return (Target - Position).Length() < mConfig.DistanceThreshold;
        }
        private bool AreClose(double a, double b) {
            Console.Write(" Diff: " + Math.Abs(a - b) + " - ret: " + (Math.Abs(a - b) < TargetAccuracy));
            return Math.Abs(a - b) < TargetAccuracy;
        }

        private double TargetAccuracy {
            get { return mCore.ControlMode == ControlMode.Absolute ? .0000000001 : .4; }
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

        internal void Stop() {
            mCore.Tick -= mTickListener;
            mCore.Update(mCore.Position, Vector3.Zero, mCore.Orientation, Rotation.Zero);
        }
    }
}

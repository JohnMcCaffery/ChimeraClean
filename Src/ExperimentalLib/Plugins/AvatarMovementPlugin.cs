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
using System.Windows.Forms;
using System.Threading;

namespace Chimera.Experimental.Plugins {
    public class AvatarMovementPlugin : XmlLoader, ISystemPlugin {
        private ILog Logger = LogManager.GetLogger("AvatarMovementPlugin");
        private bool mEnabled;
        private OpenSimController mMainController;
        private Core mCore;
        private ExperimentalConfig mConfig = new ExperimentalConfig();
        private AvatarMovementControl mPanel;
        private Action mTickListener;
        private Form mForm;

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
            Action start = () => {
                if (mTargets.Count > 0) {
                    Logger.Info("Starting loop.");

                    if (mConfig.SaveResults)
                        mConfig.SetupFPSLogs(mCore, mCore.ControlMode + "-", Logger);

                    mTargetIndex = 0;
                    mTarget = mTargets[mTargetIndex];
                    if (TargetChanged != null)
                        TargetChanged(mTarget.Key, Target);
                    mCore.Tick += new Action(mCore_Tick);
                } else {
                    Logger.Info("No targets loaded. Unable to start loop.");
                }
            };

            mCore.ControlMode = mConfig.Mode;
            if (mConfig.StartAtHome)
                mMainController.ViewerController.PressKey("H", true, false, true);

            if (mConfig.StartWaitMS > 0) {
                Thread t = new Thread(() => {
                Logger.Info("Waiting " + mConfig.StartWaitMS + "MS before starting loop.");
                    Thread.Sleep(mConfig.StartWaitMS);
                    start();
                });
                t.Name = "WalkBotStartWait";
                t.Start();
            } else
                start();
        }

        private Vector3 Target {
            get { 
                return mCore.ControlMode == ControlMode.Absolute ?
                    mTarget.Value + new Vector3(0f, 0f, mConfig.HeightOffset) :
                    new Vector3(mTarget.Value.X, mTarget.Value.Y, 0f); 
            }
        }

        private Vector3 Position {
            //get { return mCore.ControlMode == ControlMode.Absolute ? mCore.Position : mMainController.AvatarPosition; }
            get { 
                Vector3 ret = mCore.Position;
                if (mCore.ControlMode == ControlMode.Delta) {
                    ret += mMainController.PositionOffset;
                    ret.Z = 0f;
                }
                return ret;
            }
        }

        private Rotation Orientation {
            get { return mCore.Orientation; }
        }

        private Rotation Turn {
            get { return new Rotation(Target - Position); }
        }

        private Rotation GetRotationDelta(out bool move) {
            Rotation delta = Turn - Orientation;
            move = Math.Abs(delta.Yaw) < TargetAccuracy;
            delta.Yaw = GetDelta(delta.Yaw);
            //delta.Pitch = GetDelta(delta.Pitch);
            delta.Pitch = 0;
            return delta;
        }

        private double GetDelta(double delta) {
            if (delta == 0)
                return 0;

            double slowStart = 4;
            double sign = (delta >= 0 ? 1 : -1);
            double absDelta = Math.Abs(delta);
            if (absDelta > TargetAccuracy * slowStart)
                return mConfig.TurnRate * sign;

            if (absDelta > TargetAccuracy) {
                double c = mConfig.TurnRate / 5;
                double xDelta = (TargetAccuracy * slowStart) - TargetAccuracy;
                double m = (mConfig.TurnRate - c) / xDelta;
                return ((m * absDelta) + c) * sign;
            }
            
            return Math.Min(absDelta, .005) * sign;
        }

        void mCore_Tick() {
            if (TargetDistance > mConfig.DistanceThreshold) {
                bool move;
                Rotation rotationDelta = GetRotationDelta(out move);
                Rotation orientation = mCore.Orientation + rotationDelta;

                if (!move && TargetDistance > (mConfig.DistanceThreshold * 4)) {
                    mCore.Update(mCore.Position, Vector3.Zero, orientation, rotationDelta);
                } else {

                    Vector3 moveDelta = Target - Position;
                    if (moveDelta.Length() > mConfig.MoveRate)
                        moveDelta *= mConfig.MoveRate / moveDelta.Length();

                    Vector3 position = mCore.Position + moveDelta;

                    if (mCore.ControlMode == ControlMode.Absolute)
                        mCore.Update(position, moveDelta, orientation, rotationDelta);
                    else
                        mCore.Update(position, new Vector3(mConfig.MoveRate, 0f, 0f), orientation, rotationDelta);
                }
            } else {
                mCore.Update(mCore.Position, Vector3.Zero, mCore.Orientation, Rotation.Zero);
                if (++mTargetIndex < mTargets.Count) {
                    mTarget = mTargets[mTargetIndex];
                    //Turn = new Rotation(Target - mCore.Position);
                    if (TargetChanged != null)
                        TargetChanged(mTarget.Key, mTarget.Value);
                } else {
                    Logger.Info("Finished walking route.");
                    mCore.Tick -= mTickListener;
                    mConfig.StopRecordingLog(mCore);

                    if (mConfig.AutoShutdown)
                        mForm.Invoke(new Action(() => mForm.Close()));
                }
            }
        }

        private bool RotatedToTarget() {
            //Console.Write("Yaw: " + (Orientation.Yaw - Turn.Yaw) + " - Pitch: " + (Orientation.Pitch - Turn.Pitch));
            bool ret = AreClose(Orientation.Yaw, Turn.Yaw);
            if (mCore.ControlMode == ControlMode.Absolute)
                ret = ret && AreClose(Orientation.Pitch, Turn.Pitch);
            //Console.WriteLine();
            return ret;
        }

        private float TargetDistance {
            get { return (Target - Position).Length(); }
        }

        private bool AreClose(double a, double b) {
            //Console.Write(" Diff: " + Math.Abs(a - b) + " - ret: " + (Math.Abs(a - b) < TargetAccuracy));
            return Math.Abs(a - b) < TargetAccuracy;
        }

        private double TargetAccuracy {
            get { return mCore.ControlMode == ControlMode.Absolute ? .0000000001 : (mConfig.TurnRate * 1.5); }
        }


        #region ISystemPlugin Members

        public void Init(Core core) {
            mCore = core;
            mMainController = mCore.GetPlugin<OpenSimController>();
            mMainController.ClientLoginComplete += new EventHandler(mMainController_CLientLoginComplete);

            LoadTargets();
        }

        void mMainController_CLientLoginComplete(object sender, EventArgs e) {
            if (Enabled && mConfig.AutoStart)
                Start();
        }

        public void SetForm(System.Windows.Forms.Form form) {
            mForm = form;
        }

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

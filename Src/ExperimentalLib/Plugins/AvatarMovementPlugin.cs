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
using System.Drawing;

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
        private bool mRunning;

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

        public void LoadTargets() {
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
                var targetAttr = targetNode.Attributes["location"];
                Vector3 target;

                if (targetAttr != null) {
                    if (Vector3.TryParse(targetAttr.Value, out target)) {
                        mTargets.Add(new KeyValuePair<string, Vector3>(nameStr, target));
                    }
                } else {
                    foreach (var node in nodesDoc.GetElementsByTagName("name").OfType<XmlElement>()) {
                        if (node.InnerText == nameStr) {
                            XmlNode x = node.NextSibling;
                            XmlNode y = x.NextSibling;
                            XmlNode z = y.NextSibling;

                            if (!float.TryParse(x.InnerXml, out target.X) || !float.TryParse(y.InnerXml, out target.Y) || !float.TryParse(z.InnerXml, out target.Z))
                                break;

                            mTargets.Add(new KeyValuePair<string, Vector3>(nameStr, target));
                            break;
                        }
                    }
                }
            }
        }

        public event Action<string, Vector3> TargetChanged;

        public void DrawMap() {
            Vector3 prev = mMainController.AvatarPosition;
            Thread.Sleep(500);
            prev = mMainController.AvatarPosition;
            using (Bitmap map = new Bitmap(mConfig.MapFile)) {
                prev.Y = map.Height - prev.Y;
                using (Graphics g = Graphics.FromImage(map)) {
                    using (Pen p = new Pen(Brushes.Red, 5))
                    foreach (var target in mTargets.Select(t => new Vector3(t.Value.X, map.Height - t.Value.Y, t.Value.Z))) {
                        g.DrawLine(p, new PointF(prev.X, prev.Y), new PointF(target.X, target.Y));
                        prev = target;
                    }
                }
                string name = Path.GetFileNameWithoutExtension(mConfig.MapFile) + "-WithRoute" + Path.GetExtension(mConfig.MapFile);
                string file = Path.Combine(Path.GetDirectoryName(mConfig.MapFile), name);
                Logger.Info("Saving map file to " + file + ".");
                map.Save(file);
            }
        }

        public void Start() {
            lock (this) {
                if (mRunning) {
                    Logger.Warn("Unable to start, already running.");
                    return;
                }
            }
            mCore.ControlMode = mConfig.Mode;

            if (mConfig.StartAtHome)
                mMainController.ViewerController.PressKey("H", true, false, true);
            else if (mConfig.Mode == ControlMode.Delta && mConfig.TeleportToStart) {
                TeleportToStart();
            }

            if (mTargets.Count > 0) {
                Logger.Info("Starting loop.");

                if (mConfig.SaveResults)
                    mConfig.SetupFPSLogs(mCore, Logger);

                mTargetIndex = 0;
                mTarget = mTargets[mTargetIndex];
                if (TargetChanged != null)
                    TargetChanged(mTarget.Key, Target);
                lock (this) {
                    mRunning = true;
                    mCore.Tick += new Action(mCore_Tick);
                }
            } else {
                Logger.Info("No targets loaded. Unable to start loop.");
            }
        }

        private void TeleportToStart() {
            mMainController.ViewerController.PressKey("m", true, false, false);
            mMainController.ViewerController.SendString(mConfig.StartIsland);
            mMainController.ViewerController.PressKey("{ENTER}");
            Thread.Sleep(5000);
            mMainController.ViewerController.PressKey("{TAB}");
            mMainController.ViewerController.SendString(mConfig.StartLocation.X.ToString());
            mMainController.ViewerController.PressKey("{TAB}");
            mMainController.ViewerController.SendString(mConfig.StartLocation.Y.ToString());
            mMainController.ViewerController.PressKey("{TAB}");
            mMainController.ViewerController.SendString(mConfig.StartLocation.Z.ToString());
            mMainController.ViewerController.PressKey("{TAB}");


            Thread.Sleep(5000);

            mMainController.ViewerController.PressKey("{ENTER}");

            Logger.Debug("Sent start location to client.");
        }

        private Vector3 Target {
            get { 
                return mCore.ControlMode == ControlMode.Absolute ?
                    mTarget.Value + new Vector3(0f, 0f, mConfig.HeightOffset) :
                    new Vector3(mTarget.Value.X, mTarget.Value.Y, 0f); 
            }
        }

        private Vector3 Position {
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
                    lock (this) {
                        mCore.Tick -= mTickListener;
                        mRunning = false;
                        mConfig.StopRecordingLog(mCore);

                        Console.Beep(2000, 100);
                        Thread.Sleep(100);
                        Console.Beep(2000, 100);
                        Thread.Sleep(100);
                        Console.Beep(2000, 100);

                        Thread.Sleep(1000);

                        foreach (var controller in mCore.Frames.Select(f => f.Output as OpenSimController))
                            controller.Stop();

                        Thread.Sleep(500);

                    }
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
            //get { return mCore.ControlMode == ControlMode.Absolute ? .0000000001 : (mConfig.TurnRate * 1.5); }
            get { return mConfig.TurnRate * 1.5; }
        }

        #region ISystemPlugin Members

        public void Init(Core core) {
            mCore = core;
            mMainController = mCore.GetPlugin<OpenSimController>();
            mMainController.ClientLoginComplete += new EventHandler(mMainController_CLientLoginComplete);
            mCore.ControlMode = mConfig.Mode;

            LoadTargets();
        }

        void mMainController_CLientLoginComplete(object sender, EventArgs e) {
            if (Enabled && mConfig.AutoStart)
                if (mConfig.StartWaitMS > 0) {
                    Thread t = new Thread(() => {
                        Logger.Info("Waiting " + mConfig.StartWaitMS + "MS before starting loop.");
                        Thread.Sleep(mConfig.StartWaitMS);
                        Start();
                    });
                    t.Name = "WalkBotStartWait";
                    t.Start();
                } else
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
                    if (!value) {
                        mCore.Tick -= mTickListener;
                        mRunning = false;
                    }

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
            lock (this) {
                mRunning = false;
                mCore.Tick -= mTickListener;
                mCore.Update(mCore.Position, Vector3.Zero, mCore.Orientation, Rotation.Zero);
            }
        }
    }
}

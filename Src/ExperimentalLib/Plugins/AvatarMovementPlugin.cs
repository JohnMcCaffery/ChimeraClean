using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Overlay;
using Chimera.OpenSim;
using Chimera.Experimental.GUI;
using OpenMetaverse;
using System.Xml;

namespace Chimera.Experimental.Plugins {
    public class AvatarMovementPlugin : XmlLoader, ISystemPlugin {
        private bool mEnabled;
        private Dictionary<string, OpenSimController> mController = new Dictionary<string,OpenSimController>();
        private OpenSimController mMainController;
        private Core mCore;
        private ExperimentalConfig mConfig = new ExperimentalConfig();
        private AvatarMovementContrl mPanel;
        private string mFile;

        private List<Vector3> mTargets = new List<Vector3>();

        private double mYawRate = .2;
        private double mPitchRate = .2;

        public string File {
            get { return mFile; }
            set {
                mFile = value;

                XmlDocument doc = new XmlDocument();
                doc.Load(mFile);

                mTargets.Clear();
                foreach (var targetNode in doc.GetElementsByTagName("Target").OfType<XmlElement>()) {
                     var targetStr = targetNode.Attributes["value"].Value;
                    mTargets.Add(Vector3.Parse(targetStr));
                }
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
                    mPanel = new AvatarMovementContrl(this);
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

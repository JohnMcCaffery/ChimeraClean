using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera;
using log4net;
using Chimera.Overlay;
using System.Drawing;
using System.Windows.Forms;
using Chimera.Config;
using Chimera.Interfaces;
using OpenMetaverse;
using Chimera.Overlay.GUI.Plugins;

namespace Chimera.Overlay.Plugins {
    public class AxisCursorConfig : AxisConfig {
        public AxisCursorConfig()
            : base("AxisCursor") {
        }

        public override bool LoadBoundAxes {
            get { return true; }
        }
    }

    public class AxisCursorPlugin : ISystemPlugin {
        private ILog Logger = LogManager.GetLogger("AxisCursor");

        private AxisCursorPanel mPanel;

        private bool mEnabled;
        //TODO add init of this to the config
        private string mWindow = "MainWindow";

        private FrameOverlayManager mManager;
        private OverlayPlugin mOverlayPlugin;

        private Action mTickListener;
        private AxisConfig mAxisConfig;
        private IAxis[] mInitialAxes;
        private List<IAxis> mAxes = new List<IAxis>();

        public event Action<IAxis> AxisAdded;

        public IEnumerable<IAxis> Axes {
            get { return mAxes; }
        }

        public AxisCursorPlugin(params IAxis[] axes) {
            mInitialAxes = axes;
        }

        public void AddAxis(IAxis axis) {
            mAxisConfig.ConfigureAxis(axis, mOverlayPlugin.Core);
            mAxes.Add(axis);
            if (AxisAdded != null)
                AxisAdded(axis);
        }

        private void Init() {
            mTickListener = new Action(TickListener);
            mAxisConfig = new AxisCursorConfig();
            foreach (IAxis axis in mInitialAxes)
                AddAxis(axis);
        }

        private void TickListener() {
            float x = mManager.CursorPosition.X;
            float y = mManager.CursorPosition.Y;
            foreach (var axis in mAxes.Where(a => a.Binding == AxisBinding.MouseX || a.Binding == AxisBinding.MouseY)) {
                //This is the code that moves the mouse!
                if (axis.Binding == AxisBinding.MouseX) 
                    x += axis.Delta;
                else 
                    y += axis.Delta;
            }
            mManager.UpdateCursor(Math.Max(Math.Min(x, .999999999), 0), Math.Max(Math.Min(y, .999999999), 0));
        }

        #region ISystemPlugin Members

        private Action<Frame, EventArgs> mWindowAddedListener;

        public void Init(Core core) {
            if (!core.HasPlugin<OverlayPlugin>()) {
                //throw new ArgumentException("Unable to load kinect cursor. Overlay plugin is not loaded.");
                Logger.Warn("Unable to load axis cursor. Overlay plugin is not loaded.");
                Init();
                return;
            }
            mOverlayPlugin = core.GetPlugin<OverlayPlugin>();
            if (core.HasFrame(mWindow)) {
                mManager = mOverlayPlugin[mWindow];
            } else {
                mWindowAddedListener = new Action<Chimera.Frame, EventArgs>(coordinator_WindowAdded);
                core.FrameAdded += mWindowAddedListener;
            }
            Init();
        }
        #endregion

        #region IPlugin Members
        public event Action<IPlugin, bool> EnabledChanged;

        public Control ControlPanel {
            get {
                if (mPanel == null)
                    mPanel = new AxisCursorPanel(this);
                return mPanel; 
            }
        }

        public Frame Frame {
            get { return mManager.Frame; }
        }

        public string State {
            get { return ""; }
        }

        public bool Enabled {
            get { return mEnabled; }
            set {
                if (value != mEnabled) {
                    mEnabled = value;
                    if (mOverlayPlugin != null) {
                        if (value)
                            mOverlayPlugin.Core.Tick += mTickListener;
                        else
                            mOverlayPlugin.Core.Tick -= mTickListener;
                    }
                    if (EnabledChanged != null)
                        EnabledChanged(this, value);
                }
            }
        }


        public string Name {
            get { return "AxisCursor"; }
        }

        public ConfigBase Config {
            get { throw new NotImplementedException(); }
        }

        public void Close() { }

        public void Draw(Graphics graphics, Func<Vector3, Point> to2D, Action redraw, Perspective perspective) { }

        #endregion

        private void coordinator_WindowAdded(Frame frame, EventArgs args) {
            if (frame.Name == mWindow) {
                mManager = mOverlayPlugin[frame.Name];
                frame.Core.FrameAdded -= mWindowAddedListener;
            }
        }

        #region ISystemPlugin Members


        public void SetForm(Form form) {
        }

        #endregion
    }
}

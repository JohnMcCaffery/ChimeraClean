using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chimera.Config;
using Chimera.Util;
using System.Drawing;
using OpenMetaverse;
using System.Diagnostics;
using Chimera.OpenSim.GUI;

namespace Chimera.OpenSim {
    public class OpenSimController : ISystemPlugin, IOutput {
        private bool mEnabled;
        private bool mClosingViewer;
        private bool mAutoRestart;
        private ViewerConfig mConfig;
        private Window mWindow;        private OutputPanel mOutputPanel;
        private InputPanel mInputPanel;

        private SetFollowCamProperties mFollowCamProperties;
        private ProxyControllerBase mProxyController;
        private ViewerController mViewerController;
        /// <summary>
        /// Selected whenever a client logs in to the proxy.
        /// </summary>
        public event Action OnClientLoggedIn;

        internal ProxyControllerBase ProxyController {
            get { return mProxyController; }
        }

        internal ViewerController ViewerController {
            get { return mViewerController; }
        }

        public bool Fullscreen {
            get { return mConfig.Fullscreen; }
            set {
                mConfig.Fullscreen = value;
                mViewerController.FullScreen = value;
            }
        }

        public bool ControlCamera {
            get { return mConfig.ControlCamera; }
            set {
                mConfig.ControlCamera = value;
                if (value)
                    mProxyController.SetCamera();
                else
                    mProxyController.ClearCamera();
            }
        }

        #region ISystemPlugin Members

        public void Init(Coordinator coordinator) { }

        public event Action<IPlugin, bool> EnabledChanged;

        UserControl IPlugin.ControlPanel {
            get {
                if (mInputPanel == null) {
                    mFollowCamProperties = new SetFollowCamProperties(Window.Coordinator);
                    mInputPanel = new InputPanel(mFollowCamProperties);

                }
                return mInputPanel;
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
            get { return "VW-Viewer"; }
        }

        public string State {
            get { throw new NotImplementedException(); }
        }

        public ConfigBase Config {
            get { return mConfig; }
        }

        public void Close() {
            mProxyController.Stop();
            mViewerController.Close(false);
        }

        public void Draw(Func<Vector3, Point> to2D, Graphics graphics, Action redraw, Perspective perspective) { }

        #endregion

        #region IOutput Members

        public bool AutoRestart {
            get { return mAutoRestart; }
            set { mAutoRestart = value; }
        }

        public string Type {
            get { return "VW-Viewer"; }
        }

        public bool Active {
            get { throw new NotImplementedException(); }
        }

        public Window Window {
            get { return mWindow; }
        }

        public Process Process {
            get { return mViewerController.Process; }
        }
        UserControl IOutput.ControlPanel {
            get {
                if (mOutputPanel == null)
                    mOutputPanel = new OutputPanel(this);
                return mOutputPanel;
            }
        }

        public void Init(Window window) {
            mWindow = window;
            mConfig = new ViewerConfig(window.Name);

            mViewerController = new ViewerController();
            if (mConfig.BackwardsCompatible)
                mProxyController = new FullController();
            else
                mProxyController = new BackwardCompatibleController();

            mWindow.Changed += new Action<Chimera.Window,EventArgs>(mWindow_Changed);
            mWindow.MonitorChanged += new Action<Chimera.Window,Screen>(mWindow_MonitorChanged);
            mProxyController.OnClientLoggedIn += new EventHandler(mProxyController_OnClientLoggedIn);
            mProxyController.PositionChanged += new Action<Vector3,Rotation>(mProxyController_PositionChanged);
            mViewerController.Exited += new Action(mViewerController_Exited);


            if (mConfig.AutoLoginClient)
                Launch();
            else if (mConfig.AutoStartProxy)
                StartProxy();
        }

        void mWindow_Changed(Window window, EventArgs args) {
            mProxyController.SetFrustum();
        }

        void mWindow_MonitorChanged(Window window, Screen monitor) {
            mViewerController.Monitor = monitor;
        }

        void mProxyController_OnClientLoggedIn(object sender, EventArgs e) {
            if (mConfig.Fullscreen)
                mViewerController.FullScreen = true;
            foreach (var key in mConfig.StartupKeyPresses.Split(','))
                mViewerController.PressKey(key);
        }

        void mProxyController_PositionChanged(Vector3 position, Rotation rotation) {
            if (IsMaster && mWindow.Coordinator.ControlMode == ControlMode.Delta)
                mWindow.Coordinator.Update(position, Vector3.Zero, rotation, Rotation.Zero, ControlMode.Absolute);
        }

        void mViewerController_Exited() {
        }

        public bool Launch() {
            return StartProxy() && StartViewer();
        }

        public void Restart(string reason) {
            mViewerController.Close(true);
            mProxyController.Stop();
            mProxyController.Start();
        }

        #endregion

        public bool IsMaster {
            get { return mProxyController != null; }
        }

        public bool StartProxy() {
            if (mProxyController.Proxy != null)
                return true;            if (mProxyController.StartProxy(mConfig.ProxyPort, mConfig.ProxyLoginURI)) {
                if (mFollowCamProperties != null)
                    mFollowCamProperties.SetProxy(mProxyController.Proxy);

                return true;
            }
            return false;
        }

        public bool StartViewer() {
            string args = mConfig.ViewerArguments;
            args += mProxyController.LoginURI;
            if (mConfig.LoginFirstName != null && mConfig.LoginLastName != null && mConfig.LoginPassword != null)
                args += " --login " + mConfig.LoginFirstName + " " + mConfig.LoginLastName + " " + mConfig.LoginPassword;
            return mViewerController.Start(mConfig.ViewerExecutable, mConfig.ViewerWorkingDirectory, args);
        }
    }
}

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
using System.Threading;

namespace Chimera.OpenSim {
    public class OpenSimController : ISystemPlugin, IOutput {
        private bool mEnabled;
        private bool mClosingViewer;
        private ViewerConfig mConfig;
        private Window mWindow;
        private OutputPanel mOutputPanel;
        private InputPanel mInputPanel;

        private SetFollowCamProperties mFollowCamProperties;
        private ProxyControllerBase mProxyController;
        private ViewerController mViewerController;

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

        public bool ControlFrustum {
            get { return mConfig.ControlFrustum; }
            set {
                mConfig.ControlFrustum = value;
                if (value)
                    mProxyController.SetFrustum(SetCamera);
                else 
                    mProxyController.ClearFrustum();
            }
        }

        public bool ControlCamera {
            get { return mConfig.ControlCamera; }
            set {
                mConfig.ControlCamera = value;
                if (value) {
                    if (Mode == ControlMode.Absolute)
                        mProxyController.SetCamera();
                    else if (IsMaster)
                        mFollowCamProperties.Update();
                } else {
                    mProxyController.ClearCamera();
                    if (IsMaster)
                        mFollowCamProperties.Clear();
                }
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
                    if (mProxyController.Started)
                        mFollowCamProperties.SetProxy(mProxyController.Proxy);
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
            mClosingViewer = true;
            mViewerController.Close(false);
            mProxyController.Stop();
        }

        public void Draw(Graphics graphics, Func<Vector3, Point> to2D, Action redraw, Perspective perspective) { }

        #endregion

        #region IOutput Members

        public bool AutoRestart {
            get { return mConfig.AutoRestartViewer; }
            set { mConfig.AutoRestartViewer = value; }
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

        private ControlMode Mode {
            get { return Window.Coordinator.ControlMode; }
        }

        public void Init(Window window) {
            mWindow = window;
            mConfig = new ViewerConfig(window.Name);

            mViewerController = new ViewerController(mConfig.ViewerToggleHUDKey);
            if (mConfig.BackwardsCompatible)
                mProxyController = new BackwardCompatibleController(window);
            else
                mProxyController = new FullController(window);

            mWindow.Coordinator.DeltaUpdated += new Action<Coordinator,DeltaUpdateEventArgs>(Coordinator_DeltaUpdated);
            mWindow.Coordinator.CameraUpdated += new Action<Coordinator,CameraUpdateEventArgs>(Coordinator_CameraUpdated);
            mWindow.Coordinator.CameraModeChanged += new Action<Coordinator,ControlMode>(Coordinator_CameraModeChanged);
            mWindow.Coordinator.EyeUpdated += new Action<Coordinator,EventArgs>(Coordinator_EyeUpdated);
            mWindow.Changed += new Action<Chimera.Window,EventArgs>(mWindow_Changed);
            mWindow.MonitorChanged += new Action<Chimera.Window,Screen>(mWindow_MonitorChanged);
            mProxyController.OnClientLoggedIn += new EventHandler(mProxyController_OnClientLoggedIn);
            mProxyController.PositionChanged += new Action<Vector3,Rotation>(mProxyController_PositionChanged);
            mViewerController.Exited += new Action(mViewerController_Exited);


            if (mConfig.AutoStartViewer)
                Launch();
            else if (mConfig.AutoStartProxy)
                StartProxy();
        }

        public bool Launch() {
            return StartProxy() && StartViewer();
        }

        public void Restart(string reason) {
            Console.WriteLine("Restarting viewer because of " + reason + ".");
            new Thread(() => {
                mViewerController.Close(true);
                mProxyController.Stop();
                mProxyController.Start();
            }).Start();
        }

        #endregion

        public bool IsMaster {
            get { return mFollowCamProperties != null; }
        }

        public bool StartProxy() {
            if (mProxyController.Proxy != null)
                return true;

            if (mProxyController.StartProxy(mConfig.ProxyPort, mConfig.ProxyLoginURI)) {
                if (IsMaster)
                    mFollowCamProperties.SetProxy(mProxyController.Proxy);

                return true;
            }
            return false;
        }

        public bool StartViewer() {
            string args = mProxyController.LoginURI;
            if (mConfig.LoginFirstName != null && mConfig.LoginLastName != null && mConfig.LoginPassword != null)
                args += " --login " + mConfig.LoginFirstName + " " + mConfig.LoginLastName + " " + mConfig.LoginPassword;
            args += " " + mConfig.ViewerArguments.Trim();
            return mViewerController.Start(mConfig.ViewerExecutable, mConfig.ViewerWorkingDirectory, args);
        }

        public void CloseViewer() {
            mClosingViewer = true;
            mViewerController.Close(false);
        }

        #region Event Handlers

        /// <summary>
        /// Whether to set the position of the camera along with the adjusted frustum.
        /// </summary>
        private bool SetCamera {
            get { return ControlCamera && Mode == ControlMode.Absolute || !IsMaster; }
        }

        void Coordinator_DeltaUpdated(Coordinator coordinator, DeltaUpdateEventArgs args) {
            if (IsMaster && ControlCamera)
                mProxyController.Move(args.positionDelta, args.rotationDelta, mConfig.DeltaScale);
        }

        void Coordinator_CameraUpdated(Coordinator coordinator, CameraUpdateEventArgs args) {
            if (ControlCamera && mProxyController.Started && (Mode == ControlMode.Absolute || !IsMaster))
                mProxyController.SetCamera(args.positionDelta, args.rotationDelta);
        }

        void Coordinator_CameraModeChanged(Coordinator coordinator, ControlMode newMode) {
            if (ControlCamera && mProxyController.Started) {
                if (Mode == ControlMode.Absolute)
                    mProxyController.SetCamera();
                else if (IsMaster) {
                    mProxyController.ClearCamera();
                    mFollowCamProperties.Update();
                }
            }
        }

        void Coordinator_EyeUpdated(Coordinator coordinator, EventArgs args) {
            if (ControlCamera && ControlFrustum && mProxyController.Started && Mode == ControlMode.Absolute)
                mProxyController.SetFrustum(SetCamera);
        }

        void mWindow_Changed(Window window, EventArgs args) {
            if (ControlFrustum)
                mProxyController.SetFrustum(SetCamera);
        }

        void mWindow_MonitorChanged(Window window, Screen monitor) {
            mViewerController.Monitor = monitor;
        }

        void mProxyController_OnClientLoggedIn(object sender, EventArgs e) {
            mViewerController.Monitor = mWindow.Monitor;
            if (mConfig.Fullscreen)
                mViewerController.FullScreen = true;

            new Thread(() => {
                Thread.Sleep(5000);
                foreach (var key in mConfig.StartupKeyPresses.Split(','))
                    mViewerController.PressKey(key);
            }).Start();


            if (ControlCamera) {
                if (ControlFrustum)
                    mProxyController.SetFrustum(SetCamera);
                if (Mode == ControlMode.Absolute)
                    mProxyController.SetCamera();
                else if (IsMaster)
                    mFollowCamProperties.Update();
            }
        }

        void mProxyController_PositionChanged(Vector3 position, Rotation rotation) {
            if (IsMaster && Mode == ControlMode.Delta)
                mWindow.Coordinator.Update(position, Vector3.Zero, rotation, Rotation.Zero, ControlMode.Absolute);
        }

        void mViewerController_Exited() {
            if (mConfig.AutoRestartViewer && !mClosingViewer)
                Restart("UnexpectedViewerClose");
            mClosingViewer = false;
        }

        #endregion

        #region ISystemPlugin Members


        public void SetForm(Form form) {
        }

        #endregion
    }
}

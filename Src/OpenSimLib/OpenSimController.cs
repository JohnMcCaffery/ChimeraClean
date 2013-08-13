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
using log4net;
using Chimera.Overlay;

namespace Chimera.OpenSim {
    public class OpenSimController : ISystemPlugin, IOutput {
        private static Form sForm = null;

        private readonly ILog ThisLogger = LogManager.GetLogger("OpenSim");
        private bool mEnabled;
        private bool mClosingViewer;
        private bool mShuttingDown;
        private ViewerConfig mConfig;
        private Frame mFrame;
        private OutputPanel mOutputPanel;
        private InputPanel mInputPanel;
        private FrameOverlayManager mManager = null;

        private Action mExitListener;

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

        public void Init(Core coordinator) {
            mFollowCamProperties = new SetFollowCamProperties(Frame.Core);
            if (mProxyController.Started)
                mFollowCamProperties.SetProxy(mProxyController.Proxy);
        }

        void coordinator_InitialisationFinished() {
            if (mFrame.Core.HasPlugin<OverlayPlugin>())
                mManager = mFrame.Core.GetPlugin<OverlayPlugin>()[mFrame.Name];
        }

        public event Action<IPlugin, bool> EnabledChanged;

        Control IPlugin.ControlPanel {
            get {
                if (mInputPanel == null)
                    mInputPanel = new InputPanel(mFollowCamProperties);
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
            mShuttingDown = true;
            Stop();
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

        public Frame Frame {
            get { return mFrame; }
        }

        public Process Process {
            get { return mViewerController.Process; }
        }

        Control IOutput.ControlPanel {
            get {
                if (mOutputPanel == null)
                    mOutputPanel = new OutputPanel(this);
                return mOutputPanel;
            }
        }

        private ControlMode Mode {
            get { return Frame.Core.ControlMode; }
        }

        public void Init(Frame frame) {
            mFrame = frame;
            mConfig = new ViewerConfig(frame.Name);

            mViewerController = new ViewerController(mConfig.ViewerToggleHUDKey, mFrame.Name);
            if (mConfig.BackwardsCompatible)
                mProxyController = new BackwardCompatibleController(frame);
            else
                mProxyController = new FullController(frame);

            mExitListener = new Action(mViewerController_Exited);

            mFrame.Core.DeltaUpdated += new Action<Core,DeltaUpdateEventArgs>(Coordinator_DeltaUpdated);
            mFrame.Core.CameraUpdated += new Action<Core,CameraUpdateEventArgs>(Coordinator_CameraUpdated);
            mFrame.Core.ControlModeChanged += new Action<Core,ControlMode>(Coordinator_CameraModeChanged);
            mFrame.Core.EyeUpdated += new Action<Core,EventArgs>(Coordinator_EyeUpdated);
            mFrame.Core.InitialisationComplete += new Action(Core_InitialisationComplete);
            mFrame.Changed += new Action<Chimera.Frame,EventArgs>(mFrame_Changed);
            mFrame.MonitorChanged += new Action<Chimera.Frame,Screen>(mFrame_MonitorChanged);
            mProxyController.OnClientLoggedIn += new EventHandler(mProxyController_OnClientLoggedIn);
            mProxyController.PositionChanged += new Action<Vector3,Rotation>(mProxyController_PositionChanged);
            mViewerController.Exited += mExitListener;
        }

        void Core_InitialisationComplete() {
            if (mFrame.Core.HasPlugin<OverlayPlugin>())
                mManager = mFrame.Core.GetPlugin<OverlayPlugin>()[mFrame.Name];

            if (mConfig.AutoStartViewer)
                Launch();
            else if (mConfig.AutoStartProxy)
                StartProxy();
        }

        public bool Launch() {
            return StartProxy() && StartViewer();
        }

        public void Restart(string reason) {
            ThisLogger.Warn("Restarting " + mFrame.Name + " viewer because " + reason + ".");
            //new Thread(() => {
            mClosingViewer = true;
            mViewerController.Close(true);
            StopProxy();
            if (!mShuttingDown) {
                Thread.Sleep(1000);
                mClosingViewer = false;
                StartProxy();
                mViewerController.Start();
            }
            //}).Start();
        }

        #endregion

        public bool IsMaster {
            get { return mFollowCamProperties != null; }
        }

        public bool StartProxy() {
            if (mProxyController.Proxy != null)
                return true;

            Func<bool> a = () => {
                if (mProxyController.StartProxy(mConfig.ProxyPort, mConfig.ProxyLoginURI)) {
                    if (IsMaster)
                        mFollowCamProperties.SetProxy(mProxyController.Proxy);

                    return true;
                }
                return false;
            };

            if (sForm != null && sForm.Created && !sForm.Disposing && !sForm.IsDisposed)
                return (bool) sForm.Invoke(a);
            else
                return a();
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

        void Coordinator_DeltaUpdated(Core coordinator, DeltaUpdateEventArgs args) {
            CheckTimeout();
            if (IsMaster && ControlCamera)
                mProxyController.Move(args.positionDelta, args.rotationDelta, mConfig.DeltaScale);
        }

        void Coordinator_CameraUpdated(Core coordinator, CameraUpdateEventArgs args) {
            CheckTimeout();
            if (ControlCamera && mProxyController.Started && (Mode == ControlMode.Absolute || !IsMaster))
                mProxyController.SetCamera(args.positionDelta, args.rotationDelta);
        }

        void Coordinator_CameraModeChanged(Core coordinator, ControlMode newMode) {
            if (ControlCamera && mProxyController.Started) {
                if (Mode == ControlMode.Absolute)
                    mProxyController.SetCamera();
                else if (IsMaster) {
                    mProxyController.ClearCamera();
                    mFollowCamProperties.Update();
                }
            }
        }

        void Coordinator_EyeUpdated(Core coordinator, EventArgs args) {
            //if (ControlCamera && ControlFrustum && mProxyController.Started && Mode == ControlMode.Absolute)
                //mProxyController.SetFrustum(SetCamera);
        }

        void mFrame_Changed(Frame frame, EventArgs args) {
            if (ControlFrustum)
                mProxyController.SetFrustum(SetCamera);
        }

        void mFrame_MonitorChanged(Frame frame, Screen monitor) {
            mViewerController.Monitor = monitor;
        }

        void mProxyController_OnClientLoggedIn(object sender, EventArgs e) {
            if (mConfig.Fullscreen)
                mViewerController.FullScreen = true;
            mViewerController.Monitor = mFrame.Monitor;

            new Thread(() => {
                Thread.Sleep(5000);

                if (mManager != null) {
                    mManager.BringToFront();
                }

                Thread.Sleep(20000);
                foreach (var key in mConfig.StartupKeyPresses.Split(',')) {
                    ThisLogger.Info(mViewerController.Name + " viewer pressing " + key);
                    mViewerController.PressKey(key);
                }
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
                mFrame.Core.Update(position, Vector3.Zero, rotation, Rotation.Zero, ControlMode.Absolute);
        }

        void mViewerController_Exited() {
            if (mConfig.AutoRestartViewer && !mClosingViewer)
                Restart("UnexpectedViewerClose");
            mClosingViewer = false;
        }

        public void SetForm(Form form) {
            sForm = form;
        }

        #endregion

        private void CheckTimeout() {
            if (mProxyController.Started && DateTime.Now.Subtract(mProxyController.LastUpdatePacket).TotalMinutes > 1.0) {
                mProxyController.LastUpdatePacket = DateTime.Now;
                Restart("ViewerStoppedResponding");
            }
        }

        private void StopProxy() {
            if (sForm != null && sForm.Created && !sForm.Disposing && !sForm.IsDisposed)
                sForm.Invoke(new Action(() => mProxyController.Stop()));
            else
                mProxyController.Stop();
        }

        public void Stop() {
            mClosingViewer = true;
            mViewerController.Close(false);
            StopProxy();
        }

        internal void CloseViewer(bool blocking) {
            mClosingViewer = true;
            mViewerController.Close(blocking);
        }
    }
}

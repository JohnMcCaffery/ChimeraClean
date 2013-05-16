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

namespace Chimera.OpenSim {
    public class OpenSimController : ISystemPlugin, IOutput {
        private bool mEnabled;
        private bool mClosingViewer;
        private bool mAutoRestart;
        private ViewerConfig mConfig;
        private Window mWindow;

        private SetFollowCamProperties mFollowCamProperties;
        private ProxyControllerBase mProxyController;
        private ViewerController mClientController;
        /// <summary>
        /// Selected whenever the client proxy starts up.
        /// </summary>
        public event Action OnProxyStarted;
        /// <summary>
        /// Selected whenever a client logs in to the proxy.
        /// </summary>
        public event Action OnClientLoggedIn;

        #region ISystemPlugin Members

        public void Init(Coordinator coordinator) { }

        public event Action<IPlugin, bool> EnabledChanged;

        public UserControl ControlPanel {
            get { throw new NotImplementedException(); }
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
            mClientController.Close();
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
            get { return mClientController.Process; }
        }

        public void Init(Window window) {
            mWindow = window;
            mConfig = new ViewerConfig(window.Name);
        }

        public bool Launch() {
            return StartProxy() && LaunchViewer();
        }

        public void Restart(string reason) {
            mClientController.Close();
            mProxyController.Stop();
            mProxyController.Start();
        }

        #endregion

        public bool StartProxy() {
            if (mProxyController.Proxy != null)
                return true;            if (mProxyController.StartProxy(mConfig.ProxyPort, mConfig.ProxyLoginURI)) {
                if (mFollowCamProperties != null)
                    mFollowCamProperties.SetProxy(mProxyController.Proxy);

                if (OnProxyStarted != null)
                    OnProxyStarted();

                return true;
            }

            return false;
        }

        public bool LaunchViewer() {
            string args = mConfig.ViewerArguments;
            args += mProxyController.LoginURI;
            if (mConfig.LoginFirstName != null && mConfig.LoginLastName != null && mConfig.LoginPassword != null)
                args += " --login " + mConfig.LoginFirstName + " " + mConfig.LoginLastName + " " + mConfig.LoginPassword;
            return mClientController.Start(mConfig.ViewerExecutable, mConfig.ViewerWorkingDirectory, args);
        }
    }
}

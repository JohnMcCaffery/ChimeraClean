using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.OpenSim;
using System.Windows.Forms;
using System.Threading;
using log4net;
using Chimera.Experimental.GUI;
using System.IO;

namespace Chimera.Experimental.Plugins {
    public class SettingLoaderPlugin : ISystemPlugin {
        private ILog Logger = LogManager.GetLogger("SettingsChanger");
        private Core mCore;
        private ExperimentalConfig mConfig;
        private ViewerConfig mViewerConfig;
        private OpenSimController OSOut;
        private SettingLoaderControl mControl;
        private string[] mFiles;

        public event Action Set;

        public void Init(Core core) {
            mCore = core;
            mConfig = core.HasPlugin<RecorderPlugin>() ? core.GetPlugin<RecorderPlugin>().Config as ExperimentalConfig : new ExperimentalConfig();
            mViewerConfig = new ViewerConfig();

            mFiles = File.ReadAllLines(mConfig.SettingsCollectionFile);

            if (mConfig.SettingsChangerEnabled && mConfig.Setting != null) {
                mConfig.RunInfo += (mConfig.RunInfo.Length == 0 ? "" : "-") + mConfig.Value;
                OSOut = (core.Frames[0].Output as OpenSimController);
                OSOut.ClientLoginComplete += new EventHandler(SettingLoaderPlugin_ClientLoginComplete);
            }
        }

        void SettingLoaderPlugin_ClientLoginComplete(object sender, EventArgs e) {
            OSOut.ViewerController.PressKey("s", true, true, true);

            //Select the correct setting
            OSOut.ViewerController.SendString(mConfig.Setting);
            Thread.Sleep(500);
            OSOut.ViewerController.PressKey("{TAB}");
            Thread.Sleep(500);
            OSOut.ViewerController.PressKey("{TAB}");
            Thread.Sleep(500);
            OSOut.ViewerController.PressKey("{TAB}");
            //Delete the old value
            OSOut.ViewerController.PressKey("{DEL}");

            //Set the filename
            OSOut.ViewerController.SendString(mConfig.Value.ToString());
            Thread.Sleep(500);
            Logger.Info("Set " + mConfig.Setting + " to " + mConfig.Value + ". Incrementing value by " + mConfig.Increment + ".");

            mConfig.Value += mConfig.Increment;

            //Save filename and close window
            OSOut.ViewerController.PressKey("{ENTER}");
            Thread.Sleep(500);
            OSOut.ViewerController.PressKey("W", true, false, false);

            if (Set != null)
                Set();
        }

        public void SetForm(System.Windows.Forms.Form form) { }

        public event Action<IPlugin, bool> EnabledChanged;

        public Control ControlPanel {
            get {
                if (mControl == null)
                    mControl = new SettingLoaderControl(this);
                return mControl;
            }
        }

        public bool Enabled {
            get { return mConfig.SettingsChangerEnabled; }
            set {
                if (mConfig.SettingsChangerEnabled != value) {
                    mConfig.SettingsChangerEnabled = value;
                    if (EnabledChanged != null)
                        EnabledChanged(this, value);
                }
            }
        }

        public string Name {
            get { return "SettingsChanger"; }
        }

        public string State {
            get { throw new NotImplementedException(); }
        }

        public Config.ConfigBase Config {
            get { return mConfig; }
        }

        public void Close() {
            bool incremented = false;
            bool failed = false;
            foreach (var frame in mCore.Frames) {
                if (File.Exists(mConfig.GetLogFileName(frame.Name))) {
                    if (!incremented) {
                        mConfig.Index++;
                        if (mConfig.Index < mFiles.Length) {
                            if (mViewerConfig.ViewerArguments.Contains("--settings"))
                                mViewerConfig.ViewerArguments.Replace(@"--settings settings.* ", "--settings " + mFiles[mConfig.Index]);
                            else
                                mViewerConfig.ViewerArguments += " --settings " + mFiles[mConfig.Index];
                        }
                    }
                    incremented = true;
                } else
                    failed = true;
            }

            if (failed)
                mCore.ExitCode = mConfig.FailCode;
        }

        public void Draw(System.Drawing.Graphics graphics, Func<OpenMetaverse.Vector3, System.Drawing.Point> to2D, Action redraw, Perspective perspective) { }
    }
}

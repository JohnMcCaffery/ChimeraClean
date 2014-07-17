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
    public class SettingChangerPlugin : ISystemPlugin {
        private ILog Logger;
        private ExperimentalConfig mConfig;
        private Core mCore;
        private OpenSimController OSOut;
        private SettingChangerControl mControl;
        private bool mWasEnabled;

        public event Action Set;

        public void Init(Core core) {
            Logger = LogManager.GetLogger("SettingsChanger");
            mCore = core;
            mConfig = ExperimentalConfig.Instance;
            mWasEnabled = mConfig.SettingsChangerEnabled;
            //mConfig = mCore.HasPlugin<ClientRecorderPlugin>() ? mCore.GetPlugin<ClientRecorderPlugin>().Config as ExperimentalConfig : new ExperimentalConfig();
            if (mConfig.SettingsChangerEnabled && mConfig.Setting != null) {
                OSOut = (core.Frames[0].Output as OpenSimController);
                OSOut.ClientLoginComplete += new EventHandler(SettingChangerPlugin_ClientLoginComplete);

                new Thread(() => {
                    Thread.Sleep(200);
                    mConfig.RunInfo += (mConfig.RunInfo.Length == 0 ? "" : "-") + mConfig.Value;
                }).Start();
            }
        }

        void SettingChangerPlugin_ClientLoginComplete(object sender, EventArgs e) {
            OSOut.ViewerController.PressKey("s", true, true, true);

            //Select the correct setting
            OSOut.ViewerController.SendString(mConfig.Setting);
            Thread.Sleep(500);
            OSOut.ViewerController.PressKey("{TAB}");
            for (int i = 0; i < mConfig.SelectDownPress; i++) {
                Thread.Sleep(500);
                OSOut.ViewerController.PressKey("{DOWN}");
            }
            Thread.Sleep(500);
            OSOut.ViewerController.PressKey("{TAB}");
            Thread.Sleep(500);
            OSOut.ViewerController.PressKey("{TAB}");
            //Delete the old value
            OSOut.ViewerController.PressKey("{DEL}");

            //Set the filename
            OSOut.ViewerController.SendString(mConfig.Value.ToString());
            Thread.Sleep(500);
            Logger.Info("Set " + mConfig.Setting + " to " + mConfig.Value + ".");

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
                    mControl = new SettingChangerControl(this);
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
            if (mWasEnabled) {
                //This will break if ClientRecorderPlugin is not loaded.
                string file = mCore.GetPlugin<ClientRecorderPlugin>().GetCSVName();
                if (File.Exists(file)) {
                    mConfig.Value += mConfig.Increment;
                    mConfig.Increment = mConfig.Increment * mConfig.IncrementMultiplier;
                    if (mConfig.Value <= mConfig.Max) {
                        mCore.ExitCode = mConfig.RepeatCode;
                        Logger.Info(mConfig.Setting + " incremented to " + mConfig.Value + ". New increment: " + mConfig.Increment + ". Exiting with RepeatCode (" + mConfig.RepeatCode + ").");
                    } else
                        Logger.Info("Finished incrementing " + mConfig.Setting + ". No exit code set.");
                } else {
                    Logger.Info("No log file found at " + file + " after " + mConfig.RunInfo + " run. " + mConfig.Setting + " not incremented. Exiting with RepeatCode (" + mConfig.RepeatCode + ").");
                    mCore.ExitCode = mConfig.RepeatCode;
                }
            } else
                Logger.Info("Settings changer not enabled.");
        }

        public void Draw(System.Drawing.Graphics graphics, Func<OpenMetaverse.Vector3, System.Drawing.Point> to2D, Action redraw, Perspective perspective) { }
    }
}

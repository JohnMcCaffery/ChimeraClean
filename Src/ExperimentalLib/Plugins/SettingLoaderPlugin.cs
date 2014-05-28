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
using System.Text.RegularExpressions;

namespace Chimera.Experimental.Plugins {
    public class SettingLoaderPlugin : ISystemPlugin {
        private ILog Logger = LogManager.GetLogger("SettingsLoader");
        private Core mCore;
        private ExperimentalConfig mConfig;
        private SettingLoaderControl mControl;
        private string[] mFiles;

        public void Init(Core core) {
            mCore = core;
            mConfig = core.HasPlugin<RecorderPlugin>() ? core.GetPlugin<RecorderPlugin>().Config as ExperimentalConfig : new ExperimentalConfig();

            mFiles = File.ReadAllLines(mConfig.SettingsCollectionFile);

            if (mConfig.SettingsLoaderEnabled) {
                if (mConfig.Index < mFiles.Length) {
                    mConfig.RunInfo = mFiles[mConfig.Index];

                    foreach (var output in core.Frames.Select(f => f.Output)) {
                        string file = "settings-" + mFiles[mConfig.Index] + ".xml";
                        ViewerConfig viewerConfig = (output as OpenSimController).Config as ViewerConfig;
                        if (viewerConfig.ViewerArguments.Contains("--settings")) {
                            viewerConfig.ViewerArguments.Replace(@"--settings .*xml", "--settings " + file);
                            viewerConfig.ViewerArguments = Regex.Replace(viewerConfig.ViewerArguments, @"--settings .*xml", "--settings " + file);
                        } else
                            viewerConfig.ViewerArguments += " --settings " + file;
                    }

                    Logger.Info("Settings loader loading settings file: settings-" + mConfig.RunInfo + ".xml.");
                }
            }
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
            get { return "SettingsLoader"; }
        }

        public string State {
            get { throw new NotImplementedException(); }
        }

        public Config.ConfigBase Config {
            get { return mConfig; }
        }

        public void Close() {
            if (!mConfig.SettingsLoaderEnabled)
                return;

            bool incremented = false;
            bool repeat = true;

            foreach (var frame in mCore.Frames) {
                if (File.Exists(mConfig.GetLogFileName(frame.Name))) {
                    if (!incremented) {
                        mConfig.Index++;
                        if (mConfig.Index < mFiles.Length) {
                            Logger.Info("Settings file set to: settings-" + mFiles[mConfig.Index] + ".xml. Exiting with RepeatCode (" + mConfig.RepeatCode + ").");
                        } else {
                            Logger.Info("No more graphics settings. No exit code set.");
                            repeat = false;
                        }
                    }
                    incremented = true;
                } else
                    Logger.Info("No log file found after " + mConfig.RunInfo + " run. Exiting with RepeatCode (" + mConfig.RepeatCode + ").");
            }

            if (repeat)
                mCore.ExitCode = mConfig.RepeatCode;
        }

        public void Draw(System.Drawing.Graphics graphics, Func<OpenMetaverse.Vector3, System.Drawing.Point> to2D, Action redraw, Perspective perspective) { }
    }
}

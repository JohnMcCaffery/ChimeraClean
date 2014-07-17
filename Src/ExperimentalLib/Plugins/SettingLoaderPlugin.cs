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
        private string mFile;
        private bool mWasEnabled;

        public void Init(Core core) {
            mCore = core;
            mConfig = ExperimentalConfig.Instance;

            mFiles = File.ReadAllLines(mConfig.SettingsCollectionFile);

            mWasEnabled = mConfig.SettingsLoaderEnabled;

            if (mConfig.SettingsLoaderEnabled) {
                if (mConfig.Index < mFiles.Length) {
                    foreach (var output in core.Frames.Select(f => f.Output)) {
                        mFile = "settings-" + mFiles[mConfig.Index] + ".xml";
                        string file = Path.Combine(Path.GetDirectoryName(mConfig.SettingsCollectionFile), mFile);
                        ReplaceSettingsFile((output as OpenSimController).Config as ViewerConfig, file, mConfig, Logger);
                    }

                    Logger.Info("Settings loader loading settings file: settings-" + mConfig.RunInfo + ".xml.");

                    new Thread(() => {
                        Thread.Sleep(200);
                        mConfig.RunInfo = mFiles[mConfig.Index];
                    }).Start();

                }
            }
        }

        public static void ReplaceSettingsFile(ViewerConfig viewerConfig, string file, ExperimentalConfig config, ILog Logger) {
            string filename = Path.GetFileName(file);
            try {
                File.Copy(file, Path.Combine(config.UserSettingsFolder, filename), true);
                Logger.Info("Copied " + file + " to " + config.UserSettingsFolder + ".");
            } catch (IOException e) {
                Logger.Fatal("Unable to copy settings file from stored directory (" + Path.GetDirectoryName(config.SettingsFile) + ") to user settings folder (" + config.UserSettingsFolder + ").", e);
                Environment.Exit(-1);
            }

            if (viewerConfig.ViewerArguments.Contains("--settings")) {
                viewerConfig.ViewerArguments.Replace(@"--settings .*xml", "--settings " + filename);
                //viewerConfig.ViewerArguments = Regex.Replace(viewerConfig.ViewerArguments, @"--settings .*xml", "--settings " + filename);
            } else
                viewerConfig.ViewerArguments += " --settings " + filename;
        }

        public void SetForm(System.Windows.Forms.Form form) { }

        public event Action<IPlugin, bool> EnabledChanged;
        public string Setting {
            get { return mFile; }
        }

        public Control ControlPanel {
            get {
                if (mControl == null)
                    mControl = new SettingLoaderControl(this);
                return mControl;
            }
        }

        public bool Enabled {
            get { return mConfig.SettingsLoaderEnabled; }
            set {
                if (mConfig.SettingsLoaderEnabled != value) {
                    mConfig.SettingsLoaderEnabled = value;
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
            if (!mWasEnabled)
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

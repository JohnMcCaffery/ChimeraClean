using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.OpenSim;
using System.Windows.Forms;
using System.Threading;
using log4net;

namespace Chimera.Experimental.Plugins {
    class SettingChangerPlugin : ISystemPlugin {
        private ILog Logger;
        private bool mEnabled;
        private ExperimentalConfig mConfig;
        private OpenSimController OSOut;

        public void Init(Core core) {
            Logger = LogManager.GetLogger("SettingsChanger");
            mConfig = new ExperimentalConfig();
            OSOut = (core.Frames[0].Output as OpenSimController);
            OSOut.ClientLoginComplete += new EventHandler(SettingChangerPlugin_ClientLoginComplete);
        }

        void SettingChangerPlugin_ClientLoginComplete(object sender, EventArgs e) {
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
        }

        public void SetForm(System.Windows.Forms.Form form) { }

        public event Action<IPlugin, bool> EnabledChanged;

        public Control ControlPanel {
            get { return new Control(); }
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
            get { return "SettingsChanger"; }
        }

        public string State {
            get { throw new NotImplementedException(); }
        }

        public Config.ConfigBase Config {
            get { return mConfig; }
        }

        public void Close() { }

        public void Draw(System.Drawing.Graphics graphics, Func<OpenMetaverse.Vector3, System.Drawing.Point> to2D, Action redraw, Perspective perspective) { }
    }
}

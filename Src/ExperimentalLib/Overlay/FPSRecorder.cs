using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Overlay;
using Chimera.Interfaces.Overlay;
using Chimera.OpenSim;
using System.Xml;
using System.IO;
using System.Threading;
using Chimera.Flythrough;
using log4net.Core;
using log4net;

namespace Chimera.Experimental.Overlay {
    public class FPSRecorderStateFactory : XmlLoader, IStateFactory {
        public State Create(OverlayPlugin manager, XmlNode node) {
            return new FPSRecorderState(manager, node);
        }

        public State Create(OverlayPlugin manager, XmlNode node, System.Drawing.Rectangle clip) {
            return Create(manager, node);
        }

        public string Name {
            get { return "FPSRecorder"; }
        }
    }

    public class FPSRecorderState : State {
        private ILog Logger = LogManager.GetLogger(typeof(FPSRecorderState));
        private FlythroughPlugin mFlythroughPlugin;
        private ExperimentalConfig mConfig;
        private string mFolder;
        private string mFlythrough;

        public FPSRecorderState(OverlayPlugin manager, XmlNode node)
            : base(GetName(node, "initialising FPSRecorder state"), manager, node) {
            mFolder = Path.GetFullPath(GetString(node, "FPS", "Folder"));
            mFlythrough = GetString(node, "Flythroughs/Expriment.xml", "Flythrough");

            if (manager.Core.HasPlugin<FlythroughPlugin>())
                mFlythroughPlugin = manager.Core.GetPlugin<FlythroughPlugin>();
            else
                Logger.Warn("Unable to initialise FPSRecorder, flythroughPlugin not bound.");
        }

        protected override void TransitionToStart() { }

        protected override void TransitionToFinish() {
            mConfig.SetupFPSLogs(mFlythroughPlugin.Core, "");

            if (mFlythroughPlugin != null) {
                mFlythroughPlugin.Enabled = true;
                mFlythroughPlugin.Load(mFlythrough);
                mFlythroughPlugin.Play();
            }
        }

        protected override void TransitionFromStart() { 
        }

        protected override void TransitionFromFinish() {
            foreach (var frame in Manager.Core.Frames) {
                OpenSimController OSOut = frame.Output as OpenSimController;
                if (OSOut != null && OSOut.ViewerController.Started) {
                    /*
                    OSOut.ViewerController.PressKey("s", true, true, true);
                    OSOut.ViewerController.PressKey("U");
                    OSOut.ViewerController.PressKey("s");
                    OSOut.ViewerController.PressKey("e");
                    OSOut.ViewerController.PressKey("r");
                    OSOut.ViewerController.PressKey("L");
                    OSOut.ViewerController.PressKey("{TAB}");
                    OSOut.ViewerController.PressKey("{TAB}");
                    OSOut.ViewerController.PressKey("{TAB}");
                    OSOut.ViewerController.PressKey("{DEL}");
                    OSOut.ViewerController.PressKey("{TAB}");
                    OSOut.ViewerController.PressKey("W", true, false, false);
                    */


                    //Select the correct setting
                    OSOut.ViewerController.SendString("UserL");
                    OSOut.ViewerController.PressKey("{TAB}");
                    OSOut.ViewerController.PressKey("{TAB}");
                    OSOut.ViewerController.PressKey("{TAB}");
                    //Delete the test filename
                    OSOut.ViewerController.PressKey("{DEL}");
                    //Save filename and close window
                    OSOut.ViewerController.PressKey("{TAB}");
                    OSOut.ViewerController.PressKey("W", true, false, false);
                }
            }
        }
    }
}

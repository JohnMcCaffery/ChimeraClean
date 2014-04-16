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
        private string mFolder;
        private string mFlythrough;

        public FPSRecorderState(OverlayPlugin manager, XmlNode node)
            : base(GetName(node, "initialising FPSRecorder state"), manager, node) {
            mFolder = Path.GetFullPath(GetString(node, "FPS", "Folder"));
            mFlythrough = GetString(node, "Flythrough", "Flythroughs/Expriment.xml");

        }
        protected override void TransitionToStart() { }

        protected override void TransitionToFinish() {
            string started = DateTime.Now.ToString("yyyy.MM.dd.HH.mm");

            string dir = Path.Combine(mFolder, started);

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            foreach (var frame in Manager.Core.Frames) {
                OpenSimController OSOut = frame.Output as OpenSimController;
                if (OSOut != null && OSOut.ViewerController.Started) {
                    OSOut.ViewerController.PressKey("s", true, true, true);
                    Console.WriteLine("ctrl + alt + shift + s");
                    OSOut.ViewerController.PressKey("U");
                    Console.WriteLine("U");
                    OSOut.ViewerController.PressKey("s");
                    Console.WriteLine("s");
                    OSOut.ViewerController.PressKey("e");
                    Console.WriteLine("e");
                    OSOut.ViewerController.PressKey("r");
                    Console.WriteLine("r");
                    OSOut.ViewerController.PressKey("L");
                    Console.WriteLine("L");
                    OSOut.ViewerController.PressKey("{TAB}");
                    Console.WriteLine("{TAB}");
                    OSOut.ViewerController.PressKey("{TAB}");
                    Console.WriteLine("{TAB}");
                    OSOut.ViewerController.PressKey("{TAB}");
                    Console.WriteLine("{TAB}");
                    OSOut.ViewerController.PressKey("{DEL}");
                    Console.WriteLine("{DEL}");

                    string file = Path.Combine(dir, started + "-" + frame.Name + ".log");

                    foreach (char c in file) {
                        OSOut.ViewerController.PressKey(c + "");
                        Console.WriteLine(c + "");
                    }

                    OSOut.ViewerController.PressKey("{TAB}");
                    Console.WriteLine("{TAB}");

                    OSOut.ViewerController.PressKey("W", true, false, false);
                    Console.WriteLine("ctrl + W");
                }
            }
        }

        protected override void TransitionFromStart() { }

        protected override void TransitionFromFinish() {
            foreach (var frame in Manager.Core.Frames) {
                OpenSimController OSOut = frame.Output as OpenSimController;
                if (OSOut != null && OSOut.ViewerController.Started) {
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
                }
            }
        }
    }
}

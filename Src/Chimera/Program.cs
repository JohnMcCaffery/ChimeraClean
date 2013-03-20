using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Chimera.GUI.Forms;
using Chimera;
using Chimera.Util;
using Chimera.Inputs;
using Chimera.Overlay;
using Chimera.Launcher;

namespace Chimera {
    public static class ChimeraLauncher {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main() {
            Application.SetCompatibleTextRenderingDefault(false);

            SingleInstanceLauncher launcher = new SingleInstanceLauncher();

            ProcessWrangler.BlockingRunForm(launcher.Form, launcher.Coordinator);
        }
    }
}

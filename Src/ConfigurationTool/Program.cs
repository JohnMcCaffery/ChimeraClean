using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Chimera.ConfigurationTool.Controls;
using System.IO;
using System.Reflection;

namespace Chimera.ConfigurationTool {
    static class Program {
        static void Main() {
            AppDomainSetup info = new AppDomainSetup();
            info.ShadowCopyFiles = "true";
            info.ApplicationBase = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "../");
            AppDomain sub = AppDomain.CreateDomain("Cached domain", null, info);
            Launch launch = sub.CreateInstanceAndUnwrap(typeof(Launch).Assembly.FullName, typeof(Launch).FullName) as Launch;
            launch.Run();
        }
    }

    public class Launch : MarshalByRefObject {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public void Run() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ConfigurationTool());
        }
    }
}

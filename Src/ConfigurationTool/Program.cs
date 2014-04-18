using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Chimera.ConfigurationTool.Controls;
using System.IO;
using System.Reflection;
using Nini.Config;

namespace Chimera.ConfigurationTool {
    static class Program {
        static void Main() {
            AppDomainSetup info = new AppDomainSetup();
            info.ShadowCopyFiles = "true";
            info.ApplicationBase = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "../");
            AppDomain sub = AppDomain.CreateDomain("Cached domain", null, info);
            Launch launch = sub.CreateInstanceAndUnwrap(typeof(Launch).Assembly.FullName, typeof(Launch).FullName) as Launch;
            launch.Run(new ConfigFolderSetter());
        }
    }

    public class Launch : MarshalByRefObject {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public void Run(ConfigFolderSetter setter) {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ConfigurationTool(setter));
        }
    }

    public class ConfigFolderSetter : MarshalByRefObject {
        public void SetFolder(string folder) {
            DotNetConfigSource source = new DotNetConfigSource();
            IConfig cfg = source.Configs["Config"];
            if (cfg == null) {
                cfg = source.Configs.Add("Config");
            }

            cfg.Set("ConfigFolder", folder);
            source.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
}
    }
}

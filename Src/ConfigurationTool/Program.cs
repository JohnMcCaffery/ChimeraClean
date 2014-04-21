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

        [STAThread]
        static void Main() {
            Type t = typeof(Chimera.Config.ConfigBase);
            Assembly chimeraLib = t.Assembly;
            string chimeraLibFile = chimeraLib.Location;

            AppDomainSetup info = new AppDomainSetup();
            info.ShadowCopyFiles = "true";
            //info.ApplicationBase = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "../");
            AppDomain sub = AppDomain.CreateDomain("Cached domain", null, info);

            sub.AssemblyResolve += new ResolveEventHandler(sub_AssemblyResolve);
            Launch launch = sub.CreateInstanceAndUnwrap(typeof(Launch).Assembly.FullName, typeof(Launch).FullName) as Launch;
            launch.Run(new ConfigFolderSetter());
        }

        static Assembly sub_AssemblyResolve(object sender, ResolveEventArgs args) {
            Type t = typeof(Chimera.Config.ConfigBase);
            Assembly chimeraLib = t.Assembly;
            string chimeraLibFile = chimeraLib.Location;
            string folder = AppDomain.CurrentDomain.BaseDirectory;

            Assembly ass = LoadAssembly(folder, args);
            if (ass != null) {
                Console.WriteLine("Loaded " + args.Name + " from " + folder);
                return null;
            }

            folder = Path.GetFullPath(Path.Combine(folder, ".."));
            Console.WriteLine("Loaded " + args.Name + " from " + folder);
            return LoadAssembly(folder, args);
        }

        private static Assembly LoadAssembly(string folder, ResolveEventArgs args) {
            string file = Path.Combine(folder, args.Name.Split(',')[0] + ".dll");
            if (!File.Exists(file))
                return null;

            return Assembly.LoadFrom(file);
        }
    }

    public class Launch : MarshalByRefObject {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public void Run(ConfigFolderSetter setter) {
            Type t = typeof(Chimera.Config.ConfigBase);
            Assembly chimeraLib = t.Assembly;
            string chimeraLibFile = chimeraLib.Location;

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

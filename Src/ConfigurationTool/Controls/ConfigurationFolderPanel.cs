using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using Chimera.Config;
using Nini.Config;
using CfgBase = Chimera.Config.ConfigBase;
using ConfigurationTool.Controls;

namespace Chimera.ConfigurationTool.Controls {
    public partial class ConfigurationFolderPanel : UserControl {
        private string mFolder;
        private ConfigFolderSetter mSetter;
        
        public ConfigurationFolderPanel() {
            InitializeComponent();
        }

        public ConfigurationFolderPanel(string folder, ConfigFolderSetter setter) : this() {
            mFolder = folder;
            mSetter = setter;

            this.bindingsControlPanel = new BindingsControlPanel(folder);

            // 
            // bindingsControlPanel
            // 
            this.bindingsControlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bindingsControlPanel.Location = new System.Drawing.Point(3, 3);
            this.bindingsControlPanel.Name = "bindingsControlPanel";
            this.bindingsControlPanel.Size = new System.Drawing.Size(1079, 564);
            this.bindingsControlPanel.TabIndex = 0;

            this.BindingsTab.Controls.Add(this.bindingsControlPanel);

            loader.DoWork += Startup;
            //loader.RunWorkerAsync();
        }

        private void Startup(object source, DoWorkEventArgs args) {
            LoadConfigurationObjects();
            Refresh();
        }

        private bool InheritsFrom(Type t, Type p) {
            Type b = t.BaseType;
            while (b != null) {
                if (b == p)
                    return true;
                b = b.BaseType;
            }
            return false;
        }

        private bool IsConfig(Type t) {
            return InheritsFrom(t, typeof(ConfigFolderBase)) || InheritsFrom(t, typeof(CfgBase));
        }

        private class AssemblyComparer : IEqualityComparer<Assembly> {
            public bool Equals(Assembly x, Assembly y) {
                return x.FullName == y.FullName;
            }

            public int GetHashCode(Assembly obj) {
                return obj.FullName.GetHashCode();
            }
        }


        private static AssemblyComparer sAssemblyComparer = new AssemblyComparer();
        private static IEnumerable<Assembly> sAssemblies = null;

        public static IEnumerable<Assembly> LoadAssemblies(string folder) {
            if (sAssemblies != null)
                return sAssemblies;

            sAssemblies = new Assembly[] { typeof(Chimera.Config.ConfigBase).Assembly }.
                Concat(Directory.GetFiles(folder).
                Where(f => 
                    Path.GetExtension(f).ToUpper() == ".DLL" && 
                    !f.Contains("NuiLib") && 
                    !f.Contains("opencv") && 
                    !f.Contains("openjpeg") && 
                    !f.Contains("SlimDX") &&
                    !f.Contains("WMP") &&
                    f.EndsWith("Lib.dll")).
                Select(f => {
                    try {
                        return Assembly.Load(File.ReadAllBytes(f));
                    } catch (Exception e) {
                        return null;
                    }
                })).Where(a => a != null).Distinct(sAssemblyComparer);

            Console.WriteLine(String.Format("Loading Chimera assemblies. %-3i assemblies found.",  sAssemblies.Count()));
                //sAssemblies.Aggregate("", (str, assembly) => str + assembly.FullName.Split(',')[0]+",").Trim(',')                

            return sAssemblies;
        }

/*
=======
Random shit from merge - I think this is before config tool handled load errors correctly
        private void LoadConfigurationObjects() {
            string folder = Path.GetDirectoryName(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            folder = Path.GetFullPath(Path.Combine(folder, "../"));

            //Iterate through every assembly in the folder where the tool is running
            foreach (var assembly in 
                Directory.GetFiles(folder).
                    !f.Contains("SlimDX") && 
                    !f.Contains("openjpeg")).

                }).
                Where(a => a != null).
                Concat(new Assembly[] { typeof(CfgBase).Assembly })) {
                ListViewGroup g = null;
                //Iterate through every class which implements one of the interfaces on the interfaces list
                foreach (var clazz in 
                    assembly.GetTypes().
                    Where(t => 
                        !t.IsAbstract && 
                        !t.IsInterface && 
                        IsConfig(t))) {



>>>>>>> No idea what is going on.
*/


        private void LoadConfigurationObjects() {
            string folder = Path.GetDirectoryName(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            folder = Path.GetFullPath(Path.Combine(folder, "../"));

            var assemblies = ConfigurationFolderPanel.LoadAssemblies(folder);

            //Iterate through every assembly in the folder where the tool is running
            foreach (var assembly in assemblies.Concat(new Assembly[] { typeof(CfgBase).Assembly })) {

                ListViewGroup g = null;

                IEnumerable<Type> classes = null;
                try {
                    classes = assembly.GetTypes().
                        Where(t => 
                            !t.IsAbstract && 
                            !t.IsInterface && 
                            IsConfig(t));
                
                }
                catch (ReflectionTypeLoadException e) {
                    Console.WriteLine("Problem loading types for " + assembly.FullName.Split(',')[0] + ". " + e.Message);
                    foreach (var ex in e.LoaderExceptions)
                        Console.WriteLine(ex.Message);
                    continue;
                }

                Console.WriteLine("Loading {1,3} configuration objects from {0}.", assembly.FullName.Split(',')[0], classes.Count());

                //Iterate through every class which implements one of the interfaces on the interfaces list
                foreach (var clazz in classes) {
                    CfgBase config = InstantiateConfig(clazz, assembly, CfgBase.IGNORE_FRAME);
                    if (config == null)
                        continue;

                    Invoke(new Action(() => {
                        TabPage page = new TabPage(clazz.Name.Replace("Config", ""));
                        if (InheritsFrom(clazz, typeof(AxisConfig)))
                            LoadAxisConfig(page, config as AxisConfig);
                        if (config.Frame == null)
                            LoadConfig(page, config);
                        else
                            LoadFrameConfig(clazz, page, config);

                        MainTab.Controls.Add(page);
                    }));
                }
            }
        }

        private CfgBase InstantiateConfig(Type clazz, Assembly assembly, string frame) {
            if (clazz.GetConstructor(new Type[] { typeof(string), typeof(string[]) }) != null)
                return (CfgBase)assembly.CreateInstance(clazz.FullName, true, BindingFlags.CreateInstance, null, new object[] { frame, new string[0] }, null, null);
            else if (clazz.GetConstructor(new Type[] { typeof(string) }) != null)
                return (CfgBase)assembly.CreateInstance(clazz.FullName, true, BindingFlags.CreateInstance, null, new object[] { frame }, null, null);
            else if (clazz.GetConstructor(new Type[] { typeof(string[]) }) != null)
                return (CfgBase)assembly.CreateInstance(clazz.FullName, true, BindingFlags.CreateInstance, null, new object[] { new string[0] }, null, null);
            else if (clazz.GetConstructor(new Type[0]) != null)
                return (CfgBase)assembly.CreateInstance(clazz.FullName);
            else {
                MessageBox.Show("Unable to to instantiate " + clazz.Name + ". No matching constructor found.\n" +
                    "Valid constructors are:\n" +
                    "()\n" +
                    "(params string[])" +
                    "(string)" +
                    "(string, params string[])", "Unable to load configuration file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private void LoadAxisConfig(TabPage page, AxisConfig config) {
            if (config.LoadBoundAxes) {
                AxisConfigurationPanel panel = new AxisConfigurationPanel(config, bindingsControlPanel);
                panel.Dock = DockStyle.Fill;
                page.Controls.Add(panel);
            } else
                LoadConfig(page, config);
        }

        private void LoadConfig(TabPage page, CfgBase config) {
            ConfigurationObjectControlPanel panel = new ConfigurationObjectControlPanel(config);
            panel.Dock = DockStyle.Fill;
            page.Controls.Add(panel);
        }

        private void LoadFrameConfig(Type clazz, TabPage page, CfgBase config) {
            CoreConfig core = new CoreConfig();

            TabControl sectionsTab = new TabControl();

            TabPage sectionPage = new TabPage("Shared");
            ConfigurationObjectControlPanel panel = new ConfigurationObjectControlPanel(config, core.Frames);
            panel.Dock = DockStyle.Fill;

            sectionPage.Controls.Add(panel);
            sectionsTab.Controls.Add(sectionPage);

            foreach (var frame in core.Frames) {
                config = InstantiateConfig(clazz, clazz.Assembly, frame);
                sectionPage = new TabPage(frame);

                panel = new ConfigurationObjectControlPanel(config, frame);
                panel.Dock = DockStyle.Fill;

                sectionPage.Controls.Add(panel);
                sectionsTab.Controls.Add(sectionPage);
            }

            sectionsTab.Dock = DockStyle.Fill;
            page.Controls.Add(sectionsTab);
        }

        internal void LoadFolder() {
            mSetter.SetFolder(mFolder);
            bindingsControlPanel.InitialiseInterfaces();
            bindingsControlPanel.LoadDocument();
            LoadConfigurationObjects();
        }
    }
}

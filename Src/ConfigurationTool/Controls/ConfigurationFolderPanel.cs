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

namespace Chimera.ConfigurationTool.Controls {
    public partial class ConfigurationFolderPanel : UserControl {
        private static Type sConfigT = typeof(CfgBase);
        private static Type sConfigFolderT = typeof(ConfigFolderBase);

        private string mFolder;
        
        public ConfigurationFolderPanel() {
            InitializeComponent();
        }

        public ConfigurationFolderPanel(string folder) : this() {
            mFolder = folder;
        }

        private bool IsConfig(Type t) {
            Type b = t.BaseType;
            while (b != null) {
                if (b == sConfigT || b == sConfigFolderT)
                    return true;
                b = b.BaseType;
            }
            return false;
        }

        private void loadConfigsButton_Click(object sender, EventArgs e) {
                DotNetConfigSource source = new DotNetConfigSource();
                IConfig cfg = source.Configs["Config"];
                if (cfg == null) {
                    cfg = source.Configs.Add("Config");
                }

                cfg.Set("ConfigFolder", "Configs/" + mFolder);
                source.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            LoadConfigurationObjects();
        }

        private void LoadConfigurationObjects() {
            string folder = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);

            //Iterate through every assembly in the folder where the tool is running
            foreach (var assembly in 
                Directory.GetFiles(folder).
                Where(f => Path.GetExtension(f).ToUpper() == ".DLL").
                Select(f => {
                    try {
                        return Assembly.LoadFile(f);
                    } catch (Exception e) {
                        return null;
                    }
            }).Where(a => a != null)) {
                ListViewGroup g = null;

                //Iterate through every class which implements one of the interfaces on the interfaces list
                foreach (var clazz in 
                    assembly.GetTypes().
                    Where(t => 
                        !t.IsAbstract && 
                        !t.IsInterface && 
                        IsConfig(t))) {

                    CfgBase config = InstantiateConfig(clazz, assembly, CfgBase.IGNORE_FRAME);
                    if (config == null)
                        continue;

                    TabPage page = new TabPage(clazz.Name.Replace("Config", ""));
                    if (config.Frame == null)
                        LoadConfig(page, config);
                    else
                        LoadFrameConfig(clazz, page, config);

                    MainTab.Controls.Add(page);
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
    }
}

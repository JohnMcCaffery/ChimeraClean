﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using Chimera.Config;
using Chimera.ConfigurationTool.Controls;
using System.Configuration;
using Nini.Config;

namespace Chimera.ConfigurationTool {
    public partial class ConfigurationTool : Form {
        private const string NONE = "Don't Copy";
        private bool mLoaded = false;
        private ConfigFolderSetter mSetter;
        private List<ConfigurationFolderPanel> mPanels = new List<ConfigurationFolderPanel>();

        public ConfigurationTool(ConfigFolderSetter setter) {
            mSetter = setter;
            InitializeComponent();
        }

        public void LoadFolders() {
            copyList.Items.Add(NONE);
            copyList.SelectedItem = NONE;
            foreach (var folder in Directory.GetDirectories(".").Select(f => f.Substring(2))) {
                folderList.Items.Add(folder);
                //if (folderList.SelectedItem == null)
                    //folderList.SelectedItem = folder;
                copyList.Items.Add(folder);

                AddFolder(folder);
            }

            foreach (var exe in Directory.GetFiles("..").Where(e => Path.GetExtension(e).ToUpper() == ".EXE" && !e.Contains("vshost"))) {
                string app = Path.GetFileNameWithoutExtension(exe);
                applicationList.Items.Add(app);
                if (applicationList.SelectedItem == null)
                    applicationList.SelectedItem = app;
            }

            loader.DoWork += Init;
            loader.RunWorkerAsync();
        }

        private ConfigurationFolderPanel AddFolder(string folder) {            TabPage page = new TabPage(folder);

            ConfigurationFolderPanel panel = new ConfigurationFolderPanel(folder, mSetter);
            mPanels.Add(panel);
            panel.Dock = DockStyle.Fill;

            page.Controls.Add(panel);

            FoldersTab.TabPages.Add(page);

            return panel;
        }

        private void Init(object source, DoWorkEventArgs args) {
            foreach (var panel in mPanels)
                panel.LoadFolder();
        }

        private void ConfigurationTool_Load(object sender, EventArgs e) {
            LoadFolders();
        }

        private void bindButton_Click(object sender, EventArgs e) {
            string configFile = Path.Combine("..", applicationList.SelectedItem + ".exe.config");            string configPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, configFile));            DotNetConfigSource source = new DotNetConfigSource(configPath);
            IConfig cfg = source.Configs["Config"];
            if (cfg == null) {
                cfg = source.Configs.Add("Config");
            }

            cfg.Set("ConfigFolder", "Configs/" + folderList.SelectedItem);
            source.Save(configPath);

        }

        private void createButton_Click(object sender, EventArgs e) {
            if (nameBox.Text == "" || nameBox.Text == NAME_DEFAULT) {
                MessageBox.Show("You must enter a folder name for the new configuration.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string dir = Path.Combine(Environment.CurrentDirectory, nameBox.Text);
            try {
                Directory.CreateDirectory(dir);
                if (copyList.SelectedItem.ToString() != NONE) {
                    foreach (var file in Directory.GetFiles(copyList.SelectedItem.ToString()).Where(f => !f.EndsWith("~")))
                        File.Copy(file, Path.Combine(nameBox.Text, Path.GetFileName(file)));
                }

                mNewPanel = AddFolder(nameBox.Text);

                loader.DoWork -= Init;
                loader.DoWork += InitNewPanel;
                loader.RunWorkerAsync();

                folderList.Items.Add(nameBox.Text);
                copyList.Items.Add(nameBox.Text);


            } catch (Exception ex) {
                MessageBox.Show("Unable to create configuration.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private ConfigurationFolderPanel mNewPanel;

        private void InitNewPanel(object source, DoWorkEventArgs args) {
            mNewPanel.LoadFolder();
        }

        private void nameBox_Enter(object sender, EventArgs e) {
            if (nameBox.Text == "Configuration Name")
                nameBox.Text = "";
        }

        private void nameBox_Leave(object sender, EventArgs e) {
            if (nameBox.Text == "")
                nameBox.Text = NAME_DEFAULT;
        }

        private void reloadButton_Click(object sender, EventArgs e) {
            foreach (Control tab in FoldersTab.Controls)
                FoldersTab.Controls.Remove(tab);

            copyList.Items.Clear();
            folderList.Items.Clear();

            LoadFolders();
        }

        private void applicationList_SelectedIndexChanged(object sender, EventArgs e) {
            string file = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "../" + applicationList.SelectedItem + ".exe.config"));
            IConfigSource src = new DotNetConfigSource(file);
            string folder = "Configs/Test";
            if (src.Configs["Config"] != null)
                folder = src.Configs["Config"].Get("ConfigFolder", folder).Substring(8).TrimEnd('/', '\\');

            folderList.SelectedItem = folderList.Items.OfType<string>().FirstOrDefault(f => f == folder);
        }
    }
}

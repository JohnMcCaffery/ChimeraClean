using ConfigurationTool.Controls;
namespace Chimera.ConfigurationTool.Controls {
    partial class ConfigurationFolderPanel {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.MainTab = new System.Windows.Forms.TabControl();
            this.BindingsTab = new System.Windows.Forms.TabPage();
            this.launcher = new System.ComponentModel.BackgroundWorker();
            this.MainTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainTab
            // 
            this.MainTab.Controls.Add(this.BindingsTab);
            this.MainTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTab.Location = new System.Drawing.Point(0, 0);
            this.MainTab.Name = "MainTab";
            this.MainTab.SelectedIndex = 0;
            this.MainTab.Size = new System.Drawing.Size(1093, 596);
            this.MainTab.TabIndex = 2;
            // 
            // BindingsTab
            // 
            this.BindingsTab.Location = new System.Drawing.Point(4, 22);
            this.BindingsTab.Name = "BindingsTab";
            this.BindingsTab.Padding = new System.Windows.Forms.Padding(3);
            this.BindingsTab.Size = new System.Drawing.Size(1085, 570);
            this.BindingsTab.TabIndex = 0;
            this.BindingsTab.Text = "Bindings";
            this.BindingsTab.UseVisualStyleBackColor = true;
            // 
            // ConfigurationFolderPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MainTab);
            this.Name = "ConfigurationFolderPanel";
            this.Size = new System.Drawing.Size(1093, 596);
            this.MainTab.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl MainTab;
        private System.Windows.Forms.TabPage BindingsTab;
        private global::ConfigurationTool.Controls.BindingsControlPanel bindingsControlPanel;
        private System.ComponentModel.BackgroundWorker launcher;
    }
}

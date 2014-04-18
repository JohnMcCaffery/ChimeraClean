using ConfigurationTool.Controls;
namespace Chimera.ConfigurationTool {
    partial class ConfigurationTool {
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.FoldersTab = new System.Windows.Forms.TabControl();
            this.SuspendLayout();
            // 
            // FoldersTab
            // 
            this.FoldersTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FoldersTab.Location = new System.Drawing.Point(0, 0);
            this.FoldersTab.Name = "FoldersTab";
            this.FoldersTab.SelectedIndex = 0;
            this.FoldersTab.Size = new System.Drawing.Size(578, 473);
            this.FoldersTab.TabIndex = 0;
            // 
            // ConfigurationTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(578, 473);
            this.Controls.Add(this.FoldersTab);
            this.Name = "ConfigurationTool";
            this.Text = "Chimera Configuration Tool";
            this.Load += new System.EventHandler(this.ConfigurationTool_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl FoldersTab;


    }
}
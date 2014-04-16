using ConfigurationTool;
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
            this.bindingsControlPanel1 = new BindingsControlPanel();
            this.SuspendLayout();
            // 
            // bindingsControlPanel1
            // 
            this.bindingsControlPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bindingsControlPanel1.Location = new System.Drawing.Point(0, 0);
            this.bindingsControlPanel1.Name = "bindingsControlPanel1";
            this.bindingsControlPanel1.Size = new System.Drawing.Size(284, 262);
            this.bindingsControlPanel1.TabIndex = 0;
            // 
            // ConfigurationTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.bindingsControlPanel1);
            this.Name = "ConfigurationTool";
            this.Text = "Chimera Configuration Tool";
            this.ResumeLayout(false);

        }

        #endregion

        private global::ConfigurationTool.BindingsControlPanel bindingsControlPanel1;
    }
}
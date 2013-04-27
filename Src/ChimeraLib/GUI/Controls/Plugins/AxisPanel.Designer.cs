namespace Chimera.GUI.Controls.Plugins {
    partial class AxisPanel {
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
            this.mainGroup = new System.Windows.Forms.GroupBox();
            this.configPanel = new System.Windows.Forms.Panel();
            this.editBox = new System.Windows.Forms.CheckBox();
            this.bindingDropdown = new System.Windows.Forms.ComboBox();
            this.mainGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainGroup
            // 
            this.mainGroup.Controls.Add(this.configPanel);
            this.mainGroup.Controls.Add(this.editBox);
            this.mainGroup.Controls.Add(this.bindingDropdown);
            this.mainGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainGroup.Location = new System.Drawing.Point(0, 0);
            this.mainGroup.Name = "mainGroup";
            this.mainGroup.Size = new System.Drawing.Size(429, 203);
            this.mainGroup.TabIndex = 0;
            this.mainGroup.TabStop = false;
            this.mainGroup.Text = "Axis Name";
            // 
            // configPanel
            // 
            this.configPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.configPanel.Location = new System.Drawing.Point(6, 46);
            this.configPanel.Name = "configPanel";
            this.configPanel.Size = new System.Drawing.Size(417, 151);
            this.configPanel.TabIndex = 2;
            // 
            // editBox
            // 
            this.editBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.editBox.AutoSize = true;
            this.editBox.Location = new System.Drawing.Point(379, 21);
            this.editBox.Name = "editBox";
            this.editBox.Size = new System.Drawing.Size(44, 17);
            this.editBox.TabIndex = 1;
            this.editBox.Text = "Edit";
            this.editBox.UseVisualStyleBackColor = true;
            this.editBox.CheckedChanged += new System.EventHandler(this.editBox_CheckedChanged);
            // 
            // bindingDropdown
            // 
            this.bindingDropdown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bindingDropdown.FormattingEnabled = true;
            this.bindingDropdown.Location = new System.Drawing.Point(6, 19);
            this.bindingDropdown.Name = "bindingDropdown";
            this.bindingDropdown.Size = new System.Drawing.Size(367, 21);
            this.bindingDropdown.TabIndex = 0;
            this.bindingDropdown.SelectedIndexChanged += new System.EventHandler(this.bindingDropdown_SelectedIndexChanged);
            // 
            // AxisPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainGroup);
            this.MinimumSize = new System.Drawing.Size(0, 47);
            this.Name = "AxisPanel";
            this.Size = new System.Drawing.Size(429, 203);
            this.mainGroup.ResumeLayout(false);
            this.mainGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox mainGroup;
        private System.Windows.Forms.ComboBox bindingDropdown;
        private System.Windows.Forms.CheckBox editBox;
        private System.Windows.Forms.Panel configPanel;
    }
}

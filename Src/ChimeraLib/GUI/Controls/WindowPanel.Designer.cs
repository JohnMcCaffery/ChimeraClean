namespace Chimera.GUI.Controls {
    partial class WindowPanel {
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
            this.mainTab = new System.Windows.Forms.TabControl();
            this.configTab = new System.Windows.Forms.TabPage();
            this.overlayTab = new System.Windows.Forms.TabPage();
            this.bringToFrontButtin = new System.Windows.Forms.Button();
            this.fullscreenCheck = new System.Windows.Forms.CheckBox();
            this.launchOverlayButton = new System.Windows.Forms.Button();
            this.monitorPulldown = new System.Windows.Forms.ComboBox();
            this.mouseControlCheck = new System.Windows.Forms.CheckBox();
            this.mainTab.SuspendLayout();
            this.overlayTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainTab
            // 
            this.mainTab.Controls.Add(this.configTab);
            this.mainTab.Controls.Add(this.overlayTab);
            this.mainTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTab.Location = new System.Drawing.Point(0, 0);
            this.mainTab.Name = "mainTab";
            this.mainTab.SelectedIndex = 0;
            this.mainTab.Size = new System.Drawing.Size(419, 315);
            this.mainTab.TabIndex = 0;
            this.mainTab.KeyDown += new System.Windows.Forms.KeyEventHandler(this.mainTab_KeyDown);
            this.mainTab.KeyUp += new System.Windows.Forms.KeyEventHandler(this.mainTab_KeyUp);
            // 
            // configTab
            // 
            this.configTab.Location = new System.Drawing.Point(4, 22);
            this.configTab.Name = "configTab";
            this.configTab.Padding = new System.Windows.Forms.Padding(3);
            this.configTab.Size = new System.Drawing.Size(411, 289);
            this.configTab.TabIndex = 0;
            this.configTab.Text = "Configuration";
            this.configTab.UseVisualStyleBackColor = true;
            // 
            // overlayTab
            // 
            this.overlayTab.Controls.Add(this.mouseControlCheck);
            this.overlayTab.Controls.Add(this.bringToFrontButtin);
            this.overlayTab.Controls.Add(this.fullscreenCheck);
            this.overlayTab.Controls.Add(this.launchOverlayButton);
            this.overlayTab.Controls.Add(this.monitorPulldown);
            this.overlayTab.Location = new System.Drawing.Point(4, 22);
            this.overlayTab.Name = "overlayTab";
            this.overlayTab.Padding = new System.Windows.Forms.Padding(3);
            this.overlayTab.Size = new System.Drawing.Size(411, 289);
            this.overlayTab.TabIndex = 1;
            this.overlayTab.Text = "Overlay";
            this.overlayTab.UseVisualStyleBackColor = true;
            // 
            // bringToFrontButtin
            // 
            this.bringToFrontButtin.Location = new System.Drawing.Point(6, 64);
            this.bringToFrontButtin.Name = "bringToFrontButtin";
            this.bringToFrontButtin.Size = new System.Drawing.Size(121, 23);
            this.bringToFrontButtin.TabIndex = 3;
            this.bringToFrontButtin.Text = "Bring To Front";
            this.bringToFrontButtin.UseVisualStyleBackColor = true;
            this.bringToFrontButtin.Click += new System.EventHandler(this.bringToFrontButtin_Click);
            // 
            // fullscreenCheck
            // 
            this.fullscreenCheck.AutoSize = true;
            this.fullscreenCheck.Location = new System.Drawing.Point(6, 93);
            this.fullscreenCheck.Name = "fullscreenCheck";
            this.fullscreenCheck.Size = new System.Drawing.Size(74, 17);
            this.fullscreenCheck.TabIndex = 2;
            this.fullscreenCheck.Text = "Fullscreen";
            this.fullscreenCheck.UseVisualStyleBackColor = true;
            this.fullscreenCheck.CheckedChanged += new System.EventHandler(this.showBordersTextBox_CheckedChanged);
            // 
            // launchOverlayButton
            // 
            this.launchOverlayButton.Location = new System.Drawing.Point(6, 35);
            this.launchOverlayButton.Name = "launchOverlayButton";
            this.launchOverlayButton.Size = new System.Drawing.Size(121, 23);
            this.launchOverlayButton.TabIndex = 1;
            this.launchOverlayButton.Text = "Launch Overlay";
            this.launchOverlayButton.UseVisualStyleBackColor = true;
            this.launchOverlayButton.Click += new System.EventHandler(this.launchOverlayButton_Click);
            // 
            // monitorPulldown
            // 
            this.monitorPulldown.DisplayMember = "DeviceName";
            this.monitorPulldown.FormattingEnabled = true;
            this.monitorPulldown.Location = new System.Drawing.Point(6, 8);
            this.monitorPulldown.Name = "monitorPulldown";
            this.monitorPulldown.Size = new System.Drawing.Size(121, 21);
            this.monitorPulldown.TabIndex = 0;
            this.monitorPulldown.SelectedIndexChanged += new System.EventHandler(this.screenPulldown_SelectedIndexChanged);
            // 
            // mouseControlCheck
            // 
            this.mouseControlCheck.AutoSize = true;
            this.mouseControlCheck.Location = new System.Drawing.Point(6, 116);
            this.mouseControlCheck.Name = "mouseControlCheck";
            this.mouseControlCheck.Size = new System.Drawing.Size(94, 17);
            this.mouseControlCheck.TabIndex = 4;
            this.mouseControlCheck.Text = "Mouse Control";
            this.mouseControlCheck.UseVisualStyleBackColor = true;
            this.mouseControlCheck.CheckedChanged += new System.EventHandler(this.mouseControlCheck_CheckedChanged);
            // 
            // WindowPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainTab);
            this.Name = "WindowPanel";
            this.Size = new System.Drawing.Size(419, 315);
            this.mainTab.ResumeLayout(false);
            this.overlayTab.ResumeLayout(false);
            this.overlayTab.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl mainTab;
        private System.Windows.Forms.TabPage configTab;
        private System.Windows.Forms.TabPage overlayTab;
        private System.Windows.Forms.ComboBox monitorPulldown;
        private System.Windows.Forms.Button launchOverlayButton;
        private System.Windows.Forms.CheckBox fullscreenCheck;
        private System.Windows.Forms.Button bringToFrontButtin;
        private System.Windows.Forms.CheckBox mouseControlCheck;
    }
}

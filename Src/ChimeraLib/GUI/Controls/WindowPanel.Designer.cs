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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WindowPanel));
            Chimera.Util.Rotation rotation2 = new Chimera.Util.Rotation();
            this.mainTab = new System.Windows.Forms.TabControl();
            this.configTab = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.heightPanel = new KinectLib.GUI.ScalarPanel();
            this.widthPanel = new KinectLib.GUI.ScalarPanel();
            this.orientationPanel = new ProxyTestGUI.RotationPanel();
            this.topLeftPanel = new ProxyTestGUI.VectorPanel();
            this.overlayTab = new System.Windows.Forms.TabPage();
            this.mouseControlCheck = new System.Windows.Forms.CheckBox();
            this.bringToFrontButtin = new System.Windows.Forms.Button();
            this.fullscreenCheck = new System.Windows.Forms.CheckBox();
            this.launchOverlayButton = new System.Windows.Forms.Button();
            this.monitorPulldown = new System.Windows.Forms.ComboBox();
            this.mainTab.SuspendLayout();
            this.configTab.SuspendLayout();
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
            this.configTab.Controls.Add(this.label2);
            this.configTab.Controls.Add(this.label1);
            this.configTab.Controls.Add(this.heightPanel);
            this.configTab.Controls.Add(this.widthPanel);
            this.configTab.Controls.Add(this.orientationPanel);
            this.configTab.Controls.Add(this.topLeftPanel);
            this.configTab.Location = new System.Drawing.Point(4, 22);
            this.configTab.Name = "configTab";
            this.configTab.Padding = new System.Windows.Forms.Padding(3);
            this.configTab.Size = new System.Drawing.Size(411, 289);
            this.configTab.TabIndex = 0;
            this.configTab.Text = "Configuration";
            this.configTab.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 221);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(15, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "H";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 194);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(18, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "W";
            // 
            // heightPanel
            // 
            this.heightPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.heightPanel.Location = new System.Drawing.Point(23, 218);
            this.heightPanel.Max = 500;
            this.heightPanel.Min = 0;
            this.heightPanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.heightPanel.Name = "heightPanel";
            this.heightPanel.Size = new System.Drawing.Size(388, 20);
            this.heightPanel.TabIndex = 3;
            this.heightPanel.Value = 0F;
            this.heightPanel.ValueChanged += new System.Action<float>(this.heightPanel_Changed);
            // 
            // widthPanel
            // 
            this.widthPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.widthPanel.Location = new System.Drawing.Point(23, 192);
            this.widthPanel.Max = 500;
            this.widthPanel.Min = 0;
            this.widthPanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.widthPanel.Name = "widthPanel";
            this.widthPanel.Size = new System.Drawing.Size(388, 20);
            this.widthPanel.TabIndex = 2;
            this.widthPanel.Value = 0F;
            this.widthPanel.ValueChanged += new System.Action<float>(this.widthPanel_Changed);
            // 
            // orientationPanel
            // 
            this.orientationPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.orientationPanel.Text = "Orientation (cm)";
            this.orientationPanel.Location = new System.Drawing.Point(3, 91);
            this.orientationPanel.LookAtVector = ((OpenMetaverse.Vector3)(resources.GetObject("orientationPanel.LookAtVector")));
            this.orientationPanel.MinimumSize = new System.Drawing.Size(252, 95);
            this.orientationPanel.Name = "orientationPanel";
            this.orientationPanel.Pitch = 0D;
            this.orientationPanel.Quaternion = ((OpenMetaverse.Quaternion)(resources.GetObject("orientationPanel.Quaternion")));
            this.orientationPanel.Size = new System.Drawing.Size(408, 95);
            this.orientationPanel.TabIndex = 1;
            rotation2.LookAtVector = ((OpenMetaverse.Vector3)(resources.GetObject("rotation2.LookAtVector")));
            rotation2.Pitch = 0D;
            rotation2.Quaternion = ((OpenMetaverse.Quaternion)(resources.GetObject("rotation2.Quaternion")));
            rotation2.Yaw = 0D;
            this.orientationPanel.Value = rotation2;
            this.orientationPanel.Yaw = 0D;
            // 
            // topLeftPanel
            // 
            this.topLeftPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.topLeftPanel.Text = "Top Left (cm)";
            this.topLeftPanel.Location = new System.Drawing.Point(0, 0);
            this.topLeftPanel.Max = 1024f;
            this.topLeftPanel.MaxV = ((OpenMetaverse.Vector3)(resources.GetObject("topLeftPanel.MaxV")));
            this.topLeftPanel.Min = -1024f;
            this.topLeftPanel.MinimumSize = new System.Drawing.Size(103, 95);
            this.topLeftPanel.MinV = ((OpenMetaverse.Vector3)(resources.GetObject("topLeftPanel.MinV")));
            this.topLeftPanel.Name = "topLeftPanel";
            this.topLeftPanel.Size = new System.Drawing.Size(411, 95);
            this.topLeftPanel.TabIndex = 0;
            this.topLeftPanel.Value = ((OpenMetaverse.Vector3)(resources.GetObject("topLeftPanel.Value")));
            this.topLeftPanel.X = 0F;
            this.topLeftPanel.Y = 0F;
            this.topLeftPanel.Z = 0F;
            this.topLeftPanel.ValueChanged += new System.EventHandler(this.positionPanel_ValueChanged);
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
            // WindowPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainTab);
            this.Name = "WindowPanel";
            this.Size = new System.Drawing.Size(419, 315);
            this.mainTab.ResumeLayout(false);
            this.configTab.ResumeLayout(false);
            this.configTab.PerformLayout();
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
        private ProxyTestGUI.VectorPanel topLeftPanel;
        private ProxyTestGUI.RotationPanel orientationPanel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private KinectLib.GUI.ScalarPanel heightPanel;
        private KinectLib.GUI.ScalarPanel widthPanel;
    }
}

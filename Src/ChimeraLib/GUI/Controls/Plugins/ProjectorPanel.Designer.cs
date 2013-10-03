namespace Chimera.GUI.Controls.Plugins {
    partial class ProjectorPanel {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectorPanel));
            Chimera.Util.Rotation rotation2 = new Chimera.Util.Rotation();
            this.aspectRatioSplit = new System.Windows.Forms.SplitContainer();
            this.projectorNativeAspectPulldown = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.projectorAspectPulldown = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.projectorDrawLabelsCheck = new System.Windows.Forms.CheckBox();
            this.projectorDrawCheck = new System.Windows.Forms.CheckBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.lockWidthButton = new System.Windows.Forms.RadioButton();
            this.lockHeightButton = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.upsideDownCheck = new System.Windows.Forms.CheckBox();
            this.vOffsetPanel = new Chimera.GUI.ScalarPanel();
            this.wallDistancePanel = new Chimera.GUI.ScalarPanel();
            this.orientationPanel = new Chimera.GUI.RotationPanel();
            this.throwRatioPanel = new Chimera.GUI.ScalarPanel();
            this.positionPanel = new Chimera.GUI.VectorPanel();
            this.lockBox = new System.Windows.Forms.GroupBox();
            this.noLockButton = new System.Windows.Forms.RadioButton();
            this.lockPositionButton = new System.Windows.Forms.RadioButton();
            this.wallDistanceLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.aspectRatioSplit)).BeginInit();
            this.aspectRatioSplit.Panel1.SuspendLayout();
            this.aspectRatioSplit.Panel2.SuspendLayout();
            this.aspectRatioSplit.SuspendLayout();
            this.lockBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // aspectRatioSplit
            // 
            this.aspectRatioSplit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.aspectRatioSplit.IsSplitterFixed = true;
            this.aspectRatioSplit.Location = new System.Drawing.Point(0, 217);
            this.aspectRatioSplit.Name = "aspectRatioSplit";
            // 
            // aspectRatioSplit.Panel1
            // 
            this.aspectRatioSplit.Panel1.Controls.Add(this.projectorNativeAspectPulldown);
            this.aspectRatioSplit.Panel1.Controls.Add(this.label13);
            // 
            // aspectRatioSplit.Panel2
            // 
            this.aspectRatioSplit.Panel2.Controls.Add(this.projectorAspectPulldown);
            this.aspectRatioSplit.Panel2.Controls.Add(this.label14);
            this.aspectRatioSplit.Size = new System.Drawing.Size(425, 34);
            this.aspectRatioSplit.SplitterDistance = 210;
            this.aspectRatioSplit.TabIndex = 56;
            // 
            // projectorNativeAspectPulldown
            // 
            this.projectorNativeAspectPulldown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.projectorNativeAspectPulldown.FormattingEnabled = true;
            this.projectorNativeAspectPulldown.Location = new System.Drawing.Point(114, 8);
            this.projectorNativeAspectPulldown.Name = "projectorNativeAspectPulldown";
            this.projectorNativeAspectPulldown.Size = new System.Drawing.Size(93, 21);
            this.projectorNativeAspectPulldown.TabIndex = 27;
            this.projectorNativeAspectPulldown.SelectedIndexChanged += new System.EventHandler(this.projectorNativeAspectPulldown_SelectedIndexChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(6, 11);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(102, 13);
            this.label13.TabIndex = 26;
            this.label13.Text = "Native Aspect Ratio";
            // 
            // projectorAspectPulldown
            // 
            this.projectorAspectPulldown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.projectorAspectPulldown.FormattingEnabled = true;
            this.projectorAspectPulldown.Location = new System.Drawing.Point(114, 8);
            this.projectorAspectPulldown.Name = "projectorAspectPulldown";
            this.projectorAspectPulldown.Size = new System.Drawing.Size(94, 21);
            this.projectorAspectPulldown.TabIndex = 28;
            this.projectorAspectPulldown.SelectedIndexChanged += new System.EventHandler(this.projectorAspectPulldown_SelectedIndexChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(3, 11);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(105, 13);
            this.label14.TabIndex = 42;
            this.label14.Text = "Current Aspect Ratio";
            // 
            // projectorDrawLabelsCheck
            // 
            this.projectorDrawLabelsCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.projectorDrawLabelsCheck.AutoSize = true;
            this.projectorDrawLabelsCheck.Location = new System.Drawing.Point(271, 325);
            this.projectorDrawLabelsCheck.Name = "projectorDrawLabelsCheck";
            this.projectorDrawLabelsCheck.Size = new System.Drawing.Size(85, 17);
            this.projectorDrawLabelsCheck.TabIndex = 54;
            this.projectorDrawLabelsCheck.Text = "Draw Labels";
            this.projectorDrawLabelsCheck.UseVisualStyleBackColor = true;
            this.projectorDrawLabelsCheck.CheckedChanged += new System.EventHandler(this.projectorDrawLabelsCheck_CheckedChanged);
            // 
            // projectorDrawCheck
            // 
            this.projectorDrawCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.projectorDrawCheck.AutoSize = true;
            this.projectorDrawCheck.Location = new System.Drawing.Point(271, 306);
            this.projectorDrawCheck.Name = "projectorDrawCheck";
            this.projectorDrawCheck.Size = new System.Drawing.Size(51, 17);
            this.projectorDrawCheck.TabIndex = 50;
            this.projectorDrawCheck.Text = "Draw";
            this.projectorDrawCheck.UseVisualStyleBackColor = true;
            this.projectorDrawCheck.CheckedChanged += new System.EventHandler(this.projectorDrawCheck_CheckedChanged);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(0, 90);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(99, 13);
            this.label17.TabIndex = 49;
            this.label17.Text = "Wall  Distance (cm)";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(-1, 254);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(65, 13);
            this.label16.TabIndex = 47;
            this.label16.Text = "Throw Ratio";
            // 
            // lockWidthButton
            // 
            this.lockWidthButton.AutoSize = true;
            this.lockWidthButton.Location = new System.Drawing.Point(6, 19);
            this.lockWidthButton.Name = "lockWidthButton";
            this.lockWidthButton.Size = new System.Drawing.Size(53, 17);
            this.lockWidthButton.TabIndex = 57;
            this.lockWidthButton.Text = "Width";
            this.lockWidthButton.UseVisualStyleBackColor = true;
            this.lockWidthButton.CheckedChanged += new System.EventHandler(this.lockWidthButton_CheckedChanged);
            // 
            // lockHeightButton
            // 
            this.lockHeightButton.AutoSize = true;
            this.lockHeightButton.Location = new System.Drawing.Point(65, 19);
            this.lockHeightButton.Name = "lockHeightButton";
            this.lockHeightButton.Size = new System.Drawing.Size(56, 17);
            this.lockHeightButton.TabIndex = 58;
            this.lockHeightButton.TabStop = true;
            this.lockHeightButton.Text = "Height";
            this.lockHeightButton.UseVisualStyleBackColor = true;
            this.lockHeightButton.CheckedChanged += new System.EventHandler(this.lockHeightButton_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 280);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 60;
            this.label1.Text = "VOffset (%W)";
            // 
            // upsideDownCheck
            // 
            this.upsideDownCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.upsideDownCheck.AutoSize = true;
            this.upsideDownCheck.Location = new System.Drawing.Point(328, 306);
            this.upsideDownCheck.Name = "upsideDownCheck";
            this.upsideDownCheck.Size = new System.Drawing.Size(90, 17);
            this.upsideDownCheck.TabIndex = 61;
            this.upsideDownCheck.Text = "Upside Down";
            this.upsideDownCheck.UseVisualStyleBackColor = true;
            this.upsideDownCheck.CheckedChanged += new System.EventHandler(this.upsideDownCheck_CheckedChanged);
            // 
            // vOffsetPanel
            // 
            this.vOffsetPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.vOffsetPanel.Location = new System.Drawing.Point(73, 280);
            this.vOffsetPanel.Max = 1F;
            this.vOffsetPanel.Min = -1F;
            this.vOffsetPanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.vOffsetPanel.Name = "vOffsetPanel";
            this.vOffsetPanel.Size = new System.Drawing.Size(349, 20);
            this.vOffsetPanel.TabIndex = 59;
            this.vOffsetPanel.Value = 0F;
            this.vOffsetPanel.ValueChanged += new System.Action<float>(this.vOffsetPanel_ValueChanged);
            // 
            // wallDistancePanel
            // 
            this.wallDistancePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.wallDistancePanel.Location = new System.Drawing.Point(105, 90);
            this.wallDistancePanel.Max = 10000F;
            this.wallDistancePanel.Min = 0F;
            this.wallDistancePanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.wallDistancePanel.Name = "wallDistancePanel";
            this.wallDistancePanel.Size = new System.Drawing.Size(320, 20);
            this.wallDistancePanel.TabIndex = 48;
            this.wallDistancePanel.Value = 0F;
            this.wallDistancePanel.ValueChanged += new System.Action<float>(this.wallDistancePanel_ValueChanged);
            // 
            // orientationPanel
            // 
            this.orientationPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.orientationPanel.Location = new System.Drawing.Point(0, 116);
            this.orientationPanel.LookAtVector = ((OpenMetaverse.Vector3)(resources.GetObject("orientationPanel.LookAtVector")));
            this.orientationPanel.MinimumSize = new System.Drawing.Size(252, 95);
            this.orientationPanel.Name = "orientationPanel";
            this.orientationPanel.Pitch = 0D;
            this.orientationPanel.Quaternion = ((OpenMetaverse.Quaternion)(resources.GetObject("orientationPanel.Quaternion")));
            this.orientationPanel.Size = new System.Drawing.Size(425, 95);
            this.orientationPanel.TabIndex = 46;
            rotation2.LookAtVector = ((OpenMetaverse.Vector3)(resources.GetObject("rotation2.LookAtVector")));
            rotation2.Pitch = 0D;
            rotation2.Quaternion = ((OpenMetaverse.Quaternion)(resources.GetObject("rotation2.Quaternion")));
            rotation2.Yaw = 0D;
            this.orientationPanel.Value = rotation2;
            this.orientationPanel.Yaw = 0D;
            // 
            // throwRatioPanel
            // 
            this.throwRatioPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.throwRatioPanel.Location = new System.Drawing.Point(73, 254);
            this.throwRatioPanel.Max = 3F;
            this.throwRatioPanel.Min = 0F;
            this.throwRatioPanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.throwRatioPanel.Name = "throwRatioPanel";
            this.throwRatioPanel.Size = new System.Drawing.Size(351, 20);
            this.throwRatioPanel.TabIndex = 45;
            this.throwRatioPanel.Value = 0F;
            this.throwRatioPanel.ValueChanged += new System.Action<float>(this.throwRatioPanel_ValueChanged);
            // 
            // positionPanel
            // 
            this.positionPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.positionPanel.Location = new System.Drawing.Point(0, 0);
            this.positionPanel.Max = 5000F;
            this.positionPanel.MaxV = ((OpenMetaverse.Vector3)(resources.GetObject("positionPanel.MaxV")));
            this.positionPanel.Min = -5000F;
            this.positionPanel.MinimumSize = new System.Drawing.Size(103, 95);
            this.positionPanel.MinV = ((OpenMetaverse.Vector3)(resources.GetObject("positionPanel.MinV")));
            this.positionPanel.Name = "positionPanel";
            this.positionPanel.Size = new System.Drawing.Size(425, 95);
            this.positionPanel.TabIndex = 44;
            this.positionPanel.Value = ((OpenMetaverse.Vector3)(resources.GetObject("positionPanel.Value")));
            this.positionPanel.X = 0F;
            this.positionPanel.Y = 0F;
            this.positionPanel.Z = 0F;
            this.positionPanel.ValueChanged += new System.EventHandler(this.projectorPositionPanel_ValueChanged);
            // 
            // lockBox
            // 
            this.lockBox.Controls.Add(this.noLockButton);
            this.lockBox.Controls.Add(this.lockPositionButton);
            this.lockBox.Controls.Add(this.lockHeightButton);
            this.lockBox.Controls.Add(this.lockWidthButton);
            this.lockBox.Location = new System.Drawing.Point(0, 306);
            this.lockBox.Name = "lockBox";
            this.lockBox.Size = new System.Drawing.Size(265, 49);
            this.lockBox.TabIndex = 63;
            this.lockBox.TabStop = false;
            this.lockBox.Text = "Lock";
            // 
            // noLockButton
            // 
            this.noLockButton.AutoSize = true;
            this.noLockButton.Checked = true;
            this.noLockButton.Location = new System.Drawing.Point(195, 19);
            this.noLockButton.Name = "noLockButton";
            this.noLockButton.Size = new System.Drawing.Size(62, 17);
            this.noLockButton.TabIndex = 60;
            this.noLockButton.TabStop = true;
            this.noLockButton.Text = "Nothing";
            this.noLockButton.UseVisualStyleBackColor = true;
            this.noLockButton.CheckedChanged += new System.EventHandler(this.noLockButton_CheckedChanged);
            // 
            // lockPositionButton
            // 
            this.lockPositionButton.AutoSize = true;
            this.lockPositionButton.Location = new System.Drawing.Point(127, 19);
            this.lockPositionButton.Name = "lockPositionButton";
            this.lockPositionButton.Size = new System.Drawing.Size(62, 17);
            this.lockPositionButton.TabIndex = 59;
            this.lockPositionButton.Text = "Position";
            this.lockPositionButton.UseVisualStyleBackColor = true;
            this.lockPositionButton.CheckedChanged += new System.EventHandler(this.lockPositionButton_CheckedChanged);
            // 
            // wallDistanceLabel
            // 
            this.wallDistanceLabel.AutoSize = true;
            this.wallDistanceLabel.Location = new System.Drawing.Point(0, 90);
            this.wallDistanceLabel.Name = "wallDistanceLabel";
            this.wallDistanceLabel.Size = new System.Drawing.Size(99, 13);
            this.wallDistanceLabel.TabIndex = 49;
            this.wallDistanceLabel.Text = "Wall  Distance (cm)";
            // 
            // ProjectorPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.upsideDownCheck);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.vOffsetPanel);
            this.Controls.Add(this.aspectRatioSplit);
            this.Controls.Add(this.projectorDrawLabelsCheck);
            this.Controls.Add(this.projectorDrawCheck);
            this.Controls.Add(this.wallDistanceLabel);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.wallDistancePanel);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.orientationPanel);
            this.Controls.Add(this.throwRatioPanel);
            this.Controls.Add(this.positionPanel);
            this.Controls.Add(this.lockBox);
            this.MinimumSize = new System.Drawing.Size(425, 358);
            this.Name = "ProjectorPanel";
            this.Size = new System.Drawing.Size(425, 358);
            this.aspectRatioSplit.Panel1.ResumeLayout(false);
            this.aspectRatioSplit.Panel1.PerformLayout();
            this.aspectRatioSplit.Panel2.ResumeLayout(false);
            this.aspectRatioSplit.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.aspectRatioSplit)).EndInit();
            this.aspectRatioSplit.ResumeLayout(false);
            this.lockBox.ResumeLayout(false);
            this.lockBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer aspectRatioSplit;
        private System.Windows.Forms.ComboBox projectorNativeAspectPulldown;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox projectorAspectPulldown;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.CheckBox projectorDrawLabelsCheck;
        private System.Windows.Forms.CheckBox projectorDrawCheck;
        private System.Windows.Forms.Label label17;
        private ScalarPanel wallDistancePanel;
        private System.Windows.Forms.Label label16;
        private RotationPanel orientationPanel;
        private ScalarPanel throwRatioPanel;
        private VectorPanel positionPanel;
        private System.Windows.Forms.RadioButton lockWidthButton;
        private System.Windows.Forms.RadioButton lockHeightButton;
        private System.Windows.Forms.Label label1;
        private ScalarPanel vOffsetPanel;
        private System.Windows.Forms.CheckBox upsideDownCheck;
        private System.Windows.Forms.GroupBox lockBox;
        private System.Windows.Forms.RadioButton noLockButton;
        private System.Windows.Forms.RadioButton lockPositionButton;
        private System.Windows.Forms.Label wallDistanceLabel;
    }
}

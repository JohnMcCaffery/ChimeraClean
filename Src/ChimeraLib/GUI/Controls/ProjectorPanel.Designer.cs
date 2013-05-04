namespace Chimera.GUI.Controls {
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
            Chimera.Util.Rotation rotation1 = new Chimera.Util.Rotation();
            this.aspectRatioSplit = new System.Windows.Forms.SplitContainer();
            this.projectorNativeAspectPulldown = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.projectorAspectPulldown = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.projectorAutoUpdateCheck = new System.Windows.Forms.CheckBox();
            this.projectorDrawLabelsCheck = new System.Windows.Forms.CheckBox();
            this.projectorDrawRoomCheck = new System.Windows.Forms.CheckBox();
            this.projectorConfigureWindowButton = new System.Windows.Forms.Button();
            this.projectorDrawCheck = new System.Windows.Forms.CheckBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.configureWindowButton = new System.Windows.Forms.RadioButton();
            this.configureProjectorButton = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.upsideDownCheck = new System.Windows.Forms.CheckBox();
            this.vOffsetPanel = new Chimera.GUI.ScalarPanel();
            this.projectorRoomPositionPanel = new Chimera.GUI.VectorPanel();
            this.projectorWallDistancePanel = new Chimera.GUI.ScalarPanel();
            this.projectorOrientationPanel = new Chimera.GUI.RotationPanel();
            this.projectorThrowRatioPanel = new Chimera.GUI.ScalarPanel();
            this.projectorPositionPanel = new Chimera.GUI.VectorPanel();
            ((System.ComponentModel.ISupportInitialize)(this.aspectRatioSplit)).BeginInit();
            this.aspectRatioSplit.Panel1.SuspendLayout();
            this.aspectRatioSplit.Panel2.SuspendLayout();
            this.aspectRatioSplit.SuspendLayout();
            this.SuspendLayout();
            // 
            // aspectRatioSplit
            // 
            this.aspectRatioSplit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.aspectRatioSplit.IsSplitterFixed = true;
            this.aspectRatioSplit.Location = new System.Drawing.Point(0, 188);
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
            this.aspectRatioSplit.Size = new System.Drawing.Size(347, 34);
            this.aspectRatioSplit.SplitterDistance = 172;
            this.aspectRatioSplit.TabIndex = 56;
            // 
            // projectorNativeAspectPulldown
            // 
            this.projectorNativeAspectPulldown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.projectorNativeAspectPulldown.FormattingEnabled = true;
            this.projectorNativeAspectPulldown.Location = new System.Drawing.Point(114, 8);
            this.projectorNativeAspectPulldown.Name = "projectorNativeAspectPulldown";
            this.projectorNativeAspectPulldown.Size = new System.Drawing.Size(59, 21);
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
            this.projectorAspectPulldown.Size = new System.Drawing.Size(54, 21);
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
            // projectorAutoUpdateCheck
            // 
            this.projectorAutoUpdateCheck.AutoSize = true;
            this.projectorAutoUpdateCheck.Location = new System.Drawing.Point(253, 306);
            this.projectorAutoUpdateCheck.Name = "projectorAutoUpdateCheck";
            this.projectorAutoUpdateCheck.Size = new System.Drawing.Size(86, 17);
            this.projectorAutoUpdateCheck.TabIndex = 55;
            this.projectorAutoUpdateCheck.Text = "Auto Update";
            this.projectorAutoUpdateCheck.UseVisualStyleBackColor = true;
            this.projectorAutoUpdateCheck.CheckedChanged += new System.EventHandler(this.projectorAutoUpdate_CheckedChanged);
            // 
            // projectorDrawLabelsCheck
            // 
            this.projectorDrawLabelsCheck.AutoSize = true;
            this.projectorDrawLabelsCheck.Location = new System.Drawing.Point(162, 306);
            this.projectorDrawLabelsCheck.Name = "projectorDrawLabelsCheck";
            this.projectorDrawLabelsCheck.Size = new System.Drawing.Size(85, 17);
            this.projectorDrawLabelsCheck.TabIndex = 54;
            this.projectorDrawLabelsCheck.Text = "Draw Labels";
            this.projectorDrawLabelsCheck.UseVisualStyleBackColor = true;
            this.projectorDrawLabelsCheck.CheckedChanged += new System.EventHandler(this.projectorDrawLabelsCheck_CheckedChanged);
            // 
            // projectorDrawRoomCheck
            // 
            this.projectorDrawRoomCheck.AutoSize = true;
            this.projectorDrawRoomCheck.Location = new System.Drawing.Point(60, 306);
            this.projectorDrawRoomCheck.Name = "projectorDrawRoomCheck";
            this.projectorDrawRoomCheck.Size = new System.Drawing.Size(82, 17);
            this.projectorDrawRoomCheck.TabIndex = 53;
            this.projectorDrawRoomCheck.Text = "Draw Room";
            this.projectorDrawRoomCheck.UseVisualStyleBackColor = true;
            this.projectorDrawRoomCheck.CheckedChanged += new System.EventHandler(this.projectorDrawRoomChecked_CheckedChanged);
            // 
            // projectorConfigureWindowButton
            // 
            this.projectorConfigureWindowButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.projectorConfigureWindowButton.Location = new System.Drawing.Point(0, 329);
            this.projectorConfigureWindowButton.Name = "projectorConfigureWindowButton";
            this.projectorConfigureWindowButton.Size = new System.Drawing.Size(201, 23);
            this.projectorConfigureWindowButton.TabIndex = 52;
            this.projectorConfigureWindowButton.Text = "Configure";
            this.projectorConfigureWindowButton.UseVisualStyleBackColor = true;
            this.projectorConfigureWindowButton.Click += new System.EventHandler(this.projectorConfigureutton_Click);
            // 
            // projectorDrawCheck
            // 
            this.projectorDrawCheck.AutoSize = true;
            this.projectorDrawCheck.Location = new System.Drawing.Point(3, 306);
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
            this.label17.Location = new System.Drawing.Point(0, 254);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(112, 13);
            this.label17.TabIndex = 49;
            this.label17.Text = "Screen  Distance (cm)";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(0, 228);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(65, 13);
            this.label16.TabIndex = 47;
            this.label16.Text = "Throw Ratio";
            // 
            // configureWindowButton
            // 
            this.configureWindowButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.configureWindowButton.AutoSize = true;
            this.configureWindowButton.Checked = true;
            this.configureWindowButton.Location = new System.Drawing.Point(207, 332);
            this.configureWindowButton.Name = "configureWindowButton";
            this.configureWindowButton.Size = new System.Drawing.Size(64, 17);
            this.configureWindowButton.TabIndex = 57;
            this.configureWindowButton.TabStop = true;
            this.configureWindowButton.Text = "Window";
            this.configureWindowButton.UseVisualStyleBackColor = true;
            this.configureWindowButton.CheckedChanged += new System.EventHandler(this.configureWindowButton_CheckedChanged);
            // 
            // configureProjectorButton
            // 
            this.configureProjectorButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.configureProjectorButton.AutoSize = true;
            this.configureProjectorButton.Location = new System.Drawing.Point(277, 332);
            this.configureProjectorButton.Name = "configureProjectorButton";
            this.configureProjectorButton.Size = new System.Drawing.Size(67, 17);
            this.configureProjectorButton.TabIndex = 58;
            this.configureProjectorButton.TabStop = true;
            this.configureProjectorButton.Text = "Projector";
            this.configureProjectorButton.UseVisualStyleBackColor = true;
            this.configureProjectorButton.CheckedChanged += new System.EventHandler(this.configureProjectorButton_CheckedChanged);
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
            this.upsideDownCheck.Location = new System.Drawing.Point(257, 283);
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
            this.vOffsetPanel.Size = new System.Drawing.Size(178, 20);
            this.vOffsetPanel.TabIndex = 59;
            this.vOffsetPanel.Value = 0F;
            this.vOffsetPanel.ValueChanged += new System.Action<float>(this.vOffsetPanel_ValueChanged);
            // 
            // projectorRoomPositionPanel
            // 
            this.projectorRoomPositionPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.projectorRoomPositionPanel.Location = new System.Drawing.Point(0, 358);
            this.projectorRoomPositionPanel.Max = 10000F;
            this.projectorRoomPositionPanel.MaxV = ((OpenMetaverse.Vector3)(resources.GetObject("projectorRoomPositionPanel.MaxV")));
            this.projectorRoomPositionPanel.Min = -10000F;
            this.projectorRoomPositionPanel.MinimumSize = new System.Drawing.Size(103, 95);
            this.projectorRoomPositionPanel.MinV = ((OpenMetaverse.Vector3)(resources.GetObject("projectorRoomPositionPanel.MinV")));
            this.projectorRoomPositionPanel.Name = "projectorRoomPositionPanel";
            this.projectorRoomPositionPanel.Size = new System.Drawing.Size(347, 95);
            this.projectorRoomPositionPanel.TabIndex = 51;
            this.projectorRoomPositionPanel.Value = ((OpenMetaverse.Vector3)(resources.GetObject("projectorRoomPositionPanel.Value")));
            this.projectorRoomPositionPanel.X = 0F;
            this.projectorRoomPositionPanel.Y = 0F;
            this.projectorRoomPositionPanel.Z = 0F;
            this.projectorRoomPositionPanel.ValueChanged += new System.EventHandler(this.projectorEyePosition_ValueChanged);
            // 
            // projectorWallDistancePanel
            // 
            this.projectorWallDistancePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.projectorWallDistancePanel.Location = new System.Drawing.Point(114, 254);
            this.projectorWallDistancePanel.Max = 10000F;
            this.projectorWallDistancePanel.Min = 0F;
            this.projectorWallDistancePanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.projectorWallDistancePanel.Name = "projectorWallDistancePanel";
            this.projectorWallDistancePanel.Size = new System.Drawing.Size(233, 20);
            this.projectorWallDistancePanel.TabIndex = 48;
            this.projectorWallDistancePanel.Value = 0F;
            this.projectorWallDistancePanel.ValueChanged += new System.Action<float>(this.wallDistancePanel_ValueChanged);
            // 
            // projectorOrientationPanel
            // 
            this.projectorOrientationPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.projectorOrientationPanel.Location = new System.Drawing.Point(0, 101);
            this.projectorOrientationPanel.LookAtVector = ((OpenMetaverse.Vector3)(resources.GetObject("projectorOrientationPanel.LookAtVector")));
            this.projectorOrientationPanel.MinimumSize = new System.Drawing.Size(252, 95);
            this.projectorOrientationPanel.Name = "projectorOrientationPanel";
            this.projectorOrientationPanel.Pitch = 0D;
            this.projectorOrientationPanel.Quaternion = ((OpenMetaverse.Quaternion)(resources.GetObject("projectorOrientationPanel.Quaternion")));
            this.projectorOrientationPanel.Size = new System.Drawing.Size(347, 95);
            this.projectorOrientationPanel.TabIndex = 46;
            rotation1.LookAtVector = ((OpenMetaverse.Vector3)(resources.GetObject("rotation1.LookAtVector")));
            rotation1.Pitch = 0D;
            rotation1.Quaternion = ((OpenMetaverse.Quaternion)(resources.GetObject("rotation1.Quaternion")));
            rotation1.Yaw = 0D;
            this.projectorOrientationPanel.Value = rotation1;
            this.projectorOrientationPanel.Yaw = 0D;
            // 
            // projectorThrowRatioPanel
            // 
            this.projectorThrowRatioPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.projectorThrowRatioPanel.Location = new System.Drawing.Point(74, 228);
            this.projectorThrowRatioPanel.Max = 3F;
            this.projectorThrowRatioPanel.Min = 0F;
            this.projectorThrowRatioPanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.projectorThrowRatioPanel.Name = "projectorThrowRatioPanel";
            this.projectorThrowRatioPanel.Size = new System.Drawing.Size(273, 20);
            this.projectorThrowRatioPanel.TabIndex = 45;
            this.projectorThrowRatioPanel.Value = 0F;
            this.projectorThrowRatioPanel.ValueChanged += new System.Action<float>(this.throwRatioPanel_ValueChanged);
            // 
            // projectorPositionPanel
            // 
            this.projectorPositionPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.projectorPositionPanel.Location = new System.Drawing.Point(0, 0);
            this.projectorPositionPanel.Max = 5000F;
            this.projectorPositionPanel.MaxV = ((OpenMetaverse.Vector3)(resources.GetObject("projectorPositionPanel.MaxV")));
            this.projectorPositionPanel.Min = -5000F;
            this.projectorPositionPanel.MinimumSize = new System.Drawing.Size(103, 95);
            this.projectorPositionPanel.MinV = ((OpenMetaverse.Vector3)(resources.GetObject("projectorPositionPanel.MinV")));
            this.projectorPositionPanel.Name = "projectorPositionPanel";
            this.projectorPositionPanel.Size = new System.Drawing.Size(347, 95);
            this.projectorPositionPanel.TabIndex = 44;
            this.projectorPositionPanel.Value = ((OpenMetaverse.Vector3)(resources.GetObject("projectorPositionPanel.Value")));
            this.projectorPositionPanel.X = 0F;
            this.projectorPositionPanel.Y = 0F;
            this.projectorPositionPanel.Z = 0F;
            this.projectorPositionPanel.ValueChanged += new System.EventHandler(this.projectorPositionPanel_ValueChanged);
            // 
            // ProjectorPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.upsideDownCheck);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.vOffsetPanel);
            this.Controls.Add(this.configureProjectorButton);
            this.Controls.Add(this.configureWindowButton);
            this.Controls.Add(this.aspectRatioSplit);
            this.Controls.Add(this.projectorAutoUpdateCheck);
            this.Controls.Add(this.projectorDrawLabelsCheck);
            this.Controls.Add(this.projectorDrawRoomCheck);
            this.Controls.Add(this.projectorConfigureWindowButton);
            this.Controls.Add(this.projectorRoomPositionPanel);
            this.Controls.Add(this.projectorDrawCheck);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.projectorWallDistancePanel);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.projectorOrientationPanel);
            this.Controls.Add(this.projectorThrowRatioPanel);
            this.Controls.Add(this.projectorPositionPanel);
            this.Name = "ProjectorPanel";
            this.Size = new System.Drawing.Size(347, 451);
            this.aspectRatioSplit.Panel1.ResumeLayout(false);
            this.aspectRatioSplit.Panel1.PerformLayout();
            this.aspectRatioSplit.Panel2.ResumeLayout(false);
            this.aspectRatioSplit.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.aspectRatioSplit)).EndInit();
            this.aspectRatioSplit.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer aspectRatioSplit;
        private System.Windows.Forms.ComboBox projectorNativeAspectPulldown;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox projectorAspectPulldown;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.CheckBox projectorAutoUpdateCheck;
        private System.Windows.Forms.CheckBox projectorDrawLabelsCheck;
        private System.Windows.Forms.CheckBox projectorDrawRoomCheck;
        private System.Windows.Forms.Button projectorConfigureWindowButton;
        private VectorPanel projectorRoomPositionPanel;
        private System.Windows.Forms.CheckBox projectorDrawCheck;
        private System.Windows.Forms.Label label17;
        private ScalarPanel projectorWallDistancePanel;
        private System.Windows.Forms.Label label16;
        private RotationPanel projectorOrientationPanel;
        private ScalarPanel projectorThrowRatioPanel;
        private VectorPanel projectorPositionPanel;
        private System.Windows.Forms.RadioButton configureWindowButton;
        private System.Windows.Forms.RadioButton configureProjectorButton;
        private System.Windows.Forms.Label label1;
        private ScalarPanel vOffsetPanel;
        private System.Windows.Forms.CheckBox upsideDownCheck;
    }
}

namespace Chimera.Kinect.GUI {
    partial class KinectPanel {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KinectPanel));
            Chimera.Util.Rotation rotation2 = new Chimera.Util.Rotation();
            this.mainTab = new System.Windows.Forms.TabControl();
            this.controlTab = new System.Windows.Forms.TabPage();
            this.headPanel = new Chimera.GUI.UpdatedVectorPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.helpTriggerPulldown = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cursorControllerPulldown = new System.Windows.Forms.ComboBox();
            this.movementControllerPulldown = new System.Windows.Forms.ComboBox();
            this.startButton = new System.Windows.Forms.Button();
            this.orientationPanel = new Chimera.GUI.RotationPanel();
            this.positionPanel = new Chimera.GUI.VectorPanel();
            this.movementTab = new System.Windows.Forms.TabPage();
            this.triggerTab = new System.Windows.Forms.TabPage();
            this.headCheck = new System.Windows.Forms.CheckBox();
            this.mainTab.SuspendLayout();
            this.controlTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainTab
            // 
            this.mainTab.Controls.Add(this.controlTab);
            this.mainTab.Controls.Add(this.movementTab);
            this.mainTab.Controls.Add(this.triggerTab);
            this.mainTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTab.Location = new System.Drawing.Point(0, 0);
            this.mainTab.Name = "mainTab";
            this.mainTab.SelectedIndex = 0;
            this.mainTab.Size = new System.Drawing.Size(709, 520);
            this.mainTab.TabIndex = 0;
            // 
            // controlTab
            // 
            this.controlTab.AutoScroll = true;
            this.controlTab.Controls.Add(this.headCheck);
            this.controlTab.Controls.Add(this.headPanel);
            this.controlTab.Controls.Add(this.label3);
            this.controlTab.Controls.Add(this.helpTriggerPulldown);
            this.controlTab.Controls.Add(this.label2);
            this.controlTab.Controls.Add(this.label1);
            this.controlTab.Controls.Add(this.cursorControllerPulldown);
            this.controlTab.Controls.Add(this.movementControllerPulldown);
            this.controlTab.Controls.Add(this.startButton);
            this.controlTab.Controls.Add(this.orientationPanel);
            this.controlTab.Controls.Add(this.positionPanel);
            this.controlTab.Location = new System.Drawing.Point(4, 22);
            this.controlTab.Name = "controlTab";
            this.controlTab.Padding = new System.Windows.Forms.Padding(3);
            this.controlTab.Size = new System.Drawing.Size(701, 494);
            this.controlTab.TabIndex = 0;
            this.controlTab.Text = "Control";
            this.controlTab.UseVisualStyleBackColor = true;
            // 
            // headPanel
            // 
            this.headPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.headPanel.Location = new System.Drawing.Point(3, 312);
            this.headPanel.Max = 10F;
            this.headPanel.MaxV = ((OpenMetaverse.Vector3)(resources.GetObject("headPanel.MaxV")));
            this.headPanel.Min = -10F;
            this.headPanel.MinimumSize = new System.Drawing.Size(103, 95);
            this.headPanel.MinV = ((OpenMetaverse.Vector3)(resources.GetObject("headPanel.MinV")));
            this.headPanel.Name = "headPanel";
            this.headPanel.Size = new System.Drawing.Size(695, 95);
            this.headPanel.TabIndex = 11;
            this.headPanel.Value = ((OpenMetaverse.Vector3)(resources.GetObject("headPanel.Value")));
            this.headPanel.Vector = null;
            this.headPanel.X = 0F;
            this.headPanel.Y = 0F;
            this.headPanel.Z = 0F;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 261);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Triggers";
            // 
            // helpTriggerPulldown
            // 
            this.helpTriggerPulldown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.helpTriggerPulldown.FormattingEnabled = true;
            this.helpTriggerPulldown.Location = new System.Drawing.Point(116, 258);
            this.helpTriggerPulldown.Name = "helpTriggerPulldown";
            this.helpTriggerPulldown.Size = new System.Drawing.Size(579, 21);
            this.helpTriggerPulldown.TabIndex = 9;
            this.helpTriggerPulldown.SelectedIndexChanged += new System.EventHandler(this.helpTriggerPulldown_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 288);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Cursor Controller";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 234);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Movement Controller";
            // 
            // cursorControllerPulldown
            // 
            this.cursorControllerPulldown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cursorControllerPulldown.FormattingEnabled = true;
            this.cursorControllerPulldown.Location = new System.Drawing.Point(116, 285);
            this.cursorControllerPulldown.Name = "cursorControllerPulldown";
            this.cursorControllerPulldown.Size = new System.Drawing.Size(579, 21);
            this.cursorControllerPulldown.TabIndex = 6;
            this.cursorControllerPulldown.SelectedIndexChanged += new System.EventHandler(this.cursorControllerPulldown_SelectedIndexChanged);
            // 
            // movementControllerPulldown
            // 
            this.movementControllerPulldown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.movementControllerPulldown.FormattingEnabled = true;
            this.movementControllerPulldown.Location = new System.Drawing.Point(116, 231);
            this.movementControllerPulldown.Name = "movementControllerPulldown";
            this.movementControllerPulldown.Size = new System.Drawing.Size(579, 21);
            this.movementControllerPulldown.TabIndex = 5;
            this.movementControllerPulldown.SelectedIndexChanged += new System.EventHandler(this.movementPulldown_SelectedIndexChanged);
            // 
            // startButton
            // 
            this.startButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.startButton.Location = new System.Drawing.Point(0, 0);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(705, 23);
            this.startButton.TabIndex = 4;
            this.startButton.Text = "Begin";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // orientationPanel
            // 
            this.orientationPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.orientationPanel.Location = new System.Drawing.Point(0, 130);
            this.orientationPanel.LookAtVector = ((OpenMetaverse.Vector3)(resources.GetObject("orientationPanel.LookAtVector")));
            this.orientationPanel.MinimumSize = new System.Drawing.Size(252, 95);
            this.orientationPanel.Name = "orientationPanel";
            this.orientationPanel.Pitch = 0D;
            this.orientationPanel.Quaternion = ((OpenMetaverse.Quaternion)(resources.GetObject("orientationPanel.Quaternion")));
            this.orientationPanel.Size = new System.Drawing.Size(701, 95);
            this.orientationPanel.TabIndex = 2;
            rotation2.LookAtVector = ((OpenMetaverse.Vector3)(resources.GetObject("rotation2.LookAtVector")));
            rotation2.Pitch = 0D;
            rotation2.Quaternion = ((OpenMetaverse.Quaternion)(resources.GetObject("rotation2.Quaternion")));
            rotation2.Yaw = 0D;
            this.orientationPanel.Value = rotation2;
            this.orientationPanel.Yaw = 0D;
            // 
            // positionPanel
            // 
            this.positionPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.positionPanel.Location = new System.Drawing.Point(0, 29);
            this.positionPanel.Max = 1024F;
            this.positionPanel.MaxV = ((OpenMetaverse.Vector3)(resources.GetObject("positionPanel.MaxV")));
            this.positionPanel.Min = -1024F;
            this.positionPanel.MinimumSize = new System.Drawing.Size(103, 95);
            this.positionPanel.MinV = ((OpenMetaverse.Vector3)(resources.GetObject("positionPanel.MinV")));
            this.positionPanel.Name = "positionPanel";
            this.positionPanel.Size = new System.Drawing.Size(701, 95);
            this.positionPanel.TabIndex = 1;
            this.positionPanel.Value = ((OpenMetaverse.Vector3)(resources.GetObject("positionPanel.Value")));
            this.positionPanel.X = 0F;
            this.positionPanel.Y = 0F;
            this.positionPanel.Z = 0F;
            this.positionPanel.ValueChanged += new System.EventHandler(this.positionPanel_ValueChanged);
            // 
            // movementTab
            // 
            this.movementTab.Location = new System.Drawing.Point(4, 22);
            this.movementTab.Name = "movementTab";
            this.movementTab.Padding = new System.Windows.Forms.Padding(3);
            this.movementTab.Size = new System.Drawing.Size(701, 494);
            this.movementTab.TabIndex = 1;
            this.movementTab.Text = "Movement";
            this.movementTab.UseVisualStyleBackColor = true;
            // 
            // triggerTab
            // 
            this.triggerTab.Location = new System.Drawing.Point(4, 22);
            this.triggerTab.Name = "triggerTab";
            this.triggerTab.Padding = new System.Windows.Forms.Padding(3);
            this.triggerTab.Size = new System.Drawing.Size(701, 494);
            this.triggerTab.TabIndex = 2;
            this.triggerTab.Text = "CustomTrigger";
            this.triggerTab.UseVisualStyleBackColor = true;
            // 
            // headCheck
            // 
            this.headCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.headCheck.AutoSize = true;
            this.headCheck.Location = new System.Drawing.Point(573, 312);
            this.headCheck.Name = "headCheck";
            this.headCheck.Size = new System.Drawing.Size(128, 17);
            this.headCheck.TabIndex = 12;
            this.headCheck.Text = "Control Head Position";
            this.headCheck.UseVisualStyleBackColor = true;
            this.headCheck.CheckedChanged += new System.EventHandler(this.headCheck_CheckedChanged);
            // 
            // KinectPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainTab);
            this.Name = "KinectPanel";
            this.Size = new System.Drawing.Size(709, 520);
            this.mainTab.ResumeLayout(false);
            this.controlTab.ResumeLayout(false);
            this.controlTab.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl mainTab;
        private System.Windows.Forms.TabPage controlTab;
        private System.Windows.Forms.TabPage movementTab;
        private Chimera.GUI.RotationPanel orientationPanel;
        private Chimera.GUI.VectorPanel positionPanel;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.ComboBox cursorControllerPulldown;
        private System.Windows.Forms.ComboBox movementControllerPulldown;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox helpTriggerPulldown;
        private System.Windows.Forms.TabPage triggerTab;
        private Chimera.GUI.UpdatedVectorPanel headPanel;
        private System.Windows.Forms.CheckBox headCheck;
    }
}

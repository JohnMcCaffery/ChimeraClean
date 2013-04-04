namespace Chimera.OpenSim.GUI {
    partial class InputPanel {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InputPanel));
            this.activeCheckbox = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.type13Label = new System.Windows.Forms.Label();
            this.type17Label = new System.Windows.Forms.Label();
            this.type9Label = new System.Windows.Forms.Label();
            this.type10Label = new System.Windows.Forms.Label();
            this.type11Label = new System.Windows.Forms.Label();
            this.type6Label = new System.Windows.Forms.Label();
            this.type7Label = new System.Windows.Forms.Label();
            this.type8Label = new System.Windows.Forms.Label();
            this.type1Label = new System.Windows.Forms.Label();
            this.focusOffsetPanel = new Chimera.GUI.ScalarPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.focusOffset3DPanel = new Chimera.GUI.VectorPanel();
            this.pitchPanel = new Chimera.GUI.ScalarPanel();
            this.lookAtLagPanel = new Chimera.GUI.ScalarPanel();
            this.focusLagPanel = new Chimera.GUI.ScalarPanel();
            this.distancePanel = new Chimera.GUI.ScalarPanel();
            this.focusThresholdPanel = new Chimera.GUI.ScalarPanel();
            this.lookAtThresholdPanel = new Chimera.GUI.ScalarPanel();
            this.behindnessLagPanel = new Chimera.GUI.ScalarPanel();
            this.behindnessAnglePanel = new Chimera.GUI.ScalarPanel();
            this.focusPanel = new Chimera.GUI.ScalarPanel();
            this.lookAtPanel = new Chimera.GUI.ScalarPanel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // activeCheckbox
            // 
            this.activeCheckbox.AutoSize = true;
            this.activeCheckbox.CheckAlign = System.Drawing.ContentAlignment.BottomRight;
            this.activeCheckbox.Location = new System.Drawing.Point(7, 180);
            this.activeCheckbox.Name = "activeCheckbox";
            this.activeCheckbox.Size = new System.Drawing.Size(59, 17);
            this.activeCheckbox.TabIndex = 127;
            this.activeCheckbox.Text = "Control";
            this.activeCheckbox.UseVisualStyleBackColor = true;
            this.activeCheckbox.CheckedChanged += new System.EventHandler(this.activeCheckbox_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 13);
            this.label2.TabIndex = 125;
            this.label2.Text = "Focus Threshold";
            // 
            // type13Label
            // 
            this.type13Label.AutoSize = true;
            this.type13Label.Location = new System.Drawing.Point(54, 130);
            this.type13Label.Name = "type13Label";
            this.type13Label.Size = new System.Drawing.Size(41, 13);
            this.type13Label.TabIndex = 123;
            this.type13Label.Text = "LookAt";
            // 
            // type17Label
            // 
            this.type17Label.AutoSize = true;
            this.type17Label.Location = new System.Drawing.Point(59, 156);
            this.type17Label.Name = "type17Label";
            this.type17Label.Size = new System.Drawing.Size(36, 13);
            this.type17Label.TabIndex = 121;
            this.type17Label.Text = "Focus";
            // 
            // type9Label
            // 
            this.type9Label.AutoSize = true;
            this.type9Label.Location = new System.Drawing.Point(3, 0);
            this.type9Label.Name = "type9Label";
            this.type9Label.Size = new System.Drawing.Size(92, 13);
            this.type9Label.TabIndex = 119;
            this.type9Label.Text = "Behindness Angle";
            // 
            // type10Label
            // 
            this.type10Label.AutoSize = true;
            this.type10Label.Location = new System.Drawing.Point(12, 28);
            this.type10Label.Name = "type10Label";
            this.type10Label.Size = new System.Drawing.Size(83, 13);
            this.type10Label.TabIndex = 117;
            this.type10Label.Text = "Behindness Lag";
            // 
            // type11Label
            // 
            this.type11Label.AutoSize = true;
            this.type11Label.Location = new System.Drawing.Point(4, 51);
            this.type11Label.Name = "type11Label";
            this.type11Label.Size = new System.Drawing.Size(91, 13);
            this.type11Label.TabIndex = 115;
            this.type11Label.Text = "LookAt Threshold";
            // 
            // type6Label
            // 
            this.type6Label.AutoSize = true;
            this.type6Label.Location = new System.Drawing.Point(5, 155);
            this.type6Label.Name = "type6Label";
            this.type6Label.Size = new System.Drawing.Size(62, 13);
            this.type6Label.TabIndex = 113;
            this.type6Label.Text = "LookAt Lag";
            // 
            // type7Label
            // 
            this.type7Label.AutoSize = true;
            this.type7Label.Location = new System.Drawing.Point(13, 181);
            this.type7Label.Name = "type7Label";
            this.type7Label.Size = new System.Drawing.Size(57, 13);
            this.type7Label.TabIndex = 111;
            this.type7Label.Text = "Focus Lag";
            // 
            // type8Label
            // 
            this.type8Label.AutoSize = true;
            this.type8Label.Location = new System.Drawing.Point(46, 106);
            this.type8Label.Name = "type8Label";
            this.type8Label.Size = new System.Drawing.Size(49, 13);
            this.type8Label.TabIndex = 109;
            this.type8Label.Text = "Distance";
            // 
            // type1Label
            // 
            this.type1Label.AutoSize = true;
            this.type1Label.Location = new System.Drawing.Point(39, 127);
            this.type1Label.Name = "type1Label";
            this.type1Label.Size = new System.Drawing.Size(31, 13);
            this.type1Label.TabIndex = 105;
            this.type1Label.Text = "Pitch";
            // 
            // focusOffsetPanel
            // 
            this.focusOffsetPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.focusOffsetPanel.Location = new System.Drawing.Point(76, 101);
            this.focusOffsetPanel.Max = 10F;
            this.focusOffsetPanel.Min = -10F;
            this.focusOffsetPanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.focusOffsetPanel.Name = "focusOffsetPanel";
            this.focusOffsetPanel.Size = new System.Drawing.Size(388, 20);
            this.focusOffsetPanel.TabIndex = 128;
            this.focusOffsetPanel.Value = 0F;
            this.focusOffsetPanel.ValueChanged += new System.Action<float>(this.focusOffsetPanel_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 101);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 129;
            this.label1.Text = "Focus Offset";
            // 
            // focusOffset3DPanel
            // 
            this.focusOffset3DPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.focusOffset3DPanel.Location = new System.Drawing.Point(3, 0);
            this.focusOffset3DPanel.Max = 10F;
            this.focusOffset3DPanel.MaxV = ((OpenMetaverse.Vector3)(resources.GetObject("focusOffset3DPanel.MaxV")));
            this.focusOffset3DPanel.Min = -10F;
            this.focusOffset3DPanel.MinimumSize = new System.Drawing.Size(103, 95);
            this.focusOffset3DPanel.MinV = ((OpenMetaverse.Vector3)(resources.GetObject("focusOffset3DPanel.MinV")));
            this.focusOffset3DPanel.Name = "focusOffset3DPanel";
            this.focusOffset3DPanel.Size = new System.Drawing.Size(461, 95);
            this.focusOffset3DPanel.TabIndex = 130;
            this.focusOffset3DPanel.Value = ((OpenMetaverse.Vector3)(resources.GetObject("focusOffset3DPanel.Value")));
            this.focusOffset3DPanel.X = 0F;
            this.focusOffset3DPanel.Y = 0F;
            this.focusOffset3DPanel.Z = 0F;
            this.focusOffset3DPanel.ValueChanged += new System.EventHandler(this.focusOffset3DPanel_ValueChanged);
            // 
            // pitchPanel
            // 
            this.pitchPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pitchPanel.Location = new System.Drawing.Point(76, 127);
            this.pitchPanel.Max = 10F;
            this.pitchPanel.Min = -10F;
            this.pitchPanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.pitchPanel.Name = "pitchPanel";
            this.pitchPanel.Size = new System.Drawing.Size(388, 20);
            this.pitchPanel.TabIndex = 131;
            this.pitchPanel.Value = 0F;
            this.pitchPanel.ValueChanged += new System.Action<float>(this.pitchPanel_ValueChanged);
            // 
            // lookAtLagPanel
            // 
            this.lookAtLagPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lookAtLagPanel.Location = new System.Drawing.Point(76, 153);
            this.lookAtLagPanel.Max = 10F;
            this.lookAtLagPanel.Min = -10F;
            this.lookAtLagPanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.lookAtLagPanel.Name = "lookAtLagPanel";
            this.lookAtLagPanel.Size = new System.Drawing.Size(388, 20);
            this.lookAtLagPanel.TabIndex = 132;
            this.lookAtLagPanel.Value = 0F;
            this.lookAtLagPanel.ValueChanged += new System.Action<float>(this.lookAtLagPanel_ValueChanged);
            // 
            // focusLagPanel
            // 
            this.focusLagPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.focusLagPanel.Location = new System.Drawing.Point(76, 179);
            this.focusLagPanel.Max = 10F;
            this.focusLagPanel.Min = -10F;
            this.focusLagPanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.focusLagPanel.Name = "focusLagPanel";
            this.focusLagPanel.Size = new System.Drawing.Size(388, 20);
            this.focusLagPanel.TabIndex = 133;
            this.focusLagPanel.Value = 0F;
            this.focusLagPanel.ValueChanged += new System.Action<float>(this.focusLagPanel_ValueChanged);
            // 
            // distancePanel
            // 
            this.distancePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.distancePanel.Location = new System.Drawing.Point(101, 104);
            this.distancePanel.Max = 10F;
            this.distancePanel.Min = -10F;
            this.distancePanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.distancePanel.Name = "distancePanel";
            this.distancePanel.Size = new System.Drawing.Size(401, 20);
            this.distancePanel.TabIndex = 134;
            this.distancePanel.Value = 0F;
            this.distancePanel.ValueChanged += new System.Action<float>(this.distancePanel_ValueChanged);
            // 
            // focusThresholdPanel
            // 
            this.focusThresholdPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.focusThresholdPanel.Location = new System.Drawing.Point(101, 78);
            this.focusThresholdPanel.Max = 10F;
            this.focusThresholdPanel.Min = -10F;
            this.focusThresholdPanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.focusThresholdPanel.Name = "focusThresholdPanel";
            this.focusThresholdPanel.Size = new System.Drawing.Size(401, 20);
            this.focusThresholdPanel.TabIndex = 138;
            this.focusThresholdPanel.Value = 0F;
            this.focusThresholdPanel.ValueChanged += new System.Action<float>(this.focusThresholdPanel_ValueChanged);
            // 
            // lookAtThresholdPanel
            // 
            this.lookAtThresholdPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lookAtThresholdPanel.Location = new System.Drawing.Point(101, 52);
            this.lookAtThresholdPanel.Max = 10F;
            this.lookAtThresholdPanel.Min = -10F;
            this.lookAtThresholdPanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.lookAtThresholdPanel.Name = "lookAtThresholdPanel";
            this.lookAtThresholdPanel.Size = new System.Drawing.Size(401, 20);
            this.lookAtThresholdPanel.TabIndex = 137;
            this.lookAtThresholdPanel.Value = 0F;
            this.lookAtThresholdPanel.ValueChanged += new System.Action<float>(this.lookAtThresholdPanel_ValueChanged);
            // 
            // behindnessLagPanel
            // 
            this.behindnessLagPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.behindnessLagPanel.Location = new System.Drawing.Point(101, 26);
            this.behindnessLagPanel.Max = 10F;
            this.behindnessLagPanel.Min = -10F;
            this.behindnessLagPanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.behindnessLagPanel.Name = "behindnessLagPanel";
            this.behindnessLagPanel.Size = new System.Drawing.Size(401, 20);
            this.behindnessLagPanel.TabIndex = 136;
            this.behindnessLagPanel.Value = 0F;
            this.behindnessLagPanel.ValueChanged += new System.Action<float>(this.behindnessLagPanel_ValueChanged);
            // 
            // behindnessAnglePanel
            // 
            this.behindnessAnglePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.behindnessAnglePanel.Location = new System.Drawing.Point(101, 0);
            this.behindnessAnglePanel.Max = 10F;
            this.behindnessAnglePanel.Min = -10F;
            this.behindnessAnglePanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.behindnessAnglePanel.Name = "behindnessAnglePanel";
            this.behindnessAnglePanel.Size = new System.Drawing.Size(401, 20);
            this.behindnessAnglePanel.TabIndex = 135;
            this.behindnessAnglePanel.Value = 0F;
            this.behindnessAnglePanel.ValueChanged += new System.Action<float>(this.behindnessAnglePanel_ValueChanged);
            // 
            // focusPanel
            // 
            this.focusPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.focusPanel.Location = new System.Drawing.Point(101, 156);
            this.focusPanel.Max = 10F;
            this.focusPanel.Min = -10F;
            this.focusPanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.focusPanel.Name = "focusPanel";
            this.focusPanel.Size = new System.Drawing.Size(401, 20);
            this.focusPanel.TabIndex = 142;
            this.focusPanel.Value = 0F;
            this.focusPanel.ValueChanged += new System.Action<float>(this.focusPanel_ValueChanged);
            // 
            // lookAtPanel
            // 
            this.lookAtPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lookAtPanel.Location = new System.Drawing.Point(101, 130);
            this.lookAtPanel.Max = 10F;
            this.lookAtPanel.Min = -10F;
            this.lookAtPanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.lookAtPanel.Name = "lookAtPanel";
            this.lookAtPanel.Size = new System.Drawing.Size(401, 20);
            this.lookAtPanel.TabIndex = 141;
            this.lookAtPanel.Value = 0F;
            this.lookAtPanel.ValueChanged += new System.Action<float>(this.lookAtPanel_ValueChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.focusOffset3DPanel);
            this.splitContainer1.Panel1.Controls.Add(this.type1Label);
            this.splitContainer1.Panel1.Controls.Add(this.type7Label);
            this.splitContainer1.Panel1.Controls.Add(this.type6Label);
            this.splitContainer1.Panel1.Controls.Add(this.focusOffsetPanel);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.pitchPanel);
            this.splitContainer1.Panel1.Controls.Add(this.lookAtLagPanel);
            this.splitContainer1.Panel1.Controls.Add(this.focusLagPanel);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.behindnessAnglePanel);
            this.splitContainer1.Panel2.Controls.Add(this.focusPanel);
            this.splitContainer1.Panel2.Controls.Add(this.lookAtPanel);
            this.splitContainer1.Panel2.Controls.Add(this.type8Label);
            this.splitContainer1.Panel2.Controls.Add(this.activeCheckbox);
            this.splitContainer1.Panel2.Controls.Add(this.type11Label);
            this.splitContainer1.Panel2.Controls.Add(this.focusThresholdPanel);
            this.splitContainer1.Panel2.Controls.Add(this.type10Label);
            this.splitContainer1.Panel2.Controls.Add(this.lookAtThresholdPanel);
            this.splitContainer1.Panel2.Controls.Add(this.type9Label);
            this.splitContainer1.Panel2.Controls.Add(this.behindnessLagPanel);
            this.splitContainer1.Panel2.Controls.Add(this.type17Label);
            this.splitContainer1.Panel2.Controls.Add(this.type13Label);
            this.splitContainer1.Panel2.Controls.Add(this.distancePanel);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Size = new System.Drawing.Size(979, 571);
            this.splitContainer1.SplitterDistance = 470;
            this.splitContainer1.TabIndex = 143;
            // 
            // InputPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "InputPanel";
            this.Size = new System.Drawing.Size(979, 571);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox activeCheckbox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label type13Label;
        private System.Windows.Forms.Label type17Label;
        private System.Windows.Forms.Label type9Label;
        private System.Windows.Forms.Label type10Label;
        private System.Windows.Forms.Label type11Label;
        private System.Windows.Forms.Label type6Label;
        private System.Windows.Forms.Label type7Label;
        private System.Windows.Forms.Label type8Label;
        private System.Windows.Forms.Label type1Label;
        private Chimera.GUI.ScalarPanel focusOffsetPanel;
        private System.Windows.Forms.Label label1;
        private Chimera.GUI.VectorPanel focusOffset3DPanel;
        private Chimera.GUI.ScalarPanel pitchPanel;
        private Chimera.GUI.ScalarPanel lookAtLagPanel;
        private Chimera.GUI.ScalarPanel focusLagPanel;
        private Chimera.GUI.ScalarPanel distancePanel;
        private Chimera.GUI.ScalarPanel focusThresholdPanel;
        private Chimera.GUI.ScalarPanel lookAtThresholdPanel;
        private Chimera.GUI.ScalarPanel behindnessLagPanel;
        private Chimera.GUI.ScalarPanel behindnessAnglePanel;
        private Chimera.GUI.ScalarPanel focusPanel;
        private Chimera.GUI.ScalarPanel lookAtPanel;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}

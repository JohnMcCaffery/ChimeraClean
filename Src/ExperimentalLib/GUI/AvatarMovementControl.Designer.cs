namespace Chimera.Experimental.GUI {
    partial class AvatarMovementControl {
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
            this.startButton = new System.Windows.Forms.Button();
            this.statusLabel = new System.Windows.Forms.Label();
            this.yawRatePanel = new Chimera.GUI.ScalarPanel();
            this.pitchRatePanel = new Chimera.GUI.ScalarPanel();
            this.moveRatePanel = new Chimera.GUI.ScalarPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.distanceThresholdPanel = new Chimera.GUI.ScalarPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.stopButton = new System.Windows.Forms.Button();
            this.heightOffsetPanel = new Chimera.GUI.ScalarPanel();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // startButton
            // 
            this.startButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.startButton.Location = new System.Drawing.Point(3, 3);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(81, 23);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(3, 159);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(37, 13);
            this.statusLabel.TabIndex = 1;
            this.statusLabel.Text = "Status";
            // 
            // yawRatePanel
            // 
            this.yawRatePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.yawRatePanel.Location = new System.Drawing.Point(3, 32);
            this.yawRatePanel.Max = 1F;
            this.yawRatePanel.Min = 0F;
            this.yawRatePanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.yawRatePanel.Name = "yawRatePanel";
            this.yawRatePanel.Size = new System.Drawing.Size(298, 20);
            this.yawRatePanel.TabIndex = 2;
            this.yawRatePanel.Value = 0.005F;
            this.yawRatePanel.ValueChanged += new System.Action<float>(this.yawRatePanel_ValueChanged);
            // 
            // pitchRatePanel
            // 
            this.pitchRatePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pitchRatePanel.Location = new System.Drawing.Point(3, 58);
            this.pitchRatePanel.Max = 1F;
            this.pitchRatePanel.Min = 0F;
            this.pitchRatePanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.pitchRatePanel.Name = "pitchRatePanel";
            this.pitchRatePanel.Size = new System.Drawing.Size(298, 20);
            this.pitchRatePanel.TabIndex = 3;
            this.pitchRatePanel.Value = 0.005F;
            this.pitchRatePanel.ValueChanged += new System.Action<float>(this.pitchPanel_ValueChanged);
            // 
            // moveRatePanel
            // 
            this.moveRatePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.moveRatePanel.Location = new System.Drawing.Point(3, 84);
            this.moveRatePanel.Max = 1F;
            this.moveRatePanel.Min = 0F;
            this.moveRatePanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.moveRatePanel.Name = "moveRatePanel";
            this.moveRatePanel.Size = new System.Drawing.Size(298, 20);
            this.moveRatePanel.TabIndex = 4;
            this.moveRatePanel.Value = 0.005F;
            this.moveRatePanel.ValueChanged += new System.Action<float>(this.movePanel_ValueChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(273, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Yaw";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(270, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Pitch";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(267, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Move";
            // 
            // distanceThresholdPanel
            // 
            this.distanceThresholdPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.distanceThresholdPanel.Location = new System.Drawing.Point(0, 110);
            this.distanceThresholdPanel.Max = 10F;
            this.distanceThresholdPanel.Min = 0F;
            this.distanceThresholdPanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.distanceThresholdPanel.Name = "distanceThresholdPanel";
            this.distanceThresholdPanel.Size = new System.Drawing.Size(298, 20);
            this.distanceThresholdPanel.TabIndex = 8;
            this.distanceThresholdPanel.Value = 0.5F;
            this.distanceThresholdPanel.ValueChanged += new System.Action<float>(this.targetThresholdPanel_ValueChanged);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(213, 107);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Target Threshold";
            // 
            // stopButton
            // 
            this.stopButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.stopButton.Location = new System.Drawing.Point(90, 3);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 10;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // heightOffsetPanel
            // 
            this.heightOffsetPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.heightOffsetPanel.Location = new System.Drawing.Point(0, 136);
            this.heightOffsetPanel.Max = 4F;
            this.heightOffsetPanel.Min = 0F;
            this.heightOffsetPanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.heightOffsetPanel.Name = "heightOffsetPanel";
            this.heightOffsetPanel.Size = new System.Drawing.Size(298, 20);
            this.heightOffsetPanel.TabIndex = 11;
            this.heightOffsetPanel.Value = 0.5F;
            this.heightOffsetPanel.ValueChanged += new System.Action<float>(this.heightOffsetPanel_ValueChanged);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(232, 133);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Height Offset";
            // 
            // AvatarMovementControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label5);
            this.Controls.Add(this.heightOffsetPanel);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.distanceThresholdPanel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.moveRatePanel);
            this.Controls.Add(this.pitchRatePanel);
            this.Controls.Add(this.yawRatePanel);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.startButton);
            this.Name = "AvatarMovementControl";
            this.Size = new System.Drawing.Size(304, 190);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Label statusLabel;
        private Chimera.GUI.ScalarPanel yawRatePanel;
        private Chimera.GUI.ScalarPanel pitchRatePanel;
        private Chimera.GUI.ScalarPanel moveRatePanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private Chimera.GUI.ScalarPanel distanceThresholdPanel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button stopButton;
        private Chimera.GUI.ScalarPanel heightOffsetPanel;
        private System.Windows.Forms.Label label5;
    }
}

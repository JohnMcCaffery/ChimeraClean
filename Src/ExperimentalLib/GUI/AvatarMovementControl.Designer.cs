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
            this.turnRatePanel = new Chimera.GUI.ScalarPanel();
            this.moveRatePanel = new Chimera.GUI.ScalarPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.distanceThresholdPanel = new Chimera.GUI.ScalarPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.pauseButton = new System.Windows.Forms.Button();
            this.heightOffsetPanel = new Chimera.GUI.ScalarPanel();
            this.label5 = new System.Windows.Forms.Label();
            this.drawMapButton = new System.Windows.Forms.Button();
            this.mapFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.reloadTargetsButton = new System.Windows.Forms.Button();
            this.restartButton = new System.Windows.Forms.Button();
            this.prevButton = new System.Windows.Forms.Button();
            this.nextButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // startButton
            // 
            this.startButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.startButton.Location = new System.Drawing.Point(3, 3);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(34, 23);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(3, 133);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(37, 13);
            this.statusLabel.TabIndex = 1;
            this.statusLabel.Text = "Status";
            // 
            // turnRatePanel
            // 
            this.turnRatePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.turnRatePanel.Location = new System.Drawing.Point(3, 32);
            this.turnRatePanel.Max = 2F;
            this.turnRatePanel.Min = 0F;
            this.turnRatePanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.turnRatePanel.Name = "turnRatePanel";
            this.turnRatePanel.Size = new System.Drawing.Size(508, 20);
            this.turnRatePanel.TabIndex = 2;
            this.turnRatePanel.Value = 0.85F;
            this.turnRatePanel.ValueChanged += new System.Action<float>(this.turnRatePanel_ValueChanged);
            // 
            // moveRatePanel
            // 
            this.moveRatePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.moveRatePanel.Location = new System.Drawing.Point(3, 58);
            this.moveRatePanel.Max = 1F;
            this.moveRatePanel.Min = 0F;
            this.moveRatePanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.moveRatePanel.Name = "moveRatePanel";
            this.moveRatePanel.Size = new System.Drawing.Size(508, 20);
            this.moveRatePanel.TabIndex = 4;
            this.moveRatePanel.Value = 0.005F;
            this.moveRatePanel.ValueChanged += new System.Action<float>(this.movePanel_ValueChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(451, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Turn Speed";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(443, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Move Speed";
            // 
            // distanceThresholdPanel
            // 
            this.distanceThresholdPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.distanceThresholdPanel.Location = new System.Drawing.Point(3, 84);
            this.distanceThresholdPanel.Max = 10F;
            this.distanceThresholdPanel.Min = 0F;
            this.distanceThresholdPanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.distanceThresholdPanel.Name = "distanceThresholdPanel";
            this.distanceThresholdPanel.Size = new System.Drawing.Size(508, 20);
            this.distanceThresholdPanel.TabIndex = 8;
            this.distanceThresholdPanel.Value = 0.5F;
            this.distanceThresholdPanel.ValueChanged += new System.Action<float>(this.targetThresholdPanel_ValueChanged);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(423, 84);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Target Threshold";
            // 
            // pauseButton
            // 
            this.pauseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pauseButton.Location = new System.Drawing.Point(329, 3);
            this.pauseButton.Name = "pauseButton";
            this.pauseButton.Size = new System.Drawing.Size(46, 23);
            this.pauseButton.TabIndex = 10;
            this.pauseButton.Text = "Pause";
            this.pauseButton.UseVisualStyleBackColor = true;
            this.pauseButton.Click += new System.EventHandler(this.pauseButton_Click);
            // 
            // heightOffsetPanel
            // 
            this.heightOffsetPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.heightOffsetPanel.Location = new System.Drawing.Point(3, 110);
            this.heightOffsetPanel.Max = 4F;
            this.heightOffsetPanel.Min = 0F;
            this.heightOffsetPanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.heightOffsetPanel.Name = "heightOffsetPanel";
            this.heightOffsetPanel.Size = new System.Drawing.Size(508, 20);
            this.heightOffsetPanel.TabIndex = 11;
            this.heightOffsetPanel.Value = 0.5F;
            this.heightOffsetPanel.ValueChanged += new System.Action<float>(this.heightOffsetPanel_ValueChanged);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(442, 110);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Height Offset";
            // 
            // drawMapButton
            // 
            this.drawMapButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.drawMapButton.Location = new System.Drawing.Point(248, 3);
            this.drawMapButton.Name = "drawMapButton";
            this.drawMapButton.Size = new System.Drawing.Size(75, 23);
            this.drawMapButton.TabIndex = 13;
            this.drawMapButton.Text = "Draw Map";
            this.drawMapButton.UseVisualStyleBackColor = true;
            this.drawMapButton.Click += new System.EventHandler(this.drawMapButton_Click);
            // 
            // mapFileDialog
            // 
            this.mapFileDialog.FileName = "mapFileDialog";
            // 
            // reloadTargetsButton
            // 
            this.reloadTargetsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.reloadTargetsButton.Location = new System.Drawing.Point(147, 3);
            this.reloadTargetsButton.Name = "reloadTargetsButton";
            this.reloadTargetsButton.Size = new System.Drawing.Size(95, 23);
            this.reloadTargetsButton.TabIndex = 14;
            this.reloadTargetsButton.Text = "ReLoad Targets";
            this.reloadTargetsButton.UseVisualStyleBackColor = true;
            this.reloadTargetsButton.Click += new System.EventHandler(this.reloadTargetsButton_Click);
            // 
            // restartButton
            // 
            this.restartButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.restartButton.Location = new System.Drawing.Point(89, 3);
            this.restartButton.Name = "restartButton";
            this.restartButton.Size = new System.Drawing.Size(52, 23);
            this.restartButton.TabIndex = 15;
            this.restartButton.Text = "Restart";
            this.restartButton.UseVisualStyleBackColor = true;
            this.restartButton.Click += new System.EventHandler(this.restartButton_Click);
            // 
            // prevButton
            // 
            this.prevButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.prevButton.Location = new System.Drawing.Point(43, 3);
            this.prevButton.Name = "prevButton";
            this.prevButton.Size = new System.Drawing.Size(17, 23);
            this.prevButton.TabIndex = 16;
            this.prevButton.Text = "<";
            this.prevButton.UseVisualStyleBackColor = true;
            this.prevButton.Click += new System.EventHandler(this.prevButton_Click);
            // 
            // nextButton
            // 
            this.nextButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nextButton.Location = new System.Drawing.Point(66, 3);
            this.nextButton.Name = "nextButton";
            this.nextButton.Size = new System.Drawing.Size(17, 23);
            this.nextButton.TabIndex = 17;
            this.nextButton.Text = ">";
            this.nextButton.UseVisualStyleBackColor = true;
            this.nextButton.Click += new System.EventHandler(this.nextButton_Click);
            // 
            // AvatarMovementControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.nextButton);
            this.Controls.Add(this.prevButton);
            this.Controls.Add(this.restartButton);
            this.Controls.Add(this.reloadTargetsButton);
            this.Controls.Add(this.drawMapButton);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.pauseButton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.turnRatePanel);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.heightOffsetPanel);
            this.Controls.Add(this.distanceThresholdPanel);
            this.Controls.Add(this.moveRatePanel);
            this.Name = "AvatarMovementControl";
            this.Size = new System.Drawing.Size(514, 190);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Label statusLabel;
        private Chimera.GUI.ScalarPanel turnRatePanel;
        private Chimera.GUI.ScalarPanel moveRatePanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private Chimera.GUI.ScalarPanel distanceThresholdPanel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button pauseButton;
        private Chimera.GUI.ScalarPanel heightOffsetPanel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button drawMapButton;
        private System.Windows.Forms.OpenFileDialog mapFileDialog;
        private System.Windows.Forms.Button reloadTargetsButton;
        private System.Windows.Forms.Button restartButton;
        private System.Windows.Forms.Button prevButton;
        private System.Windows.Forms.Button nextButton;
    }
}

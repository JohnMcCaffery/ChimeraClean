namespace FlythroughLib.Panels {
    partial class FlythroughPanel {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FlythroughPanel));
            this.playButton = new System.Windows.Forms.Button();
            this.lengthValue = new System.Windows.Forms.NumericUpDown();
            this.Length = new System.Windows.Forms.Label();
            this.moveToTakeCurrentButton = new System.Windows.Forms.Button();
            this.moveToTargetVectorPanel = new ProxyTestGUI.VectorPanel();
            this.pitchValue = new System.Windows.Forms.NumericUpDown();
            this.yawValue = new System.Windows.Forms.NumericUpDown();
            this.pitchLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lockLookAtVectorPanel = new ProxyTestGUI.VectorPanel();
            this.rotateToTakeCurrentButton = new System.Windows.Forms.Button();
            this.lookAtTakeCurrentButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.lengthValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pitchValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.yawValue)).BeginInit();
            this.SuspendLayout();
            // 
            // playButton
            // 
            this.playButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.playButton.Location = new System.Drawing.Point(134, 93);
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(371, 23);
            this.playButton.TabIndex = 8;
            this.playButton.Text = "Play";
            this.playButton.UseVisualStyleBackColor = true;
            this.playButton.Click += new System.EventHandler(this.playButton_Click);
            // 
            // lengthValue
            // 
            this.lengthValue.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.lengthValue.Location = new System.Drawing.Point(68, 96);
            this.lengthValue.Maximum = new decimal(new int[] {
            60000,
            0,
            0,
            0});
            this.lengthValue.Name = "lengthValue";
            this.lengthValue.Size = new System.Drawing.Size(60, 20);
            this.lengthValue.TabIndex = 7;
            this.lengthValue.Value = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            // 
            // Length
            // 
            this.Length.AutoSize = true;
            this.Length.Location = new System.Drawing.Point(3, 98);
            this.Length.Name = "Length";
            this.Length.Size = new System.Drawing.Size(59, 13);
            this.Length.TabIndex = 6;
            this.Length.Text = "Length(ms)";
            // 
            // moveToTakeCurrentButton
            // 
            this.moveToTakeCurrentButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.moveToTakeCurrentButton.Location = new System.Drawing.Point(398, 3);
            this.moveToTakeCurrentButton.Name = "moveToTakeCurrentButton";
            this.moveToTakeCurrentButton.Size = new System.Drawing.Size(107, 23);
            this.moveToTakeCurrentButton.TabIndex = 9;
            this.moveToTakeCurrentButton.Text = "Take Current";
            this.moveToTakeCurrentButton.UseVisualStyleBackColor = true;
            this.moveToTakeCurrentButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // targetVectorPanel
            // 
            this.moveToTargetVectorPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.moveToTargetVectorPanel.DisplayName = "Move To Target";
            this.moveToTargetVectorPanel.Location = new System.Drawing.Point(0, 3);
            this.moveToTargetVectorPanel.Max = 1024D;
            this.moveToTargetVectorPanel.Min = -1024D;
            this.moveToTargetVectorPanel.Name = "targetVectorPanel";
            this.moveToTargetVectorPanel.Size = new System.Drawing.Size(508, 98);
            this.moveToTargetVectorPanel.TabIndex = 5;
            this.moveToTargetVectorPanel.Value = ((OpenMetaverse.Vector3)(resources.GetObject("targetVectorPanel.Value")));
            this.moveToTargetVectorPanel.X = 128F;
            this.moveToTargetVectorPanel.Y = 128F;
            this.moveToTargetVectorPanel.Z = 60F;
            // 
            // pitchValue
            // 
            this.pitchValue.Location = new System.Drawing.Point(40, 129);
            this.pitchValue.Name = "pitchValue";
            this.pitchValue.Size = new System.Drawing.Size(65, 20);
            this.pitchValue.TabIndex = 10;
            // 
            // yawValue
            // 
            this.yawValue.Location = new System.Drawing.Point(145, 129);
            this.yawValue.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.yawValue.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
            this.yawValue.Name = "yawValue";
            this.yawValue.Size = new System.Drawing.Size(65, 20);
            this.yawValue.TabIndex = 11;
            // 
            // pitchLabel
            // 
            this.pitchLabel.AutoSize = true;
            this.pitchLabel.Location = new System.Drawing.Point(3, 131);
            this.pitchLabel.Name = "pitchLabel";
            this.pitchLabel.Size = new System.Drawing.Size(31, 13);
            this.pitchLabel.TabIndex = 12;
            this.pitchLabel.Text = "Pitch";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(111, 131);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Yaw";
            // 
            // lockLookAtVectorPanel
            // 
            this.lockLookAtVectorPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lockLookAtVectorPanel.DisplayName = "Lock Look At Target";
            this.lockLookAtVectorPanel.Location = new System.Drawing.Point(-3, 155);
            this.lockLookAtVectorPanel.Max = 1024D;
            this.lockLookAtVectorPanel.Min = -1024D;
            this.lockLookAtVectorPanel.Name = "lockLookAtVectorPanel";
            this.lockLookAtVectorPanel.Size = new System.Drawing.Size(508, 98);
            this.lockLookAtVectorPanel.TabIndex = 14;
            this.lockLookAtVectorPanel.Value = ((OpenMetaverse.Vector3)(resources.GetObject("lockLookAtVectorPanel.Value")));
            this.lockLookAtVectorPanel.X = 128F;
            this.lockLookAtVectorPanel.Y = 128F;
            this.lockLookAtVectorPanel.Z = 60F;
            // 
            // rotateToTakeCurrentButton
            // 
            this.rotateToTakeCurrentButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rotateToTakeCurrentButton.Location = new System.Drawing.Point(398, 126);
            this.rotateToTakeCurrentButton.Name = "rotateToTakeCurrentButton";
            this.rotateToTakeCurrentButton.Size = new System.Drawing.Size(107, 23);
            this.rotateToTakeCurrentButton.TabIndex = 15;
            this.rotateToTakeCurrentButton.Text = "Take Current";
            this.rotateToTakeCurrentButton.UseVisualStyleBackColor = true;
            this.rotateToTakeCurrentButton.Click += new System.EventHandler(this.rotateToTakeCurrentButton_Click);
            // 
            // lookAtTakeCurrentButton
            // 
            this.lookAtTakeCurrentButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lookAtTakeCurrentButton.Location = new System.Drawing.Point(398, 168);
            this.lookAtTakeCurrentButton.Name = "lookAtTakeCurrentButton";
            this.lookAtTakeCurrentButton.Size = new System.Drawing.Size(107, 23);
            this.lookAtTakeCurrentButton.TabIndex = 16;
            this.lookAtTakeCurrentButton.Text = "Take Current";
            this.lookAtTakeCurrentButton.UseVisualStyleBackColor = true;
            this.lookAtTakeCurrentButton.Click += new System.EventHandler(this.lookAtTakeCurrentButton_Click);
            // 
            // FlythroughPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lookAtTakeCurrentButton);
            this.Controls.Add(this.rotateToTakeCurrentButton);
            this.Controls.Add(this.lockLookAtVectorPanel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pitchLabel);
            this.Controls.Add(this.yawValue);
            this.Controls.Add(this.pitchValue);
            this.Controls.Add(this.moveToTakeCurrentButton);
            this.Controls.Add(this.playButton);
            this.Controls.Add(this.lengthValue);
            this.Controls.Add(this.Length);
            this.Controls.Add(this.moveToTargetVectorPanel);
            this.Name = "FlythroughPanel";
            this.Size = new System.Drawing.Size(508, 348);
            ((System.ComponentModel.ISupportInitialize)(this.lengthValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pitchValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.yawValue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button playButton;
        private System.Windows.Forms.NumericUpDown lengthValue;
        private System.Windows.Forms.Label Length;
        private ProxyTestGUI.VectorPanel moveToTargetVectorPanel;
        private System.Windows.Forms.Button moveToTakeCurrentButton;
        private System.Windows.Forms.NumericUpDown pitchValue;
        private System.Windows.Forms.NumericUpDown yawValue;
        private System.Windows.Forms.Label pitchLabel;
        private System.Windows.Forms.Label label2;
        private ProxyTestGUI.VectorPanel lockLookAtVectorPanel;
        private System.Windows.Forms.Button rotateToTakeCurrentButton;
        private System.Windows.Forms.Button lookAtTakeCurrentButton;
    }
}

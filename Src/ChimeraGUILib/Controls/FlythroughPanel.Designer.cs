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
            this.button1 = new System.Windows.Forms.Button();
            this.targetVectorPanel = new ProxyTestGUI.VectorPanel();
            this.pitchValue = new System.Windows.Forms.NumericUpDown();
            this.yawValue = new System.Windows.Forms.NumericUpDown();
            this.pitchLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
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
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(398, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(107, 23);
            this.button1.TabIndex = 9;
            this.button1.Text = "Take Current";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // targetVectorPanel
            // 
            this.targetVectorPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.targetVectorPanel.DisplayName = "Name";
            this.targetVectorPanel.Location = new System.Drawing.Point(0, 3);
            this.targetVectorPanel.Max = 1024D;
            this.targetVectorPanel.Min = -1024D;
            this.targetVectorPanel.Name = "targetVectorPanel";
            this.targetVectorPanel.Size = new System.Drawing.Size(508, 98);
            this.targetVectorPanel.TabIndex = 5;
            this.targetVectorPanel.Value = ((OpenMetaverse.Vector3)(resources.GetObject("targetVectorPanel.Value")));
            this.targetVectorPanel.X = 128F;
            this.targetVectorPanel.Y = 128F;
            this.targetVectorPanel.Z = 60F;
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
            // FlythroughPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pitchLabel);
            this.Controls.Add(this.yawValue);
            this.Controls.Add(this.pitchValue);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.playButton);
            this.Controls.Add(this.lengthValue);
            this.Controls.Add(this.Length);
            this.Controls.Add(this.targetVectorPanel);
            this.Name = "FlythroughPanel";
            this.Size = new System.Drawing.Size(508, 160);
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
        private ProxyTestGUI.VectorPanel targetVectorPanel;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.NumericUpDown pitchValue;
        private System.Windows.Forms.NumericUpDown yawValue;
        private System.Windows.Forms.Label pitchLabel;
        private System.Windows.Forms.Label label2;
    }
}

namespace ChimeraGUILib.Controls.FlythroughEventPanels {
    partial class RotateToPanel {
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
            this.rotateToTakeCurrentButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.pitchLabel = new System.Windows.Forms.Label();
            this.yawValue = new System.Windows.Forms.NumericUpDown();
            this.pitchValue = new System.Windows.Forms.NumericUpDown();
            this.lengthValue = new System.Windows.Forms.NumericUpDown();
            this.Length = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.yawValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pitchValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lengthValue)).BeginInit();
            this.SuspendLayout();
            // 
            // rotateToTakeCurrentButton
            // 
            this.rotateToTakeCurrentButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rotateToTakeCurrentButton.Location = new System.Drawing.Point(215, 0);
            this.rotateToTakeCurrentButton.Name = "rotateToTakeCurrentButton";
            this.rotateToTakeCurrentButton.Size = new System.Drawing.Size(107, 23);
            this.rotateToTakeCurrentButton.TabIndex = 20;
            this.rotateToTakeCurrentButton.Text = "Take Current";
            this.rotateToTakeCurrentButton.UseVisualStyleBackColor = true;
            this.rotateToTakeCurrentButton.Click += new System.EventHandler(this.rotateToTakeCurrentButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(111, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Yaw";
            // 
            // pitchLabel
            // 
            this.pitchLabel.AutoSize = true;
            this.pitchLabel.Location = new System.Drawing.Point(3, 5);
            this.pitchLabel.Name = "pitchLabel";
            this.pitchLabel.Size = new System.Drawing.Size(31, 13);
            this.pitchLabel.TabIndex = 18;
            this.pitchLabel.Text = "Pitch";
            // 
            // yawValue
            // 
            this.yawValue.Location = new System.Drawing.Point(145, 3);
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
            this.yawValue.TabIndex = 17;
            // 
            // pitchValue
            // 
            this.pitchValue.Location = new System.Drawing.Point(40, 3);
            this.pitchValue.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.pitchValue.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
            this.pitchValue.Name = "pitchValue";
            this.pitchValue.Size = new System.Drawing.Size(65, 20);
            this.pitchValue.TabIndex = 16;
            // 
            // lengthValue
            // 
            this.lengthValue.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.lengthValue.Location = new System.Drawing.Point(79, 29);
            this.lengthValue.Maximum = new decimal(new int[] {
            60000,
            0,
            0,
            0});
            this.lengthValue.Name = "lengthValue";
            this.lengthValue.Size = new System.Drawing.Size(60, 20);
            this.lengthValue.TabIndex = 22;
            this.lengthValue.Value = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            // 
            // Length
            // 
            this.Length.AutoSize = true;
            this.Length.Location = new System.Drawing.Point(14, 31);
            this.Length.Name = "Length";
            this.Length.Size = new System.Drawing.Size(59, 13);
            this.Length.TabIndex = 21;
            this.Length.Text = "Length(ms)";
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(3, 55);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(319, 23);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar.TabIndex = 23;
            // 
            // RotateToPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.lengthValue);
            this.Controls.Add(this.Length);
            this.Controls.Add(this.rotateToTakeCurrentButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pitchLabel);
            this.Controls.Add(this.yawValue);
            this.Controls.Add(this.pitchValue);
            this.MinimumSize = new System.Drawing.Size(325, 176);
            this.Name = "RotateToPanel";
            this.Size = new System.Drawing.Size(325, 176);
            ((System.ComponentModel.ISupportInitialize)(this.yawValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pitchValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lengthValue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button rotateToTakeCurrentButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label pitchLabel;
        private System.Windows.Forms.NumericUpDown yawValue;
        private System.Windows.Forms.NumericUpDown pitchValue;
        private System.Windows.Forms.NumericUpDown lengthValue;
        private System.Windows.Forms.Label Length;
        private System.Windows.Forms.ProgressBar progressBar;
    }
}

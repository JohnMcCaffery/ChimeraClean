namespace Chimera.Flythrough.GUI {
    partial class LookAtPanel {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LookAtPanel));
            this.moveToTakeCurrentButton = new System.Windows.Forms.Button();
            this.lengthValue = new System.Windows.Forms.NumericUpDown();
            this.Length = new System.Windows.Forms.Label();
            this.targetVectorPanel = new Chimera.GUI.VectorPanel();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.goToTargetButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.lengthValue)).BeginInit();
            this.SuspendLayout();
            // 
            // moveToTakeCurrentButton
            // 
            this.moveToTakeCurrentButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.moveToTakeCurrentButton.Location = new System.Drawing.Point(219, 99);
            this.moveToTakeCurrentButton.Name = "moveToTakeCurrentButton";
            this.moveToTakeCurrentButton.Size = new System.Drawing.Size(106, 23);
            this.moveToTakeCurrentButton.TabIndex = 11;
            this.moveToTakeCurrentButton.Text = "Take Current";
            this.moveToTakeCurrentButton.UseVisualStyleBackColor = true;
            this.moveToTakeCurrentButton.Click += new System.EventHandler(this.moveToTakeCurrentButton_Click);
            // 
            // lengthValue
            // 
            this.lengthValue.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.lengthValue.Location = new System.Drawing.Point(68, 99);
            this.lengthValue.Maximum = new decimal(new int[] {
            60000,
            0,
            0,
            0});
            this.lengthValue.Name = "lengthValue";
            this.lengthValue.Size = new System.Drawing.Size(60, 20);
            this.lengthValue.TabIndex = 13;
            this.lengthValue.Value = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            // 
            // Length
            // 
            this.Length.AutoSize = true;
            this.Length.Location = new System.Drawing.Point(3, 101);
            this.Length.Name = "Length";
            this.Length.Size = new System.Drawing.Size(59, 13);
            this.Length.TabIndex = 12;
            this.Length.Text = "Length(ms)";
            // 
            // targetVectorPanel
            // 
            this.targetVectorPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.targetVectorPanel.Text = "Move To Target";
            this.targetVectorPanel.Location = new System.Drawing.Point(0, 0);
            this.targetVectorPanel.Max = 20489f;
            this.targetVectorPanel.MaxV = ((OpenMetaverse.Vector3)(resources.GetObject("targetVectorPanel.MaxV")));
            this.targetVectorPanel.Min = -2048f;
            this.targetVectorPanel.MinimumSize = new System.Drawing.Size(103, 95);
            this.targetVectorPanel.MinV = ((OpenMetaverse.Vector3)(resources.GetObject("targetVectorPanel.MinV")));
            this.targetVectorPanel.Name = "targetVectorPanel";
            this.targetVectorPanel.Size = new System.Drawing.Size(325, 98);
            this.targetVectorPanel.TabIndex = 10;
            this.targetVectorPanel.Value = ((OpenMetaverse.Vector3)(resources.GetObject("targetVectorPanel.Value")));
            this.targetVectorPanel.X = 128F;
            this.targetVectorPanel.Y = 128F;
            this.targetVectorPanel.Z = 60F;
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(3, 125);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(322, 23);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar.TabIndex = 14;
            // 
            // goToTargetButton
            // 
            this.goToTargetButton.Location = new System.Drawing.Point(134, 99);
            this.goToTargetButton.Name = "goToTargetButton";
            this.goToTargetButton.Size = new System.Drawing.Size(79, 23);
            this.goToTargetButton.TabIndex = 15;
            this.goToTargetButton.Text = "Go To Target";
            this.goToTargetButton.UseVisualStyleBackColor = true;
            this.goToTargetButton.Click += new System.EventHandler(this.goToTargetButton_Click);
            // 
            // LookAtPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.goToTargetButton);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.lengthValue);
            this.Controls.Add(this.Length);
            this.Controls.Add(this.moveToTakeCurrentButton);
            this.Controls.Add(this.targetVectorPanel);
            this.MinimumSize = new System.Drawing.Size(214, 149);
            this.Name = "LookAtPanel";
            this.Size = new System.Drawing.Size(325, 149);
            ((System.ComponentModel.ISupportInitialize)(this.lengthValue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button moveToTakeCurrentButton;
        private Chimera.GUI.VectorPanel targetVectorPanel;
        private System.Windows.Forms.NumericUpDown lengthValue;
        private System.Windows.Forms.Label Length;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button goToTargetButton;
    }
}

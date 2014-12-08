namespace Chimera.GUI.Controls.Plugins {
    partial class PanoramaPanel {
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
            this.captureButton = new System.Windows.Forms.Button();
            this.folderAddress = new System.Windows.Forms.TextBox();
            this.folderButton = new System.Windows.Forms.Button();
            this.folderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.captureDelay = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.captureDelay)).BeginInit();
            this.SuspendLayout();
            // 
            // captureButton
            // 
            this.captureButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.captureButton.Location = new System.Drawing.Point(3, 3);
            this.captureButton.Name = "captureButton";
            this.captureButton.Size = new System.Drawing.Size(549, 23);
            this.captureButton.TabIndex = 0;
            this.captureButton.Text = "Capture";
            this.captureButton.UseVisualStyleBackColor = true;
            this.captureButton.Click += new System.EventHandler(this.captureButton_Click);
            // 
            // folderAddress
            // 
            this.folderAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.folderAddress.Location = new System.Drawing.Point(108, 33);
            this.folderAddress.Name = "folderAddress";
            this.folderAddress.Size = new System.Drawing.Size(394, 20);
            this.folderAddress.TabIndex = 1;
            this.folderAddress.TextChanged += new System.EventHandler(this.folderAddress_TextChanged);
            // 
            // folderButton
            // 
            this.folderButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.folderButton.Location = new System.Drawing.Point(508, 33);
            this.folderButton.Name = "folderButton";
            this.folderButton.Size = new System.Drawing.Size(44, 23);
            this.folderButton.TabIndex = 2;
            this.folderButton.Text = "...";
            this.folderButton.UseVisualStyleBackColor = true;
            this.folderButton.Click += new System.EventHandler(this.folderButton_Click);
            // 
            // captureDelay
            // 
            this.captureDelay.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.captureDelay.Location = new System.Drawing.Point(108, 59);
            this.captureDelay.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.captureDelay.Name = "captureDelay";
            this.captureDelay.Size = new System.Drawing.Size(73, 20);
            this.captureDelay.TabIndex = 3;
            this.captureDelay.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.captureDelay.ValueChanged += new System.EventHandler(this.captureDelay_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Capture Delay (MS)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Save Path";
            // 
            // PanoramaPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.captureDelay);
            this.Controls.Add(this.folderButton);
            this.Controls.Add(this.folderAddress);
            this.Controls.Add(this.captureButton);
            this.Name = "PanoramaPanel";
            this.Size = new System.Drawing.Size(555, 290);
            ((System.ComponentModel.ISupportInitialize)(this.captureDelay)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button captureButton;
        private System.Windows.Forms.TextBox folderAddress;
        private System.Windows.Forms.Button folderButton;
        private System.Windows.Forms.FolderBrowserDialog folderDialog;
        private System.Windows.Forms.NumericUpDown captureDelay;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

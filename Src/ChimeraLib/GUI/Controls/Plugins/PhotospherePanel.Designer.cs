namespace Chimera.GUI.Controls.Plugins {
    partial class PhotospherePanel {
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
            this.screenshotButton = new System.Windows.Forms.Button();
            this.nextImageButton = new System.Windows.Forms.Button();
            this.setCentreButton = new System.Windows.Forms.Button();
            this.nameBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.widthBox = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.heightLabel = new System.Windows.Forms.Label();
            this.detailsLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.captureDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.widthBox)).BeginInit();
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
            // screenshotButton
            // 
            this.screenshotButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.screenshotButton.Location = new System.Drawing.Point(3, 155);
            this.screenshotButton.Name = "screenshotButton";
            this.screenshotButton.Size = new System.Drawing.Size(546, 23);
            this.screenshotButton.TabIndex = 6;
            this.screenshotButton.Text = "Take Screenshot";
            this.screenshotButton.UseVisualStyleBackColor = true;
            this.screenshotButton.Click += new System.EventHandler(this.screenshotButton_Click);
            // 
            // nextImageButton
            // 
            this.nextImageButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nextImageButton.Location = new System.Drawing.Point(6, 126);
            this.nextImageButton.Name = "nextImageButton";
            this.nextImageButton.Size = new System.Drawing.Size(546, 23);
            this.nextImageButton.TabIndex = 7;
            this.nextImageButton.Text = "Next Position";
            this.nextImageButton.UseVisualStyleBackColor = true;
            this.nextImageButton.Click += new System.EventHandler(this.nextImageButton_Click);
            // 
            // setCentreButton
            // 
            this.setCentreButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.setCentreButton.Location = new System.Drawing.Point(6, 97);
            this.setCentreButton.Name = "setCentreButton";
            this.setCentreButton.Size = new System.Drawing.Size(546, 23);
            this.setCentreButton.TabIndex = 8;
            this.setCentreButton.Text = "Set Centre";
            this.setCentreButton.UseVisualStyleBackColor = true;
            this.setCentreButton.Click += new System.EventHandler(this.setCentreButton_Click);
            // 
            // nameBox
            // 
            this.nameBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nameBox.Location = new System.Drawing.Point(229, 58);
            this.nameBox.Name = "nameBox";
            this.nameBox.Size = new System.Drawing.Size(122, 20);
            this.nameBox.TabIndex = 9;
            this.nameBox.Text = "Photosphere";
            this.nameBox.TextChanged += new System.EventHandler(this.NameBox_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(187, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Name";
            // 
            // widthBox
            // 
            this.widthBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.widthBox.Increment = new decimal(new int[] {
            512,
            0,
            0,
            0});
            this.widthBox.Location = new System.Drawing.Point(433, 58);
            this.widthBox.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.widthBox.Name = "widthBox";
            this.widthBox.Size = new System.Drawing.Size(69, 20);
            this.widthBox.TabIndex = 11;
            this.widthBox.Value = new decimal(new int[] {
            8192,
            0,
            0,
            0});
            this.widthBox.ValueChanged += new System.EventHandler(this.widthBox_ValueChanged);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(357, 61);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Output Width";
            // 
            // heightLabel
            // 
            this.heightLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.heightLabel.AutoSize = true;
            this.heightLabel.Location = new System.Drawing.Point(510, 61);
            this.heightLabel.Name = "heightLabel";
            this.heightLabel.Size = new System.Drawing.Size(39, 13);
            this.heightLabel.TabIndex = 13;
            this.heightLabel.Text = "x 4096";
            // 
            // detailsLabel
            // 
            this.detailsLabel.AutoSize = true;
            this.detailsLabel.Location = new System.Drawing.Point(6, 185);
            this.detailsLabel.Name = "detailsLabel";
            this.detailsLabel.Size = new System.Drawing.Size(35, 13);
            this.detailsLabel.TabIndex = 14;
            this.detailsLabel.Text = "label5";
            // 
            // PhotospherePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.detailsLabel);
            this.Controls.Add(this.heightLabel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.widthBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.nameBox);
            this.Controls.Add(this.setCentreButton);
            this.Controls.Add(this.nextImageButton);
            this.Controls.Add(this.screenshotButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.captureDelay);
            this.Controls.Add(this.folderButton);
            this.Controls.Add(this.folderAddress);
            this.Controls.Add(this.captureButton);
            this.Name = "PhotospherePanel";
            this.Size = new System.Drawing.Size(555, 290);
            ((System.ComponentModel.ISupportInitialize)(this.captureDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.widthBox)).EndInit();
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
        private System.Windows.Forms.Button screenshotButton;
        private System.Windows.Forms.Button nextImageButton;
        private System.Windows.Forms.Button setCentreButton;
        private System.Windows.Forms.TextBox nameBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown widthBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label heightLabel;
        private System.Windows.Forms.Label detailsLabel;
    }
}

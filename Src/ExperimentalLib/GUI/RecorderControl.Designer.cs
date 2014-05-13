namespace Chimera.Experimental.GUI {
    partial class RecorderControl {
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
            this.components = new System.ComponentModel.Container();
            this.updateTimer = new System.Windows.Forms.Timer(this.components);
            this.updateFreq = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.statsList = new System.Windows.Forms.ListView();
            this.pingButton = new System.Windows.Forms.Button();
            this.timestampButton = new System.Windows.Forms.Button();
            this.clipboardLabel = new System.Windows.Forms.Label();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.loadFileButton = new System.Windows.Forms.Button();
            this.saveCSVButton = new System.Windows.Forms.Button();
            this.loadCSVButton = new System.Windows.Forms.Button();
            this.recordFPS = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.updateFreq)).BeginInit();
            this.SuspendLayout();
            // 
            // updateTimer
            // 
            this.updateTimer.Tick += new System.EventHandler(this.updateTimer_Tick);
            // 
            // updateFreq
            // 
            this.updateFreq.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.updateFreq.Location = new System.Drawing.Point(532, 204);
            this.updateFreq.Name = "updateFreq";
            this.updateFreq.Size = new System.Drawing.Size(33, 20);
            this.updateFreq.TabIndex = 38;
            this.updateFreq.ValueChanged += new System.EventHandler(this.updateFreq_ValueChanged);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(434, 207);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 13);
            this.label3.TabIndex = 39;
            this.label3.Text = "UpdateFrequency";
            // 
            // statsList
            // 
            this.statsList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.statsList.Location = new System.Drawing.Point(7, 230);
            this.statsList.Name = "statsList";
            this.statsList.Size = new System.Drawing.Size(558, 168);
            this.statsList.TabIndex = 40;
            this.statsList.UseCompatibleStateImageBehavior = false;
            this.statsList.View = System.Windows.Forms.View.Details;
            // 
            // pingButton
            // 
            this.pingButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pingButton.Location = new System.Drawing.Point(244, 201);
            this.pingButton.Name = "pingButton";
            this.pingButton.Size = new System.Drawing.Size(89, 23);
            this.pingButton.TabIndex = 41;
            this.pingButton.Text = "Load PingTime";
            this.pingButton.UseVisualStyleBackColor = true;
            this.pingButton.Click += new System.EventHandler(this.pingButton_Click);
            // 
            // timestampButton
            // 
            this.timestampButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.timestampButton.Location = new System.Drawing.Point(471, 175);
            this.timestampButton.Name = "timestampButton";
            this.timestampButton.Size = new System.Drawing.Size(94, 23);
            this.timestampButton.TabIndex = 42;
            this.timestampButton.Text = "Save Timestamp";
            this.timestampButton.UseVisualStyleBackColor = true;
            this.timestampButton.Click += new System.EventHandler(this.timestampButton_Click);
            // 
            // clipboardLabel
            // 
            this.clipboardLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.clipboardLabel.Location = new System.Drawing.Point(36, 174);
            this.clipboardLabel.Name = "clipboardLabel";
            this.clipboardLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.clipboardLabel.Size = new System.Drawing.Size(429, 23);
            this.clipboardLabel.TabIndex = 43;
            this.clipboardLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // openFileDialog
            // 
            this.openFileDialog.InitialDirectory = "U:\\Results";
            this.openFileDialog.RestoreDirectory = true;
            // 
            // loadFileButton
            // 
            this.loadFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.loadFileButton.Location = new System.Drawing.Point(174, 201);
            this.loadFileButton.Name = "loadFileButton";
            this.loadFileButton.Size = new System.Drawing.Size(63, 23);
            this.loadFileButton.TabIndex = 44;
            this.loadFileButton.Text = "Load FPS";
            this.loadFileButton.UseVisualStyleBackColor = true;
            this.loadFileButton.Click += new System.EventHandler(this.loadFPSFileButton_Click);
            // 
            // saveCSVButton
            // 
            this.saveCSVButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.saveCSVButton.Location = new System.Drawing.Point(339, 201);
            this.saveCSVButton.Name = "saveCSVButton";
            this.saveCSVButton.Size = new System.Drawing.Size(89, 23);
            this.saveCSVButton.TabIndex = 45;
            this.saveCSVButton.Text = "Save CSV";
            this.saveCSVButton.UseVisualStyleBackColor = true;
            this.saveCSVButton.Click += new System.EventHandler(this.saveCSVButton_Click);
            // 
            // loadCSVButton
            // 
            this.loadCSVButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.loadCSVButton.Location = new System.Drawing.Point(79, 201);
            this.loadCSVButton.Name = "loadCSVButton";
            this.loadCSVButton.Size = new System.Drawing.Size(89, 23);
            this.loadCSVButton.TabIndex = 46;
            this.loadCSVButton.Text = "Load CSV";
            this.loadCSVButton.UseVisualStyleBackColor = true;
            this.loadCSVButton.Click += new System.EventHandler(this.loadCSVButton_Click);
            // 
            // recordFPS
            // 
            this.recordFPS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.recordFPS.Location = new System.Drawing.Point(-16, 202);
            this.recordFPS.Name = "recordFPS";
            this.recordFPS.Size = new System.Drawing.Size(89, 23);
            this.recordFPS.TabIndex = 47;
            this.recordFPS.Text = "Record FPS";
            this.recordFPS.UseVisualStyleBackColor = true;
            this.recordFPS.Click += new System.EventHandler(this.recordFPS_Click);
            // 
            // RecorderControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.recordFPS);
            this.Controls.Add(this.loadCSVButton);
            this.Controls.Add(this.saveCSVButton);
            this.Controls.Add(this.loadFileButton);
            this.Controls.Add(this.clipboardLabel);
            this.Controls.Add(this.timestampButton);
            this.Controls.Add(this.pingButton);
            this.Controls.Add(this.statsList);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.updateFreq);
            this.Name = "RecorderControl";
            this.Controls.SetChildIndex(this.updateFreq, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.statsList, 0);
            this.Controls.SetChildIndex(this.pingButton, 0);
            this.Controls.SetChildIndex(this.timestampButton, 0);
            this.Controls.SetChildIndex(this.clipboardLabel, 0);
            this.Controls.SetChildIndex(this.loadFileButton, 0);
            this.Controls.SetChildIndex(this.saveCSVButton, 0);
            this.Controls.SetChildIndex(this.loadCSVButton, 0);
            this.Controls.SetChildIndex(this.recordFPS, 0);
            ((System.ComponentModel.ISupportInitialize)(this.updateFreq)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer updateTimer;
        private System.Windows.Forms.NumericUpDown updateFreq;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListView statsList;
        private System.Windows.Forms.Button pingButton;
        private System.Windows.Forms.Button timestampButton;
        private System.Windows.Forms.Label clipboardLabel;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button loadFileButton;
        private System.Windows.Forms.Button saveCSVButton;
        private System.Windows.Forms.Button loadCSVButton;
        private System.Windows.Forms.Button recordFPS;
    }
}

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
            this.openLogFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.loadFileButton = new System.Windows.Forms.Button();
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
            this.updateFreq.Location = new System.Drawing.Point(532, 173);
            this.updateFreq.Name = "updateFreq";
            this.updateFreq.Size = new System.Drawing.Size(33, 20);
            this.updateFreq.TabIndex = 38;
            this.updateFreq.ValueChanged += new System.EventHandler(this.updateFreq_ValueChanged);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(434, 176);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 13);
            this.label3.TabIndex = 39;
            this.label3.Text = "UpdateFrequency";
            // 
            // statsList
            // 
            this.statsList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.statsList.Location = new System.Drawing.Point(7, 199);
            this.statsList.Name = "statsList";
            this.statsList.Size = new System.Drawing.Size(558, 199);
            this.statsList.TabIndex = 40;
            this.statsList.UseCompatibleStateImageBehavior = false;
            this.statsList.View = System.Windows.Forms.View.Details;
            // 
            // pingButton
            // 
            this.pingButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pingButton.Location = new System.Drawing.Point(339, 170);
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
            this.timestampButton.Location = new System.Drawing.Point(174, 171);
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
            this.clipboardLabel.Location = new System.Drawing.Point(-261, 170);
            this.clipboardLabel.Name = "clipboardLabel";
            this.clipboardLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.clipboardLabel.Size = new System.Drawing.Size(429, 23);
            this.clipboardLabel.TabIndex = 43;
            this.clipboardLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // openLogFileDialog
            // 
            this.openLogFileDialog.InitialDirectory = "U:\\Results";
            this.openLogFileDialog.RestoreDirectory = true;
            // 
            // loadFileButton
            // 
            this.loadFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.loadFileButton.Location = new System.Drawing.Point(274, 170);
            this.loadFileButton.Name = "loadFileButton";
            this.loadFileButton.Size = new System.Drawing.Size(59, 23);
            this.loadFileButton.TabIndex = 44;
            this.loadFileButton.Text = "Load File";
            this.loadFileButton.UseVisualStyleBackColor = true;
            this.loadFileButton.Click += new System.EventHandler(this.loadFileButton_Click);
            // 
            // RecorderControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
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
        private System.Windows.Forms.OpenFileDialog openLogFileDialog;
        private System.Windows.Forms.Button loadFileButton;
    }
}

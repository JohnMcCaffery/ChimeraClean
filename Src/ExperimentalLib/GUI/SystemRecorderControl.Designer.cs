namespace Chimera.Experimental.GUI {
    partial class SystemRecorderControl {
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
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveCSVButton = new System.Windows.Forms.Button();
            this.loadCSVButton = new System.Windows.Forms.Button();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
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
            this.updateFreq.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.updateFreq.Location = new System.Drawing.Point(709, 8);
            this.updateFreq.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.updateFreq.Name = "updateFreq";
            this.updateFreq.Size = new System.Drawing.Size(55, 20);
            this.updateFreq.TabIndex = 38;
            this.updateFreq.ValueChanged += new System.EventHandler(this.updateFreq_ValueChanged);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(611, 10);
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
            this.statsList.Location = new System.Drawing.Point(0, 34);
            this.statsList.Name = "statsList";
            this.statsList.Size = new System.Drawing.Size(886, 433);
            this.statsList.TabIndex = 40;
            this.statsList.UseCompatibleStateImageBehavior = false;
            this.statsList.View = System.Windows.Forms.View.Details;
            // 
            // openFileDialog
            // 
            this.openFileDialog.InitialDirectory = "U:\\Results";
            this.openFileDialog.RestoreDirectory = true;
            // 
            // saveCSVButton
            // 
            this.saveCSVButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.saveCSVButton.Location = new System.Drawing.Point(73, 5);
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
            this.loadCSVButton.Location = new System.Drawing.Point(3, 5);
            this.loadCSVButton.Name = "loadCSVButton";
            this.loadCSVButton.Size = new System.Drawing.Size(64, 23);
            this.loadCSVButton.TabIndex = 46;
            this.loadCSVButton.Text = "Load CSV";
            this.loadCSVButton.UseVisualStyleBackColor = true;
            this.loadCSVButton.Click += new System.EventHandler(this.loadCSVButton_Click);
            // 
            // SystemRecorderControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.loadCSVButton);
            this.Controls.Add(this.saveCSVButton);
            this.Controls.Add(this.statsList);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.updateFreq);
            this.Name = "SystemRecorderControl";
            this.Size = new System.Drawing.Size(889, 470);
            ((System.ComponentModel.ISupportInitialize)(this.updateFreq)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer updateTimer;
        private System.Windows.Forms.NumericUpDown updateFreq;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListView statsList;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button saveCSVButton;
        private System.Windows.Forms.Button loadCSVButton;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
    }
}

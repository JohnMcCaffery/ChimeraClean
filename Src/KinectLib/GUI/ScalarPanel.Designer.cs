namespace KinectLib.GUI {
    partial class ScalarPanel {
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
            this.value = new System.Windows.Forms.NumericUpDown();
            this.valueSlider = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.value)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.valueSlider)).BeginInit();
            this.SuspendLayout();
            // 
            // value
            // 
            this.value.Location = new System.Drawing.Point(0, 0);
            this.value.Name = "value";
            this.value.Size = new System.Drawing.Size(62, 20);
            this.value.TabIndex = 0;
            this.value.ValueChanged += new System.EventHandler(this.value_ValueChanged);
            // 
            // valueSlider
            // 
            this.valueSlider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.valueSlider.Location = new System.Drawing.Point(68, 0);
            this.valueSlider.Name = "valueSlider";
            this.valueSlider.Size = new System.Drawing.Size(288, 42);
            this.valueSlider.TabIndex = 1;
            this.valueSlider.TickStyle = System.Windows.Forms.TickStyle.None;
            this.valueSlider.Scroll += new System.EventHandler(this.valueSlider_Scroll);
            // 
            // ScalarPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.valueSlider);
            this.Controls.Add(this.value);
            this.MinimumSize = new System.Drawing.Size(95, 20);
            this.Name = "ScalarPanel";
            this.Size = new System.Drawing.Size(356, 20);
            ((System.ComponentModel.ISupportInitialize)(this.value)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.valueSlider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown value;
        private System.Windows.Forms.TrackBar valueSlider;
    }
}

namespace Chimera.GUI {
    partial class UpdatedScalarPanel {
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
            this.value.Size = new System.Drawing.Size(63, 20);
            this.value.TabIndex = 0;
            // 
            // valueSlider
            // 
            this.valueSlider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.valueSlider.LargeChange = 100;
            this.valueSlider.Location = new System.Drawing.Point(60, 0);
            this.valueSlider.Maximum = 1000;
            this.valueSlider.Minimum = -1000;
            this.valueSlider.Name = "valueSlider";
            this.valueSlider.Size = new System.Drawing.Size(296, 45);
            this.valueSlider.SmallChange = 10;
            this.valueSlider.TabIndex = 1;
            this.valueSlider.TickFrequency = 100;
            this.valueSlider.TickStyle = System.Windows.Forms.TickStyle.None;
            // 
            // UpdatedScalarPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.value);
            this.Controls.Add(this.valueSlider);
            this.Name = "UpdatedScalarPanel";
            this.ValueChanged += new System.Action<float>(this.UpdatedScalarPanel_ValueChanged);
            this.Controls.SetChildIndex(this.valueSlider, 0);
            this.Controls.SetChildIndex(this.value, 0);
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

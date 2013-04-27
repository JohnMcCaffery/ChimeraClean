namespace Chimera.GUI.Controls.Inputs {
    partial class AxisBasedDeltaPanel {
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
            this.axesBox = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.scalePanel = new Chimera.GUI.ScalarPanel();
            this.SuspendLayout();
            // 
            // axesBox
            // 
            this.axesBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.axesBox.Location = new System.Drawing.Point(3, 29);
            this.axesBox.Name = "axesBox";
            this.axesBox.Size = new System.Drawing.Size(494, 410);
            this.axesBox.TabIndex = 0;
            this.axesBox.TabStop = false;
            this.axesBox.Text = "Axes";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Scale";
            // 
            // scalePanel
            // 
            this.scalePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scalePanel.Location = new System.Drawing.Point(44, 3);
            this.scalePanel.Max = 4F;
            this.scalePanel.Min = 0F;
            this.scalePanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.scalePanel.Name = "scalePanel";
            this.scalePanel.Size = new System.Drawing.Size(453, 20);
            this.scalePanel.TabIndex = 1;
            this.scalePanel.Value = 0F;
            this.scalePanel.ValueChanged += new System.Action<float>(this.scalePanel_ValueChanged);
            // 
            // AxisBasedDeltaPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.scalePanel);
            this.Controls.Add(this.axesBox);
            this.Name = "AxisBasedDeltaPanel";
            this.Size = new System.Drawing.Size(500, 442);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox axesBox;
        private ScalarPanel scalePanel;
        private System.Windows.Forms.Label label1;
    }
}

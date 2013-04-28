namespace Chimera.GUI.Controls.Plugins {
    partial class ConstrainedAxisPanel {
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
            this.deadzonePanel = new Chimera.GUI.ScalarPanel();
            this.scalePanel = new Chimera.GUI.ScalarPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // deadzonePanel
            // 
            this.deadzonePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.deadzonePanel.Location = new System.Drawing.Point(66, 5);
            this.deadzonePanel.Max = 100F;
            this.deadzonePanel.Min = 0F;
            this.deadzonePanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.deadzonePanel.Name = "deadzonePanel";
            this.deadzonePanel.Size = new System.Drawing.Size(95, 20);
            this.deadzonePanel.TabIndex = 0;
            this.deadzonePanel.Value = 0F;
            // 
            // scalePanel
            // 
            this.scalePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scalePanel.Location = new System.Drawing.Point(65, 31);
            this.scalePanel.Max = 10F;
            this.scalePanel.Min = 0F;
            this.scalePanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.scalePanel.Name = "scalePanel";
            this.scalePanel.Size = new System.Drawing.Size(95, 20);
            this.scalePanel.TabIndex = 3;
            this.scalePanel.Value = 0F;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Deadzone";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 31);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Scale";
            // 
            // ConstrainedAxisPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.scalePanel);
            this.Controls.Add(this.deadzonePanel);
            this.MinimumSize = new System.Drawing.Size(163, 57);
            this.Name = "ConstrainedAxisPanel";
            this.Size = new System.Drawing.Size(163, 57);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ScalarPanel deadzonePanel;
        private ScalarPanel scalePanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
    }
}

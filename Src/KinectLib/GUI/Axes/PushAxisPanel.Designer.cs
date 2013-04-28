namespace Chimera.Kinect.GUI.Axes {
    partial class PushAxisPanel {
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
            this.label1 = new System.Windows.Forms.Label();
            this.constrainedAxisPanel = new Chimera.GUI.Controls.Plugins.ConstrainedAxisPanel();
            this.pushPanel = new Chimera.GUI.UpdatedScalarPanel();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Push";
            // 
            // constrainedAxisPanel
            // 
            this.constrainedAxisPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.constrainedAxisPanel.Axis = null;
            this.constrainedAxisPanel.Location = new System.Drawing.Point(0, 3);
            this.constrainedAxisPanel.MinimumSize = new System.Drawing.Size(163, 57);
            this.constrainedAxisPanel.Name = "constrainedAxisPanel";
            this.constrainedAxisPanel.Size = new System.Drawing.Size(170, 57);
            this.constrainedAxisPanel.TabIndex = 0;
            // 
            // pushPanel
            // 
            this.pushPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pushPanel.Location = new System.Drawing.Point(66, 60);
            this.pushPanel.Max = 10F;
            this.pushPanel.Min = -10F;
            this.pushPanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.pushPanel.Name = "pushPanel";
            this.pushPanel.Scalar = null;
            this.pushPanel.Size = new System.Drawing.Size(104, 20);
            this.pushPanel.TabIndex = 3;
            this.pushPanel.Value = 0F;
            // 
            // PushAxisPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pushPanel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.constrainedAxisPanel);
            this.MinimumSize = new System.Drawing.Size(170, 83);
            this.Name = "PushAxisPanel";
            this.Size = new System.Drawing.Size(170, 85);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Chimera.GUI.Controls.Plugins.ConstrainedAxisPanel constrainedAxisPanel;
        private System.Windows.Forms.Label label1;
        private Chimera.GUI.UpdatedScalarPanel pushPanel;
    }
}

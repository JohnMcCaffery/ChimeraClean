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
            this.scalarPanel1 = new Chimera.GUI.ScalarPanel();
            this.constrainedAxisPanel = new Chimera.GUI.Controls.Plugins.ConstrainedAxisPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // scalarPanel1
            // 
            this.scalarPanel1.Location = new System.Drawing.Point(67, 60);
            this.scalarPanel1.Max = 10F;
            this.scalarPanel1.Min = -10F;
            this.scalarPanel1.MinimumSize = new System.Drawing.Size(95, 20);
            this.scalarPanel1.Name = "scalarPanel1";
            this.scalarPanel1.Size = new System.Drawing.Size(103, 20);
            this.scalarPanel1.TabIndex = 1;
            this.scalarPanel1.Value = 0F;
            // 
            // constrainedAxisPanel
            // 
            this.constrainedAxisPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.constrainedAxisPanel.Location = new System.Drawing.Point(0, 3);
            this.constrainedAxisPanel.MinimumSize = new System.Drawing.Size(163, 57);
            this.constrainedAxisPanel.Name = "constrainedAxisPanel";
            this.constrainedAxisPanel.Size = new System.Drawing.Size(170, 57);
            this.constrainedAxisPanel.TabIndex = 0;
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
            // PushAxisPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.scalarPanel1);
            this.Controls.Add(this.constrainedAxisPanel);
            this.MinimumSize = new System.Drawing.Size(170, 88);
            this.Name = "PushAxisPanel";
            this.Size = new System.Drawing.Size(170, 88);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Chimera.GUI.Controls.Plugins.ConstrainedAxisPanel constrainedAxisPanel;
        private Chimera.GUI.ScalarPanel scalarPanel1;
        private System.Windows.Forms.Label label1;
    }
}

namespace Chimera.Kinect.GUI.Axes {
    partial class KinectScaledAxisPanel {
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
            this.stateLabel = new System.Windows.Forms.Label();
            this.constrainedAxisPanel = new Chimera.GUI.Controls.Plugins.ConstrainedAxisPanel();
            this.scalePanel = new Chimera.GUI.UpdatedScalarPanel();
            this.deadzonePanel = new Chimera.GUI.UpdatedScalarPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // stateLabel
            // 
            this.stateLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.stateLabel.AutoSize = true;
            this.stateLabel.Location = new System.Drawing.Point(283, 72);
            this.stateLabel.Name = "stateLabel";
            this.stateLabel.Size = new System.Drawing.Size(48, 13);
            this.stateLabel.TabIndex = 4;
            this.stateLabel.Text = "Disabled";
            // 
            // constrainedAxisPanel
            // 
            this.constrainedAxisPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.constrainedAxisPanel.Axis = null;
            this.constrainedAxisPanel.Location = new System.Drawing.Point(0, 0);
            this.constrainedAxisPanel.MinimumSize = new System.Drawing.Size(163, 57);
            this.constrainedAxisPanel.Name = "constrainedAxisPanel";
            this.constrainedAxisPanel.Size = new System.Drawing.Size(333, 57);
            this.constrainedAxisPanel.TabIndex = 0;
            // 
            // scalePanel
            // 
            this.scalePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.scalePanel.Location = new System.Drawing.Point(74, 82);
            this.scalePanel.Max = 10F;
            this.scalePanel.Min = 0F;
            this.scalePanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.scalePanel.Name = "scalePanel";
            this.scalePanel.Scalar = null;
            this.scalePanel.Size = new System.Drawing.Size(256, 20);
            this.scalePanel.TabIndex = 7;
            this.scalePanel.Value = 0F;
            // 
            // deadzonePanel
            // 
            this.deadzonePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.deadzonePanel.Location = new System.Drawing.Point(96, 57);
            this.deadzonePanel.Max = 10F;
            this.deadzonePanel.Min = 0F;
            this.deadzonePanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.deadzonePanel.Name = "deadzonePanel";
            this.deadzonePanel.Scalar = null;
            this.deadzonePanel.Size = new System.Drawing.Size(234, 20);
            this.deadzonePanel.TabIndex = 6;
            this.deadzonePanel.Value = 0F;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Deadzone Scale";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Scale Scale";
            // 
            // KinectScaledAxisPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.stateLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.constrainedAxisPanel);
            this.Controls.Add(this.scalePanel);
            this.Controls.Add(this.deadzonePanel);
            this.MinimumSize = new System.Drawing.Size(333, 106);
            this.Name = "KinectScaledAxisPanel";
            this.Size = new System.Drawing.Size(333, 106);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Chimera.GUI.Controls.Plugins.ConstrainedAxisPanel constrainedAxisPanel;
        private System.Windows.Forms.Label stateLabel;
        private Chimera.GUI.UpdatedScalarPanel scalePanel;
        private Chimera.GUI.UpdatedScalarPanel deadzonePanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

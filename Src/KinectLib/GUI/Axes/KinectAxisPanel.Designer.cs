namespace Chimera.Kinect.GUI.Axes {
    partial class KinectAxisPanel {
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
            this.stateLabel = new System.Windows.Forms.Label();
            this.pushPanel = new Chimera.GUI.UpdatedScalarPanel();
            this.constrainedAxisPanel = new Chimera.GUI.Controls.Plugins.ConstrainedAxisPanel();
            this.anchorScaleSplit = new System.Windows.Forms.SplitContainer();
            this.deadzonePanel = new Chimera.GUI.UpdatedScalarPanel();
            this.scalePanel = new Chimera.GUI.UpdatedScalarPanel();
            ((System.ComponentModel.ISupportInitialize)(this.anchorScaleSplit)).BeginInit();
            this.anchorScaleSplit.Panel1.SuspendLayout();
            this.anchorScaleSplit.Panel2.SuspendLayout();
            this.anchorScaleSplit.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Raw";
            // 
            // stateLabel
            // 
            this.stateLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.stateLabel.AutoSize = true;
            this.stateLabel.Location = new System.Drawing.Point(184, 63);
            this.stateLabel.Name = "stateLabel";
            this.stateLabel.Size = new System.Drawing.Size(48, 13);
            this.stateLabel.TabIndex = 4;
            this.stateLabel.Text = "Disabled";
            // 
            // pushPanel
            // 
            this.pushPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pushPanel.Location = new System.Drawing.Point(68, 59);
            this.pushPanel.Max = 10F;
            this.pushPanel.Min = -10F;
            this.pushPanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.pushPanel.Name = "pushPanel";
            this.pushPanel.Scalar = null;
            this.pushPanel.Size = new System.Drawing.Size(111, 20);
            this.pushPanel.TabIndex = 3;
            this.pushPanel.Value = 0F;
            // 
            // constrainedAxisPanel
            // 
            this.constrainedAxisPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.constrainedAxisPanel.Axis = null;
            this.constrainedAxisPanel.Location = new System.Drawing.Point(0, 0);
            this.constrainedAxisPanel.MinimumSize = new System.Drawing.Size(163, 57);
            this.constrainedAxisPanel.Name = "constrainedAxisPanel";
            this.constrainedAxisPanel.Size = new System.Drawing.Size(163, 57);
            this.constrainedAxisPanel.TabIndex = 0;
            // 
            // anchorScaleSplit
            // 
            this.anchorScaleSplit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.anchorScaleSplit.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.anchorScaleSplit.IsSplitterFixed = true;
            this.anchorScaleSplit.Location = new System.Drawing.Point(0, 0);
            this.anchorScaleSplit.Name = "anchorScaleSplit";
            // 
            // anchorScaleSplit.Panel1
            // 
            this.anchorScaleSplit.Panel1.Controls.Add(this.constrainedAxisPanel);
            // 
            // anchorScaleSplit.Panel2
            // 
            this.anchorScaleSplit.Panel2.Controls.Add(this.scalePanel);
            this.anchorScaleSplit.Panel2.Controls.Add(this.deadzonePanel);
            this.anchorScaleSplit.Size = new System.Drawing.Size(235, 57);
            this.anchorScaleSplit.SplitterDistance = 130;
            this.anchorScaleSplit.TabIndex = 5;
            // 
            // deadzonePanel
            // 
            this.deadzonePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.deadzonePanel.Location = new System.Drawing.Point(0, 5);
            this.deadzonePanel.Max = 10F;
            this.deadzonePanel.Min = -10F;
            this.deadzonePanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.deadzonePanel.Name = "deadzonePanel";
            this.deadzonePanel.Scalar = null;
            this.deadzonePanel.Size = new System.Drawing.Size(101, 20);
            this.deadzonePanel.TabIndex = 6;
            this.deadzonePanel.Value = 0F;
            // 
            // scalePanel
            // 
            this.scalePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scalePanel.Location = new System.Drawing.Point(1, 31);
            this.scalePanel.Max = 10F;
            this.scalePanel.Min = -10F;
            this.scalePanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.scalePanel.Name = "scalePanel";
            this.scalePanel.Scalar = null;
            this.scalePanel.Size = new System.Drawing.Size(100, 20);
            this.scalePanel.TabIndex = 7;
            this.scalePanel.Value = 0F;
            // 
            // KinectAxisPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.anchorScaleSplit);
            this.Controls.Add(this.stateLabel);
            this.Controls.Add(this.pushPanel);
            this.Controls.Add(this.label1);
            this.MinimumSize = new System.Drawing.Size(235, 83);
            this.Name = "KinectAxisPanel";
            this.Size = new System.Drawing.Size(235, 83);
            this.anchorScaleSplit.Panel1.ResumeLayout(false);
            this.anchorScaleSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.anchorScaleSplit)).EndInit();
            this.anchorScaleSplit.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Chimera.GUI.Controls.Plugins.ConstrainedAxisPanel constrainedAxisPanel;
        private System.Windows.Forms.Label label1;
        private Chimera.GUI.UpdatedScalarPanel pushPanel;
        private System.Windows.Forms.Label stateLabel;
        private System.Windows.Forms.SplitContainer anchorScaleSplit;
        private Chimera.GUI.UpdatedScalarPanel scalePanel;
        private Chimera.GUI.UpdatedScalarPanel deadzonePanel;
    }
}

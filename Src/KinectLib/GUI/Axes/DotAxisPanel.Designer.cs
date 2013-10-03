namespace Chimera.Kinect.GUI.Axes {
    partial class DotAxisPanel {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DotAxisPanel));
            this.inputSplit = new System.Windows.Forms.SplitContainer();
            this.aPanel = new Chimera.GUI.UpdatedVectorPanel();
            this.bPanel = new Chimera.GUI.UpdatedVectorPanel();
            this.kinectAxisPanel = new Chimera.Kinect.GUI.Axes.KinectAxisPanel();
            ((System.ComponentModel.ISupportInitialize)(this.inputSplit)).BeginInit();
            this.inputSplit.Panel1.SuspendLayout();
            this.inputSplit.Panel2.SuspendLayout();
            this.inputSplit.SuspendLayout();
            this.SuspendLayout();
            // 
            // inputSplit
            // 
            this.inputSplit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.inputSplit.IsSplitterFixed = true;
            this.inputSplit.Location = new System.Drawing.Point(0, 89);
            this.inputSplit.Name = "inputSplit";
            // 
            // inputSplit.Panel1
            // 
            this.inputSplit.Panel1.Controls.Add(this.aPanel);
            // 
            // inputSplit.Panel2
            // 
            this.inputSplit.Panel2.Controls.Add(this.bPanel);
            this.inputSplit.Size = new System.Drawing.Size(560, 137);
            this.inputSplit.SplitterDistance = 278;
            this.inputSplit.TabIndex = 0;
            // 
            // aPanel
            // 
            this.aPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.aPanel.Location = new System.Drawing.Point(0, 0);
            this.aPanel.Max = 10F;
            this.aPanel.MaxV = ((OpenMetaverse.Vector3)(resources.GetObject("aPanel.MaxV")));
            this.aPanel.Min = -10F;
            this.aPanel.MinimumSize = new System.Drawing.Size(103, 95);
            this.aPanel.MinV = ((OpenMetaverse.Vector3)(resources.GetObject("aPanel.MinV")));
            this.aPanel.Name = "aPanel";
            this.aPanel.Size = new System.Drawing.Size(278, 137);
            this.aPanel.TabIndex = 0;
            this.aPanel.Value = ((OpenMetaverse.Vector3)(resources.GetObject("aPanel.Value")));
            this.aPanel.Vector = null;
            this.aPanel.X = 0F;
            this.aPanel.Y = 0F;
            this.aPanel.Z = 0F;
            // 
            // bPanel
            // 
            this.bPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bPanel.Location = new System.Drawing.Point(0, 0);
            this.bPanel.Max = 10F;
            this.bPanel.MaxV = ((OpenMetaverse.Vector3)(resources.GetObject("bPanel.MaxV")));
            this.bPanel.Min = -10F;
            this.bPanel.MinimumSize = new System.Drawing.Size(103, 95);
            this.bPanel.MinV = ((OpenMetaverse.Vector3)(resources.GetObject("bPanel.MinV")));
            this.bPanel.Name = "bPanel";
            this.bPanel.Size = new System.Drawing.Size(278, 137);
            this.bPanel.TabIndex = 0;
            this.bPanel.Value = ((OpenMetaverse.Vector3)(resources.GetObject("bPanel.Value")));
            this.bPanel.Vector = null;
            this.bPanel.X = 0F;
            this.bPanel.Y = 0F;
            this.bPanel.Z = 0F;
            // 
            // kinectAxisPanel
            // 
            this.kinectAxisPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.kinectAxisPanel.Location = new System.Drawing.Point(0, 0);
            this.kinectAxisPanel.MinimumSize = new System.Drawing.Size(170, 83);
            this.kinectAxisPanel.Name = "kinectAxisPanel";
            this.kinectAxisPanel.Size = new System.Drawing.Size(560, 83);
            this.kinectAxisPanel.TabIndex = 1;
            // 
            // AngleAxisPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.kinectAxisPanel);
            this.Controls.Add(this.inputSplit);
            this.Name = "AngleAxisPanel";
            this.Size = new System.Drawing.Size(560, 226);
            this.inputSplit.Panel1.ResumeLayout(false);
            this.inputSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.inputSplit)).EndInit();
            this.inputSplit.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer inputSplit;
        private Chimera.GUI.UpdatedVectorPanel aPanel;
        private Chimera.GUI.UpdatedVectorPanel bPanel;
        private KinectAxisPanel kinectAxisPanel;
    }
}

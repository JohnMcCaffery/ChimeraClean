namespace Test {
    partial class TestForm {
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestForm));
            this.W = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.yPanel = new KinectLib.GUI.UpdatedScalarPanel();
            this.xPanel = new KinectLib.GUI.UpdatedScalarPanel();
            this.intersectionPanel = new KinectLib.GUI.UpdatedVectorPanel();
            this.pointDirPanel = new KinectLib.GUI.UpdatedVectorPanel();
            this.pointStartPanel = new KinectLib.GUI.UpdatedVectorPanel();
            this.sidePanel = new KinectLib.GUI.UpdatedVectorPanel();
            this.topPanel = new KinectLib.GUI.UpdatedVectorPanel();
            this.normalPanel = new KinectLib.GUI.UpdatedVectorPanel();
            this.hPanel = new KinectLib.GUI.UpdatedScalarPanel();
            this.topLeftPanel = new KinectLib.GUI.UpdatedVectorPanel();
            this.wPanel = new KinectLib.GUI.UpdatedScalarPanel();
            this.SuspendLayout();
            // 
            // W
            // 
            this.W.AutoSize = true;
            this.W.Location = new System.Drawing.Point(16, 320);
            this.W.Name = "W";
            this.W.Size = new System.Drawing.Size(18, 13);
            this.W.TabIndex = 9;
            this.W.Text = "W";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 345);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(15, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "H";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 397);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Y";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 372);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "X";
            // 
            // yPanel
            // 
            this.yPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.yPanel.Location = new System.Drawing.Point(35, 394);
            this.yPanel.Max = 10;
            this.yPanel.Min = -10;
            this.yPanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.yPanel.Name = "yPanel";
            this.yPanel.Scalar = null;
            this.yPanel.Size = new System.Drawing.Size(678, 20);
            this.yPanel.TabIndex = 12;
            this.yPanel.Value = 0F;
            // 
            // xPanel
            // 
            this.xPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.xPanel.Location = new System.Drawing.Point(35, 368);
            this.xPanel.Max = 10;
            this.xPanel.Min = -10;
            this.xPanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.xPanel.Name = "xPanel";
            this.xPanel.Scalar = null;
            this.xPanel.Size = new System.Drawing.Size(678, 20);
            this.xPanel.TabIndex = 11;
            this.xPanel.Value = 0F;
            // 
            // intersectionPanel
            // 
            this.intersectionPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.intersectionPanel.Text = "Name";
            this.intersectionPanel.Location = new System.Drawing.Point(13, 215);
            this.intersectionPanel.Max = 1024f;
            this.intersectionPanel.MaxV = ((OpenMetaverse.Vector3)(resources.GetObject("intersectionPanel.MaxV")));
            this.intersectionPanel.Min = -1024f;
            this.intersectionPanel.MinimumSize = new System.Drawing.Size(103, 95);
            this.intersectionPanel.MinV = ((OpenMetaverse.Vector3)(resources.GetObject("intersectionPanel.MinV")));
            this.intersectionPanel.Name = "intersectionPanel";
            this.intersectionPanel.Size = new System.Drawing.Size(700, 95);
            this.intersectionPanel.TabIndex = 8;
            this.intersectionPanel.Value = ((OpenMetaverse.Vector3)(resources.GetObject("intersectionPanel.Value")));
            this.intersectionPanel.Vector = null;
            this.intersectionPanel.X = 0F;
            this.intersectionPanel.Y = 0F;
            this.intersectionPanel.Z = 0F;
            // 
            // pointDirPanel
            // 
            this.pointDirPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pointDirPanel.Text = "Point Direction";
            this.pointDirPanel.Location = new System.Drawing.Point(478, 114);
            this.pointDirPanel.Max = 1024f;
            this.pointDirPanel.MaxV = ((OpenMetaverse.Vector3)(resources.GetObject("pointDirPanel.MaxV")));
            this.pointDirPanel.Min = -1024f;
            this.pointDirPanel.MinimumSize = new System.Drawing.Size(103, 95);
            this.pointDirPanel.MinV = ((OpenMetaverse.Vector3)(resources.GetObject("pointDirPanel.MinV")));
            this.pointDirPanel.Name = "pointDirPanel";
            this.pointDirPanel.Size = new System.Drawing.Size(235, 95);
            this.pointDirPanel.TabIndex = 7;
            this.pointDirPanel.Value = ((OpenMetaverse.Vector3)(resources.GetObject("pointDirPanel.Value")));
            this.pointDirPanel.Vector = null;
            this.pointDirPanel.X = 0F;
            this.pointDirPanel.Y = 0F;
            this.pointDirPanel.Z = 0F;
            // 
            // pointStartPanel
            // 
            this.pointStartPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pointStartPanel.Text = "Point Start";
            this.pointStartPanel.Location = new System.Drawing.Point(478, 12);
            this.pointStartPanel.Max = 1024f;
            this.pointStartPanel.MaxV = ((OpenMetaverse.Vector3)(resources.GetObject("pointStartPanel.MaxV")));
            this.pointStartPanel.Min = -1024f;
            this.pointStartPanel.MinimumSize = new System.Drawing.Size(103, 95);
            this.pointStartPanel.MinV = ((OpenMetaverse.Vector3)(resources.GetObject("pointStartPanel.MinV")));
            this.pointStartPanel.Name = "pointStartPanel";
            this.pointStartPanel.Size = new System.Drawing.Size(235, 95);
            this.pointStartPanel.TabIndex = 6;
            this.pointStartPanel.Value = ((OpenMetaverse.Vector3)(resources.GetObject("pointStartPanel.Value")));
            this.pointStartPanel.Vector = null;
            this.pointStartPanel.X = 0F;
            this.pointStartPanel.Y = 0F;
            this.pointStartPanel.Z = 0F;
            // 
            // sidePanel
            // 
            this.sidePanel.Text = "Side";
            this.sidePanel.Location = new System.Drawing.Point(234, 114);
            this.sidePanel.Max = 1024f;
            this.sidePanel.MaxV = ((OpenMetaverse.Vector3)(resources.GetObject("sidePanel.MaxV")));
            this.sidePanel.Min = -1024f;
            this.sidePanel.MinimumSize = new System.Drawing.Size(103, 95);
            this.sidePanel.MinV = ((OpenMetaverse.Vector3)(resources.GetObject("sidePanel.MinV")));
            this.sidePanel.Name = "sidePanel";
            this.sidePanel.Size = new System.Drawing.Size(238, 95);
            this.sidePanel.TabIndex = 5;
            this.sidePanel.Value = ((OpenMetaverse.Vector3)(resources.GetObject("sidePanel.Value")));
            this.sidePanel.Vector = null;
            this.sidePanel.X = 0F;
            this.sidePanel.Y = 0F;
            this.sidePanel.Z = 0F;
            // 
            // topPanel
            // 
            this.topPanel.Text = "Top";
            this.topPanel.Location = new System.Drawing.Point(234, 12);
            this.topPanel.Max = 1024f;
            this.topPanel.MaxV = ((OpenMetaverse.Vector3)(resources.GetObject("topPanel.MaxV")));
            this.topPanel.Min = -1024f;
            this.topPanel.MinimumSize = new System.Drawing.Size(103, 95);
            this.topPanel.MinV = ((OpenMetaverse.Vector3)(resources.GetObject("topPanel.MinV")));
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(238, 95);
            this.topPanel.TabIndex = 4;
            this.topPanel.Value = ((OpenMetaverse.Vector3)(resources.GetObject("topPanel.Value")));
            this.topPanel.Vector = null;
            this.topPanel.X = 0F;
            this.topPanel.Y = 0F;
            this.topPanel.Z = 0F;
            // 
            // normalPanel
            // 
            this.normalPanel.Text = "Normal";
            this.normalPanel.Location = new System.Drawing.Point(13, 114);
            this.normalPanel.Max = 1024f;
            this.normalPanel.MaxV = ((OpenMetaverse.Vector3)(resources.GetObject("normalPanel.MaxV")));
            this.normalPanel.Min = -1024f;
            this.normalPanel.MinimumSize = new System.Drawing.Size(103, 95);
            this.normalPanel.MinV = ((OpenMetaverse.Vector3)(resources.GetObject("normalPanel.MinV")));
            this.normalPanel.Name = "normalPanel";
            this.normalPanel.Size = new System.Drawing.Size(215, 95);
            this.normalPanel.TabIndex = 3;
            this.normalPanel.Value = ((OpenMetaverse.Vector3)(resources.GetObject("normalPanel.Value")));
            this.normalPanel.Vector = null;
            this.normalPanel.X = 0F;
            this.normalPanel.Y = 0F;
            this.normalPanel.Z = 0F;
            // 
            // hPanel
            // 
            this.hPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.hPanel.Location = new System.Drawing.Point(35, 342);
            this.hPanel.Max = 10;
            this.hPanel.Min = -10;
            this.hPanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.hPanel.Name = "hPanel";
            this.hPanel.Scalar = null;
            this.hPanel.Size = new System.Drawing.Size(678, 20);
            this.hPanel.TabIndex = 2;
            this.hPanel.Value = 0F;
            // 
            // topLeftPanel
            // 
            this.topLeftPanel.Text = "Top Left";
            this.topLeftPanel.Location = new System.Drawing.Point(12, 12);
            this.topLeftPanel.Max = 1024f;
            this.topLeftPanel.MaxV = ((OpenMetaverse.Vector3)(resources.GetObject("topLeftPanel.MaxV")));
            this.topLeftPanel.Min = -1024f;
            this.topLeftPanel.MinimumSize = new System.Drawing.Size(103, 95);
            this.topLeftPanel.MinV = ((OpenMetaverse.Vector3)(resources.GetObject("topLeftPanel.MinV")));
            this.topLeftPanel.Name = "topLeftPanel";
            this.topLeftPanel.Size = new System.Drawing.Size(216, 95);
            this.topLeftPanel.TabIndex = 1;
            this.topLeftPanel.Value = ((OpenMetaverse.Vector3)(resources.GetObject("topLeftPanel.Value")));
            this.topLeftPanel.Vector = null;
            this.topLeftPanel.X = 0F;
            this.topLeftPanel.Y = 0F;
            this.topLeftPanel.Z = 0F;
            // 
            // wPanel
            // 
            this.wPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.wPanel.Location = new System.Drawing.Point(35, 316);
            this.wPanel.Max = 10;
            this.wPanel.Min = -10;
            this.wPanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.wPanel.Name = "wPanel";
            this.wPanel.Scalar = null;
            this.wPanel.Size = new System.Drawing.Size(678, 20);
            this.wPanel.TabIndex = 0;
            this.wPanel.Value = 0F;
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(725, 545);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.yPanel);
            this.Controls.Add(this.xPanel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.W);
            this.Controls.Add(this.intersectionPanel);
            this.Controls.Add(this.pointDirPanel);
            this.Controls.Add(this.pointStartPanel);
            this.Controls.Add(this.sidePanel);
            this.Controls.Add(this.topPanel);
            this.Controls.Add(this.normalPanel);
            this.Controls.Add(this.hPanel);
            this.Controls.Add(this.topLeftPanel);
            this.Controls.Add(this.wPanel);
            this.Name = "TestForm";
            this.Text = "TestForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private KinectLib.GUI.UpdatedScalarPanel wPanel;
        private KinectLib.GUI.UpdatedVectorPanel topLeftPanel;
        private KinectLib.GUI.UpdatedScalarPanel hPanel;
        private KinectLib.GUI.UpdatedVectorPanel normalPanel;
        private KinectLib.GUI.UpdatedVectorPanel topPanel;
        private KinectLib.GUI.UpdatedVectorPanel sidePanel;
        private KinectLib.GUI.UpdatedVectorPanel pointStartPanel;
        private KinectLib.GUI.UpdatedVectorPanel pointDirPanel;
        private KinectLib.GUI.UpdatedVectorPanel intersectionPanel;
        private System.Windows.Forms.Label W;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private KinectLib.GUI.UpdatedScalarPanel yPanel;
        private KinectLib.GUI.UpdatedScalarPanel xPanel;
    }
}
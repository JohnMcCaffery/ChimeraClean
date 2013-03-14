namespace Chimera.Kinect.GUI {
    partial class KinectWindowPanel {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KinectWindowPanel));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.yPanel = new KinectLib.GUI.ScalarPanel();
            this.xPanel = new KinectLib.GUI.ScalarPanel();
            this.sidePanel = new KinectLib.GUI.UpdatedVectorPanel();
            this.topPanel = new KinectLib.GUI.UpdatedVectorPanel();
            this.topLeftPanel = new KinectLib.GUI.UpdatedVectorPanel();
            this.intersectionPanel = new KinectLib.GUI.UpdatedVectorPanel();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 303);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "X";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 329);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Y";
            // 
            // yPanel
            // 
            this.yPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.yPanel.Location = new System.Drawing.Point(22, 329);
            this.yPanel.Max = 1000;
            this.yPanel.Min = -1000;
            this.yPanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.yPanel.Name = "yPanel";
            this.yPanel.Scalar = null;
            this.yPanel.Size = new System.Drawing.Size(801, 20);
            this.yPanel.TabIndex = 4;
            // 
            // xPanel
            // 
            this.xPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.xPanel.Location = new System.Drawing.Point(22, 303);
            this.xPanel.Max = 1000;
            this.xPanel.Min = -1000;
            this.xPanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.xPanel.Name = "xPanel";
            this.xPanel.Scalar = null;
            this.xPanel.Size = new System.Drawing.Size(801, 20);
            this.xPanel.TabIndex = 3;
            // 
            // sidePanel
            // 
            this.sidePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.sidePanel.DisplayName = "Side";
            this.sidePanel.Location = new System.Drawing.Point(444, 101);
            this.sidePanel.Max = 1024D;
            this.sidePanel.MaxV = ((OpenMetaverse.Vector3)(resources.GetObject("sidePanel.MaxV")));
            this.sidePanel.Min = -1024D;
            this.sidePanel.MinimumSize = new System.Drawing.Size(103, 95);
            this.sidePanel.MinV = ((OpenMetaverse.Vector3)(resources.GetObject("sidePanel.MinV")));
            this.sidePanel.Name = "sidePanel";
            this.sidePanel.Size = new System.Drawing.Size(379, 95);
            this.sidePanel.TabIndex = 2;
            this.sidePanel.Value = ((OpenMetaverse.Vector3)(resources.GetObject("sidePanel.Value")));
            this.sidePanel.Vector = null;
            this.sidePanel.X = 0F;
            this.sidePanel.Y = 0F;
            this.sidePanel.Z = 0F;
            // 
            // topPanel
            // 
            this.topPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.topPanel.DisplayName = "Top";
            this.topPanel.Location = new System.Drawing.Point(0, 101);
            this.topPanel.Max = 1024D;
            this.topPanel.MaxV = ((OpenMetaverse.Vector3)(resources.GetObject("topPanel.MaxV")));
            this.topPanel.Min = -1024D;
            this.topPanel.MinimumSize = new System.Drawing.Size(103, 95);
            this.topPanel.MinV = ((OpenMetaverse.Vector3)(resources.GetObject("topPanel.MinV")));
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(438, 95);
            this.topPanel.TabIndex = 1;
            this.topPanel.Value = ((OpenMetaverse.Vector3)(resources.GetObject("topPanel.Value")));
            this.topPanel.Vector = null;
            this.topPanel.X = 0F;
            this.topPanel.Y = 0F;
            this.topPanel.Z = 0F;
            // 
            // topLeftPanel
            // 
            this.topLeftPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.topLeftPanel.DisplayName = "Top Left";
            this.topLeftPanel.Location = new System.Drawing.Point(0, 0);
            this.topLeftPanel.Max = 1024D;
            this.topLeftPanel.MaxV = ((OpenMetaverse.Vector3)(resources.GetObject("topLeftPanel.MaxV")));
            this.topLeftPanel.Min = -1024D;
            this.topLeftPanel.MinimumSize = new System.Drawing.Size(103, 95);
            this.topLeftPanel.MinV = ((OpenMetaverse.Vector3)(resources.GetObject("topLeftPanel.MinV")));
            this.topLeftPanel.Name = "topLeftPanel";
            this.topLeftPanel.Size = new System.Drawing.Size(823, 95);
            this.topLeftPanel.TabIndex = 0;
            this.topLeftPanel.Value = ((OpenMetaverse.Vector3)(resources.GetObject("topLeftPanel.Value")));
            this.topLeftPanel.Vector = null;
            this.topLeftPanel.X = 0F;
            this.topLeftPanel.Y = 0F;
            this.topLeftPanel.Z = 0F;
            // 
            // intersectionPanel
            // 
            this.intersectionPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.intersectionPanel.DisplayName = "Intersection";
            this.intersectionPanel.Location = new System.Drawing.Point(0, 205);
            this.intersectionPanel.Max = 1024D;
            this.intersectionPanel.MaxV = ((OpenMetaverse.Vector3)(resources.GetObject("intersectionPanel.MaxV")));
            this.intersectionPanel.Min = -1024D;
            this.intersectionPanel.MinimumSize = new System.Drawing.Size(103, 95);
            this.intersectionPanel.MinV = ((OpenMetaverse.Vector3)(resources.GetObject("intersectionPanel.MinV")));
            this.intersectionPanel.Name = "intersectionPanel";
            this.intersectionPanel.Size = new System.Drawing.Size(823, 95);
            this.intersectionPanel.TabIndex = 7;
            this.intersectionPanel.Value = ((OpenMetaverse.Vector3)(resources.GetObject("intersectionPanel.Value")));
            this.intersectionPanel.Vector = null;
            this.intersectionPanel.X = 0F;
            this.intersectionPanel.Y = 0F;
            this.intersectionPanel.Z = 0F;
            // 
            // KinectWindowPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.intersectionPanel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.yPanel);
            this.Controls.Add(this.xPanel);
            this.Controls.Add(this.sidePanel);
            this.Controls.Add(this.topPanel);
            this.Controls.Add(this.topLeftPanel);
            this.Name = "KinectWindowPanel";
            this.Size = new System.Drawing.Size(823, 368);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private KinectLib.GUI.UpdatedVectorPanel topLeftPanel;
        private KinectLib.GUI.UpdatedVectorPanel topPanel;
        private KinectLib.GUI.UpdatedVectorPanel sidePanel;
        private KinectLib.GUI.ScalarPanel xPanel;
        private KinectLib.GUI.ScalarPanel yPanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private KinectLib.GUI.UpdatedVectorPanel intersectionPanel;
    }
}

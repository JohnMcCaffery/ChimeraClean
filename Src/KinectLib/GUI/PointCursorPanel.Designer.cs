namespace Chimera.Kinect.GUI {
    partial class PointCursorPanel {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PointCursorPanel));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.xPanel = new KinectLib.GUI.UpdatedScalarPanel();
            this.intersectionPanel = new KinectLib.GUI.UpdatedVectorPanel();
            this.pointDirPanel = new KinectLib.GUI.UpdatedVectorPanel();
            this.pointStartPanel = new KinectLib.GUI.UpdatedVectorPanel();
            this.screenHPanel = new KinectLib.GUI.UpdatedScalarPanel();
            this.screenWPanel = new KinectLib.GUI.UpdatedScalarPanel();
            this.topLeftPanel = new KinectLib.GUI.UpdatedVectorPanel();
            this.worldHPanel = new KinectLib.GUI.UpdatedScalarPanel();
            this.worldWPanel = new KinectLib.GUI.UpdatedScalarPanel();
            this.normalPanel = new KinectLib.GUI.UpdatedVectorPanel();
            this.yPanel = new KinectLib.GUI.UpdatedScalarPanel();
            this.sidePanel = new KinectLib.GUI.UpdatedVectorPanel();
            this.topPanel = new KinectLib.GUI.UpdatedVectorPanel();
            this.enabledCheck = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 274);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "X";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 300);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Y";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 330);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "World W";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 353);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "World H";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(258, 351);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Screen H";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(257, 328);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Screen W";
            // 
            // xPanel
            // 
            this.xPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.xPanel.Location = new System.Drawing.Point(23, 274);
            this.xPanel.Max = 1F;
            this.xPanel.Min = 0F;
            this.xPanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.xPanel.Name = "xPanel";
            this.xPanel.Scalar = null;
            this.xPanel.Size = new System.Drawing.Size(439, 20);
            this.xPanel.TabIndex = 3;
            this.xPanel.Value = 0F;
            // 
            // intersectionPanel
            // 
            this.intersectionPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.intersectionPanel.Location = new System.Drawing.Point(0, 180);
            this.intersectionPanel.Max = 1024F;
            this.intersectionPanel.MaxV = ((OpenMetaverse.Vector3)(resources.GetObject("intersectionPanel.MaxV")));
            this.intersectionPanel.Min = -1024F;
            this.intersectionPanel.MinimumSize = new System.Drawing.Size(103, 95);
            this.intersectionPanel.MinV = ((OpenMetaverse.Vector3)(resources.GetObject("intersectionPanel.MinV")));
            this.intersectionPanel.Name = "intersectionPanel";
            this.intersectionPanel.Size = new System.Drawing.Size(465, 95);
            this.intersectionPanel.TabIndex = 7;
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
            this.pointDirPanel.Location = new System.Drawing.Point(0, 91);
            this.pointDirPanel.Max = 1024F;
            this.pointDirPanel.MaxV = ((OpenMetaverse.Vector3)(resources.GetObject("pointDirPanel.MaxV")));
            this.pointDirPanel.Min = -1024F;
            this.pointDirPanel.MinimumSize = new System.Drawing.Size(103, 95);
            this.pointDirPanel.MinV = ((OpenMetaverse.Vector3)(resources.GetObject("pointDirPanel.MinV")));
            this.pointDirPanel.Name = "pointDirPanel";
            this.pointDirPanel.Size = new System.Drawing.Size(164, 95);
            this.pointDirPanel.TabIndex = 18;
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
            this.pointStartPanel.Location = new System.Drawing.Point(0, 0);
            this.pointStartPanel.Max = 1024F;
            this.pointStartPanel.MaxV = ((OpenMetaverse.Vector3)(resources.GetObject("pointStartPanel.MaxV")));
            this.pointStartPanel.Min = -1024F;
            this.pointStartPanel.MinimumSize = new System.Drawing.Size(103, 95);
            this.pointStartPanel.MinV = ((OpenMetaverse.Vector3)(resources.GetObject("pointStartPanel.MinV")));
            this.pointStartPanel.Name = "pointStartPanel";
            this.pointStartPanel.Size = new System.Drawing.Size(161, 95);
            this.pointStartPanel.TabIndex = 17;
            this.pointStartPanel.Value = ((OpenMetaverse.Vector3)(resources.GetObject("pointStartPanel.Value")));
            this.pointStartPanel.Vector = null;
            this.pointStartPanel.X = 0F;
            this.pointStartPanel.Y = 0F;
            this.pointStartPanel.Z = 0F;
            // 
            // screenHPanel
            // 
            this.screenHPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.screenHPanel.Location = new System.Drawing.Point(313, 349);
            this.screenHPanel.Max = 1000F;
            this.screenHPanel.Min = -1000F;
            this.screenHPanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.screenHPanel.Name = "screenHPanel";
            this.screenHPanel.Scalar = null;
            this.screenHPanel.Size = new System.Drawing.Size(149, 20);
            this.screenHPanel.TabIndex = 15;
            this.screenHPanel.Value = 0F;
            // 
            // screenWPanel
            // 
            this.screenWPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.screenWPanel.Location = new System.Drawing.Point(313, 326);
            this.screenWPanel.Max = 1000F;
            this.screenWPanel.Min = -1000F;
            this.screenWPanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.screenWPanel.Name = "screenWPanel";
            this.screenWPanel.Scalar = null;
            this.screenWPanel.Size = new System.Drawing.Size(149, 20);
            this.screenWPanel.TabIndex = 13;
            this.screenWPanel.Value = 0F;
            // 
            // topLeftPanel
            // 
            this.topLeftPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.topLeftPanel.Location = new System.Drawing.Point(313, 91);
            this.topLeftPanel.Max = 1024F;
            this.topLeftPanel.MaxV = ((OpenMetaverse.Vector3)(resources.GetObject("topLeftPanel.MaxV")));
            this.topLeftPanel.Min = -1024F;
            this.topLeftPanel.MinimumSize = new System.Drawing.Size(103, 95);
            this.topLeftPanel.MinV = ((OpenMetaverse.Vector3)(resources.GetObject("topLeftPanel.MinV")));
            this.topLeftPanel.Name = "topLeftPanel";
            this.topLeftPanel.Size = new System.Drawing.Size(149, 95);
            this.topLeftPanel.TabIndex = 0;
            this.topLeftPanel.Value = ((OpenMetaverse.Vector3)(resources.GetObject("topLeftPanel.Value")));
            this.topLeftPanel.Vector = null;
            this.topLeftPanel.X = 0F;
            this.topLeftPanel.Y = 0F;
            this.topLeftPanel.Z = 0F;
            // 
            // worldHPanel
            // 
            this.worldHPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.worldHPanel.Location = new System.Drawing.Point(58, 349);
            this.worldHPanel.Max = 1000F;
            this.worldHPanel.Min = -1000F;
            this.worldHPanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.worldHPanel.Name = "worldHPanel";
            this.worldHPanel.Scalar = null;
            this.worldHPanel.Size = new System.Drawing.Size(194, 20);
            this.worldHPanel.TabIndex = 11;
            this.worldHPanel.Value = 0F;
            // 
            // worldWPanel
            // 
            this.worldWPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.worldWPanel.Location = new System.Drawing.Point(58, 326);
            this.worldWPanel.Max = 1000F;
            this.worldWPanel.Min = -1000F;
            this.worldWPanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.worldWPanel.Name = "worldWPanel";
            this.worldWPanel.Scalar = null;
            this.worldWPanel.Size = new System.Drawing.Size(194, 20);
            this.worldWPanel.TabIndex = 9;
            this.worldWPanel.Value = 0F;
            // 
            // normalPanel
            // 
            this.normalPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.normalPanel.Location = new System.Drawing.Point(313, 3);
            this.normalPanel.Max = 1024F;
            this.normalPanel.MaxV = ((OpenMetaverse.Vector3)(resources.GetObject("normalPanel.MaxV")));
            this.normalPanel.Min = -1024F;
            this.normalPanel.MinimumSize = new System.Drawing.Size(103, 95);
            this.normalPanel.MinV = ((OpenMetaverse.Vector3)(resources.GetObject("normalPanel.MinV")));
            this.normalPanel.Name = "normalPanel";
            this.normalPanel.Size = new System.Drawing.Size(149, 95);
            this.normalPanel.TabIndex = 8;
            this.normalPanel.Value = ((OpenMetaverse.Vector3)(resources.GetObject("normalPanel.Value")));
            this.normalPanel.Vector = null;
            this.normalPanel.X = 0F;
            this.normalPanel.Y = 0F;
            this.normalPanel.Z = 0F;
            // 
            // yPanel
            // 
            this.yPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.yPanel.Location = new System.Drawing.Point(23, 300);
            this.yPanel.Max = 1F;
            this.yPanel.Min = 0F;
            this.yPanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.yPanel.Name = "yPanel";
            this.yPanel.Scalar = null;
            this.yPanel.Size = new System.Drawing.Size(439, 20);
            this.yPanel.TabIndex = 4;
            this.yPanel.Value = 0F;
            // 
            // sidePanel
            // 
            this.sidePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.sidePanel.Location = new System.Drawing.Point(167, 91);
            this.sidePanel.Max = 1024F;
            this.sidePanel.MaxV = ((OpenMetaverse.Vector3)(resources.GetObject("sidePanel.MaxV")));
            this.sidePanel.Min = -1024F;
            this.sidePanel.MinimumSize = new System.Drawing.Size(103, 95);
            this.sidePanel.MinV = ((OpenMetaverse.Vector3)(resources.GetObject("sidePanel.MinV")));
            this.sidePanel.Name = "sidePanel";
            this.sidePanel.Size = new System.Drawing.Size(143, 95);
            this.sidePanel.TabIndex = 2;
            this.sidePanel.Value = ((OpenMetaverse.Vector3)(resources.GetObject("sidePanel.Value")));
            this.sidePanel.Vector = null;
            this.sidePanel.X = 0F;
            this.sidePanel.Y = 0F;
            this.sidePanel.Z = 0F;
            // 
            // topPanel
            // 
            this.topPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.topPanel.Location = new System.Drawing.Point(167, 0);
            this.topPanel.Max = 1024F;
            this.topPanel.MaxV = ((OpenMetaverse.Vector3)(resources.GetObject("topPanel.MaxV")));
            this.topPanel.Min = -1024F;
            this.topPanel.MinimumSize = new System.Drawing.Size(103, 95);
            this.topPanel.MinV = ((OpenMetaverse.Vector3)(resources.GetObject("topPanel.MinV")));
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(140, 95);
            this.topPanel.TabIndex = 1;
            this.topPanel.Value = ((OpenMetaverse.Vector3)(resources.GetObject("topPanel.Value")));
            this.topPanel.Vector = null;
            this.topPanel.X = 0F;
            this.topPanel.Y = 0F;
            this.topPanel.Z = 0F;
            // 
            // enabledCheck
            // 
            this.enabledCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.enabledCheck.AutoSize = true;
            this.enabledCheck.Location = new System.Drawing.Point(397, 0);
            this.enabledCheck.Name = "enabledCheck";
            this.enabledCheck.Size = new System.Drawing.Size(65, 17);
            this.enabledCheck.TabIndex = 19;
            this.enabledCheck.Text = "Enabled";
            this.enabledCheck.UseVisualStyleBackColor = true;
            this.enabledCheck.CheckedChanged += new System.EventHandler(this.enabledCheck_CheckedChanged);
            // 
            // PointCursorPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.enabledCheck);
            this.Controls.Add(this.xPanel);
            this.Controls.Add(this.intersectionPanel);
            this.Controls.Add(this.pointDirPanel);
            this.Controls.Add(this.pointStartPanel);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.screenHPanel);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.screenWPanel);
            this.Controls.Add(this.topLeftPanel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.worldHPanel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.worldWPanel);
            this.Controls.Add(this.normalPanel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.yPanel);
            this.Controls.Add(this.sidePanel);
            this.Controls.Add(this.topPanel);
            this.MinimumSize = new System.Drawing.Size(465, 375);
            this.Name = "PointCursorPanel";
            this.Size = new System.Drawing.Size(465, 375);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private KinectLib.GUI.UpdatedVectorPanel topLeftPanel;
        private KinectLib.GUI.UpdatedVectorPanel topPanel;
        private KinectLib.GUI.UpdatedVectorPanel sidePanel;
        private KinectLib.GUI.UpdatedScalarPanel xPanel;
        private KinectLib.GUI.UpdatedScalarPanel yPanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private KinectLib.GUI.UpdatedVectorPanel intersectionPanel;
        private KinectLib.GUI.UpdatedVectorPanel normalPanel;
        private System.Windows.Forms.Label label3;
        private KinectLib.GUI.UpdatedScalarPanel worldWPanel;
        private System.Windows.Forms.Label label4;
        private KinectLib.GUI.UpdatedScalarPanel worldHPanel;
        private System.Windows.Forms.Label label5;
        private KinectLib.GUI.UpdatedScalarPanel screenHPanel;
        private System.Windows.Forms.Label label6;
        private KinectLib.GUI.UpdatedScalarPanel screenWPanel;
        private KinectLib.GUI.UpdatedVectorPanel pointDirPanel;
        private KinectLib.GUI.UpdatedVectorPanel pointStartPanel;
        private System.Windows.Forms.CheckBox enabledCheck;
    }
}

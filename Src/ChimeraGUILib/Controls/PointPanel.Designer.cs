namespace KinectLib {
    partial class PointPanel {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PointPanel));
            ChimeraLib.Window window4 = new ChimeraLib.Window();
            UtilLib.Rotation rotation4 = new UtilLib.Rotation();
            this.xLabel = new System.Windows.Forms.Label();
            this.yLabel = new System.Windows.Forms.Label();
            this.initButton = new System.Windows.Forms.Button();
            this.topYLabel = new System.Windows.Forms.Label();
            this.topXLabel = new System.Windows.Forms.Label();
            this.topZLabel = new System.Windows.Forms.Label();
            this.sideZLabel = new System.Windows.Forms.Label();
            this.sideYLabel = new System.Windows.Forms.Label();
            this.sideXLabel = new System.Windows.Forms.Label();
            this.graphicBox = new System.Windows.Forms.GroupBox();
            this.drawPanel = new System.Windows.Forms.Panel();
            this.planeNormalZLabel = new System.Windows.Forms.Label();
            this.planeNormalYLabel = new System.Windows.Forms.Label();
            this.planeNormalXLabel = new System.Windows.Forms.Label();
            this.topLeftZLabel = new System.Windows.Forms.Label();
            this.topLeftYLabel = new System.Windows.Forms.Label();
            this.topLeftXLabel = new System.Windows.Forms.Label();
            this.pointDirPanel = new ProxyTestGUI.RotationPanel();
            this.pointStartPanel = new ProxyTestGUI.VectorPanel();
            this.kinectRotationPanel = new ProxyTestGUI.RotationPanel();
            this.kinectPositionPanel = new ProxyTestGUI.VectorPanel();
            this.windowPanel = new ChimeraLib.Controls.WindowPanel();
            this.intersectionZLabel = new System.Windows.Forms.Label();
            this.intersectionYLabel = new System.Windows.Forms.Label();
            this.intersectionXLabel = new System.Windows.Forms.Label();
            this.graphicBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // xLabel
            // 
            this.xLabel.AutoSize = true;
            this.xLabel.Location = new System.Drawing.Point(3, 358);
            this.xLabel.Name = "xLabel";
            this.xLabel.Size = new System.Drawing.Size(20, 13);
            this.xLabel.TabIndex = 5;
            this.xLabel.Text = "X: ";
            // 
            // yLabel
            // 
            this.yLabel.AutoSize = true;
            this.yLabel.Location = new System.Drawing.Point(75, 358);
            this.yLabel.Name = "yLabel";
            this.yLabel.Size = new System.Drawing.Size(20, 13);
            this.yLabel.TabIndex = 6;
            this.yLabel.Text = "Y: ";
            // 
            // initButton
            // 
            this.initButton.Location = new System.Drawing.Point(152, 349);
            this.initButton.Name = "initButton";
            this.initButton.Size = new System.Drawing.Size(227, 22);
            this.initButton.TabIndex = 9;
            this.initButton.Text = "Init";
            this.initButton.UseVisualStyleBackColor = true;
            this.initButton.Click += new System.EventHandler(this.initButton_Click);
            // 
            // topYLabel
            // 
            this.topYLabel.AutoSize = true;
            this.topYLabel.Location = new System.Drawing.Point(87, 413);
            this.topYLabel.Name = "topYLabel";
            this.topYLabel.Size = new System.Drawing.Size(42, 13);
            this.topYLabel.TabIndex = 11;
            this.topYLabel.Text = "Top Y: ";
            // 
            // topXLabel
            // 
            this.topXLabel.AutoSize = true;
            this.topXLabel.Location = new System.Drawing.Point(164, 413);
            this.topXLabel.Name = "topXLabel";
            this.topXLabel.Size = new System.Drawing.Size(42, 13);
            this.topXLabel.TabIndex = 10;
            this.topXLabel.Text = "Top X: ";
            // 
            // topZLabel
            // 
            this.topZLabel.AutoSize = true;
            this.topZLabel.Location = new System.Drawing.Point(3, 413);
            this.topZLabel.Name = "topZLabel";
            this.topZLabel.Size = new System.Drawing.Size(42, 13);
            this.topZLabel.TabIndex = 12;
            this.topZLabel.Text = "Top Z: ";
            // 
            // sideZLabel
            // 
            this.sideZLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sideZLabel.AutoSize = true;
            this.sideZLabel.Location = new System.Drawing.Point(775, 413);
            this.sideZLabel.Name = "sideZLabel";
            this.sideZLabel.Size = new System.Drawing.Size(44, 13);
            this.sideZLabel.TabIndex = 15;
            this.sideZLabel.Text = "Side Z: ";
            // 
            // sideYLabel
            // 
            this.sideYLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sideYLabel.AutoSize = true;
            this.sideYLabel.Location = new System.Drawing.Point(676, 413);
            this.sideYLabel.Name = "sideYLabel";
            this.sideYLabel.Size = new System.Drawing.Size(44, 13);
            this.sideYLabel.TabIndex = 14;
            this.sideYLabel.Text = "Side Y: ";
            // 
            // sideXLabel
            // 
            this.sideXLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sideXLabel.AutoSize = true;
            this.sideXLabel.Location = new System.Drawing.Point(575, 413);
            this.sideXLabel.Name = "sideXLabel";
            this.sideXLabel.Size = new System.Drawing.Size(44, 13);
            this.sideXLabel.TabIndex = 13;
            this.sideXLabel.Text = "Side X: ";
            // 
            // graphicBox
            // 
            this.graphicBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.graphicBox.Controls.Add(this.drawPanel);
            this.graphicBox.Location = new System.Drawing.Point(3, 432);
            this.graphicBox.Name = "graphicBox";
            this.graphicBox.Size = new System.Drawing.Size(819, 216);
            this.graphicBox.TabIndex = 16;
            this.graphicBox.TabStop = false;
            this.graphicBox.Text = "Diagram";
            // 
            // drawPanel
            // 
            this.drawPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.drawPanel.Location = new System.Drawing.Point(3, 16);
            this.drawPanel.Name = "drawPanel";
            this.drawPanel.Size = new System.Drawing.Size(813, 197);
            this.drawPanel.TabIndex = 0;
            this.drawPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.graphicBox_Paint);
            // 
            // planeNormalZLabel
            // 
            this.planeNormalZLabel.AutoSize = true;
            this.planeNormalZLabel.Location = new System.Drawing.Point(294, 397);
            this.planeNormalZLabel.Name = "planeNormalZLabel";
            this.planeNormalZLabel.Size = new System.Drawing.Size(56, 13);
            this.planeNormalZLabel.TabIndex = 22;
            this.planeNormalZLabel.Text = "Normal Z: ";
            // 
            // planeNormalYLabel
            // 
            this.planeNormalYLabel.AutoSize = true;
            this.planeNormalYLabel.Location = new System.Drawing.Point(294, 384);
            this.planeNormalYLabel.Name = "planeNormalYLabel";
            this.planeNormalYLabel.Size = new System.Drawing.Size(56, 13);
            this.planeNormalYLabel.TabIndex = 21;
            this.planeNormalYLabel.Text = "Normal Y: ";
            // 
            // planeNormalXLabel
            // 
            this.planeNormalXLabel.AutoSize = true;
            this.planeNormalXLabel.Location = new System.Drawing.Point(294, 371);
            this.planeNormalXLabel.Name = "planeNormalXLabel";
            this.planeNormalXLabel.Size = new System.Drawing.Size(56, 13);
            this.planeNormalXLabel.TabIndex = 20;
            this.planeNormalXLabel.Text = "Normal X: ";
            // 
            // topLeftZLabel
            // 
            this.topLeftZLabel.AutoSize = true;
            this.topLeftZLabel.Location = new System.Drawing.Point(164, 397);
            this.topLeftZLabel.Name = "topLeftZLabel";
            this.topLeftZLabel.Size = new System.Drawing.Size(60, 13);
            this.topLeftZLabel.TabIndex = 19;
            this.topLeftZLabel.Text = "TopLeft Z: ";
            // 
            // topLeftYLabel
            // 
            this.topLeftYLabel.AutoSize = true;
            this.topLeftYLabel.Location = new System.Drawing.Point(164, 384);
            this.topLeftYLabel.Name = "topLeftYLabel";
            this.topLeftYLabel.Size = new System.Drawing.Size(60, 13);
            this.topLeftYLabel.TabIndex = 18;
            this.topLeftYLabel.Text = "TopLeft Y: ";
            // 
            // topLeftXLabel
            // 
            this.topLeftXLabel.AutoSize = true;
            this.topLeftXLabel.Location = new System.Drawing.Point(164, 371);
            this.topLeftXLabel.Name = "topLeftXLabel";
            this.topLeftXLabel.Size = new System.Drawing.Size(60, 13);
            this.topLeftXLabel.TabIndex = 17;
            this.topLeftXLabel.Text = "TopLeft X: ";
            // 
            // pointDirPanel
            // 
            this.pointDirPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pointDirPanel.DisplayName = "Point Direction";
            this.pointDirPanel.Location = new System.Drawing.Point(385, 279);
            this.pointDirPanel.LookAtVector = ((OpenMetaverse.Vector3)(resources.GetObject("pointDirPanel.LookAtVector")));
            this.pointDirPanel.Name = "pointDirPanel";
            this.pointDirPanel.Pitch = 90F;
            this.pointDirPanel.Rotation = ((OpenMetaverse.Quaternion)(resources.GetObject("pointDirPanel.Rotation")));
            this.pointDirPanel.Size = new System.Drawing.Size(437, 147);
            this.pointDirPanel.TabIndex = 7;
            this.pointDirPanel.Yaw = 0F;
            // 
            // pointStartPanel
            // 
            this.pointStartPanel.DisplayName = "Point Start";
            this.pointStartPanel.Location = new System.Drawing.Point(3, 257);
            this.pointStartPanel.Max = 1024D;
            this.pointStartPanel.Min = -1024D;
            this.pointStartPanel.Name = "pointStartPanel";
            this.pointStartPanel.Size = new System.Drawing.Size(376, 98);
            this.pointStartPanel.TabIndex = 3;
            this.pointStartPanel.Value = ((OpenMetaverse.Vector3)(resources.GetObject("pointStartPanel.Value")));
            this.pointStartPanel.X = 0F;
            this.pointStartPanel.Y = 0F;
            this.pointStartPanel.Z = 10F;
            // 
            // kinectRotationPanel
            // 
            this.kinectRotationPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.kinectRotationPanel.DisplayName = "KinectRotation";
            this.kinectRotationPanel.Location = new System.Drawing.Point(0, 104);
            this.kinectRotationPanel.LookAtVector = ((OpenMetaverse.Vector3)(resources.GetObject("kinectRotationPanel.LookAtVector")));
            this.kinectRotationPanel.Name = "kinectRotationPanel";
            this.kinectRotationPanel.Pitch = 0F;
            this.kinectRotationPanel.Rotation = ((OpenMetaverse.Quaternion)(resources.GetObject("kinectRotationPanel.Rotation")));
            this.kinectRotationPanel.Size = new System.Drawing.Size(179, 147);
            this.kinectRotationPanel.TabIndex = 2;
            this.kinectRotationPanel.Yaw = 180F;
            // 
            // kinectPositionPanel
            // 
            this.kinectPositionPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.kinectPositionPanel.DisplayName = "Kinect Position";
            this.kinectPositionPanel.Location = new System.Drawing.Point(0, 0);
            this.kinectPositionPanel.Max = 1024D;
            this.kinectPositionPanel.Min = -1024D;
            this.kinectPositionPanel.Name = "kinectPositionPanel";
            this.kinectPositionPanel.Size = new System.Drawing.Size(179, 98);
            this.kinectPositionPanel.TabIndex = 1;
            this.kinectPositionPanel.Value = ((OpenMetaverse.Vector3)(resources.GetObject("kinectPositionPanel.Value")));
            this.kinectPositionPanel.X = 0F;
            this.kinectPositionPanel.Y = 0F;
            this.kinectPositionPanel.Z = 0F;
            // 
            // windowPanel
            // 
            this.windowPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.windowPanel.Location = new System.Drawing.Point(185, 3);
            this.windowPanel.MinimumSize = new System.Drawing.Size(637, 270);
            this.windowPanel.Name = "windowPanel";
            this.windowPanel.Size = new System.Drawing.Size(637, 270);
            this.windowPanel.TabIndex = 0;
            window4.AspectRatio = 0.48051948051948051D;
            window4.Diagonal = 427.14166268347088D;
            window4.EyePosition = ((OpenMetaverse.Vector3)(resources.GetObject("window4.EyePosition")));
            window4.FieldOfView = 1.5707963267948966D;
            window4.Height = 185.00000000000003D;
            window4.LockScreenPosition = true;
            rotation4.LookAtVector = ((OpenMetaverse.Vector3)(resources.GetObject("rotation4.LookAtVector")));
            rotation4.Pitch = 0F;
            rotation4.Quaternion = ((OpenMetaverse.Quaternion)(resources.GetObject("rotation4.Quaternion")));
            rotation4.Yaw = 0F;
            window4.RotationOffset = rotation4;
            window4.ScreenPosition = ((OpenMetaverse.Vector3)(resources.GetObject("window4.ScreenPosition")));
            window4.Width = 385.00000000000006D;
            this.windowPanel.Window = window4;
            // 
            // intersectionZLabel
            // 
            this.intersectionZLabel.AutoSize = true;
            this.intersectionZLabel.Location = new System.Drawing.Point(3, 397);
            this.intersectionZLabel.Name = "intersectionZLabel";
            this.intersectionZLabel.Size = new System.Drawing.Size(78, 13);
            this.intersectionZLabel.TabIndex = 25;
            this.intersectionZLabel.Text = "Intersection Z: ";
            // 
            // intersectionYLabel
            // 
            this.intersectionYLabel.AutoSize = true;
            this.intersectionYLabel.Location = new System.Drawing.Point(3, 384);
            this.intersectionYLabel.Name = "intersectionYLabel";
            this.intersectionYLabel.Size = new System.Drawing.Size(78, 13);
            this.intersectionYLabel.TabIndex = 24;
            this.intersectionYLabel.Text = "Intersection Y: ";
            // 
            // intersectionXLabel
            // 
            this.intersectionXLabel.AutoSize = true;
            this.intersectionXLabel.Location = new System.Drawing.Point(3, 371);
            this.intersectionXLabel.Name = "intersectionXLabel";
            this.intersectionXLabel.Size = new System.Drawing.Size(78, 13);
            this.intersectionXLabel.TabIndex = 23;
            this.intersectionXLabel.Text = "Intersection X: ";
            // 
            // PointPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.intersectionZLabel);
            this.Controls.Add(this.intersectionYLabel);
            this.Controls.Add(this.intersectionXLabel);
            this.Controls.Add(this.planeNormalZLabel);
            this.Controls.Add(this.planeNormalYLabel);
            this.Controls.Add(this.planeNormalXLabel);
            this.Controls.Add(this.topLeftZLabel);
            this.Controls.Add(this.topLeftYLabel);
            this.Controls.Add(this.topLeftXLabel);
            this.Controls.Add(this.graphicBox);
            this.Controls.Add(this.sideZLabel);
            this.Controls.Add(this.sideYLabel);
            this.Controls.Add(this.sideXLabel);
            this.Controls.Add(this.topZLabel);
            this.Controls.Add(this.topYLabel);
            this.Controls.Add(this.topXLabel);
            this.Controls.Add(this.initButton);
            this.Controls.Add(this.pointDirPanel);
            this.Controls.Add(this.yLabel);
            this.Controls.Add(this.xLabel);
            this.Controls.Add(this.kinectRotationPanel);
            this.Controls.Add(this.kinectPositionPanel);
            this.Controls.Add(this.windowPanel);
            this.Controls.Add(this.pointStartPanel);
            this.Name = "PointPanel";
            this.Size = new System.Drawing.Size(825, 648);
            this.graphicBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ChimeraLib.Controls.WindowPanel windowPanel;
        private ProxyTestGUI.VectorPanel kinectPositionPanel;
        private ProxyTestGUI.RotationPanel kinectRotationPanel;
        private ProxyTestGUI.VectorPanel pointStartPanel;
        private System.Windows.Forms.Label xLabel;
        private System.Windows.Forms.Label yLabel;
        private ProxyTestGUI.RotationPanel pointDirPanel;
        private System.Windows.Forms.Button initButton;
        private System.Windows.Forms.Label topYLabel;
        private System.Windows.Forms.Label topXLabel;
        private System.Windows.Forms.Label topZLabel;
        private System.Windows.Forms.Label sideZLabel;
        private System.Windows.Forms.Label sideYLabel;
        private System.Windows.Forms.Label sideXLabel;
        private System.Windows.Forms.GroupBox graphicBox;
        private System.Windows.Forms.Panel drawPanel;
        private System.Windows.Forms.Label planeNormalZLabel;
        private System.Windows.Forms.Label planeNormalYLabel;
        private System.Windows.Forms.Label planeNormalXLabel;
        private System.Windows.Forms.Label topLeftZLabel;
        private System.Windows.Forms.Label topLeftYLabel;
        private System.Windows.Forms.Label topLeftXLabel;
        private System.Windows.Forms.Label intersectionZLabel;
        private System.Windows.Forms.Label intersectionYLabel;
        private System.Windows.Forms.Label intersectionXLabel;
    }
}

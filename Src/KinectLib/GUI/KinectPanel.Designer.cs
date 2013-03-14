namespace Chimera.Kinect.GUI {
    partial class KinectPanel {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KinectPanel));
            Chimera.Util.Rotation rotation1 = new Chimera.Util.Rotation();
            this.mainTab = new System.Windows.Forms.TabControl();
            this.controlTab = new System.Windows.Forms.TabPage();
            this.startButton = new System.Windows.Forms.Button();
            this.pointDirPanel = new KinectLib.GUI.UpdatedVectorPanel();
            this.orientationPanel = new ProxyTestGUI.RotationPanel();
            this.positionPanel = new ProxyTestGUI.VectorPanel();
            this.pointStartPanel = new KinectLib.GUI.UpdatedVectorPanel();
            this.movementTab = new System.Windows.Forms.TabPage();
            this.mainTab.SuspendLayout();
            this.controlTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainTab
            // 
            this.mainTab.Controls.Add(this.controlTab);
            this.mainTab.Controls.Add(this.movementTab);
            this.mainTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTab.Location = new System.Drawing.Point(0, 0);
            this.mainTab.Name = "mainTab";
            this.mainTab.SelectedIndex = 0;
            this.mainTab.Size = new System.Drawing.Size(709, 520);
            this.mainTab.TabIndex = 0;
            // 
            // controlTab
            // 
            this.controlTab.AutoScroll = true;
            this.controlTab.Controls.Add(this.startButton);
            this.controlTab.Controls.Add(this.pointDirPanel);
            this.controlTab.Controls.Add(this.orientationPanel);
            this.controlTab.Controls.Add(this.positionPanel);
            this.controlTab.Controls.Add(this.pointStartPanel);
            this.controlTab.Location = new System.Drawing.Point(4, 22);
            this.controlTab.Name = "controlTab";
            this.controlTab.Padding = new System.Windows.Forms.Padding(3);
            this.controlTab.Size = new System.Drawing.Size(701, 494);
            this.controlTab.TabIndex = 0;
            this.controlTab.Text = "Control";
            this.controlTab.UseVisualStyleBackColor = true;
            // 
            // startButton
            // 
            this.startButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.startButton.Location = new System.Drawing.Point(0, 0);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(705, 23);
            this.startButton.TabIndex = 4;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // pointDirPanel
            // 
            this.pointDirPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pointDirPanel.DisplayName = "Point Direction";
            this.pointDirPanel.Location = new System.Drawing.Point(0, 332);
            this.pointDirPanel.Max = 1024D;
            this.pointDirPanel.MaxV = ((OpenMetaverse.Vector3)(resources.GetObject("pointDirPanel.MaxV")));
            this.pointDirPanel.Min = -1024D;
            this.pointDirPanel.MinimumSize = new System.Drawing.Size(103, 95);
            this.pointDirPanel.MinV = ((OpenMetaverse.Vector3)(resources.GetObject("pointDirPanel.MinV")));
            this.pointDirPanel.Name = "pointDirPanel";
            this.pointDirPanel.Size = new System.Drawing.Size(701, 95);
            this.pointDirPanel.TabIndex = 3;
            this.pointDirPanel.Value = ((OpenMetaverse.Vector3)(resources.GetObject("pointDirPanel.Value")));
            this.pointDirPanel.Vector = null;
            this.pointDirPanel.X = 0F;
            this.pointDirPanel.Y = 0F;
            this.pointDirPanel.Z = 0F;
            // 
            // orientationPanel
            // 
            this.orientationPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.orientationPanel.DisplayName = "Orientation";
            this.orientationPanel.Location = new System.Drawing.Point(0, 130);
            this.orientationPanel.LookAtVector = ((OpenMetaverse.Vector3)(resources.GetObject("orientationPanel.LookAtVector")));
            this.orientationPanel.MinimumSize = new System.Drawing.Size(252, 95);
            this.orientationPanel.Name = "orientationPanel";
            this.orientationPanel.Pitch = 0D;
            this.orientationPanel.Quaternion = ((OpenMetaverse.Quaternion)(resources.GetObject("orientationPanel.Quaternion")));
            this.orientationPanel.Size = new System.Drawing.Size(701, 95);
            this.orientationPanel.TabIndex = 2;
            rotation1.LookAtVector = ((OpenMetaverse.Vector3)(resources.GetObject("rotation1.LookAtVector")));
            rotation1.Pitch = 0D;
            rotation1.Quaternion = ((OpenMetaverse.Quaternion)(resources.GetObject("rotation1.Quaternion")));
            rotation1.Yaw = 0D;
            this.orientationPanel.Value = rotation1;
            this.orientationPanel.Yaw = 0D;
            // 
            // positionPanel
            // 
            this.positionPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.positionPanel.DisplayName = "Position (cm)";
            this.positionPanel.Location = new System.Drawing.Point(0, 29);
            this.positionPanel.Max = 1024D;
            this.positionPanel.MaxV = ((OpenMetaverse.Vector3)(resources.GetObject("positionPanel.MaxV")));
            this.positionPanel.Min = -1024D;
            this.positionPanel.MinimumSize = new System.Drawing.Size(103, 95);
            this.positionPanel.MinV = ((OpenMetaverse.Vector3)(resources.GetObject("positionPanel.MinV")));
            this.positionPanel.Name = "positionPanel";
            this.positionPanel.Size = new System.Drawing.Size(701, 95);
            this.positionPanel.TabIndex = 1;
            this.positionPanel.Value = ((OpenMetaverse.Vector3)(resources.GetObject("positionPanel.Value")));
            this.positionPanel.X = 0F;
            this.positionPanel.Y = 0F;
            this.positionPanel.Z = 0F;
            this.positionPanel.ValueChanged += new System.EventHandler(this.positionPanel_ValueChanged);
            // 
            // pointStartPanel
            // 
            this.pointStartPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pointStartPanel.DisplayName = "Point Start";
            this.pointStartPanel.Location = new System.Drawing.Point(0, 231);
            this.pointStartPanel.Max = 1024D;
            this.pointStartPanel.MaxV = ((OpenMetaverse.Vector3)(resources.GetObject("pointStartPanel.MaxV")));
            this.pointStartPanel.Min = -1024D;
            this.pointStartPanel.MinimumSize = new System.Drawing.Size(103, 95);
            this.pointStartPanel.MinV = ((OpenMetaverse.Vector3)(resources.GetObject("pointStartPanel.MinV")));
            this.pointStartPanel.Name = "pointStartPanel";
            this.pointStartPanel.Size = new System.Drawing.Size(701, 95);
            this.pointStartPanel.TabIndex = 0;
            this.pointStartPanel.Value = ((OpenMetaverse.Vector3)(resources.GetObject("pointStartPanel.Value")));
            this.pointStartPanel.Vector = null;
            this.pointStartPanel.X = 0F;
            this.pointStartPanel.Y = 0F;
            this.pointStartPanel.Z = 0F;
            // 
            // movementTab
            // 
            this.movementTab.Location = new System.Drawing.Point(4, 22);
            this.movementTab.Name = "movementTab";
            this.movementTab.Padding = new System.Windows.Forms.Padding(3);
            this.movementTab.Size = new System.Drawing.Size(701, 494);
            this.movementTab.TabIndex = 1;
            this.movementTab.Text = "Movement";
            this.movementTab.UseVisualStyleBackColor = true;
            // 
            // KinectPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainTab);
            this.Name = "KinectPanel";
            this.Size = new System.Drawing.Size(709, 520);
            this.mainTab.ResumeLayout(false);
            this.controlTab.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl mainTab;
        private System.Windows.Forms.TabPage controlTab;
        private System.Windows.Forms.TabPage movementTab;
        private KinectLib.GUI.UpdatedVectorPanel pointDirPanel;
        private ProxyTestGUI.RotationPanel orientationPanel;
        private ProxyTestGUI.VectorPanel positionPanel;
        private KinectLib.GUI.UpdatedVectorPanel pointStartPanel;
        private System.Windows.Forms.Button startButton;
    }
}

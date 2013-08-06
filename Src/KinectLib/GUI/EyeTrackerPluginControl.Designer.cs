namespace Chimera.Kinect.GUI {
    partial class EyeTrackerPluginControl {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EyeTrackerPluginControl));
            Chimera.Util.Rotation rotation1 = new Chimera.Util.Rotation();
            this.positionPanel = new Chimera.GUI.VectorPanel();
            this.orientationPanel = new Chimera.GUI.RotationPanel();
            this.SuspendLayout();
            // 
            // positionPanel
            // 
            this.positionPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.positionPanel.Location = new System.Drawing.Point(3, 0);
            this.positionPanel.Max = 5000F;
            this.positionPanel.MaxV = ((OpenMetaverse.Vector3)(resources.GetObject("positionPanel.MaxV")));
            this.positionPanel.Min = -5000F;
            this.positionPanel.MinimumSize = new System.Drawing.Size(103, 95);
            this.positionPanel.MinV = ((OpenMetaverse.Vector3)(resources.GetObject("positionPanel.MinV")));
            this.positionPanel.Name = "positionPanel";
            this.positionPanel.Size = new System.Drawing.Size(354, 95);
            this.positionPanel.TabIndex = 0;
            this.positionPanel.Value = ((OpenMetaverse.Vector3)(resources.GetObject("positionPanel.Value")));
            this.positionPanel.X = 0F;
            this.positionPanel.Y = 0F;
            this.positionPanel.Z = 0F;
            this.positionPanel.ValueChanged += new System.EventHandler(this.positionPanel_ValueChanged);
            // 
            // orientationPanel
            // 
            this.orientationPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.orientationPanel.Location = new System.Drawing.Point(3, 101);
            this.orientationPanel.LookAtVector = ((OpenMetaverse.Vector3)(resources.GetObject("orientationPanel.LookAtVector")));
            this.orientationPanel.MinimumSize = new System.Drawing.Size(252, 95);
            this.orientationPanel.Name = "orientationPanel";
            this.orientationPanel.Pitch = 0D;
            this.orientationPanel.Quaternion = ((OpenMetaverse.Quaternion)(resources.GetObject("orientationPanel.Quaternion")));
            this.orientationPanel.Size = new System.Drawing.Size(351, 95);
            this.orientationPanel.TabIndex = 1;
            rotation1.LookAtVector = ((OpenMetaverse.Vector3)(resources.GetObject("rotation1.LookAtVector")));
            rotation1.Pitch = 0D;
            rotation1.Quaternion = ((OpenMetaverse.Quaternion)(resources.GetObject("rotation1.Quaternion")));
            rotation1.Yaw = 0D;
            this.orientationPanel.Value = rotation1;
            this.orientationPanel.Yaw = 0D;
            // 
            // EyeTrackerPluginControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.orientationPanel);
            this.Controls.Add(this.positionPanel);
            this.Name = "EyeTrackerPluginControl";
            this.Size = new System.Drawing.Size(357, 268);
            this.ResumeLayout(false);

        }

        #endregion

        private Chimera.GUI.VectorPanel positionPanel;
        private Chimera.GUI.RotationPanel orientationPanel;
    }
}

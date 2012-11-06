namespace ConsoleTest {
    partial class SlaveForm {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SlaveForm));
            this.rotationOffsetPanel = new ProxyTestGUI.RotationPanel();
            this.positionOffsetPanel = new ProxyTestGUI.VectorPanel();
            this.SuspendLayout();
            // 
            // rotationOffsetPanel
            // 
            this.rotationOffsetPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rotationOffsetPanel.DisplayName = "MasterRotation Offset";
            this.rotationOffsetPanel.Location = new System.Drawing.Point(12, 12);
            this.rotationOffsetPanel.Name = "rotationOffsetPanel";
            this.rotationOffsetPanel.Pitch = 0F;
            this.rotationOffsetPanel.Rotation = ((OpenMetaverse.Quaternion)(resources.GetObject("rotationOffsetPanel.MasterRotation")));
            this.rotationOffsetPanel.Size = new System.Drawing.Size(517, 147);
            this.rotationOffsetPanel.TabIndex = 0;
            this.rotationOffsetPanel.Vector = ((OpenMetaverse.Vector3)(resources.GetObject("rotationOffsetPanel.Vector")));
            this.rotationOffsetPanel.Yaw = 0F;
            this.rotationOffsetPanel.OnChange += new System.EventHandler(this.rotationOffsetPanel_OnChange);
            // 
            // positionOffsetPanel
            // 
            this.positionOffsetPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.positionOffsetPanel.DisplayName = "PositionOffset Offset";
            this.positionOffsetPanel.Location = new System.Drawing.Point(12, 165);
            this.positionOffsetPanel.Max = 1024D;
            this.positionOffsetPanel.Min = -1024D;
            this.positionOffsetPanel.Name = "positionOffsetPanel";
            this.positionOffsetPanel.Size = new System.Drawing.Size(517, 98);
            this.positionOffsetPanel.TabIndex = 1;
            this.positionOffsetPanel.Value = ((OpenMetaverse.Vector3)(resources.GetObject("positionOffsetPanel.Value")));
            this.positionOffsetPanel.X = 0F;
            this.positionOffsetPanel.Y = 0F;
            this.positionOffsetPanel.Z = 0F;
            this.positionOffsetPanel.OnChange += new System.EventHandler(this.positionOffset_OnChange);
            // 
            // SlaveForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 322);
            this.Controls.Add(this.positionOffsetPanel);
            this.Controls.Add(this.rotationOffsetPanel);
            this.Name = "SlaveForm";
            this.Text = "SlaveForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SlaveForm_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private ProxyTestGUI.RotationPanel rotationOffsetPanel;
        private ProxyTestGUI.VectorPanel positionOffsetPanel;


    }
}
namespace ConsoleTest {
    partial class MasterForm {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MasterForm));
            this.mainTab = new System.Windows.Forms.TabControl();
            this.rawTab = new System.Windows.Forms.TabPage();
            this.rawPosition = new ProxyTestGUI.VectorPanel();
            this.rawRotation = new ProxyTestGUI.RotationPanel();
            this.mainTab.SuspendLayout();
            this.rawTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainTab
            // 
            this.mainTab.Controls.Add(this.rawTab);
            this.mainTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTab.Location = new System.Drawing.Point(0, 0);
            this.mainTab.Name = "mainTab";
            this.mainTab.SelectedIndex = 0;
            this.mainTab.Size = new System.Drawing.Size(617, 291);
            this.mainTab.TabIndex = 0;
            // 
            // rawTab
            // 
            this.rawTab.Controls.Add(this.rawPosition);
            this.rawTab.Controls.Add(this.rawRotation);
            this.rawTab.Location = new System.Drawing.Point(4, 22);
            this.rawTab.Name = "rawTab";
            this.rawTab.Padding = new System.Windows.Forms.Padding(3);
            this.rawTab.Size = new System.Drawing.Size(609, 265);
            this.rawTab.TabIndex = 0;
            this.rawTab.Text = "Input Values";
            this.rawTab.UseVisualStyleBackColor = true;
            // 
            // rawPosition
            // 
            this.rawPosition.DisplayName = "Raw Position";
            this.rawPosition.Location = new System.Drawing.Point(0, 153);
            this.rawPosition.Max = 1024D;
            this.rawPosition.Min = -1024D;
            this.rawPosition.Name = "rawPosition";
            this.rawPosition.Size = new System.Drawing.Size(609, 98);
            this.rawPosition.TabIndex = 1;
            this.rawPosition.Value = ((OpenMetaverse.Vector3)(resources.GetObject("rawPosition.Value")));
            this.rawPosition.X = 128F;
            this.rawPosition.Y = 128F;
            this.rawPosition.Z = 128F;
            this.rawPosition.OnChange += new System.EventHandler(this.rawPosition_OnChange);
            // 
            // rawRotation
            // 
            this.rawRotation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rawRotation.DisplayName = "Raw Rotation";
            this.rawRotation.Location = new System.Drawing.Point(0, 0);
            this.rawRotation.LookAtVector = ((OpenMetaverse.Vector3)(resources.GetObject("rawRotation.LookAtVector")));
            this.rawRotation.Name = "rawRotation";
            this.rawRotation.Pitch = 0F;
            this.rawRotation.Rotation = ((OpenMetaverse.Quaternion)(resources.GetObject("rawRotation.Rotation")));
            this.rawRotation.Size = new System.Drawing.Size(613, 147);
            this.rawRotation.TabIndex = 0;
            this.rawRotation.Yaw = 0F;
            this.rawRotation.OnChange += new System.EventHandler(this.rawRotation_OnChange);
            // 
            // MasterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(617, 291);
            this.Controls.Add(this.mainTab);
            this.Name = "MasterForm";
            this.Text = "MasterForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MasterForm_FormClosing);
            this.mainTab.ResumeLayout(false);
            this.rawTab.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl mainTab;
        private System.Windows.Forms.TabPage rawTab;
        private ProxyTestGUI.VectorPanel rawPosition;
        private ProxyTestGUI.RotationPanel rawRotation;
    }
}
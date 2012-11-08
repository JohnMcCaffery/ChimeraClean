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
            this.finalTab = new System.Windows.Forms.TabPage();
            this.finalRotation = new ProxyTestGUI.RotationPanel();
            this.finalPosition = new ProxyTestGUI.VectorPanel();
            this.rawTab = new System.Windows.Forms.TabPage();
            this.masterRotation = new ProxyTestGUI.RotationPanel();
            this.masterPosition = new ProxyTestGUI.VectorPanel();
            this.offsetTab = new System.Windows.Forms.TabPage();
            this.rotationOffsetPanel = new ProxyTestGUI.RotationPanel();
            this.positionOffsetPanel = new ProxyTestGUI.VectorPanel();
            this.mainTabContainer = new System.Windows.Forms.TabControl();
            this.proxyTab = new System.Windows.Forms.TabPage();
            this.proxyPanel = new UtilLib.ProxyPanel();
            this.finalTab.SuspendLayout();
            this.rawTab.SuspendLayout();
            this.offsetTab.SuspendLayout();
            this.mainTabContainer.SuspendLayout();
            this.proxyTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // finalTab
            // 
            this.finalTab.Controls.Add(this.finalRotation);
            this.finalTab.Controls.Add(this.finalPosition);
            this.finalTab.Location = new System.Drawing.Point(4, 22);
            this.finalTab.Name = "finalTab";
            this.finalTab.Padding = new System.Windows.Forms.Padding(3);
            this.finalTab.Size = new System.Drawing.Size(632, 279);
            this.finalTab.TabIndex = 2;
            this.finalTab.Text = "Final Values";
            this.finalTab.UseVisualStyleBackColor = true;
            // 
            // finalRotation
            // 
            this.finalRotation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.finalRotation.DisplayName = "Final Rotation";
            this.finalRotation.Enabled = false;
            this.finalRotation.Location = new System.Drawing.Point(0, 0);
            this.finalRotation.LookAtVector = ((OpenMetaverse.Vector3)(resources.GetObject("finalRotation.LookAtVector")));
            this.finalRotation.Name = "finalRotation";
            this.finalRotation.Pitch = 0F;
            this.finalRotation.Rotation = ((OpenMetaverse.Quaternion)(resources.GetObject("finalRotation.Rotation")));
            this.finalRotation.Size = new System.Drawing.Size(632, 147);
            this.finalRotation.TabIndex = 2;
            this.finalRotation.Yaw = 0F;
            // 
            // finalPosition
            // 
            this.finalPosition.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.finalPosition.DisplayName = "Final Position";
            this.finalPosition.Enabled = false;
            this.finalPosition.Location = new System.Drawing.Point(0, 153);
            this.finalPosition.Max = 1024D;
            this.finalPosition.Min = -1024D;
            this.finalPosition.Name = "finalPosition";
            this.finalPosition.Size = new System.Drawing.Size(632, 98);
            this.finalPosition.TabIndex = 3;
            this.finalPosition.Value = ((OpenMetaverse.Vector3)(resources.GetObject("finalPosition.Value")));
            this.finalPosition.X = 0F;
            this.finalPosition.Y = 0F;
            this.finalPosition.Z = 0F;
            // 
            // rawTab
            // 
            this.rawTab.Controls.Add(this.masterRotation);
            this.rawTab.Controls.Add(this.masterPosition);
            this.rawTab.Location = new System.Drawing.Point(4, 22);
            this.rawTab.Name = "rawTab";
            this.rawTab.Padding = new System.Windows.Forms.Padding(3);
            this.rawTab.Size = new System.Drawing.Size(632, 279);
            this.rawTab.TabIndex = 1;
            this.rawTab.Text = "Master Values";
            this.rawTab.UseVisualStyleBackColor = true;
            // 
            // masterRotation
            // 
            this.masterRotation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.masterRotation.DisplayName = "Master Rotation";
            this.masterRotation.Location = new System.Drawing.Point(0, 0);
            this.masterRotation.LookAtVector = ((OpenMetaverse.Vector3)(resources.GetObject("masterRotation.LookAtVector")));
            this.masterRotation.Name = "masterRotation";
            this.masterRotation.Pitch = 0F;
            this.masterRotation.Rotation = ((OpenMetaverse.Quaternion)(resources.GetObject("masterRotation.Rotation")));
            this.masterRotation.Size = new System.Drawing.Size(632, 147);
            this.masterRotation.TabIndex = 2;
            this.masterRotation.Yaw = 0F;
            this.masterRotation.OnChange += new System.EventHandler(this.rawRotation_OnChange);
            // 
            // masterPosition
            // 
            this.masterPosition.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.masterPosition.DisplayName = "Master Position";
            this.masterPosition.Location = new System.Drawing.Point(0, 153);
            this.masterPosition.Max = 1024D;
            this.masterPosition.Min = -1024D;
            this.masterPosition.Name = "masterPosition";
            this.masterPosition.Size = new System.Drawing.Size(632, 98);
            this.masterPosition.TabIndex = 3;
            this.masterPosition.Value = ((OpenMetaverse.Vector3)(resources.GetObject("masterPosition.Value")));
            this.masterPosition.X = 0F;
            this.masterPosition.Y = 0F;
            this.masterPosition.Z = 0F;
            this.masterPosition.OnChange += new System.EventHandler(this.rawPosition_OnChange);
            // 
            // offsetTab
            // 
            this.offsetTab.Controls.Add(this.rotationOffsetPanel);
            this.offsetTab.Controls.Add(this.positionOffsetPanel);
            this.offsetTab.Location = new System.Drawing.Point(4, 22);
            this.offsetTab.Name = "offsetTab";
            this.offsetTab.Padding = new System.Windows.Forms.Padding(3);
            this.offsetTab.Size = new System.Drawing.Size(632, 279);
            this.offsetTab.TabIndex = 0;
            this.offsetTab.Text = "Offsets";
            this.offsetTab.UseVisualStyleBackColor = true;
            // 
            // rotationOffsetPanel
            // 
            this.rotationOffsetPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rotationOffsetPanel.DisplayName = "Rotation Offset";
            this.rotationOffsetPanel.Location = new System.Drawing.Point(0, 0);
            this.rotationOffsetPanel.LookAtVector = ((OpenMetaverse.Vector3)(resources.GetObject("rotationOffsetPanel.LookAtVector")));
            this.rotationOffsetPanel.Name = "rotationOffsetPanel";
            this.rotationOffsetPanel.Pitch = 0F;
            this.rotationOffsetPanel.Rotation = ((OpenMetaverse.Quaternion)(resources.GetObject("rotationOffsetPanel.Rotation")));
            this.rotationOffsetPanel.Size = new System.Drawing.Size(632, 147);
            this.rotationOffsetPanel.TabIndex = 0;
            this.rotationOffsetPanel.Yaw = 0F;
            this.rotationOffsetPanel.OnChange += new System.EventHandler(this.rotationOffsetPanel_OnChange);
            // 
            // positionOffsetPanel
            // 
            this.positionOffsetPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.positionOffsetPanel.DisplayName = "Position Offset";
            this.positionOffsetPanel.Location = new System.Drawing.Point(0, 153);
            this.positionOffsetPanel.Max = 1024D;
            this.positionOffsetPanel.Min = -1024D;
            this.positionOffsetPanel.Name = "positionOffsetPanel";
            this.positionOffsetPanel.Size = new System.Drawing.Size(632, 98);
            this.positionOffsetPanel.TabIndex = 1;
            this.positionOffsetPanel.Value = ((OpenMetaverse.Vector3)(resources.GetObject("positionOffsetPanel.Value")));
            this.positionOffsetPanel.X = 0F;
            this.positionOffsetPanel.Y = 0F;
            this.positionOffsetPanel.Z = 0F;
            this.positionOffsetPanel.OnChange += new System.EventHandler(this.positionOffset_OnChange);
            // 
            // mainTabContainer
            // 
            this.mainTabContainer.Controls.Add(this.offsetTab);
            this.mainTabContainer.Controls.Add(this.rawTab);
            this.mainTabContainer.Controls.Add(this.finalTab);
            this.mainTabContainer.Controls.Add(this.proxyTab);
            this.mainTabContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTabContainer.Location = new System.Drawing.Point(0, 0);
            this.mainTabContainer.Name = "mainTabContainer";
            this.mainTabContainer.SelectedIndex = 0;
            this.mainTabContainer.Size = new System.Drawing.Size(640, 305);
            this.mainTabContainer.TabIndex = 2;
            // 
            // proxyTab
            // 
            this.proxyTab.Controls.Add(this.proxyPanel);
            this.proxyTab.Location = new System.Drawing.Point(4, 22);
            this.proxyTab.Name = "proxyTab";
            this.proxyTab.Padding = new System.Windows.Forms.Padding(3);
            this.proxyTab.Size = new System.Drawing.Size(632, 279);
            this.proxyTab.TabIndex = 3;
            this.proxyTab.Text = "Proxy";
            this.proxyTab.UseVisualStyleBackColor = true;
            // 
            // proxyPanel
            // 
            this.proxyPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.proxyPanel.FirstName = "Routing";
            this.proxyPanel.LastName = "God";
            this.proxyPanel.Location = new System.Drawing.Point(3, 3);
            this.proxyPanel.LoginURI = "http://apollo.cs.st-andrews.ac.uk:8002";
            this.proxyPanel.Name = "proxyPanel1";
            this.proxyPanel.Password = "1245";
            this.proxyPanel.Port = "8080";
            this.proxyPanel.Proxy = null;
            this.proxyPanel.Size = new System.Drawing.Size(626, 273);
            this.proxyPanel.TabIndex = 0;
            // 
            // SlaveForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 305);
            this.Controls.Add(this.mainTabContainer);
            this.Name = "SlaveForm";
            this.Text = "SlaveForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SlaveForm_FormClosing);
            this.finalTab.ResumeLayout(false);
            this.rawTab.ResumeLayout(false);
            this.offsetTab.ResumeLayout(false);
            this.mainTabContainer.ResumeLayout(false);
            this.proxyTab.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage finalTab;
        private System.Windows.Forms.TabPage rawTab;
        private System.Windows.Forms.TabPage offsetTab;
        private ProxyTestGUI.RotationPanel rotationOffsetPanel;
        private ProxyTestGUI.VectorPanel positionOffsetPanel;
        private System.Windows.Forms.TabControl mainTabContainer;
        private ProxyTestGUI.RotationPanel finalRotation;
        private ProxyTestGUI.VectorPanel finalPosition;
        private ProxyTestGUI.RotationPanel masterRotation;
        private ProxyTestGUI.VectorPanel masterPosition;
        private System.Windows.Forms.TabPage proxyTab;
        private UtilLib.ProxyPanel proxyPanel;



    }
}
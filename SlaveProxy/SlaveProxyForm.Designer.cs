namespace SlaveProxy {
    partial class SlaveProxyForm {
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SlaveProxyForm));
            this.nameBox = new System.Windows.Forms.TextBox();
            this.masterURIBox = new System.Windows.Forms.TextBox();
            this.connectButton = new System.Windows.Forms.Button();
            this.checkTimer = new System.Windows.Forms.Timer(this.components);
            this.countLabel = new System.Windows.Forms.Label();
            this.listenIPBox = new System.Windows.Forms.TextBox();
            this.listenIPLabel = new System.Windows.Forms.Label();
            this.portBox = new System.Windows.Forms.TextBox();
            this.portLabel = new System.Windows.Forms.Label();
            this.masterLabel = new System.Windows.Forms.Label();
            this.nameLabel = new System.Windows.Forms.Label();
            this.xmlRpcPortBox = new System.Windows.Forms.TextBox();
            this.xmlRpcPortLabel = new System.Windows.Forms.Label();
            this.packetsToProcessLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.unprocessedPacketsLabel = new System.Windows.Forms.Label();
            this.tabContainer = new System.Windows.Forms.TabControl();
            this.networkTab = new System.Windows.Forms.TabPage();
            this.cameraTab = new System.Windows.Forms.TabPage();
            this.setFollowCamPropertiesPanel = new UtilLib.SetFollowCamPropertiesPanel();
            this.avatarsTab = new System.Windows.Forms.TabPage();
            this.avatarList = new System.Windows.Forms.ListBox();
            this.packetTab = new System.Windows.Forms.TabPage();
            this.velocityPanel = new ProxyTestGUI.VectorPanel();
            this.rotationalVelocityPanel = new ProxyTestGUI.VectorPanel();
            this.accelerationPanel = new ProxyTestGUI.VectorPanel();
            this.positionPanel = new ProxyTestGUI.VectorPanel();
            this.rotationPanel = new ProxyTestGUI.RotationPanel();
            this.updateTimer = new System.Windows.Forms.Timer(this.components);
            this.proxyPanel = new UtilLib.ProxyPanel();
            this.tabContainer.SuspendLayout();
            this.networkTab.SuspendLayout();
            this.cameraTab.SuspendLayout();
            this.avatarsTab.SuspendLayout();
            this.packetTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // nameBox
            // 
            this.nameBox.Location = new System.Drawing.Point(130, 8);
            this.nameBox.Name = "nameBox";
            this.nameBox.Size = new System.Drawing.Size(45, 20);
            this.nameBox.TabIndex = 0;
            this.nameBox.Text = "Slave 1";
            // 
            // masterURIBox
            // 
            this.masterURIBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.masterURIBox.Location = new System.Drawing.Point(130, 34);
            this.masterURIBox.Name = "masterURIBox";
            this.masterURIBox.Size = new System.Drawing.Size(800, 20);
            this.masterURIBox.TabIndex = 2;
            this.masterURIBox.Text = "http://localhost:5678/Master.rem";
            // 
            // connectButton
            // 
            this.connectButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.connectButton.Location = new System.Drawing.Point(181, 6);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(749, 23);
            this.connectButton.TabIndex = 3;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // checkTimer
            // 
            this.checkTimer.Enabled = true;
            this.checkTimer.Tick += new System.EventHandler(this.checkTimer_Tick);
            // 
            // countLabel
            // 
            this.countLabel.AutoSize = true;
            this.countLabel.Location = new System.Drawing.Point(127, 109);
            this.countLabel.Name = "countLabel";
            this.countLabel.Size = new System.Drawing.Size(13, 13);
            this.countLabel.TabIndex = 4;
            this.countLabel.Text = "0";
            // 
            // listenIPBox
            // 
            this.listenIPBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listenIPBox.Location = new System.Drawing.Point(130, 86);
            this.listenIPBox.Name = "listenIPBox";
            this.listenIPBox.Size = new System.Drawing.Size(800, 20);
            this.listenIPBox.TabIndex = 9;
            this.listenIPBox.Text = "127.0.0.1";
            // 
            // listenIPLabel
            // 
            this.listenIPLabel.AutoSize = true;
            this.listenIPLabel.Location = new System.Drawing.Point(76, 89);
            this.listenIPLabel.Name = "listenIPLabel";
            this.listenIPLabel.Size = new System.Drawing.Size(48, 13);
            this.listenIPLabel.TabIndex = 8;
            this.listenIPLabel.Text = "Listen IP";
            // 
            // portBox
            // 
            this.portBox.Location = new System.Drawing.Point(130, 60);
            this.portBox.Name = "portBox";
            this.portBox.Size = new System.Drawing.Size(41, 20);
            this.portBox.TabIndex = 7;
            this.portBox.Text = "8090";
            // 
            // portLabel
            // 
            this.portLabel.AutoSize = true;
            this.portLabel.Location = new System.Drawing.Point(98, 63);
            this.portLabel.Name = "portLabel";
            this.portLabel.Size = new System.Drawing.Size(26, 13);
            this.portLabel.TabIndex = 6;
            this.portLabel.Text = "Port";
            // 
            // masterLabel
            // 
            this.masterLabel.AutoSize = true;
            this.masterLabel.Location = new System.Drawing.Point(44, 37);
            this.masterLabel.Name = "masterLabel";
            this.masterLabel.Size = new System.Drawing.Size(80, 13);
            this.masterLabel.TabIndex = 10;
            this.masterLabel.Text = "Master Address";
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(89, 11);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(35, 13);
            this.nameLabel.TabIndex = 11;
            this.nameLabel.Text = "Name";
            // 
            // xmlRpcPortBox
            // 
            this.xmlRpcPortBox.Location = new System.Drawing.Point(259, 60);
            this.xmlRpcPortBox.Name = "xmlRpcPortBox";
            this.xmlRpcPortBox.Size = new System.Drawing.Size(38, 20);
            this.xmlRpcPortBox.TabIndex = 15;
            this.xmlRpcPortBox.Text = "4567";
            // 
            // xmlRpcPortLabel
            // 
            this.xmlRpcPortLabel.AutoSize = true;
            this.xmlRpcPortLabel.Location = new System.Drawing.Point(177, 63);
            this.xmlRpcPortLabel.Name = "xmlRpcPortLabel";
            this.xmlRpcPortLabel.Size = new System.Drawing.Size(76, 13);
            this.xmlRpcPortLabel.TabIndex = 14;
            this.xmlRpcPortLabel.Text = "XML-RPC Port";
            // 
            // packetsToProcessLabel
            // 
            this.packetsToProcessLabel.AutoSize = true;
            this.packetsToProcessLabel.Location = new System.Drawing.Point(127, 122);
            this.packetsToProcessLabel.Name = "packetsToProcessLabel";
            this.packetsToProcessLabel.Size = new System.Drawing.Size(13, 13);
            this.packetsToProcessLabel.TabIndex = 16;
            this.packetsToProcessLabel.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 109);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Packets Received";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 122);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "Packets To Process";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 135);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "Unprocessed Packets";
            // 
            // unprocessedPacketsLabel
            // 
            this.unprocessedPacketsLabel.AutoSize = true;
            this.unprocessedPacketsLabel.Location = new System.Drawing.Point(127, 135);
            this.unprocessedPacketsLabel.Name = "unprocessedPacketsLabel";
            this.unprocessedPacketsLabel.Size = new System.Drawing.Size(13, 13);
            this.unprocessedPacketsLabel.TabIndex = 19;
            this.unprocessedPacketsLabel.Text = "0";
            // 
            // tabContainer
            // 
            this.tabContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabContainer.Controls.Add(this.networkTab);
            this.tabContainer.Controls.Add(this.cameraTab);
            this.tabContainer.Controls.Add(this.avatarsTab);
            this.tabContainer.Controls.Add(this.packetTab);
            this.tabContainer.Location = new System.Drawing.Point(-1, 36);
            this.tabContainer.Name = "tabContainer";
            this.tabContainer.SelectedIndex = 0;
            this.tabContainer.Size = new System.Drawing.Size(949, 599);
            this.tabContainer.TabIndex = 21;
            // 
            // networkTab
            // 
            this.networkTab.Controls.Add(this.label1);
            this.networkTab.Controls.Add(this.xmlRpcPortBox);
            this.networkTab.Controls.Add(this.label3);
            this.networkTab.Controls.Add(this.xmlRpcPortLabel);
            this.networkTab.Controls.Add(this.countLabel);
            this.networkTab.Controls.Add(this.unprocessedPacketsLabel);
            this.networkTab.Controls.Add(this.nameLabel);
            this.networkTab.Controls.Add(this.packetsToProcessLabel);
            this.networkTab.Controls.Add(this.masterLabel);
            this.networkTab.Controls.Add(this.label2);
            this.networkTab.Controls.Add(this.listenIPBox);
            this.networkTab.Controls.Add(this.connectButton);
            this.networkTab.Controls.Add(this.listenIPLabel);
            this.networkTab.Controls.Add(this.nameBox);
            this.networkTab.Controls.Add(this.portBox);
            this.networkTab.Controls.Add(this.masterURIBox);
            this.networkTab.Controls.Add(this.portLabel);
            this.networkTab.Location = new System.Drawing.Point(4, 22);
            this.networkTab.Name = "networkTab";
            this.networkTab.Padding = new System.Windows.Forms.Padding(3);
            this.networkTab.Size = new System.Drawing.Size(941, 573);
            this.networkTab.TabIndex = 0;
            this.networkTab.Text = "Network";
            this.networkTab.UseVisualStyleBackColor = true;
            // 
            // cameraTab
            // 
            this.cameraTab.AutoScroll = true;
            this.cameraTab.Controls.Add(this.setFollowCamPropertiesPanel);
            this.cameraTab.Location = new System.Drawing.Point(4, 22);
            this.cameraTab.Name = "cameraTab";
            this.cameraTab.Padding = new System.Windows.Forms.Padding(3);
            this.cameraTab.Size = new System.Drawing.Size(941, 573);
            this.cameraTab.TabIndex = 1;
            this.cameraTab.Text = "Camera";
            this.cameraTab.UseVisualStyleBackColor = true;
            // 
            // setFollowCamPropertiesPanel
            // 
            this.setFollowCamPropertiesPanel.Acceleration = ((OpenMetaverse.Vector3)(resources.GetObject("setFollowCamPropertiesPanel.Acceleration")));
            this.setFollowCamPropertiesPanel.AngularAcceleration = ((OpenMetaverse.Vector3)(resources.GetObject("setFollowCamPropertiesPanel.AngularAcceleration")));
            this.setFollowCamPropertiesPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.setFollowCamPropertiesPanel.Location = new System.Drawing.Point(3, 3);
            this.setFollowCamPropertiesPanel.Name = "setFollowCamPropertiesPanel";
            this.setFollowCamPropertiesPanel.Position = ((OpenMetaverse.Vector3)(resources.GetObject("setFollowCamPropertiesPanel.Position")));
            this.setFollowCamPropertiesPanel.Rotation = ((OpenMetaverse.Quaternion)(resources.GetObject("setFollowCamPropertiesPanel.Rotation")));
            this.setFollowCamPropertiesPanel.Size = new System.Drawing.Size(935, 567);
            this.setFollowCamPropertiesPanel.TabIndex = 0;
            this.setFollowCamPropertiesPanel.Velocity = ((OpenMetaverse.Vector3)(resources.GetObject("setFollowCamPropertiesPanel.Velocity")));
            // 
            // avatarsTab
            // 
            this.avatarsTab.Controls.Add(this.avatarList);
            this.avatarsTab.Location = new System.Drawing.Point(4, 22);
            this.avatarsTab.Name = "avatarsTab";
            this.avatarsTab.Padding = new System.Windows.Forms.Padding(3);
            this.avatarsTab.Size = new System.Drawing.Size(941, 573);
            this.avatarsTab.TabIndex = 2;
            this.avatarsTab.Text = "Avatars";
            this.avatarsTab.UseVisualStyleBackColor = true;
            // 
            // avatarList
            // 
            this.avatarList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.avatarList.FormattingEnabled = true;
            this.avatarList.Location = new System.Drawing.Point(3, 3);
            this.avatarList.Name = "avatarList";
            this.avatarList.Size = new System.Drawing.Size(935, 567);
            this.avatarList.TabIndex = 22;
            this.avatarList.SelectedIndexChanged += new System.EventHandler(this.avatarList_SelectedIndexChanged);
            // 
            // packetTab
            // 
            this.packetTab.AutoScroll = true;
            this.packetTab.Controls.Add(this.velocityPanel);
            this.packetTab.Controls.Add(this.rotationalVelocityPanel);
            this.packetTab.Controls.Add(this.accelerationPanel);
            this.packetTab.Controls.Add(this.positionPanel);
            this.packetTab.Controls.Add(this.rotationPanel);
            this.packetTab.Location = new System.Drawing.Point(4, 22);
            this.packetTab.Name = "packetTab";
            this.packetTab.Padding = new System.Windows.Forms.Padding(3);
            this.packetTab.Size = new System.Drawing.Size(941, 573);
            this.packetTab.TabIndex = 3;
            this.packetTab.Text = "Packet";
            this.packetTab.UseVisualStyleBackColor = true;
            // 
            // velocityPanel
            // 
            this.velocityPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.velocityPanel.DisplayName = "Velocity";
            this.velocityPanel.Location = new System.Drawing.Point(9, 267);
            this.velocityPanel.Max = 32D;
            this.velocityPanel.Min = -32D;
            this.velocityPanel.Name = "velocityPanel";
            this.velocityPanel.Size = new System.Drawing.Size(910, 98);
            this.velocityPanel.TabIndex = 4;
            this.velocityPanel.Value = ((OpenMetaverse.Vector3)(resources.GetObject("velocityPanel.Value")));
            this.velocityPanel.X = 0F;
            this.velocityPanel.Y = 0F;
            this.velocityPanel.Z = 0F;
            // 
            // rotationalVelocityPanel
            // 
            this.rotationalVelocityPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rotationalVelocityPanel.DisplayName = "Rotational Velocity";
            this.rotationalVelocityPanel.Location = new System.Drawing.Point(9, 475);
            this.rotationalVelocityPanel.Max = 32D;
            this.rotationalVelocityPanel.Min = -32D;
            this.rotationalVelocityPanel.Name = "rotationalVelocityPanel";
            this.rotationalVelocityPanel.Size = new System.Drawing.Size(910, 98);
            this.rotationalVelocityPanel.TabIndex = 3;
            this.rotationalVelocityPanel.Value = ((OpenMetaverse.Vector3)(resources.GetObject("rotationalVelocityPanel.Value")));
            this.rotationalVelocityPanel.X = 0F;
            this.rotationalVelocityPanel.Y = 0F;
            this.rotationalVelocityPanel.Z = 0F;
            // 
            // accelerationPanel
            // 
            this.accelerationPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.accelerationPanel.DisplayName = "Acceleration";
            this.accelerationPanel.Location = new System.Drawing.Point(9, 371);
            this.accelerationPanel.Max = 32D;
            this.accelerationPanel.Min = -32D;
            this.accelerationPanel.Name = "accelerationPanel";
            this.accelerationPanel.Size = new System.Drawing.Size(910, 98);
            this.accelerationPanel.TabIndex = 2;
            this.accelerationPanel.Value = ((OpenMetaverse.Vector3)(resources.GetObject("accelerationPanel.Value")));
            this.accelerationPanel.X = 0F;
            this.accelerationPanel.Y = 0F;
            this.accelerationPanel.Z = 0F;
            // 
            // positionPanel
            // 
            this.positionPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.positionPanel.DisplayName = "Position";
            this.positionPanel.Location = new System.Drawing.Point(9, 10);
            this.positionPanel.Max = 256D;
            this.positionPanel.Min = 0D;
            this.positionPanel.Name = "positionPanel";
            this.positionPanel.Size = new System.Drawing.Size(910, 98);
            this.positionPanel.TabIndex = 1;
            this.positionPanel.Value = ((OpenMetaverse.Vector3)(resources.GetObject("positionPanel.Value")));
            this.positionPanel.X = 0F;
            this.positionPanel.Y = 0F;
            this.positionPanel.Z = 0F;
            this.positionPanel.OnChange += new System.EventHandler(this.positionPanel_OnChange);
            // 
            // rotationPanel
            // 
            this.rotationPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rotationPanel.DisplayName = "Rotation";
            this.rotationPanel.Location = new System.Drawing.Point(9, 114);
            this.rotationPanel.Name = "rotationPanel";
            this.rotationPanel.Rotation = ((OpenMetaverse.Quaternion)(resources.GetObject("rotationPanel.Rotation")));
            this.rotationPanel.Size = new System.Drawing.Size(910, 147);
            this.rotationPanel.TabIndex = 0;
            this.rotationPanel.Vector = ((OpenMetaverse.Vector3)(resources.GetObject("rotationPanel.Vector")));
            this.rotationPanel.OnChange += new System.EventHandler(this.rotationPanel_OnChange);
            // 
            // updateTimer
            // 
            this.updateTimer.Enabled = true;
            this.updateTimer.Interval = 20;
            this.updateTimer.Tick += new System.EventHandler(this.updateTimer_Tick);
            // 
            // proxyPanel
            // 
            this.proxyPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.proxyPanel.ListenIP = "127.0.0.1";
            this.proxyPanel.Location = new System.Drawing.Point(-1, 1);
            this.proxyPanel.LoginURI = "http://localhost:9000";
            this.proxyPanel.Name = "proxyPanel";
            this.proxyPanel.Port = "8081";
            this.proxyPanel.Size = new System.Drawing.Size(949, 29);
            this.proxyPanel.TabIndex = 13;
            // 
            // SlaveProxyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(945, 626);
            this.Controls.Add(this.tabContainer);
            this.Controls.Add(this.proxyPanel);
            this.Name = "SlaveProxyForm";
            this.Text = "Slave Proxy GUI";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SlaveProxyForm_FormClosing);
            this.tabContainer.ResumeLayout(false);
            this.networkTab.ResumeLayout(false);
            this.networkTab.PerformLayout();
            this.cameraTab.ResumeLayout(false);
            this.avatarsTab.ResumeLayout(false);
            this.packetTab.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox nameBox;
        private System.Windows.Forms.TextBox masterURIBox;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.Timer checkTimer;
        private System.Windows.Forms.Label countLabel;
        private System.Windows.Forms.TextBox listenIPBox;
        private System.Windows.Forms.Label listenIPLabel;
        private System.Windows.Forms.TextBox portBox;
        private System.Windows.Forms.Label portLabel;
        private System.Windows.Forms.Label masterLabel;
        private System.Windows.Forms.Label nameLabel;
        private UtilLib.ProxyPanel proxyPanel;
        private System.Windows.Forms.TextBox xmlRpcPortBox;
        private System.Windows.Forms.Label xmlRpcPortLabel;
        private System.Windows.Forms.Label packetsToProcessLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label unprocessedPacketsLabel;
        private System.Windows.Forms.TabControl tabContainer;
        private System.Windows.Forms.TabPage networkTab;
        private System.Windows.Forms.TabPage cameraTab;
        private UtilLib.SetFollowCamPropertiesPanel setFollowCamPropertiesPanel;
        private System.Windows.Forms.TabPage avatarsTab;
        private System.Windows.Forms.ListBox avatarList;
        private System.Windows.Forms.Timer updateTimer;
        private System.Windows.Forms.TabPage packetTab;
        private ProxyTestGUI.VectorPanel accelerationPanel;
        private ProxyTestGUI.VectorPanel positionPanel;
        private ProxyTestGUI.RotationPanel rotationPanel;
        private ProxyTestGUI.VectorPanel rotationalVelocityPanel;
        private ProxyTestGUI.VectorPanel velocityPanel;
    }
}


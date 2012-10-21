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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SlaveProxyForm));
            this.nameBox = new System.Windows.Forms.TextBox();
            this.masterAddressBox = new System.Windows.Forms.TextBox();
            this.connectButton = new System.Windows.Forms.Button();
            this.countLabel = new System.Windows.Forms.Label();
            this.listenIPBox = new System.Windows.Forms.TextBox();
            this.listenIPLabel = new System.Windows.Forms.Label();
            this.masterLabel = new System.Windows.Forms.Label();
            this.nameLabel = new System.Windows.Forms.Label();
            this.masterXmlRpcPortBox = new System.Windows.Forms.TextBox();
            this.xmlRpcPortLabel = new System.Windows.Forms.Label();
            this.packetsToProcessLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.processedPacketsLabel = new System.Windows.Forms.Label();
            this.tabContainer = new System.Windows.Forms.TabControl();
            this.networkTab = new System.Windows.Forms.TabPage();
            this.timerValueLabel = new System.Windows.Forms.Label();
            this.timerValue = new System.Windows.Forms.NumericUpDown();
            this.controlCameraCheck = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.unprocessedPacketsLabel = new System.Windows.Forms.Label();
            this.slaveBox = new System.Windows.Forms.GroupBox();
            this.xmlRpcPortBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.portBox = new System.Windows.Forms.TextBox();
            this.masterBox = new System.Windows.Forms.GroupBox();
            this.processObjectUpdatesCheck = new System.Windows.Forms.CheckBox();
            this.processAgentUpdatesCheck = new System.Windows.Forms.CheckBox();
            this.cameraTab = new System.Windows.Forms.TabPage();
            this.setFollowCamPropertiesPanel = new UtilLib.SetFollowCamPropertiesPanel();
            this.avatarsTab = new System.Windows.Forms.TabPage();
            this.avatarList = new System.Windows.Forms.ListBox();
            this.packetTab = new System.Windows.Forms.TabPage();
            this.rotationPanel = new ProxyTestGUI.RotationPanel();
            this.velocityPanel = new ProxyTestGUI.VectorPanel();
            this.rotationalVelocityPanel = new ProxyTestGUI.VectorPanel();
            this.accelerationPanel = new ProxyTestGUI.VectorPanel();
            this.positionPanel = new ProxyTestGUI.VectorPanel();
            this.proxyPanel = new UtilLib.ProxyPanel();
            this.tabContainer.SuspendLayout();
            this.networkTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timerValue)).BeginInit();
            this.slaveBox.SuspendLayout();
            this.masterBox.SuspendLayout();
            this.cameraTab.SuspendLayout();
            this.avatarsTab.SuspendLayout();
            this.packetTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // nameBox
            // 
            this.nameBox.Location = new System.Drawing.Point(47, 8);
            this.nameBox.Name = "nameBox";
            this.nameBox.Size = new System.Drawing.Size(45, 20);
            this.nameBox.TabIndex = 0;
            this.nameBox.Text = "Slave 1";
            // 
            // masterAddressBox
            // 
            this.masterAddressBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.masterAddressBox.Location = new System.Drawing.Point(64, 21);
            this.masterAddressBox.Name = "masterAddressBox";
            this.masterAddressBox.Size = new System.Drawing.Size(284, 20);
            this.masterAddressBox.TabIndex = 2;
            this.masterAddressBox.Text = "127.0.0.1";
            // 
            // connectButton
            // 
            this.connectButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.connectButton.Location = new System.Drawing.Point(98, 6);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(387, 23);
            this.connectButton.TabIndex = 3;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // countLabel
            // 
            this.countLabel.AutoSize = true;
            this.countLabel.Location = new System.Drawing.Point(118, 148);
            this.countLabel.Name = "countLabel";
            this.countLabel.Size = new System.Drawing.Size(13, 13);
            this.countLabel.TabIndex = 4;
            this.countLabel.Text = "0";
            // 
            // listenIPBox
            // 
            this.listenIPBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listenIPBox.Location = new System.Drawing.Point(61, 19);
            this.listenIPBox.Name = "listenIPBox";
            this.listenIPBox.Size = new System.Drawing.Size(182, 20);
            this.listenIPBox.TabIndex = 9;
            this.listenIPBox.Text = "127.0.0.1";
            // 
            // listenIPLabel
            // 
            this.listenIPLabel.AutoSize = true;
            this.listenIPLabel.Location = new System.Drawing.Point(38, 22);
            this.listenIPLabel.Name = "listenIPLabel";
            this.listenIPLabel.Size = new System.Drawing.Size(17, 13);
            this.listenIPLabel.TabIndex = 8;
            this.listenIPLabel.Text = "IP";
            // 
            // masterLabel
            // 
            this.masterLabel.AutoSize = true;
            this.masterLabel.Location = new System.Drawing.Point(13, 24);
            this.masterLabel.Name = "masterLabel";
            this.masterLabel.Size = new System.Drawing.Size(45, 13);
            this.masterLabel.TabIndex = 10;
            this.masterLabel.Text = "Address";
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(6, 11);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(35, 13);
            this.nameLabel.TabIndex = 11;
            this.nameLabel.Text = "Name";
            // 
            // masterXmlRpcPortBox
            // 
            this.masterXmlRpcPortBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.masterXmlRpcPortBox.Location = new System.Drawing.Point(436, 21);
            this.masterXmlRpcPortBox.Name = "masterXmlRpcPortBox";
            this.masterXmlRpcPortBox.Size = new System.Drawing.Size(38, 20);
            this.masterXmlRpcPortBox.TabIndex = 15;
            this.masterXmlRpcPortBox.Text = "5678";
            // 
            // xmlRpcPortLabel
            // 
            this.xmlRpcPortLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.xmlRpcPortLabel.AutoSize = true;
            this.xmlRpcPortLabel.Location = new System.Drawing.Point(354, 24);
            this.xmlRpcPortLabel.Name = "xmlRpcPortLabel";
            this.xmlRpcPortLabel.Size = new System.Drawing.Size(76, 13);
            this.xmlRpcPortLabel.TabIndex = 14;
            this.xmlRpcPortLabel.Text = "XML-RPC Port";
            // 
            // packetsToProcessLabel
            // 
            this.packetsToProcessLabel.AutoSize = true;
            this.packetsToProcessLabel.Location = new System.Drawing.Point(118, 161);
            this.packetsToProcessLabel.Name = "packetsToProcessLabel";
            this.packetsToProcessLabel.Size = new System.Drawing.Size(13, 13);
            this.packetsToProcessLabel.TabIndex = 16;
            this.packetsToProcessLabel.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 148);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Packets Processed";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 161);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "Packets To Process";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 174);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "Processed Packets";
            // 
            // processedPacketsLabel
            // 
            this.processedPacketsLabel.AutoSize = true;
            this.processedPacketsLabel.Location = new System.Drawing.Point(118, 174);
            this.processedPacketsLabel.Name = "processedPacketsLabel";
            this.processedPacketsLabel.Size = new System.Drawing.Size(13, 13);
            this.processedPacketsLabel.TabIndex = 19;
            this.processedPacketsLabel.Text = "0";
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
            this.tabContainer.Size = new System.Drawing.Size(496, 387);
            this.tabContainer.TabIndex = 21;
            // 
            // networkTab
            // 
            this.networkTab.Controls.Add(this.timerValueLabel);
            this.networkTab.Controls.Add(this.timerValue);
            this.networkTab.Controls.Add(this.controlCameraCheck);
            this.networkTab.Controls.Add(this.label6);
            this.networkTab.Controls.Add(this.unprocessedPacketsLabel);
            this.networkTab.Controls.Add(this.slaveBox);
            this.networkTab.Controls.Add(this.masterBox);
            this.networkTab.Controls.Add(this.processObjectUpdatesCheck);
            this.networkTab.Controls.Add(this.processAgentUpdatesCheck);
            this.networkTab.Controls.Add(this.label1);
            this.networkTab.Controls.Add(this.label3);
            this.networkTab.Controls.Add(this.countLabel);
            this.networkTab.Controls.Add(this.processedPacketsLabel);
            this.networkTab.Controls.Add(this.nameLabel);
            this.networkTab.Controls.Add(this.packetsToProcessLabel);
            this.networkTab.Controls.Add(this.label2);
            this.networkTab.Controls.Add(this.connectButton);
            this.networkTab.Controls.Add(this.nameBox);
            this.networkTab.Location = new System.Drawing.Point(4, 22);
            this.networkTab.Name = "networkTab";
            this.networkTab.Padding = new System.Windows.Forms.Padding(3);
            this.networkTab.Size = new System.Drawing.Size(488, 361);
            this.networkTab.TabIndex = 0;
            this.networkTab.Text = "Network";
            this.networkTab.UseVisualStyleBackColor = true;
            // 
            // timerValueLabel
            // 
            this.timerValueLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.timerValueLabel.AutoSize = true;
            this.timerValueLabel.Location = new System.Drawing.Point(302, 219);
            this.timerValueLabel.Name = "timerValueLabel";
            this.timerValueLabel.Size = new System.Drawing.Size(122, 13);
            this.timerValueLabel.TabIndex = 34;
            this.timerValueLabel.Text = "Packet Send Frequency";
            // 
            // timerValue
            // 
            this.timerValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.timerValue.Location = new System.Drawing.Point(430, 217);
            this.timerValue.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.timerValue.Name = "timerValue";
            this.timerValue.Size = new System.Drawing.Size(52, 20);
            this.timerValue.TabIndex = 33;
            this.timerValue.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // controlCameraCheck
            // 
            this.controlCameraCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.controlCameraCheck.AutoSize = true;
            this.controlCameraCheck.Location = new System.Drawing.Point(317, 194);
            this.controlCameraCheck.Name = "controlCameraCheck";
            this.controlCameraCheck.Size = new System.Drawing.Size(168, 17);
            this.controlCameraCheck.TabIndex = 32;
            this.controlCameraCheck.Text = "Send Camera Control Packets";
            this.controlCameraCheck.UseVisualStyleBackColor = true;
            this.controlCameraCheck.CheckedChanged += new System.EventHandler(this.controlCameraCheck_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 187);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(112, 13);
            this.label6.TabIndex = 31;
            this.label6.Text = "Unprocessed Packets";
            // 
            // unprocessedPacketsLabel
            // 
            this.unprocessedPacketsLabel.AutoSize = true;
            this.unprocessedPacketsLabel.Location = new System.Drawing.Point(118, 187);
            this.unprocessedPacketsLabel.Name = "unprocessedPacketsLabel";
            this.unprocessedPacketsLabel.Size = new System.Drawing.Size(13, 13);
            this.unprocessedPacketsLabel.TabIndex = 30;
            this.unprocessedPacketsLabel.Text = "0";
            // 
            // slaveBox
            // 
            this.slaveBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.slaveBox.Controls.Add(this.listenIPBox);
            this.slaveBox.Controls.Add(this.listenIPLabel);
            this.slaveBox.Controls.Add(this.xmlRpcPortBox);
            this.slaveBox.Controls.Add(this.label5);
            this.slaveBox.Controls.Add(this.label4);
            this.slaveBox.Controls.Add(this.portBox);
            this.slaveBox.Location = new System.Drawing.Point(3, 93);
            this.slaveBox.Name = "slaveBox";
            this.slaveBox.Size = new System.Drawing.Size(482, 52);
            this.slaveBox.TabIndex = 29;
            this.slaveBox.TabStop = false;
            this.slaveBox.Text = "Published End Point";
            // 
            // xmlRpcPortBox
            // 
            this.xmlRpcPortBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.xmlRpcPortBox.Location = new System.Drawing.Point(436, 19);
            this.xmlRpcPortBox.Name = "xmlRpcPortBox";
            this.xmlRpcPortBox.Size = new System.Drawing.Size(38, 20);
            this.xmlRpcPortBox.TabIndex = 26;
            this.xmlRpcPortBox.Text = "4567";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(249, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 13);
            this.label5.TabIndex = 23;
            this.label5.Text = "UDP Port";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(354, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 13);
            this.label4.TabIndex = 25;
            this.label4.Text = "XML-RPC Port";
            // 
            // portBox
            // 
            this.portBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.portBox.Location = new System.Drawing.Point(307, 19);
            this.portBox.Name = "portBox";
            this.portBox.Size = new System.Drawing.Size(41, 20);
            this.portBox.TabIndex = 24;
            this.portBox.Text = "8091";
            // 
            // masterBox
            // 
            this.masterBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.masterBox.Controls.Add(this.masterAddressBox);
            this.masterBox.Controls.Add(this.masterLabel);
            this.masterBox.Controls.Add(this.xmlRpcPortLabel);
            this.masterBox.Controls.Add(this.masterXmlRpcPortBox);
            this.masterBox.Location = new System.Drawing.Point(3, 35);
            this.masterBox.Name = "masterBox";
            this.masterBox.Size = new System.Drawing.Size(482, 52);
            this.masterBox.TabIndex = 28;
            this.masterBox.TabStop = false;
            this.masterBox.Text = "Master End Point";
            // 
            // processObjectUpdatesCheck
            // 
            this.processObjectUpdatesCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.processObjectUpdatesCheck.AutoSize = true;
            this.processObjectUpdatesCheck.Location = new System.Drawing.Point(317, 171);
            this.processObjectUpdatesCheck.Name = "processObjectUpdatesCheck";
            this.processObjectUpdatesCheck.Size = new System.Drawing.Size(141, 17);
            this.processObjectUpdatesCheck.TabIndex = 22;
            this.processObjectUpdatesCheck.Text = "Process Object Updates";
            this.processObjectUpdatesCheck.UseVisualStyleBackColor = true;
            this.processObjectUpdatesCheck.CheckedChanged += new System.EventHandler(this.processObjectUpdatesCheck_CheckedChanged);
            // 
            // processAgentUpdatesCheck
            // 
            this.processAgentUpdatesCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.processAgentUpdatesCheck.AutoSize = true;
            this.processAgentUpdatesCheck.Location = new System.Drawing.Point(317, 148);
            this.processAgentUpdatesCheck.Name = "processAgentUpdatesCheck";
            this.processAgentUpdatesCheck.Size = new System.Drawing.Size(138, 17);
            this.processAgentUpdatesCheck.TabIndex = 21;
            this.processAgentUpdatesCheck.Text = "Process Agent Updates";
            this.processAgentUpdatesCheck.UseVisualStyleBackColor = true;
            this.processAgentUpdatesCheck.CheckedChanged += new System.EventHandler(this.processAgentUpdatesCheck_CheckedChanged);
            // 
            // cameraTab
            // 
            this.cameraTab.AutoScroll = true;
            this.cameraTab.Controls.Add(this.setFollowCamPropertiesPanel);
            this.cameraTab.Location = new System.Drawing.Point(4, 22);
            this.cameraTab.Name = "cameraTab";
            this.cameraTab.Padding = new System.Windows.Forms.Padding(3);
            this.cameraTab.Size = new System.Drawing.Size(488, 361);
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
            this.setFollowCamPropertiesPanel.MinimumSize = new System.Drawing.Size(485, 343);
            this.setFollowCamPropertiesPanel.Name = "setFollowCamPropertiesPanel";
            this.setFollowCamPropertiesPanel.Position = ((OpenMetaverse.Vector3)(resources.GetObject("setFollowCamPropertiesPanel.Position")));
            this.setFollowCamPropertiesPanel.Rotation = ((OpenMetaverse.Quaternion)(resources.GetObject("setFollowCamPropertiesPanel.Rotation")));
            this.setFollowCamPropertiesPanel.Size = new System.Drawing.Size(485, 355);
            this.setFollowCamPropertiesPanel.TabIndex = 0;
            this.setFollowCamPropertiesPanel.Velocity = ((OpenMetaverse.Vector3)(resources.GetObject("setFollowCamPropertiesPanel.Velocity")));
            // 
            // avatarsTab
            // 
            this.avatarsTab.Controls.Add(this.avatarList);
            this.avatarsTab.Location = new System.Drawing.Point(4, 22);
            this.avatarsTab.Name = "avatarsTab";
            this.avatarsTab.Padding = new System.Windows.Forms.Padding(3);
            this.avatarsTab.Size = new System.Drawing.Size(488, 361);
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
            this.avatarList.Size = new System.Drawing.Size(482, 355);
            this.avatarList.TabIndex = 22;
            this.avatarList.SelectedIndexChanged += new System.EventHandler(this.avatarList_SelectedIndexChanged);
            // 
            // packetTab
            // 
            this.packetTab.AutoScroll = true;
            this.packetTab.Controls.Add(this.rotationPanel);
            this.packetTab.Controls.Add(this.velocityPanel);
            this.packetTab.Controls.Add(this.rotationalVelocityPanel);
            this.packetTab.Controls.Add(this.accelerationPanel);
            this.packetTab.Controls.Add(this.positionPanel);
            this.packetTab.Location = new System.Drawing.Point(4, 22);
            this.packetTab.Name = "packetTab";
            this.packetTab.Padding = new System.Windows.Forms.Padding(3);
            this.packetTab.Size = new System.Drawing.Size(488, 361);
            this.packetTab.TabIndex = 3;
            this.packetTab.Text = "Packet";
            this.packetTab.UseVisualStyleBackColor = true;
            // 
            // rotationPanel
            // 
            this.rotationPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rotationPanel.DisplayName = "Rotation";
            this.rotationPanel.Location = new System.Drawing.Point(0, 110);
            this.rotationPanel.Name = "rotationPanel";
            this.rotationPanel.Pitch = 0F;
            this.rotationPanel.Rotation = ((OpenMetaverse.Quaternion)(resources.GetObject("rotationPanel.Rotation")));
            this.rotationPanel.Size = new System.Drawing.Size(461, 147);
            this.rotationPanel.TabIndex = 4;
            this.rotationPanel.Vector = ((OpenMetaverse.Vector3)(resources.GetObject("rotationPanel.Vector")));
            this.rotationPanel.Yaw = 0F;
            this.rotationPanel.OnChange += new System.EventHandler(this.rotationPanel_OnChange);
            // 
            // velocityPanel
            // 
            this.velocityPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.velocityPanel.DisplayName = "Velocity";
            this.velocityPanel.Location = new System.Drawing.Point(3, 263);
            this.velocityPanel.Max = 5D;
            this.velocityPanel.Min = -5D;
            this.velocityPanel.Name = "velocityPanel";
            this.velocityPanel.Size = new System.Drawing.Size(458, 98);
            this.velocityPanel.TabIndex = 3;
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
            this.rotationalVelocityPanel.Location = new System.Drawing.Point(3, 471);
            this.rotationalVelocityPanel.Max = 5D;
            this.rotationalVelocityPanel.Min = -5D;
            this.rotationalVelocityPanel.Name = "rotationalVelocityPanel";
            this.rotationalVelocityPanel.Size = new System.Drawing.Size(458, 98);
            this.rotationalVelocityPanel.TabIndex = 2;
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
            this.accelerationPanel.Location = new System.Drawing.Point(3, 367);
            this.accelerationPanel.Max = 5D;
            this.accelerationPanel.Min = -5D;
            this.accelerationPanel.Name = "accelerationPanel";
            this.accelerationPanel.Size = new System.Drawing.Size(458, 98);
            this.accelerationPanel.TabIndex = 1;
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
            this.positionPanel.Location = new System.Drawing.Point(3, 6);
            this.positionPanel.Max = 256D;
            this.positionPanel.Min = 0D;
            this.positionPanel.MinimumSize = new System.Drawing.Size(474, 98);
            this.positionPanel.Name = "positionPanel";
            this.positionPanel.Size = new System.Drawing.Size(474, 98);
            this.positionPanel.TabIndex = 0;
            this.positionPanel.Value = ((OpenMetaverse.Vector3)(resources.GetObject("positionPanel.Value")));
            this.positionPanel.X = 128F;
            this.positionPanel.Y = 128F;
            this.positionPanel.Z = 128F;
            this.positionPanel.OnChange += new System.EventHandler(this.positionPanel_OnChange);
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
            this.proxyPanel.Size = new System.Drawing.Size(496, 29);
            this.proxyPanel.TabIndex = 13;
            // 
            // SlaveProxyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 420);
            this.Controls.Add(this.tabContainer);
            this.Controls.Add(this.proxyPanel);
            this.MinimumSize = new System.Drawing.Size(500, 447);
            this.Name = "SlaveProxyForm";
            this.Text = "Slave Proxy GUI";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SlaveProxyForm_FormClosing);
            this.tabContainer.ResumeLayout(false);
            this.networkTab.ResumeLayout(false);
            this.networkTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timerValue)).EndInit();
            this.slaveBox.ResumeLayout(false);
            this.slaveBox.PerformLayout();
            this.masterBox.ResumeLayout(false);
            this.masterBox.PerformLayout();
            this.cameraTab.ResumeLayout(false);
            this.avatarsTab.ResumeLayout(false);
            this.packetTab.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox nameBox;
        private System.Windows.Forms.TextBox masterAddressBox;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.Label countLabel;
        private System.Windows.Forms.TextBox listenIPBox;
        private System.Windows.Forms.Label listenIPLabel;
        private System.Windows.Forms.Label masterLabel;
        private System.Windows.Forms.Label nameLabel;
        private UtilLib.ProxyPanel proxyPanel;
        private System.Windows.Forms.TextBox masterXmlRpcPortBox;
        private System.Windows.Forms.Label xmlRpcPortLabel;
        private System.Windows.Forms.Label packetsToProcessLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label processedPacketsLabel;
        private System.Windows.Forms.TabControl tabContainer;
        private System.Windows.Forms.TabPage networkTab;
        private System.Windows.Forms.TabPage cameraTab;
        private System.Windows.Forms.TabPage avatarsTab;
        private System.Windows.Forms.ListBox avatarList;
        private System.Windows.Forms.TabPage packetTab;
        //private ProxyTestGUI.VectorPanel accelerationPanel;
        //private ProxyTestGUI.VectorPanel positionPanel;
        //private ProxyTestGUI.RotationPanel rotationPanel;
        //private ProxyTestGUI.VectorPanel rotationalVelocityPanel;
        //private ProxyTestGUI.VectorPanel velocityPanel;
        private System.Windows.Forms.CheckBox processObjectUpdatesCheck;
        private System.Windows.Forms.CheckBox processAgentUpdatesCheck;
        private System.Windows.Forms.TextBox xmlRpcPortBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox portBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox masterBox;
        private System.Windows.Forms.GroupBox slaveBox;
        private UtilLib.SetFollowCamPropertiesPanel setFollowCamPropertiesPanel;
        private ProxyTestGUI.RotationPanel rotationPanel;
        private ProxyTestGUI.VectorPanel velocityPanel;
        private ProxyTestGUI.VectorPanel rotationalVelocityPanel;
        private ProxyTestGUI.VectorPanel accelerationPanel;
        private ProxyTestGUI.VectorPanel positionPanel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label unprocessedPacketsLabel;
        private System.Windows.Forms.CheckBox controlCameraCheck;
        private System.Windows.Forms.Label timerValueLabel;
        private System.Windows.Forms.NumericUpDown timerValue;
    }
}


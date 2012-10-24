namespace MasterProxy {
    partial class MasterProxyForm {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MasterProxyForm));
            this.listTimer = new System.Windows.Forms.Timer(this.components);
            this.slavesListBox = new System.Windows.Forms.ListBox();
            this.packetCountLabel = new System.Windows.Forms.Label();
            this.packetList = new System.Windows.Forms.ListView();
            this.packetNameColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.selectAll = new System.Windows.Forms.CheckBox();
            this.forwardLoginCheck = new System.Windows.Forms.CheckBox();
            this.udpPortBox = new System.Windows.Forms.TextBox();
            this.localAddressLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.forwardedCountLabel = new System.Windows.Forms.Label();
            this.proxyPanel = new UtilLib.ProxyPanel();
            this.tabContainer = new System.Windows.Forms.TabControl();
            this.configTab = new System.Windows.Forms.TabPage();
            this.controlTab = new System.Windows.Forms.TabPage();
            this.rotationPanel = new ProxyTestGUI.RotationPanel();
            this.positionPanel = new ProxyTestGUI.VectorPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.createdCountLabel = new System.Windows.Forms.Label();
            this.tabContainer.SuspendLayout();
            this.configTab.SuspendLayout();
            this.controlTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // listTimer
            // 
            this.listTimer.Enabled = true;
            this.listTimer.Interval = 5;
            this.listTimer.Tick += new System.EventHandler(this.listTimer_Tick);
            // 
            // slavesListBox
            // 
            this.slavesListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.slavesListBox.FormattingEnabled = true;
            this.slavesListBox.Location = new System.Drawing.Point(0, 3);
            this.slavesListBox.Name = "slavesListBox";
            this.slavesListBox.Size = new System.Drawing.Size(160, 290);
            this.slavesListBox.TabIndex = 1;
            // 
            // packetCountLabel
            // 
            this.packetCountLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.packetCountLabel.AutoSize = true;
            this.packetCountLabel.Location = new System.Drawing.Point(63, 366);
            this.packetCountLabel.Name = "packetCountLabel";
            this.packetCountLabel.Size = new System.Drawing.Size(13, 13);
            this.packetCountLabel.TabIndex = 9;
            this.packetCountLabel.Text = "0";
            // 
            // packetList
            // 
            this.packetList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.packetList.CheckBoxes = true;
            this.packetList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.packetNameColumn});
            this.packetList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.packetList.Location = new System.Drawing.Point(166, 3);
            this.packetList.MultiSelect = false;
            this.packetList.Name = "packetList";
            this.packetList.Size = new System.Drawing.Size(466, 290);
            this.packetList.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.packetList.TabIndex = 11;
            this.packetList.UseCompatibleStateImageBehavior = false;
            this.packetList.View = System.Windows.Forms.View.Details;
            this.packetList.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.packetList_ItemChecked);
            // 
            // packetNameColumn
            // 
            this.packetNameColumn.Text = "Packet Name";
            this.packetNameColumn.Width = 340;
            // 
            // selectAll
            // 
            this.selectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.selectAll.AutoSize = true;
            this.selectAll.Location = new System.Drawing.Point(517, 365);
            this.selectAll.Name = "selectAll";
            this.selectAll.Size = new System.Drawing.Size(120, 17);
            this.selectAll.TabIndex = 12;
            this.selectAll.Text = "Check/Uncheck All";
            this.selectAll.UseVisualStyleBackColor = true;
            this.selectAll.CheckedChanged += new System.EventHandler(this.selectAll_CheckedChanged);
            // 
            // forwardLoginCheck
            // 
            this.forwardLoginCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.forwardLoginCheck.AutoSize = true;
            this.forwardLoginCheck.Location = new System.Drawing.Point(413, 365);
            this.forwardLoginCheck.Name = "forwardLoginCheck";
            this.forwardLoginCheck.Size = new System.Drawing.Size(93, 17);
            this.forwardLoginCheck.TabIndex = 13;
            this.forwardLoginCheck.Text = "Forward Login";
            this.forwardLoginCheck.UseVisualStyleBackColor = true;
            this.forwardLoginCheck.CheckedChanged += new System.EventHandler(this.forwardLoginCheck_CheckedChanged);
            // 
            // udpPortBox
            // 
            this.udpPortBox.Location = new System.Drawing.Point(72, 9);
            this.udpPortBox.Name = "udpPortBox";
            this.udpPortBox.Size = new System.Drawing.Size(32, 20);
            this.udpPortBox.TabIndex = 14;
            this.udpPortBox.Text = "8090";
            // 
            // localAddressLabel
            // 
            this.localAddressLabel.AutoSize = true;
            this.localAddressLabel.Location = new System.Drawing.Point(12, 12);
            this.localAddressLabel.Name = "localAddressLabel";
            this.localAddressLabel.Size = new System.Drawing.Size(52, 13);
            this.localAddressLabel.TabIndex = 15;
            this.localAddressLabel.Text = "UDP Port";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 366);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Packets <";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(98, 366);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "Packets >";
            // 
            // forwardedCountLabel
            // 
            this.forwardedCountLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.forwardedCountLabel.AutoSize = true;
            this.forwardedCountLabel.Location = new System.Drawing.Point(152, 366);
            this.forwardedCountLabel.Name = "forwardedCountLabel";
            this.forwardedCountLabel.Size = new System.Drawing.Size(13, 13);
            this.forwardedCountLabel.TabIndex = 17;
            this.forwardedCountLabel.Text = "0";
            // 
            // proxyPanel
            // 
            this.proxyPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.proxyPanel.ListenIP = "127.0.0.1";
            this.proxyPanel.Location = new System.Drawing.Point(110, 5);
            this.proxyPanel.LoginURI = "http://mimuve.cs.st-andrews.ac.uk:8002";
            this.proxyPanel.Name = "proxyPanel";
            this.proxyPanel.Port = "8080";
            this.proxyPanel.Size = new System.Drawing.Size(531, 30);
            this.proxyPanel.TabIndex = 10;
            this.proxyPanel.OnStarted += new System.EventHandler(this.proxyPanel_Started);
            this.proxyPanel.KeyDown += new System.Windows.Forms.KeyEventHandler(this.keyDown);
            this.proxyPanel.KeyUp += new System.Windows.Forms.KeyEventHandler(this.keyUp);
            // 
            // tabContainer
            // 
            this.tabContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabContainer.Controls.Add(this.configTab);
            this.tabContainer.Controls.Add(this.controlTab);
            this.tabContainer.Location = new System.Drawing.Point(1, 41);
            this.tabContainer.Name = "tabContainer";
            this.tabContainer.SelectedIndex = 0;
            this.tabContainer.Size = new System.Drawing.Size(640, 318);
            this.tabContainer.TabIndex = 19;
            this.tabContainer.KeyDown += new System.Windows.Forms.KeyEventHandler(this.keyDown);
            this.tabContainer.KeyUp += new System.Windows.Forms.KeyEventHandler(this.keyUp);
            // 
            // configTab
            // 
            this.configTab.Controls.Add(this.slavesListBox);
            this.configTab.Controls.Add(this.packetList);
            this.configTab.Location = new System.Drawing.Point(4, 22);
            this.configTab.Name = "configTab";
            this.configTab.Padding = new System.Windows.Forms.Padding(3);
            this.configTab.Size = new System.Drawing.Size(632, 292);
            this.configTab.TabIndex = 0;
            this.configTab.Text = "Configuration";
            this.configTab.UseVisualStyleBackColor = true;
            // 
            // controlTab
            // 
            this.controlTab.Controls.Add(this.rotationPanel);
            this.controlTab.Controls.Add(this.positionPanel);
            this.controlTab.Location = new System.Drawing.Point(4, 22);
            this.controlTab.Name = "controlTab";
            this.controlTab.Padding = new System.Windows.Forms.Padding(3);
            this.controlTab.Size = new System.Drawing.Size(632, 292);
            this.controlTab.TabIndex = 1;
            this.controlTab.Text = "Control";
            this.controlTab.UseVisualStyleBackColor = true;
            // 
            // rotationPanel
            // 
            this.rotationPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rotationPanel.DisplayName = "Rotation";
            this.rotationPanel.Location = new System.Drawing.Point(0, 107);
            this.rotationPanel.Name = "rotationPanel";
            this.rotationPanel.Pitch = 0F;
            this.rotationPanel.Rotation = ((OpenMetaverse.Quaternion)(resources.GetObject("rotationPanel.Rotation")));
            this.rotationPanel.Size = new System.Drawing.Size(632, 147);
            this.rotationPanel.TabIndex = 1;
            this.rotationPanel.Vector = ((OpenMetaverse.Vector3)(resources.GetObject("rotationPanel.Vector")));
            this.rotationPanel.Yaw = 0F;
            this.rotationPanel.OnChange += new System.EventHandler(this.onChange);
            this.rotationPanel.KeyDown += new System.Windows.Forms.KeyEventHandler(this.keyDown);
            this.rotationPanel.KeyUp += new System.Windows.Forms.KeyEventHandler(this.keyUp);
            // 
            // positionPanel
            // 
            this.positionPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.positionPanel.DisplayName = "Position";
            this.positionPanel.Location = new System.Drawing.Point(0, 3);
            this.positionPanel.Max = 256D;
            this.positionPanel.Min = 0D;
            this.positionPanel.Name = "positionPanel";
            this.positionPanel.Size = new System.Drawing.Size(632, 98);
            this.positionPanel.TabIndex = 0;
            this.positionPanel.Value = ((OpenMetaverse.Vector3)(resources.GetObject("positionPanel.Value")));
            this.positionPanel.X = 128F;
            this.positionPanel.Y = 128F;
            this.positionPanel.Z = 24F;
            this.positionPanel.OnChange += new System.EventHandler(this.onChange);
            this.positionPanel.KeyDown += new System.Windows.Forms.KeyEventHandler(this.keyDown);
            this.positionPanel.KeyUp += new System.Windows.Forms.KeyEventHandler(this.keyUp);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(199, 366);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "Packets Created ";
            // 
            // createdCountLabel
            // 
            this.createdCountLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.createdCountLabel.AutoSize = true;
            this.createdCountLabel.Location = new System.Drawing.Point(285, 366);
            this.createdCountLabel.Name = "createdCountLabel";
            this.createdCountLabel.Size = new System.Drawing.Size(13, 13);
            this.createdCountLabel.TabIndex = 20;
            this.createdCountLabel.Text = "0";
            // 
            // MasterProxyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(644, 386);
            this.Controls.Add(this.createdCountLabel);
            this.Controls.Add(this.tabContainer);
            this.Controls.Add(this.forwardedCountLabel);
            this.Controls.Add(this.localAddressLabel);
            this.Controls.Add(this.udpPortBox);
            this.Controls.Add(this.forwardLoginCheck);
            this.Controls.Add(this.selectAll);
            this.Controls.Add(this.proxyPanel);
            this.Controls.Add(this.packetCountLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Name = "MasterProxyForm";
            this.Text = "Master Proxy";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MasterProxyForm_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.keyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.keyUp);
            this.tabContainer.ResumeLayout(false);
            this.configTab.ResumeLayout(false);
            this.controlTab.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer listTimer;
        private System.Windows.Forms.ListBox slavesListBox;
        private System.Windows.Forms.Label packetCountLabel;
        private UtilLib.ProxyPanel proxyPanel;
        private System.Windows.Forms.ListView packetList;
        private System.Windows.Forms.CheckBox selectAll;
        private System.Windows.Forms.ColumnHeader packetNameColumn;
        private System.Windows.Forms.CheckBox forwardLoginCheck;
        private System.Windows.Forms.TextBox udpPortBox;
        private System.Windows.Forms.Label localAddressLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label forwardedCountLabel;
        private System.Windows.Forms.TabControl tabContainer;
        private System.Windows.Forms.TabPage configTab;
        private System.Windows.Forms.TabPage controlTab;
        private ProxyTestGUI.VectorPanel positionPanel;
        private ProxyTestGUI.RotationPanel rotationPanel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label createdCountLabel;
    }
}


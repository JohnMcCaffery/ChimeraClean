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
            this.SuspendLayout();
            // 
            // listTimer
            // 
            this.listTimer.Enabled = true;
            this.listTimer.Tick += new System.EventHandler(this.listTimer_Tick);
            // 
            // slavesListBox
            // 
            this.slavesListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.slavesListBox.FormattingEnabled = true;
            this.slavesListBox.Location = new System.Drawing.Point(12, 41);
            this.slavesListBox.Name = "slavesListBox";
            this.slavesListBox.Size = new System.Drawing.Size(153, 212);
            this.slavesListBox.TabIndex = 1;
            // 
            // packetCountLabel
            // 
            this.packetCountLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.packetCountLabel.AutoSize = true;
            this.packetCountLabel.Location = new System.Drawing.Point(110, 264);
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
            this.packetList.Location = new System.Drawing.Point(171, 41);
            this.packetList.MultiSelect = false;
            this.packetList.Name = "packetList";
            this.packetList.Size = new System.Drawing.Size(393, 212);
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
            this.selectAll.Location = new System.Drawing.Point(444, 260);
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
            this.forwardLoginCheck.Location = new System.Drawing.Point(345, 260);
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
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 264);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Packets Received";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(168, 264);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "Packets Forwarded";
            // 
            // forwardedCountLabel
            // 
            this.forwardedCountLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.forwardedCountLabel.AutoSize = true;
            this.forwardedCountLabel.Location = new System.Drawing.Point(273, 264);
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
            this.proxyPanel.LoginURI = "http://localhost:9000";
            this.proxyPanel.Name = "proxyPanel";
            this.proxyPanel.Port = "8080";
            this.proxyPanel.Size = new System.Drawing.Size(468, 30);
            this.proxyPanel.TabIndex = 10;
            this.proxyPanel.OnStarted += new System.EventHandler(this.proxyPanel_Started);
            // 
            // MasterProxyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(576, 281);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.forwardedCountLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.localAddressLabel);
            this.Controls.Add(this.udpPortBox);
            this.Controls.Add(this.forwardLoginCheck);
            this.Controls.Add(this.selectAll);
            this.Controls.Add(this.packetList);
            this.Controls.Add(this.proxyPanel);
            this.Controls.Add(this.packetCountLabel);
            this.Controls.Add(this.slavesListBox);
            this.Name = "MasterProxyForm";
            this.Text = "Master Proxy";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MasterProxyForm_FormClosing);
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
    }
}


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
            this.proxyPanel = new UtilLib.ProxyPanel();
            this.forwardLoginCheck = new System.Windows.Forms.CheckBox();
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
            this.packetCountLabel.Location = new System.Drawing.Point(12, 260);
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
            this.packetList.Size = new System.Drawing.Size(444, 212);
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
            this.selectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.selectAll.AutoSize = true;
            this.selectAll.Location = new System.Drawing.Point(171, 259);
            this.selectAll.Name = "selectAll";
            this.selectAll.Size = new System.Drawing.Size(120, 17);
            this.selectAll.TabIndex = 12;
            this.selectAll.Text = "Check/Uncheck All";
            this.selectAll.UseVisualStyleBackColor = true;
            this.selectAll.CheckedChanged += new System.EventHandler(this.selectAll_CheckedChanged);
            // 
            // proxyPanel
            // 
            this.proxyPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.proxyPanel.ListenIP = "127.0.0.1";
            this.proxyPanel.Location = new System.Drawing.Point(1, 5);
            this.proxyPanel.LoginURI = "http://localhost:9000";
            this.proxyPanel.Name = "proxyPanel";
            this.proxyPanel.Port = "8080";
            this.proxyPanel.Size = new System.Drawing.Size(628, 30);
            this.proxyPanel.TabIndex = 10;
            this.proxyPanel.OnStarted += new System.EventHandler(this.proxyPanel_Started);
            // 
            // forwardLoginCheck
            // 
            this.forwardLoginCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.forwardLoginCheck.AutoSize = true;
            this.forwardLoginCheck.Location = new System.Drawing.Point(72, 259);
            this.forwardLoginCheck.Name = "forwardLoginCheck";
            this.forwardLoginCheck.Size = new System.Drawing.Size(93, 17);
            this.forwardLoginCheck.TabIndex = 13;
            this.forwardLoginCheck.Text = "Forward Login";
            this.forwardLoginCheck.UseVisualStyleBackColor = true;
            this.forwardLoginCheck.CheckedChanged += new System.EventHandler(this.forwardLoginCheck_CheckedChanged);
            // 
            // MasterProxyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(627, 281);
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
    }
}


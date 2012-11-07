/*************************************************************************
Copyright (c) 2012 John McCaffery 

This file is part of Armadillo ClientProxy.

Routing Project is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Routing Project is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Routing Project.  If not, see <http://www.gnu.org/licenses/>.

**************************************************************************/

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
            this.tabContainer = new System.Windows.Forms.TabControl();
            this.proxyTab = new System.Windows.Forms.TabPage();
            this.proxyPanel = new UtilLib.ProxyPanel();
            this.configTab = new System.Windows.Forms.TabPage();
            this.controlTab = new System.Windows.Forms.TabPage();
            this.rotationPanel = new ProxyTestGUI.RotationPanel();
            this.positionPanel = new ProxyTestGUI.VectorPanel();
            this.mouseTab = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.mouseScaleSlider = new System.Windows.Forms.TrackBar();
            this.moveScaleSlider = new System.Windows.Forms.TrackBar();
            this.yawLabel = new System.Windows.Forms.Label();
            this.pitchLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.createdCountLabel = new System.Windows.Forms.Label();
            this.debugTab = new System.Windows.Forms.TabPage();
            this.logPanel1 = new UtilLib.LogPanel();
            this.tabContainer.SuspendLayout();
            this.proxyTab.SuspendLayout();
            this.configTab.SuspendLayout();
            this.controlTab.SuspendLayout();
            this.mouseTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mouseScaleSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.moveScaleSlider)).BeginInit();
            this.debugTab.SuspendLayout();
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
            this.udpPortBox.Location = new System.Drawing.Point(64, 272);
            this.udpPortBox.Name = "udpPortBox";
            this.udpPortBox.Size = new System.Drawing.Size(32, 20);
            this.udpPortBox.TabIndex = 14;
            this.udpPortBox.Text = "8090";
            // 
            // localAddressLabel
            // 
            this.localAddressLabel.AutoSize = true;
            this.localAddressLabel.Location = new System.Drawing.Point(7, 275);
            this.localAddressLabel.Name = "localAddressLabel";
            this.localAddressLabel.Size = new System.Drawing.Size(52, 13);
            this.localAddressLabel.TabIndex = 15;
            this.localAddressLabel.Text = "UDP SlavePort";
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
            // tabContainer
            // 
            this.tabContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabContainer.Controls.Add(this.proxyTab);
            this.tabContainer.Controls.Add(this.configTab);
            this.tabContainer.Controls.Add(this.controlTab);
            this.tabContainer.Controls.Add(this.mouseTab);
            this.tabContainer.Controls.Add(this.debugTab);
            this.tabContainer.Location = new System.Drawing.Point(1, 4);
            this.tabContainer.Name = "tabContainer";
            this.tabContainer.SelectedIndex = 0;
            this.tabContainer.Size = new System.Drawing.Size(640, 355);
            this.tabContainer.TabIndex = 19;
            this.tabContainer.KeyDown += new System.Windows.Forms.KeyEventHandler(this.keyDown);
            this.tabContainer.KeyUp += new System.Windows.Forms.KeyEventHandler(this.keyUp);
            // 
            // proxyTab
            // 
            this.proxyTab.Controls.Add(this.proxyPanel);
            this.proxyTab.Controls.Add(this.localAddressLabel);
            this.proxyTab.Controls.Add(this.udpPortBox);
            this.proxyTab.Location = new System.Drawing.Point(4, 22);
            this.proxyTab.Name = "proxyTab";
            this.proxyTab.Padding = new System.Windows.Forms.Padding(3);
            this.proxyTab.Size = new System.Drawing.Size(632, 329);
            this.proxyTab.TabIndex = 2;
            this.proxyTab.Text = "ClientProxy";
            this.proxyTab.UseVisualStyleBackColor = true;
            // 
            // proxyPanel
            // 
            this.proxyPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.proxyPanel.FirstName = "Routing";
            this.proxyPanel.LastName = "God";
            this.proxyPanel.ListenIP = "127.0.0.1";
            this.proxyPanel.Location = new System.Drawing.Point(3, 3);
            this.proxyPanel.LoginURI = "http://apollo.cs.st-andrews.ac.uk:8002";
            this.proxyPanel.Name = "proxyPanel";
            this.proxyPanel.Password = "1245";
            this.proxyPanel.Port = "8080";
            this.proxyPanel.Size = new System.Drawing.Size(629, 197);
            this.proxyPanel.TabIndex = 10;
            this.proxyPanel.OnStarted += new System.EventHandler(this.proxyPanel_Started);
            this.proxyPanel.KeyDown += new System.Windows.Forms.KeyEventHandler(this.keyDown);
            this.proxyPanel.KeyUp += new System.Windows.Forms.KeyEventHandler(this.keyUp);
            // 
            // configTab
            // 
            this.configTab.Controls.Add(this.slavesListBox);
            this.configTab.Controls.Add(this.packetList);
            this.configTab.Location = new System.Drawing.Point(4, 22);
            this.configTab.Name = "configTab";
            this.configTab.Padding = new System.Windows.Forms.Padding(3);
            this.configTab.Size = new System.Drawing.Size(632, 329);
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
            this.controlTab.Size = new System.Drawing.Size(632, 329);
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
            this.positionPanel.DisplayName = "LookAt";
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
            // mouseTab
            // 
            this.mouseTab.Controls.Add(this.label5);
            this.mouseTab.Controls.Add(this.label4);
            this.mouseTab.Controls.Add(this.mouseScaleSlider);
            this.mouseTab.Controls.Add(this.moveScaleSlider);
            this.mouseTab.Controls.Add(this.yawLabel);
            this.mouseTab.Controls.Add(this.pitchLabel);
            this.mouseTab.Location = new System.Drawing.Point(4, 22);
            this.mouseTab.Name = "mouseTab";
            this.mouseTab.Padding = new System.Windows.Forms.Padding(3);
            this.mouseTab.Size = new System.Drawing.Size(632, 329);
            this.mouseTab.TabIndex = 3;
            this.mouseTab.Text = "Mouse";
            this.mouseTab.UseVisualStyleBackColor = true;
            this.mouseTab.Paint += new System.Windows.Forms.PaintEventHandler(this.mouseTab_Paint);
            this.mouseTab.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mouseTab_MouseDown);
            this.mouseTab.MouseMove += new System.Windows.Forms.MouseEventHandler(this.mouseTab_MouseMove);
            this.mouseTab.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseTab_MouseUp);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 233);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(84, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Move Sensitivity";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 281);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Mouse Sensitivity";
            // 
            // mouseScaleSlider
            // 
            this.mouseScaleSlider.Location = new System.Drawing.Point(101, 281);
            this.mouseScaleSlider.Maximum = 40;
            this.mouseScaleSlider.Minimum = 10;
            this.mouseScaleSlider.Name = "mouseScaleSlider";
            this.mouseScaleSlider.Size = new System.Drawing.Size(531, 42);
            this.mouseScaleSlider.TabIndex = 2;
            this.mouseScaleSlider.Value = 20;
            // 
            // moveScaleSlider
            // 
            this.moveScaleSlider.Location = new System.Drawing.Point(101, 233);
            this.moveScaleSlider.Maximum = 40;
            this.moveScaleSlider.Minimum = 10;
            this.moveScaleSlider.Name = "moveScaleSlider";
            this.moveScaleSlider.Size = new System.Drawing.Size(531, 42);
            this.moveScaleSlider.TabIndex = 1;
            this.moveScaleSlider.Value = 20;
            // 
            // yawLabel
            // 
            this.yawLabel.AutoSize = true;
            this.yawLabel.Location = new System.Drawing.Point(6, 20);
            this.yawLabel.Name = "yawLabel";
            this.yawLabel.Size = new System.Drawing.Size(31, 13);
            this.yawLabel.TabIndex = 1;
            this.yawLabel.Text = "Yaw:";
            // 
            // pitchLabel
            // 
            this.pitchLabel.AutoSize = true;
            this.pitchLabel.Location = new System.Drawing.Point(7, 7);
            this.pitchLabel.Name = "pitchLabel";
            this.pitchLabel.Size = new System.Drawing.Size(34, 13);
            this.pitchLabel.TabIndex = 0;
            this.pitchLabel.Text = "Pitch:";
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
            // debugTab
            // 
            this.debugTab.Controls.Add(this.logPanel1);
            this.debugTab.Location = new System.Drawing.Point(4, 22);
            this.debugTab.Name = "debugTab";
            this.debugTab.Padding = new System.Windows.Forms.Padding(3);
            this.debugTab.Size = new System.Drawing.Size(632, 329);
            this.debugTab.TabIndex = 4;
            this.debugTab.Text = "Debug";
            this.debugTab.UseVisualStyleBackColor = true;
            // 
            // logPanel1
            // 
            this.logPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logPanel1.Location = new System.Drawing.Point(3, 3);
            this.logPanel1.Name = "logPanel1";
            this.logPanel1.Size = new System.Drawing.Size(626, 323);
            this.logPanel1.TabIndex = 0;
            // 
            // MasterProxyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(644, 386);
            this.Controls.Add(this.createdCountLabel);
            this.Controls.Add(this.tabContainer);
            this.Controls.Add(this.forwardedCountLabel);
            this.Controls.Add(this.forwardLoginCheck);
            this.Controls.Add(this.selectAll);
            this.Controls.Add(this.packetCountLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Name = "MasterProxyForm";
            this.Text = "Master ClientProxy";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MasterProxyForm_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.keyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.keyUp);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MasterProxyForm_MouseMove);
            this.tabContainer.ResumeLayout(false);
            this.proxyTab.ResumeLayout(false);
            this.proxyTab.PerformLayout();
            this.configTab.ResumeLayout(false);
            this.controlTab.ResumeLayout(false);
            this.mouseTab.ResumeLayout(false);
            this.mouseTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mouseScaleSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.moveScaleSlider)).EndInit();
            this.debugTab.ResumeLayout(false);
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
        private System.Windows.Forms.TabPage proxyTab;
        private System.Windows.Forms.TabPage mouseTab;
        private System.Windows.Forms.Label yawLabel;
        private System.Windows.Forms.Label pitchLabel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TrackBar mouseScaleSlider;
        private System.Windows.Forms.TrackBar moveScaleSlider;
        private System.Windows.Forms.TabPage debugTab;
        private UtilLib.LogPanel logPanel1;
    }
}


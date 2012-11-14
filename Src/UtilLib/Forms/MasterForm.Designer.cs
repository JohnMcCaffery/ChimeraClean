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
            this.slavesTab = new System.Windows.Forms.TabControl();
            this.rawTab = new System.Windows.Forms.TabPage();
            this.rawPosition = new ProxyTestGUI.VectorPanel();
            this.rawRotation = new ProxyTestGUI.RotationPanel();
            this.visualSlavesSplit = new System.Windows.Forms.SplitContainer();
            this.displayTab = new System.Windows.Forms.TabControl();
            this.bothTab = new System.Windows.Forms.TabPage();
            this.hvSplit = new System.Windows.Forms.SplitContainer();
            this.hBox = new System.Windows.Forms.GroupBox();
            this.vBox = new System.Windows.Forms.GroupBox();
            this.proxyTab = new System.Windows.Forms.TabPage();
            this.proxyPanel = new UtilLib.ProxyPanel();
            this.networkTab = new System.Windows.Forms.TabPage();
            this.debugTab = new System.Windows.Forms.TabPage();
            this.debugPanel = new UtilLib.LogPanel();
            this.portBox = new System.Windows.Forms.MaskedTextBox();
            this.portLabel = new System.Windows.Forms.Label();
            this.bindButton = new System.Windows.Forms.Button();
            this.statusLabel = new System.Windows.Forms.Label();
            this.generatedLabel = new System.Windows.Forms.Label();
            this.forwardedLabel = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.processedLabel = new System.Windows.Forms.Label();
            this.receivedLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.slavesTab.SuspendLayout();
            this.rawTab.SuspendLayout();
            this.visualSlavesSplit.Panel1.SuspendLayout();
            this.visualSlavesSplit.Panel2.SuspendLayout();
            this.visualSlavesSplit.SuspendLayout();
            this.displayTab.SuspendLayout();
            this.bothTab.SuspendLayout();
            this.hvSplit.Panel1.SuspendLayout();
            this.hvSplit.Panel2.SuspendLayout();
            this.hvSplit.SuspendLayout();
            this.proxyTab.SuspendLayout();
            this.networkTab.SuspendLayout();
            this.debugTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // slavesTab
            // 
            this.slavesTab.Controls.Add(this.rawTab);
            this.slavesTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.slavesTab.Location = new System.Drawing.Point(0, 0);
            this.slavesTab.Name = "slavesTab";
            this.slavesTab.SelectedIndex = 0;
            this.slavesTab.Size = new System.Drawing.Size(617, 284);
            this.slavesTab.TabIndex = 0;
            // 
            // rawTab
            // 
            this.rawTab.AutoScroll = true;
            this.rawTab.Controls.Add(this.rawPosition);
            this.rawTab.Controls.Add(this.rawRotation);
            this.rawTab.Location = new System.Drawing.Point(4, 22);
            this.rawTab.Name = "rawTab";
            this.rawTab.Padding = new System.Windows.Forms.Padding(3);
            this.rawTab.Size = new System.Drawing.Size(609, 258);
            this.rawTab.TabIndex = 0;
            this.rawTab.Text = "Input Values";
            this.rawTab.UseVisualStyleBackColor = true;
            // 
            // rawPosition
            // 
            this.rawPosition.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rawPosition.DisplayName = "Raw Position";
            this.rawPosition.Location = new System.Drawing.Point(0, 153);
            this.rawPosition.Max = 2048D;
            this.rawPosition.Min = -2048D;
            this.rawPosition.Name = "rawPosition";
            this.rawPosition.Size = new System.Drawing.Size(601, 98);
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
            this.rawRotation.Size = new System.Drawing.Size(601, 147);
            this.rawRotation.TabIndex = 0;
            this.rawRotation.Yaw = 0F;
            this.rawRotation.OnChange += new System.EventHandler(this.rawRotation_OnChange);
            // 
            // visualSlavesSplit
            // 
            this.visualSlavesSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.visualSlavesSplit.Location = new System.Drawing.Point(0, 0);
            this.visualSlavesSplit.Name = "visualSlavesSplit";
            this.visualSlavesSplit.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // visualSlavesSplit.Panel1
            // 
            this.visualSlavesSplit.Panel1.Controls.Add(this.displayTab);
            // 
            // visualSlavesSplit.Panel2
            // 
            this.visualSlavesSplit.Panel2.AutoScroll = true;
            this.visualSlavesSplit.Panel2.Controls.Add(this.slavesTab);
            this.visualSlavesSplit.Size = new System.Drawing.Size(617, 457);
            this.visualSlavesSplit.SplitterDistance = 169;
            this.visualSlavesSplit.TabIndex = 1;
            // 
            // displayTab
            // 
            this.displayTab.Controls.Add(this.bothTab);
            this.displayTab.Controls.Add(this.proxyTab);
            this.displayTab.Controls.Add(this.networkTab);
            this.displayTab.Controls.Add(this.debugTab);
            this.displayTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.displayTab.Location = new System.Drawing.Point(0, 0);
            this.displayTab.Name = "displayTab";
            this.displayTab.SelectedIndex = 0;
            this.displayTab.Size = new System.Drawing.Size(617, 169);
            this.displayTab.TabIndex = 0;
            // 
            // bothTab
            // 
            this.bothTab.Controls.Add(this.hvSplit);
            this.bothTab.Location = new System.Drawing.Point(4, 22);
            this.bothTab.Name = "bothTab";
            this.bothTab.Padding = new System.Windows.Forms.Padding(3);
            this.bothTab.Size = new System.Drawing.Size(609, 143);
            this.bothTab.TabIndex = 2;
            this.bothTab.Text = "Horizontal and Vertical";
            this.bothTab.UseVisualStyleBackColor = true;
            // 
            // hvSplit
            // 
            this.hvSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hvSplit.Location = new System.Drawing.Point(3, 3);
            this.hvSplit.Name = "hvSplit";
            // 
            // hvSplit.Panel1
            // 
            this.hvSplit.Panel1.Controls.Add(this.hBox);
            // 
            // hvSplit.Panel2
            // 
            this.hvSplit.Panel2.Controls.Add(this.vBox);
            this.hvSplit.Size = new System.Drawing.Size(603, 137);
            this.hvSplit.SplitterDistance = 293;
            this.hvSplit.TabIndex = 0;
            // 
            // hBox
            // 
            this.hBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hBox.Location = new System.Drawing.Point(0, 0);
            this.hBox.Name = "hBox";
            this.hBox.Size = new System.Drawing.Size(293, 137);
            this.hBox.TabIndex = 0;
            this.hBox.TabStop = false;
            this.hBox.Text = "Horizontal";
            this.hBox.Paint += new System.Windows.Forms.PaintEventHandler(this.hTab_Paint);
            // 
            // vBox
            // 
            this.vBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vBox.Location = new System.Drawing.Point(0, 0);
            this.vBox.Name = "vBox";
            this.vBox.Size = new System.Drawing.Size(306, 137);
            this.vBox.TabIndex = 0;
            this.vBox.TabStop = false;
            this.vBox.Text = "Vertical";
            this.vBox.Paint += new System.Windows.Forms.PaintEventHandler(this.vTab_Paint);
            // 
            // proxyTab
            // 
            this.proxyTab.Controls.Add(this.proxyPanel);
            this.proxyTab.Location = new System.Drawing.Point(4, 22);
            this.proxyTab.Name = "proxyTab";
            this.proxyTab.Padding = new System.Windows.Forms.Padding(3);
            this.proxyTab.Size = new System.Drawing.Size(609, 196);
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
            this.proxyPanel.Name = "proxyPanel";
            this.proxyPanel.Password = "1245";
            this.proxyPanel.Port = "8080";
            this.proxyPanel.Proxy = null;
            this.proxyPanel.Size = new System.Drawing.Size(603, 190);
            this.proxyPanel.TabIndex = 0;
            // 
            // networkTab
            // 
            this.networkTab.Controls.Add(this.generatedLabel);
            this.networkTab.Controls.Add(this.forwardedLabel);
            this.networkTab.Controls.Add(this.label6);
            this.networkTab.Controls.Add(this.label7);
            this.networkTab.Controls.Add(this.processedLabel);
            this.networkTab.Controls.Add(this.receivedLabel);
            this.networkTab.Controls.Add(this.label4);
            this.networkTab.Controls.Add(this.label1);
            this.networkTab.Controls.Add(this.statusLabel);
            this.networkTab.Controls.Add(this.bindButton);
            this.networkTab.Controls.Add(this.portBox);
            this.networkTab.Controls.Add(this.portLabel);
            this.networkTab.Location = new System.Drawing.Point(4, 22);
            this.networkTab.Name = "networkTab";
            this.networkTab.Padding = new System.Windows.Forms.Padding(3);
            this.networkTab.Size = new System.Drawing.Size(609, 143);
            this.networkTab.TabIndex = 4;
            this.networkTab.Text = "Network";
            this.networkTab.UseVisualStyleBackColor = true;
            // 
            // debugTab
            // 
            this.debugTab.Controls.Add(this.debugPanel);
            this.debugTab.Location = new System.Drawing.Point(4, 22);
            this.debugTab.Name = "debugTab";
            this.debugTab.Padding = new System.Windows.Forms.Padding(3);
            this.debugTab.Size = new System.Drawing.Size(609, 196);
            this.debugTab.TabIndex = 5;
            this.debugTab.Text = "Debug";
            this.debugTab.UseVisualStyleBackColor = true;
            // 
            // debugPanel
            // 
            this.debugPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.debugPanel.Location = new System.Drawing.Point(3, 3);
            this.debugPanel.Name = "debugPanel";
            this.debugPanel.Size = new System.Drawing.Size(603, 190);
            this.debugPanel.TabIndex = 0;
            // 
            // portBox
            // 
            this.portBox.Location = new System.Drawing.Point(35, 8);
            this.portBox.Mask = "0000#";
            this.portBox.Name = "portBox";
            this.portBox.Size = new System.Drawing.Size(50, 20);
            this.portBox.TabIndex = 12;
            this.portBox.Text = "8090";
            // 
            // portLabel
            // 
            this.portLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.portLabel.AutoSize = true;
            this.portLabel.Location = new System.Drawing.Point(3, 11);
            this.portLabel.Name = "portLabel";
            this.portLabel.Size = new System.Drawing.Size(26, 13);
            this.portLabel.TabIndex = 11;
            this.portLabel.Text = "Port";
            // 
            // bindButton
            // 
            this.bindButton.Location = new System.Drawing.Point(91, 6);
            this.bindButton.Name = "bindButton";
            this.bindButton.Size = new System.Drawing.Size(61, 23);
            this.bindButton.TabIndex = 13;
            this.bindButton.Text = "Bind";
            this.bindButton.UseVisualStyleBackColor = true;
            this.bindButton.Click += new System.EventHandler(this.bindButton_Click);
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(158, 11);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(51, 13);
            this.statusLabel.TabIndex = 14;
            this.statusLabel.Text = "Unbound";
            // 
            // generatedLabel
            // 
            this.generatedLabel.AutoSize = true;
            this.generatedLabel.Location = new System.Drawing.Point(111, 71);
            this.generatedLabel.Name = "generatedLabel";
            this.generatedLabel.Size = new System.Drawing.Size(13, 13);
            this.generatedLabel.TabIndex = 26;
            this.generatedLabel.Text = "0";
            // 
            // forwardedLabel
            // 
            this.forwardedLabel.AutoSize = true;
            this.forwardedLabel.Location = new System.Drawing.Point(111, 58);
            this.forwardedLabel.Name = "forwardedLabel";
            this.forwardedLabel.Size = new System.Drawing.Size(13, 13);
            this.forwardedLabel.TabIndex = 25;
            this.forwardedLabel.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 71);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(105, 13);
            this.label6.TabIndex = 24;
            this.label6.Text = "Packets Generated: ";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 58);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(105, 13);
            this.label7.TabIndex = 23;
            this.label7.Text = "Packets Forwarded: ";
            // 
            // processedLabel
            // 
            this.processedLabel.AutoSize = true;
            this.processedLabel.Location = new System.Drawing.Point(111, 45);
            this.processedLabel.Name = "processedLabel";
            this.processedLabel.Size = new System.Drawing.Size(13, 13);
            this.processedLabel.TabIndex = 22;
            this.processedLabel.Text = "0";
            // 
            // receivedLabel
            // 
            this.receivedLabel.AutoSize = true;
            this.receivedLabel.Location = new System.Drawing.Point(111, 32);
            this.receivedLabel.Name = "receivedLabel";
            this.receivedLabel.Size = new System.Drawing.Size(13, 13);
            this.receivedLabel.TabIndex = 21;
            this.receivedLabel.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(105, 13);
            this.label4.TabIndex = 20;
            this.label4.Text = "Packets Processed: ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "Packets Recieved: ";
            // 
            // MasterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(617, 457);
            this.Controls.Add(this.visualSlavesSplit);
            this.Name = "MasterForm";
            this.Text = "MasterForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MasterForm_FormClosing);
            this.slavesTab.ResumeLayout(false);
            this.rawTab.ResumeLayout(false);
            this.visualSlavesSplit.Panel1.ResumeLayout(false);
            this.visualSlavesSplit.Panel2.ResumeLayout(false);
            this.visualSlavesSplit.ResumeLayout(false);
            this.displayTab.ResumeLayout(false);
            this.bothTab.ResumeLayout(false);
            this.hvSplit.Panel1.ResumeLayout(false);
            this.hvSplit.Panel2.ResumeLayout(false);
            this.hvSplit.ResumeLayout(false);
            this.proxyTab.ResumeLayout(false);
            this.networkTab.ResumeLayout(false);
            this.networkTab.PerformLayout();
            this.debugTab.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl slavesTab;
        private System.Windows.Forms.TabPage rawTab;
        private ProxyTestGUI.VectorPanel rawPosition;
        private ProxyTestGUI.RotationPanel rawRotation;
        private System.Windows.Forms.SplitContainer visualSlavesSplit;
        private System.Windows.Forms.TabControl displayTab;
        private System.Windows.Forms.TabPage bothTab;
        private System.Windows.Forms.SplitContainer hvSplit;
        private System.Windows.Forms.GroupBox hBox;
        private System.Windows.Forms.GroupBox vBox;
        private System.Windows.Forms.TabPage proxyTab;
        private UtilLib.ProxyPanel proxyPanel;
        private System.Windows.Forms.TabPage networkTab;
        private System.Windows.Forms.TabPage debugTab;
        private UtilLib.LogPanel debugPanel;
        private System.Windows.Forms.MaskedTextBox portBox;
        private System.Windows.Forms.Label portLabel;
        private System.Windows.Forms.Button bindButton;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Label generatedLabel;
        private System.Windows.Forms.Label forwardedLabel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label processedLabel;
        private System.Windows.Forms.Label receivedLabel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
    }
}
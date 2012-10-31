/*************************************************************************
Copyright (c) 2012 John McCaffery 

This file is part of Armadillo Proxy.

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

namespace UtilLib {
    partial class ProxyPanel {
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.loginURIBox = new System.Windows.Forms.TextBox();
            this.loginURILabel = new System.Windows.Forms.Label();
            this.listenIPBox = new System.Windows.Forms.TextBox();
            this.listenIPLabel = new System.Windows.Forms.Label();
            this.portBox = new System.Windows.Forms.TextBox();
            this.portLabel = new System.Windows.Forms.Label();
            this.proxyStartButton = new System.Windows.Forms.Button();
            this.updateTimer = new System.Windows.Forms.Timer(this.components);
            this.clientStartButton = new System.Windows.Forms.Button();
            this.firstNameBox = new System.Windows.Forms.TextBox();
            this.firstNameLabel = new System.Windows.Forms.Label();
            this.lastNameLabel = new System.Windows.Forms.Label();
            this.lastNameBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.passwordBox = new System.Windows.Forms.TextBox();
            this.targetBox = new System.Windows.Forms.TextBox();
            this.clientLabel = new System.Windows.Forms.Label();
            this.proxyStatusLabelLabel = new System.Windows.Forms.Label();
            this.proxyStatusLabel = new System.Windows.Forms.Label();
            this.clientStatusLabel = new System.Windows.Forms.Label();
            this.clientStatusLabelLabel = new System.Windows.Forms.Label();
            this.useLoginURICheck = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // loginURIBox
            // 
            this.loginURIBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.loginURIBox.Location = new System.Drawing.Point(64, 5);
            this.loginURIBox.Name = "loginURIBox";
            this.loginURIBox.Size = new System.Drawing.Size(616, 20);
            this.loginURIBox.TabIndex = 16;
            this.loginURIBox.Text = "http://apollo.cs.st-andrews.ac.uk:8002";
            // 
            // loginURILabel
            // 
            this.loginURILabel.AutoSize = true;
            this.loginURILabel.Location = new System.Drawing.Point(3, 8);
            this.loginURILabel.Name = "loginURILabel";
            this.loginURILabel.Size = new System.Drawing.Size(55, 13);
            this.loginURILabel.TabIndex = 15;
            this.loginURILabel.Text = "Login URI";
            // 
            // listenIPBox
            // 
            this.listenIPBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listenIPBox.Location = new System.Drawing.Point(57, 29);
            this.listenIPBox.Name = "listenIPBox";
            this.listenIPBox.Size = new System.Drawing.Size(546, 20);
            this.listenIPBox.TabIndex = 14;
            this.listenIPBox.Text = "127.0.0.1";
            // 
            // listenIPLabel
            // 
            this.listenIPLabel.AutoSize = true;
            this.listenIPLabel.Location = new System.Drawing.Point(3, 32);
            this.listenIPLabel.Name = "listenIPLabel";
            this.listenIPLabel.Size = new System.Drawing.Size(48, 13);
            this.listenIPLabel.TabIndex = 13;
            this.listenIPLabel.Text = "Listen IP";
            // 
            // portBox
            // 
            this.portBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.portBox.Location = new System.Drawing.Point(641, 29);
            this.portBox.Name = "portBox";
            this.portBox.Size = new System.Drawing.Size(39, 20);
            this.portBox.TabIndex = 12;
            this.portBox.Text = "8080";
            // 
            // portLabel
            // 
            this.portLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.portLabel.AutoSize = true;
            this.portLabel.Location = new System.Drawing.Point(609, 32);
            this.portLabel.Name = "portLabel";
            this.portLabel.Size = new System.Drawing.Size(26, 13);
            this.portLabel.TabIndex = 11;
            this.portLabel.Text = "Port";
            // 
            // proxyStartButton
            // 
            this.proxyStartButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.proxyStartButton.Location = new System.Drawing.Point(6, 55);
            this.proxyStartButton.Name = "proxyStartButton";
            this.proxyStartButton.Size = new System.Drawing.Size(674, 20);
            this.proxyStartButton.TabIndex = 18;
            this.proxyStartButton.Text = "Start Proxy";
            this.proxyStartButton.UseVisualStyleBackColor = true;
            this.proxyStartButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // updateTimer
            // 
            this.updateTimer.Enabled = true;
            this.updateTimer.Tick += new System.EventHandler(this.updateTimer_Tick);
            // 
            // clientStartButton
            // 
            this.clientStartButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.clientStartButton.Location = new System.Drawing.Point(6, 133);
            this.clientStartButton.Name = "clientStartButton";
            this.clientStartButton.Size = new System.Drawing.Size(674, 23);
            this.clientStartButton.TabIndex = 19;
            this.clientStartButton.Text = "Start Client";
            this.clientStartButton.UseVisualStyleBackColor = true;
            this.clientStartButton.Click += new System.EventHandler(this.clientStartButton_Click);
            // 
            // firstNameBox
            // 
            this.firstNameBox.Location = new System.Drawing.Point(64, 81);
            this.firstNameBox.Name = "firstNameBox";
            this.firstNameBox.Size = new System.Drawing.Size(116, 20);
            this.firstNameBox.TabIndex = 20;
            this.firstNameBox.Text = "Routing";
            // 
            // firstNameLabel
            // 
            this.firstNameLabel.AutoSize = true;
            this.firstNameLabel.Location = new System.Drawing.Point(3, 84);
            this.firstNameLabel.Name = "firstNameLabel";
            this.firstNameLabel.Size = new System.Drawing.Size(57, 13);
            this.firstNameLabel.TabIndex = 21;
            this.firstNameLabel.Text = "First Name";
            // 
            // lastNameLabel
            // 
            this.lastNameLabel.AutoSize = true;
            this.lastNameLabel.Location = new System.Drawing.Point(186, 84);
            this.lastNameLabel.Name = "lastNameLabel";
            this.lastNameLabel.Size = new System.Drawing.Size(58, 13);
            this.lastNameLabel.TabIndex = 23;
            this.lastNameLabel.Text = "Last Name";
            // 
            // lastNameBox
            // 
            this.lastNameBox.Location = new System.Drawing.Point(250, 81);
            this.lastNameBox.Name = "lastNameBox";
            this.lastNameBox.Size = new System.Drawing.Size(122, 20);
            this.lastNameBox.TabIndex = 22;
            this.lastNameBox.Text = "God";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(378, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 25;
            this.label1.Text = "Password";
            // 
            // passwordBox
            // 
            this.passwordBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.passwordBox.Location = new System.Drawing.Point(437, 81);
            this.passwordBox.Name = "passwordBox";
            this.passwordBox.PasswordChar = '*';
            this.passwordBox.Size = new System.Drawing.Size(243, 20);
            this.passwordBox.TabIndex = 24;
            this.passwordBox.Text = "1245";
            // 
            // targetBox
            // 
            this.targetBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.targetBox.Location = new System.Drawing.Point(42, 107);
            this.targetBox.Name = "targetBox";
            this.targetBox.Size = new System.Drawing.Size(638, 20);
            this.targetBox.TabIndex = 26;
            this.targetBox.Text = "C:\\\\Program Files (x86)\\\\Firestorm-Release\\\\Firestorm-Release.exe";
            // 
            // clientLabel
            // 
            this.clientLabel.AutoSize = true;
            this.clientLabel.Location = new System.Drawing.Point(3, 110);
            this.clientLabel.Name = "clientLabel";
            this.clientLabel.Size = new System.Drawing.Size(33, 13);
            this.clientLabel.TabIndex = 27;
            this.clientLabel.Text = "Client";
            // 
            // proxyStatusLabelLabel
            // 
            this.proxyStatusLabelLabel.AutoSize = true;
            this.proxyStatusLabelLabel.Location = new System.Drawing.Point(3, 159);
            this.proxyStatusLabelLabel.Name = "proxyStatusLabelLabel";
            this.proxyStatusLabelLabel.Size = new System.Drawing.Size(72, 13);
            this.proxyStatusLabelLabel.TabIndex = 28;
            this.proxyStatusLabelLabel.Text = "Proxy Status: ";
            // 
            // proxyStatusLabel
            // 
            this.proxyStatusLabel.AutoSize = true;
            this.proxyStatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.proxyStatusLabel.Location = new System.Drawing.Point(81, 159);
            this.proxyStatusLabel.Name = "proxyStatusLabel";
            this.proxyStatusLabel.Size = new System.Drawing.Size(28, 13);
            this.proxyStatusLabel.TabIndex = 29;
            this.proxyStatusLabel.Text = "Idle";
            // 
            // clientStatusLabel
            // 
            this.clientStatusLabel.AutoSize = true;
            this.clientStatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clientStatusLabel.Location = new System.Drawing.Point(81, 172);
            this.clientStatusLabel.Name = "clientStatusLabel";
            this.clientStatusLabel.Size = new System.Drawing.Size(28, 13);
            this.clientStatusLabel.TabIndex = 31;
            this.clientStatusLabel.Text = "Idle";
            // 
            // clientStatusLabelLabel
            // 
            this.clientStatusLabelLabel.AutoSize = true;
            this.clientStatusLabelLabel.Location = new System.Drawing.Point(3, 172);
            this.clientStatusLabelLabel.Name = "clientStatusLabelLabel";
            this.clientStatusLabelLabel.Size = new System.Drawing.Size(72, 13);
            this.clientStatusLabelLabel.TabIndex = 30;
            this.clientStatusLabelLabel.Text = "Client Status: ";
            // 
            // checkBox1
            // 
            this.useLoginURICheck.AutoSize = true;
            this.useLoginURICheck.Location = new System.Drawing.Point(250, 162);
            this.useLoginURICheck.Name = "checkBox1";
            this.useLoginURICheck.Size = new System.Drawing.Size(93, 17);
            this.useLoginURICheck.TabIndex = 32;
            this.useLoginURICheck.Text = "Use LoginURI";
            this.useLoginURICheck.UseVisualStyleBackColor = true;
            // 
            // ProxyPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.useLoginURICheck);
            this.Controls.Add(this.clientStatusLabel);
            this.Controls.Add(this.clientStatusLabelLabel);
            this.Controls.Add(this.proxyStatusLabel);
            this.Controls.Add(this.proxyStatusLabelLabel);
            this.Controls.Add(this.clientLabel);
            this.Controls.Add(this.targetBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.passwordBox);
            this.Controls.Add(this.lastNameLabel);
            this.Controls.Add(this.lastNameBox);
            this.Controls.Add(this.firstNameLabel);
            this.Controls.Add(this.firstNameBox);
            this.Controls.Add(this.clientStartButton);
            this.Controls.Add(this.proxyStartButton);
            this.Controls.Add(this.loginURIBox);
            this.Controls.Add(this.loginURILabel);
            this.Controls.Add(this.listenIPBox);
            this.Controls.Add(this.listenIPLabel);
            this.Controls.Add(this.portBox);
            this.Controls.Add(this.portLabel);
            this.Name = "ProxyPanel";
            this.Size = new System.Drawing.Size(683, 283);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox loginURIBox;
        private System.Windows.Forms.Label loginURILabel;
        private System.Windows.Forms.TextBox listenIPBox;
        private System.Windows.Forms.Label listenIPLabel;
        private System.Windows.Forms.TextBox portBox;
        private System.Windows.Forms.Label portLabel;
        private System.Windows.Forms.Button proxyStartButton;
        private System.Windows.Forms.Timer updateTimer;
        private System.Windows.Forms.Button clientStartButton;
        private System.Windows.Forms.TextBox firstNameBox;
        private System.Windows.Forms.Label firstNameLabel;
        private System.Windows.Forms.Label lastNameLabel;
        private System.Windows.Forms.TextBox lastNameBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox passwordBox;
        private System.Windows.Forms.TextBox targetBox;
        private System.Windows.Forms.Label clientLabel;
        private System.Windows.Forms.Label proxyStatusLabelLabel;
        private System.Windows.Forms.Label proxyStatusLabel;
        private System.Windows.Forms.Label clientStatusLabel;
        private System.Windows.Forms.Label clientStatusLabelLabel;
        private System.Windows.Forms.CheckBox useLoginURICheck;
    }
}

/*************************************************************************
Copyright (c) 2012 John McCaffery 

This file is part of Armadillo SlaveProxy.

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

namespace Chimera.OpenSim.GUI {
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
            this.loginURIBox = new System.Windows.Forms.TextBox();
            this.loginURILabel = new System.Windows.Forms.Label();
            this.portBox = new System.Windows.Forms.TextBox();
            this.portLabel = new System.Windows.Forms.Label();
            this.proxyStartButton = new System.Windows.Forms.Button();
            this.viewerLaunchButton = new System.Windows.Forms.Button();
            this.firstNameBox = new System.Windows.Forms.TextBox();
            this.firstNameLabel = new System.Windows.Forms.Label();
            this.lastNameLabel = new System.Windows.Forms.Label();
            this.lastNameBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.passwordBox = new System.Windows.Forms.TextBox();
            this.viewerExeBox = new System.Windows.Forms.TextBox();
            this.clientLabel = new System.Windows.Forms.Label();
            this.proxyStatusLabelLabel = new System.Windows.Forms.Label();
            this.proxyStatusLabel = new System.Windows.Forms.Label();
            this.clientStatusLabel = new System.Windows.Forms.Label();
            this.clientStatusLabelLabel = new System.Windows.Forms.Label();
            this.gridCheck = new System.Windows.Forms.CheckBox();
            this.gridBox = new System.Windows.Forms.TextBox();
            this.controlCamera = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.workingDirectoryBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // loginURIBox
            // 
            this.loginURIBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.loginURIBox.Location = new System.Drawing.Point(64, 5);
            this.loginURIBox.Name = "loginURIBox";
            this.loginURIBox.Size = new System.Drawing.Size(200, 20);
            this.loginURIBox.TabIndex = 16;
            this.loginURIBox.Text = "http://apollo.cs.st-andrews.ac.uk:8002";
            this.loginURIBox.TextChanged += new System.EventHandler(this.loginURIBox_TextChanged);
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
            // portBox
            // 
            this.portBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.portBox.Location = new System.Drawing.Point(340, 5);
            this.portBox.Name = "portBox";
            this.portBox.Size = new System.Drawing.Size(39, 20);
            this.portBox.TabIndex = 12;
            this.portBox.Text = "8080";
            this.portBox.TextChanged += new System.EventHandler(this.portBox_TextChanged);
            // 
            // portLabel
            // 
            this.portLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.portLabel.AutoSize = true;
            this.portLabel.Location = new System.Drawing.Point(270, 8);
            this.portLabel.Name = "portLabel";
            this.portLabel.Size = new System.Drawing.Size(64, 13);
            this.portLabel.TabIndex = 11;
            this.portLabel.Text = "Access Port";
            // 
            // proxyStartButton
            // 
            this.proxyStartButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.proxyStartButton.Location = new System.Drawing.Point(6, 31);
            this.proxyStartButton.Name = "proxyStartButton";
            this.proxyStartButton.Size = new System.Drawing.Size(373, 20);
            this.proxyStartButton.TabIndex = 18;
            this.proxyStartButton.Text = "Plane Proxy";
            this.proxyStartButton.UseVisualStyleBackColor = true;
            this.proxyStartButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // viewerLaunchButton
            // 
            this.viewerLaunchButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.viewerLaunchButton.Location = new System.Drawing.Point(3, 133);
            this.viewerLaunchButton.Name = "viewerLaunchButton";
            this.viewerLaunchButton.Size = new System.Drawing.Size(376, 23);
            this.viewerLaunchButton.TabIndex = 19;
            this.viewerLaunchButton.Text = "Launch Viewer";
            this.viewerLaunchButton.UseVisualStyleBackColor = true;
            this.viewerLaunchButton.Click += new System.EventHandler(this.viewerLaunchButton_Click);
            // 
            // firstNameBox
            // 
            this.firstNameBox.Location = new System.Drawing.Point(64, 55);
            this.firstNameBox.Name = "firstNameBox";
            this.firstNameBox.Size = new System.Drawing.Size(60, 20);
            this.firstNameBox.TabIndex = 20;
            this.firstNameBox.Text = "Routing";
            this.firstNameBox.TextChanged += new System.EventHandler(this.firstNameBox_TextChanged);
            // 
            // firstNameLabel
            // 
            this.firstNameLabel.AutoSize = true;
            this.firstNameLabel.Location = new System.Drawing.Point(3, 58);
            this.firstNameLabel.Name = "firstNameLabel";
            this.firstNameLabel.Size = new System.Drawing.Size(57, 13);
            this.firstNameLabel.TabIndex = 21;
            this.firstNameLabel.Text = "First Name";
            // 
            // lastNameLabel
            // 
            this.lastNameLabel.AutoSize = true;
            this.lastNameLabel.Location = new System.Drawing.Point(130, 58);
            this.lastNameLabel.Name = "lastNameLabel";
            this.lastNameLabel.Size = new System.Drawing.Size(58, 13);
            this.lastNameLabel.TabIndex = 23;
            this.lastNameLabel.Text = "Last Name";
            // 
            // lastNameBox
            // 
            this.lastNameBox.Location = new System.Drawing.Point(194, 55);
            this.lastNameBox.Name = "lastNameBox";
            this.lastNameBox.Size = new System.Drawing.Size(49, 20);
            this.lastNameBox.TabIndex = 22;
            this.lastNameBox.Text = "God";
            this.lastNameBox.TextChanged += new System.EventHandler(this.lastNameBox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(249, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 25;
            this.label1.Text = "Password";
            // 
            // passwordBox
            // 
            this.passwordBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.passwordBox.Location = new System.Drawing.Point(308, 55);
            this.passwordBox.Name = "passwordBox";
            this.passwordBox.PasswordChar = '*';
            this.passwordBox.Size = new System.Drawing.Size(71, 20);
            this.passwordBox.TabIndex = 24;
            this.passwordBox.Text = "1245";
            this.passwordBox.TextChanged += new System.EventHandler(this.passwordBox_TextChanged);
            // 
            // viewerExeBox
            // 
            this.viewerExeBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.viewerExeBox.Location = new System.Drawing.Point(48, 81);
            this.viewerExeBox.Name = "viewerExeBox";
            this.viewerExeBox.Size = new System.Drawing.Size(195, 20);
            this.viewerExeBox.TabIndex = 26;
            this.viewerExeBox.Text = "C:\\\\Program Files (x86)\\\\Firestorm-Release\\\\Firestorm-Release.exe";
            this.viewerExeBox.TextChanged += new System.EventHandler(this.targetBox_TextChanged);
            // 
            // clientLabel
            // 
            this.clientLabel.AutoSize = true;
            this.clientLabel.Location = new System.Drawing.Point(3, 84);
            this.clientLabel.Name = "clientLabel";
            this.clientLabel.Size = new System.Drawing.Size(39, 13);
            this.clientLabel.TabIndex = 27;
            this.clientLabel.Text = "Viewer";
            // 
            // proxyStatusLabelLabel
            // 
            this.proxyStatusLabelLabel.AutoSize = true;
            this.proxyStatusLabelLabel.Location = new System.Drawing.Point(0, 160);
            this.proxyStatusLabelLabel.Name = "proxyStatusLabelLabel";
            this.proxyStatusLabelLabel.Size = new System.Drawing.Size(99, 13);
            this.proxyStatusLabelLabel.TabIndex = 28;
            this.proxyStatusLabelLabel.Text = "SlaveProxy Status: ";
            // 
            // proxyStatusLabel
            // 
            this.proxyStatusLabel.AutoSize = true;
            this.proxyStatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.proxyStatusLabel.Location = new System.Drawing.Point(78, 160);
            this.proxyStatusLabel.Name = "proxyStatusLabel";
            this.proxyStatusLabel.Size = new System.Drawing.Size(28, 13);
            this.proxyStatusLabel.TabIndex = 29;
            this.proxyStatusLabel.Text = "Idle";
            // 
            // clientStatusLabel
            // 
            this.clientStatusLabel.AutoSize = true;
            this.clientStatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clientStatusLabel.Location = new System.Drawing.Point(78, 173);
            this.clientStatusLabel.Name = "clientStatusLabel";
            this.clientStatusLabel.Size = new System.Drawing.Size(28, 13);
            this.clientStatusLabel.TabIndex = 31;
            this.clientStatusLabel.Text = "Idle";
            // 
            // clientStatusLabelLabel
            // 
            this.clientStatusLabelLabel.AutoSize = true;
            this.clientStatusLabelLabel.Location = new System.Drawing.Point(0, 173);
            this.clientStatusLabelLabel.Name = "clientStatusLabelLabel";
            this.clientStatusLabelLabel.Size = new System.Drawing.Size(72, 13);
            this.clientStatusLabelLabel.TabIndex = 30;
            this.clientStatusLabelLabel.Text = "Client Status: ";
            // 
            // gridCheck
            // 
            this.gridCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gridCheck.AutoSize = true;
            this.gridCheck.CheckAlign = System.Drawing.ContentAlignment.BottomRight;
            this.gridCheck.Location = new System.Drawing.Point(249, 83);
            this.gridCheck.Name = "gridCheck";
            this.gridCheck.Size = new System.Drawing.Size(45, 17);
            this.gridCheck.TabIndex = 32;
            this.gridCheck.Text = "Grid";
            this.gridCheck.UseVisualStyleBackColor = true;
            this.gridCheck.CheckedChanged += new System.EventHandler(this.gridCheck_CheckedChanged);
            // 
            // gridBox
            // 
            this.gridBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gridBox.Enabled = false;
            this.gridBox.Location = new System.Drawing.Point(300, 81);
            this.gridBox.Name = "gridBox";
            this.gridBox.Size = new System.Drawing.Size(79, 20);
            this.gridBox.TabIndex = 33;
            this.gridBox.TextChanged += new System.EventHandler(this.gridBox_TextChanged);
            // 
            // controlCamera
            // 
            this.controlCamera.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.controlCamera.AutoSize = true;
            this.controlCamera.Location = new System.Drawing.Point(281, 162);
            this.controlCamera.Name = "controlCamera";
            this.controlCamera.Size = new System.Drawing.Size(98, 17);
            this.controlCamera.TabIndex = 34;
            this.controlCamera.Text = "Control Camera";
            this.controlCamera.UseVisualStyleBackColor = true;
            this.controlCamera.CheckedChanged += new System.EventHandler(this.controlCamera_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 110);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 13);
            this.label2.TabIndex = 36;
            this.label2.Text = "Working Directory";
            // 
            // workingDirectoryBox
            // 
            this.workingDirectoryBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.workingDirectoryBox.Location = new System.Drawing.Point(101, 107);
            this.workingDirectoryBox.Name = "workingDirectoryBox";
            this.workingDirectoryBox.Size = new System.Drawing.Size(278, 20);
            this.workingDirectoryBox.TabIndex = 35;
            this.workingDirectoryBox.Text = "C:\\\\Program Files (x86)\\\\Firestorm-Release\\\\";
            this.workingDirectoryBox.TextChanged += new System.EventHandler(this.workingDirectory_TextChanged);
            // 
            // ProxyPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.workingDirectoryBox);
            this.Controls.Add(this.controlCamera);
            this.Controls.Add(this.gridBox);
            this.Controls.Add(this.gridCheck);
            this.Controls.Add(this.clientStatusLabel);
            this.Controls.Add(this.clientStatusLabelLabel);
            this.Controls.Add(this.proxyStatusLabel);
            this.Controls.Add(this.proxyStatusLabelLabel);
            this.Controls.Add(this.clientLabel);
            this.Controls.Add(this.viewerExeBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.passwordBox);
            this.Controls.Add(this.lastNameLabel);
            this.Controls.Add(this.lastNameBox);
            this.Controls.Add(this.firstNameLabel);
            this.Controls.Add(this.firstNameBox);
            this.Controls.Add(this.viewerLaunchButton);
            this.Controls.Add(this.proxyStartButton);
            this.Controls.Add(this.loginURIBox);
            this.Controls.Add(this.loginURILabel);
            this.Controls.Add(this.portBox);
            this.Controls.Add(this.portLabel);
            this.MinimumSize = new System.Drawing.Size(382, 193);
            this.Name = "ProxyPanel";
            this.Size = new System.Drawing.Size(382, 193);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox loginURIBox;
        private System.Windows.Forms.Label loginURILabel;
        private System.Windows.Forms.TextBox portBox;
        private System.Windows.Forms.Label portLabel;
        private System.Windows.Forms.Button proxyStartButton;
        private System.Windows.Forms.Button viewerLaunchButton;
        private System.Windows.Forms.TextBox firstNameBox;
        private System.Windows.Forms.Label firstNameLabel;
        private System.Windows.Forms.Label lastNameLabel;
        private System.Windows.Forms.TextBox lastNameBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox passwordBox;
        private System.Windows.Forms.TextBox viewerExeBox;
        private System.Windows.Forms.Label clientLabel;
        private System.Windows.Forms.Label proxyStatusLabelLabel;
        private System.Windows.Forms.Label proxyStatusLabel;
        private System.Windows.Forms.Label clientStatusLabel;
        private System.Windows.Forms.Label clientStatusLabelLabel;
        private System.Windows.Forms.CheckBox gridCheck;
        private System.Windows.Forms.TextBox gridBox;
        private System.Windows.Forms.CheckBox controlCamera;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox workingDirectoryBox;
    }
}

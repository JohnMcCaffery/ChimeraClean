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
            this.portLabel = new System.Windows.Forms.Label();
            this.portBox = new System.Windows.Forms.TextBox();
            this.listenIPBox = new System.Windows.Forms.TextBox();
            this.listenIPLabel = new System.Windows.Forms.Label();
            this.loginURIBox = new System.Windows.Forms.TextBox();
            this.loginURILabel = new System.Windows.Forms.Label();
            this.connectButton = new System.Windows.Forms.Button();
            this.packetCountLabel = new System.Windows.Forms.Label();
            this.loggedInLabel = new System.Windows.Forms.Label();
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
            this.slavesListBox.Location = new System.Drawing.Point(12, 12);
            this.slavesListBox.Name = "slavesListBox";
            this.slavesListBox.Size = new System.Drawing.Size(172, 264);
            this.slavesListBox.TabIndex = 1;
            // 
            // portLabel
            // 
            this.portLabel.AutoSize = true;
            this.portLabel.Location = new System.Drawing.Point(190, 16);
            this.portLabel.Name = "portLabel";
            this.portLabel.Size = new System.Drawing.Size(26, 13);
            this.portLabel.TabIndex = 2;
            this.portLabel.Text = "Port";
            // 
            // portBox
            // 
            this.portBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.portBox.Location = new System.Drawing.Point(243, 13);
            this.portBox.Name = "portBox";
            this.portBox.Size = new System.Drawing.Size(38, 20);
            this.portBox.TabIndex = 3;
            this.portBox.Text = "8080";
            // 
            // listenIPBox
            // 
            this.listenIPBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listenIPBox.Location = new System.Drawing.Point(243, 39);
            this.listenIPBox.Name = "listenIPBox";
            this.listenIPBox.Size = new System.Drawing.Size(267, 20);
            this.listenIPBox.TabIndex = 5;
            this.listenIPBox.Text = "127.0.0.1";
            // 
            // listenIPLabel
            // 
            this.listenIPLabel.AutoSize = true;
            this.listenIPLabel.Location = new System.Drawing.Point(190, 42);
            this.listenIPLabel.Name = "listenIPLabel";
            this.listenIPLabel.Size = new System.Drawing.Size(48, 13);
            this.listenIPLabel.TabIndex = 4;
            this.listenIPLabel.Text = "Listen IP";
            // 
            // loginURIBox
            // 
            this.loginURIBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.loginURIBox.Location = new System.Drawing.Point(243, 65);
            this.loginURIBox.Name = "loginURIBox";
            this.loginURIBox.Size = new System.Drawing.Size(267, 20);
            this.loginURIBox.TabIndex = 7;
            this.loginURIBox.Text = "http://diana.cs.st-andrews.ac.uk:8002";
            // 
            // loginURILabel
            // 
            this.loginURILabel.AutoSize = true;
            this.loginURILabel.Location = new System.Drawing.Point(190, 68);
            this.loginURILabel.Name = "loginURILabel";
            this.loginURILabel.Size = new System.Drawing.Size(26, 13);
            this.loginURILabel.TabIndex = 6;
            this.loginURILabel.Text = "Port";
            // 
            // connectButton
            // 
            this.connectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.connectButton.Location = new System.Drawing.Point(193, 91);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(317, 185);
            this.connectButton.TabIndex = 8;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // packetCountLabel
            // 
            this.packetCountLabel.AutoSize = true;
            this.packetCountLabel.Location = new System.Drawing.Point(287, 16);
            this.packetCountLabel.Name = "packetCountLabel";
            this.packetCountLabel.Size = new System.Drawing.Size(13, 13);
            this.packetCountLabel.TabIndex = 9;
            this.packetCountLabel.Text = "0";
            // 
            // loggedInLabel
            // 
            this.loggedInLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.loggedInLabel.AutoSize = true;
            this.loggedInLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loggedInLabel.Location = new System.Drawing.Point(455, 13);
            this.loggedInLabel.Name = "loggedInLabel";
            this.loggedInLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.loggedInLabel.Size = new System.Drawing.Size(0, 13);
            this.loggedInLabel.TabIndex = 10;
            // 
            // MasterProxyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(522, 285);
            this.Controls.Add(this.loggedInLabel);
            this.Controls.Add(this.packetCountLabel);
            this.Controls.Add(this.connectButton);
            this.Controls.Add(this.loginURIBox);
            this.Controls.Add(this.loginURILabel);
            this.Controls.Add(this.listenIPBox);
            this.Controls.Add(this.listenIPLabel);
            this.Controls.Add(this.portBox);
            this.Controls.Add(this.portLabel);
            this.Controls.Add(this.slavesListBox);
            this.Name = "MasterProxyForm";
            this.Text = "Master Proxy";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer listTimer;
        private System.Windows.Forms.ListBox slavesListBox;
        private System.Windows.Forms.Label portLabel;
        private System.Windows.Forms.TextBox portBox;
        private System.Windows.Forms.TextBox listenIPBox;
        private System.Windows.Forms.Label listenIPLabel;
        private System.Windows.Forms.TextBox loginURIBox;
        private System.Windows.Forms.Label loginURILabel;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.Label packetCountLabel;
        private System.Windows.Forms.Label loggedInLabel;
    }
}


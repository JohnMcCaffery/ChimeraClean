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
            this.startButton = new System.Windows.Forms.Button();
            this.updateTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // loginURIBox
            // 
            this.loginURIBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.loginURIBox.Location = new System.Drawing.Point(286, 5);
            this.loginURIBox.Name = "loginURIBox";
            this.loginURIBox.Size = new System.Drawing.Size(323, 20);
            this.loginURIBox.TabIndex = 16;
            this.loginURIBox.Text = "http://apollo.cs.st-andrews.ac.uk:8002";
            // 
            // loginURILabel
            // 
            this.loginURILabel.AutoSize = true;
            this.loginURILabel.Location = new System.Drawing.Point(225, 9);
            this.loginURILabel.Name = "loginURILabel";
            this.loginURILabel.Size = new System.Drawing.Size(55, 13);
            this.loginURILabel.TabIndex = 15;
            this.loginURILabel.Text = "Login URI";
            // 
            // listenIPBox
            // 
            this.listenIPBox.Location = new System.Drawing.Point(132, 5);
            this.listenIPBox.Name = "listenIPBox";
            this.listenIPBox.Size = new System.Drawing.Size(87, 20);
            this.listenIPBox.TabIndex = 14;
            this.listenIPBox.Text = "127.0.0.1";
            // 
            // listenIPLabel
            // 
            this.listenIPLabel.AutoSize = true;
            this.listenIPLabel.Location = new System.Drawing.Point(78, 8);
            this.listenIPLabel.Name = "listenIPLabel";
            this.listenIPLabel.Size = new System.Drawing.Size(48, 13);
            this.listenIPLabel.TabIndex = 13;
            this.listenIPLabel.Text = "Listen IP";
            // 
            // portBox
            // 
            this.portBox.Location = new System.Drawing.Point(33, 5);
            this.portBox.Name = "portBox";
            this.portBox.Size = new System.Drawing.Size(39, 20);
            this.portBox.TabIndex = 12;
            this.portBox.Text = "8080";
            // 
            // portLabel
            // 
            this.portLabel.AutoSize = true;
            this.portLabel.Location = new System.Drawing.Point(1, 8);
            this.portLabel.Name = "portLabel";
            this.portLabel.Size = new System.Drawing.Size(26, 13);
            this.portLabel.TabIndex = 11;
            this.portLabel.Text = "Port";
            // 
            // startButton
            // 
            this.startButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.startButton.Location = new System.Drawing.Point(615, 4);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(65, 20);
            this.startButton.TabIndex = 18;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // updateTimer
            // 
            this.updateTimer.Enabled = true;
            this.updateTimer.Tick += new System.EventHandler(this.updateTimer_Tick);
            // 
            // ProxyPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.loginURIBox);
            this.Controls.Add(this.loginURILabel);
            this.Controls.Add(this.listenIPBox);
            this.Controls.Add(this.listenIPLabel);
            this.Controls.Add(this.portBox);
            this.Controls.Add(this.portLabel);
            this.Name = "ProxyPanel";
            this.Size = new System.Drawing.Size(683, 30);
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
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Timer updateTimer;
    }
}

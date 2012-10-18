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
            this.components = new System.ComponentModel.Container();
            this.nameBox = new System.Windows.Forms.TextBox();
            this.masterURIBox = new System.Windows.Forms.TextBox();
            this.connectButton = new System.Windows.Forms.Button();
            this.checkTimer = new System.Windows.Forms.Timer(this.components);
            this.countLabel = new System.Windows.Forms.Label();
            this.pingedLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // nameBox
            // 
            this.nameBox.Location = new System.Drawing.Point(12, 12);
            this.nameBox.Name = "nameBox";
            this.nameBox.Size = new System.Drawing.Size(100, 20);
            this.nameBox.TabIndex = 0;
            this.nameBox.Text = "Slave 1";
            // 
            // masterURIBox
            // 
            this.masterURIBox.Location = new System.Drawing.Point(12, 38);
            this.masterURIBox.Name = "masterURIBox";
            this.masterURIBox.Size = new System.Drawing.Size(241, 20);
            this.masterURIBox.TabIndex = 2;
            this.masterURIBox.Text = "http://localhost:5678/Master.rem";
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(259, 38);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(57, 23);
            this.connectButton.TabIndex = 3;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // checkTimer
            // 
            this.checkTimer.Enabled = true;
            this.checkTimer.Tick += new System.EventHandler(this.checkTimer_Tick);
            // 
            // countLabel
            // 
            this.countLabel.AutoSize = true;
            this.countLabel.Location = new System.Drawing.Point(12, 61);
            this.countLabel.Name = "countLabel";
            this.countLabel.Size = new System.Drawing.Size(13, 13);
            this.countLabel.TabIndex = 4;
            this.countLabel.Text = "0";
            // 
            // pingedLabel
            // 
            this.pingedLabel.AutoSize = true;
            this.pingedLabel.Location = new System.Drawing.Point(118, 15);
            this.pingedLabel.Name = "pingedLabel";
            this.pingedLabel.Size = new System.Drawing.Size(0, 13);
            this.pingedLabel.TabIndex = 5;
            // 
            // SlaveProxyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(328, 273);
            this.Controls.Add(this.pingedLabel);
            this.Controls.Add(this.countLabel);
            this.Controls.Add(this.connectButton);
            this.Controls.Add(this.masterURIBox);
            this.Controls.Add(this.nameBox);
            this.Name = "SlaveProxyForm";
            this.Text = "Slave Proxy GUI";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox nameBox;
        private System.Windows.Forms.TextBox masterURIBox;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.Timer checkTimer;
        private System.Windows.Forms.Label countLabel;
        private System.Windows.Forms.Label pingedLabel;
    }
}


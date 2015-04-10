namespace SandboxTest {
    partial class UDPTest {
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
            this.responseLabel = new System.Windows.Forms.Label();
            this.MainSplit = new System.Windows.Forms.SplitContainer();
            this.unrealPanel = new System.Windows.Forms.Panel();
            this.toSendBox = new System.Windows.Forms.TextBox();
            this.sendButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.MainSplit)).BeginInit();
            this.MainSplit.Panel2.SuspendLayout();
            this.MainSplit.SuspendLayout();
            this.SuspendLayout();
            // 
            // responseLabel
            // 
            this.responseLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.responseLabel.AutoSize = true;
            this.responseLabel.Location = new System.Drawing.Point(3, 4);
            this.responseLabel.Name = "responseLabel";
            this.responseLabel.Size = new System.Drawing.Size(0, 13);
            this.responseLabel.TabIndex = 2;
            // 
            // MainSplit
            // 
            this.MainSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainSplit.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.MainSplit.Location = new System.Drawing.Point(0, 0);
            this.MainSplit.Name = "MainSplit";
            this.MainSplit.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // MainSplit.Panel2
            // 
            this.MainSplit.Panel2.Controls.Add(this.responseLabel);
            this.MainSplit.Panel2.Controls.Add(this.unrealPanel);
            this.MainSplit.Panel2.Controls.Add(this.toSendBox);
            this.MainSplit.Panel2.Controls.Add(this.sendButton);
            this.MainSplit.Size = new System.Drawing.Size(991, 633);
            this.MainSplit.SplitterDistance = 604;
            this.MainSplit.TabIndex = 3;
            // 
            // unrealPanel
            // 
            this.unrealPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.unrealPanel.Location = new System.Drawing.Point(3, 3);
            this.unrealPanel.Name = "unrealPanel";
            this.unrealPanel.Size = new System.Drawing.Size(985, 0);
            this.unrealPanel.TabIndex = 4;
            this.unrealPanel.DoubleClick += new System.EventHandler(this.unrealPanel_DoubleClick);
            // 
            // toSendBox
            // 
            this.toSendBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.toSendBox.Location = new System.Drawing.Point(63, 2);
            this.toSendBox.Name = "toSendBox";
            this.toSendBox.Size = new System.Drawing.Size(875, 20);
            this.toSendBox.TabIndex = 3;
            this.toSendBox.Leave += new System.EventHandler(this.sendButton_Click);
            // 
            // sendButton
            // 
            this.sendButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sendButton.Location = new System.Drawing.Point(944, -1);
            this.sendButton.Name = "sendButton";
            this.sendButton.Size = new System.Drawing.Size(44, 23);
            this.sendButton.TabIndex = 2;
            this.sendButton.Text = "Start";
            this.sendButton.UseVisualStyleBackColor = true;
            this.sendButton.Click += new System.EventHandler(this.sendButton_Click);
            // 
            // UDPTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(991, 633);
            this.Controls.Add(this.MainSplit);
            this.Name = "UDPTest";
            this.Text = "UDPTest";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UDPTest_FormClosing);
            this.MainSplit.Panel2.ResumeLayout(false);
            this.MainSplit.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainSplit)).EndInit();
            this.MainSplit.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label responseLabel;
        private System.Windows.Forms.SplitContainer MainSplit;
        private System.Windows.Forms.TextBox toSendBox;
        private System.Windows.Forms.Button sendButton;
        private System.Windows.Forms.Panel unrealPanel;
    }
}
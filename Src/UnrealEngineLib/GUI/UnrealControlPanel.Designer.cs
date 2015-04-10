namespace UnrealEngineLib.GUI {
    partial class UnrealControlPanel {
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
            this.clientLabel = new System.Windows.Forms.Label();
            this.unrealExeBox = new System.Windows.Forms.TextBox();
            this.unrealLaunchButton = new System.Windows.Forms.Button();
            this.startUDPButton = new System.Windows.Forms.Button();
            this.listenPortUpDown = new System.Windows.Forms.NumericUpDown();
            this.sendBox = new System.Windows.Forms.TextBox();
            this.sendButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.listenPortUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // clientLabel
            // 
            this.clientLabel.AutoSize = true;
            this.clientLabel.Location = new System.Drawing.Point(7, 6);
            this.clientLabel.Name = "clientLabel";
            this.clientLabel.Size = new System.Drawing.Size(59, 13);
            this.clientLabel.TabIndex = 30;
            this.clientLabel.Text = "Unreal Exe";
            // 
            // unrealExeBox
            // 
            this.unrealExeBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.unrealExeBox.Location = new System.Drawing.Point(72, 3);
            this.unrealExeBox.Name = "unrealExeBox";
            this.unrealExeBox.Size = new System.Drawing.Size(722, 20);
            this.unrealExeBox.TabIndex = 29;
            this.unrealExeBox.Text = "E:/Engines/Unreal Editor/ChimeraLinkTest/WindowsNoEditor/ChimeraLinkTest.exe";
            // 
            // unrealLaunchButton
            // 
            this.unrealLaunchButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.unrealLaunchButton.Location = new System.Drawing.Point(184, 29);
            this.unrealLaunchButton.Name = "unrealLaunchButton";
            this.unrealLaunchButton.Size = new System.Drawing.Size(610, 23);
            this.unrealLaunchButton.TabIndex = 28;
            this.unrealLaunchButton.Text = "Launch Unreal";
            this.unrealLaunchButton.UseVisualStyleBackColor = true;
            this.unrealLaunchButton.Click += new System.EventHandler(this.unrealLaunchButton_Click);
            // 
            // startUDPButton
            // 
            this.startUDPButton.Location = new System.Drawing.Point(103, 29);
            this.startUDPButton.Name = "startUDPButton";
            this.startUDPButton.Size = new System.Drawing.Size(75, 23);
            this.startUDPButton.TabIndex = 31;
            this.startUDPButton.Text = "Start UDP Pipe";
            this.startUDPButton.UseVisualStyleBackColor = true;
            this.startUDPButton.Click += new System.EventHandler(this.startUDPButton_Click);
            // 
            // listenPortUpDown
            // 
            this.listenPortUpDown.Location = new System.Drawing.Point(10, 32);
            this.listenPortUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.listenPortUpDown.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.listenPortUpDown.Name = "listenPortUpDown";
            this.listenPortUpDown.Size = new System.Drawing.Size(87, 20);
            this.listenPortUpDown.TabIndex = 32;
            this.listenPortUpDown.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // sendBox
            // 
            this.sendBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sendBox.Location = new System.Drawing.Point(10, 59);
            this.sendBox.Name = "sendBox";
            this.sendBox.Size = new System.Drawing.Size(703, 20);
            this.sendBox.TabIndex = 33;
            this.sendBox.Text = "~console exit";
            this.sendBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.sendBox_KeyPress);
            // 
            // sendButton
            // 
            this.sendButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.sendButton.Location = new System.Drawing.Point(719, 57);
            this.sendButton.Name = "sendButton";
            this.sendButton.Size = new System.Drawing.Size(75, 23);
            this.sendButton.TabIndex = 34;
            this.sendButton.Text = "Send";
            this.sendButton.UseVisualStyleBackColor = true;
            this.sendButton.Click += new System.EventHandler(this.sendButton_Click);
            // 
            // UnrealControlPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.sendButton);
            this.Controls.Add(this.sendBox);
            this.Controls.Add(this.listenPortUpDown);
            this.Controls.Add(this.startUDPButton);
            this.Controls.Add(this.clientLabel);
            this.Controls.Add(this.unrealExeBox);
            this.Controls.Add(this.unrealLaunchButton);
            this.Name = "UnrealControlPanel";
            this.Size = new System.Drawing.Size(797, 307);
            ((System.ComponentModel.ISupportInitialize)(this.listenPortUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label clientLabel;
        private System.Windows.Forms.TextBox unrealExeBox;
        private System.Windows.Forms.Button unrealLaunchButton;
        private System.Windows.Forms.Button startUDPButton;
        private System.Windows.Forms.NumericUpDown listenPortUpDown;
        private System.Windows.Forms.TextBox sendBox;
        private System.Windows.Forms.Button sendButton;
    }
}

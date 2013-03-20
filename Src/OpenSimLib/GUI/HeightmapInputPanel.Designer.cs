namespace Chimera.OpenSim.GUI {
    partial class HeightmapInputPanel {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HeightmapInputPanel));
            this.loginButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.passwordBox = new System.Windows.Forms.TextBox();
            this.lastNameLabel = new System.Windows.Forms.Label();
            this.lastNameBox = new System.Windows.Forms.TextBox();
            this.firstNameLabel = new System.Windows.Forms.Label();
            this.firstNameBox = new System.Windows.Forms.TextBox();
            this.startPositionPanel = new Chimera.GUI.VectorPanel();
            this.regionBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.teleportButton = new System.Windows.Forms.Button();
            this.loginErrorLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // loginButton
            // 
            this.loginButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.loginButton.Location = new System.Drawing.Point(4, 4);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(561, 23);
            this.loginButton.TabIndex = 0;
            this.loginButton.Text = "Login";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(249, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 31;
            this.label1.Text = "Password";
            // 
            // passwordBox
            // 
            this.passwordBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.passwordBox.Location = new System.Drawing.Point(308, 33);
            this.passwordBox.Name = "passwordBox";
            this.passwordBox.PasswordChar = '*';
            this.passwordBox.Size = new System.Drawing.Size(257, 20);
            this.passwordBox.TabIndex = 30;
            this.passwordBox.Text = "1245";
            this.passwordBox.TextChanged += new System.EventHandler(this.passwordBox_TextChanged);
            // 
            // lastNameLabel
            // 
            this.lastNameLabel.AutoSize = true;
            this.lastNameLabel.Location = new System.Drawing.Point(130, 36);
            this.lastNameLabel.Name = "lastNameLabel";
            this.lastNameLabel.Size = new System.Drawing.Size(58, 13);
            this.lastNameLabel.TabIndex = 29;
            this.lastNameLabel.Text = "Last Name";
            // 
            // lastNameBox
            // 
            this.lastNameBox.Location = new System.Drawing.Point(194, 33);
            this.lastNameBox.Name = "lastNameBox";
            this.lastNameBox.Size = new System.Drawing.Size(49, 20);
            this.lastNameBox.TabIndex = 28;
            this.lastNameBox.Text = "God";
            this.lastNameBox.TextChanged += new System.EventHandler(this.lastNameBox_TextChanged);
            // 
            // firstNameLabel
            // 
            this.firstNameLabel.AutoSize = true;
            this.firstNameLabel.Location = new System.Drawing.Point(1, 36);
            this.firstNameLabel.Name = "firstNameLabel";
            this.firstNameLabel.Size = new System.Drawing.Size(57, 13);
            this.firstNameLabel.TabIndex = 27;
            this.firstNameLabel.Text = "First Name";
            // 
            // firstNameBox
            // 
            this.firstNameBox.Location = new System.Drawing.Point(64, 33);
            this.firstNameBox.Name = "firstNameBox";
            this.firstNameBox.Size = new System.Drawing.Size(60, 20);
            this.firstNameBox.TabIndex = 26;
            this.firstNameBox.Text = "Routing";
            this.firstNameBox.TextChanged += new System.EventHandler(this.firstNameBox_TextChanged);
            // 
            // startPositionPanel
            // 
            this.startPositionPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.startPositionPanel.Location = new System.Drawing.Point(0, 78);
            this.startPositionPanel.Max = 256F;
            this.startPositionPanel.MaxV = ((OpenMetaverse.Vector3)(resources.GetObject("startPositionPanel.MaxV")));
            this.startPositionPanel.Min = 0F;
            this.startPositionPanel.MinimumSize = new System.Drawing.Size(103, 95);
            this.startPositionPanel.MinV = ((OpenMetaverse.Vector3)(resources.GetObject("startPositionPanel.MinV")));
            this.startPositionPanel.Name = "startPositionPanel";
            this.startPositionPanel.Size = new System.Drawing.Size(565, 95);
            this.startPositionPanel.TabIndex = 32;
            this.startPositionPanel.Value = ((OpenMetaverse.Vector3)(resources.GetObject("startPositionPanel.Value")));
            this.startPositionPanel.X = 0F;
            this.startPositionPanel.Y = 0F;
            this.startPositionPanel.Z = 0F;
            this.startPositionPanel.ValueChanged += new System.EventHandler(this.startPositionPanel_ValueChanged);
            // 
            // regionBox
            // 
            this.regionBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.regionBox.Location = new System.Drawing.Point(58, 59);
            this.regionBox.Name = "regionBox";
            this.regionBox.Size = new System.Drawing.Size(426, 20);
            this.regionBox.TabIndex = 33;
            this.regionBox.TextChanged += new System.EventHandler(this.regionBox_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 34;
            this.label2.Text = "Region";
            // 
            // teleportButton
            // 
            this.teleportButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.teleportButton.Enabled = false;
            this.teleportButton.Location = new System.Drawing.Point(490, 59);
            this.teleportButton.Name = "teleportButton";
            this.teleportButton.Size = new System.Drawing.Size(75, 23);
            this.teleportButton.TabIndex = 35;
            this.teleportButton.Text = "Teleport";
            this.teleportButton.UseVisualStyleBackColor = true;
            this.teleportButton.Click += new System.EventHandler(this.teleportButton_Click);
            // 
            // label3
            // 
            this.loginErrorLabel.AutoSize = true;
            this.loginErrorLabel.Location = new System.Drawing.Point(4, 168);
            this.loginErrorLabel.Name = "label3";
            this.loginErrorLabel.Size = new System.Drawing.Size(58, 13);
            this.loginErrorLabel.TabIndex = 36;
            this.loginErrorLabel.Text = "Login Error";
            // 
            // HeightmapInputPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.loginErrorLabel);
            this.Controls.Add(this.teleportButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.regionBox);
            this.Controls.Add(this.startPositionPanel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.passwordBox);
            this.Controls.Add(this.lastNameLabel);
            this.Controls.Add(this.lastNameBox);
            this.Controls.Add(this.firstNameLabel);
            this.Controls.Add(this.firstNameBox);
            this.Controls.Add(this.loginButton);
            this.Name = "HeightmapInputPanel";
            this.Size = new System.Drawing.Size(568, 401);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button loginButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox passwordBox;
        private System.Windows.Forms.Label lastNameLabel;
        private System.Windows.Forms.TextBox lastNameBox;
        private System.Windows.Forms.Label firstNameLabel;
        private System.Windows.Forms.TextBox firstNameBox;
        private Chimera.GUI.VectorPanel startPositionPanel;
        private System.Windows.Forms.TextBox regionBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button teleportButton;
        private System.Windows.Forms.Label loginErrorLabel;
    }
}

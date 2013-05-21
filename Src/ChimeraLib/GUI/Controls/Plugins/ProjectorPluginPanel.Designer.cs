namespace Chimera.GUI.Controls.Plugins {
    partial class ProjectorPluginPanel {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectorPluginPanel));
            this.roomTab = new System.Windows.Forms.TabPage();
            this.drawLabelsCheck = new System.Windows.Forms.CheckBox();
            this.drawRoomCheck = new System.Windows.Forms.CheckBox();
            this.roomPositionPanel = new Chimera.GUI.VectorPanel();
            this.mainTab = new System.Windows.Forms.TabControl();
            this.roomTab.SuspendLayout();
            this.mainTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // roomTab
            // 
            this.roomTab.Controls.Add(this.drawLabelsCheck);
            this.roomTab.Controls.Add(this.drawRoomCheck);
            this.roomTab.Controls.Add(this.roomPositionPanel);
            this.roomTab.Location = new System.Drawing.Point(4, 22);
            this.roomTab.Name = "roomTab";
            this.roomTab.Padding = new System.Windows.Forms.Padding(3);
            this.roomTab.Size = new System.Drawing.Size(540, 480);
            this.roomTab.TabIndex = 0;
            this.roomTab.Text = "Room";
            this.roomTab.UseVisualStyleBackColor = true;
            // 
            // drawLabelsCheck
            // 
            this.drawLabelsCheck.AutoSize = true;
            this.drawLabelsCheck.Location = new System.Drawing.Point(94, 101);
            this.drawLabelsCheck.Name = "drawLabelsCheck";
            this.drawLabelsCheck.Size = new System.Drawing.Size(85, 17);
            this.drawLabelsCheck.TabIndex = 55;
            this.drawLabelsCheck.Text = "Draw Labels";
            this.drawLabelsCheck.UseVisualStyleBackColor = true;
            this.drawLabelsCheck.CheckedChanged += new System.EventHandler(this.projectorDrawLabelsCheck_CheckedChanged);
            // 
            // drawRoomCheck
            // 
            this.drawRoomCheck.AutoSize = true;
            this.drawRoomCheck.Location = new System.Drawing.Point(6, 101);
            this.drawRoomCheck.Name = "drawRoomCheck";
            this.drawRoomCheck.Size = new System.Drawing.Size(82, 17);
            this.drawRoomCheck.TabIndex = 54;
            this.drawRoomCheck.Text = "Draw Room";
            this.drawRoomCheck.UseVisualStyleBackColor = true;
            this.drawRoomCheck.CheckedChanged += new System.EventHandler(this.projectorDrawRoomChecked_CheckedChanged);
            // 
            // roomPositionPanel
            // 
            this.roomPositionPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.roomPositionPanel.Location = new System.Drawing.Point(0, 0);
            this.roomPositionPanel.Max = 10000F;
            this.roomPositionPanel.MaxV = ((OpenMetaverse.Vector3)(resources.GetObject("roomPositionPanel.MaxV")));
            this.roomPositionPanel.Min = -10000F;
            this.roomPositionPanel.MinimumSize = new System.Drawing.Size(103, 95);
            this.roomPositionPanel.MinV = ((OpenMetaverse.Vector3)(resources.GetObject("roomPositionPanel.MinV")));
            this.roomPositionPanel.Name = "roomPositionPanel";
            this.roomPositionPanel.Size = new System.Drawing.Size(540, 95);
            this.roomPositionPanel.TabIndex = 52;
            this.roomPositionPanel.Value = ((OpenMetaverse.Vector3)(resources.GetObject("roomPositionPanel.Value")));
            this.roomPositionPanel.X = 0F;
            this.roomPositionPanel.Y = 0F;
            this.roomPositionPanel.Z = 0F;
            this.roomPositionPanel.ValueChanged += new System.EventHandler(this.roomPosition_ValueChanged);
            // 
            // mainTab
            // 
            this.mainTab.Controls.Add(this.roomTab);
            this.mainTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTab.Location = new System.Drawing.Point(0, 0);
            this.mainTab.Name = "mainTab";
            this.mainTab.SelectedIndex = 0;
            this.mainTab.Size = new System.Drawing.Size(548, 506);
            this.mainTab.TabIndex = 0;
            // 
            // ProjectorPluginPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainTab);
            this.Name = "ProjectorPluginPanel";
            this.Size = new System.Drawing.Size(548, 506);
            this.roomTab.ResumeLayout(false);
            this.roomTab.PerformLayout();
            this.mainTab.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage roomTab;
        private System.Windows.Forms.TabControl mainTab;
        private VectorPanel roomPositionPanel;
        private System.Windows.Forms.CheckBox drawRoomCheck;
        private System.Windows.Forms.CheckBox drawLabelsCheck;

    }
}

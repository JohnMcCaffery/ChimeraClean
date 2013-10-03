namespace Touchscreen.GUI {
    partial class VerticalAxisPanel {
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
            this.wPanel = new Chimera.GUI.ScalarPanel();
            this.hPanel = new Chimera.GUI.ScalarPanel();
            this.paddingHPanel = new Chimera.GUI.ScalarPanel();
            this.paddingVPanel = new Chimera.GUI.ScalarPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.constrainedAxisPanel = new Chimera.GUI.Controls.Plugins.ConstrainedAxisPanel();
            this.SuspendLayout();
            // 
            // wPanel
            // 
            this.wPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wPanel.Location = new System.Drawing.Point(25, 0);
            this.wPanel.Max = 1F;
            this.wPanel.Min = 0F;
            this.wPanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.wPanel.Name = "wPanel";
            this.wPanel.Size = new System.Drawing.Size(145, 20);
            this.wPanel.TabIndex = 0;
            this.wPanel.Value = 0F;
            this.wPanel.ValueChanged += new System.Action<float>(this.wPanel_ValueChanged);
            // 
            // hPanel
            // 
            this.hPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hPanel.Location = new System.Drawing.Point(25, 26);
            this.hPanel.Max = 1F;
            this.hPanel.Min = 0F;
            this.hPanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.hPanel.Name = "hPanel";
            this.hPanel.Size = new System.Drawing.Size(145, 20);
            this.hPanel.TabIndex = 1;
            this.hPanel.Value = 0F;
            this.hPanel.ValueChanged += new System.Action<float>(this.hPanel_ValueChanged);
            // 
            // paddingHPanel
            // 
            this.paddingHPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.paddingHPanel.Location = new System.Drawing.Point(65, 52);
            this.paddingHPanel.Max = 1F;
            this.paddingHPanel.Min = 0F;
            this.paddingHPanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.paddingHPanel.Name = "paddingHPanel";
            this.paddingHPanel.Size = new System.Drawing.Size(105, 20);
            this.paddingHPanel.TabIndex = 2;
            this.paddingHPanel.Value = 0F;
            this.paddingHPanel.ValueChanged += new System.Action<float>(this.paddingHPanel_ValueChanged);
            // 
            // paddingVPanel
            // 
            this.paddingVPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.paddingVPanel.Location = new System.Drawing.Point(65, 78);
            this.paddingVPanel.Max = 1F;
            this.paddingVPanel.Min = 0F;
            this.paddingVPanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.paddingVPanel.Name = "paddingVPanel";
            this.paddingVPanel.Size = new System.Drawing.Size(105, 20);
            this.paddingVPanel.TabIndex = 3;
            this.paddingVPanel.Value = 0F;
            this.paddingVPanel.ValueChanged += new System.Action<float>(this.paddingVPanel_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(18, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "W";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(15, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "H";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Padding H";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 81);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Padding V";
            // 
            // constrainedAxisPanel
            // 
            this.constrainedAxisPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.constrainedAxisPanel.Axis = null;
            this.constrainedAxisPanel.Location = new System.Drawing.Point(0, 104);
            this.constrainedAxisPanel.MinimumSize = new System.Drawing.Size(163, 57);
            this.constrainedAxisPanel.Name = "constrainedAxisPanel";
            this.constrainedAxisPanel.Size = new System.Drawing.Size(170, 57);
            this.constrainedAxisPanel.TabIndex = 8;
            // 
            // VerticalAxisPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.constrainedAxisPanel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.paddingVPanel);
            this.Controls.Add(this.paddingHPanel);
            this.Controls.Add(this.hPanel);
            this.Controls.Add(this.wPanel);
            this.MinimumSize = new System.Drawing.Size(170, 160);
            this.Name = "VerticalAxisPanel";
            this.Size = new System.Drawing.Size(170, 160);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Chimera.GUI.ScalarPanel wPanel;
        private Chimera.GUI.ScalarPanel hPanel;
        private Chimera.GUI.ScalarPanel paddingHPanel;
        private Chimera.GUI.ScalarPanel paddingVPanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private Chimera.GUI.Controls.Plugins.ConstrainedAxisPanel constrainedAxisPanel;
    }
}

namespace Touchscreen.GUI {
    partial class TouchscreenPluginPanel {
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
            this.axisBasedDeltaPanel = new Chimera.GUI.Controls.Plugins.AxisBasedDeltaPanel();
            this.singleAxisBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // axisBasedDeltaPanel
            // 
            this.axisBasedDeltaPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.axisBasedDeltaPanel.Location = new System.Drawing.Point(0, 30);
            this.axisBasedDeltaPanel.Name = "axisBasedDeltaPanel";
            this.axisBasedDeltaPanel.Size = new System.Drawing.Size(774, 570);
            this.axisBasedDeltaPanel.TabIndex = 0;
            // 
            // singleAxisBox
            // 
            this.singleAxisBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.singleAxisBox.FormattingEnabled = true;
            this.singleAxisBox.Location = new System.Drawing.Point(108, 3);
            this.singleAxisBox.Name = "singleAxisBox";
            this.singleAxisBox.Size = new System.Drawing.Size(663, 21);
            this.singleAxisBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Single Axis Position";
            // 
            // TouchscreenPluginPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.singleAxisBox);
            this.Controls.Add(this.axisBasedDeltaPanel);
            this.Name = "TouchscreenPluginPanel";
            this.Size = new System.Drawing.Size(774, 600);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Chimera.GUI.Controls.Plugins.AxisBasedDeltaPanel axisBasedDeltaPanel;
        private System.Windows.Forms.ComboBox singleAxisBox;
        private System.Windows.Forms.Label label1;
    }
}

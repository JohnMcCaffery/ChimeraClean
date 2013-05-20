namespace Chimera.Overlay.GUI {
    partial class ControllableSelector {
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
            this.namesBox = new System.Windows.Forms.ListBox();
            this.controlPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // namesBox
            // 
            this.namesBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.namesBox.DisplayMember = "Name";
            this.namesBox.FormattingEnabled = true;
            this.namesBox.Location = new System.Drawing.Point(0, 0);
            this.namesBox.Name = "namesBox";
            this.namesBox.Size = new System.Drawing.Size(120, 342);
            this.namesBox.TabIndex = 0;
            this.namesBox.SelectedIndexChanged += new System.EventHandler(this.namesBox_SelectedIndexChanged);
            // 
            // controlPanel
            // 
            this.controlPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.controlPanel.Location = new System.Drawing.Point(126, 3);
            this.controlPanel.Name = "controlPanel";
            this.controlPanel.Size = new System.Drawing.Size(401, 337);
            this.controlPanel.TabIndex = 1;
            // 
            // ControllableSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.controlPanel);
            this.Controls.Add(this.namesBox);
            this.Name = "ControllableSelector";
            this.Size = new System.Drawing.Size(530, 343);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox namesBox;
        private System.Windows.Forms.Panel controlPanel;
    }
}

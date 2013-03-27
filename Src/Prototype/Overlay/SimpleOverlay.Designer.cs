namespace Chimera.GUI.Forms {
    partial class SimpleOverlay {
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
            this.drawPanel = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.drawPanel)).BeginInit();
            this.SuspendLayout();
            // 
            // drawPanel
            // 
            this.drawPanel.BackColor = System.Drawing.Color.Purple;
            this.drawPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.drawPanel.Location = new System.Drawing.Point(0, 0);
            this.drawPanel.Name = "drawPanel";
            this.drawPanel.Size = new System.Drawing.Size(735, 595);
            this.drawPanel.TabIndex = 1;
            this.drawPanel.TabStop = false;
            this.drawPanel.Click += new System.EventHandler(this.drawPanel_Click);
            this.drawPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.drawPanel_Paint);
            // 
            // SimpleOverlay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(735, 595);
            this.Controls.Add(this.drawPanel);
            this.Name = "SimpleOverlay";
            this.Text = "SimpleOverlay";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SimpleOverlay_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SimpleOverlay_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.SimpleOverlay_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.drawPanel)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox drawPanel;
    }
}
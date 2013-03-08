namespace Chimera.Inputs {
    partial class KBMousePanel {
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
            this.mousePanel = new System.Windows.Forms.Panel();
            this.ignorePitchCheck = new System.Windows.Forms.CheckBox();
            this.mouseScaleSlider = new System.Windows.Forms.TrackBar();
            this.mouseContainer = new System.Windows.Forms.GroupBox();
            this.moveScaleSlider = new System.Windows.Forms.TrackBar();
            this.mousePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mouseScaleSlider)).BeginInit();
            this.mouseContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.moveScaleSlider)).BeginInit();
            this.SuspendLayout();
            // 
            // mousePanel
            // 
            this.mousePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mousePanel.Controls.Add(this.moveScaleSlider);
            this.mousePanel.Controls.Add(this.mouseScaleSlider);
            this.mousePanel.Controls.Add(this.ignorePitchCheck);
            this.mousePanel.Location = new System.Drawing.Point(3, 13);
            this.mousePanel.Name = "mousePanel";
            this.mousePanel.Size = new System.Drawing.Size(812, 485);
            this.mousePanel.TabIndex = 7;
            // 
            // ignorePitchCheck
            // 
            this.ignorePitchCheck.AutoSize = true;
            this.ignorePitchCheck.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ignorePitchCheck.Location = new System.Drawing.Point(8, 3);
            this.ignorePitchCheck.Name = "ignorePitchCheck";
            this.ignorePitchCheck.Size = new System.Drawing.Size(83, 17);
            this.ignorePitchCheck.TabIndex = 0;
            this.ignorePitchCheck.Text = "Ignore Pitch";
            this.ignorePitchCheck.UseVisualStyleBackColor = true;
            // 
            // mouseScaleSlider
            // 
            this.mouseScaleSlider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mouseScaleSlider.Location = new System.Drawing.Point(3, 412);
            this.mouseScaleSlider.Maximum = 40;
            this.mouseScaleSlider.Minimum = 10;
            this.mouseScaleSlider.Name = "mouseScaleSlider";
            this.mouseScaleSlider.Size = new System.Drawing.Size(806, 42);
            this.mouseScaleSlider.TabIndex = 10;
            this.mouseScaleSlider.Value = 20;
            // 
            // mouseContainer
            // 
            this.mouseContainer.Controls.Add(this.mousePanel);
            this.mouseContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mouseContainer.Location = new System.Drawing.Point(0, 0);
            this.mouseContainer.Name = "mouseContainer";
            this.mouseContainer.Size = new System.Drawing.Size(821, 504);
            this.mouseContainer.TabIndex = 11;
            this.mouseContainer.TabStop = false;
            this.mouseContainer.Text = "Mouselook";
            // 
            // moveScaleSlider
            // 
            this.moveScaleSlider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.moveScaleSlider.Location = new System.Drawing.Point(3, 443);
            this.moveScaleSlider.Maximum = 40;
            this.moveScaleSlider.Minimum = 10;
            this.moveScaleSlider.Name = "moveScaleSlider";
            this.moveScaleSlider.Size = new System.Drawing.Size(806, 42);
            this.moveScaleSlider.TabIndex = 9;
            this.moveScaleSlider.Value = 20;
            // 
            // KBMousePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mouseContainer);
            this.Name = "KBMousePanel";
            this.Size = new System.Drawing.Size(821, 504);
            this.mousePanel.ResumeLayout(false);
            this.mousePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mouseScaleSlider)).EndInit();
            this.mouseContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.moveScaleSlider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel mousePanel;
        private System.Windows.Forms.CheckBox ignorePitchCheck;
        private System.Windows.Forms.TrackBar mouseScaleSlider;
        private System.Windows.Forms.GroupBox mouseContainer;
        private System.Windows.Forms.TrackBar moveScaleSlider;
    }
}

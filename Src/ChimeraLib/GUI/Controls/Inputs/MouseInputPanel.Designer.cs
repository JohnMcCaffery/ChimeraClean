namespace Chimera.GUI.Controls.Inputs {
    partial class MouseInputPanel {
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
            this.positionLabel = new System.Windows.Forms.Label();
            this.cursorHandleLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // positionLabel
            // 
            this.positionLabel.AutoSize = true;
            this.positionLabel.Location = new System.Drawing.Point(3, 0);
            this.positionLabel.Name = "positionLabel";
            this.positionLabel.Size = new System.Drawing.Size(24, 13);
            this.positionLabel.TabIndex = 0;
            this.positionLabel.Text = "X,Y";
            // 
            // cursorHandleLabel
            // 
            this.cursorHandleLabel.AutoSize = true;
            this.cursorHandleLabel.Location = new System.Drawing.Point(6, 40);
            this.cursorHandleLabel.Name = "cursorHandleLabel";
            this.cursorHandleLabel.Size = new System.Drawing.Size(74, 13);
            this.cursorHandleLabel.TabIndex = 1;
            this.cursorHandleLabel.Text = "Cursor Handle";
            // 
            // MouseInputPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cursorHandleLabel);
            this.Controls.Add(this.positionLabel);
            this.Name = "MouseInputPanel";
            this.Size = new System.Drawing.Size(460, 432);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label positionLabel;
        private System.Windows.Forms.Label cursorHandleLabel;
    }
}

namespace Chimera.Inputs {
    partial class CameraControlForm {
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
            this.SuspendLayout();
            // 
            // CameraControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Name = "CameraControlForm";
            this.Text = "Camera";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.CameraControlForm_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CameraControlForm_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.CameraControlForm_KeyUp);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.CameraControlForm_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CameraControlForm_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion

    }
}
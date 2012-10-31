namespace UtilLib {
    partial class LogPanel {
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
            this.debugTextBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // debugTextBox
            // 
            this.debugTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.debugTextBox.Location = new System.Drawing.Point(0, 0);
            this.debugTextBox.Name = "debugTextBox";
            this.debugTextBox.Size = new System.Drawing.Size(150, 150);
            this.debugTextBox.TabIndex = 0;
            this.debugTextBox.Text = "";
            // 
            // LogPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.debugTextBox);
            this.Name = "LogPanel";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox debugTextBox;
    }
}

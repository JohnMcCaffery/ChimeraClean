namespace Chimera.Overlay.GUI.Plugins {
    partial class AxisCursorPanel {
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
            this.axesBox = new System.Windows.Forms.GroupBox();
            this.SuspendLayout();
            // 
            // axesBox
            // 
            this.axesBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axesBox.Location = new System.Drawing.Point(0, 0);
            this.axesBox.Name = "axesBox";
            this.axesBox.Size = new System.Drawing.Size(1213, 665);
            this.axesBox.TabIndex = 1;
            this.axesBox.TabStop = false;
            this.axesBox.Text = "Axes";
            // 
            // AxisCursorPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.axesBox);
            this.Name = "AxisCursorPanel";
            this.Size = new System.Drawing.Size(1213, 665);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox axesBox;

    }
}

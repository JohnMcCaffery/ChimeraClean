/*************************************************************************
Copyright (c) 2012 John McCaffery 

This file is part of Chimera.

Chimera is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Chimera is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Chimera.  If not, see <http://www.gnu.org/licenses/>.

**************************************************************************/
namespace Chimera.Flythrough.GUI {
    partial class BlankPanel<T> {
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
            this.lengthValue = new System.Windows.Forms.NumericUpDown();
            this.Length = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.lengthValue)).BeginInit();
            this.SuspendLayout();
            // 
            // lengthValue
            // 
            this.lengthValue.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.lengthValue.Location = new System.Drawing.Point(66, 3);
            this.lengthValue.Maximum = new decimal(new int[] {
            60000,
            0,
            0,
            0});
            this.lengthValue.Name = "lengthValue";
            this.lengthValue.Size = new System.Drawing.Size(60, 20);
            this.lengthValue.TabIndex = 15;
            this.lengthValue.Value = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            // 
            // Length
            // 
            this.Length.AutoSize = true;
            this.Length.Location = new System.Drawing.Point(1, 5);
            this.Length.Name = "Length";
            this.Length.Size = new System.Drawing.Size(59, 13);
            this.Length.TabIndex = 14;
            this.Length.Text = "Length(ms)";
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(4, 29);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(455, 23);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar.TabIndex = 16;
            // 
            // BlankPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.lengthValue);
            this.Controls.Add(this.Length);
            this.Name = "BlankPanel";
            this.Size = new System.Drawing.Size(462, 156);
            ((System.ComponentModel.ISupportInitialize)(this.lengthValue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown lengthValue;
        private System.Windows.Forms.Label Length;
        private System.Windows.Forms.ProgressBar progressBar;
    }
}

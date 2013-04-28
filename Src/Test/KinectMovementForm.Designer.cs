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
namespace Test {
    partial class KinectMovementForm {
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
            this.dolphinMovementPanel1 = new Chimera.Kinect.GUI.DolphinMovementPanel();
            this.SuspendLayout();
            // 
            // dolphinMovementPanel1
            // 
            this.dolphinMovementPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dolphinMovementPanel1.Location = new System.Drawing.Point(0, 0);
            this.dolphinMovementPanel1.Name = "dolphinMovementPanel1";
            this.dolphinMovementPanel1.Size = new System.Drawing.Size(549, 478);
            this.dolphinMovementPanel1.TabIndex = 0;
            // 
            // KinectMovementForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(549, 478);
            this.Controls.Add(this.dolphinMovementPanel1);
            this.Name = "KinectMovementForm";
            this.Text = "KinectMovementForm";
            this.ResumeLayout(false);

        }

        #endregion

        private Chimera.Kinect.GUI.DolphinMovementPanel dolphinMovementPanel1;
    }
}

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
namespace Chimera.Kinect.GUI {
    partial class KinectFramePlugin {
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
            this.colourFrameButton = new System.Windows.Forms.RadioButton();
            this.depthFrameButton = new System.Windows.Forms.RadioButton();
            this.frameImage = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.frameImage)).BeginInit();
            this.SuspendLayout();
            // 
            // colourFrameButton
            // 
            this.colourFrameButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.colourFrameButton.AutoSize = true;
            this.colourFrameButton.BackColor = System.Drawing.Color.Transparent;
            this.colourFrameButton.Location = new System.Drawing.Point(433, 419);
            this.colourFrameButton.Name = "colourFrameButton";
            this.colourFrameButton.Size = new System.Drawing.Size(55, 17);
            this.colourFrameButton.TabIndex = 5;
            this.colourFrameButton.Text = "Colour";
            this.colourFrameButton.UseVisualStyleBackColor = false;
            // 
            // depthFrameButton
            // 
            this.depthFrameButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.depthFrameButton.AutoSize = true;
            this.depthFrameButton.BackColor = System.Drawing.Color.Transparent;
            this.depthFrameButton.Checked = true;
            this.depthFrameButton.Location = new System.Drawing.Point(373, 419);
            this.depthFrameButton.Name = "depthFrameButton";
            this.depthFrameButton.Size = new System.Drawing.Size(54, 17);
            this.depthFrameButton.TabIndex = 4;
            this.depthFrameButton.TabStop = true;
            this.depthFrameButton.Text = "Depth";
            this.depthFrameButton.UseVisualStyleBackColor = false;
            // 
            // frameImage
            // 
            this.frameImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.frameImage.Location = new System.Drawing.Point(0, 0);
            this.frameImage.Name = "frameImage";
            this.frameImage.Size = new System.Drawing.Size(492, 439);
            this.frameImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.frameImage.TabIndex = 3;
            this.frameImage.TabStop = false;
            // 
            // KinectFramePlugin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.colourFrameButton);
            this.Controls.Add(this.depthFrameButton);
            this.Controls.Add(this.frameImage);
            this.Name = "KinectFramePlugin";
            this.Size = new System.Drawing.Size(492, 439);
            ((System.ComponentModel.ISupportInitialize)(this.frameImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton colourFrameButton;
        private System.Windows.Forms.RadioButton depthFrameButton;
        private System.Windows.Forms.PictureBox frameImage;
    }
}

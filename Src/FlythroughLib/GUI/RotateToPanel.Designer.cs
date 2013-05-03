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
    partial class RotateToPanel {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RotateToPanel));
            Chimera.Util.Rotation rotation4 = new Chimera.Util.Rotation();
            this.rotateToTakeCurrentButton = new System.Windows.Forms.Button();
            this.lengthValue = new System.Windows.Forms.NumericUpDown();
            this.Length = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.rotationPanel = new Chimera.GUI.RotationPanel();
            ((System.ComponentModel.ISupportInitialize)(this.lengthValue)).BeginInit();
            this.SuspendLayout();
            // 
            // rotateToTakeCurrentButton
            // 
            this.rotateToTakeCurrentButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rotateToTakeCurrentButton.Location = new System.Drawing.Point(0, 125);
            this.rotateToTakeCurrentButton.Name = "rotateToTakeCurrentButton";
            this.rotateToTakeCurrentButton.Size = new System.Drawing.Size(133, 23);
            this.rotateToTakeCurrentButton.TabIndex = 20;
            this.rotateToTakeCurrentButton.Text = "Take Current";
            this.rotateToTakeCurrentButton.UseVisualStyleBackColor = true;
            this.rotateToTakeCurrentButton.Click += new System.EventHandler(this.rotateToTakeCurrentButton_Click);
            // 
            // lengthValue
            // 
            this.lengthValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lengthValue.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.lengthValue.Location = new System.Drawing.Point(68, 99);
            this.lengthValue.Maximum = new decimal(new int[] {
            60000,
            0,
            0,
            0});
            this.lengthValue.Name = "lengthValue";
            this.lengthValue.Size = new System.Drawing.Size(65, 20);
            this.lengthValue.TabIndex = 22;
            this.lengthValue.Value = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            // 
            // Length
            // 
            this.Length.AutoSize = true;
            this.Length.Location = new System.Drawing.Point(3, 99);
            this.Length.Name = "Length";
            this.Length.Size = new System.Drawing.Size(59, 13);
            this.Length.TabIndex = 21;
            this.Length.Text = "Length(ms)";
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(0, 156);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(133, 23);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar.TabIndex = 23;
            // 
            // rotationPanel
            // 
            this.rotationPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rotationPanel.Location = new System.Drawing.Point(0, 0);
            this.rotationPanel.LookAtVector = ((OpenMetaverse.Vector3)(resources.GetObject("rotationPanel.LookAtVector")));
            this.rotationPanel.Name = "rotationPanel";
            this.rotationPanel.Pitch = 0D;
            this.rotationPanel.Quaternion = ((OpenMetaverse.Quaternion)(resources.GetObject("rotationPanel.Quaternion")));
            this.rotationPanel.Size = new System.Drawing.Size(133, 95);
            this.rotationPanel.TabIndex = 24;
            rotation4.LookAtVector = ((OpenMetaverse.Vector3)(resources.GetObject("rotation4.LookAtVector")));
            rotation4.Pitch = 0D;
            rotation4.Quaternion = ((OpenMetaverse.Quaternion)(resources.GetObject("rotation4.Quaternion")));
            rotation4.Yaw = 0D;
            this.rotationPanel.Value = rotation4;
            this.rotationPanel.Yaw = 0D;
            // 
            // RotateToPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rotationPanel);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.lengthValue);
            this.Controls.Add(this.Length);
            this.Controls.Add(this.rotateToTakeCurrentButton);
            this.MinimumSize = new System.Drawing.Size(133, 179);
            this.Name = "RotateToPanel";
            this.Size = new System.Drawing.Size(133, 179);
            ((System.ComponentModel.ISupportInitialize)(this.lengthValue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button rotateToTakeCurrentButton;
        private System.Windows.Forms.NumericUpDown lengthValue;
        private System.Windows.Forms.Label Length;
        private System.Windows.Forms.ProgressBar progressBar;
        private Chimera.GUI.RotationPanel rotationPanel;
    }
}

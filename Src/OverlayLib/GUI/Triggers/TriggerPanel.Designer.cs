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
namespace Chimera.Overlay.GUI.Triggers {
    partial class TriggerPanel {
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
            this.forceTriggerButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // forceTriggerButton
            // 
            this.forceTriggerButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.forceTriggerButton.Location = new System.Drawing.Point(3, 3);
            this.forceTriggerButton.Name = "forceTriggerButton";
            this.forceTriggerButton.Size = new System.Drawing.Size(536, 23);
            this.forceTriggerButton.TabIndex = 12;
            this.forceTriggerButton.Text = "Force Trigger";
            this.forceTriggerButton.UseVisualStyleBackColor = true;
            this.forceTriggerButton.Click += new System.EventHandler(this.forceTriggerButton_Click);
            // 
            // TriggerPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.forceTriggerButton);
            this.Name = "TriggerPanel";
            this.Size = new System.Drawing.Size(542, 492);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button forceTriggerButton;
    }
}

﻿/*************************************************************************
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
namespace Chimera.GUI.Controls.Plugins {
    partial class MousePluginPanel {
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

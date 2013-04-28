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
namespace Chimera.GUI.Controls.Plugins {
    partial class PluginSelectorPanel {
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
            this.inputSelectionBox = new System.Windows.Forms.ComboBox();
            this.inputPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // inputSelectionBox
            // 
            this.inputSelectionBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.inputSelectionBox.DisplayMember = "Name";
            this.inputSelectionBox.FormattingEnabled = true;
            this.inputSelectionBox.Location = new System.Drawing.Point(3, 3);
            this.inputSelectionBox.Name = "inputSelectionBox";
            this.inputSelectionBox.Size = new System.Drawing.Size(370, 21);
            this.inputSelectionBox.TabIndex = 0;
            this.inputSelectionBox.SelectedIndexChanged += new System.EventHandler(this.inputSelectionBox_SelectedIndexChanged);
            // 
            // inputPanel
            // 
            this.inputPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.inputPanel.Location = new System.Drawing.Point(3, 30);
            this.inputPanel.Name = "inputPanel";
            this.inputPanel.Size = new System.Drawing.Size(370, 151);
            this.inputPanel.TabIndex = 1;
            // 
            // SelectableInputPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.inputPanel);
            this.Controls.Add(this.inputSelectionBox);
            this.Name = "SelectableInputPanel";
            this.Size = new System.Drawing.Size(376, 184);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox inputSelectionBox;
        private System.Windows.Forms.Panel inputPanel;
    }
}

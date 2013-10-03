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
    partial class AxisBasedDeltaPanel {
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.axesPanel = new System.Windows.Forms.Panel();
            this.rotXMovePanel = new Chimera.GUI.ScalarPanel();
            this.scalePanel = new Chimera.GUI.ScalarPanel();
            this.axesPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // axesBox
            // 
            this.axesBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.axesBox.Location = new System.Drawing.Point(3, 3);
            this.axesBox.Name = "axesBox";
            this.axesBox.Size = new System.Drawing.Size(494, 381);
            this.axesBox.TabIndex = 0;
            this.axesBox.TabStop = false;
            this.axesBox.Text = "Axes";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Scale";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Rot * Move";
            // 
            // axesPanel
            // 
            this.axesPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.axesPanel.AutoScroll = true;
            this.axesPanel.Controls.Add(this.axesBox);
            this.axesPanel.Location = new System.Drawing.Point(0, 55);
            this.axesPanel.Name = "axesPanel";
            this.axesPanel.Size = new System.Drawing.Size(500, 387);
            this.axesPanel.TabIndex = 5;
            // 
            // rotXMovePanel
            // 
            this.rotXMovePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rotXMovePanel.Location = new System.Drawing.Point(71, 29);
            this.rotXMovePanel.Max = 10F;
            this.rotXMovePanel.Min = 0F;
            this.rotXMovePanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.rotXMovePanel.Name = "rotXMovePanel";
            this.rotXMovePanel.Size = new System.Drawing.Size(426, 20);
            this.rotXMovePanel.TabIndex = 3;
            this.rotXMovePanel.Value = 0F;
            this.rotXMovePanel.ValueChanged += new System.Action<float>(this.rotXMove_ValueChanged);
            // 
            // scalePanel
            // 
            this.scalePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scalePanel.Location = new System.Drawing.Point(44, 3);
            this.scalePanel.Max = 4F;
            this.scalePanel.Min = 0F;
            this.scalePanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.scalePanel.Name = "scalePanel";
            this.scalePanel.Size = new System.Drawing.Size(316, 20);
            this.scalePanel.TabIndex = 1;
            this.scalePanel.Value = 0F;
            this.scalePanel.ValueChanged += new System.Action<float>(this.scalePanel_ValueChanged);
            // 
            // AxisBasedDeltaPanel
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.axesPanel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.rotXMovePanel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.scalePanel);
            this.Name = "AxisBasedDeltaPanel";
            this.Size = new System.Drawing.Size(500, 442);
            this.axesPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox axesBox;
        private ScalarPanel scalePanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private ScalarPanel rotXMovePanel;
        private System.Windows.Forms.Panel axesPanel;
    }
}

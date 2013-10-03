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
    partial class ConstrainedAxisPanel {
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
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.outputPanel = new Chimera.GUI.UpdatedScalarPanel();
            this.rawPanel = new Chimera.GUI.UpdatedScalarPanel();
            this.scalePanel = new Chimera.GUI.UpdatedScalarPanel();
            this.deadzonePanel = new Chimera.GUI.UpdatedScalarPanel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Deadzone";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 29);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Scale";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Output";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Raw";
            // 
            // outputPanel
            // 
            this.outputPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.outputPanel.Location = new System.Drawing.Point(46, 29);
            this.outputPanel.Max = 3F;
            this.outputPanel.Min = -3F;
            this.outputPanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.outputPanel.Name = "outputPanel";
            this.outputPanel.Scalar = null;
            this.outputPanel.Size = new System.Drawing.Size(116, 20);
            this.outputPanel.TabIndex = 9;
            this.outputPanel.Value = 0F;
            // 
            // rawPanel
            // 
            this.rawPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rawPanel.Location = new System.Drawing.Point(36, 3);
            this.rawPanel.Max = 100F;
            this.rawPanel.Min = -100F;
            this.rawPanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.rawPanel.Name = "rawPanel";
            this.rawPanel.Scalar = null;
            this.rawPanel.Size = new System.Drawing.Size(126, 20);
            this.rawPanel.TabIndex = 8;
            this.rawPanel.Value = 0F;
            // 
            // scalePanel
            // 
            this.scalePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.scalePanel.Location = new System.Drawing.Point(43, 29);
            this.scalePanel.Max = 10F;
            this.scalePanel.Min = -10F;
            this.scalePanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.scalePanel.Name = "scalePanel";
            this.scalePanel.Scalar = null;
            this.scalePanel.Size = new System.Drawing.Size(120, 20);
            this.scalePanel.TabIndex = 3;
            this.scalePanel.Value = 0F;
            // 
            // deadzonePanel
            // 
            this.deadzonePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.deadzonePanel.Location = new System.Drawing.Point(68, 3);
            this.deadzonePanel.Max = 100F;
            this.deadzonePanel.Min = 0F;
            this.deadzonePanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.deadzonePanel.Name = "deadzonePanel";
            this.deadzonePanel.Scalar = null;
            this.deadzonePanel.Size = new System.Drawing.Size(95, 20);
            this.deadzonePanel.TabIndex = 0;
            this.deadzonePanel.Value = 0F;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.deadzonePanel);
            this.splitContainer1.Panel1.Controls.Add(this.label4);
            this.splitContainer1.Panel1.Controls.Add(this.scalePanel);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.rawPanel);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Panel2.Controls.Add(this.outputPanel);
            this.splitContainer1.Panel2.Controls.Add(this.label3);
            this.splitContainer1.Size = new System.Drawing.Size(333, 50);
            this.splitContainer1.SplitterDistance = 164;
            this.splitContainer1.TabIndex = 12;
            // 
            // ConstrainedAxisPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.MinimumSize = new System.Drawing.Size(333, 50);
            this.Name = "ConstrainedAxisPanel";
            this.Size = new System.Drawing.Size(333, 50);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private UpdatedScalarPanel deadzonePanel;
        private UpdatedScalarPanel scalePanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private UpdatedScalarPanel outputPanel;
        private UpdatedScalarPanel rawPanel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}

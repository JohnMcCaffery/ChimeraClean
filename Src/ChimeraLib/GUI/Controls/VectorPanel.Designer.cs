/*************************************************************************
Copyright (c) 2012 John McCaffery 

This file is part of Armadillo SlaveProxy.

Routing Project is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Routing Project is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Routing Project.  If not, see <http://www.gnu.org/licenses/>.

**************************************************************************/

namespace Chimera.GUI {
    partial class VectorPanel {
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
            this.nameLabel = new System.Windows.Forms.Label();
            this.xLabel = new System.Windows.Forms.Label();
            this.yLabel = new System.Windows.Forms.Label();
            this.zLabel = new System.Windows.Forms.Label();
            this.xPanel = new Chimera.GUI.ScalarPanel();
            this.yPanel = new Chimera.GUI.ScalarPanel();
            this.zPanel = new Chimera.GUI.ScalarPanel();
            this.SuspendLayout();
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(3, 0);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(35, 13);
            this.nameLabel.TabIndex = 3;
            this.nameLabel.Text = "Name";
            // 
            // xLabel
            // 
            this.xLabel.AutoSize = true;
            this.xLabel.Location = new System.Drawing.Point(3, 18);
            this.xLabel.Name = "xLabel";
            this.xLabel.Size = new System.Drawing.Size(14, 13);
            this.xLabel.TabIndex = 11;
            this.xLabel.Text = "X";
            // 
            // yLabel
            // 
            this.yLabel.AutoSize = true;
            this.yLabel.Location = new System.Drawing.Point(3, 44);
            this.yLabel.Name = "yLabel";
            this.yLabel.Size = new System.Drawing.Size(14, 13);
            this.yLabel.TabIndex = 12;
            this.yLabel.Text = "Y";
            // 
            // zLabel
            // 
            this.zLabel.AutoSize = true;
            this.zLabel.Location = new System.Drawing.Point(3, 70);
            this.zLabel.Name = "zLabel";
            this.zLabel.Size = new System.Drawing.Size(14, 13);
            this.zLabel.TabIndex = 13;
            this.zLabel.Text = "Z";
            // 
            // xPanel
            // 
            this.xPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.xPanel.Location = new System.Drawing.Point(19, 15);
            this.xPanel.Max = 10F;
            this.xPanel.Min = -10F;
            this.xPanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.xPanel.Name = "xPanel";
            this.xPanel.Size = new System.Drawing.Size(326, 20);
            this.xPanel.TabIndex = 14;
            this.xPanel.Value = 0F;
            this.xPanel.ValueChanged += new System.Action<float>(this.panel_Changed);
            // 
            // yPanel
            // 
            this.yPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.yPanel.Location = new System.Drawing.Point(19, 41);
            this.yPanel.Max = 10F;
            this.yPanel.Min = -10F;
            this.yPanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.yPanel.Name = "yPanel";
            this.yPanel.Size = new System.Drawing.Size(326, 20);
            this.yPanel.TabIndex = 15;
            this.yPanel.Value = 0F;
            this.yPanel.ValueChanged += new System.Action<float>(this.panel_Changed);
            // 
            // zPanel
            // 
            this.zPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.zPanel.Location = new System.Drawing.Point(19, 67);
            this.zPanel.Max = 10F;
            this.zPanel.Min = -10F;
            this.zPanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.zPanel.Name = "zPanel";
            this.zPanel.Size = new System.Drawing.Size(326, 20);
            this.zPanel.TabIndex = 16;
            this.zPanel.Value = 0F;
            this.zPanel.ValueChanged += new System.Action<float>(this.panel_Changed);
            // 
            // VectorPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.zPanel);
            this.Controls.Add(this.yPanel);
            this.Controls.Add(this.xPanel);
            this.Controls.Add(this.zLabel);
            this.Controls.Add(this.yLabel);
            this.Controls.Add(this.xLabel);
            this.Controls.Add(this.nameLabel);
            this.MinimumSize = new System.Drawing.Size(103, 95);
            this.Name = "VectorPanel";
            this.Size = new System.Drawing.Size(345, 95);
            this.EnabledChanged += new System.EventHandler(this.VectorPanel_EnabledChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Label xLabel;
        private System.Windows.Forms.Label yLabel;
        private System.Windows.Forms.Label zLabel;
        private Chimera.GUI.ScalarPanel xPanel;
        private Chimera.GUI.ScalarPanel yPanel;
        private Chimera.GUI.ScalarPanel zPanel;
    }
}

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

namespace ProxyTestGUI {
    partial class RotationPanel {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RotationPanel));
            this.vectorPanel = new ProxyTestGUI.VectorPanel();
            this.yawValue = new System.Windows.Forms.NumericUpDown();
            this.yawSlider = new System.Windows.Forms.TrackBar();
            this.pitchValue = new System.Windows.Forms.NumericUpDown();
            this.pitchSlider = new System.Windows.Forms.TrackBar();
            this.yawLabel = new System.Windows.Forms.Label();
            this.pitchLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.yawValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.yawSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pitchValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pitchSlider)).BeginInit();
            this.SuspendLayout();
            // 
            // vectorPanel
            // 
            this.vectorPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vectorPanel.DisplayName = "Name";
            this.vectorPanel.Location = new System.Drawing.Point(3, 3);
            this.vectorPanel.Max = 1D;
            this.vectorPanel.Min = -1D;
            this.vectorPanel.Name = "vectorPanel";
            this.vectorPanel.Size = new System.Drawing.Size(404, 98);
            this.vectorPanel.TabIndex = 0;
            this.vectorPanel.X = 1F;
            this.vectorPanel.Y = 0F;
            this.vectorPanel.Z = 0F;
            this.vectorPanel.OnChange += new System.EventHandler(this.vectorPanel_OnChange);
            // 
            // yawValue
            // 
            this.yawValue.DecimalPlaces = 2;
            this.yawValue.Location = new System.Drawing.Point(26, 96);
            this.yawValue.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.yawValue.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
            this.yawValue.Name = "yawValue";
            this.yawValue.Size = new System.Drawing.Size(63, 20);
            this.yawValue.TabIndex = 83;
            this.yawValue.ValueChanged += new System.EventHandler(this.yawValue_ValueChanged);
            // 
            // yawSlider
            // 
            this.yawSlider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.yawSlider.Location = new System.Drawing.Point(87, 96);
            this.yawSlider.Maximum = 180;
            this.yawSlider.Minimum = -180;
            this.yawSlider.Name = "yawSlider";
            this.yawSlider.Size = new System.Drawing.Size(320, 42);
            this.yawSlider.TabIndex = 84;
            this.yawSlider.TickStyle = System.Windows.Forms.TickStyle.None;
            this.yawSlider.Scroll += new System.EventHandler(this.yawSlider_Scroll);
            // 
            // pitchValue
            // 
            this.pitchValue.DecimalPlaces = 2;
            this.pitchValue.Location = new System.Drawing.Point(26, 122);
            this.pitchValue.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.pitchValue.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
            this.pitchValue.Name = "pitchValue";
            this.pitchValue.Size = new System.Drawing.Size(63, 20);
            this.pitchValue.TabIndex = 85;
            this.pitchValue.ValueChanged += new System.EventHandler(this.pitchValue_ValueChanged);
            // 
            // pitchSlider
            // 
            this.pitchSlider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pitchSlider.Location = new System.Drawing.Point(87, 122);
            this.pitchSlider.Maximum = 180;
            this.pitchSlider.Minimum = -180;
            this.pitchSlider.Name = "pitchSlider";
            this.pitchSlider.Size = new System.Drawing.Size(320, 42);
            this.pitchSlider.TabIndex = 86;
            this.pitchSlider.TickStyle = System.Windows.Forms.TickStyle.None;
            this.pitchSlider.Scroll += new System.EventHandler(this.pitchSlider_Scroll);
            // 
            // yawLabel
            // 
            this.yawLabel.AutoSize = true;
            this.yawLabel.Location = new System.Drawing.Point(6, 98);
            this.yawLabel.Name = "yawLabel";
            this.yawLabel.Size = new System.Drawing.Size(14, 13);
            this.yawLabel.TabIndex = 87;
            this.yawLabel.Text = "Y";
            // 
            // pitchLabel
            // 
            this.pitchLabel.AutoSize = true;
            this.pitchLabel.Location = new System.Drawing.Point(6, 124);
            this.pitchLabel.Name = "pitchLabel";
            this.pitchLabel.Size = new System.Drawing.Size(14, 13);
            this.pitchLabel.TabIndex = 88;
            this.pitchLabel.Text = "P";
            // 
            // RotationPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pitchLabel);
            this.Controls.Add(this.yawValue);
            this.Controls.Add(this.yawLabel);
            this.Controls.Add(this.pitchValue);
            this.Controls.Add(this.pitchSlider);
            this.Controls.Add(this.yawSlider);
            this.Controls.Add(this.vectorPanel);
            this.Name = "RotationPanel";
            this.Size = new System.Drawing.Size(410, 147);
            ((System.ComponentModel.ISupportInitialize)(this.yawValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.yawSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pitchValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pitchSlider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private VectorPanel vectorPanel;
        private System.Windows.Forms.NumericUpDown yawValue;
        private System.Windows.Forms.TrackBar yawSlider;
        private System.Windows.Forms.NumericUpDown pitchValue;
        private System.Windows.Forms.TrackBar pitchSlider;
        private System.Windows.Forms.Label yawLabel;
        private System.Windows.Forms.Label pitchLabel;
    }
}

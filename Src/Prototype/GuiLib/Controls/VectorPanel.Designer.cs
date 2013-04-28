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
            this.xSlider = new System.Windows.Forms.TrackBar();
            this.nameLabel = new System.Windows.Forms.Label();
            this.xValue = new System.Windows.Forms.NumericUpDown();
            this.yValue = new System.Windows.Forms.NumericUpDown();
            this.zValue = new System.Windows.Forms.NumericUpDown();
            this.ySlider = new System.Windows.Forms.TrackBar();
            this.zSlider = new System.Windows.Forms.TrackBar();
            this.xLabel = new System.Windows.Forms.Label();
            this.yLabel = new System.Windows.Forms.Label();
            this.zLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.xSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.yValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.zValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ySlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.zSlider)).BeginInit();
            this.SuspendLayout();
            // 
            // xSlider
            // 
            this.xSlider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.xSlider.Location = new System.Drawing.Point(83, 16);
            this.xSlider.Maximum = 25500;
            this.xSlider.Name = "xSlider";
            this.xSlider.Size = new System.Drawing.Size(413, 42);
            this.xSlider.TabIndex = 1;
            this.xSlider.TickFrequency = 80;
            this.xSlider.TickStyle = System.Windows.Forms.TickStyle.None;
            this.xSlider.Scroll += new System.EventHandler(this.xSlider_Scroll);
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
            // xValue
            // 
            this.xValue.DecimalPlaces = 2;
            this.xValue.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.xValue.Location = new System.Drawing.Point(23, 16);
            this.xValue.Maximum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.xValue.Minimum = new decimal(new int[] {
            1024,
            0,
            0,
            -2147483648});
            this.xValue.Name = "xValue";
            this.xValue.Size = new System.Drawing.Size(63, 20);
            this.xValue.TabIndex = 6;
            this.xValue.ValueChanged += new System.EventHandler(this.xValue_ValueChanged);
            // 
            // yValue
            // 
            this.yValue.DecimalPlaces = 2;
            this.yValue.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.yValue.Location = new System.Drawing.Point(23, 42);
            this.yValue.Maximum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.yValue.Minimum = new decimal(new int[] {
            1024,
            0,
            0,
            -2147483648});
            this.yValue.Name = "yValue";
            this.yValue.Size = new System.Drawing.Size(63, 20);
            this.yValue.TabIndex = 7;
            this.yValue.ValueChanged += new System.EventHandler(this.yValue_ValueChanged);
            // 
            // zValue
            // 
            this.zValue.DecimalPlaces = 2;
            this.zValue.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.zValue.Location = new System.Drawing.Point(23, 68);
            this.zValue.Maximum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.zValue.Minimum = new decimal(new int[] {
            1024,
            0,
            0,
            -2147483648});
            this.zValue.Name = "zValue";
            this.zValue.Size = new System.Drawing.Size(63, 20);
            this.zValue.TabIndex = 8;
            this.zValue.ValueChanged += new System.EventHandler(this.zValue_ValueChanged);
            // 
            // ySlider
            // 
            this.ySlider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ySlider.Location = new System.Drawing.Point(83, 42);
            this.ySlider.Maximum = 25500;
            this.ySlider.Name = "ySlider";
            this.ySlider.Size = new System.Drawing.Size(413, 42);
            this.ySlider.TabIndex = 9;
            this.ySlider.TickFrequency = 80;
            this.ySlider.TickStyle = System.Windows.Forms.TickStyle.None;
            this.ySlider.Scroll += new System.EventHandler(this.ySlider_Scroll);
            // 
            // zSlider
            // 
            this.zSlider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.zSlider.Location = new System.Drawing.Point(83, 68);
            this.zSlider.Maximum = 25500;
            this.zSlider.Name = "zSlider";
            this.zSlider.Size = new System.Drawing.Size(413, 42);
            this.zSlider.TabIndex = 10;
            this.zSlider.TickFrequency = 80;
            this.zSlider.TickStyle = System.Windows.Forms.TickStyle.None;
            this.zSlider.Scroll += new System.EventHandler(this.zSlider_Scroll);
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
            // VectorPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.zValue);
            this.Controls.Add(this.yValue);
            this.Controls.Add(this.xValue);
            this.Controls.Add(this.zLabel);
            this.Controls.Add(this.yLabel);
            this.Controls.Add(this.xLabel);
            this.Controls.Add(this.zSlider);
            this.Controls.Add(this.ySlider);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.xSlider);
            this.Name = "VectorPanel";
            this.Size = new System.Drawing.Size(496, 98);
            this.EnabledChanged += new System.EventHandler(this.VectorPanel_EnabledChanged);
            ((System.ComponentModel.ISupportInitialize)(this.xSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.yValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.zValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ySlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.zSlider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar xSlider;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.NumericUpDown xValue;
        private System.Windows.Forms.NumericUpDown yValue;
        private System.Windows.Forms.NumericUpDown zValue;
        private System.Windows.Forms.TrackBar ySlider;
        private System.Windows.Forms.TrackBar zSlider;
        private System.Windows.Forms.Label xLabel;
        private System.Windows.Forms.Label yLabel;
        private System.Windows.Forms.Label zLabel;
    }
}

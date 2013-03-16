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
            this.rollValue = new System.Windows.Forms.NumericUpDown();
            this.rollSlider = new System.Windows.Forms.TrackBar();
            this.rollLabel = new System.Windows.Forms.Label();
            this.pitchValue = new System.Windows.Forms.NumericUpDown();
            this.pitchSlider = new System.Windows.Forms.TrackBar();
            this.pitchLabel = new System.Windows.Forms.Label();
            this.yawValue = new System.Windows.Forms.NumericUpDown();
            this.yawSlider = new System.Windows.Forms.TrackBar();
            this.yawLabel = new System.Windows.Forms.Label();
            this.rpyButton = new System.Windows.Forms.RadioButton();
            this.lookAtButton = new System.Windows.Forms.RadioButton();
            this.vectorPanel = new ProxyTestGUI.VectorPanel();
            this.nameLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.rollValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rollSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pitchValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pitchSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.yawValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.yawSlider)).BeginInit();
            this.SuspendLayout();
            // 
            // rollValue
            // 
            this.rollValue.DecimalPlaces = 2;
            this.rollValue.Location = new System.Drawing.Point(23, 16);
            this.rollValue.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.rollValue.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
            this.rollValue.Name = "rollValue";
            this.rollValue.Size = new System.Drawing.Size(63, 20);
            this.rollValue.TabIndex = 83;
            this.rollValue.ValueChanged += new System.EventHandler(this.rollValue_ValueChanged);
            // 
            // rollSlider
            // 
            this.rollSlider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rollSlider.Location = new System.Drawing.Point(83, 16);
            this.rollSlider.Maximum = 180;
            this.rollSlider.Minimum = -180;
            this.rollSlider.Name = "rollSlider";
            this.rollSlider.Size = new System.Drawing.Size(513, 42);
            this.rollSlider.TabIndex = 84;
            this.rollSlider.TickStyle = System.Windows.Forms.TickStyle.None;
            this.rollSlider.Scroll += new System.EventHandler(this.rollSlider_Scroll);
            // 
            // rollLabel
            // 
            this.rollLabel.AutoSize = true;
            this.rollLabel.Location = new System.Drawing.Point(3, 18);
            this.rollLabel.Name = "rollLabel";
            this.rollLabel.Size = new System.Drawing.Size(15, 13);
            this.rollLabel.TabIndex = 87;
            this.rollLabel.Text = "R";
            // 
            // pitchValue
            // 
            this.pitchValue.DecimalPlaces = 2;
            this.pitchValue.Location = new System.Drawing.Point(23, 42);
            this.pitchValue.Maximum = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.pitchValue.Minimum = new decimal(new int[] {
            90,
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
            this.pitchSlider.Location = new System.Drawing.Point(83, 42);
            this.pitchSlider.Maximum = 90;
            this.pitchSlider.Minimum = -90;
            this.pitchSlider.Name = "pitchSlider";
            this.pitchSlider.Size = new System.Drawing.Size(513, 42);
            this.pitchSlider.TabIndex = 86;
            this.pitchSlider.TickStyle = System.Windows.Forms.TickStyle.None;
            this.pitchSlider.Scroll += new System.EventHandler(this.pitchSlider_Scroll);
            // 
            // pitchLabel
            // 
            this.pitchLabel.AutoSize = true;
            this.pitchLabel.Location = new System.Drawing.Point(3, 44);
            this.pitchLabel.Name = "pitchLabel";
            this.pitchLabel.Size = new System.Drawing.Size(14, 13);
            this.pitchLabel.TabIndex = 88;
            this.pitchLabel.Text = "P";
            // 
            // yawValue
            // 
            this.yawValue.DecimalPlaces = 2;
            this.yawValue.Location = new System.Drawing.Point(23, 68);
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
            this.yawValue.TabIndex = 89;
            this.yawValue.ValueChanged += new System.EventHandler(this.yawValue_ValueChanged);
            // 
            // yawSlider
            // 
            this.yawSlider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.yawSlider.Location = new System.Drawing.Point(83, 68);
            this.yawSlider.Maximum = 180;
            this.yawSlider.Minimum = -179;
            this.yawSlider.Name = "yawSlider";
            this.yawSlider.Size = new System.Drawing.Size(513, 42);
            this.yawSlider.TabIndex = 92;
            this.yawSlider.TickStyle = System.Windows.Forms.TickStyle.None;
            this.yawSlider.Scroll += new System.EventHandler(this.yawSlider_Scroll);
            // 
            // yawLabel
            // 
            this.yawLabel.AutoSize = true;
            this.yawLabel.Location = new System.Drawing.Point(3, 70);
            this.yawLabel.Name = "yawLabel";
            this.yawLabel.Size = new System.Drawing.Size(14, 13);
            this.yawLabel.TabIndex = 90;
            this.yawLabel.Text = "Y";
            // 
            // rpyButton
            // 
            this.rpyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rpyButton.AutoSize = true;
            this.rpyButton.Checked = true;
            this.rpyButton.Location = new System.Drawing.Point(437, 3);
            this.rpyButton.Name = "rpyButton";
            this.rpyButton.Size = new System.Drawing.Size(98, 17);
            this.rpyButton.TabIndex = 93;
            this.rpyButton.TabStop = true;
            this.rpyButton.Text = "Roll/Pitch/Yaw";
            this.rpyButton.UseVisualStyleBackColor = true;
            this.rpyButton.CheckedChanged += new System.EventHandler(this.rpyButton_CheckedChanged);
            // 
            // lookAtButton
            // 
            this.lookAtButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lookAtButton.AutoSize = true;
            this.lookAtButton.Location = new System.Drawing.Point(531, 3);
            this.lookAtButton.Name = "lookAtButton";
            this.lookAtButton.Size = new System.Drawing.Size(62, 17);
            this.lookAtButton.TabIndex = 94;
            this.lookAtButton.Text = "Look At";
            this.lookAtButton.UseVisualStyleBackColor = true;
            this.lookAtButton.CheckedChanged += new System.EventHandler(this.lookAtButton_CheckedChanged);
            // 
            // vectorPanel
            // 
            this.vectorPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vectorPanel.Text = "Name";
            this.vectorPanel.Location = new System.Drawing.Point(0, 0);
            this.vectorPanel.Max = 1f;
            this.vectorPanel.Min = -1f;
            this.vectorPanel.MinimumSize = new System.Drawing.Size(103, 95);
            this.vectorPanel.Name = "vectorPanel";
            this.vectorPanel.Size = new System.Drawing.Size(596, 95);
            this.vectorPanel.TabIndex = 0;
            this.vectorPanel.Value = ((OpenMetaverse.Vector3)(resources.GetObject("vectorPanel.Value")));
            this.vectorPanel.Visible = false;
            this.vectorPanel.X = 1F;
            this.vectorPanel.Y = 0F;
            this.vectorPanel.Z = 0F;
            this.vectorPanel.ValueChanged += new System.EventHandler(this.vectorPanel_OnChange);
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(3, 0);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(35, 13);
            this.nameLabel.TabIndex = 95;
            this.nameLabel.Text = "Name";
            // 
            // RotationPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lookAtButton);
            this.Controls.Add(this.rpyButton);
            this.Controls.Add(this.vectorPanel);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.yawValue);
            this.Controls.Add(this.yawSlider);
            this.Controls.Add(this.yawLabel);
            this.Controls.Add(this.pitchValue);
            this.Controls.Add(this.pitchSlider);
            this.Controls.Add(this.pitchLabel);
            this.Controls.Add(this.rollValue);
            this.Controls.Add(this.rollSlider);
            this.Controls.Add(this.rollLabel);
            this.MinimumSize = new System.Drawing.Size(252, 95);
            this.Name = "RotationPanel";
            this.Size = new System.Drawing.Size(596, 95);
            ((System.ComponentModel.ISupportInitialize)(this.rollValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rollSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pitchValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pitchSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.yawValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.yawSlider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private VectorPanel vectorPanel;
        private System.Windows.Forms.NumericUpDown rollValue;
        private System.Windows.Forms.TrackBar rollSlider;
        private System.Windows.Forms.NumericUpDown pitchValue;
        private System.Windows.Forms.TrackBar yawSlider;
        private System.Windows.Forms.NumericUpDown yawValue;
        private System.Windows.Forms.TrackBar pitchSlider;
        private System.Windows.Forms.Label rollLabel;
        private System.Windows.Forms.Label pitchLabel;
        private System.Windows.Forms.Label yawLabel;
        private System.Windows.Forms.RadioButton rpyButton;
        private System.Windows.Forms.RadioButton lookAtButton;
        private System.Windows.Forms.Label nameLabel;
    }
}

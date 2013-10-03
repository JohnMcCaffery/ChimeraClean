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
namespace Chimera.Plugins {
    partial class KBMousePanel {
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
            this.mouseContainer = new System.Windows.Forms.GroupBox();
            this.ignorePitchCheck = new System.Windows.Forms.CheckBox();
            this.mouseScaleSlider = new System.Windows.Forms.TrackBar();
            this.keyboardScaleSlider = new System.Windows.Forms.TrackBar();
            this.showWindowButton = new System.Windows.Forms.Button();
            this.mousePanel = new System.Windows.Forms.Panel();
            this.mouseContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mouseScaleSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.keyboardScaleSlider)).BeginInit();
            this.mousePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mouseContainer
            // 
            this.mouseContainer.Controls.Add(this.mousePanel);
            this.mouseContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mouseContainer.Location = new System.Drawing.Point(0, 0);
            this.mouseContainer.Name = "mouseContainer";
            this.mouseContainer.Size = new System.Drawing.Size(821, 504);
            this.mouseContainer.TabIndex = 11;
            this.mouseContainer.TabStop = false;
            this.mouseContainer.Text = "Mouselook";
            // 
            // ignorePitchCheck
            // 
            this.ignorePitchCheck.AutoSize = true;
            this.ignorePitchCheck.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ignorePitchCheck.Location = new System.Drawing.Point(0, 3);
            this.ignorePitchCheck.Name = "ignorePitchCheck";
            this.ignorePitchCheck.Size = new System.Drawing.Size(83, 17);
            this.ignorePitchCheck.TabIndex = 0;
            this.ignorePitchCheck.Text = "Ignore Pitch";
            this.ignorePitchCheck.UseVisualStyleBackColor = true;
            this.ignorePitchCheck.CheckedChanged += new System.EventHandler(this.ignorePitchCheck_CheckedChanged);
            // 
            // mouseScaleSlider
            // 
            this.mouseScaleSlider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.mouseScaleSlider.Location = new System.Drawing.Point(3, 412);
            this.mouseScaleSlider.Maximum = 1000;
            this.mouseScaleSlider.Minimum = 1;
            this.mouseScaleSlider.Name = "mouseScaleSlider";
            this.mouseScaleSlider.Size = new System.Drawing.Size(806, 45);
            this.mouseScaleSlider.TabIndex = 10;
            this.mouseScaleSlider.TickFrequency = 10;
            this.mouseScaleSlider.Value = 200;
            this.mouseScaleSlider.Scroll += new System.EventHandler(this.mouseScaleSlider_Scroll);
            // 
            // keyboardScaleSlider
            // 
            this.keyboardScaleSlider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.keyboardScaleSlider.Location = new System.Drawing.Point(3, 446);
            this.keyboardScaleSlider.Maximum = 1000;
            this.keyboardScaleSlider.Minimum = 1;
            this.keyboardScaleSlider.Name = "keyboardScaleSlider";
            this.keyboardScaleSlider.Size = new System.Drawing.Size(806, 45);
            this.keyboardScaleSlider.TabIndex = 11;
            this.keyboardScaleSlider.TickFrequency = 10;
            this.keyboardScaleSlider.Value = 200;
            this.keyboardScaleSlider.Scroll += new System.EventHandler(this.keyboardScaleSlider_Scroll);
            // 
            // showWindowButton
            // 
            this.showWindowButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.showWindowButton.Location = new System.Drawing.Point(716, 7);
            this.showWindowButton.Name = "showWindowButton";
            this.showWindowButton.Size = new System.Drawing.Size(92, 23);
            this.showWindowButton.TabIndex = 12;
            this.showWindowButton.Text = "Show Window";
            this.showWindowButton.UseVisualStyleBackColor = true;
            this.showWindowButton.Click += new System.EventHandler(this.showWindowButton_Click);
            // 
            // mousePanel
            // 
            this.mousePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.mousePanel.Controls.Add(this.showWindowButton);
            this.mousePanel.Controls.Add(this.keyboardScaleSlider);
            this.mousePanel.Controls.Add(this.mouseScaleSlider);
            this.mousePanel.Controls.Add(this.ignorePitchCheck);
            this.mousePanel.Location = new System.Drawing.Point(3, 13);
            this.mousePanel.Name = "mousePanel";
            this.mousePanel.Size = new System.Drawing.Size(812, 485);
            this.mousePanel.TabIndex = 7;
            this.mousePanel.Paint += new System.Windows.Forms.PaintEventHandler(this.mousePanel_Paint);
            this.mousePanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.mousePanel_MouseMove);
            this.mousePanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mousePanel_MouseUp);
            // 
            // KBMousePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mouseContainer);
            this.Name = "KBMousePanel";
            this.Size = new System.Drawing.Size(821, 504);
            this.mouseContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mouseScaleSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.keyboardScaleSlider)).EndInit();
            this.mousePanel.ResumeLayout(false);
            this.mousePanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox mouseContainer;
        private System.Windows.Forms.Panel mousePanel;
        private System.Windows.Forms.Button showWindowButton;
        private System.Windows.Forms.TrackBar keyboardScaleSlider;
        private System.Windows.Forms.TrackBar mouseScaleSlider;
        private System.Windows.Forms.CheckBox ignorePitchCheck;
    }
}

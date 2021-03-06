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
namespace Chimera.Kinect.GUI {
    partial class CrossArmsTriggerPanel {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RaiseArmTriggerPanel));
            this.AngleL = new Chimera.GUI.UpdatedScalarPanel();
            this.AngleR = new Chimera.GUI.UpdatedScalarPanel();
            this.AngleThreshold = new Chimera.GUI.UpdatedScalarPanel();
            this.HeightThreshold = new Chimera.GUI.UpdatedScalarPanel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ForeArmL = new Chimera.GUI.UpdatedVectorPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.TriggerL = new Chimera.GUI.ConditionalCheck();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ForeArmR = new Chimera.GUI.UpdatedVectorPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.TriggerR = new Chimera.GUI.ConditionalCheck();
            this.Trigger = new Chimera.GUI.ConditionalCheck();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.forceTriggerButton = new System.Windows.Forms.Button();
            this.hipCentrePanel = new Chimera.GUI.UpdatedVectorPanel();
            this.label5 = new System.Windows.Forms.Label();
            this.DepthThreshold = new Chimera.GUI.UpdatedScalarPanel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // AngleL
            // 
            this.AngleL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.AngleL.Location = new System.Drawing.Point(44, 120);
            this.AngleL.Max = 1F;
            this.AngleL.Min = 0F;
            this.AngleL.MinimumSize = new System.Drawing.Size(95, 20);
            this.AngleL.Name = "AngleL";
            this.AngleL.Scalar = null;
            this.AngleL.Size = new System.Drawing.Size(196, 20);
            this.AngleL.TabIndex = 2;
            this.AngleL.Value = 0F;
            // 
            // AngleR
            // 
            this.AngleR.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.AngleR.Location = new System.Drawing.Point(46, 120);
            this.AngleR.Max = 1F;
            this.AngleR.Min = 0F;
            this.AngleR.MinimumSize = new System.Drawing.Size(95, 20);
            this.AngleR.Name = "AngleR";
            this.AngleR.Scalar = null;
            this.AngleR.Size = new System.Drawing.Size(246, 20);
            this.AngleR.TabIndex = 3;
            this.AngleR.Value = 0F;
            // 
            // AngleThreshold
            // 
            this.AngleThreshold.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.AngleThreshold.Location = new System.Drawing.Point(98, 202);
            this.AngleThreshold.Max = 1F;
            this.AngleThreshold.Min = 0F;
            this.AngleThreshold.MinimumSize = new System.Drawing.Size(95, 20);
            this.AngleThreshold.Name = "AngleThreshold";
            this.AngleThreshold.Scalar = null;
            this.AngleThreshold.Size = new System.Drawing.Size(444, 20);
            this.AngleThreshold.TabIndex = 5;
            this.AngleThreshold.Value = 0F;
            // 
            // HeightThreshold
            // 
            this.HeightThreshold.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.HeightThreshold.Location = new System.Drawing.Point(98, 176);
            this.HeightThreshold.Max = 1F;
            this.HeightThreshold.Min = 0F;
            this.HeightThreshold.MinimumSize = new System.Drawing.Size(95, 20);
            this.HeightThreshold.Name = "HeightThreshold";
            this.HeightThreshold.Scalar = null;
            this.HeightThreshold.Size = new System.Drawing.Size(444, 20);
            this.HeightThreshold.TabIndex = 4;
            this.HeightThreshold.Value = 0F;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer1.Size = new System.Drawing.Size(542, 170);
            this.splitContainer1.SplitterDistance = 243;
            this.splitContainer1.TabIndex = 6;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ForeArmL);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.AngleL);
            this.groupBox1.Controls.Add(this.TriggerL);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(243, 170);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Left";
            // 
            // ArmL
            // 
            this.ForeArmL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ForeArmL.Location = new System.Drawing.Point(7, 19);
            this.ForeArmL.Max = 10F;
            this.ForeArmL.MaxV = ((OpenMetaverse.Vector3)(resources.GetObject("ArmL.MaxV")));
            this.ForeArmL.Min = -10F;
            this.ForeArmL.MinimumSize = new System.Drawing.Size(103, 95);
            this.ForeArmL.MinV = ((OpenMetaverse.Vector3)(resources.GetObject("ArmL.MinV")));
            this.ForeArmL.Name = "ArmL";
            this.ForeArmL.Size = new System.Drawing.Size(230, 95);
            this.ForeArmL.TabIndex = 9;
            this.ForeArmL.Value = ((OpenMetaverse.Vector3)(resources.GetObject("ArmL.Value")));
            this.ForeArmL.Vector = null;
            this.ForeArmL.X = 0F;
            this.ForeArmL.Y = 0F;
            this.ForeArmL.Z = 0F;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 120);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Angle";
            // 
            // TriggerL
            // 
            this.TriggerL.AutoSize = true;
            this.TriggerL.Condition = null;
            this.TriggerL.Location = new System.Drawing.Point(6, 146);
            this.TriggerL.Name = "TriggerL";
            this.TriggerL.Size = new System.Drawing.Size(92, 17);
            this.TriggerL.TabIndex = 7;
            this.TriggerL.Text = "Left Triggered";
            this.TriggerL.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ForeArmR);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.AngleR);
            this.groupBox2.Controls.Add(this.TriggerR);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(295, 170);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Right";
            // 
            // ArmR
            // 
            this.ForeArmR.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ForeArmR.Location = new System.Drawing.Point(6, 19);
            this.ForeArmR.Max = 10F;
            this.ForeArmR.MaxV = ((OpenMetaverse.Vector3)(resources.GetObject("ArmR.MaxV")));
            this.ForeArmR.Min = -10F;
            this.ForeArmR.MinimumSize = new System.Drawing.Size(103, 95);
            this.ForeArmR.MinV = ((OpenMetaverse.Vector3)(resources.GetObject("ArmR.MinV")));
            this.ForeArmR.Name = "ArmR";
            this.ForeArmR.Size = new System.Drawing.Size(283, 95);
            this.ForeArmR.TabIndex = 10;
            this.ForeArmR.Value = ((OpenMetaverse.Vector3)(resources.GetObject("ArmR.Value")));
            this.ForeArmR.Vector = null;
            this.ForeArmR.X = 0F;
            this.ForeArmR.Y = 0F;
            this.ForeArmR.Z = 0F;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 120);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Angle";
            // 
            // TriggerR
            // 
            this.TriggerR.AutoSize = true;
            this.TriggerR.Condition = null;
            this.TriggerR.Location = new System.Drawing.Point(6, 146);
            this.TriggerR.Name = "TriggerR";
            this.TriggerR.Size = new System.Drawing.Size(99, 17);
            this.TriggerR.TabIndex = 8;
            this.TriggerR.Text = "Right Triggered";
            this.TriggerR.UseVisualStyleBackColor = true;
            // 
            // Trigger
            // 
            this.Trigger.AutoSize = true;
            this.Trigger.Condition = null;
            this.Trigger.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Trigger.Location = new System.Drawing.Point(6, 248);
            this.Trigger.Name = "Trigger";
            this.Trigger.Size = new System.Drawing.Size(80, 17);
            this.Trigger.TabIndex = 9;
            this.Trigger.Text = "Triggered";
            this.Trigger.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 176);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Height Threshold";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 202);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Angle Threshold";
            // 
            // forceTriggerButton
            // 
            this.forceTriggerButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.forceTriggerButton.Location = new System.Drawing.Point(92, 248);
            this.forceTriggerButton.Name = "forceTriggerButton";
            this.forceTriggerButton.Size = new System.Drawing.Size(447, 23);
            this.forceTriggerButton.TabIndex = 12;
            this.forceTriggerButton.Text = "Force Trigger";
            this.forceTriggerButton.UseVisualStyleBackColor = true;
            this.forceTriggerButton.Click += new System.EventHandler(this.forceTriggerButton_Click);
            // 
            // hipCentrePanel
            // 
            this.hipCentrePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.hipCentrePanel.Location = new System.Drawing.Point(3, 277);
            this.hipCentrePanel.Max = 10F;
            this.hipCentrePanel.MaxV = ((OpenMetaverse.Vector3)(resources.GetObject("hipCentrePanel.MaxV")));
            this.hipCentrePanel.Min = -10F;
            this.hipCentrePanel.MinimumSize = new System.Drawing.Size(103, 95);
            this.hipCentrePanel.MinV = ((OpenMetaverse.Vector3)(resources.GetObject("hipCentrePanel.MinV")));
            this.hipCentrePanel.Name = "hipCentrePanel";
            this.hipCentrePanel.Size = new System.Drawing.Size(539, 95);
            this.hipCentrePanel.TabIndex = 13;
            this.hipCentrePanel.Value = ((OpenMetaverse.Vector3)(resources.GetObject("hipCentrePanel.Value")));
            this.hipCentrePanel.Vector = null;
            this.hipCentrePanel.X = 0F;
            this.hipCentrePanel.Y = 0F;
            this.hipCentrePanel.Z = 0F;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 228);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(86, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Depth Threshold";
            // 
            // DepthThreshold
            // 
            this.DepthThreshold.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.DepthThreshold.Location = new System.Drawing.Point(98, 228);
            this.DepthThreshold.Max = 1F;
            this.DepthThreshold.Min = 0F;
            this.DepthThreshold.MinimumSize = new System.Drawing.Size(95, 20);
            this.DepthThreshold.Name = "DepthThreshold";
            this.DepthThreshold.Scalar = null;
            this.DepthThreshold.Size = new System.Drawing.Size(444, 20);
            this.DepthThreshold.TabIndex = 14;
            this.DepthThreshold.Value = 0F;
            // 
            // RaiseArmTriggerPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label5);
            this.Controls.Add(this.DepthThreshold);
            this.Controls.Add(this.hipCentrePanel);
            this.Controls.Add(this.forceTriggerButton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Trigger);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.AngleThreshold);
            this.Controls.Add(this.HeightThreshold);
            this.Name = "RaiseArmTriggerPanel";
            this.Size = new System.Drawing.Size(542, 492);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Chimera.GUI.UpdatedScalarPanel AngleL;
        private Chimera.GUI.UpdatedScalarPanel AngleR;
        private Chimera.GUI.UpdatedScalarPanel AngleThreshold;
        private Chimera.GUI.UpdatedScalarPanel HeightThreshold;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private Chimera.GUI.ConditionalCheck TriggerL;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private Chimera.GUI.ConditionalCheck TriggerR;
        private Chimera.GUI.ConditionalCheck Trigger;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button forceTriggerButton;
        private Chimera.GUI.UpdatedVectorPanel ForeArmL;
        private Chimera.GUI.UpdatedVectorPanel ForeArmR;
        private Chimera.GUI.UpdatedVectorPanel hipCentrePanel;
        private System.Windows.Forms.Label label5;
        private Chimera.GUI.UpdatedScalarPanel DepthThreshold;
    }
}

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
namespace Chimera.Kinect.GUI {
    partial class DolphinMovementPanel {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DolphinMovementPanel));
            this.mainTab = new System.Windows.Forms.TabControl();
            this.controlTab = new System.Windows.Forms.TabPage();
            this.enabled = new System.Windows.Forms.CheckBox();
            this.valuePanel = new Chimera.GUI.VectorPanel();
            this.walkTab = new System.Windows.Forms.TabPage();
            this.walkEnabled = new System.Windows.Forms.CheckBox();
            this.lrSplit = new System.Windows.Forms.SplitContainer();
            this.leftBox = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.walkValL = new Chimera.GUI.UpdatedScalarPanel();
            this.walkDiffL = new Chimera.GUI.UpdatedScalarPanel();
            this.rightBox = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.walkDiffR = new Chimera.GUI.UpdatedScalarPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.walkValR = new Chimera.GUI.UpdatedScalarPanel();
            this.label19 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.walkThreshold = new Chimera.GUI.UpdatedScalarPanel();
            this.walkVal = new Chimera.GUI.UpdatedScalarPanel();
            this.walkScale = new Chimera.GUI.UpdatedScalarPanel();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label9 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.flyMin = new Chimera.GUI.UpdatedScalarPanel();
            this.flyTimer = new Chimera.GUI.UpdatedScalarPanel();
            this.flyEnabled = new System.Windows.Forms.CheckBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.constrainedFlyAngleL = new Chimera.GUI.UpdatedScalarPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.constrainedFlyAngleR = new Chimera.GUI.UpdatedScalarPanel();
            this.label12 = new System.Windows.Forms.Label();
            this.flyVal = new Chimera.GUI.UpdatedScalarPanel();
            this.flyMax = new Chimera.GUI.UpdatedScalarPanel();
            this.flyThreshold = new Chimera.GUI.UpdatedScalarPanel();
            this.flyScale = new Chimera.GUI.UpdatedScalarPanel();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.yawEnabled = new System.Windows.Forms.CheckBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.yawValue = new Chimera.GUI.UpdatedScalarPanel();
            this.yawThreshold = new Chimera.GUI.UpdatedScalarPanel();
            this.yawScale = new Chimera.GUI.UpdatedScalarPanel();
            this.yawLean = new Chimera.GUI.UpdatedScalarPanel();
            this.label20 = new System.Windows.Forms.Label();
            this.flyAngleL = new Chimera.GUI.UpdatedScalarPanel();
            this.flyAngleR = new Chimera.GUI.UpdatedScalarPanel();
            this.label21 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.mainTab.SuspendLayout();
            this.controlTab.SuspendLayout();
            this.walkTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lrSplit)).BeginInit();
            this.lrSplit.Panel1.SuspendLayout();
            this.lrSplit.Panel2.SuspendLayout();
            this.lrSplit.SuspendLayout();
            this.leftBox.SuspendLayout();
            this.rightBox.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainTab
            // 
            this.mainTab.Controls.Add(this.controlTab);
            this.mainTab.Controls.Add(this.walkTab);
            this.mainTab.Controls.Add(this.tabPage2);
            this.mainTab.Controls.Add(this.tabPage1);
            this.mainTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTab.Location = new System.Drawing.Point(0, 0);
            this.mainTab.Name = "mainTab";
            this.mainTab.SelectedIndex = 0;
            this.mainTab.Size = new System.Drawing.Size(674, 447);
            this.mainTab.TabIndex = 0;
            // 
            // controlTab
            // 
            this.controlTab.Controls.Add(this.enabled);
            this.controlTab.Controls.Add(this.valuePanel);
            this.controlTab.Location = new System.Drawing.Point(4, 22);
            this.controlTab.Name = "controlTab";
            this.controlTab.Padding = new System.Windows.Forms.Padding(3);
            this.controlTab.Size = new System.Drawing.Size(666, 421);
            this.controlTab.TabIndex = 0;
            this.controlTab.Text = "Controls";
            this.controlTab.UseVisualStyleBackColor = true;
            // 
            // enabled
            // 
            this.enabled.AutoSize = true;
            this.enabled.Location = new System.Drawing.Point(6, 104);
            this.enabled.Name = "enabled";
            this.enabled.Size = new System.Drawing.Size(65, 17);
            this.enabled.TabIndex = 9;
            this.enabled.Text = "Active";
            this.enabled.UseVisualStyleBackColor = true;
            this.enabled.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // valuePanel
            // 
            this.valuePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.valuePanel.Location = new System.Drawing.Point(3, 3);
            this.valuePanel.Max = 1024F;
            this.valuePanel.MaxV = ((OpenMetaverse.Vector3)(resources.GetObject("valuePanel.MaxV")));
            this.valuePanel.Min = -1024F;
            this.valuePanel.MinimumSize = new System.Drawing.Size(103, 95);
            this.valuePanel.MinV = ((OpenMetaverse.Vector3)(resources.GetObject("valuePanel.MinV")));
            this.valuePanel.Name = "valuePanel";
            this.valuePanel.Size = new System.Drawing.Size(657, 95);
            this.valuePanel.TabIndex = 0;
            this.valuePanel.Value = ((OpenMetaverse.Vector3)(resources.GetObject("valuePanel.Value")));
            this.valuePanel.X = 0F;
            this.valuePanel.Y = 0F;
            this.valuePanel.Z = 0F;
            // 
            // walkTab
            // 
            this.walkTab.Controls.Add(this.walkEnabled);
            this.walkTab.Controls.Add(this.lrSplit);
            this.walkTab.Controls.Add(this.label19);
            this.walkTab.Controls.Add(this.label6);
            this.walkTab.Controls.Add(this.label5);
            this.walkTab.Controls.Add(this.walkThreshold);
            this.walkTab.Controls.Add(this.walkVal);
            this.walkTab.Controls.Add(this.walkScale);
            this.walkTab.Location = new System.Drawing.Point(4, 22);
            this.walkTab.Name = "walkTab";
            this.walkTab.Padding = new System.Windows.Forms.Padding(3);
            this.walkTab.Size = new System.Drawing.Size(666, 421);
            this.walkTab.TabIndex = 1;
            this.walkTab.Text = "Walk";
            this.walkTab.UseVisualStyleBackColor = true;
            // 
            // walkEnabled
            // 
            this.walkEnabled.AutoSize = true;
            this.walkEnabled.Location = new System.Drawing.Point(8, 160);
            this.walkEnabled.Name = "walkEnabled";
            this.walkEnabled.Size = new System.Drawing.Size(65, 17);
            this.walkEnabled.TabIndex = 8;
            this.walkEnabled.Text = "Active";
            this.walkEnabled.UseVisualStyleBackColor = true;
            this.walkEnabled.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // lrSplit
            // 
            this.lrSplit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lrSplit.IsSplitterFixed = true;
            this.lrSplit.Location = new System.Drawing.Point(3, 6);
            this.lrSplit.Name = "lrSplit";
            // 
            // lrSplit.Panel1
            // 
            this.lrSplit.Panel1.Controls.Add(this.leftBox);
            // 
            // lrSplit.Panel2
            // 
            this.lrSplit.Panel2.Controls.Add(this.rightBox);
            this.lrSplit.Size = new System.Drawing.Size(660, 69);
            this.lrSplit.SplitterDistance = 317;
            this.lrSplit.TabIndex = 7;
            // 
            // leftBox
            // 
            this.leftBox.Controls.Add(this.label2);
            this.leftBox.Controls.Add(this.label1);
            this.leftBox.Controls.Add(this.walkValL);
            this.leftBox.Controls.Add(this.walkDiffL);
            this.leftBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.leftBox.Location = new System.Drawing.Point(0, 0);
            this.leftBox.Name = "leftBox";
            this.leftBox.Size = new System.Drawing.Size(317, 69);
            this.leftBox.TabIndex = 0;
            this.leftBox.TabStop = false;
            this.leftBox.Text = "Left";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Constrained";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Difference";
            // 
            // walkValL
            // 
            this.walkValL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.walkValL.Location = new System.Drawing.Point(69, 45);
            this.walkValL.Max = 1F;
            this.walkValL.Min = -1F;
            this.walkValL.MinimumSize = new System.Drawing.Size(95, 20);
            this.walkValL.Name = "walkValL";
            this.walkValL.Scalar = null;
            this.walkValL.Size = new System.Drawing.Size(242, 20);
            this.walkValL.TabIndex = 1;
            this.walkValL.Value = 0F;
            // 
            // walkDiffL
            // 
            this.walkDiffL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.walkDiffL.Location = new System.Drawing.Point(69, 19);
            this.walkDiffL.Max = 1F;
            this.walkDiffL.Min = -1F;
            this.walkDiffL.MinimumSize = new System.Drawing.Size(95, 20);
            this.walkDiffL.Name = "walkDiffL";
            this.walkDiffL.Scalar = null;
            this.walkDiffL.Size = new System.Drawing.Size(242, 20);
            this.walkDiffL.TabIndex = 0;
            this.walkDiffL.Value = 0F;
            // 
            // rightBox
            // 
            this.rightBox.Controls.Add(this.label3);
            this.rightBox.Controls.Add(this.walkDiffR);
            this.rightBox.Controls.Add(this.label4);
            this.rightBox.Controls.Add(this.walkValR);
            this.rightBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rightBox.Location = new System.Drawing.Point(0, 0);
            this.rightBox.Name = "rightBox";
            this.rightBox.Size = new System.Drawing.Size(339, 69);
            this.rightBox.TabIndex = 1;
            this.rightBox.TabStop = false;
            this.rightBox.Text = "Right";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Constrained";
            // 
            // walkDiffR
            // 
            this.walkDiffR.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.walkDiffR.Location = new System.Drawing.Point(75, 19);
            this.walkDiffR.Max = 1F;
            this.walkDiffR.Min = -1F;
            this.walkDiffR.MinimumSize = new System.Drawing.Size(95, 20);
            this.walkDiffR.Name = "walkDiffR";
            this.walkDiffR.Scalar = null;
            this.walkDiffR.Size = new System.Drawing.Size(258, 20);
            this.walkDiffR.TabIndex = 4;
            this.walkDiffR.Value = 0F;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Difference";
            // 
            // walkValR
            // 
            this.walkValR.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.walkValR.Location = new System.Drawing.Point(75, 45);
            this.walkValR.Max = 1F;
            this.walkValR.Min = -1F;
            this.walkValR.MinimumSize = new System.Drawing.Size(95, 20);
            this.walkValR.Name = "walkValR";
            this.walkValR.Scalar = null;
            this.walkValR.Size = new System.Drawing.Size(258, 20);
            this.walkValR.TabIndex = 5;
            this.walkValR.Value = 0F;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(3, 107);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(54, 13);
            this.label19.TabIndex = 6;
            this.label19.Text = "Threshold";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(5, 81);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Scale";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 133);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(34, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Value";
            // 
            // walkThreshold
            // 
            this.walkThreshold.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.walkThreshold.Location = new System.Drawing.Point(64, 103);
            this.walkThreshold.Max = 1F;
            this.walkThreshold.Min = 0F;
            this.walkThreshold.MinimumSize = new System.Drawing.Size(95, 20);
            this.walkThreshold.Name = "walkThreshold";
            this.walkThreshold.Scalar = null;
            this.walkThreshold.Size = new System.Drawing.Size(596, 20);
            this.walkThreshold.TabIndex = 5;
            this.walkThreshold.Value = 0F;
            // 
            // walkVal
            // 
            this.walkVal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.walkVal.Location = new System.Drawing.Point(64, 129);
            this.walkVal.Max = 1F;
            this.walkVal.Min = -1F;
            this.walkVal.MinimumSize = new System.Drawing.Size(95, 20);
            this.walkVal.Name = "walkVal";
            this.walkVal.Scalar = null;
            this.walkVal.Size = new System.Drawing.Size(596, 20);
            this.walkVal.TabIndex = 2;
            this.walkVal.Value = 0F;
            // 
            // walkScale
            // 
            this.walkScale.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.walkScale.Location = new System.Drawing.Point(64, 78);
            this.walkScale.Max = 1F;
            this.walkScale.Min = 0F;
            this.walkScale.MinimumSize = new System.Drawing.Size(95, 20);
            this.walkScale.Name = "walkScale";
            this.walkScale.Scalar = null;
            this.walkScale.Size = new System.Drawing.Size(596, 20);
            this.walkScale.TabIndex = 1;
            this.walkScale.Value = 0F;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label9);
            this.tabPage2.Controls.Add(this.label11);
            this.tabPage2.Controls.Add(this.flyMin);
            this.tabPage2.Controls.Add(this.flyTimer);
            this.tabPage2.Controls.Add(this.flyEnabled);
            this.tabPage2.Controls.Add(this.label14);
            this.tabPage2.Controls.Add(this.label13);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.splitContainer1);
            this.tabPage2.Controls.Add(this.flyVal);
            this.tabPage2.Controls.Add(this.flyMax);
            this.tabPage2.Controls.Add(this.flyThreshold);
            this.tabPage2.Controls.Add(this.flyScale);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(666, 421);
            this.tabPage2.TabIndex = 3;
            this.tabPage2.Text = "Fly";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(4, 194);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(48, 13);
            this.label9.TabIndex = 14;
            this.label9.Text = "Minimum";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(4, 168);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(33, 13);
            this.label11.TabIndex = 12;
            this.label11.Text = "Timer";
            // 
            // flyMin
            // 
            this.flyMin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.flyMin.Location = new System.Drawing.Point(61, 190);
            this.flyMin.Max = 1F;
            this.flyMin.Min = 0F;
            this.flyMin.MinimumSize = new System.Drawing.Size(95, 20);
            this.flyMin.Name = "flyMin";
            this.flyMin.Scalar = null;
            this.flyMin.Size = new System.Drawing.Size(599, 20);
            this.flyMin.TabIndex = 13;
            this.flyMin.Value = 0F;
            // 
            // flyTimer
            // 
            this.flyTimer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.flyTimer.Location = new System.Drawing.Point(61, 164);
            this.flyTimer.Max = 1000F;
            this.flyTimer.Min = 0F;
            this.flyTimer.MinimumSize = new System.Drawing.Size(95, 20);
            this.flyTimer.Name = "flyTimer";
            this.flyTimer.Scalar = null;
            this.flyTimer.Size = new System.Drawing.Size(599, 20);
            this.flyTimer.TabIndex = 11;
            this.flyTimer.Value = 500F;
            // 
            // flyEnabled
            // 
            this.flyEnabled.AutoSize = true;
            this.flyEnabled.Location = new System.Drawing.Point(6, 242);
            this.flyEnabled.Name = "flyEnabled";
            this.flyEnabled.Size = new System.Drawing.Size(65, 17);
            this.flyEnabled.TabIndex = 10;
            this.flyEnabled.Text = "Active";
            this.flyEnabled.UseVisualStyleBackColor = true;
            this.flyEnabled.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(5, 223);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(34, 13);
            this.label14.TabIndex = 8;
            this.label14.Text = "Value";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(4, 142);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(51, 13);
            this.label13.TabIndex = 6;
            this.label13.Text = "Maximum";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(5, 89);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(34, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Scale";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(4, 116);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(54, 13);
            this.label8.TabIndex = 3;
            this.label8.Text = "Threshold";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer1.Size = new System.Drawing.Size(657, 77);
            this.splitContainer1.SplitterDistance = 315;
            this.splitContainer1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label20);
            this.groupBox1.Controls.Add(this.flyAngleL);
            this.groupBox1.Controls.Add(this.constrainedFlyAngleL);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(315, 77);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Left";
            // 
            // constrainedFlyAngleL
            // 
            this.constrainedFlyAngleL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.constrainedFlyAngleL.Location = new System.Drawing.Point(105, 42);
            this.constrainedFlyAngleL.Max = 90F;
            this.constrainedFlyAngleL.Min = -90F;
            this.constrainedFlyAngleL.MinimumSize = new System.Drawing.Size(95, 20);
            this.constrainedFlyAngleL.Name = "constrainedFlyAngleL";
            this.constrainedFlyAngleL.Scalar = null;
            this.constrainedFlyAngleL.Size = new System.Drawing.Size(204, 20);
            this.constrainedFlyAngleL.TabIndex = 0;
            this.constrainedFlyAngleL.Value = 0F;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.flyAngleR);
            this.groupBox2.Controls.Add(this.label21);
            this.groupBox2.Controls.Add(this.constrainedFlyAngleR);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(338, 77);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Right";
            // 
            // constrainedFlyAngleR
            // 
            this.constrainedFlyAngleR.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.constrainedFlyAngleR.Location = new System.Drawing.Point(108, 42);
            this.constrainedFlyAngleR.Max = 90F;
            this.constrainedFlyAngleR.Min = -90F;
            this.constrainedFlyAngleR.MinimumSize = new System.Drawing.Size(95, 20);
            this.constrainedFlyAngleR.Name = "constrainedFlyAngleR";
            this.constrainedFlyAngleR.Scalar = null;
            this.constrainedFlyAngleR.Size = new System.Drawing.Size(224, 20);
            this.constrainedFlyAngleR.TabIndex = 4;
            this.constrainedFlyAngleR.Value = 0F;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 46);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(93, 13);
            this.label12.TabIndex = 6;
            this.label12.Text = "Constrained Angle";
            // 
            // flyVal
            // 
            this.flyVal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.flyVal.Location = new System.Drawing.Point(61, 216);
            this.flyVal.Max = 1F;
            this.flyVal.Min = -1F;
            this.flyVal.MinimumSize = new System.Drawing.Size(95, 20);
            this.flyVal.Name = "flyVal";
            this.flyVal.Scalar = null;
            this.flyVal.Size = new System.Drawing.Size(599, 20);
            this.flyVal.TabIndex = 7;
            this.flyVal.Value = 0F;
            // 
            // flyMax
            // 
            this.flyMax.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.flyMax.Location = new System.Drawing.Point(61, 138);
            this.flyMax.Max = 1F;
            this.flyMax.Min = 1F;
            this.flyMax.MinimumSize = new System.Drawing.Size(95, 20);
            this.flyMax.Name = "flyMax";
            this.flyMax.Scalar = null;
            this.flyMax.Size = new System.Drawing.Size(599, 20);
            this.flyMax.TabIndex = 5;
            this.flyMax.Value = 1F;
            // 
            // flyThreshold
            // 
            this.flyThreshold.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.flyThreshold.Location = new System.Drawing.Point(61, 112);
            this.flyThreshold.Max = 1F;
            this.flyThreshold.Min = 0F;
            this.flyThreshold.MinimumSize = new System.Drawing.Size(95, 20);
            this.flyThreshold.Name = "flyThreshold";
            this.flyThreshold.Scalar = null;
            this.flyThreshold.Size = new System.Drawing.Size(599, 20);
            this.flyThreshold.TabIndex = 2;
            this.flyThreshold.Value = 0F;
            // 
            // flyScale
            // 
            this.flyScale.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.flyScale.Location = new System.Drawing.Point(61, 86);
            this.flyScale.Max = 5F;
            this.flyScale.Min = 0F;
            this.flyScale.MinimumSize = new System.Drawing.Size(95, 20);
            this.flyScale.Name = "flyScale";
            this.flyScale.Scalar = null;
            this.flyScale.Size = new System.Drawing.Size(599, 20);
            this.flyScale.TabIndex = 1;
            this.flyScale.Value = 0F;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.yawEnabled);
            this.tabPage1.Controls.Add(this.label18);
            this.tabPage1.Controls.Add(this.label17);
            this.tabPage1.Controls.Add(this.label16);
            this.tabPage1.Controls.Add(this.label15);
            this.tabPage1.Controls.Add(this.yawValue);
            this.tabPage1.Controls.Add(this.yawThreshold);
            this.tabPage1.Controls.Add(this.yawScale);
            this.tabPage1.Controls.Add(this.yawLean);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(666, 421);
            this.tabPage1.TabIndex = 4;
            this.tabPage1.Text = "Yaw";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // yawEnabled
            // 
            this.yawEnabled.AutoSize = true;
            this.yawEnabled.Location = new System.Drawing.Point(8, 103);
            this.yawEnabled.Name = "yawEnabled";
            this.yawEnabled.Size = new System.Drawing.Size(65, 17);
            this.yawEnabled.TabIndex = 10;
            this.yawEnabled.Text = "Active";
            this.yawEnabled.UseVisualStyleBackColor = true;
            this.yawEnabled.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(6, 87);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(34, 13);
            this.label18.TabIndex = 7;
            this.label18.Text = "Value";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(6, 62);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(54, 13);
            this.label17.TabIndex = 6;
            this.label17.Text = "Threshold";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(6, 38);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(34, 13);
            this.label16.TabIndex = 5;
            this.label16.Text = "Scale";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(5, 11);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(31, 13);
            this.label15.TabIndex = 4;
            this.label15.Text = "Lean";
            // 
            // yawValue
            // 
            this.yawValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.yawValue.Location = new System.Drawing.Point(66, 84);
            this.yawValue.Max = 1F;
            this.yawValue.Min = -1F;
            this.yawValue.MinimumSize = new System.Drawing.Size(95, 20);
            this.yawValue.Name = "yawValue";
            this.yawValue.Scalar = null;
            this.yawValue.Size = new System.Drawing.Size(594, 20);
            this.yawValue.TabIndex = 3;
            this.yawValue.Value = 0F;
            // 
            // yawThreshold
            // 
            this.yawThreshold.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.yawThreshold.Location = new System.Drawing.Point(66, 58);
            this.yawThreshold.Max = 1F;
            this.yawThreshold.Min = 0F;
            this.yawThreshold.MinimumSize = new System.Drawing.Size(95, 20);
            this.yawThreshold.Name = "yawThreshold";
            this.yawThreshold.Scalar = null;
            this.yawThreshold.Size = new System.Drawing.Size(594, 20);
            this.yawThreshold.TabIndex = 2;
            this.yawThreshold.Value = 0F;
            // 
            // yawScale
            // 
            this.yawScale.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.yawScale.Location = new System.Drawing.Point(66, 32);
            this.yawScale.Max = 1F;
            this.yawScale.Min = 0F;
            this.yawScale.MinimumSize = new System.Drawing.Size(95, 20);
            this.yawScale.Name = "yawScale";
            this.yawScale.Scalar = null;
            this.yawScale.Size = new System.Drawing.Size(594, 20);
            this.yawScale.TabIndex = 1;
            this.yawScale.Value = 0F;
            // 
            // yawLean
            // 
            this.yawLean.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.yawLean.Location = new System.Drawing.Point(66, 6);
            this.yawLean.Max = 90F;
            this.yawLean.Min = -90F;
            this.yawLean.MinimumSize = new System.Drawing.Size(95, 20);
            this.yawLean.Name = "yawLean";
            this.yawLean.Scalar = null;
            this.yawLean.Size = new System.Drawing.Size(594, 20);
            this.yawLean.TabIndex = 0;
            this.yawLean.Value = 0F;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(6, 20);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(34, 13);
            this.label20.TabIndex = 4;
            this.label20.Text = "Angle";
            // 
            // flyAngleL
            // 
            this.flyAngleL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.flyAngleL.Location = new System.Drawing.Point(105, 16);
            this.flyAngleL.Max = 90F;
            this.flyAngleL.Min = -90F;
            this.flyAngleL.MinimumSize = new System.Drawing.Size(95, 20);
            this.flyAngleL.Name = "flyAngleL";
            this.flyAngleL.Scalar = null;
            this.flyAngleL.Size = new System.Drawing.Size(204, 20);
            this.flyAngleL.TabIndex = 3;
            this.flyAngleL.Value = 0F;
            // 
            // flyAngleR
            // 
            this.flyAngleR.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.flyAngleR.Location = new System.Drawing.Point(108, 16);
            this.flyAngleR.Max = 90F;
            this.flyAngleR.Min = -90F;
            this.flyAngleR.MinimumSize = new System.Drawing.Size(95, 20);
            this.flyAngleR.Name = "flyAngleR";
            this.flyAngleR.Scalar = null;
            this.flyAngleR.Size = new System.Drawing.Size(224, 20);
            this.flyAngleR.TabIndex = 7;
            this.flyAngleR.Value = 0F;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(6, 20);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(34, 13);
            this.label21.TabIndex = 8;
            this.label21.Text = "Angle";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 46);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(93, 13);
            this.label10.TabIndex = 7;
            this.label10.Text = "Constrained Angle";
            // 
            // DolphinMovementPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainTab);
            this.Name = "DolphinMovementPanel";
            this.Size = new System.Drawing.Size(674, 447);
            this.mainTab.ResumeLayout(false);
            this.controlTab.ResumeLayout(false);
            this.controlTab.PerformLayout();
            this.walkTab.ResumeLayout(false);
            this.walkTab.PerformLayout();
            this.lrSplit.Panel1.ResumeLayout(false);
            this.lrSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lrSplit)).EndInit();
            this.lrSplit.ResumeLayout(false);
            this.leftBox.ResumeLayout(false);
            this.leftBox.PerformLayout();
            this.rightBox.ResumeLayout(false);
            this.rightBox.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl mainTab;
        private System.Windows.Forms.TabPage controlTab;
        private System.Windows.Forms.TabPage walkTab;
        private Chimera.GUI.UpdatedScalarPanel walkScale;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private Chimera.GUI.UpdatedScalarPanel walkVal;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label14;
        private Chimera.GUI.UpdatedScalarPanel flyVal;
        private System.Windows.Forms.Label label13;
        private Chimera.GUI.UpdatedScalarPanel flyMax;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private Chimera.GUI.UpdatedScalarPanel flyThreshold;
        private Chimera.GUI.UpdatedScalarPanel flyScale;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private Chimera.GUI.UpdatedScalarPanel constrainedFlyAngleL;
        private System.Windows.Forms.GroupBox groupBox2;
        private Chimera.GUI.UpdatedScalarPanel constrainedFlyAngleR;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TabPage tabPage1;
        private Chimera.GUI.UpdatedScalarPanel yawValue;
        private Chimera.GUI.UpdatedScalarPanel yawThreshold;
        private Chimera.GUI.UpdatedScalarPanel yawScale;
        private Chimera.GUI.UpdatedScalarPanel yawLean;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private Chimera.GUI.VectorPanel valuePanel;
        private System.Windows.Forms.Label label19;
        private Chimera.GUI.UpdatedScalarPanel walkThreshold;
        private System.Windows.Forms.SplitContainer lrSplit;
        private System.Windows.Forms.GroupBox leftBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private Chimera.GUI.UpdatedScalarPanel walkValL;
        private Chimera.GUI.UpdatedScalarPanel walkDiffL;
        private System.Windows.Forms.GroupBox rightBox;
        private System.Windows.Forms.Label label3;
        private Chimera.GUI.UpdatedScalarPanel walkDiffR;
        private System.Windows.Forms.Label label4;
        private Chimera.GUI.UpdatedScalarPanel walkValR;
        private System.Windows.Forms.CheckBox walkEnabled;
        private System.Windows.Forms.CheckBox enabled;
        private System.Windows.Forms.CheckBox flyEnabled;
        private System.Windows.Forms.CheckBox yawEnabled;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label11;
        private Chimera.GUI.UpdatedScalarPanel flyMin;
        private Chimera.GUI.UpdatedScalarPanel flyTimer;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label20;
        private Chimera.GUI.UpdatedScalarPanel flyAngleL;
        private Chimera.GUI.UpdatedScalarPanel flyAngleR;
        private System.Windows.Forms.Label label21;
    }
}

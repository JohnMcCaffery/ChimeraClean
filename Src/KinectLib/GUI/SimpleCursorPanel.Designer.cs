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
    partial class SimpleCursorPanel {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimpleCursorPanel));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lab3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.XLable = new System.Windows.Forms.Label();
            this.YLable = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.RawYRight = new Chimera.GUI.UpdatedScalarPanel();
            this.RawXRight = new Chimera.GUI.UpdatedScalarPanel();
            this.ConstrainedYRight = new Chimera.GUI.UpdatedScalarPanel();
            this.ConstrainedXRight = new Chimera.GUI.UpdatedScalarPanel();
            this.Anchor = new Chimera.GUI.UpdatedVectorPanel();
            this.TopLeftY = new Chimera.GUI.UpdatedScalarPanel();
            this.TopLeftX = new Chimera.GUI.UpdatedScalarPanel();
            this.Y = new Chimera.GUI.UpdatedScalarPanel();
            this.X = new Chimera.GUI.UpdatedScalarPanel();
            this.Height = new Chimera.GUI.UpdatedScalarPanel();
            this.Width = new Chimera.GUI.UpdatedScalarPanel();
            this.UpShift = new Chimera.GUI.UpdatedScalarPanel();
            this.LeftShift = new Chimera.GUI.UpdatedScalarPanel();
            this.HandR = new Chimera.GUI.UpdatedVectorPanel();
            this.enabledCheck = new System.Windows.Forms.CheckBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.leftBox = new System.Windows.Forms.GroupBox();
            this.leftHandShiftLabel = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.LeftHandShift = new Chimera.GUI.UpdatedScalarPanel();
            this.HandL = new Chimera.GUI.UpdatedVectorPanel();
            this.ConstrainedXLeft = new Chimera.GUI.UpdatedScalarPanel();
            this.RawXLeft = new Chimera.GUI.UpdatedScalarPanel();
            this.label11 = new System.Windows.Forms.Label();
            this.RawYLeft = new Chimera.GUI.UpdatedScalarPanel();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.ConstrainedYLeft = new Chimera.GUI.UpdatedScalarPanel();
            this.rightBox = new System.Windows.Forms.GroupBox();
            this.SmoothingFactor = new Chimera.GUI.UpdatedScalarPanel();
            this.label14 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.leftBox.SuspendLayout();
            this.rightBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Left Shift";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Up Shift";
            // 
            // lab3
            // 
            this.lab3.AutoSize = true;
            this.lab3.Location = new System.Drawing.Point(8, 10);
            this.lab3.Name = "lab3";
            this.lab3.Size = new System.Drawing.Size(35, 13);
            this.lab3.TabIndex = 9;
            this.lab3.Text = "Width";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 36);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Height";
            // 
            // XLable
            // 
            this.XLable.AutoSize = true;
            this.XLable.Location = new System.Drawing.Point(0, 426);
            this.XLable.Name = "XLable";
            this.XLable.Size = new System.Drawing.Size(14, 13);
            this.XLable.TabIndex = 11;
            this.XLable.Text = "X";
            // 
            // YLable
            // 
            this.YLable.AutoSize = true;
            this.YLable.Location = new System.Drawing.Point(0, 452);
            this.YLable.Name = "YLable";
            this.YLable.Size = new System.Drawing.Size(14, 13);
            this.YLable.TabIndex = 12;
            this.YLable.Text = "Y";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Top Left Y";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 70);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Top Left X";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 201);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(73, 13);
            this.label6.TabIndex = 21;
            this.label6.Text = "Constrained Y";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 175);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(73, 13);
            this.label7.TabIndex = 20;
            this.label7.Text = "Constrained X";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 148);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(39, 13);
            this.label8.TabIndex = 25;
            this.label8.Text = "Raw Y";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 122);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(39, 13);
            this.label9.TabIndex = 24;
            this.label9.Text = "Raw X";
            // 
            // RawYRight
            // 
            this.RawYRight.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RawYRight.Location = new System.Drawing.Point(48, 146);
            this.RawYRight.Max = 1F;
            this.RawYRight.Min = -1F;
            this.RawYRight.MinimumSize = new System.Drawing.Size(95, 20);
            this.RawYRight.Name = "RawYRight";
            this.RawYRight.Scalar = null;
            this.RawYRight.Size = new System.Drawing.Size(593, 20);
            this.RawYRight.TabIndex = 23;
            this.RawYRight.Value = 0F;
            // 
            // RawXRight
            // 
            this.RawXRight.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RawXRight.Location = new System.Drawing.Point(48, 120);
            this.RawXRight.Max = 1F;
            this.RawXRight.Min = -1F;
            this.RawXRight.MinimumSize = new System.Drawing.Size(95, 20);
            this.RawXRight.Name = "RawXRight";
            this.RawXRight.Scalar = null;
            this.RawXRight.Size = new System.Drawing.Size(593, 20);
            this.RawXRight.TabIndex = 22;
            this.RawXRight.Value = 0F;
            // 
            // ConstrainedYRight
            // 
            this.ConstrainedYRight.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ConstrainedYRight.Location = new System.Drawing.Point(82, 198);
            this.ConstrainedYRight.Max = 1F;
            this.ConstrainedYRight.Min = 0F;
            this.ConstrainedYRight.MinimumSize = new System.Drawing.Size(95, 20);
            this.ConstrainedYRight.Name = "ConstrainedYRight";
            this.ConstrainedYRight.Scalar = null;
            this.ConstrainedYRight.Size = new System.Drawing.Size(559, 20);
            this.ConstrainedYRight.TabIndex = 19;
            this.ConstrainedYRight.Value = 0F;
            // 
            // ConstrainedXRight
            // 
            this.ConstrainedXRight.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ConstrainedXRight.Location = new System.Drawing.Point(82, 172);
            this.ConstrainedXRight.Max = 1F;
            this.ConstrainedXRight.Min = 0F;
            this.ConstrainedXRight.MinimumSize = new System.Drawing.Size(95, 20);
            this.ConstrainedXRight.Name = "ConstrainedXRight";
            this.ConstrainedXRight.Scalar = null;
            this.ConstrainedXRight.Size = new System.Drawing.Size(559, 20);
            this.ConstrainedXRight.TabIndex = 18;
            this.ConstrainedXRight.Value = 0F;
            // 
            // Anchor
            // 
            this.Anchor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Anchor.Location = new System.Drawing.Point(9, 55);
            this.Anchor.Max = 10F;
            this.Anchor.MaxV = ((OpenMetaverse.Vector3)(resources.GetObject("Anchor.MaxV")));
            this.Anchor.Min = -10F;
            this.Anchor.MinimumSize = new System.Drawing.Size(103, 95);
            this.Anchor.MinV = ((OpenMetaverse.Vector3)(resources.GetObject("Anchor.MinV")));
            this.Anchor.Name = "Anchor";
            this.Anchor.Size = new System.Drawing.Size(644, 95);
            this.Anchor.TabIndex = 17;
            this.Anchor.Value = ((OpenMetaverse.Vector3)(resources.GetObject("Anchor.Value")));
            this.Anchor.Vector = null;
            this.Anchor.X = 0F;
            this.Anchor.Y = 0F;
            this.Anchor.Z = 0F;
            // 
            // TopLeftY
            // 
            this.TopLeftY.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TopLeftY.Location = new System.Drawing.Point(71, 96);
            this.TopLeftY.Max = 10F;
            this.TopLeftY.Min = -10F;
            this.TopLeftY.MinimumSize = new System.Drawing.Size(95, 20);
            this.TopLeftY.Name = "TopLeftY";
            this.TopLeftY.Scalar = null;
            this.TopLeftY.Size = new System.Drawing.Size(330, 20);
            this.TopLeftY.TabIndex = 14;
            this.TopLeftY.Value = 0F;
            // 
            // TopLeftX
            // 
            this.TopLeftX.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TopLeftX.Location = new System.Drawing.Point(71, 70);
            this.TopLeftX.Max = 10F;
            this.TopLeftX.Min = -10F;
            this.TopLeftX.MinimumSize = new System.Drawing.Size(95, 20);
            this.TopLeftX.Name = "TopLeftX";
            this.TopLeftX.Scalar = null;
            this.TopLeftX.Size = new System.Drawing.Size(330, 20);
            this.TopLeftX.TabIndex = 13;
            this.TopLeftX.Value = 0F;
            // 
            // Y
            // 
            this.Y.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Y.Location = new System.Drawing.Point(21, 452);
            this.Y.Max = 1F;
            this.Y.Min = 0F;
            this.Y.MinimumSize = new System.Drawing.Size(95, 20);
            this.Y.Name = "Y";
            this.Y.Scalar = null;
            this.Y.Size = new System.Drawing.Size(1043, 20);
            this.Y.TabIndex = 6;
            this.Y.Value = 0F;
            // 
            // X
            // 
            this.X.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.X.Location = new System.Drawing.Point(21, 426);
            this.X.Max = 1F;
            this.X.Min = 0F;
            this.X.MinimumSize = new System.Drawing.Size(95, 20);
            this.X.Name = "X";
            this.X.Scalar = null;
            this.X.Size = new System.Drawing.Size(1043, 20);
            this.X.TabIndex = 5;
            this.X.Value = 0F;
            // 
            // Height
            // 
            this.Height.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Height.Location = new System.Drawing.Point(49, 29);
            this.Height.Max = 3F;
            this.Height.Min = 0F;
            this.Height.MinimumSize = new System.Drawing.Size(95, 20);
            this.Height.Name = "Height";
            this.Height.Scalar = null;
            this.Height.Size = new System.Drawing.Size(352, 20);
            this.Height.TabIndex = 4;
            this.Height.Value = 0F;
            // 
            // Width
            // 
            this.Width.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Width.Location = new System.Drawing.Point(49, 3);
            this.Width.Max = 3F;
            this.Width.Min = 0F;
            this.Width.MinimumSize = new System.Drawing.Size(95, 20);
            this.Width.Name = "Width";
            this.Width.Scalar = null;
            this.Width.Size = new System.Drawing.Size(352, 20);
            this.Width.TabIndex = 3;
            this.Width.Value = 0F;
            // 
            // UpShift
            // 
            this.UpShift.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.UpShift.Location = new System.Drawing.Point(61, 29);
            this.UpShift.Max = 2F;
            this.UpShift.Min = -2F;
            this.UpShift.MinimumSize = new System.Drawing.Size(95, 20);
            this.UpShift.Name = "UpShift";
            this.UpShift.Scalar = null;
            this.UpShift.Size = new System.Drawing.Size(592, 20);
            this.UpShift.TabIndex = 2;
            this.UpShift.Value = 0F;
            // 
            // LeftShift
            // 
            this.LeftShift.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LeftShift.Location = new System.Drawing.Point(61, 3);
            this.LeftShift.Max = 2F;
            this.LeftShift.Min = -2F;
            this.LeftShift.MinimumSize = new System.Drawing.Size(95, 20);
            this.LeftShift.Name = "LeftShift";
            this.LeftShift.Scalar = null;
            this.LeftShift.Size = new System.Drawing.Size(589, 20);
            this.LeftShift.TabIndex = 1;
            this.LeftShift.Value = 0F;
            // 
            // HandR
            // 
            this.HandR.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.HandR.Location = new System.Drawing.Point(6, 19);
            this.HandR.Max = 10F;
            this.HandR.MaxV = ((OpenMetaverse.Vector3)(resources.GetObject("HandR.MaxV")));
            this.HandR.Min = -10F;
            this.HandR.MinimumSize = new System.Drawing.Size(103, 95);
            this.HandR.MinV = ((OpenMetaverse.Vector3)(resources.GetObject("HandR.MinV")));
            this.HandR.Name = "HandR";
            this.HandR.Size = new System.Drawing.Size(635, 95);
            this.HandR.TabIndex = 0;
            this.HandR.Value = ((OpenMetaverse.Vector3)(resources.GetObject("HandR.Value")));
            this.HandR.Vector = null;
            this.HandR.X = 0F;
            this.HandR.Y = 0F;
            this.HandR.Z = 0F;
            // 
            // enabledCheck
            // 
            this.enabledCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.enabledCheck.AutoSize = true;
            this.enabledCheck.Location = new System.Drawing.Point(1008, 3);
            this.enabledCheck.Name = "enabledCheck";
            this.enabledCheck.Size = new System.Drawing.Size(56, 17);
            this.enabledCheck.TabIndex = 30;
            this.enabledCheck.Text = "Active";
            this.enabledCheck.UseVisualStyleBackColor = true;
            this.enabledCheck.CheckedChanged += new System.EventHandler(this.enabledCheck_CheckedChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(3, 26);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.leftBox);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.label4);
            this.splitContainer1.Panel1.Controls.Add(this.lab3);
            this.splitContainer1.Panel1.Controls.Add(this.label5);
            this.splitContainer1.Panel1.Controls.Add(this.TopLeftY);
            this.splitContainer1.Panel1.Controls.Add(this.Height);
            this.splitContainer1.Panel1.Controls.Add(this.TopLeftX);
            this.splitContainer1.Panel1.Controls.Add(this.Width);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.rightBox);
            this.splitContainer1.Panel2.Controls.Add(this.Anchor);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Panel2.Controls.Add(this.LeftShift);
            this.splitContainer1.Panel2.Controls.Add(this.UpShift);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Size = new System.Drawing.Size(1064, 388);
            this.splitContainer1.SplitterDistance = 404;
            this.splitContainer1.TabIndex = 31;
            // 
            // leftBox
            // 
            this.leftBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.leftBox.Controls.Add(this.leftHandShiftLabel);
            this.leftBox.Controls.Add(this.label10);
            this.leftBox.Controls.Add(this.LeftHandShift);
            this.leftBox.Controls.Add(this.HandL);
            this.leftBox.Controls.Add(this.ConstrainedXLeft);
            this.leftBox.Controls.Add(this.RawXLeft);
            this.leftBox.Controls.Add(this.label11);
            this.leftBox.Controls.Add(this.RawYLeft);
            this.leftBox.Controls.Add(this.label12);
            this.leftBox.Controls.Add(this.label13);
            this.leftBox.Controls.Add(this.ConstrainedYLeft);
            this.leftBox.Location = new System.Drawing.Point(6, 122);
            this.leftBox.Name = "leftBox";
            this.leftBox.Size = new System.Drawing.Size(395, 260);
            this.leftBox.TabIndex = 26;
            this.leftBox.TabStop = false;
            this.leftBox.Text = "Left";
            // 
            // leftHandShiftLabel
            // 
            this.leftHandShiftLabel.AutoSize = true;
            this.leftHandShiftLabel.Location = new System.Drawing.Point(4, 21);
            this.leftHandShiftLabel.Name = "leftHandShiftLabel";
            this.leftHandShiftLabel.Size = new System.Drawing.Size(78, 13);
            this.leftHandShiftLabel.TabIndex = 28;
            this.leftHandShiftLabel.Text = "Left Hand Shift";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(8, 148);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(39, 13);
            this.label10.TabIndex = 24;
            this.label10.Text = "Raw X";
            // 
            // LeftHandShift
            // 
            this.LeftHandShift.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LeftHandShift.Location = new System.Drawing.Point(87, 19);
            this.LeftHandShift.Max = 1F;
            this.LeftHandShift.Min = 0F;
            this.LeftHandShift.MinimumSize = new System.Drawing.Size(95, 20);
            this.LeftHandShift.Name = "LeftHandShift";
            this.LeftHandShift.Scalar = null;
            this.LeftHandShift.Size = new System.Drawing.Size(308, 20);
            this.LeftHandShift.TabIndex = 27;
            this.LeftHandShift.Value = 0F;
            // 
            // HandL
            // 
            this.HandL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.HandL.Location = new System.Drawing.Point(6, 45);
            this.HandL.Max = 10F;
            this.HandL.MaxV = ((OpenMetaverse.Vector3)(resources.GetObject("HandL.MaxV")));
            this.HandL.Min = -10F;
            this.HandL.MinimumSize = new System.Drawing.Size(103, 95);
            this.HandL.MinV = ((OpenMetaverse.Vector3)(resources.GetObject("HandL.MinV")));
            this.HandL.Name = "HandL";
            this.HandL.Size = new System.Drawing.Size(389, 95);
            this.HandL.TabIndex = 0;
            this.HandL.Value = ((OpenMetaverse.Vector3)(resources.GetObject("HandL.Value")));
            this.HandL.Vector = null;
            this.HandL.X = 0F;
            this.HandL.Y = 0F;
            this.HandL.Z = 0F;
            // 
            // ConstrainedXLeft
            // 
            this.ConstrainedXLeft.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ConstrainedXLeft.Location = new System.Drawing.Point(87, 198);
            this.ConstrainedXLeft.Max = 1F;
            this.ConstrainedXLeft.Min = 0F;
            this.ConstrainedXLeft.MinimumSize = new System.Drawing.Size(95, 20);
            this.ConstrainedXLeft.Name = "ConstrainedXLeft";
            this.ConstrainedXLeft.Scalar = null;
            this.ConstrainedXLeft.Size = new System.Drawing.Size(308, 20);
            this.ConstrainedXLeft.TabIndex = 18;
            this.ConstrainedXLeft.Value = 0F;
            // 
            // RawXLeft
            // 
            this.RawXLeft.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RawXLeft.Location = new System.Drawing.Point(52, 146);
            this.RawXLeft.Max = 1F;
            this.RawXLeft.Min = -1F;
            this.RawXLeft.MinimumSize = new System.Drawing.Size(95, 20);
            this.RawXLeft.Name = "RawXLeft";
            this.RawXLeft.Scalar = null;
            this.RawXLeft.Size = new System.Drawing.Size(343, 20);
            this.RawXLeft.TabIndex = 22;
            this.RawXLeft.Value = 0F;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(8, 227);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(73, 13);
            this.label11.TabIndex = 21;
            this.label11.Text = "Constrained Y";
            // 
            // RawYLeft
            // 
            this.RawYLeft.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RawYLeft.Location = new System.Drawing.Point(52, 172);
            this.RawYLeft.Max = 1F;
            this.RawYLeft.Min = -1F;
            this.RawYLeft.MinimumSize = new System.Drawing.Size(95, 20);
            this.RawYLeft.Name = "RawYLeft";
            this.RawYLeft.Scalar = null;
            this.RawYLeft.Size = new System.Drawing.Size(343, 20);
            this.RawYLeft.TabIndex = 23;
            this.RawYLeft.Value = 0F;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(8, 174);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(39, 13);
            this.label12.TabIndex = 25;
            this.label12.Text = "Raw Y";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(8, 201);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(73, 13);
            this.label13.TabIndex = 20;
            this.label13.Text = "Constrained X";
            // 
            // ConstrainedYLeft
            // 
            this.ConstrainedYLeft.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ConstrainedYLeft.Location = new System.Drawing.Point(87, 224);
            this.ConstrainedYLeft.Max = 1F;
            this.ConstrainedYLeft.Min = 0F;
            this.ConstrainedYLeft.MinimumSize = new System.Drawing.Size(95, 20);
            this.ConstrainedYLeft.Name = "ConstrainedYLeft";
            this.ConstrainedYLeft.Scalar = null;
            this.ConstrainedYLeft.Size = new System.Drawing.Size(308, 20);
            this.ConstrainedYLeft.TabIndex = 19;
            this.ConstrainedYLeft.Value = 0F;
            // 
            // rightBox
            // 
            this.rightBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rightBox.Controls.Add(this.label9);
            this.rightBox.Controls.Add(this.HandR);
            this.rightBox.Controls.Add(this.ConstrainedXRight);
            this.rightBox.Controls.Add(this.RawXRight);
            this.rightBox.Controls.Add(this.label6);
            this.rightBox.Controls.Add(this.RawYRight);
            this.rightBox.Controls.Add(this.label8);
            this.rightBox.Controls.Add(this.label7);
            this.rightBox.Controls.Add(this.ConstrainedYRight);
            this.rightBox.Location = new System.Drawing.Point(3, 156);
            this.rightBox.Name = "rightBox";
            this.rightBox.Size = new System.Drawing.Size(647, 226);
            this.rightBox.TabIndex = 18;
            this.rightBox.TabStop = false;
            this.rightBox.Text = "Right";
            // 
            // SmoothingFactor
            // 
            this.SmoothingFactor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SmoothingFactor.Location = new System.Drawing.Point(96, 3);
            this.SmoothingFactor.Max = 20F;
            this.SmoothingFactor.Min = 0F;
            this.SmoothingFactor.MinimumSize = new System.Drawing.Size(95, 20);
            this.SmoothingFactor.Name = "SmoothingFactor";
            this.SmoothingFactor.Scalar = null;
            this.SmoothingFactor.Size = new System.Drawing.Size(906, 20);
            this.SmoothingFactor.TabIndex = 32;
            this.SmoothingFactor.Value = 0F;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(3, 7);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(90, 13);
            this.label14.TabIndex = 33;
            this.label14.Text = "Smoothing Factor";
            // 
            // SimpleCursorPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label14);
            this.Controls.Add(this.SmoothingFactor);
            this.Controls.Add(this.enabledCheck);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.YLable);
            this.Controls.Add(this.XLable);
            this.Controls.Add(this.Y);
            this.Controls.Add(this.X);
            this.Name = "SimpleCursorPanel";
            this.Size = new System.Drawing.Size(1067, 534);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.leftBox.ResumeLayout(false);
            this.leftBox.PerformLayout();
            this.rightBox.ResumeLayout(false);
            this.rightBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Chimera.GUI.UpdatedVectorPanel HandR;
        private Chimera.GUI.UpdatedScalarPanel LeftShift;
        private Chimera.GUI.UpdatedScalarPanel UpShift;
        private Chimera.GUI.UpdatedScalarPanel Width;
        private Chimera.GUI.UpdatedScalarPanel Height;
        private Chimera.GUI.UpdatedScalarPanel X;
        private Chimera.GUI.UpdatedScalarPanel Y;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lab3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label XLable;
        private System.Windows.Forms.Label YLable;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private Chimera.GUI.UpdatedScalarPanel TopLeftY;
        private Chimera.GUI.UpdatedScalarPanel TopLeftX;
        private Chimera.GUI.UpdatedVectorPanel Anchor;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private Chimera.GUI.UpdatedScalarPanel ConstrainedYRight;
        private Chimera.GUI.UpdatedScalarPanel ConstrainedXRight;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private Chimera.GUI.UpdatedScalarPanel RawYRight;
        private Chimera.GUI.UpdatedScalarPanel RawXRight;
        private System.Windows.Forms.CheckBox enabledCheck;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox leftBox;
        private System.Windows.Forms.Label label10;
        private Chimera.GUI.UpdatedVectorPanel HandL;
        private Chimera.GUI.UpdatedScalarPanel ConstrainedXLeft;
        private Chimera.GUI.UpdatedScalarPanel RawXLeft;
        private System.Windows.Forms.Label label11;
        private Chimera.GUI.UpdatedScalarPanel RawYLeft;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private Chimera.GUI.UpdatedScalarPanel ConstrainedYLeft;
        private System.Windows.Forms.GroupBox rightBox;
        private System.Windows.Forms.Label leftHandShiftLabel;
        private Chimera.GUI.UpdatedScalarPanel LeftHandShift;
        private Chimera.GUI.UpdatedScalarPanel SmoothingFactor;
        private System.Windows.Forms.Label label14;
    }
}

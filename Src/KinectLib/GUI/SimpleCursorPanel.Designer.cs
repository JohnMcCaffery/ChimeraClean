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
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.HeightScale = new KinectLib.GUI.UpdatedScalarPanel();
            this.WidthScale = new KinectLib.GUI.UpdatedScalarPanel();
            this.RawY = new KinectLib.GUI.UpdatedScalarPanel();
            this.RawX = new KinectLib.GUI.UpdatedScalarPanel();
            this.ConstrainedY = new KinectLib.GUI.UpdatedScalarPanel();
            this.ConstrainedX = new KinectLib.GUI.UpdatedScalarPanel();
            this.Anchor = new KinectLib.GUI.UpdatedVectorPanel();
            this.TopLeftY = new KinectLib.GUI.UpdatedScalarPanel();
            this.TopLeftX = new KinectLib.GUI.UpdatedScalarPanel();
            this.Y = new KinectLib.GUI.UpdatedScalarPanel();
            this.X = new KinectLib.GUI.UpdatedScalarPanel();
            this.Height = new KinectLib.GUI.UpdatedScalarPanel();
            this.Width = new KinectLib.GUI.UpdatedScalarPanel();
            this.UpShift = new KinectLib.GUI.UpdatedScalarPanel();
            this.LeftShift = new KinectLib.GUI.UpdatedScalarPanel();
            this.HandR = new KinectLib.GUI.UpdatedVectorPanel();
            this.enabledCheck = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(415, 102);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Left Shift";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(415, 127);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Up Shift";
            // 
            // lab3
            // 
            this.lab3.AutoSize = true;
            this.lab3.Location = new System.Drawing.Point(4, 160);
            this.lab3.Name = "lab3";
            this.lab3.Size = new System.Drawing.Size(35, 13);
            this.lab3.TabIndex = 9;
            this.lab3.Text = "Width";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 186);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Height";
            // 
            // XLable
            // 
            this.XLable.AutoSize = true;
            this.XLable.Location = new System.Drawing.Point(3, 264);
            this.XLable.Name = "XLable";
            this.XLable.Size = new System.Drawing.Size(14, 13);
            this.XLable.TabIndex = 11;
            this.XLable.Text = "X";
            // 
            // YLable
            // 
            this.YLable.AutoSize = true;
            this.YLable.Location = new System.Drawing.Point(3, 290);
            this.YLable.Name = "YLable";
            this.YLable.Size = new System.Drawing.Size(14, 13);
            this.YLable.TabIndex = 12;
            this.YLable.Text = "Y";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 127);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Top Left Y";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 102);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Top Left X";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(415, 238);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(73, 13);
            this.label6.TabIndex = 21;
            this.label6.Text = "Constrained Y";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(415, 212);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(73, 13);
            this.label7.TabIndex = 20;
            this.label7.Text = "Constrained X";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 238);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(39, 13);
            this.label8.TabIndex = 25;
            this.label8.Text = "Raw Y";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 212);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(39, 13);
            this.label9.TabIndex = 24;
            this.label9.Text = "Raw X";
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(415, 182);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(68, 13);
            this.label10.TabIndex = 29;
            this.label10.Text = "Height Scale";
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(415, 156);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(65, 13);
            this.label11.TabIndex = 28;
            this.label11.Text = "Width Scale";
            // 
            // HeightScale
            // 
            this.HeightScale.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.HeightScale.Location = new System.Drawing.Point(494, 179);
            this.HeightScale.Max = 4F;
            this.HeightScale.Min = 0F;
            this.HeightScale.MinimumSize = new System.Drawing.Size(95, 20);
            this.HeightScale.Name = "HeightScale";
            this.HeightScale.Scalar = null;
            this.HeightScale.Size = new System.Drawing.Size(249, 20);
            this.HeightScale.TabIndex = 27;
            this.HeightScale.Value = 0F;
            // 
            // WidthScale
            // 
            this.WidthScale.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.WidthScale.Location = new System.Drawing.Point(494, 153);
            this.WidthScale.Max = 4F;
            this.WidthScale.Min = 0F;
            this.WidthScale.MinimumSize = new System.Drawing.Size(95, 20);
            this.WidthScale.Name = "WidthScale";
            this.WidthScale.Scalar = null;
            this.WidthScale.Size = new System.Drawing.Size(249, 20);
            this.WidthScale.TabIndex = 26;
            this.WidthScale.Value = 0F;
            // 
            // RawY
            // 
            this.RawY.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.RawY.Location = new System.Drawing.Point(48, 231);
            this.RawY.Max = 1F;
            this.RawY.Min = -1F;
            this.RawY.MinimumSize = new System.Drawing.Size(95, 20);
            this.RawY.Name = "RawY";
            this.RawY.Scalar = null;
            this.RawY.Size = new System.Drawing.Size(361, 20);
            this.RawY.TabIndex = 23;
            this.RawY.Value = 0F;
            // 
            // RawX
            // 
            this.RawX.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.RawX.Location = new System.Drawing.Point(48, 205);
            this.RawX.Max = 1F;
            this.RawX.Min = -1F;
            this.RawX.MinimumSize = new System.Drawing.Size(95, 20);
            this.RawX.Name = "RawX";
            this.RawX.Scalar = null;
            this.RawX.Size = new System.Drawing.Size(361, 20);
            this.RawX.TabIndex = 22;
            this.RawX.Value = 0F;
            // 
            // ConstrainedY
            // 
            this.ConstrainedY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ConstrainedY.Location = new System.Drawing.Point(494, 235);
            this.ConstrainedY.Max = 1F;
            this.ConstrainedY.Min = 0F;
            this.ConstrainedY.MinimumSize = new System.Drawing.Size(95, 20);
            this.ConstrainedY.Name = "ConstrainedY";
            this.ConstrainedY.Scalar = null;
            this.ConstrainedY.Size = new System.Drawing.Size(249, 20);
            this.ConstrainedY.TabIndex = 19;
            this.ConstrainedY.Value = 0F;
            // 
            // ConstrainedX
            // 
            this.ConstrainedX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ConstrainedX.Location = new System.Drawing.Point(494, 209);
            this.ConstrainedX.Max = 1F;
            this.ConstrainedX.Min = 0F;
            this.ConstrainedX.MinimumSize = new System.Drawing.Size(95, 20);
            this.ConstrainedX.Name = "ConstrainedX";
            this.ConstrainedX.Scalar = null;
            this.ConstrainedX.Size = new System.Drawing.Size(249, 20);
            this.ConstrainedX.TabIndex = 18;
            this.ConstrainedX.Value = 0F;
            // 
            // Anchor
            // 
            this.Anchor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Anchor.Location = new System.Drawing.Point(331, 0);
            this.Anchor.Max = 10F;
            this.Anchor.MaxV = ((OpenMetaverse.Vector3)(resources.GetObject("Anchor.MaxV")));
            this.Anchor.Min = -10F;
            this.Anchor.MinimumSize = new System.Drawing.Size(103, 95);
            this.Anchor.MinV = ((OpenMetaverse.Vector3)(resources.GetObject("Anchor.MinV")));
            this.Anchor.Name = "Anchor";
            this.Anchor.Size = new System.Drawing.Size(412, 95);
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
            this.TopLeftY.Location = new System.Drawing.Point(67, 127);
            this.TopLeftY.Max = 10F;
            this.TopLeftY.Min = -10F;
            this.TopLeftY.MinimumSize = new System.Drawing.Size(95, 20);
            this.TopLeftY.Name = "TopLeftY";
            this.TopLeftY.Scalar = null;
            this.TopLeftY.Size = new System.Drawing.Size(342, 20);
            this.TopLeftY.TabIndex = 14;
            this.TopLeftY.Value = 0F;
            // 
            // TopLeftX
            // 
            this.TopLeftX.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.TopLeftX.Location = new System.Drawing.Point(67, 101);
            this.TopLeftX.Max = 10F;
            this.TopLeftX.Min = -10F;
            this.TopLeftX.MinimumSize = new System.Drawing.Size(95, 20);
            this.TopLeftX.Name = "TopLeftX";
            this.TopLeftX.Scalar = null;
            this.TopLeftX.Size = new System.Drawing.Size(342, 20);
            this.TopLeftX.TabIndex = 13;
            this.TopLeftX.Value = 0F;
            // 
            // Y
            // 
            this.Y.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Y.Location = new System.Drawing.Point(24, 283);
            this.Y.Max = 1F;
            this.Y.Min = 0F;
            this.Y.MinimumSize = new System.Drawing.Size(95, 20);
            this.Y.Name = "Y";
            this.Y.Scalar = null;
            this.Y.Size = new System.Drawing.Size(719, 20);
            this.Y.TabIndex = 6;
            this.Y.Value = 0F;
            // 
            // X
            // 
            this.X.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.X.Location = new System.Drawing.Point(24, 257);
            this.X.Max = 1F;
            this.X.Min = 0F;
            this.X.MinimumSize = new System.Drawing.Size(95, 20);
            this.X.Name = "X";
            this.X.Scalar = null;
            this.X.Size = new System.Drawing.Size(719, 20);
            this.X.TabIndex = 5;
            this.X.Value = 0F;
            // 
            // Height
            // 
            this.Height.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Height.Location = new System.Drawing.Point(48, 179);
            this.Height.Max = 3F;
            this.Height.Min = 0F;
            this.Height.MinimumSize = new System.Drawing.Size(95, 20);
            this.Height.Name = "Height";
            this.Height.Scalar = null;
            this.Height.Size = new System.Drawing.Size(361, 20);
            this.Height.TabIndex = 4;
            this.Height.Value = 0F;
            // 
            // Width
            // 
            this.Width.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Width.Location = new System.Drawing.Point(48, 153);
            this.Width.Max = 3F;
            this.Width.Min = 0F;
            this.Width.MinimumSize = new System.Drawing.Size(95, 20);
            this.Width.Name = "Width";
            this.Width.Scalar = null;
            this.Width.Size = new System.Drawing.Size(361, 20);
            this.Width.TabIndex = 3;
            this.Width.Value = 0F;
            // 
            // UpShift
            // 
            this.UpShift.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.UpShift.Location = new System.Drawing.Point(470, 127);
            this.UpShift.Max = 2F;
            this.UpShift.Min = -2F;
            this.UpShift.MinimumSize = new System.Drawing.Size(95, 20);
            this.UpShift.Name = "UpShift";
            this.UpShift.Scalar = null;
            this.UpShift.Size = new System.Drawing.Size(273, 20);
            this.UpShift.TabIndex = 2;
            this.UpShift.Value = 0F;
            // 
            // LeftShift
            // 
            this.LeftShift.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LeftShift.Location = new System.Drawing.Point(470, 101);
            this.LeftShift.Max = 2F;
            this.LeftShift.Min = -2F;
            this.LeftShift.MinimumSize = new System.Drawing.Size(95, 20);
            this.LeftShift.Name = "LeftShift";
            this.LeftShift.Scalar = null;
            this.LeftShift.Size = new System.Drawing.Size(273, 20);
            this.LeftShift.TabIndex = 1;
            this.LeftShift.Value = 0F;
            // 
            // HandR
            // 
            this.HandR.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.HandR.Location = new System.Drawing.Point(0, 0);
            this.HandR.Max = 10F;
            this.HandR.MaxV = ((OpenMetaverse.Vector3)(resources.GetObject("HandR.MaxV")));
            this.HandR.Min = -10F;
            this.HandR.MinimumSize = new System.Drawing.Size(103, 95);
            this.HandR.MinV = ((OpenMetaverse.Vector3)(resources.GetObject("HandR.MinV")));
            this.HandR.Name = "HandR";
            this.HandR.Size = new System.Drawing.Size(325, 95);
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
            this.enabledCheck.Location = new System.Drawing.Point(675, 3);
            this.enabledCheck.Name = "enabledCheck";
            this.enabledCheck.Size = new System.Drawing.Size(65, 17);
            this.enabledCheck.TabIndex = 30;
            this.enabledCheck.Text = "Enabled";
            this.enabledCheck.UseVisualStyleBackColor = true;
            this.enabledCheck.CheckedChanged += new System.EventHandler(this.enabledCheck_CheckedChanged);
            // 
            // SimpleCursorPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.enabledCheck);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.HeightScale);
            this.Controls.Add(this.WidthScale);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.RawY);
            this.Controls.Add(this.RawX);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.ConstrainedY);
            this.Controls.Add(this.ConstrainedX);
            this.Controls.Add(this.Anchor);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.TopLeftY);
            this.Controls.Add(this.TopLeftX);
            this.Controls.Add(this.YLable);
            this.Controls.Add(this.XLable);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lab3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Y);
            this.Controls.Add(this.X);
            this.Controls.Add(this.Height);
            this.Controls.Add(this.Width);
            this.Controls.Add(this.UpShift);
            this.Controls.Add(this.LeftShift);
            this.Controls.Add(this.HandR);
            this.Name = "SimpleCursorPanel";
            this.Size = new System.Drawing.Size(743, 534);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private KinectLib.GUI.UpdatedVectorPanel HandR;
        private KinectLib.GUI.UpdatedScalarPanel LeftShift;
        private KinectLib.GUI.UpdatedScalarPanel UpShift;
        private KinectLib.GUI.UpdatedScalarPanel Width;
        private KinectLib.GUI.UpdatedScalarPanel Height;
        private KinectLib.GUI.UpdatedScalarPanel X;
        private KinectLib.GUI.UpdatedScalarPanel Y;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lab3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label XLable;
        private System.Windows.Forms.Label YLable;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private KinectLib.GUI.UpdatedScalarPanel TopLeftY;
        private KinectLib.GUI.UpdatedScalarPanel TopLeftX;
        private KinectLib.GUI.UpdatedVectorPanel Anchor;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private KinectLib.GUI.UpdatedScalarPanel ConstrainedY;
        private KinectLib.GUI.UpdatedScalarPanel ConstrainedX;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private KinectLib.GUI.UpdatedScalarPanel RawY;
        private KinectLib.GUI.UpdatedScalarPanel RawX;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private KinectLib.GUI.UpdatedScalarPanel HeightScale;
        private KinectLib.GUI.UpdatedScalarPanel WidthScale;
        private System.Windows.Forms.CheckBox enabledCheck;
    }
}

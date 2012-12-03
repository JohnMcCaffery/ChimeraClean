using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UtilLib;

namespace ChimeraLib.Controls {
    public partial class WindowPanel : UserControl {
        private static readonly decimal ASPECT_RATIO_TOLERANCE = new decimal(0.0001);
        private static double INCH2MM = 25.4;
        private Window window;

        public WindowPanel() : this(new Window()) {
        }
        public WindowPanel(Window window) {
            InitializeComponent();
            aspectRatioValue.Value = aspectRatioHValue.Value / aspectRatioWValue.Value;
            Window = window;
        }

        public Window Window {
            get { return window;  }
            set {
                if (value == null)
                    return;
                if (window != null)
                    window.OnChange -= WindowChanged;
                window = value;
                window.OnChange += WindowChanged;
                WindowChanged(value, null);
            }
        }
        private bool init;
        private void WindowChanged(object source, EventArgs args) {
            if (window == null)
                return;
            Action a = () => {
                double max = Math.Max(window.Height, window.Width);
                if (max > heightSlider.Maximum) {
                    return;
                    //heightSlider.Maximum = (int) max;
                    //heightValue.Maximum = new decimal(max / 10.0);
                    //widthSlider.Maximum = (int) max;
                    //widthValue.Maximum = new decimal(max / 10.0);
                }

                double diagonalInch = Window.Diagonal / INCH2MM;
                if (diagonalInch * 10.0 > diagonalSlider.Maximum) {
                    return;
                    //diagonalSlider.Maximum = (int) (diagonalInch * 10.0);
                    //diagonalValue.Maximum = new decimal(diagonalInch);
                }

                init = true;
                decimal aspectRatio = aspectRatioValue.Value;
                lockScreenCheck.Checked = window.LockScreenPosition;
                widthSlider.Value = (int) (window.Width);
                widthValue.Value = new decimal(window.Width / 10.0);
                heightSlider.Value = (int) (window.Height);
                heightValue.Value = new decimal(window.Height / 10.0);
                diagonalSlider.Value = (int) (diagonalInch * 10);
                diagonalValue.Value = new decimal(diagonalInch);
                fovSlider.Value = (int)(window.FieldOfView * Rotation.RAD2DEG * 100);
                fovValue.Value = new decimal(window.FieldOfView * Rotation.RAD2DEG);
                aspectRatioValue.Value = new decimal(window.AspectRatio);
                if (Math.Abs(aspectRatio - aspectRatioValue.Value) > ASPECT_RATIO_TOLERANCE) {
                    aspectRatioWValue.Value = new decimal(window.Width);
                    aspectRatioHValue.Value = new decimal(window.Height);
                }
                screenPositionPanel.Value = window.ScreenPosition / 100f;
                eyeOffsetPanel.Value = window.EyeOffset / 10f;
                rotationOffsetPanel.Rotation = window.RotationOffset.Quaternion;
                fovLabel.Text = "Field of View (radians): " + Math.Round(window.FieldOfView, 5);
                widthLabel.Text = "Normalized Width (x offset / width): " + Math.Round(window.ScreenPosition.X / window.Width, 5);
                heightLabel.Text = "Normalized Height (y offset / height): " + Math.Round(window.ScreenPosition.Y / window.Height, 5);
                init = false;
            };

            if (InvokeRequired)
                Invoke(a);
            else
                a();

        }
        private void widthSlider_Scroll(object sender, EventArgs e) {
            if (window != null && !init)
                window.Width = widthSlider.Value;
        }

        private void widthValue_ValueChanged(object sender, EventArgs e) {
            if (window != null && !init)
                window.Width = decimal.ToDouble(widthValue.Value) * 10.0;
        }

        private void heightSlider_Scroll(object sender, EventArgs e) {
            if (window != null && !init)
                window.Height = heightSlider.Value;
        }

        private void heightValue_ValueChanged(object sender, EventArgs e) {
            if (window != null && !init)
                window.Height = decimal.ToDouble(heightValue.Value) * 10.0;
        }

        private void diagonalSlider_Scroll(object sender, EventArgs e) {
            if (window != null && !init)
                window.Diagonal = (diagonalSlider.Value / 10) * INCH2MM;
        }

        private void diagonalValue_ValueChanged(object sender, EventArgs e) {
            if (window != null && !init)
                window.Diagonal = decimal.ToDouble(diagonalValue.Value) * INCH2MM;
        }

        private void fovSlider_Scroll(object sender, EventArgs e) {
            if (window != null && !init)
                window.FieldOfView = (fovSlider.Value / 100) * Rotation.DEG2RAD;
        }

        private void fovValue_ValueChanged(object sender, EventArgs e) {
            if (window != null && !init)
                window.FieldOfView = decimal.ToDouble(fovValue.Value) * Rotation.DEG2RAD;
        }

        private void aspectComponent_ValueChanged(object sender, EventArgs e) {
            if (window != null && aspectRatioHValue != null && aspectRatioWValue != null && !init)
                aspectRatioValue.Value = aspectRatioHValue.Value / aspectRatioWValue.Value;
        }

        private void aspectRatioValue_ValueChanged(object sender, EventArgs e) {
            if (window != null && aspectRatioHValue != null && aspectRatioWValue != null && !init)
                window.AspectRatio = decimal.ToDouble(aspectRatioValue.Value);
        }

        private void screenPositionPanel_OnChange(object sender, EventArgs e) {
            if (window != null && !init)
                window.ScreenPosition = screenPositionPanel.Value * 100f;
        }

        private void eyeOffsetPanel_OnChange(object sender, EventArgs e) {
            if (window != null && !init)
                window.EyeOffset = eyeOffsetPanel.Value * 10f;
        }

        private void rotationOffsetPanel_OnChange(object sender, EventArgs e) {
            if (window != null && !init) {
                window.RotationOffset.Yaw = rotationOffsetPanel.Yaw;
                window.RotationOffset.Pitch = rotationOffsetPanel.Pitch;
            }
        }

        private void lockScreenCheck_CheckedChanged(object sender, EventArgs e) {
            if (window != null && !init) 
                window.LockScreenPosition = lockScreenCheck.Checked;
        }

        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WindowPanel));
            this.mainTab = new System.Windows.Forms.TabControl();
            this.screenTab = new System.Windows.Forms.TabPage();
            this.heightLabel = new System.Windows.Forms.Label();
            this.widthLabel = new System.Windows.Forms.Label();
            this.fovLabel = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.aspectRatioValue = new System.Windows.Forms.NumericUpDown();
            this.aspectRatioHValue = new System.Windows.Forms.NumericUpDown();
            this.diagonalSlider = new System.Windows.Forms.TrackBar();
            this.heightSlider = new System.Windows.Forms.TrackBar();
            this.widthSlider = new System.Windows.Forms.TrackBar();
            this.fovSlider = new System.Windows.Forms.TrackBar();
            this.label5 = new System.Windows.Forms.Label();
            this.aspectRatioWValue = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.diagonalValue = new System.Windows.Forms.NumericUpDown();
            this.heightValue = new System.Windows.Forms.NumericUpDown();
            this.widthValue = new System.Windows.Forms.NumericUpDown();
            this.fovValue = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.screenPositionPanel = new ProxyTestGUI.VectorPanel();
            this.offsetTab = new System.Windows.Forms.TabPage();
            this.lockScreenCheck = new System.Windows.Forms.CheckBox();
            this.eyeOffsetPanel = new ProxyTestGUI.VectorPanel();
            this.rotationOffsetPanel = new ProxyTestGUI.RotationPanel();
            this.mainTab.SuspendLayout();
            this.screenTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.aspectRatioValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.aspectRatioHValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.diagonalSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.heightSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.widthSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fovSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.aspectRatioWValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.diagonalValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.heightValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.widthValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fovValue)).BeginInit();
            this.offsetTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainTab
            // 
            this.mainTab.Controls.Add(this.screenTab);
            this.mainTab.Controls.Add(this.offsetTab);
            this.mainTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTab.Location = new System.Drawing.Point(0, 0);
            this.mainTab.Name = "mainTab";
            this.mainTab.SelectedIndex = 0;
            this.mainTab.Size = new System.Drawing.Size(819, 280);
            this.mainTab.TabIndex = 0;
            // 
            // screenTab
            // 
            this.screenTab.AutoScroll = true;
            this.screenTab.Controls.Add(this.heightLabel);
            this.screenTab.Controls.Add(this.widthLabel);
            this.screenTab.Controls.Add(this.fovLabel);
            this.screenTab.Controls.Add(this.label7);
            this.screenTab.Controls.Add(this.label6);
            this.screenTab.Controls.Add(this.aspectRatioValue);
            this.screenTab.Controls.Add(this.aspectRatioHValue);
            this.screenTab.Controls.Add(this.diagonalSlider);
            this.screenTab.Controls.Add(this.heightSlider);
            this.screenTab.Controls.Add(this.widthSlider);
            this.screenTab.Controls.Add(this.fovSlider);
            this.screenTab.Controls.Add(this.label5);
            this.screenTab.Controls.Add(this.aspectRatioWValue);
            this.screenTab.Controls.Add(this.label4);
            this.screenTab.Controls.Add(this.diagonalValue);
            this.screenTab.Controls.Add(this.heightValue);
            this.screenTab.Controls.Add(this.widthValue);
            this.screenTab.Controls.Add(this.fovValue);
            this.screenTab.Controls.Add(this.label3);
            this.screenTab.Controls.Add(this.label2);
            this.screenTab.Controls.Add(this.label1);
            this.screenTab.Controls.Add(this.screenPositionPanel);
            this.screenTab.Location = new System.Drawing.Point(4, 22);
            this.screenTab.Name = "screenTab";
            this.screenTab.Padding = new System.Windows.Forms.Padding(3);
            this.screenTab.Size = new System.Drawing.Size(811, 254);
            this.screenTab.TabIndex = 0;
            this.screenTab.Text = "Screen";
            this.screenTab.UseVisualStyleBackColor = true;
            // 
            // heightLabel
            // 
            this.heightLabel.AutoSize = true;
            this.heightLabel.Location = new System.Drawing.Point(432, 232);
            this.heightLabel.Name = "heightLabel";
            this.heightLabel.Size = new System.Drawing.Size(184, 13);
            this.heightLabel.TabIndex = 21;
            this.heightLabel.Text = "Normalized Width (x offset / width):  0";
            // 
            // widthLabel
            // 
            this.widthLabel.AutoSize = true;
            this.widthLabel.Location = new System.Drawing.Point(176, 232);
            this.widthLabel.Name = "widthLabel";
            this.widthLabel.Size = new System.Drawing.Size(191, 13);
            this.widthLabel.TabIndex = 20;
            this.widthLabel.Text = "Normalized Height (y offset / height):  0";
            // 
            // fovLabel
            // 
            this.fovLabel.AutoSize = true;
            this.fovLabel.Location = new System.Drawing.Point(8, 232);
            this.fovLabel.Name = "fovLabel";
            this.fovLabel.Size = new System.Drawing.Size(145, 13);
            this.fovLabel.TabIndex = 19;
            this.fovLabel.Text = "Field Of View (radians): 1.571";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(407, 209);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(22, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = " = ";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(256, 209);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(19, 13);
            this.label6.TabIndex = 17;
            this.label6.Text = " : ";
            // 
            // aspectRatioValue
            // 
            this.aspectRatioValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.aspectRatioValue.DecimalPlaces = 4;
            this.aspectRatioValue.Location = new System.Drawing.Point(435, 207);
            this.aspectRatioValue.Maximum = new decimal(new int[] {
            1569325056,
            23283064,
            0,
            0});
            this.aspectRatioValue.Name = "aspectRatioValue";
            this.aspectRatioValue.Size = new System.Drawing.Size(370, 20);
            this.aspectRatioValue.TabIndex = 16;
            this.aspectRatioValue.ValueChanged += new System.EventHandler(this.aspectRatioValue_ValueChanged);
            // 
            // aspectRatioHValue
            // 
            this.aspectRatioHValue.Location = new System.Drawing.Point(281, 207);
            this.aspectRatioHValue.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.aspectRatioHValue.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.aspectRatioHValue.Name = "aspectRatioHValue";
            this.aspectRatioHValue.Size = new System.Drawing.Size(120, 20);
            this.aspectRatioHValue.TabIndex = 15;
            this.aspectRatioHValue.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.aspectRatioHValue.ValueChanged += new System.EventHandler(this.aspectComponent_ValueChanged);
            // 
            // diagonalSlider
            // 
            this.diagonalSlider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.diagonalSlider.Location = new System.Drawing.Point(256, 180);
            this.diagonalSlider.Maximum = 1810;
            this.diagonalSlider.Minimum = 1;
            this.diagonalSlider.Name = "diagonalSlider";
            this.diagonalSlider.Size = new System.Drawing.Size(555, 42);
            this.diagonalSlider.TabIndex = 14;
            this.diagonalSlider.TickStyle = System.Windows.Forms.TickStyle.None;
            this.diagonalSlider.Value = 190;
            this.diagonalSlider.ValueChanged += new System.EventHandler(this.diagonalSlider_Scroll);
            // 
            // heightSlider
            // 
            this.heightSlider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.heightSlider.Location = new System.Drawing.Point(256, 155);
            this.heightSlider.Maximum = 4000;
            this.heightSlider.Minimum = 1;
            this.heightSlider.Name = "heightSlider";
            this.heightSlider.Size = new System.Drawing.Size(555, 42);
            this.heightSlider.TabIndex = 13;
            this.heightSlider.TickStyle = System.Windows.Forms.TickStyle.None;
            this.heightSlider.Value = 260;
            this.heightSlider.ValueChanged += new System.EventHandler(this.heightSlider_Scroll);
            // 
            // widthSlider
            // 
            this.widthSlider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.widthSlider.Location = new System.Drawing.Point(256, 126);
            this.widthSlider.Maximum = 4000;
            this.widthSlider.Minimum = 1;
            this.widthSlider.Name = "widthSlider";
            this.widthSlider.Size = new System.Drawing.Size(555, 42);
            this.widthSlider.TabIndex = 12;
            this.widthSlider.TickStyle = System.Windows.Forms.TickStyle.None;
            this.widthSlider.Value = 410;
            this.widthSlider.ValueChanged += new System.EventHandler(this.widthSlider_Scroll);
            // 
            // fovSlider
            // 
            this.fovSlider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fovSlider.Location = new System.Drawing.Point(256, 99);
            this.fovSlider.Maximum = 17900;
            this.fovSlider.Minimum = 1;
            this.fovSlider.Name = "fovSlider";
            this.fovSlider.Size = new System.Drawing.Size(555, 42);
            this.fovSlider.TabIndex = 11;
            this.fovSlider.TickStyle = System.Windows.Forms.TickStyle.None;
            this.fovSlider.Value = 9000;
            this.fovSlider.ValueChanged += new System.EventHandler(this.fovSlider_Scroll);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 209);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(99, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Aspect Ratio (W:H)";
            // 
            // aspectRatioWValue
            // 
            this.aspectRatioWValue.Location = new System.Drawing.Point(130, 207);
            this.aspectRatioWValue.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.aspectRatioWValue.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.aspectRatioWValue.Name = "aspectRatioWValue";
            this.aspectRatioWValue.Size = new System.Drawing.Size(120, 20);
            this.aspectRatioWValue.TabIndex = 9;
            this.aspectRatioWValue.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.aspectRatioWValue.ValueChanged += new System.EventHandler(this.aspectComponent_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 182);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Diagonal (inches)";
            // 
            // diagonalValue
            // 
            this.diagonalValue.DecimalPlaces = 1;
            this.diagonalValue.Location = new System.Drawing.Point(130, 180);
            this.diagonalValue.Maximum = new decimal(new int[] {
            181,
            0,
            0,
            0});
            this.diagonalValue.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.diagonalValue.Name = "diagonalValue";
            this.diagonalValue.Size = new System.Drawing.Size(120, 20);
            this.diagonalValue.TabIndex = 7;
            this.diagonalValue.Value = new decimal(new int[] {
            19,
            0,
            0,
            0});
            this.diagonalValue.ValueChanged += new System.EventHandler(this.diagonalValue_ValueChanged);
            // 
            // heightValue
            // 
            this.heightValue.DecimalPlaces = 1;
            this.heightValue.Location = new System.Drawing.Point(130, 153);
            this.heightValue.Maximum = new decimal(new int[] {
            400,
            0,
            0,
            0});
            this.heightValue.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.heightValue.Name = "heightValue";
            this.heightValue.Size = new System.Drawing.Size(120, 20);
            this.heightValue.TabIndex = 6;
            this.heightValue.Value = new decimal(new int[] {
            26,
            0,
            0,
            0});
            this.heightValue.ValueChanged += new System.EventHandler(this.heightValue_ValueChanged);
            // 
            // widthValue
            // 
            this.widthValue.DecimalPlaces = 1;
            this.widthValue.Location = new System.Drawing.Point(130, 126);
            this.widthValue.Maximum = new decimal(new int[] {
            400,
            0,
            0,
            0});
            this.widthValue.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.widthValue.Name = "widthValue";
            this.widthValue.Size = new System.Drawing.Size(120, 20);
            this.widthValue.TabIndex = 5;
            this.widthValue.Value = new decimal(new int[] {
            41,
            0,
            0,
            0});
            this.widthValue.ValueChanged += new System.EventHandler(this.widthValue_ValueChanged);
            // 
            // fovValue
            // 
            this.fovValue.DecimalPlaces = 2;
            this.fovValue.Location = new System.Drawing.Point(130, 99);
            this.fovValue.Maximum = new decimal(new int[] {
            179,
            0,
            0,
            0});
            this.fovValue.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.fovValue.Name = "fovValue";
            this.fovValue.Size = new System.Drawing.Size(120, 20);
            this.fovValue.TabIndex = 4;
            this.fovValue.Value = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.fovValue.ValueChanged += new System.EventHandler(this.fovValue_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 155);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Height (cm)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 128);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Width (cm)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 101);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Field Of View (degrees)";
            // 
            // screenPositionPanel
            // 
            this.screenPositionPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.screenPositionPanel.DisplayName = "Screen Position (m)";
            this.screenPositionPanel.Location = new System.Drawing.Point(3, 6);
            this.screenPositionPanel.Max = 10D;
            this.screenPositionPanel.Min = -10D;
            this.screenPositionPanel.Name = "screenPositionPanel";
            this.screenPositionPanel.Size = new System.Drawing.Size(808, 98);
            this.screenPositionPanel.TabIndex = 0;
            this.screenPositionPanel.Value = ((OpenMetaverse.Vector3)(resources.GetObject("screenPositionPanel.Value")));
            this.screenPositionPanel.X = 0F;
            this.screenPositionPanel.Y = 0F;
            this.screenPositionPanel.Z = 4F;
            this.screenPositionPanel.OnChange += new System.EventHandler(this.screenPositionPanel_OnChange);
            // 
            // offsetTab
            // 
            this.offsetTab.AutoScroll = true;
            this.offsetTab.Controls.Add(this.lockScreenCheck);
            this.offsetTab.Controls.Add(this.eyeOffsetPanel);
            this.offsetTab.Controls.Add(this.rotationOffsetPanel);
            this.offsetTab.Location = new System.Drawing.Point(4, 22);
            this.offsetTab.Name = "offsetTab";
            this.offsetTab.Padding = new System.Windows.Forms.Padding(3);
            this.offsetTab.Size = new System.Drawing.Size(811, 254);
            this.offsetTab.TabIndex = 1;
            this.offsetTab.Text = "Offsets";
            this.offsetTab.UseVisualStyleBackColor = true;
            // 
            // lockScreenCheck
            // 
            this.lockScreenCheck.AutoSize = true;
            this.lockScreenCheck.Location = new System.Drawing.Point(100, 6);
            this.lockScreenCheck.Name = "lockScreenCheck";
            this.lockScreenCheck.Size = new System.Drawing.Size(132, 17);
            this.lockScreenCheck.TabIndex = 2;
            this.lockScreenCheck.Text = "Lock Screen Positions";
            this.lockScreenCheck.UseVisualStyleBackColor = true;
            this.lockScreenCheck.CheckedChanged += new System.EventHandler(this.lockScreenCheck_CheckedChanged);
            // 
            // eyeOffsetPanel
            // 
            this.eyeOffsetPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.eyeOffsetPanel.DisplayName = "Eye Offset (cm)";
            this.eyeOffsetPanel.Location = new System.Drawing.Point(3, 156);
            this.eyeOffsetPanel.Max = 1000D;
            this.eyeOffsetPanel.Min = -1000D;
            this.eyeOffsetPanel.Name = "eyeOffsetPanel";
            this.eyeOffsetPanel.Size = new System.Drawing.Size(789, 98);
            this.eyeOffsetPanel.TabIndex = 1;
            this.eyeOffsetPanel.Value = ((OpenMetaverse.Vector3)(resources.GetObject("eyeOffsetPanel.Value")));
            this.eyeOffsetPanel.X = 0F;
            this.eyeOffsetPanel.Y = 0F;
            this.eyeOffsetPanel.Z = 0F;
            this.eyeOffsetPanel.OnChange += new System.EventHandler(this.eyeOffsetPanel_OnChange);
            // 
            // rotationOffsetPanel
            // 
            this.rotationOffsetPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rotationOffsetPanel.DisplayName = "Rotation Offset";
            this.rotationOffsetPanel.Location = new System.Drawing.Point(3, 3);
            this.rotationOffsetPanel.LookAtVector = ((OpenMetaverse.Vector3)(resources.GetObject("rotationOffsetPanel.LookAtVector")));
            this.rotationOffsetPanel.Name = "rotationOffsetPanel";
            this.rotationOffsetPanel.Pitch = 0F;
            this.rotationOffsetPanel.Rotation = ((OpenMetaverse.Quaternion)(resources.GetObject("rotationOffsetPanel.Rotation")));
            this.rotationOffsetPanel.Size = new System.Drawing.Size(789, 147);
            this.rotationOffsetPanel.TabIndex = 0;
            this.rotationOffsetPanel.Yaw = 0F;
            this.rotationOffsetPanel.OnChange += new System.EventHandler(this.rotationOffsetPanel_OnChange);
            // 
            // WindowPanel
            // 
            this.Controls.Add(this.mainTab);
            this.Name = "WindowPanel";
            this.Size = new System.Drawing.Size(819, 280);
            this.mainTab.ResumeLayout(false);
            this.screenTab.ResumeLayout(false);
            this.screenTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.aspectRatioValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.aspectRatioHValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.diagonalSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.heightSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.widthSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fovSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.aspectRatioWValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.diagonalValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.heightValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.widthValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fovValue)).EndInit();
            this.offsetTab.ResumeLayout(false);
            this.offsetTab.PerformLayout();
            this.ResumeLayout(false);

        }
    }
}

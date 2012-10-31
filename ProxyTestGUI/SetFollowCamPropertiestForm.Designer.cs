/*************************************************************************
Copyright (c) 2012 John McCaffery 

This file is part of Armadillo ClientProxy.

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
    partial class SetFollowCamPropertiestForm {
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetFollowCamPropertiestForm));
            this.type1Value = new System.Windows.Forms.NumericUpDown();
            this.type1Label = new System.Windows.Forms.Label();
            this.type2Value = new System.Windows.Forms.NumericUpDown();
            this.type2Label = new System.Windows.Forms.Label();
            this.type6Value = new System.Windows.Forms.NumericUpDown();
            this.type6Label = new System.Windows.Forms.Label();
            this.type7Value = new System.Windows.Forms.NumericUpDown();
            this.type7Label = new System.Windows.Forms.Label();
            this.type8Value = new System.Windows.Forms.NumericUpDown();
            this.type8Label = new System.Windows.Forms.Label();
            this.type9Value = new System.Windows.Forms.NumericUpDown();
            this.type9Label = new System.Windows.Forms.Label();
            this.type10Value = new System.Windows.Forms.NumericUpDown();
            this.type10Label = new System.Windows.Forms.Label();
            this.type11Value = new System.Windows.Forms.NumericUpDown();
            this.type11Label = new System.Windows.Forms.Label();
            this.type13Value = new System.Windows.Forms.NumericUpDown();
            this.type13Label = new System.Windows.Forms.Label();
            this.type17Value = new System.Windows.Forms.NumericUpDown();
            this.type17Label = new System.Windows.Forms.Label();
            this.updateTimer = new System.Windows.Forms.Timer(this.components);
            this.startButton = new System.Windows.Forms.Button();
            this.avatarsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.avatarsListBox = new System.Windows.Forms.ListBox();
            this.focusYPanel = new ProxyTestGUI.ValuePanel();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.thresholdZPanel = new ProxyTestGUI.ValuePanel();
            this.focusOffsetVectorPanel = new ProxyTestGUI.VectorPanel();
            this.positionVectorPanel = new ProxyTestGUI.VectorPanel();
            this.activeCheckbox = new System.Windows.Forms.CheckBox();
            this.focusLockedCheckbox = new System.Windows.Forms.CheckBox();
            this.positionLockedCheckbox = new System.Windows.Forms.CheckBox();
            this.timeValue = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.sendPacketCheckbox = new System.Windows.Forms.CheckBox();
            this.focusRotationPanel = new ProxyTestGUI.RotationPanel();
            ((System.ComponentModel.ISupportInitialize)(this.type1Value)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.type2Value)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.type6Value)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.type7Value)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.type8Value)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.type9Value)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.type10Value)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.type11Value)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.type13Value)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.type17Value)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.avatarsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeValue)).BeginInit();
            this.SuspendLayout();
            // 
            // type1Value
            // 
            this.type1Value.Location = new System.Drawing.Point(100, 7);
            this.type1Value.Name = "type1Value";
            this.type1Value.Size = new System.Drawing.Size(50, 20);
            this.type1Value.TabIndex = 1;
            // 
            // type1Label
            // 
            this.type1Label.AutoSize = true;
            this.type1Label.Location = new System.Drawing.Point(63, 9);
            this.type1Label.Name = "type1Label";
            this.type1Label.Size = new System.Drawing.Size(31, 13);
            this.type1Label.TabIndex = 0;
            this.type1Label.Text = "Pitch";
            // 
            // type2Value
            // 
            this.type2Value.DecimalPlaces = 2;
            this.type2Value.Location = new System.Drawing.Point(100, 240);
            this.type2Value.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.type2Value.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            this.type2Value.Name = "type2Value";
            this.type2Value.Size = new System.Drawing.Size(50, 20);
            this.type2Value.TabIndex = 3;
            this.type2Value.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // type2Label
            // 
            this.type2Label.AutoSize = true;
            this.type2Label.Location = new System.Drawing.Point(27, 242);
            this.type2Label.Name = "type2Label";
            this.type2Label.Size = new System.Drawing.Size(67, 13);
            this.type2Label.TabIndex = 2;
            this.type2Label.Text = "Focus Offset";
            // 
            // type6Value
            // 
            this.type6Value.DecimalPlaces = 2;
            this.type6Value.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.type6Value.Location = new System.Drawing.Point(100, 35);
            this.type6Value.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.type6Value.Name = "type6Value";
            this.type6Value.Size = new System.Drawing.Size(50, 20);
            this.type6Value.TabIndex = 13;
            // 
            // type6Label
            // 
            this.type6Label.AutoSize = true;
            this.type6Label.Location = new System.Drawing.Point(29, 37);
            this.type6Label.Name = "type6Label";
            this.type6Label.Size = new System.Drawing.Size(65, 13);
            this.type6Label.TabIndex = 12;
            this.type6Label.Text = "LookAt Lag";
            // 
            // type7Value
            // 
            this.type7Value.DecimalPlaces = 2;
            this.type7Value.Location = new System.Drawing.Point(100, 61);
            this.type7Value.Name = "type7Value";
            this.type7Value.Size = new System.Drawing.Size(50, 20);
            this.type7Value.TabIndex = 11;
            // 
            // type7Label
            // 
            this.type7Label.AutoSize = true;
            this.type7Label.Location = new System.Drawing.Point(37, 63);
            this.type7Label.Name = "type7Label";
            this.type7Label.Size = new System.Drawing.Size(57, 13);
            this.type7Label.TabIndex = 10;
            this.type7Label.Text = "Focus Lag";
            // 
            // type8Value
            // 
            this.type8Value.DecimalPlaces = 2;
            this.type8Value.Location = new System.Drawing.Point(100, 87);
            this.type8Value.Name = "type8Value";
            this.type8Value.Size = new System.Drawing.Size(50, 20);
            this.type8Value.TabIndex = 9;
            // 
            // type8Label
            // 
            this.type8Label.AutoSize = true;
            this.type8Label.Location = new System.Drawing.Point(45, 89);
            this.type8Label.Name = "type8Label";
            this.type8Label.Size = new System.Drawing.Size(49, 13);
            this.type8Label.TabIndex = 8;
            this.type8Label.Text = "Distance";
            // 
            // type9Value
            // 
            this.type9Value.DecimalPlaces = 2;
            this.type9Value.Location = new System.Drawing.Point(100, 113);
            this.type9Value.Name = "type9Value";
            this.type9Value.Size = new System.Drawing.Size(50, 20);
            this.type9Value.TabIndex = 21;
            // 
            // type9Label
            // 
            this.type9Label.AutoSize = true;
            this.type9Label.Location = new System.Drawing.Point(2, 115);
            this.type9Label.Name = "type9Label";
            this.type9Label.Size = new System.Drawing.Size(92, 13);
            this.type9Label.TabIndex = 20;
            this.type9Label.Text = "Behindness Angle";
            // 
            // type10Value
            // 
            this.type10Value.DecimalPlaces = 2;
            this.type10Value.Location = new System.Drawing.Point(100, 139);
            this.type10Value.Name = "type10Value";
            this.type10Value.Size = new System.Drawing.Size(50, 20);
            this.type10Value.TabIndex = 19;
            // 
            // type10Label
            // 
            this.type10Label.AutoSize = true;
            this.type10Label.Location = new System.Drawing.Point(11, 141);
            this.type10Label.Name = "type10Label";
            this.type10Label.Size = new System.Drawing.Size(83, 13);
            this.type10Label.TabIndex = 18;
            this.type10Label.Text = "Behindness Lag";
            // 
            // type11Value
            // 
            this.type11Value.DecimalPlaces = 2;
            this.type11Value.Location = new System.Drawing.Point(100, 165);
            this.type11Value.Name = "type11Value";
            this.type11Value.Size = new System.Drawing.Size(50, 20);
            this.type11Value.TabIndex = 17;
            // 
            // type11Label
            // 
            this.type11Label.AutoSize = true;
            this.type11Label.Location = new System.Drawing.Point(0, 167);
            this.type11Label.Name = "type11Label";
            this.type11Label.Size = new System.Drawing.Size(94, 13);
            this.type11Label.TabIndex = 16;
            this.type11Label.Text = "LookAt Threshold";
            // 
            // type13Value
            // 
            this.type13Value.Location = new System.Drawing.Point(100, 266);
            this.type13Value.Name = "type13Value";
            this.type13Value.Size = new System.Drawing.Size(50, 20);
            this.type13Value.TabIndex = 41;
            // 
            // type13Label
            // 
            this.type13Label.AutoSize = true;
            this.type13Label.Location = new System.Drawing.Point(50, 268);
            this.type13Label.Name = "type13Label";
            this.type13Label.Size = new System.Drawing.Size(44, 13);
            this.type13Label.TabIndex = 40;
            this.type13Label.Text = "LookAt";
            // 
            // type17Value
            // 
            this.type17Value.Location = new System.Drawing.Point(100, 292);
            this.type17Value.Name = "type17Value";
            this.type17Value.Size = new System.Drawing.Size(50, 20);
            this.type17Value.TabIndex = 33;
            // 
            // type17Label
            // 
            this.type17Label.AutoSize = true;
            this.type17Label.Location = new System.Drawing.Point(58, 294);
            this.type17Label.Name = "type17Label";
            this.type17Label.Size = new System.Drawing.Size(36, 13);
            this.type17Label.TabIndex = 32;
            this.type17Label.Text = "Focus";
            // 
            // updateTimer
            // 
            this.updateTimer.Enabled = true;
            this.updateTimer.Interval = 50;
            this.updateTimer.Tick += new System.EventHandler(this.updateTimer_Tick);
            // 
            // startButton
            // 
            this.startButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.startButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startButton.Location = new System.Drawing.Point(343, 338);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(438, 20);
            this.startButton.TabIndex = 46;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // avatarsBindingSource
            // 
            this.avatarsBindingSource.AllowNew = false;
            this.avatarsBindingSource.DataSource = typeof(ProxyTestGUI.SetFollowCamPropertiestForm.Avatar);
            // 
            // avatarsListBox
            // 
            this.avatarsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.avatarsListBox.DataSource = this.avatarsBindingSource;
            this.avatarsListBox.DisplayMember = "Name";
            this.avatarsListBox.FormattingEnabled = true;
            this.avatarsListBox.Location = new System.Drawing.Point(3, 386);
            this.avatarsListBox.MultiColumn = true;
            this.avatarsListBox.Name = "avatarsListBox";
            this.avatarsListBox.Size = new System.Drawing.Size(778, 95);
            this.avatarsListBox.TabIndex = 49;
            // 
            // focusYPanel
            // 
            this.focusYPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.focusYPanel.DisplayText = "- Y";
            this.focusYPanel.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.focusYPanel.Location = new System.Drawing.Point(12, 1064);
            this.focusYPanel.Max = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.focusYPanel.Min = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.focusYPanel.Name = "focusYPanel";
            this.focusYPanel.Size = new System.Drawing.Size(194, 53);
            this.focusYPanel.SliderMultiplier = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.focusYPanel.TabIndex = 63;
            this.focusYPanel.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.DecimalPlaces = 2;
            this.numericUpDown1.Location = new System.Drawing.Point(100, 191);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(50, 20);
            this.numericUpDown1.TabIndex = 71;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 193);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 13);
            this.label2.TabIndex = 70;
            this.label2.Text = "Focus Threshold";
            // 
            // thresholdZPanel
            // 
            this.thresholdZPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.thresholdZPanel.DisplayText = "- Z";
            this.thresholdZPanel.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.thresholdZPanel.Location = new System.Drawing.Point(12, 1123);
            this.thresholdZPanel.Max = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.thresholdZPanel.Min = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.thresholdZPanel.Name = "thresholdZPanel";
            this.thresholdZPanel.Size = new System.Drawing.Size(202, 53);
            this.thresholdZPanel.SliderMultiplier = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.thresholdZPanel.TabIndex = 72;
            this.thresholdZPanel.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // focusOffsetVectorPanel
            // 
            this.focusOffsetVectorPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.focusOffsetVectorPanel.DisplayName = "Focus Offset Vector";
            this.focusOffsetVectorPanel.Location = new System.Drawing.Point(156, 9);
            this.focusOffsetVectorPanel.Max = 10D;
            this.focusOffsetVectorPanel.Min = -10D;
            this.focusOffsetVectorPanel.Name = "focusOffsetVectorPanel";
            this.focusOffsetVectorPanel.Size = new System.Drawing.Size(625, 98);
            this.focusOffsetVectorPanel.TabIndex = 74;
            this.focusOffsetVectorPanel.Value = ((OpenMetaverse.Vector3)(resources.GetObject("focusOffsetVectorPanel.Value")));
            this.focusOffsetVectorPanel.X = 0F;
            this.focusOffsetVectorPanel.Y = 0F;
            this.focusOffsetVectorPanel.Z = 0F;
            // 
            // positionVectorPanel
            // 
            this.positionVectorPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.positionVectorPanel.DisplayName = "LookAt Vector";
            this.positionVectorPanel.Location = new System.Drawing.Point(156, 98);
            this.positionVectorPanel.Max = 256D;
            this.positionVectorPanel.Min = 0D;
            this.positionVectorPanel.Name = "positionVectorPanel";
            this.positionVectorPanel.Size = new System.Drawing.Size(625, 98);
            this.positionVectorPanel.TabIndex = 75;
            this.positionVectorPanel.Value = ((OpenMetaverse.Vector3)(resources.GetObject("positionVectorPanel.Value")));
            this.positionVectorPanel.X = 128F;
            this.positionVectorPanel.Y = 128F;
            this.positionVectorPanel.Z = 22F;
            // 
            // activeCheckbox
            // 
            this.activeCheckbox.AutoSize = true;
            this.activeCheckbox.CheckAlign = System.Drawing.ContentAlignment.BottomRight;
            this.activeCheckbox.Checked = true;
            this.activeCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.activeCheckbox.Location = new System.Drawing.Point(57, 217);
            this.activeCheckbox.Name = "activeCheckbox";
            this.activeCheckbox.Size = new System.Drawing.Size(56, 17);
            this.activeCheckbox.TabIndex = 76;
            this.activeCheckbox.Text = "Active";
            this.activeCheckbox.UseVisualStyleBackColor = true;
            // 
            // focusLockedCheckbox
            // 
            this.focusLockedCheckbox.AutoSize = true;
            this.focusLockedCheckbox.CheckAlign = System.Drawing.ContentAlignment.BottomRight;
            this.focusLockedCheckbox.Checked = true;
            this.focusLockedCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.focusLockedCheckbox.Location = new System.Drawing.Point(19, 341);
            this.focusLockedCheckbox.Name = "focusLockedCheckbox";
            this.focusLockedCheckbox.Size = new System.Drawing.Size(94, 17);
            this.focusLockedCheckbox.TabIndex = 77;
            this.focusLockedCheckbox.Text = "Focus Locked";
            this.focusLockedCheckbox.UseVisualStyleBackColor = true;
            // 
            // positionLockedCheckbox
            // 
            this.positionLockedCheckbox.AutoSize = true;
            this.positionLockedCheckbox.CheckAlign = System.Drawing.ContentAlignment.BottomRight;
            this.positionLockedCheckbox.Checked = true;
            this.positionLockedCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.positionLockedCheckbox.Location = new System.Drawing.Point(11, 318);
            this.positionLockedCheckbox.Name = "positionLockedCheckbox";
            this.positionLockedCheckbox.Size = new System.Drawing.Size(102, 17);
            this.positionLockedCheckbox.TabIndex = 78;
            this.positionLockedCheckbox.Text = "LookAt Locked";
            this.positionLockedCheckbox.UseVisualStyleBackColor = true;
            // 
            // timeValue
            // 
            this.timeValue.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.timeValue.Location = new System.Drawing.Point(188, 340);
            this.timeValue.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.timeValue.Name = "timeValue";
            this.timeValue.Size = new System.Drawing.Size(50, 20);
            this.timeValue.TabIndex = 80;
            this.timeValue.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.timeValue.ValueChanged += new System.EventHandler(this.timerValue_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(119, 342);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 79;
            this.label3.Text = "Timer Delay";
            // 
            // sendPacketCheckbox
            // 
            this.sendPacketCheckbox.AutoSize = true;
            this.sendPacketCheckbox.CheckAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.sendPacketCheckbox.Checked = true;
            this.sendPacketCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.sendPacketCheckbox.Location = new System.Drawing.Point(244, 341);
            this.sendPacketCheckbox.Name = "sendPacketCheckbox";
            this.sendPacketCheckbox.Size = new System.Drawing.Size(93, 17);
            this.sendPacketCheckbox.TabIndex = 81;
            this.sendPacketCheckbox.Text = "Send Packets";
            this.sendPacketCheckbox.UseVisualStyleBackColor = true;
            // 
            // focusRotationPanel
            // 
            this.focusRotationPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.focusRotationPanel.DisplayName = "Focus";
            this.focusRotationPanel.Location = new System.Drawing.Point(156, 191);
            this.focusRotationPanel.Name = "focusRotationPanel";
            this.focusRotationPanel.Size = new System.Drawing.Size(625, 153);
            this.focusRotationPanel.TabIndex = 83;
            this.focusRotationPanel.Vector = ((OpenMetaverse.Vector3)(resources.GetObject("focusRotationPanel.Value")));
            // 
            // SetFollowCamPropertiestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(793, 491);
            this.Controls.Add(this.sendPacketCheckbox);
            this.Controls.Add(this.timeValue);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.positionLockedCheckbox);
            this.Controls.Add(this.focusLockedCheckbox);
            this.Controls.Add(this.activeCheckbox);
            this.Controls.Add(this.positionVectorPanel);
            this.Controls.Add(this.focusOffsetVectorPanel);
            this.Controls.Add(this.thresholdZPanel);
            this.Controls.Add(this.focusYPanel);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.type13Value);
            this.Controls.Add(this.type13Label);
            this.Controls.Add(this.type17Value);
            this.Controls.Add(this.type17Label);
            this.Controls.Add(this.type9Value);
            this.Controls.Add(this.type9Label);
            this.Controls.Add(this.type10Value);
            this.Controls.Add(this.type10Label);
            this.Controls.Add(this.type11Value);
            this.Controls.Add(this.type11Label);
            this.Controls.Add(this.type6Value);
            this.Controls.Add(this.type6Label);
            this.Controls.Add(this.type7Value);
            this.Controls.Add(this.type7Label);
            this.Controls.Add(this.type8Value);
            this.Controls.Add(this.type8Label);
            this.Controls.Add(this.type2Value);
            this.Controls.Add(this.type2Label);
            this.Controls.Add(this.type1Value);
            this.Controls.Add(this.type1Label);
            this.Controls.Add(this.avatarsListBox);
            this.Controls.Add(this.focusRotationPanel);
            this.Name = "SetFollowCamPropertiestForm";
            this.Text = "SetFollowCamProperties Packet GUI";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.type1Value)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.type2Value)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.type6Value)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.type7Value)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.type8Value)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.type9Value)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.type10Value)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.type11Value)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.type13Value)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.type17Value)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.avatarsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeValue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label type1Label;
        private System.Windows.Forms.NumericUpDown type1Value;
        private System.Windows.Forms.NumericUpDown type2Value;
        private System.Windows.Forms.Label type2Label;
        private System.Windows.Forms.NumericUpDown type6Value;
        private System.Windows.Forms.Label type6Label;
        private System.Windows.Forms.NumericUpDown type7Value;
        private System.Windows.Forms.Label type7Label;
        private System.Windows.Forms.NumericUpDown type8Value;
        private System.Windows.Forms.Label type8Label;
        private System.Windows.Forms.NumericUpDown type9Value;
        private System.Windows.Forms.Label type9Label;
        private System.Windows.Forms.NumericUpDown type10Value;
        private System.Windows.Forms.Label type10Label;
        private System.Windows.Forms.NumericUpDown type11Value;
        private System.Windows.Forms.Label type11Label;
        private System.Windows.Forms.NumericUpDown type13Value;
        private System.Windows.Forms.Label type13Label;
        private System.Windows.Forms.NumericUpDown type17Value;
        private System.Windows.Forms.Label type17Label;
        private System.Windows.Forms.Timer updateTimer;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.BindingSource avatarsBindingSource;
        private System.Windows.Forms.ListBox avatarsListBox;
        private ValuePanel focusYPanel;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label2;
        private ValuePanel thresholdZPanel;
        private VectorPanel focusOffsetVectorPanel;
        private VectorPanel positionVectorPanel;
        private System.Windows.Forms.CheckBox activeCheckbox;
        private System.Windows.Forms.CheckBox focusLockedCheckbox;
        private System.Windows.Forms.CheckBox positionLockedCheckbox;
        private System.Windows.Forms.NumericUpDown timeValue;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox sendPacketCheckbox;
        private RotationPanel focusRotationPanel;
    }
}


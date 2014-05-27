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
namespace Chimera.GUI.Forms {
    partial class CoordinatorForm {
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

        #region Frames Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CoordinatorForm));
            Chimera.Util.Rotation rotation2 = new Chimera.Util.Rotation();
            this.hSplit = new System.Windows.Forms.SplitContainer();
            this.diagramWorldSplit = new System.Windows.Forms.SplitContainer();
            this.diagSplit = new System.Windows.Forms.SplitContainer();
            this.realSpaceGroup = new System.Windows.Forms.GroupBox();
            this.yPerspectiveButton = new System.Windows.Forms.RadioButton();
            this.zPerspectiveButton = new System.Windows.Forms.RadioButton();
            this.xPerspectiveButton = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.testButton = new System.Windows.Forms.Button();
            this.realSpacePanel = new System.Windows.Forms.PictureBox();
            this.realSpaceScale = new System.Windows.Forms.TrackBar();
            this.virtualSpaceGroup = new System.Windows.Forms.GroupBox();
            this.heightmapPanel = new System.Windows.Forms.PictureBox();
            this.heightmapScale = new System.Windows.Forms.TrackBar();
            this.globalBox = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tickLengthPanel = new Chimera.GUI.ScalarPanel();
            this.enableUpdates = new System.Windows.Forms.CheckBox();
            this.deltaModeButton = new System.Windows.Forms.RadioButton();
            this.absoluteModeButton = new System.Windows.Forms.RadioButton();
            this.eyePositionPanel = new Chimera.GUI.VectorPanel();
            this.virtualOrientationPanel = new Chimera.GUI.RotationPanel();
            this.virtualPositionPanel = new Chimera.GUI.VectorPanel();
            this.windowsPluginsSplit = new System.Windows.Forms.SplitContainer();
            this.windowsGroup = new System.Windows.Forms.GroupBox();
            this.windowsTab = new System.Windows.Forms.TabControl();
            this.inputsGroup = new System.Windows.Forms.GroupBox();
            this.pluginsTab = new System.Windows.Forms.TabControl();
            this.overlayStatsBox = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.hSplit)).BeginInit();
            this.hSplit.Panel1.SuspendLayout();
            this.hSplit.Panel2.SuspendLayout();
            this.hSplit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.diagramWorldSplit)).BeginInit();
            this.diagramWorldSplit.Panel1.SuspendLayout();
            this.diagramWorldSplit.Panel2.SuspendLayout();
            this.diagramWorldSplit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.diagSplit)).BeginInit();
            this.diagSplit.Panel1.SuspendLayout();
            this.diagSplit.Panel2.SuspendLayout();
            this.diagSplit.SuspendLayout();
            this.realSpaceGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.realSpacePanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.realSpaceScale)).BeginInit();
            this.virtualSpaceGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.heightmapPanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.heightmapScale)).BeginInit();
            this.globalBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.windowsPluginsSplit)).BeginInit();
            this.windowsPluginsSplit.Panel1.SuspendLayout();
            this.windowsPluginsSplit.Panel2.SuspendLayout();
            this.windowsPluginsSplit.SuspendLayout();
            this.windowsGroup.SuspendLayout();
            this.inputsGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // hSplit
            // 
            this.hSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hSplit.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.hSplit.Location = new System.Drawing.Point(0, 0);
            this.hSplit.MinimumSize = new System.Drawing.Size(858, 581);
            this.hSplit.Name = "hSplit";
            this.hSplit.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // hSplit.Panel1
            // 
            this.hSplit.Panel1.Controls.Add(this.diagramWorldSplit);
            // 
            // hSplit.Panel2
            // 
            this.hSplit.Panel2.Controls.Add(this.windowsPluginsSplit);
            this.hSplit.Size = new System.Drawing.Size(911, 822);
            this.hSplit.SplitterDistance = 319;
            this.hSplit.TabIndex = 0;
            this.hSplit.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CoordinatorForm_KeyDown);
            this.hSplit.KeyUp += new System.Windows.Forms.KeyEventHandler(this.CoordinatorForm_KeyUp);
            // 
            // diagramWorldSplit
            // 
            this.diagramWorldSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.diagramWorldSplit.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.diagramWorldSplit.Location = new System.Drawing.Point(0, 0);
            this.diagramWorldSplit.Name = "diagramWorldSplit";
            // 
            // diagramWorldSplit.Panel1
            // 
            this.diagramWorldSplit.Panel1.Controls.Add(this.diagSplit);
            // 
            // diagramWorldSplit.Panel2
            // 
            this.diagramWorldSplit.Panel2.AutoScroll = true;
            this.diagramWorldSplit.Panel2.Controls.Add(this.globalBox);
            this.diagramWorldSplit.Size = new System.Drawing.Size(911, 319);
            this.diagramWorldSplit.SplitterDistance = 646;
            this.diagramWorldSplit.TabIndex = 0;
            // 
            // diagSplit
            // 
            this.diagSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.diagSplit.Location = new System.Drawing.Point(0, 0);
            this.diagSplit.Name = "diagSplit";
            // 
            // diagSplit.Panel1
            // 
            this.diagSplit.Panel1.Controls.Add(this.realSpaceGroup);
            // 
            // diagSplit.Panel2
            // 
            this.diagSplit.Panel2.Controls.Add(this.virtualSpaceGroup);
            this.diagSplit.Size = new System.Drawing.Size(646, 319);
            this.diagSplit.SplitterDistance = 361;
            this.diagSplit.TabIndex = 1;
            // 
            // realSpaceGroup
            // 
            this.realSpaceGroup.Controls.Add(this.yPerspectiveButton);
            this.realSpaceGroup.Controls.Add(this.zPerspectiveButton);
            this.realSpaceGroup.Controls.Add(this.xPerspectiveButton);
            this.realSpaceGroup.Controls.Add(this.button1);
            this.realSpaceGroup.Controls.Add(this.testButton);
            this.realSpaceGroup.Controls.Add(this.realSpacePanel);
            this.realSpaceGroup.Controls.Add(this.realSpaceScale);
            this.realSpaceGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.realSpaceGroup.Location = new System.Drawing.Point(0, 0);
            this.realSpaceGroup.Name = "realSpaceGroup";
            this.realSpaceGroup.Size = new System.Drawing.Size(361, 319);
            this.realSpaceGroup.TabIndex = 0;
            this.realSpaceGroup.TabStop = false;
            this.realSpaceGroup.Text = "Real Space";
            // 
            // yPerspectiveButton
            // 
            this.yPerspectiveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.yPerspectiveButton.AutoSize = true;
            this.yPerspectiveButton.Location = new System.Drawing.Point(284, 265);
            this.yPerspectiveButton.Name = "yPerspectiveButton";
            this.yPerspectiveButton.Size = new System.Drawing.Size(32, 17);
            this.yPerspectiveButton.TabIndex = 6;
            this.yPerspectiveButton.Text = "Y";
            this.yPerspectiveButton.UseVisualStyleBackColor = true;
            this.yPerspectiveButton.CheckedChanged += new System.EventHandler(this.PerspectiveButton_CheckedChanged);
            // 
            // zPerspectiveButton
            // 
            this.zPerspectiveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.zPerspectiveButton.AutoSize = true;
            this.zPerspectiveButton.Checked = true;
            this.zPerspectiveButton.Location = new System.Drawing.Point(322, 265);
            this.zPerspectiveButton.Name = "zPerspectiveButton";
            this.zPerspectiveButton.Size = new System.Drawing.Size(32, 17);
            this.zPerspectiveButton.TabIndex = 5;
            this.zPerspectiveButton.TabStop = true;
            this.zPerspectiveButton.Text = "Z";
            this.zPerspectiveButton.UseVisualStyleBackColor = true;
            this.zPerspectiveButton.CheckedChanged += new System.EventHandler(this.PerspectiveButton_CheckedChanged);
            // 
            // xPerspectiveButton
            // 
            this.xPerspectiveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.xPerspectiveButton.AutoSize = true;
            this.xPerspectiveButton.Location = new System.Drawing.Point(246, 265);
            this.xPerspectiveButton.Name = "xPerspectiveButton";
            this.xPerspectiveButton.Size = new System.Drawing.Size(32, 17);
            this.xPerspectiveButton.TabIndex = 4;
            this.xPerspectiveButton.Text = "X";
            this.xPerspectiveButton.UseVisualStyleBackColor = true;
            this.xPerspectiveButton.CheckedChanged += new System.EventHandler(this.PerspectiveButton_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 48);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(91, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Crash - Thread";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // testButton
            // 
            this.testButton.Location = new System.Drawing.Point(6, 19);
            this.testButton.Name = "testButton";
            this.testButton.Size = new System.Drawing.Size(91, 23);
            this.testButton.TabIndex = 1;
            this.testButton.Text = "Crash - GUI";
            this.testButton.UseVisualStyleBackColor = true;
            this.testButton.Click += new System.EventHandler(this.testButton_Click);
            // 
            // realSpacePanel
            // 
            this.realSpacePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.realSpacePanel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.realSpacePanel.Location = new System.Drawing.Point(3, 16);
            this.realSpacePanel.Name = "realSpacePanel";
            this.realSpacePanel.Size = new System.Drawing.Size(355, 274);
            this.realSpacePanel.TabIndex = 3;
            this.realSpacePanel.TabStop = false;
            this.realSpacePanel.Paint += new System.Windows.Forms.PaintEventHandler(this.realSpacePanel_Paint);
            this.realSpacePanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.realSpacePanel_MouseDown);
            this.realSpacePanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.realSpacePanel_MouseMove);
            this.realSpacePanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.realSpacePanel_MouseUp);
            this.realSpacePanel.Resize += new System.EventHandler(this.realSpacePanel_Resize);
            // 
            // realSpaceScale
            // 
            this.realSpaceScale.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.realSpaceScale.Location = new System.Drawing.Point(5, 291);
            this.realSpaceScale.Maximum = 1000;
            this.realSpaceScale.Minimum = 1;
            this.realSpaceScale.Name = "realSpaceScale";
            this.realSpaceScale.Size = new System.Drawing.Size(353, 42);
            this.realSpaceScale.TabIndex = 3;
            this.realSpaceScale.TickStyle = System.Windows.Forms.TickStyle.None;
            this.realSpaceScale.Value = 1000;
            this.realSpaceScale.Scroll += new System.EventHandler(this.realSpaceScale_Scroll);
            // 
            // virtualSpaceGroup
            // 
            this.virtualSpaceGroup.Controls.Add(this.heightmapPanel);
            this.virtualSpaceGroup.Controls.Add(this.heightmapScale);
            this.virtualSpaceGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.virtualSpaceGroup.Location = new System.Drawing.Point(0, 0);
            this.virtualSpaceGroup.Name = "virtualSpaceGroup";
            this.virtualSpaceGroup.Size = new System.Drawing.Size(281, 319);
            this.virtualSpaceGroup.TabIndex = 0;
            this.virtualSpaceGroup.TabStop = false;
            this.virtualSpaceGroup.Text = "Virtual Space";
            // 
            // heightmapPanel
            // 
            this.heightmapPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.heightmapPanel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.heightmapPanel.Location = new System.Drawing.Point(3, 16);
            this.heightmapPanel.Name = "heightmapPanel";
            this.heightmapPanel.Size = new System.Drawing.Size(275, 275);
            this.heightmapPanel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.heightmapPanel.TabIndex = 1;
            this.heightmapPanel.TabStop = false;
            this.heightmapPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.heightmapPanel_Paint);
            this.heightmapPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.heightmapPanel_MouseDown);
            this.heightmapPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.heightmapPanel_MouseMove);
            this.heightmapPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.heightmapPanel_MouseUp);
            // 
            // heightmapScale
            // 
            this.heightmapScale.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.heightmapScale.Location = new System.Drawing.Point(3, 291);
            this.heightmapScale.Maximum = 16000;
            this.heightmapScale.Minimum = 1000;
            this.heightmapScale.Name = "heightmapScale";
            this.heightmapScale.Size = new System.Drawing.Size(275, 42);
            this.heightmapScale.TabIndex = 2;
            this.heightmapScale.TickStyle = System.Windows.Forms.TickStyle.None;
            this.heightmapScale.Value = 1000;
            this.heightmapScale.Scroll += new System.EventHandler(this.virtualZoom_Scroll);
            // 
            // globalBox
            // 
            this.globalBox.Controls.Add(this.label1);
            this.globalBox.Controls.Add(this.tickLengthPanel);
            this.globalBox.Controls.Add(this.enableUpdates);
            this.globalBox.Controls.Add(this.deltaModeButton);
            this.globalBox.Controls.Add(this.absoluteModeButton);
            this.globalBox.Controls.Add(this.eyePositionPanel);
            this.globalBox.Controls.Add(this.virtualOrientationPanel);
            this.globalBox.Controls.Add(this.virtualPositionPanel);
            this.globalBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.globalBox.Location = new System.Drawing.Point(0, 0);
            this.globalBox.MinimumSize = new System.Drawing.Size(261, 290);
            this.globalBox.Name = "globalBox";
            this.globalBox.Size = new System.Drawing.Size(261, 319);
            this.globalBox.TabIndex = 0;
            this.globalBox.TabStop = false;
            this.globalBox.Text = "Global";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(191, 277);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Tick Length";
            // 
            // tickLengthPanel
            // 
            this.tickLengthPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tickLengthPanel.Location = new System.Drawing.Point(104, 291);
            this.tickLengthPanel.Max = 500F;
            this.tickLengthPanel.Min = 0F;
            this.tickLengthPanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.tickLengthPanel.Name = "tickLengthPanel";
            this.tickLengthPanel.Size = new System.Drawing.Size(151, 20);
            this.tickLengthPanel.TabIndex = 7;
            this.tickLengthPanel.Value = 0F;
            this.tickLengthPanel.ValueChanged += new System.Action<float>(this.tickLengthPanel_ValueChanged);
            // 
            // enableUpdates
            // 
            this.enableUpdates.AutoSize = true;
            this.enableUpdates.Location = new System.Drawing.Point(6, 291);
            this.enableUpdates.Name = "enableUpdates";
            this.enableUpdates.Size = new System.Drawing.Size(101, 17);
            this.enableUpdates.TabIndex = 6;
            this.enableUpdates.Text = "Control Enabled";
            this.enableUpdates.UseVisualStyleBackColor = true;
            this.enableUpdates.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // deltaModeButton
            // 
            this.deltaModeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.deltaModeButton.AutoSize = true;
            this.deltaModeButton.Location = new System.Drawing.Point(199, 12);
            this.deltaModeButton.Name = "deltaModeButton";
            this.deltaModeButton.Size = new System.Drawing.Size(50, 17);
            this.deltaModeButton.TabIndex = 5;
            this.deltaModeButton.TabStop = true;
            this.deltaModeButton.Text = "Delta";
            this.deltaModeButton.UseVisualStyleBackColor = true;
            this.deltaModeButton.CheckedChanged += new System.EventHandler(this.deltaModeButton_CheckedChanged);
            // 
            // absoluteModeButton
            // 
            this.absoluteModeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.absoluteModeButton.AutoSize = true;
            this.absoluteModeButton.Location = new System.Drawing.Point(127, 12);
            this.absoluteModeButton.Name = "absoluteModeButton";
            this.absoluteModeButton.Size = new System.Drawing.Size(66, 17);
            this.absoluteModeButton.TabIndex = 4;
            this.absoluteModeButton.TabStop = true;
            this.absoluteModeButton.Text = "Absolute";
            this.absoluteModeButton.UseVisualStyleBackColor = true;
            this.absoluteModeButton.CheckedChanged += new System.EventHandler(this.absoluteModeButton_CheckedChanged);
            // 
            // eyePositionPanel
            // 
            this.eyePositionPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.eyePositionPanel.Location = new System.Drawing.Point(3, 195);
            this.eyePositionPanel.Max = 5000F;
            this.eyePositionPanel.MaxV = ((OpenMetaverse.Vector3)(resources.GetObject("eyePositionPanel.MaxV")));
            this.eyePositionPanel.Min = -5000F;
            this.eyePositionPanel.MinimumSize = new System.Drawing.Size(103, 95);
            this.eyePositionPanel.MinV = ((OpenMetaverse.Vector3)(resources.GetObject("eyePositionPanel.MinV")));
            this.eyePositionPanel.Name = "eyePositionPanel";
            this.eyePositionPanel.Size = new System.Drawing.Size(255, 95);
            this.eyePositionPanel.TabIndex = 1;
            this.eyePositionPanel.Value = ((OpenMetaverse.Vector3)(resources.GetObject("eyePositionPanel.Value")));
            this.eyePositionPanel.X = 0F;
            this.eyePositionPanel.Y = 0F;
            this.eyePositionPanel.Z = 0F;
            this.eyePositionPanel.ValueChanged += new System.EventHandler(this.eyePositionPanel_OnChange);
            // 
            // virtualOrientationPanel
            // 
            this.virtualOrientationPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.virtualOrientationPanel.Location = new System.Drawing.Point(3, 105);
            this.virtualOrientationPanel.LookAtVector = ((OpenMetaverse.Vector3)(resources.GetObject("virtualOrientationPanel.LookAtVector")));
            this.virtualOrientationPanel.MinimumSize = new System.Drawing.Size(252, 95);
            this.virtualOrientationPanel.Name = "virtualOrientationPanel";
            this.virtualOrientationPanel.Pitch = 0D;
            this.virtualOrientationPanel.Quaternion = ((OpenMetaverse.Quaternion)(resources.GetObject("virtualOrientationPanel.Quaternion")));
            this.virtualOrientationPanel.Size = new System.Drawing.Size(255, 95);
            this.virtualOrientationPanel.TabIndex = 2;
            rotation2.LookAtVector = ((OpenMetaverse.Vector3)(resources.GetObject("rotation2.LookAtVector")));
            rotation2.Pitch = 0D;
            rotation2.Quaternion = ((OpenMetaverse.Quaternion)(resources.GetObject("rotation2.Quaternion")));
            rotation2.Yaw = 0D;
            this.virtualOrientationPanel.Value = rotation2;
            this.virtualOrientationPanel.Yaw = 0D;
            this.virtualOrientationPanel.OnChange += new System.EventHandler(this.virtualRotation_OnChange);
            // 
            // virtualPositionPanel
            // 
            this.virtualPositionPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.virtualPositionPanel.Location = new System.Drawing.Point(3, 12);
            this.virtualPositionPanel.Max = 1024F;
            this.virtualPositionPanel.MaxV = ((OpenMetaverse.Vector3)(resources.GetObject("virtualPositionPanel.MaxV")));
            this.virtualPositionPanel.Min = -1024F;
            this.virtualPositionPanel.MinimumSize = new System.Drawing.Size(103, 95);
            this.virtualPositionPanel.MinV = ((OpenMetaverse.Vector3)(resources.GetObject("virtualPositionPanel.MinV")));
            this.virtualPositionPanel.Name = "virtualPositionPanel";
            this.virtualPositionPanel.Size = new System.Drawing.Size(255, 95);
            this.virtualPositionPanel.TabIndex = 0;
            this.virtualPositionPanel.Value = ((OpenMetaverse.Vector3)(resources.GetObject("virtualPositionPanel.Value")));
            this.virtualPositionPanel.X = 0F;
            this.virtualPositionPanel.Y = 0F;
            this.virtualPositionPanel.Z = 0F;
            this.virtualPositionPanel.ValueChanged += new System.EventHandler(this.virtualPositionPanel_OnChange);
            // 
            // windowsPluginsSplit
            // 
            this.windowsPluginsSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.windowsPluginsSplit.Location = new System.Drawing.Point(0, 0);
            this.windowsPluginsSplit.Name = "windowsPluginsSplit";
            // 
            // windowsPluginsSplit.Panel1
            // 
            this.windowsPluginsSplit.Panel1.Controls.Add(this.windowsGroup);
            // 
            // windowsPluginsSplit.Panel2
            // 
            this.windowsPluginsSplit.Panel2.Controls.Add(this.inputsGroup);
            this.windowsPluginsSplit.Size = new System.Drawing.Size(911, 499);
            this.windowsPluginsSplit.SplitterDistance = 445;
            this.windowsPluginsSplit.TabIndex = 0;
            this.windowsPluginsSplit.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CoordinatorForm_KeyDown);
            this.windowsPluginsSplit.KeyUp += new System.Windows.Forms.KeyEventHandler(this.CoordinatorForm_KeyUp);
            // 
            // windowsGroup
            // 
            this.windowsGroup.Controls.Add(this.windowsTab);
            this.windowsGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.windowsGroup.Location = new System.Drawing.Point(0, 0);
            this.windowsGroup.Name = "windowsGroup";
            this.windowsGroup.Size = new System.Drawing.Size(445, 499);
            this.windowsGroup.TabIndex = 0;
            this.windowsGroup.TabStop = false;
            this.windowsGroup.Text = "Frames";
            // 
            // windowsTab
            // 
            this.windowsTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.windowsTab.Location = new System.Drawing.Point(3, 16);
            this.windowsTab.Name = "windowsTab";
            this.windowsTab.SelectedIndex = 0;
            this.windowsTab.Size = new System.Drawing.Size(439, 480);
            this.windowsTab.TabIndex = 0;
            this.windowsTab.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CoordinatorForm_KeyDown);
            this.windowsTab.KeyUp += new System.Windows.Forms.KeyEventHandler(this.CoordinatorForm_KeyUp);
            // 
            // inputsGroup
            // 
            this.inputsGroup.Controls.Add(this.pluginsTab);
            this.inputsGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.inputsGroup.Location = new System.Drawing.Point(0, 0);
            this.inputsGroup.Name = "inputsGroup";
            this.inputsGroup.Size = new System.Drawing.Size(462, 499);
            this.inputsGroup.TabIndex = 0;
            this.inputsGroup.TabStop = false;
            this.inputsGroup.Text = "Plugins";
            // 
            // pluginsTab
            // 
            this.pluginsTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pluginsTab.Location = new System.Drawing.Point(3, 16);
            this.pluginsTab.Name = "pluginsTab";
            this.pluginsTab.SelectedIndex = 0;
            this.pluginsTab.Size = new System.Drawing.Size(456, 480);
            this.pluginsTab.TabIndex = 0;
            this.pluginsTab.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CoordinatorForm_KeyDown);
            this.pluginsTab.KeyUp += new System.Windows.Forms.KeyEventHandler(this.CoordinatorForm_KeyUp);
            // 
            // overlayStatsBox
            // 
            this.overlayStatsBox.Location = new System.Drawing.Point(0, 0);
            this.overlayStatsBox.Name = "overlayStatsBox";
            this.overlayStatsBox.Size = new System.Drawing.Size(100, 96);
            this.overlayStatsBox.TabIndex = 0;
            this.overlayStatsBox.Text = "";
            // 
            // CoordinatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(911, 822);
            this.Controls.Add(this.hSplit);
            this.Name = "CoordinatorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Caen Control Panel";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CoordinatorForm_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CoordinatorForm_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.CoordinatorForm_KeyUp);
            this.hSplit.Panel1.ResumeLayout(false);
            this.hSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.hSplit)).EndInit();
            this.hSplit.ResumeLayout(false);
            this.diagramWorldSplit.Panel1.ResumeLayout(false);
            this.diagramWorldSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.diagramWorldSplit)).EndInit();
            this.diagramWorldSplit.ResumeLayout(false);
            this.diagSplit.Panel1.ResumeLayout(false);
            this.diagSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.diagSplit)).EndInit();
            this.diagSplit.ResumeLayout(false);
            this.realSpaceGroup.ResumeLayout(false);
            this.realSpaceGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.realSpacePanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.realSpaceScale)).EndInit();
            this.virtualSpaceGroup.ResumeLayout(false);
            this.virtualSpaceGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.heightmapPanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.heightmapScale)).EndInit();
            this.globalBox.ResumeLayout(false);
            this.globalBox.PerformLayout();
            this.windowsPluginsSplit.Panel1.ResumeLayout(false);
            this.windowsPluginsSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.windowsPluginsSplit)).EndInit();
            this.windowsPluginsSplit.ResumeLayout(false);
            this.windowsGroup.ResumeLayout(false);
            this.inputsGroup.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer hSplit;
        private System.Windows.Forms.SplitContainer diagramWorldSplit;
        private System.Windows.Forms.SplitContainer windowsPluginsSplit;
        private System.Windows.Forms.GroupBox globalBox;
        private System.Windows.Forms.GroupBox windowsGroup;
        private System.Windows.Forms.GroupBox inputsGroup;
        private System.Windows.Forms.TabControl windowsTab;
        private System.Windows.Forms.TabControl pluginsTab;
        private Chimera.GUI.VectorPanel virtualPositionPanel;
        private Chimera.GUI.VectorPanel eyePositionPanel;
        private Chimera.GUI.RotationPanel virtualOrientationPanel;
        private System.Windows.Forms.SplitContainer diagSplit;
        private System.Windows.Forms.GroupBox realSpaceGroup;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button testButton;
        private System.Windows.Forms.GroupBox virtualSpaceGroup;
        private System.Windows.Forms.PictureBox heightmapPanel;
        private System.Windows.Forms.PictureBox realSpacePanel;
        private System.Windows.Forms.RadioButton yPerspectiveButton;
        private System.Windows.Forms.RadioButton zPerspectiveButton;
        private System.Windows.Forms.RadioButton xPerspectiveButton;
        private System.Windows.Forms.TrackBar heightmapScale;
        private System.Windows.Forms.TrackBar realSpaceScale;
        private System.Windows.Forms.RadioButton deltaModeButton;
        private System.Windows.Forms.RadioButton absoluteModeButton;
        private System.Windows.Forms.RichTextBox overlayStatsBox;
        private System.Windows.Forms.CheckBox enableUpdates;
#if DEBUG
        private System.Windows.Forms.TabPage statisticsTab;
        private System.Windows.Forms.TabControl statsTabs;
        private System.Windows.Forms.TabPage tickTab;
        private System.Windows.Forms.TabPage cameraTab;
        private System.Windows.Forms.TabPage deltaTab;
        private System.Windows.Forms.TabPage usageTab;
        private Controls.StatisticsPanel tickStatsPanel;
        private Controls.StatisticsPanel cameraStatsPanel;
        private Controls.StatisticsPanel deltaStatsPanel;
        private System.Windows.Forms.TabPage updatesTab;
        private Controls.StatisticsPanel updateStatsPanel;
        private Controls.StatisticsCollectionPanel tickListenersPanel;
        private System.Windows.Forms.TabPage tickListenersTab;
#endif
        private System.Windows.Forms.Label label1;
        private ScalarPanel tickLengthPanel;
    }
}

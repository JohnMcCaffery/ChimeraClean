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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CoordinatorForm));
            Chimera.Util.Rotation rotation1 = new Chimera.Util.Rotation();
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
            this.deltaModeButton = new System.Windows.Forms.RadioButton();
            this.absoluteModeButton = new System.Windows.Forms.RadioButton();
            this.triggerHelpButton = new System.Windows.Forms.Button();
            this.eyePositionPanel = new Chimera.GUI.VectorPanel();
            this.virtualOrientationPanel = new Chimera.GUI.RotationPanel();
            this.virtualPositionPanel = new Chimera.GUI.VectorPanel();
            this.windowsPluginsSplit = new System.Windows.Forms.SplitContainer();
            this.windowsGroup = new System.Windows.Forms.GroupBox();
            this.windowsTab = new System.Windows.Forms.TabControl();
            this.inputsGroup = new System.Windows.Forms.GroupBox();
            this.inputsTab = new System.Windows.Forms.TabControl();
            this.statisticsTab = new System.Windows.Forms.TabPage();
            this.overlayStatsBox = new System.Windows.Forms.RichTextBox();
            this.tickCountLabel = new System.Windows.Forms.Label();
            this.shortestWorkLabel = new System.Windows.Forms.Label();
            this.longestWorkLabel = new System.Windows.Forms.Label();
            this.meanWorkLabel = new System.Windows.Forms.Label();
            this.longestTickLabel = new System.Windows.Forms.Label();
            this.shortestTickLabel = new System.Windows.Forms.Label();
            this.meanTickLabel = new System.Windows.Forms.Label();
            this.tpsLabel = new System.Windows.Forms.Label();
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
            this.statisticsTab.SuspendLayout();
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
            // 
            // testButton
            // 
            this.testButton.Location = new System.Drawing.Point(6, 19);
            this.testButton.Name = "testButton";
            this.testButton.Size = new System.Drawing.Size(91, 23);
            this.testButton.TabIndex = 1;
            this.testButton.Text = "Crash - GUI";
            this.testButton.UseVisualStyleBackColor = true;
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
            this.realSpaceScale.Size = new System.Drawing.Size(353, 45);
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
            this.heightmapScale.Size = new System.Drawing.Size(275, 45);
            this.heightmapScale.TabIndex = 2;
            this.heightmapScale.TickStyle = System.Windows.Forms.TickStyle.None;
            this.heightmapScale.Value = 1000;
            this.heightmapScale.Scroll += new System.EventHandler(this.virtualZoom_Scroll);
            // 
            // globalBox
            // 
            this.globalBox.Controls.Add(this.deltaModeButton);
            this.globalBox.Controls.Add(this.absoluteModeButton);
            this.globalBox.Controls.Add(this.triggerHelpButton);
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
            // triggerHelpButton
            // 
            this.triggerHelpButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.triggerHelpButton.Location = new System.Drawing.Point(7, 288);
            this.triggerHelpButton.Name = "triggerHelpButton";
            this.triggerHelpButton.Size = new System.Drawing.Size(248, 23);
            this.triggerHelpButton.TabIndex = 3;
            this.triggerHelpButton.Text = "CustomTrigger Help";
            this.triggerHelpButton.UseVisualStyleBackColor = true;
            this.triggerHelpButton.Click += new System.EventHandler(this.triggerHelpButton_Click);
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
            rotation1.LookAtVector = ((OpenMetaverse.Vector3)(resources.GetObject("rotation1.LookAtVector")));
            rotation1.Pitch = 0D;
            rotation1.Quaternion = ((OpenMetaverse.Quaternion)(resources.GetObject("rotation1.Quaternion")));
            rotation1.Yaw = 0D;
            this.virtualOrientationPanel.Value = rotation1;
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
            this.windowsGroup.Text = "Windows";
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
            this.inputsGroup.Controls.Add(this.inputsTab);
            this.inputsGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.inputsGroup.Location = new System.Drawing.Point(0, 0);
            this.inputsGroup.Name = "inputsGroup";
            this.inputsGroup.Size = new System.Drawing.Size(462, 499);
            this.inputsGroup.TabIndex = 0;
            this.inputsGroup.TabStop = false;
            this.inputsGroup.Text = "Inputs";
            // 
            // inputsTab
            // 
            this.inputsTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.inputsTab.Location = new System.Drawing.Point(3, 16);
            this.inputsTab.Name = "inputsTab";
            this.inputsTab.SelectedIndex = 0;
            this.inputsTab.Size = new System.Drawing.Size(456, 480);
            this.inputsTab.TabIndex = 0;
            this.inputsTab.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CoordinatorForm_KeyDown);
            this.inputsTab.KeyUp += new System.Windows.Forms.KeyEventHandler(this.CoordinatorForm_KeyUp);
            // 
            // statisticsTab
            // 
            this.statisticsTab.Controls.Add(this.overlayStatsBox);
            this.statisticsTab.Controls.Add(this.tickCountLabel);
            this.statisticsTab.Controls.Add(this.shortestWorkLabel);
            this.statisticsTab.Controls.Add(this.longestWorkLabel);
            this.statisticsTab.Controls.Add(this.meanWorkLabel);
            this.statisticsTab.Controls.Add(this.longestTickLabel);
            this.statisticsTab.Controls.Add(this.shortestTickLabel);
            this.statisticsTab.Controls.Add(this.meanTickLabel);
            this.statisticsTab.Controls.Add(this.tpsLabel);
            this.statisticsTab.Location = new System.Drawing.Point(4, 22);
            this.statisticsTab.Name = "statisticsTab";
            this.statisticsTab.Padding = new System.Windows.Forms.Padding(3);
            this.statisticsTab.Size = new System.Drawing.Size(448, 454);
            this.statisticsTab.TabIndex = 0;
            this.statisticsTab.Text = "Statistics";
            this.statisticsTab.UseVisualStyleBackColor = true;
            // 
            // overlayStatsBox
            // 
            this.overlayStatsBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.overlayStatsBox.Enabled = false;
            this.overlayStatsBox.Location = new System.Drawing.Point(111, -2);
            this.overlayStatsBox.Name = "overlayStatsBox";
            this.overlayStatsBox.Size = new System.Drawing.Size(334, 450);
            this.overlayStatsBox.TabIndex = 8;
            this.overlayStatsBox.Text = "";
            // 
            // tickCountLabel
            // 
            this.tickCountLabel.AutoSize = true;
            this.tickCountLabel.Location = new System.Drawing.Point(6, 94);
            this.tickCountLabel.Name = "tickCountLabel";
            this.tickCountLabel.Size = new System.Drawing.Size(59, 13);
            this.tickCountLabel.TabIndex = 7;
            this.tickCountLabel.Text = "Tick Count";
            // 
            // shortestWorkLabel
            // 
            this.shortestWorkLabel.AutoSize = true;
            this.shortestWorkLabel.Location = new System.Drawing.Point(6, 81);
            this.shortestWorkLabel.Name = "shortestWorkLabel";
            this.shortestWorkLabel.Size = new System.Drawing.Size(75, 13);
            this.shortestWorkLabel.TabIndex = 6;
            this.shortestWorkLabel.Text = "Shortest Work";
            // 
            // longestWorkLabel
            // 
            this.longestWorkLabel.AutoSize = true;
            this.longestWorkLabel.Location = new System.Drawing.Point(6, 68);
            this.longestWorkLabel.Name = "longestWorkLabel";
            this.longestWorkLabel.Size = new System.Drawing.Size(74, 13);
            this.longestWorkLabel.TabIndex = 5;
            this.longestWorkLabel.Text = "Longest Work";
            // 
            // meanWorkLabel
            // 
            this.meanWorkLabel.AutoSize = true;
            this.meanWorkLabel.Location = new System.Drawing.Point(6, 55);
            this.meanWorkLabel.Name = "meanWorkLabel";
            this.meanWorkLabel.Size = new System.Drawing.Size(99, 13);
            this.meanWorkLabel.TabIndex = 4;
            this.meanWorkLabel.Text = "Mean Work Length";
            // 
            // longestTickLabel
            // 
            this.longestTickLabel.AutoSize = true;
            this.longestTickLabel.Location = new System.Drawing.Point(6, 29);
            this.longestTickLabel.Name = "longestTickLabel";
            this.longestTickLabel.Size = new System.Drawing.Size(69, 13);
            this.longestTickLabel.TabIndex = 3;
            this.longestTickLabel.Text = "Longest Tick";
            // 
            // shortestTickLabel
            // 
            this.shortestTickLabel.AutoSize = true;
            this.shortestTickLabel.Location = new System.Drawing.Point(6, 42);
            this.shortestTickLabel.Name = "shortestTickLabel";
            this.shortestTickLabel.Size = new System.Drawing.Size(70, 13);
            this.shortestTickLabel.TabIndex = 2;
            this.shortestTickLabel.Text = "Shortest Tick";
            // 
            // meanTickLabel
            // 
            this.meanTickLabel.AutoSize = true;
            this.meanTickLabel.Location = new System.Drawing.Point(6, 16);
            this.meanTickLabel.Name = "meanTickLabel";
            this.meanTickLabel.Size = new System.Drawing.Size(94, 13);
            this.meanTickLabel.TabIndex = 1;
            this.meanTickLabel.Text = "Mean Tick Length";
            // 
            // tpsLabel
            // 
            this.tpsLabel.AutoSize = true;
            this.tpsLabel.Location = new System.Drawing.Point(6, 3);
            this.tpsLabel.Name = "tpsLabel";
            this.tpsLabel.Size = new System.Drawing.Size(81, 13);
            this.tpsLabel.TabIndex = 0;
            this.tpsLabel.Text = "Ticks / Second";
            // 
            // CoordinatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(911, 822);
            this.Controls.Add(this.hSplit);
            this.Name = "CoordinatorForm";
            this.Text = "Caen Control Panel";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
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
            this.statisticsTab.ResumeLayout(false);
            this.statisticsTab.PerformLayout();
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
        private System.Windows.Forms.TabControl inputsTab;
        private Chimera.GUI.VectorPanel virtualPositionPanel;
        private Chimera.GUI.VectorPanel eyePositionPanel;
        private Chimera.GUI.RotationPanel virtualOrientationPanel;
        private System.Windows.Forms.Button triggerHelpButton;
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
        private System.Windows.Forms.TabPage statisticsTab;
        private System.Windows.Forms.Label shortestWorkLabel;
        private System.Windows.Forms.Label longestWorkLabel;
        private System.Windows.Forms.Label meanWorkLabel;
        private System.Windows.Forms.Label longestTickLabel;
        private System.Windows.Forms.Label shortestTickLabel;
        private System.Windows.Forms.Label meanTickLabel;
        private System.Windows.Forms.Label tpsLabel;
        private System.Windows.Forms.Label tickCountLabel;
        private System.Windows.Forms.RadioButton deltaModeButton;
        private System.Windows.Forms.RadioButton absoluteModeButton;
        private System.Windows.Forms.RichTextBox overlayStatsBox;
    }
}
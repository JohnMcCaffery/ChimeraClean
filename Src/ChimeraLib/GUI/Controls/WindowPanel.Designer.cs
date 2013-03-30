namespace Chimera.GUI.Controls {
    partial class WindowPanel {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WindowPanel));
            Chimera.Util.Rotation rotation2 = new Chimera.Util.Rotation();
            this.mainTab = new System.Windows.Forms.TabControl();
            this.configTab = new System.Windows.Forms.TabPage();
            this.restartButton = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.linkFOVCheck = new System.Windows.Forms.CheckBox();
            this.anchorBox = new System.Windows.Forms.GroupBox();
            this.centreAnchorButton = new System.Windows.Forms.RadioButton();
            this.topLeftAnchorButton = new System.Windows.Forms.RadioButton();
            this.widthPanel = new Chimera.GUI.ScalarPanel();
            this.projectionGroup = new System.Windows.Forms.GroupBox();
            this.calculatedProjectionButton = new System.Windows.Forms.RadioButton();
            this.skewedProjectionButton = new System.Windows.Forms.RadioButton();
            this.simpleProjectionButton = new System.Windows.Forms.RadioButton();
            this.label11 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.monitorPulldown = new System.Windows.Forms.ComboBox();
            this.heightPanel = new Chimera.GUI.ScalarPanel();
            this.label10 = new System.Windows.Forms.Label();
            this.fovVPanel = new Chimera.GUI.ScalarPanel();
            this.distancePanel = new Chimera.GUI.ScalarPanel();
            this.label6 = new System.Windows.Forms.Label();
            this.skewHPanel = new Chimera.GUI.ScalarPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.skewVPanel = new Chimera.GUI.ScalarPanel();
            this.aspectRatioWValue = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.aspectRatioValue = new System.Windows.Forms.NumericUpDown();
            this.fovHPanel = new Chimera.GUI.ScalarPanel();
            this.aspectRatioHValue = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.orientationPanel = new Chimera.GUI.RotationPanel();
            this.centrePanel = new Chimera.GUI.VectorPanel();
            this.topLeftPanel = new Chimera.GUI.VectorPanel();
            this.overlayTab = new System.Windows.Forms.TabPage();
            this.controlCursor = new System.Windows.Forms.CheckBox();
            this.bringToFrontButtin = new System.Windows.Forms.Button();
            this.fullscreenCheck = new System.Windows.Forms.CheckBox();
            this.launchOverlayButton = new System.Windows.Forms.Button();
            this.mainTab.SuspendLayout();
            this.configTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.anchorBox.SuspendLayout();
            this.projectionGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.aspectRatioWValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.aspectRatioValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.aspectRatioHValue)).BeginInit();
            this.overlayTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainTab
            // 
            this.mainTab.Controls.Add(this.configTab);
            this.mainTab.Controls.Add(this.overlayTab);
            this.mainTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTab.Location = new System.Drawing.Point(0, 0);
            this.mainTab.Name = "mainTab";
            this.mainTab.SelectedIndex = 0;
            this.mainTab.Size = new System.Drawing.Size(841, 625);
            this.mainTab.TabIndex = 0;
            this.mainTab.KeyDown += new System.Windows.Forms.KeyEventHandler(this.mainTab_KeyDown);
            this.mainTab.KeyUp += new System.Windows.Forms.KeyEventHandler(this.mainTab_KeyUp);
            // 
            // configTab
            // 
            this.configTab.Controls.Add(this.restartButton);
            this.configTab.Controls.Add(this.splitContainer1);
            this.configTab.Location = new System.Drawing.Point(4, 22);
            this.configTab.Name = "configTab";
            this.configTab.Padding = new System.Windows.Forms.Padding(3);
            this.configTab.Size = new System.Drawing.Size(833, 599);
            this.configTab.TabIndex = 0;
            this.configTab.Text = "Configuration";
            this.configTab.UseVisualStyleBackColor = true;
            // 
            // restartButton
            // 
            this.restartButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.restartButton.BackColor = System.Drawing.Color.Red;
            this.restartButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.restartButton.Location = new System.Drawing.Point(3, 298);
            this.restartButton.Name = "restartButton";
            this.restartButton.Size = new System.Drawing.Size(830, 295);
            this.restartButton.TabIndex = 7;
            this.restartButton.Text = "Restart";
            this.restartButton.UseVisualStyleBackColor = false;
            this.restartButton.Click += new System.EventHandler(this.restartButton_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(3, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.linkFOVCheck);
            this.splitContainer1.Panel1.Controls.Add(this.anchorBox);
            this.splitContainer1.Panel1.Controls.Add(this.widthPanel);
            this.splitContainer1.Panel1.Controls.Add(this.projectionGroup);
            this.splitContainer1.Panel1.Controls.Add(this.label11);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.monitorPulldown);
            this.splitContainer1.Panel1.Controls.Add(this.heightPanel);
            this.splitContainer1.Panel1.Controls.Add(this.label10);
            this.splitContainer1.Panel1.Controls.Add(this.fovVPanel);
            this.splitContainer1.Panel1.Controls.Add(this.distancePanel);
            this.splitContainer1.Panel1.Controls.Add(this.label6);
            this.splitContainer1.Panel1.Controls.Add(this.skewHPanel);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.label5);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.skewVPanel);
            this.splitContainer1.Panel1.Controls.Add(this.aspectRatioWValue);
            this.splitContainer1.Panel1.Controls.Add(this.label9);
            this.splitContainer1.Panel1.Controls.Add(this.aspectRatioValue);
            this.splitContainer1.Panel1.Controls.Add(this.fovHPanel);
            this.splitContainer1.Panel1.Controls.Add(this.aspectRatioHValue);
            this.splitContainer1.Panel1.Controls.Add(this.label4);
            this.splitContainer1.Panel1.Controls.Add(this.label7);
            this.splitContainer1.Panel1.Controls.Add(this.label8);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.orientationPanel);
            this.splitContainer1.Panel2.Controls.Add(this.centrePanel);
            this.splitContainer1.Panel2.Controls.Add(this.topLeftPanel);
            this.splitContainer1.Size = new System.Drawing.Size(830, 295);
            this.splitContainer1.SplitterDistance = 565;
            this.splitContainer1.TabIndex = 8;
            // 
            // linkFOVCheck
            // 
            this.linkFOVCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkFOVCheck.AutoSize = true;
            this.linkFOVCheck.Location = new System.Drawing.Point(515, 206);
            this.linkFOVCheck.Name = "linkFOVCheck";
            this.linkFOVCheck.Size = new System.Drawing.Size(46, 17);
            this.linkFOVCheck.TabIndex = 34;
            this.linkFOVCheck.Text = "Link";
            this.linkFOVCheck.UseVisualStyleBackColor = true;
            // 
            // anchorBox
            // 
            this.anchorBox.Controls.Add(this.centreAnchorButton);
            this.anchorBox.Controls.Add(this.topLeftAnchorButton);
            this.anchorBox.Location = new System.Drawing.Point(422, 245);
            this.anchorBox.Name = "anchorBox";
            this.anchorBox.Size = new System.Drawing.Size(139, 47);
            this.anchorBox.TabIndex = 33;
            this.anchorBox.TabStop = false;
            this.anchorBox.Text = "Position Anchor";
            // 
            // centreAnchorButton
            // 
            this.centreAnchorButton.AutoSize = true;
            this.centreAnchorButton.Location = new System.Drawing.Point(79, 20);
            this.centreAnchorButton.Name = "centreAnchorButton";
            this.centreAnchorButton.Size = new System.Drawing.Size(56, 17);
            this.centreAnchorButton.TabIndex = 1;
            this.centreAnchorButton.TabStop = true;
            this.centreAnchorButton.Text = "Centre";
            this.centreAnchorButton.UseVisualStyleBackColor = true;
            this.centreAnchorButton.CheckedChanged += new System.EventHandler(this.AnchorButton_CheckedChanged);
            // 
            // topLeftAnchorButton
            // 
            this.topLeftAnchorButton.AutoSize = true;
            this.topLeftAnchorButton.Location = new System.Drawing.Point(7, 20);
            this.topLeftAnchorButton.Name = "topLeftAnchorButton";
            this.topLeftAnchorButton.Size = new System.Drawing.Size(65, 17);
            this.topLeftAnchorButton.TabIndex = 0;
            this.topLeftAnchorButton.TabStop = true;
            this.topLeftAnchorButton.Text = "Top Left";
            this.topLeftAnchorButton.UseVisualStyleBackColor = true;
            this.topLeftAnchorButton.CheckedChanged += new System.EventHandler(this.AnchorButton_CheckedChanged);
            // 
            // widthPanel
            // 
            this.widthPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.widthPanel.Location = new System.Drawing.Point(78, 111);
            this.widthPanel.Max = 500F;
            this.widthPanel.Min = 0F;
            this.widthPanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.widthPanel.Name = "widthPanel";
            this.widthPanel.Size = new System.Drawing.Size(484, 20);
            this.widthPanel.TabIndex = 2;
            this.widthPanel.Value = 0F;
            this.widthPanel.ValueChanged += new System.Action<float>(this.widthPanel_Changed);
            // 
            // projectionGroup
            // 
            this.projectionGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.projectionGroup.Controls.Add(this.calculatedProjectionButton);
            this.projectionGroup.Controls.Add(this.skewedProjectionButton);
            this.projectionGroup.Controls.Add(this.simpleProjectionButton);
            this.projectionGroup.Location = new System.Drawing.Point(7, 245);
            this.projectionGroup.Name = "projectionGroup";
            this.projectionGroup.Size = new System.Drawing.Size(409, 47);
            this.projectionGroup.TabIndex = 32;
            this.projectionGroup.TabStop = false;
            this.projectionGroup.Text = "Projection";
            // 
            // calculatedProjectionButton
            // 
            this.calculatedProjectionButton.AutoSize = true;
            this.calculatedProjectionButton.Location = new System.Drawing.Point(142, 20);
            this.calculatedProjectionButton.Name = "calculatedProjectionButton";
            this.calculatedProjectionButton.Size = new System.Drawing.Size(75, 17);
            this.calculatedProjectionButton.TabIndex = 2;
            this.calculatedProjectionButton.TabStop = true;
            this.calculatedProjectionButton.Text = "Calculated";
            this.calculatedProjectionButton.UseVisualStyleBackColor = true;
            this.calculatedProjectionButton.CheckedChanged += new System.EventHandler(this.ProjectionButton_CheckedChanged);
            // 
            // skewedProjectionButton
            // 
            this.skewedProjectionButton.AutoSize = true;
            this.skewedProjectionButton.Location = new System.Drawing.Point(71, 20);
            this.skewedProjectionButton.Name = "skewedProjectionButton";
            this.skewedProjectionButton.Size = new System.Drawing.Size(64, 17);
            this.skewedProjectionButton.TabIndex = 1;
            this.skewedProjectionButton.TabStop = true;
            this.skewedProjectionButton.Text = "Skewed";
            this.skewedProjectionButton.UseVisualStyleBackColor = true;
            this.skewedProjectionButton.CheckedChanged += new System.EventHandler(this.ProjectionButton_CheckedChanged);
            // 
            // simpleProjectionButton
            // 
            this.simpleProjectionButton.AutoSize = true;
            this.simpleProjectionButton.Location = new System.Drawing.Point(7, 20);
            this.simpleProjectionButton.Name = "simpleProjectionButton";
            this.simpleProjectionButton.Size = new System.Drawing.Size(56, 17);
            this.simpleProjectionButton.TabIndex = 0;
            this.simpleProjectionButton.TabStop = true;
            this.simpleProjectionButton.Text = "Simple";
            this.simpleProjectionButton.UseVisualStyleBackColor = true;
            this.simpleProjectionButton.CheckedChanged += new System.EventHandler(this.ProjectionButton_CheckedChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(5, 9);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(42, 13);
            this.label11.TabIndex = 31;
            this.label11.Text = "Monitor";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 113);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Width (cm)";
            // 
            // monitorPulldown
            // 
            this.monitorPulldown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.monitorPulldown.DisplayMember = "DeviceName";
            this.monitorPulldown.FormattingEnabled = true;
            this.monitorPulldown.Location = new System.Drawing.Point(78, 6);
            this.monitorPulldown.Name = "monitorPulldown";
            this.monitorPulldown.Size = new System.Drawing.Size(484, 21);
            this.monitorPulldown.TabIndex = 6;
            this.monitorPulldown.SelectedIndexChanged += new System.EventHandler(this.monitorPulldown_SelectedIndexChanged);
            // 
            // heightPanel
            // 
            this.heightPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.heightPanel.Location = new System.Drawing.Point(78, 137);
            this.heightPanel.Max = 500F;
            this.heightPanel.Min = 0F;
            this.heightPanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.heightPanel.Name = "heightPanel";
            this.heightPanel.Size = new System.Drawing.Size(484, 20);
            this.heightPanel.TabIndex = 3;
            this.heightPanel.Value = 0F;
            this.heightPanel.ValueChanged += new System.Action<float>(this.heightPanel_Changed);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(4, 36);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(69, 13);
            this.label10.TabIndex = 30;
            this.label10.Text = "Distance(cm)";
            // 
            // fovVPanel
            // 
            this.fovVPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fovVPanel.Location = new System.Drawing.Point(78, 218);
            this.fovVPanel.Max = 179.9F;
            this.fovVPanel.Min = 0.1F;
            this.fovVPanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.fovVPanel.Name = "fovVPanel";
            this.fovVPanel.Size = new System.Drawing.Size(483, 20);
            this.fovVPanel.TabIndex = 8;
            this.fovVPanel.Value = 90F;
            this.fovVPanel.ValueChanged += new System.Action<float>(this.fovVPanel_ValueChanged);
            // 
            // distancePanel
            // 
            this.distancePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.distancePanel.Location = new System.Drawing.Point(78, 33);
            this.distancePanel.Max = 10000F;
            this.distancePanel.Min = 0F;
            this.distancePanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.distancePanel.Name = "distancePanel";
            this.distancePanel.Size = new System.Drawing.Size(484, 20);
            this.distancePanel.TabIndex = 29;
            this.distancePanel.Value = 0F;
            this.distancePanel.ValueChanged += new System.Action<float>(this.distancePanel_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(4, 165);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 13);
            this.label6.TabIndex = 20;
            this.label6.Text = "Aspect(W:H)";
            // 
            // skewHPanel
            // 
            this.skewHPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.skewHPanel.Location = new System.Drawing.Point(78, 59);
            this.skewHPanel.Max = 10000F;
            this.skewHPanel.Min = -10000F;
            this.skewHPanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.skewHPanel.Name = "skewHPanel";
            this.skewHPanel.Size = new System.Drawing.Size(484, 20);
            this.skewHPanel.TabIndex = 25;
            this.skewHPanel.Value = 0F;
            this.skewHPanel.ValueChanged += new System.Action<float>(this.skewHPanel_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 140);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Height (cm)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 88);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 13);
            this.label5.TabIndex = 28;
            this.label5.Text = "Skew V(cm)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 218);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "FoV - V (deg)";
            // 
            // skewVPanel
            // 
            this.skewVPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.skewVPanel.Location = new System.Drawing.Point(78, 85);
            this.skewVPanel.Max = 10000F;
            this.skewVPanel.Min = -10000F;
            this.skewVPanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.skewVPanel.Name = "skewVPanel";
            this.skewVPanel.Size = new System.Drawing.Size(484, 20);
            this.skewVPanel.TabIndex = 26;
            this.skewVPanel.Value = 0F;
            this.skewVPanel.ValueChanged += new System.Action<float>(this.skewVPanel_ValueChanged);
            // 
            // aspectRatioWValue
            // 
            this.aspectRatioWValue.Location = new System.Drawing.Point(78, 163);
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
            this.aspectRatioWValue.Size = new System.Drawing.Size(51, 20);
            this.aspectRatioWValue.TabIndex = 19;
            this.aspectRatioWValue.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.aspectRatioWValue.ValueChanged += new System.EventHandler(this.aspectRatioWValue_ValueChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(4, 61);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 13);
            this.label9.TabIndex = 27;
            this.label9.Text = "Skew H(cm)";
            // 
            // aspectRatioValue
            // 
            this.aspectRatioValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.aspectRatioValue.DecimalPlaces = 4;
            this.aspectRatioValue.Location = new System.Drawing.Point(196, 163);
            this.aspectRatioValue.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.aspectRatioValue.Name = "aspectRatioValue";
            this.aspectRatioValue.Size = new System.Drawing.Size(366, 20);
            this.aspectRatioValue.TabIndex = 22;
            this.aspectRatioValue.ValueChanged += new System.EventHandler(this.aspectRatioValue_ValueChanged);
            // 
            // fovHPanel
            // 
            this.fovHPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fovHPanel.Location = new System.Drawing.Point(78, 189);
            this.fovHPanel.Max = 179.9F;
            this.fovHPanel.Min = 0.1F;
            this.fovHPanel.MinimumSize = new System.Drawing.Size(95, 20);
            this.fovHPanel.Name = "fovHPanel";
            this.fovHPanel.Size = new System.Drawing.Size(483, 20);
            this.fovHPanel.TabIndex = 1;
            this.fovHPanel.Value = 90F;
            this.fovHPanel.ValueChanged += new System.Action<float>(this.fovHPanel_ValueChanged);
            // 
            // aspectRatioHValue
            // 
            this.aspectRatioHValue.Location = new System.Drawing.Point(135, 163);
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
            this.aspectRatioHValue.Size = new System.Drawing.Size(52, 20);
            this.aspectRatioHValue.TabIndex = 21;
            this.aspectRatioHValue.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.aspectRatioHValue.ValueChanged += new System.EventHandler(this.aspectRatioHValue_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 191);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "FoV - H (deg)";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(180, 166);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(22, 13);
            this.label7.TabIndex = 23;
            this.label7.Text = " = ";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(123, 165);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(19, 13);
            this.label8.TabIndex = 24;
            this.label8.Text = " : ";
            // 
            // orientationPanel
            // 
            this.orientationPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.orientationPanel.Location = new System.Drawing.Point(6, 203);
            this.orientationPanel.LookAtVector = ((OpenMetaverse.Vector3)(resources.GetObject("orientationPanel.LookAtVector")));
            this.orientationPanel.MinimumSize = new System.Drawing.Size(252, 95);
            this.orientationPanel.Name = "orientationPanel";
            this.orientationPanel.Pitch = 0D;
            this.orientationPanel.Quaternion = ((OpenMetaverse.Quaternion)(resources.GetObject("orientationPanel.Quaternion")));
            this.orientationPanel.Size = new System.Drawing.Size(252, 95);
            this.orientationPanel.TabIndex = 1;
            rotation2.LookAtVector = ((OpenMetaverse.Vector3)(resources.GetObject("rotation2.LookAtVector")));
            rotation2.Pitch = 0D;
            rotation2.Quaternion = ((OpenMetaverse.Quaternion)(resources.GetObject("rotation2.Quaternion")));
            rotation2.Yaw = 0D;
            this.orientationPanel.Value = rotation2;
            this.orientationPanel.Yaw = 0D;
            // 
            // centrePanel
            // 
            this.centrePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.centrePanel.Location = new System.Drawing.Point(6, 3);
            this.centrePanel.Max = 1024F;
            this.centrePanel.MaxV = ((OpenMetaverse.Vector3)(resources.GetObject("centrePanel.MaxV")));
            this.centrePanel.Min = -1024F;
            this.centrePanel.MinimumSize = new System.Drawing.Size(103, 95);
            this.centrePanel.MinV = ((OpenMetaverse.Vector3)(resources.GetObject("centrePanel.MinV")));
            this.centrePanel.Name = "centrePanel";
            this.centrePanel.Size = new System.Drawing.Size(249, 95);
            this.centrePanel.TabIndex = 1;
            this.centrePanel.Value = ((OpenMetaverse.Vector3)(resources.GetObject("centrePanel.Value")));
            this.centrePanel.X = 0F;
            this.centrePanel.Y = 0F;
            this.centrePanel.Z = 0F;
            this.centrePanel.ValueChanged += new System.EventHandler(this.centrePanel_ValueChanged);
            // 
            // topLeftPanel
            // 
            this.topLeftPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.topLeftPanel.Location = new System.Drawing.Point(6, 102);
            this.topLeftPanel.Max = 1024F;
            this.topLeftPanel.MaxV = ((OpenMetaverse.Vector3)(resources.GetObject("topLeftPanel.MaxV")));
            this.topLeftPanel.Min = -1024F;
            this.topLeftPanel.MinimumSize = new System.Drawing.Size(103, 95);
            this.topLeftPanel.MinV = ((OpenMetaverse.Vector3)(resources.GetObject("topLeftPanel.MinV")));
            this.topLeftPanel.Name = "topLeftPanel";
            this.topLeftPanel.Size = new System.Drawing.Size(249, 95);
            this.topLeftPanel.TabIndex = 0;
            this.topLeftPanel.Value = ((OpenMetaverse.Vector3)(resources.GetObject("topLeftPanel.Value")));
            this.topLeftPanel.X = 0F;
            this.topLeftPanel.Y = 0F;
            this.topLeftPanel.Z = 0F;
            this.topLeftPanel.ValueChanged += new System.EventHandler(this.topLeftPanel_ValueChanged);
            // 
            // overlayTab
            // 
            this.overlayTab.Controls.Add(this.controlCursor);
            this.overlayTab.Controls.Add(this.bringToFrontButtin);
            this.overlayTab.Controls.Add(this.fullscreenCheck);
            this.overlayTab.Controls.Add(this.launchOverlayButton);
            this.overlayTab.Location = new System.Drawing.Point(4, 22);
            this.overlayTab.Name = "overlayTab";
            this.overlayTab.Padding = new System.Windows.Forms.Padding(3);
            this.overlayTab.Size = new System.Drawing.Size(833, 599);
            this.overlayTab.TabIndex = 1;
            this.overlayTab.Text = "Overlay";
            this.overlayTab.UseVisualStyleBackColor = true;
            // 
            // controlCursor
            // 
            this.controlCursor.AutoSize = true;
            this.controlCursor.Location = new System.Drawing.Point(7, 88);
            this.controlCursor.Name = "controlCursor";
            this.controlCursor.Size = new System.Drawing.Size(92, 17);
            this.controlCursor.TabIndex = 4;
            this.controlCursor.Text = "Control Cursor";
            this.controlCursor.UseVisualStyleBackColor = true;
            this.controlCursor.CheckedChanged += new System.EventHandler(this.controlCursor_CheckedChanged);
            // 
            // bringToFrontButtin
            // 
            this.bringToFrontButtin.Location = new System.Drawing.Point(6, 35);
            this.bringToFrontButtin.Name = "bringToFrontButtin";
            this.bringToFrontButtin.Size = new System.Drawing.Size(121, 23);
            this.bringToFrontButtin.TabIndex = 3;
            this.bringToFrontButtin.Text = "Bring To Front";
            this.bringToFrontButtin.UseVisualStyleBackColor = true;
            this.bringToFrontButtin.Click += new System.EventHandler(this.bringToFrontButtin_Click);
            // 
            // fullscreenCheck
            // 
            this.fullscreenCheck.AutoSize = true;
            this.fullscreenCheck.Location = new System.Drawing.Point(6, 64);
            this.fullscreenCheck.Name = "fullscreenCheck";
            this.fullscreenCheck.Size = new System.Drawing.Size(74, 17);
            this.fullscreenCheck.TabIndex = 2;
            this.fullscreenCheck.Text = "Fullscreen";
            this.fullscreenCheck.UseVisualStyleBackColor = true;
            this.fullscreenCheck.CheckedChanged += new System.EventHandler(this.showBordersTextBox_CheckedChanged);
            // 
            // launchOverlayButton
            // 
            this.launchOverlayButton.Location = new System.Drawing.Point(6, 6);
            this.launchOverlayButton.Name = "launchOverlayButton";
            this.launchOverlayButton.Size = new System.Drawing.Size(121, 23);
            this.launchOverlayButton.TabIndex = 1;
            this.launchOverlayButton.Text = "Launch Overlay";
            this.launchOverlayButton.UseVisualStyleBackColor = true;
            this.launchOverlayButton.Click += new System.EventHandler(this.launchOverlayButton_Click);
            // 
            // WindowPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainTab);
            this.Name = "WindowPanel";
            this.Size = new System.Drawing.Size(841, 625);
            this.mainTab.ResumeLayout(false);
            this.configTab.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.anchorBox.ResumeLayout(false);
            this.anchorBox.PerformLayout();
            this.projectionGroup.ResumeLayout(false);
            this.projectionGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.aspectRatioWValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.aspectRatioValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.aspectRatioHValue)).EndInit();
            this.overlayTab.ResumeLayout(false);
            this.overlayTab.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl mainTab;
        private System.Windows.Forms.TabPage configTab;
        private System.Windows.Forms.TabPage overlayTab;
        private System.Windows.Forms.Button launchOverlayButton;
        private System.Windows.Forms.CheckBox fullscreenCheck;
        private System.Windows.Forms.Button bringToFrontButtin;
        private Chimera.GUI.VectorPanel topLeftPanel;
        private Chimera.GUI.RotationPanel orientationPanel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private Chimera.GUI.ScalarPanel heightPanel;
        private Chimera.GUI.ScalarPanel widthPanel;
        private System.Windows.Forms.ComboBox monitorPulldown;
        private System.Windows.Forms.CheckBox controlCursor;
        private System.Windows.Forms.Button restartButton;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private VectorPanel centrePanel;
        private ScalarPanel fovHPanel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private ScalarPanel fovVPanel;
        private System.Windows.Forms.NumericUpDown aspectRatioHValue;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown aspectRatioValue;
        private System.Windows.Forms.NumericUpDown aspectRatioWValue;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private ScalarPanel skewHPanel;
        private System.Windows.Forms.Label label5;
        private ScalarPanel skewVPanel;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private ScalarPanel distancePanel;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.GroupBox projectionGroup;
        private System.Windows.Forms.GroupBox anchorBox;
        private System.Windows.Forms.CheckBox linkFOVCheck;
        private System.Windows.Forms.RadioButton simpleProjectionButton;
        private System.Windows.Forms.RadioButton skewedProjectionButton;
        private System.Windows.Forms.RadioButton calculatedProjectionButton;
        private System.Windows.Forms.RadioButton topLeftAnchorButton;
        private System.Windows.Forms.RadioButton centreAnchorButton;
    }
}

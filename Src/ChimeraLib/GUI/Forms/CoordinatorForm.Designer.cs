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
            this.globalBox = new System.Windows.Forms.GroupBox();
            this.triggerHelpButton = new System.Windows.Forms.Button();
            this.eyePositionPanel = new Chimera.GUI.VectorPanel();
            this.virtualOrientationPanel = new Chimera.GUI.RotationPanel();
            this.virtualPositionPanel = new Chimera.GUI.VectorPanel();
            this.windowsPluginsSplit = new System.Windows.Forms.SplitContainer();
            this.windowsGroup = new System.Windows.Forms.GroupBox();
            this.windowsTab = new System.Windows.Forms.TabControl();
            this.inputsGroup = new System.Windows.Forms.GroupBox();
            this.inputsTab = new System.Windows.Forms.TabControl();
            this.diagSplit = new System.Windows.Forms.SplitContainer();
            this.realSpaceGroup = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.testButton = new System.Windows.Forms.Button();
            this.virtualSpaceGroup = new System.Windows.Forms.GroupBox();
            this.heightmapPanel = new System.Windows.Forms.PictureBox();
            this.realSpacePanel = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.hSplit)).BeginInit();
            this.hSplit.Panel1.SuspendLayout();
            this.hSplit.Panel2.SuspendLayout();
            this.hSplit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.diagramWorldSplit)).BeginInit();
            this.diagramWorldSplit.Panel1.SuspendLayout();
            this.diagramWorldSplit.Panel2.SuspendLayout();
            this.diagramWorldSplit.SuspendLayout();
            this.globalBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.windowsPluginsSplit)).BeginInit();
            this.windowsPluginsSplit.Panel1.SuspendLayout();
            this.windowsPluginsSplit.Panel2.SuspendLayout();
            this.windowsPluginsSplit.SuspendLayout();
            this.windowsGroup.SuspendLayout();
            this.inputsGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.diagSplit)).BeginInit();
            this.diagSplit.Panel1.SuspendLayout();
            this.diagSplit.Panel2.SuspendLayout();
            this.diagSplit.SuspendLayout();
            this.realSpaceGroup.SuspendLayout();
            this.virtualSpaceGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.heightmapPanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.realSpacePanel)).BeginInit();
            this.SuspendLayout();
            // 
            // hSplit
            // 
            this.hSplit.Dock = System.Windows.Forms.DockStyle.Fill;
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
            this.hSplit.SplitterDistance = 320;
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
            this.diagramWorldSplit.Size = new System.Drawing.Size(911, 320);
            this.diagramWorldSplit.SplitterDistance = 646;
            this.diagramWorldSplit.TabIndex = 0;
            // 
            // globalBox
            // 
            this.globalBox.Controls.Add(this.triggerHelpButton);
            this.globalBox.Controls.Add(this.eyePositionPanel);
            this.globalBox.Controls.Add(this.virtualOrientationPanel);
            this.globalBox.Controls.Add(this.virtualPositionPanel);
            this.globalBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.globalBox.Location = new System.Drawing.Point(0, 0);
            this.globalBox.MinimumSize = new System.Drawing.Size(261, 290);
            this.globalBox.Name = "globalBox";
            this.globalBox.Size = new System.Drawing.Size(261, 320);
            this.globalBox.TabIndex = 0;
            this.globalBox.TabStop = false;
            this.globalBox.Text = "Global";
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
            rotation1.Pitch = 0D;
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
            this.windowsPluginsSplit.Size = new System.Drawing.Size(911, 498);
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
            this.windowsGroup.Size = new System.Drawing.Size(445, 498);
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
            this.windowsTab.Size = new System.Drawing.Size(439, 479);
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
            this.inputsGroup.Size = new System.Drawing.Size(462, 498);
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
            this.inputsTab.Size = new System.Drawing.Size(456, 479);
            this.inputsTab.TabIndex = 0;
            this.inputsTab.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CoordinatorForm_KeyDown);
            this.inputsTab.KeyUp += new System.Windows.Forms.KeyEventHandler(this.CoordinatorForm_KeyUp);
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
            this.diagSplit.Size = new System.Drawing.Size(646, 320);
            this.diagSplit.SplitterDistance = 310;
            this.diagSplit.TabIndex = 1;
            // 
            // realSpaceGroup
            // 
            this.realSpaceGroup.Controls.Add(this.button1);
            this.realSpaceGroup.Controls.Add(this.testButton);
            this.realSpaceGroup.Controls.Add(this.realSpacePanel);
            this.realSpaceGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.realSpaceGroup.Location = new System.Drawing.Point(0, 0);
            this.realSpaceGroup.Name = "realSpaceGroup";
            this.realSpaceGroup.Size = new System.Drawing.Size(310, 320);
            this.realSpaceGroup.TabIndex = 0;
            this.realSpaceGroup.TabStop = false;
            this.realSpaceGroup.Text = "Real Space";
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
            // virtualSpaceGroup
            // 
            this.virtualSpaceGroup.Controls.Add(this.heightmapPanel);
            this.virtualSpaceGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.virtualSpaceGroup.Location = new System.Drawing.Point(0, 0);
            this.virtualSpaceGroup.Name = "virtualSpaceGroup";
            this.virtualSpaceGroup.Size = new System.Drawing.Size(332, 320);
            this.virtualSpaceGroup.TabIndex = 0;
            this.virtualSpaceGroup.TabStop = false;
            this.virtualSpaceGroup.Text = "Virtual Space";
            // 
            // heightmapPanel
            // 
            this.heightmapPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.heightmapPanel.Location = new System.Drawing.Point(3, 16);
            this.heightmapPanel.Name = "heightmapPanel";
            this.heightmapPanel.Size = new System.Drawing.Size(326, 301);
            this.heightmapPanel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.heightmapPanel.TabIndex = 1;
            this.heightmapPanel.TabStop = false;
            this.heightmapPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.heightmapPanel_Paint);
            // 
            // realSpacePanel
            // 
            this.realSpacePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.realSpacePanel.Location = new System.Drawing.Point(3, 16);
            this.realSpacePanel.Name = "realSpacePanel";
            this.realSpacePanel.Size = new System.Drawing.Size(304, 301);
            this.realSpacePanel.TabIndex = 3;
            this.realSpacePanel.TabStop = false;
            this.realSpacePanel.Paint += new System.Windows.Forms.PaintEventHandler(this.realSpacePanel_Paint);
            // 
            // CoordinatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(911, 822);
            this.Controls.Add(this.hSplit);
            this.Name = "CoordinatorForm";
            this.Text = "Caen Control Panel";
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
            this.globalBox.ResumeLayout(false);
            this.windowsPluginsSplit.Panel1.ResumeLayout(false);
            this.windowsPluginsSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.windowsPluginsSplit)).EndInit();
            this.windowsPluginsSplit.ResumeLayout(false);
            this.windowsGroup.ResumeLayout(false);
            this.inputsGroup.ResumeLayout(false);
            this.diagSplit.Panel1.ResumeLayout(false);
            this.diagSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.diagSplit)).EndInit();
            this.diagSplit.ResumeLayout(false);
            this.realSpaceGroup.ResumeLayout(false);
            this.virtualSpaceGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.heightmapPanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.realSpacePanel)).EndInit();
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
    }
}
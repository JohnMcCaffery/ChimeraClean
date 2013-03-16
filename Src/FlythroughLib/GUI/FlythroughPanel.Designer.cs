﻿namespace Chimera.FlythroughLib.GUI {
    partial class FlythroughPanel {
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FlythroughPanel));
            Chimera.Util.Rotation rotation5 = new Chimera.Util.Rotation();
            this.saveSequenceDialog = new System.Windows.Forms.SaveFileDialog();
            this.eventsList = new System.Windows.Forms.ListBox();
            this.eventsContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadSequenceDialog = new System.Windows.Forms.OpenFileDialog();
            this.loadButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.loopCheck = new System.Windows.Forms.CheckBox();
            this.playButton = new System.Windows.Forms.Button();
            this.eventPanel = new System.Windows.Forms.Panel();
            this.startPanel = new System.Windows.Forms.Panel();
            this.takeCurrentCameraButton = new System.Windows.Forms.Button();
            this.takeOrientationButton = new System.Windows.Forms.Button();
            this.currentPositionButton = new System.Windows.Forms.Button();
            this.startOrientationPanel = new ProxyTestGUI.RotationPanel();
            this.startPositionPanel = new ProxyTestGUI.VectorPanel();
            this.autoStepCheck = new System.Windows.Forms.CheckBox();
            this.timeSlider = new System.Windows.Forms.TrackBar();
            this.lengthLabel = new System.Windows.Forms.Label();
            this.timeLabel = new System.Windows.Forms.Label();
            this.startButton = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.stepBackButton = new System.Windows.Forms.Button();
            this.stepForwardButton = new System.Windows.Forms.Button();
            this.stepButton = new System.Windows.Forms.Button();
            this.eventsContextMenu.SuspendLayout();
            this.eventPanel.SuspendLayout();
            this.startPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timeSlider)).BeginInit();
            this.SuspendLayout();
            // 
            // eventsList
            // 
            this.eventsList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.eventsList.ContextMenuStrip = this.eventsContextMenu;
            this.eventsList.DisplayMember = "Name";
            this.eventsList.FormattingEnabled = true;
            this.eventsList.Location = new System.Drawing.Point(3, 3);
            this.eventsList.Name = "eventsList";
            this.eventsList.Size = new System.Drawing.Size(120, 316);
            this.eventsList.TabIndex = 9;
            this.eventsList.SelectedValueChanged += new System.EventHandler(this.eventsList_SelectedValueChanged);
            this.eventsList.DoubleClick += new System.EventHandler(this.eventsList_DoubleClick);
            // 
            // eventsContextMenu
            // 
            this.eventsContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem,
            this.removeToolStripMenuItem,
            this.moveUpToolStripMenuItem});
            this.eventsContextMenu.Name = "eventsContextMenu";
            this.eventsContextMenu.Size = new System.Drawing.Size(120, 70);
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.addToolStripMenuItem.Text = "Add";
            this.addToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.removeToolStripMenuItem.Text = "Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // moveUpToolStripMenuItem
            // 
            this.moveUpToolStripMenuItem.Name = "moveUpToolStripMenuItem";
            this.moveUpToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.moveUpToolStripMenuItem.Text = "MoveUp";
            this.moveUpToolStripMenuItem.Click += new System.EventHandler(this.moveUpToolStripMenuItem_Click);
            // 
            // loadSequenceDialog
            // 
            this.loadSequenceDialog.FileName = "loadSequenceDialog";
            // 
            // loadButton
            // 
            this.loadButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.loadButton.Location = new System.Drawing.Point(213, 322);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(40, 23);
            this.loadButton.TabIndex = 10;
            this.loadButton.Text = "Load";
            this.loadButton.UseVisualStyleBackColor = true;
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.Location = new System.Drawing.Point(259, 322);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(40, 23);
            this.saveButton.TabIndex = 11;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // loopCheck
            // 
            this.loopCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.loopCheck.AutoSize = true;
            this.loopCheck.Location = new System.Drawing.Point(478, 326);
            this.loopCheck.Name = "loopCheck";
            this.loopCheck.Size = new System.Drawing.Size(50, 17);
            this.loopCheck.TabIndex = 12;
            this.loopCheck.Text = "Loop";
            this.loopCheck.UseVisualStyleBackColor = true;
            this.loopCheck.CheckedChanged += new System.EventHandler(this.loopCheck_CheckedChanged);
            // 
            // playButton
            // 
            this.playButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.playButton.Location = new System.Drawing.Point(305, 322);
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(45, 23);
            this.playButton.TabIndex = 13;
            this.playButton.Text = "Play";
            this.playButton.UseVisualStyleBackColor = true;
            this.playButton.Click += new System.EventHandler(this.playButton_Click);
            // 
            // eventPanel
            // 
            this.eventPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.eventPanel.Controls.Add(this.startPanel);
            this.eventPanel.Location = new System.Drawing.Point(129, 0);
            this.eventPanel.Name = "eventPanel";
            this.eventPanel.Size = new System.Drawing.Size(402, 287);
            this.eventPanel.TabIndex = 14;
            // 
            // startPanel
            // 
            this.startPanel.Controls.Add(this.takeCurrentCameraButton);
            this.startPanel.Controls.Add(this.takeOrientationButton);
            this.startPanel.Controls.Add(this.currentPositionButton);
            this.startPanel.Controls.Add(this.startOrientationPanel);
            this.startPanel.Controls.Add(this.startPositionPanel);
            this.startPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.startPanel.Location = new System.Drawing.Point(0, 0);
            this.startPanel.Name = "startPanel";
            this.startPanel.Size = new System.Drawing.Size(402, 287);
            this.startPanel.TabIndex = 0;
            // 
            // takeCurrentCameraButton
            // 
            this.takeCurrentCameraButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.takeCurrentCameraButton.Location = new System.Drawing.Point(0, 263);
            this.takeCurrentCameraButton.Name = "takeCurrentCameraButton";
            this.takeCurrentCameraButton.Size = new System.Drawing.Size(402, 23);
            this.takeCurrentCameraButton.TabIndex = 4;
            this.takeCurrentCameraButton.Text = "Take Current Camera";
            this.takeCurrentCameraButton.UseVisualStyleBackColor = true;
            this.takeCurrentCameraButton.Click += new System.EventHandler(this.takeCurrentCameraButton_Click);
            // 
            // takeOrientationButton
            // 
            this.takeOrientationButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.takeOrientationButton.Location = new System.Drawing.Point(0, 234);
            this.takeOrientationButton.Name = "takeOrientationButton";
            this.takeOrientationButton.Size = new System.Drawing.Size(402, 23);
            this.takeOrientationButton.TabIndex = 3;
            this.takeOrientationButton.Text = "Take Current Orientation";
            this.takeOrientationButton.UseVisualStyleBackColor = true;
            this.takeOrientationButton.Click += new System.EventHandler(this.takeOrientationButton_Click);
            // 
            // currentPositionButton
            // 
            this.currentPositionButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.currentPositionButton.Location = new System.Drawing.Point(0, 104);
            this.currentPositionButton.Name = "currentPositionButton";
            this.currentPositionButton.Size = new System.Drawing.Size(402, 23);
            this.currentPositionButton.TabIndex = 2;
            this.currentPositionButton.Text = "Take Current Position";
            this.currentPositionButton.UseVisualStyleBackColor = true;
            this.currentPositionButton.Click += new System.EventHandler(this.currentPositionButton_Click);
            // 
            // startOrientationPanel
            // 
            this.startOrientationPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.startOrientationPanel.Text = "Start Orientation";
            this.startOrientationPanel.Location = new System.Drawing.Point(0, 133);
            this.startOrientationPanel.LookAtVector = ((OpenMetaverse.Vector3)(resources.GetObject("startOrientationPanel.LookAtVector")));
            this.startOrientationPanel.MinimumSize = new System.Drawing.Size(252, 95);
            this.startOrientationPanel.Name = "startOrientationPanel";
            this.startOrientationPanel.Pitch = 0D;
            this.startOrientationPanel.Quaternion = ((OpenMetaverse.Quaternion)(resources.GetObject("startOrientationPanel.Quaternion")));
            this.startOrientationPanel.Size = new System.Drawing.Size(402, 95);
            this.startOrientationPanel.TabIndex = 1;
            rotation5.LookAtVector = ((OpenMetaverse.Vector3)(resources.GetObject("rotation5.LookAtVector")));
            rotation5.Pitch = 0D;
            rotation5.Quaternion = ((OpenMetaverse.Quaternion)(resources.GetObject("rotation5.Quaternion")));
            rotation5.Yaw = 0D;
            this.startOrientationPanel.Value = rotation5;
            this.startOrientationPanel.Yaw = 0D;
            this.startOrientationPanel.OnChange += new System.EventHandler(this.startOrientationPanel_OnChange);
            // 
            // startPositionPanel
            // 
            this.startPositionPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.startPositionPanel.Text = "Start Position";
            this.startPositionPanel.Location = new System.Drawing.Point(0, 3);
            this.startPositionPanel.Max = 1024D;
            this.startPositionPanel.MaxV = ((OpenMetaverse.Vector3)(resources.GetObject("startPositionPanel.MaxV")));
            this.startPositionPanel.Min = -1024D;
            this.startPositionPanel.MinimumSize = new System.Drawing.Size(103, 95);
            this.startPositionPanel.MinV = ((OpenMetaverse.Vector3)(resources.GetObject("startPositionPanel.MinV")));
            this.startPositionPanel.Name = "startPositionPanel";
            this.startPositionPanel.Size = new System.Drawing.Size(399, 95);
            this.startPositionPanel.TabIndex = 0;
            this.startPositionPanel.Value = ((OpenMetaverse.Vector3)(resources.GetObject("startPositionPanel.Value")));
            this.startPositionPanel.X = 0F;
            this.startPositionPanel.Y = 0F;
            this.startPositionPanel.Z = 0F;
            this.startPositionPanel.ValueChanged += new System.EventHandler(this.startPositionPanel_OnChange);
            // 
            // autoStepCheck
            // 
            this.autoStepCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.autoStepCheck.AutoSize = true;
            this.autoStepCheck.Location = new System.Drawing.Point(399, 326);
            this.autoStepCheck.Name = "autoStepCheck";
            this.autoStepCheck.Size = new System.Drawing.Size(73, 17);
            this.autoStepCheck.TabIndex = 15;
            this.autoStepCheck.Text = "Auto Step";
            this.autoStepCheck.UseVisualStyleBackColor = true;
            this.autoStepCheck.CheckedChanged += new System.EventHandler(this.autoStepBox_CheckedChanged);
            // 
            // timeSlider
            // 
            this.timeSlider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.timeSlider.Location = new System.Drawing.Point(138, 293);
            this.timeSlider.Maximum = 0;
            this.timeSlider.Name = "timeSlider";
            this.timeSlider.Size = new System.Drawing.Size(380, 45);
            this.timeSlider.TabIndex = 0;
            this.timeSlider.TickStyle = System.Windows.Forms.TickStyle.None;
            this.timeSlider.Scroll += new System.EventHandler(this.timeSlider_Scroll);
            // 
            // lengthLabel
            // 
            this.lengthLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lengthLabel.AutoSize = true;
            this.lengthLabel.Location = new System.Drawing.Point(3, 327);
            this.lengthLabel.Name = "lengthLabel";
            this.lengthLabel.Size = new System.Drawing.Size(40, 13);
            this.lengthLabel.TabIndex = 0;
            this.lengthLabel.Text = "Length";
            // 
            // timeLabel
            // 
            this.timeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.timeLabel.AutoSize = true;
            this.timeLabel.Location = new System.Drawing.Point(87, 326);
            this.timeLabel.Name = "timeLabel";
            this.timeLabel.Size = new System.Drawing.Size(30, 13);
            this.timeLabel.TabIndex = 16;
            this.timeLabel.Text = "Time";
            // 
            // startButton
            // 
            this.startButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.startButton.Location = new System.Drawing.Point(170, 322);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(37, 23);
            this.startButton.TabIndex = 17;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "loadSequenceDialog";
            // 
            // stepBackButton
            // 
            this.stepBackButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.stepBackButton.Location = new System.Drawing.Point(129, 293);
            this.stepBackButton.Name = "stepBackButton";
            this.stepBackButton.Size = new System.Drawing.Size(14, 23);
            this.stepBackButton.TabIndex = 2;
            this.stepBackButton.Text = "<";
            this.stepBackButton.UseVisualStyleBackColor = true;
            this.stepBackButton.Click += new System.EventHandler(this.stepBackButton_Click);
            // 
            // stepForwardButton
            // 
            this.stepForwardButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.stepForwardButton.Location = new System.Drawing.Point(514, 293);
            this.stepForwardButton.Name = "stepForwardButton";
            this.stepForwardButton.Size = new System.Drawing.Size(14, 23);
            this.stepForwardButton.TabIndex = 18;
            this.stepForwardButton.Text = ">";
            this.stepForwardButton.UseVisualStyleBackColor = true;
            this.stepForwardButton.Click += new System.EventHandler(this.stepForwardButton_Click);
            // 
            // stepButton
            // 
            this.stepButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.stepButton.Location = new System.Drawing.Point(356, 322);
            this.stepButton.Name = "stepButton";
            this.stepButton.Size = new System.Drawing.Size(37, 23);
            this.stepButton.TabIndex = 19;
            this.stepButton.Text = "Step";
            this.stepButton.UseVisualStyleBackColor = true;
            this.stepButton.Click += new System.EventHandler(this.stepButton_Click);
            // 
            // FlythroughPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.loadButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.stepButton);
            this.Controls.Add(this.stepForwardButton);
            this.Controls.Add(this.stepBackButton);
            this.Controls.Add(this.timeLabel);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.lengthLabel);
            this.Controls.Add(this.autoStepCheck);
            this.Controls.Add(this.eventPanel);
            this.Controls.Add(this.eventsList);
            this.Controls.Add(this.loopCheck);
            this.Controls.Add(this.playButton);
            this.Controls.Add(this.timeSlider);
            this.Name = "FlythroughPanel";
            this.Size = new System.Drawing.Size(531, 348);
            this.eventsContextMenu.ResumeLayout(false);
            this.eventPanel.ResumeLayout(false);
            this.startPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.timeSlider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SaveFileDialog saveSequenceDialog;
        private System.Windows.Forms.ListBox eventsList;
        private System.Windows.Forms.ContextMenuStrip eventsContextMenu;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog loadSequenceDialog;
        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.CheckBox loopCheck;
        private System.Windows.Forms.Button playButton;
        private System.Windows.Forms.Panel eventPanel;
        private System.Windows.Forms.CheckBox autoStepCheck;
        private System.Windows.Forms.TrackBar timeSlider;
        private System.Windows.Forms.Label lengthLabel;
        private System.Windows.Forms.Label timeLabel;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveUpToolStripMenuItem;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Panel startPanel;
        private ProxyTestGUI.VectorPanel startPositionPanel;
        private ProxyTestGUI.RotationPanel startOrientationPanel;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button stepBackButton;
        private System.Windows.Forms.Button stepForwardButton;
        private System.Windows.Forms.Button currentPositionButton;
        private System.Windows.Forms.Button takeOrientationButton;
        private System.Windows.Forms.Button takeCurrentCameraButton;
        private System.Windows.Forms.Button stepButton;
    }
}

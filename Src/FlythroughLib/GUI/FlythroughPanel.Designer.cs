namespace Chimera.FlythroughLib.GUI {
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
            this.saveSequenceDialog = new System.Windows.Forms.SaveFileDialog();
            this.eventsList = new System.Windows.Forms.ListBox();
            this.eventsContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadSequenceDialog = new System.Windows.Forms.OpenFileDialog();
            this.loadButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.loopCheck = new System.Windows.Forms.CheckBox();
            this.playButton = new System.Windows.Forms.Button();
            this.eventPanel = new System.Windows.Forms.Panel();
            this.autoStepBox = new System.Windows.Forms.CheckBox();
            this.timeSlider = new System.Windows.Forms.TrackBar();
            this.lengthLabel = new System.Windows.Forms.Label();
            this.timeLabel = new System.Windows.Forms.Label();
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eventsContextMenu.SuspendLayout();
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
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.removeToolStripMenuItem.Text = "Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // loadSequenceDialog
            // 
            this.loadSequenceDialog.FileName = "loadSequenceDialog";
            // 
            // loadButton
            // 
            this.loadButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.loadButton.Location = new System.Drawing.Point(129, 322);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(40, 23);
            this.loadButton.TabIndex = 10;
            this.loadButton.Text = "Load";
            this.loadButton.UseVisualStyleBackColor = true;
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.Location = new System.Drawing.Point(175, 322);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(43, 23);
            this.saveButton.TabIndex = 11;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            // 
            // loopCheck
            // 
            this.loopCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.loopCheck.AutoSize = true;
            this.loopCheck.Location = new System.Drawing.Point(365, 326);
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
            this.playButton.Location = new System.Drawing.Point(224, 322);
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(56, 23);
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
            this.eventPanel.Location = new System.Drawing.Point(129, 0);
            this.eventPanel.Name = "eventPanel";
            this.eventPanel.Size = new System.Drawing.Size(289, 287);
            this.eventPanel.TabIndex = 14;
            // 
            // autoStepBox
            // 
            this.autoStepBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.autoStepBox.AutoSize = true;
            this.autoStepBox.Location = new System.Drawing.Point(286, 326);
            this.autoStepBox.Name = "autoStepBox";
            this.autoStepBox.Size = new System.Drawing.Size(73, 17);
            this.autoStepBox.TabIndex = 15;
            this.autoStepBox.Text = "Auto Step";
            this.autoStepBox.UseVisualStyleBackColor = true;
            this.autoStepBox.CheckedChanged += new System.EventHandler(this.autoStepBox_CheckedChanged);
            // 
            // timeSlider
            // 
            this.timeSlider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.timeSlider.Location = new System.Drawing.Point(123, 293);
            this.timeSlider.Maximum = 0;
            this.timeSlider.Name = "timeSlider";
            this.timeSlider.Size = new System.Drawing.Size(295, 45);
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
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.addToolStripMenuItem.Text = "Add";
            this.addToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
            // 
            // moveUpToolStripMenuItem
            // 
            this.moveUpToolStripMenuItem.Name = "moveUpToolStripMenuItem";
            this.moveUpToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.moveUpToolStripMenuItem.Text = "MoveUp";
            this.moveUpToolStripMenuItem.Click += new System.EventHandler(this.moveUpToolStripMenuItem_Click);
            // 
            // FlythroughPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.timeLabel);
            this.Controls.Add(this.lengthLabel);
            this.Controls.Add(this.autoStepBox);
            this.Controls.Add(this.eventPanel);
            this.Controls.Add(this.eventsList);
            this.Controls.Add(this.playButton);
            this.Controls.Add(this.loopCheck);
            this.Controls.Add(this.loadButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.timeSlider);
            this.Name = "FlythroughPanel";
            this.Size = new System.Drawing.Size(418, 348);
            this.eventsContextMenu.ResumeLayout(false);
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
        private System.Windows.Forms.CheckBox autoStepBox;
        private System.Windows.Forms.TrackBar timeSlider;
        private System.Windows.Forms.Label lengthLabel;
        private System.Windows.Forms.Label timeLabel;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveUpToolStripMenuItem;
    }
}

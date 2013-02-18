namespace FlythroughLib.Panels {
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
            this.playButton = new System.Windows.Forms.Button();
            this.saveSequenceDialog = new System.Windows.Forms.SaveFileDialog();
            this.eventsList = new System.Windows.Forms.ListBox();
            this.eventsContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveToEventToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rotateToEventToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lookAtEventToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.comboEventToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.blankEventToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadSequenceDialog = new System.Windows.Forms.OpenFileDialog();
            this.loadButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.loopCheck = new System.Windows.Forms.CheckBox();
            this.pauseButton = new System.Windows.Forms.Button();
            this.resetButton = new System.Windows.Forms.Button();
            this.resetCurrentButton = new System.Windows.Forms.Button();
            this.eventsContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // playButton
            // 
            this.playButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.playButton.Location = new System.Drawing.Point(293, 322);
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(156, 23);
            this.playButton.TabIndex = 8;
            this.playButton.Text = "Play";
            this.playButton.UseVisualStyleBackColor = true;
            this.playButton.Click += new System.EventHandler(this.playButton_Click);
            // 
            // eventsList
            // 
            this.eventsList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.eventsList.ContextMenuStrip = this.eventsContextMenu;
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
            this.removeToolStripMenuItem});
            this.eventsContextMenu.Name = "eventsContextMenu";
            this.eventsContextMenu.Size = new System.Drawing.Size(118, 48);
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.moveToEventToolStripMenuItem,
            this.rotateToEventToolStripMenuItem,
            this.lookAtEventToolStripMenuItem,
            this.comboEventToolStripMenuItem,
            this.blankEventToolStripMenuItem});
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.addToolStripMenuItem.Text = "Add";
            // 
            // moveToEventToolStripMenuItem
            // 
            this.moveToEventToolStripMenuItem.Name = "moveToEventToolStripMenuItem";
            this.moveToEventToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.moveToEventToolStripMenuItem.Text = "Move To Event";
            this.moveToEventToolStripMenuItem.Click += new System.EventHandler(this.moveToEventToolStripMenuItem_Click);
            // 
            // rotateToEventToolStripMenuItem
            // 
            this.rotateToEventToolStripMenuItem.Name = "rotateToEventToolStripMenuItem";
            this.rotateToEventToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.rotateToEventToolStripMenuItem.Text = "Rotate To Event";
            this.rotateToEventToolStripMenuItem.Click += new System.EventHandler(this.rotateToEventToolStripMenuItem_Click);
            // 
            // lookAtEventToolStripMenuItem
            // 
            this.lookAtEventToolStripMenuItem.Name = "lookAtEventToolStripMenuItem";
            this.lookAtEventToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.lookAtEventToolStripMenuItem.Text = "Look At Event";
            this.lookAtEventToolStripMenuItem.Click += new System.EventHandler(this.lookAtEventToolStripMenuItem_Click);
            // 
            // comboEventToolStripMenuItem
            // 
            this.comboEventToolStripMenuItem.Name = "comboEventToolStripMenuItem";
            this.comboEventToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.comboEventToolStripMenuItem.Text = "Combo Event";
            this.comboEventToolStripMenuItem.Click += new System.EventHandler(this.comboEventToolStripMenuItem_Click);
            // 
            // blankEventToolStripMenuItem
            // 
            this.blankEventToolStripMenuItem.Name = "blankEventToolStripMenuItem";
            this.blankEventToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.blankEventToolStripMenuItem.Text = "Blank Event";
            this.blankEventToolStripMenuItem.Click += new System.EventHandler(this.blankEventToolStripMenuItem_Click);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.removeToolStripMenuItem.Text = "Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // loadSequenceDialog
            // 
            this.loadSequenceDialog.FileName = "loadSequenceDialog";
            // 
            // loadButton
            // 
            this.loadButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.loadButton.Location = new System.Drawing.Point(3, 322);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(40, 23);
            this.loadButton.TabIndex = 10;
            this.loadButton.Text = "Load";
            this.loadButton.UseVisualStyleBackColor = true;
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.saveButton.Location = new System.Drawing.Point(49, 322);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(43, 23);
            this.saveButton.TabIndex = 11;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // loopCheck
            // 
            this.loopCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.loopCheck.AutoSize = true;
            this.loopCheck.Location = new System.Drawing.Point(455, 326);
            this.loopCheck.Name = "loopCheck";
            this.loopCheck.Size = new System.Drawing.Size(50, 17);
            this.loopCheck.TabIndex = 12;
            this.loopCheck.Text = "Loop";
            this.loopCheck.UseVisualStyleBackColor = true;
            // 
            // pauseButton
            // 
            this.pauseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pauseButton.Location = new System.Drawing.Point(98, 322);
            this.pauseButton.Name = "pauseButton";
            this.pauseButton.Size = new System.Drawing.Size(52, 23);
            this.pauseButton.TabIndex = 13;
            this.pauseButton.Text = "Pause";
            this.pauseButton.UseVisualStyleBackColor = true;
            this.pauseButton.Click += new System.EventHandler(this.pauseButton_Click);
            // 
            // resetButton
            // 
            this.resetButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.resetButton.Location = new System.Drawing.Point(156, 322);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(45, 23);
            this.resetButton.TabIndex = 14;
            this.resetButton.Text = "Reset";
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // resetCurrentButton
            // 
            this.resetCurrentButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.resetCurrentButton.Location = new System.Drawing.Point(207, 322);
            this.resetCurrentButton.Name = "resetCurrentButton";
            this.resetCurrentButton.Size = new System.Drawing.Size(80, 23);
            this.resetCurrentButton.TabIndex = 15;
            this.resetCurrentButton.Text = "Reset Current";
            this.resetCurrentButton.UseVisualStyleBackColor = true;
            this.resetCurrentButton.Click += new System.EventHandler(this.resetCurrentButton_Click);
            // 
            // FlythroughPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.resetCurrentButton);
            this.Controls.Add(this.resetButton);
            this.Controls.Add(this.pauseButton);
            this.Controls.Add(this.eventsList);
            this.Controls.Add(this.loadButton);
            this.Controls.Add(this.loopCheck);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.playButton);
            this.Name = "FlythroughPanel";
            this.Size = new System.Drawing.Size(508, 348);
            this.eventsContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button playButton;
        private System.Windows.Forms.SaveFileDialog saveSequenceDialog;
        private System.Windows.Forms.ListBox eventsList;
        private System.Windows.Forms.ContextMenuStrip eventsContextMenu;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem comboEventToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rotateToEventToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveToEventToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lookAtEventToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog loadSequenceDialog;
        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.ToolStripMenuItem blankEventToolStripMenuItem;
        private System.Windows.Forms.CheckBox loopCheck;
        private System.Windows.Forms.Button pauseButton;
        private System.Windows.Forms.Button resetButton;
        private System.Windows.Forms.Button resetCurrentButton;
    }
}

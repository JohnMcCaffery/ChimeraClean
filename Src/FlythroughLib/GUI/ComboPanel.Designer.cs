namespace Chimera.FlythroughLib.GUI {
    partial class ComboPanel {
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
            this.positionsList = new System.Windows.Forms.ListBox();
            this.eventsLabel = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.positionPanel = new System.Windows.Forms.Panel();
            this.orientationPanel = new System.Windows.Forms.Panel();
            this.orientationsList = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.positionsContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveToEventToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.blankPositionEventToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removePositionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveUpPositionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.orientationsContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.rotateToEventToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lookAtEventToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.blankOrientationEventToolStripItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeOrientationToolStripItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveUpOrientationToolStripItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.positionsContextMenu.SuspendLayout();
            this.orientationsContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // positionsList
            // 
            this.positionsList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.positionsList.ContextMenuStrip = this.positionsContextMenu;
            this.positionsList.DisplayMember = "Name";
            this.positionsList.FormattingEnabled = true;
            this.positionsList.Location = new System.Drawing.Point(6, 13);
            this.positionsList.Name = "positionsList";
            this.positionsList.Size = new System.Drawing.Size(120, 303);
            this.positionsList.TabIndex = 13;
            this.positionsList.SelectedValueChanged += new System.EventHandler(this.positionsList_SelectedValueChanged);
            // 
            // eventsLabel
            // 
            this.eventsLabel.AutoSize = true;
            this.eventsLabel.Location = new System.Drawing.Point(3, 0);
            this.eventsLabel.Name = "eventsLabel";
            this.eventsLabel.Size = new System.Drawing.Size(49, 13);
            this.eventsLabel.TabIndex = 14;
            this.eventsLabel.Text = "Positions";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.positionPanel);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.orientationPanel);
            this.splitContainer1.Panel2.Controls.Add(this.orientationsList);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Size = new System.Drawing.Size(740, 320);
            this.splitContainer1.SplitterDistance = 369;
            this.splitContainer1.TabIndex = 15;
            // 
            // positionPanel
            // 
            this.positionPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.positionPanel.Location = new System.Drawing.Point(132, 0);
            this.positionPanel.Name = "positionPanel";
            this.positionPanel.Size = new System.Drawing.Size(234, 320);
            this.positionPanel.TabIndex = 19;
            // 
            // orientationPanel
            // 
            this.orientationPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.orientationPanel.Location = new System.Drawing.Point(132, 0);
            this.orientationPanel.Name = "orientationPanel";
            this.orientationPanel.Size = new System.Drawing.Size(235, 320);
            this.orientationPanel.TabIndex = 18;
            // 
            // orientationsList
            // 
            this.orientationsList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.orientationsList.ContextMenuStrip = this.orientationsContextMenu;
            this.orientationsList.DisplayMember = "Name";
            this.orientationsList.FormattingEnabled = true;
            this.orientationsList.Location = new System.Drawing.Point(6, 13);
            this.orientationsList.Name = "orientationsList";
            this.orientationsList.Size = new System.Drawing.Size(120, 303);
            this.orientationsList.TabIndex = 16;
            this.orientationsList.SelectedValueChanged += new System.EventHandler(this.orientationsList_SelectedValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Orientations";
            // 
            // positionsContextMenu
            // 
            this.positionsContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem,
            this.removePositionToolStripMenuItem,
            this.moveUpPositionToolStripMenuItem});
            this.positionsContextMenu.Name = "eventsContextMenu";
            this.positionsContextMenu.Size = new System.Drawing.Size(123, 70);
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.moveToEventToolStripMenuItem,
            this.blankPositionEventToolStripMenuItem});
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.addToolStripMenuItem.Text = "Add";
            // 
            // moveToEventToolStripMenuItem
            // 
            this.moveToEventToolStripMenuItem.Name = "moveToEventToolStripMenuItem";
            this.moveToEventToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.moveToEventToolStripMenuItem.Text = "Move To Event";
            this.moveToEventToolStripMenuItem.Click += new System.EventHandler(this.moveToEventToolStripMenuItem_Click);
            // 
            // blankPositionEventToolStripMenuItem
            // 
            this.blankPositionEventToolStripMenuItem.Name = "blankPositionEventToolStripMenuItem";
            this.blankPositionEventToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.blankPositionEventToolStripMenuItem.Text = "Blank Event";
            this.blankPositionEventToolStripMenuItem.Click += new System.EventHandler(this.blankPositionEventToolStripMenuItem_Click);
            // 
            // removePositionToolStripMenuItem
            // 
            this.removePositionToolStripMenuItem.Name = "removePositionToolStripMenuItem";
            this.removePositionToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.removePositionToolStripMenuItem.Text = "Remove";
            this.removePositionToolStripMenuItem.Click += new System.EventHandler(this.removePositionToolStripMenuItem_Click);
            // 
            // moveUpPositionToolStripMenuItem
            // 
            this.moveUpPositionToolStripMenuItem.Name = "moveUpPositionToolStripMenuItem";
            this.moveUpPositionToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.moveUpPositionToolStripMenuItem.Text = "Move Up";
            this.moveUpPositionToolStripMenuItem.Click += new System.EventHandler(this.moveUpPositionToolStripMenuItem_Click);
            // 
            // orientationsContextMenu
            // 
            this.orientationsContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.removeOrientationToolStripItem,
            this.moveUpOrientationToolStripItem});
            this.orientationsContextMenu.Name = "eventsContextMenu";
            this.orientationsContextMenu.Size = new System.Drawing.Size(123, 70);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rotateToEventToolStripMenuItem,
            this.lookAtEventToolStripMenuItem,
            this.blankOrientationEventToolStripItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem1.Text = "Add";
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
            // 
            // blankOrientationEventToolStripItem
            // 
            this.blankOrientationEventToolStripItem.Name = "blankOrientationEventToolStripItem";
            this.blankOrientationEventToolStripItem.Size = new System.Drawing.Size(157, 22);
            this.blankOrientationEventToolStripItem.Text = "Blank Event";
            this.blankOrientationEventToolStripItem.Click += new System.EventHandler(this.blankOrientationEventToolStripItem_Click);
            // 
            // removeOrientationToolStripItem
            // 
            this.removeOrientationToolStripItem.Name = "removeOrientationToolStripItem";
            this.removeOrientationToolStripItem.Size = new System.Drawing.Size(152, 22);
            this.removeOrientationToolStripItem.Text = "Remove";
            this.removeOrientationToolStripItem.Click += new System.EventHandler(this.removeOrientationToolStripItem_Click);
            // 
            // moveUpOrientationToolStripItem
            // 
            this.moveUpOrientationToolStripItem.Name = "moveUpOrientationToolStripItem";
            this.moveUpOrientationToolStripItem.Size = new System.Drawing.Size(152, 22);
            this.moveUpOrientationToolStripItem.Text = "Move Up";
            this.moveUpOrientationToolStripItem.Click += new System.EventHandler(this.moveUpOrientationToolStripItem_Click);
            // 
            // ComboPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.positionsList);
            this.Controls.Add(this.eventsLabel);
            this.Controls.Add(this.splitContainer1);
            this.MinimumSize = new System.Drawing.Size(740, 320);
            this.Name = "ComboPanel";
            this.Size = new System.Drawing.Size(740, 320);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.positionsContextMenu.ResumeLayout(false);
            this.orientationsContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox positionsList;
        private System.Windows.Forms.Label eventsLabel;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListBox orientationsList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel positionPanel;
        private System.Windows.Forms.Panel orientationPanel;
        private System.Windows.Forms.ContextMenuStrip positionsContextMenu;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveToEventToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem blankPositionEventToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removePositionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveUpPositionToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip orientationsContextMenu;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem rotateToEventToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lookAtEventToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem blankOrientationEventToolStripItem;
        private System.Windows.Forms.ToolStripMenuItem removeOrientationToolStripItem;
        private System.Windows.Forms.ToolStripMenuItem moveUpOrientationToolStripItem;

    }
}

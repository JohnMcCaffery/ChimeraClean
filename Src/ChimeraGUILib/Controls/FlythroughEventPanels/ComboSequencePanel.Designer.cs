namespace ChimeraGUILib.Controls.FlythroughEventPanels {
    partial class ComboSequencePanel {
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
            this.eventsList = new System.Windows.Forms.ListBox();
            this.eventsContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveToEventToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rotateToEventToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lookAtEventToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eventsLabel = new System.Windows.Forms.Label();
            this.blankEventToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eventsContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // eventsList
            // 
            this.eventsList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.eventsList.ContextMenuStrip = this.eventsContextMenu;
            this.eventsList.FormattingEnabled = true;
            this.eventsList.Location = new System.Drawing.Point(0, 13);
            this.eventsList.Name = "eventsList";
            this.eventsList.Size = new System.Drawing.Size(120, 290);
            this.eventsList.TabIndex = 10;
            this.eventsList.SelectedValueChanged += new System.EventHandler(this.eventsList_SelectedValueChanged);
            // 
            // eventsContextMenu
            // 
            this.eventsContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem,
            this.removeToolStripMenuItem,
            this.moveUpToolStripMenuItem});
            this.eventsContextMenu.Name = "eventsContextMenu";
            this.eventsContextMenu.Size = new System.Drawing.Size(153, 92);
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.moveToEventToolStripMenuItem,
            this.rotateToEventToolStripMenuItem,
            this.lookAtEventToolStripMenuItem,
            this.blankEventToolStripMenuItem});
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
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
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.removeToolStripMenuItem.Text = "Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // eventsLabel
            // 
            this.eventsLabel.AutoSize = true;
            this.eventsLabel.Location = new System.Drawing.Point(-3, 0);
            this.eventsLabel.Name = "eventsLabel";
            this.eventsLabel.Size = new System.Drawing.Size(40, 13);
            this.eventsLabel.TabIndex = 12;
            this.eventsLabel.Text = "Events";
            // 
            // blankEventToolStripMenuItem
            // 
            this.blankEventToolStripMenuItem.Name = "blankEventToolStripMenuItem";
            this.blankEventToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.blankEventToolStripMenuItem.Text = "Blank Event";
            this.blankEventToolStripMenuItem.Click += new System.EventHandler(this.blankEventToolStripMenuItem_Click);
            // 
            // moveUpToolStripMenuItem
            // 
            this.moveUpToolStripMenuItem.Name = "moveUpToolStripMenuItem";
            this.moveUpToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.moveUpToolStripMenuItem.Text = "Move Up";
            this.moveUpToolStripMenuItem.Click += new System.EventHandler(this.moveUpToolStripMenuItem_Click);
            // 
            // ComboSequencePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.eventsList);
            this.Controls.Add(this.eventsLabel);
            this.Name = "ComboSequencePanel";
            this.Size = new System.Drawing.Size(744, 314);
            this.eventsContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox eventsList;
        private System.Windows.Forms.ContextMenuStrip eventsContextMenu;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rotateToEventToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveToEventToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lookAtEventToolStripMenuItem;
        private System.Windows.Forms.Label eventsLabel;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem blankEventToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveUpToolStripMenuItem;
    }
}

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
namespace Chimera.Flythrough.GUI {
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
            this.positionsContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveToEventToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.blankPositionEventToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removePositionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveUpPositionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eventsLabel = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.copyCurrentPositionButton = new System.Windows.Forms.Button();
            this.positionEndButton = new System.Windows.Forms.Button();
            this.positionStartButton = new System.Windows.Forms.Button();
            this.positionPanel = new System.Windows.Forms.Panel();
            this.copyCurrentPairButton = new System.Windows.Forms.Button();
            this.copyCurrentOrientationButton = new System.Windows.Forms.Button();
            this.orientationFinishButton = new System.Windows.Forms.Button();
            this.orientationStartButton = new System.Windows.Forms.Button();
            this.orientationPanel = new System.Windows.Forms.Panel();
            this.orientationsList = new System.Windows.Forms.ListBox();
            this.orientationsContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.rotateToEventToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lookAtEventToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.blankOrientationEventToolStripItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeOrientationToolStripItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveUpOrientationToolStripItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.splitContainer5 = new System.Windows.Forms.SplitContainer();
            this.positionsContextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.orientationsContextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).BeginInit();
            this.splitContainer5.Panel1.SuspendLayout();
            this.splitContainer5.Panel2.SuspendLayout();
            this.splitContainer5.SuspendLayout();
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
            this.positionsList.Size = new System.Drawing.Size(120, 238);
            this.positionsList.TabIndex = 13;
            this.positionsList.SelectedValueChanged += new System.EventHandler(this.positionsList_SelectedValueChanged);
            this.positionsList.DoubleClick += new System.EventHandler(this.positionsList_DoubleClick);
            this.positionsList.KeyUp += new System.Windows.Forms.KeyEventHandler(this.positionsList_KeyUp);
            this.positionsList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.positionsList_MouseDown);
            this.positionsList.MouseLeave += new System.EventHandler(this.positionsList_MouseLeave);
            this.positionsList.MouseMove += new System.Windows.Forms.MouseEventHandler(this.positionsList_MouseMove);
            this.positionsList.MouseUp += new System.Windows.Forms.MouseEventHandler(this.positionsList_MouseUp);
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
            this.addToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
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
            this.removePositionToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.removePositionToolStripMenuItem.Text = "Remove";
            this.removePositionToolStripMenuItem.Click += new System.EventHandler(this.removePositionToolStripMenuItem_Click);
            // 
            // moveUpPositionToolStripMenuItem
            // 
            this.moveUpPositionToolStripMenuItem.Name = "moveUpPositionToolStripMenuItem";
            this.moveUpPositionToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.moveUpPositionToolStripMenuItem.Text = "Move Up";
            this.moveUpPositionToolStripMenuItem.Click += new System.EventHandler(this.moveUpPositionToolStripMenuItem_Click);
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
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer4);
            this.splitContainer1.Panel1.Controls.Add(this.positionPanel);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Panel2.Controls.Add(this.copyCurrentPairButton);
            this.splitContainer1.Panel2.Controls.Add(this.orientationPanel);
            this.splitContainer1.Panel2.Controls.Add(this.orientationsList);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Size = new System.Drawing.Size(891, 259);
            this.splitContainer1.SplitterDistance = 444;
            this.splitContainer1.TabIndex = 15;
            // 
            // copyCurrentPositionButton
            // 
            this.copyCurrentPositionButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.copyCurrentPositionButton.Location = new System.Drawing.Point(3, 10);
            this.copyCurrentPositionButton.Name = "copyCurrentPositionButton";
            this.copyCurrentPositionButton.Size = new System.Drawing.Size(134, 23);
            this.copyCurrentPositionButton.TabIndex = 22;
            this.copyCurrentPositionButton.Text = "Copy Current Position";
            this.copyCurrentPositionButton.UseVisualStyleBackColor = true;
            this.copyCurrentPositionButton.Click += new System.EventHandler(this.copyCurrentPositionButton_Click);
            // 
            // positionEndButton
            // 
            this.positionEndButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.positionEndButton.Location = new System.Drawing.Point(3, 10);
            this.positionEndButton.Name = "positionEndButton";
            this.positionEndButton.Size = new System.Drawing.Size(75, 23);
            this.positionEndButton.TabIndex = 21;
            this.positionEndButton.Text = "Go To Finish";
            this.positionEndButton.UseVisualStyleBackColor = true;
            this.positionEndButton.Click += new System.EventHandler(this.positionEndButton_Click);
            // 
            // positionStartButton
            // 
            this.positionStartButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.positionStartButton.Location = new System.Drawing.Point(4, 10);
            this.positionStartButton.Name = "positionStartButton";
            this.positionStartButton.Size = new System.Drawing.Size(75, 23);
            this.positionStartButton.TabIndex = 20;
            this.positionStartButton.Text = "Go To Begin";
            this.positionStartButton.UseVisualStyleBackColor = true;
            this.positionStartButton.Click += new System.EventHandler(this.positionStartButton_Click);
            // 
            // positionPanel
            // 
            this.positionPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.positionPanel.Location = new System.Drawing.Point(132, 0);
            this.positionPanel.Name = "positionPanel";
            this.positionPanel.Size = new System.Drawing.Size(309, 226);
            this.positionPanel.TabIndex = 19;
            // 
            // copyCurrentPairButton
            // 
            this.copyCurrentPairButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.copyCurrentPairButton.Location = new System.Drawing.Point(-1, 232);
            this.copyCurrentPairButton.Name = "copyCurrentPairButton";
            this.copyCurrentPairButton.Size = new System.Drawing.Size(127, 23);
            this.copyCurrentPairButton.TabIndex = 24;
            this.copyCurrentPairButton.Text = "Copy Current Pair";
            this.copyCurrentPairButton.UseVisualStyleBackColor = true;
            this.copyCurrentPairButton.Click += new System.EventHandler(this.copyCurrentPairButton_Click);
            // 
            // copyCurrentOrientationButton
            // 
            this.copyCurrentOrientationButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.copyCurrentOrientationButton.Location = new System.Drawing.Point(3, 9);
            this.copyCurrentOrientationButton.Name = "copyCurrentOrientationButton";
            this.copyCurrentOrientationButton.Size = new System.Drawing.Size(134, 23);
            this.copyCurrentOrientationButton.TabIndex = 23;
            this.copyCurrentOrientationButton.Text = "Copy Current Orientation";
            this.copyCurrentOrientationButton.UseVisualStyleBackColor = true;
            this.copyCurrentOrientationButton.Click += new System.EventHandler(this.copyCurrentOrientationButton_Click);
            // 
            // orientationFinishButton
            // 
            this.orientationFinishButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.orientationFinishButton.Location = new System.Drawing.Point(3, 10);
            this.orientationFinishButton.Name = "orientationFinishButton";
            this.orientationFinishButton.Size = new System.Drawing.Size(75, 23);
            this.orientationFinishButton.TabIndex = 23;
            this.orientationFinishButton.Text = "Go To Finish";
            this.orientationFinishButton.UseVisualStyleBackColor = true;
            this.orientationFinishButton.Click += new System.EventHandler(this.orientationFinishButton_Click);
            // 
            // orientationStartButton
            // 
            this.orientationStartButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.orientationStartButton.Location = new System.Drawing.Point(3, 9);
            this.orientationStartButton.Name = "orientationStartButton";
            this.orientationStartButton.Size = new System.Drawing.Size(76, 23);
            this.orientationStartButton.TabIndex = 22;
            this.orientationStartButton.Text = "Go To Begin";
            this.orientationStartButton.UseVisualStyleBackColor = true;
            this.orientationStartButton.Click += new System.EventHandler(this.orientationStartButton_Click);
            // 
            // orientationPanel
            // 
            this.orientationPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.orientationPanel.Location = new System.Drawing.Point(132, 0);
            this.orientationPanel.Name = "orientationPanel";
            this.orientationPanel.Size = new System.Drawing.Size(311, 226);
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
            this.orientationsList.Size = new System.Drawing.Size(120, 212);
            this.orientationsList.TabIndex = 16;
            this.orientationsList.SelectedValueChanged += new System.EventHandler(this.orientationsList_SelectedValueChanged);
            this.orientationsList.DoubleClick += new System.EventHandler(this.orientationsList_DoubleClick);
            this.orientationsList.KeyUp += new System.Windows.Forms.KeyEventHandler(this.orientationsList_KeyUp);
            this.orientationsList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.orientationsList_MouseDown);
            this.orientationsList.MouseLeave += new System.EventHandler(this.orientationsList_MouseLeave);
            this.orientationsList.MouseMove += new System.Windows.Forms.MouseEventHandler(this.orientationsList_MouseMove);
            this.orientationsList.MouseUp += new System.Windows.Forms.MouseEventHandler(this.orientationsList_MouseUp);
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
            this.lookAtEventToolStripMenuItem.Click += new System.EventHandler(this.lookAtEventToolStripMenuItem_Click);
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Orientations";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer2.Location = new System.Drawing.Point(132, 223);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.orientationStartButton);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer2.Size = new System.Drawing.Size(311, 36);
            this.splitContainer2.SplitterDistance = 82;
            this.splitContainer2.TabIndex = 25;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.copyCurrentOrientationButton);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.orientationFinishButton);
            this.splitContainer3.Size = new System.Drawing.Size(225, 36);
            this.splitContainer3.SplitterDistance = 140;
            this.splitContainer3.TabIndex = 26;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer4.Location = new System.Drawing.Point(130, 223);
            this.splitContainer4.Name = "splitContainer4";
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.positionStartButton);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.splitContainer5);
            this.splitContainer4.Size = new System.Drawing.Size(311, 36);
            this.splitContainer4.SplitterDistance = 82;
            this.splitContainer4.TabIndex = 26;
            // 
            // splitContainer5
            // 
            this.splitContainer5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer5.Location = new System.Drawing.Point(0, 0);
            this.splitContainer5.Name = "splitContainer5";
            // 
            // splitContainer5.Panel1
            // 
            this.splitContainer5.Panel1.Controls.Add(this.copyCurrentPositionButton);
            // 
            // splitContainer5.Panel2
            // 
            this.splitContainer5.Panel2.Controls.Add(this.positionEndButton);
            this.splitContainer5.Size = new System.Drawing.Size(225, 36);
            this.splitContainer5.SplitterDistance = 140;
            this.splitContainer5.TabIndex = 26;
            // 
            // ComboPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.positionsList);
            this.Controls.Add(this.eventsLabel);
            this.Controls.Add(this.splitContainer1);
            this.MinimumSize = new System.Drawing.Size(470, 100);
            this.Name = "ComboPanel";
            this.Size = new System.Drawing.Size(891, 259);
            this.VisibleChanged += new System.EventHandler(this.ComboPanel_VisibleChanged);
            this.positionsContextMenu.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.orientationsContextMenu.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.splitContainer5.Panel1.ResumeLayout(false);
            this.splitContainer5.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).EndInit();
            this.splitContainer5.ResumeLayout(false);
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
        private System.Windows.Forms.Button positionEndButton;
        private System.Windows.Forms.Button positionStartButton;
        private System.Windows.Forms.Button orientationFinishButton;
        private System.Windows.Forms.Button orientationStartButton;
        private System.Windows.Forms.Button copyCurrentPositionButton;
        private System.Windows.Forms.Button copyCurrentOrientationButton;
        private System.Windows.Forms.Button copyCurrentPairButton;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.SplitContainer splitContainer5;

    }
}

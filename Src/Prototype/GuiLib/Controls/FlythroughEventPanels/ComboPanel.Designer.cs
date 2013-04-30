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
namespace ChimeraGUILib.Controls.FlythroughEventPanels {
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
            this.sequenceSplit = new System.Windows.Forms.SplitContainer();
            this.sequence1Panel = new ChimeraGUILib.Controls.FlythroughEventPanels.ComboSequencePanel();
            this.sequence2Panel = new ChimeraGUILib.Controls.FlythroughEventPanels.ComboSequencePanel();
            ((System.ComponentModel.ISupportInitialize)(this.sequenceSplit)).BeginInit();
            this.sequenceSplit.Panel1.SuspendLayout();
            this.sequenceSplit.Panel2.SuspendLayout();
            this.sequenceSplit.SuspendLayout();
            this.SuspendLayout();
            // 
            // sequenceSplit
            // 
            this.sequenceSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sequenceSplit.Location = new System.Drawing.Point(0, 0);
            this.sequenceSplit.Name = "sequenceSplit";
            // 
            // sequenceSplit.Panel1
            // 
            this.sequenceSplit.Panel1.Controls.Add(this.sequence1Panel);
            // 
            // sequenceSplit.Panel2
            // 
            this.sequenceSplit.Panel2.Controls.Add(this.sequence2Panel);
            this.sequenceSplit.Size = new System.Drawing.Size(740, 320);
            this.sequenceSplit.SplitterDistance = 386;
            this.sequenceSplit.TabIndex = 0;
            // 
            // sequence1Panel
            // 
            this.sequence1Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sequence1Panel.Location = new System.Drawing.Point(0, 0);
            this.sequence1Panel.Name = "sequence1Panel";
            this.sequence1Panel.Size = new System.Drawing.Size(386, 320);
            this.sequence1Panel.TabIndex = 0;
            // 
            // sequence2Panel
            // 
            this.sequence2Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sequence2Panel.Location = new System.Drawing.Point(0, 0);
            this.sequence2Panel.Name = "sequence2Panel";
            this.sequence2Panel.Size = new System.Drawing.Size(350, 320);
            this.sequence2Panel.TabIndex = 0;
            // 
            // ComboPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.sequenceSplit);
            this.MinimumSize = new System.Drawing.Size(740, 320);
            this.Name = "ComboPanel";
            this.Size = new System.Drawing.Size(740, 320);
            this.sequenceSplit.Panel1.ResumeLayout(false);
            this.sequenceSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sequenceSplit)).EndInit();
            this.sequenceSplit.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer sequenceSplit;
        private ComboSequencePanel sequence1Panel;
        private ComboSequencePanel sequence2Panel;
    }
}

namespace Chimera.GUI.Controls.Plugins {
    partial class SplitAxisPanel {
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.positiveBox = new System.Windows.Forms.GroupBox();
            this.negativeBox = new System.Windows.Forms.GroupBox();
            this.positivePulldown = new System.Windows.Forms.ComboBox();
            this.negativePulldown = new System.Windows.Forms.ComboBox();
            this.positivePanel = new System.Windows.Forms.Panel();
            this.negativePanel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.positiveBox.SuspendLayout();
            this.negativeBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.positiveBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.negativeBox);
            this.splitContainer1.Size = new System.Drawing.Size(323, 130);
            this.splitContainer1.SplitterDistance = 160;
            this.splitContainer1.TabIndex = 0;
            // 
            // positiveBox
            // 
            this.positiveBox.Controls.Add(this.positivePanel);
            this.positiveBox.Controls.Add(this.positivePulldown);
            this.positiveBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.positiveBox.Location = new System.Drawing.Point(0, 0);
            this.positiveBox.Name = "positiveBox";
            this.positiveBox.Size = new System.Drawing.Size(160, 130);
            this.positiveBox.TabIndex = 0;
            this.positiveBox.TabStop = false;
            this.positiveBox.Text = "Positive";
            // 
            // negativeBox
            // 
            this.negativeBox.Controls.Add(this.negativePanel);
            this.negativeBox.Controls.Add(this.negativePulldown);
            this.negativeBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.negativeBox.Location = new System.Drawing.Point(0, 0);
            this.negativeBox.Name = "negativeBox";
            this.negativeBox.Size = new System.Drawing.Size(159, 130);
            this.negativeBox.TabIndex = 0;
            this.negativeBox.TabStop = false;
            this.negativeBox.Text = "Negative";
            // 
            // positivePulldown
            // 
            this.positivePulldown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.positivePulldown.DisplayMember = "Name";
            this.positivePulldown.FormattingEnabled = true;
            this.positivePulldown.Location = new System.Drawing.Point(6, 19);
            this.positivePulldown.Name = "positivePulldown";
            this.positivePulldown.Size = new System.Drawing.Size(148, 21);
            this.positivePulldown.TabIndex = 0;
            this.positivePulldown.SelectedIndexChanged += new System.EventHandler(this.positivePulldown_SelectedIndexChanged);
            // 
            // negativePulldown
            // 
            this.negativePulldown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.negativePulldown.DisplayMember = "Name";
            this.negativePulldown.FormattingEnabled = true;
            this.negativePulldown.Location = new System.Drawing.Point(6, 19);
            this.negativePulldown.Name = "negativePulldown";
            this.negativePulldown.Size = new System.Drawing.Size(148, 21);
            this.negativePulldown.TabIndex = 1;
            this.negativePulldown.SelectedIndexChanged += new System.EventHandler(this.negativePulldown_SelectedIndexChanged);
            // 
            // positivePanel
            // 
            this.positivePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.positivePanel.Location = new System.Drawing.Point(6, 46);
            this.positivePanel.Name = "positivePanel";
            this.positivePanel.Size = new System.Drawing.Size(148, 81);
            this.positivePanel.TabIndex = 1;
            // 
            // negativePanel
            // 
            this.negativePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.negativePanel.Location = new System.Drawing.Point(6, 46);
            this.negativePanel.Name = "negativePanel";
            this.negativePanel.Size = new System.Drawing.Size(148, 81);
            this.negativePanel.TabIndex = 2;
            // 
            // SplitAxisPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "SplitAxisPanel";
            this.Size = new System.Drawing.Size(323, 130);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.positiveBox.ResumeLayout(false);
            this.negativeBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox positiveBox;
        private System.Windows.Forms.GroupBox negativeBox;
        private System.Windows.Forms.ComboBox positivePulldown;
        private System.Windows.Forms.ComboBox negativePulldown;
        private System.Windows.Forms.Panel positivePanel;
        private System.Windows.Forms.Panel negativePanel;
    }
}

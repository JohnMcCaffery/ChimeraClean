namespace Chimera.RemoteControl {
    partial class RemoteControlForm {
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
            this.hSplit = new System.Windows.Forms.SplitContainer();
            this.shutdownButton = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.leftButton = new System.Windows.Forms.Button();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.centreButton = new System.Windows.Forms.Button();
            this.rightButton = new System.Windows.Forms.Button();
            this.shutdownCheck = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.hSplit)).BeginInit();
            this.hSplit.Panel1.SuspendLayout();
            this.hSplit.Panel2.SuspendLayout();
            this.hSplit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // hSplit
            // 
            this.hSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hSplit.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.hSplit.Location = new System.Drawing.Point(0, 0);
            this.hSplit.Name = "hSplit";
            this.hSplit.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // hSplit.Panel1
            // 
            this.hSplit.Panel1.Controls.Add(this.shutdownCheck);
            this.hSplit.Panel1.Controls.Add(this.shutdownButton);
            // 
            // hSplit.Panel2
            // 
            this.hSplit.Panel2.Controls.Add(this.splitContainer1);
            this.hSplit.Panel2MinSize = 0;
            this.hSplit.Size = new System.Drawing.Size(638, 450);
            this.hSplit.SplitterDistance = 360;
            this.hSplit.TabIndex = 0;
            // 
            // shutdownButton
            // 
            this.shutdownButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.shutdownButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 50F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.shutdownButton.Location = new System.Drawing.Point(0, 0);
            this.shutdownButton.Name = "shutdownButton";
            this.shutdownButton.Size = new System.Drawing.Size(638, 360);
            this.shutdownButton.TabIndex = 0;
            this.shutdownButton.Text = "Shut Down";
            this.shutdownButton.UseVisualStyleBackColor = true;
            this.shutdownButton.Click += new System.EventHandler(this.shutdownButton_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.leftButton);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(638, 86);
            this.splitContainer1.SplitterDistance = 211;
            this.splitContainer1.TabIndex = 0;
            // 
            // leftButton
            // 
            this.leftButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.leftButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.leftButton.Location = new System.Drawing.Point(0, 0);
            this.leftButton.Name = "leftButton";
            this.leftButton.Size = new System.Drawing.Size(211, 86);
            this.leftButton.TabIndex = 0;
            this.leftButton.Text = "Restart Left Screen";
            this.leftButton.UseVisualStyleBackColor = true;
            this.leftButton.Click += new System.EventHandler(this.leftButton_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.centreButton);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.rightButton);
            this.splitContainer2.Size = new System.Drawing.Size(423, 86);
            this.splitContainer2.SplitterDistance = 218;
            this.splitContainer2.TabIndex = 0;
            // 
            // centreButton
            // 
            this.centreButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.centreButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.centreButton.Location = new System.Drawing.Point(0, 0);
            this.centreButton.Name = "centreButton";
            this.centreButton.Size = new System.Drawing.Size(218, 86);
            this.centreButton.TabIndex = 0;
            this.centreButton.Text = "Restart Centre Screen";
            this.centreButton.UseVisualStyleBackColor = true;
            this.centreButton.Click += new System.EventHandler(this.centreButton_Click);
            // 
            // rightButton
            // 
            this.rightButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rightButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rightButton.Location = new System.Drawing.Point(0, 0);
            this.rightButton.Name = "rightButton";
            this.rightButton.Size = new System.Drawing.Size(201, 86);
            this.rightButton.TabIndex = 0;
            this.rightButton.Text = "Restart Right Screen";
            this.rightButton.UseVisualStyleBackColor = true;
            this.rightButton.Click += new System.EventHandler(this.rightButton_Click);
            // 
            // shutdownCheck
            // 
            this.shutdownCheck.AutoSize = true;
            this.shutdownCheck.Checked = true;
            this.shutdownCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.shutdownCheck.Location = new System.Drawing.Point(13, 13);
            this.shutdownCheck.Name = "shutdownCheck";
            this.shutdownCheck.Size = new System.Drawing.Size(127, 17);
            this.shutdownCheck.TabIndex = 1;
            this.shutdownCheck.Text = "Shut Down Computer";
            this.shutdownCheck.UseVisualStyleBackColor = true;
            // 
            // RemoteControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(638, 450);
            this.Controls.Add(this.hSplit);
            this.Name = "RemoteControlForm";
            this.Text = "Caen Remote Control";
            this.hSplit.Panel1.ResumeLayout(false);
            this.hSplit.Panel1.PerformLayout();
            this.hSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.hSplit)).EndInit();
            this.hSplit.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer hSplit;
        private System.Windows.Forms.Button shutdownButton;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button leftButton;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Button centreButton;
        private System.Windows.Forms.Button rightButton;
        private System.Windows.Forms.CheckBox shutdownCheck;
    }
}
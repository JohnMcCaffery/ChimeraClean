namespace Chimera.GUI.Forms {
    partial class SimpleForm {
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
            ((System.ComponentModel.ISupportInitialize)(this.hSplit)).BeginInit();
            this.hSplit.Panel1.SuspendLayout();
            this.hSplit.SuspendLayout();
            this.SuspendLayout();
            // 
            // hSplit
            // 
            this.hSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hSplit.IsSplitterFixed = true;
            this.hSplit.Location = new System.Drawing.Point(0, 0);
            this.hSplit.Name = "hSplit";
            this.hSplit.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // hSplit.Panel1
            // 
            this.hSplit.Panel1.Controls.Add(this.shutdownButton);
            this.hSplit.Size = new System.Drawing.Size(284, 262);
            this.hSplit.SplitterDistance = 128;
            this.hSplit.TabIndex = 0;
            // 
            // shutdownButton
            // 
            this.shutdownButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.shutdownButton.Location = new System.Drawing.Point(0, 0);
            this.shutdownButton.Name = "shutdownButton";
            this.shutdownButton.Size = new System.Drawing.Size(284, 128);
            this.shutdownButton.TabIndex = 0;
            this.shutdownButton.Text = "Shut down";
            this.shutdownButton.UseVisualStyleBackColor = true;
            this.shutdownButton.Click += new System.EventHandler(this.shutdownButton_Click);
            // 
            // SimpleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.hSplit);
            this.Name = "SimpleForm";
            this.Text = "SimpleForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SimpleForm_FormClosing);
            this.hSplit.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.hSplit)).EndInit();
            this.hSplit.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer hSplit;
        private System.Windows.Forms.Button shutdownButton;
    }
}
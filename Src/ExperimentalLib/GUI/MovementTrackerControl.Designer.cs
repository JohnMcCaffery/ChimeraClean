namespace Chimera.Experimental.GUI {
    partial class MovementTrackerControl {
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
            this.timeLabel = new System.Windows.Forms.Label();
            this.prepCheck = new System.Windows.Forms.CheckBox();
            this.stateLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // timeLabel
            // 
            this.timeLabel.AutoSize = true;
            this.timeLabel.Location = new System.Drawing.Point(4, 22);
            this.timeLabel.Name = "timeLabel";
            this.timeLabel.Size = new System.Drawing.Size(67, 13);
            this.timeLabel.TabIndex = 0;
            this.timeLabel.Text = "Not Running";
            // 
            // prepCheck
            // 
            this.prepCheck.AutoSize = true;
            this.prepCheck.Location = new System.Drawing.Point(7, 47);
            this.prepCheck.Name = "prepCheck";
            this.prepCheck.Size = new System.Drawing.Size(48, 17);
            this.prepCheck.TabIndex = 1;
            this.prepCheck.Text = "Prep";
            this.prepCheck.UseVisualStyleBackColor = true;
            this.prepCheck.CheckedChanged += new System.EventHandler(this.prepCheck_CheckedChanged);
            // 
            // stateLabel
            // 
            this.stateLabel.AutoSize = true;
            this.stateLabel.Location = new System.Drawing.Point(4, 0);
            this.stateLabel.Name = "stateLabel";
            this.stateLabel.Size = new System.Drawing.Size(81, 13);
            this.stateLabel.TabIndex = 2;
            this.stateLabel.Text = "State: FirstLoad";
            // 
            // MovementTrackerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.stateLabel);
            this.Controls.Add(this.prepCheck);
            this.Controls.Add(this.timeLabel);
            this.Name = "MovementTrackerControl";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label timeLabel;
        private System.Windows.Forms.CheckBox prepCheck;
        private System.Windows.Forms.Label stateLabel;
    }
}

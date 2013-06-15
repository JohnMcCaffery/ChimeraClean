namespace Chimera.GUI.Forms {
    partial class StatisticsForm {
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
            this.statisticsPanel = new Chimera.GUI.Controls.StatisticsPanel();
            this.SuspendLayout();
            // 
            // statisticsPanel
            // 
            this.statisticsPanel.Active = false;
            this.statisticsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statisticsPanel.Location = new System.Drawing.Point(0, 0);
            this.statisticsPanel.Name = "statisticsPanel";
            this.statisticsPanel.ShowTick = true;
            this.statisticsPanel.Size = new System.Drawing.Size(284, 262);
            this.statisticsPanel.TabIndex = 0;
            // 
            // StatisticsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.statisticsPanel);
            this.Name = "StatisticsForm";
            this.Text = "StatisticsForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.StatisticsForm_FormClosing);
            this.Load += new System.EventHandler(this.StatisticsForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Chimera.GUI.Controls.StatisticsPanel statisticsPanel;
    }
}
namespace ConsoleTest {
    partial class ProxyForm {
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
            this.proxyPanel = new UtilLib.ProxyPanel();
            this.SuspendLayout();
            // 
            // proxyPanel
            // 
            this.proxyPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.proxyPanel.FirstName = "Routing";
            this.proxyPanel.LastName = "God";
            this.proxyPanel.Location = new System.Drawing.Point(0, 0);
            this.proxyPanel.LoginURI = "http://localhost:9000";
            this.proxyPanel.Name = "proxyPanel";
            this.proxyPanel.Password = "1245";
            this.proxyPanel.Port = "8080";
            this.proxyPanel.Proxy = null;
            this.proxyPanel.Size = new System.Drawing.Size(564, 213);
            this.proxyPanel.TabIndex = 0;
            // 
            // ProxyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(564, 213);
            this.Controls.Add(this.proxyPanel);
            this.Name = "ProxyForm";
            this.Text = "ProxyForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ProxyForm_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private UtilLib.ProxyPanel proxyPanel;
    }
}
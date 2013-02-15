namespace ChimeraGUILib.Controls.FlythroughEventPanels {
    partial class LookAtPanel {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LookAtPanel));
            this.lookAtTakeCurrentButton = new System.Windows.Forms.Button();
            this.targetVectorPanel = new ProxyTestGUI.VectorPanel();
            this.lengthValue = new System.Windows.Forms.NumericUpDown();
            this.Length = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.lengthValue)).BeginInit();
            this.SuspendLayout();
            // 
            // lookAtTakeCurrentButton
            // 
            this.lookAtTakeCurrentButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lookAtTakeCurrentButton.Location = new System.Drawing.Point(132, 99);
            this.lookAtTakeCurrentButton.Name = "lookAtTakeCurrentButton";
            this.lookAtTakeCurrentButton.Size = new System.Drawing.Size(107, 23);
            this.lookAtTakeCurrentButton.TabIndex = 18;
            this.lookAtTakeCurrentButton.Text = "Take Current";
            this.lookAtTakeCurrentButton.UseVisualStyleBackColor = true;
            this.lookAtTakeCurrentButton.Click += new System.EventHandler(this.lookAtTakeCurrentButton_Click);
            // 
            // targetVectorPanel
            // 
            this.targetVectorPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.targetVectorPanel.DisplayName = "Look At Target";
            this.targetVectorPanel.Location = new System.Drawing.Point(0, 0);
            this.targetVectorPanel.Max = 1024D;
            this.targetVectorPanel.Min = -1024D;
            this.targetVectorPanel.Name = "targetVectorPanel";
            this.targetVectorPanel.Size = new System.Drawing.Size(239, 98);
            this.targetVectorPanel.TabIndex = 17;
            this.targetVectorPanel.Value = ((OpenMetaverse.Vector3)(resources.GetObject("targetVectorPanel.Value")));
            this.targetVectorPanel.X = 128F;
            this.targetVectorPanel.Y = 128F;
            this.targetVectorPanel.Z = 60F;
            // 
            // lengthValue
            // 
            this.lengthValue.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.lengthValue.Location = new System.Drawing.Point(68, 99);
            this.lengthValue.Maximum = new decimal(new int[] {
            60000,
            0,
            0,
            0});
            this.lengthValue.Name = "lengthValue";
            this.lengthValue.Size = new System.Drawing.Size(60, 20);
            this.lengthValue.TabIndex = 20;
            this.lengthValue.Value = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            // 
            // Length
            // 
            this.Length.AutoSize = true;
            this.Length.Location = new System.Drawing.Point(3, 101);
            this.Length.Name = "Length";
            this.Length.Size = new System.Drawing.Size(59, 13);
            this.Length.TabIndex = 19;
            this.Length.Text = "Length(ms)";
            // 
            // LookAtPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lengthValue);
            this.Controls.Add(this.Length);
            this.Controls.Add(this.lookAtTakeCurrentButton);
            this.Controls.Add(this.targetVectorPanel);
            this.MinimumSize = new System.Drawing.Size(242, 158);
            this.Name = "LookAtPanel";
            this.Size = new System.Drawing.Size(242, 158);
            ((System.ComponentModel.ISupportInitialize)(this.lengthValue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button lookAtTakeCurrentButton;
        private ProxyTestGUI.VectorPanel targetVectorPanel;
        private System.Windows.Forms.NumericUpDown lengthValue;
        private System.Windows.Forms.Label Length;
    }
}

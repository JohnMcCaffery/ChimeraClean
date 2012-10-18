namespace ProxyTestGUI {
    partial class RotationPanel {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RotationPanel));
            this.vectorPanel = new ProxyTestGUI.VectorPanel();
            this.rotationValue = new System.Windows.Forms.NumericUpDown();
            this.rotationSlider = new System.Windows.Forms.TrackBar();
            this.pitchValue = new System.Windows.Forms.NumericUpDown();
            this.pitchSlider = new System.Windows.Forms.TrackBar();
            this.yawLabel = new System.Windows.Forms.Label();
            this.pitchLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.rotationValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rotationSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pitchValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pitchSlider)).BeginInit();
            this.SuspendLayout();
            // 
            // vectorPanel
            // 
            this.vectorPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vectorPanel.DisplayName = "Name";
            this.vectorPanel.Location = new System.Drawing.Point(3, 3);
            this.vectorPanel.Max = 1D;
            this.vectorPanel.Min = -1D;
            this.vectorPanel.Name = "vectorPanel";
            this.vectorPanel.Size = new System.Drawing.Size(404, 98);
            this.vectorPanel.TabIndex = 0;
            this.vectorPanel.Value = ((OpenMetaverse.Vector3)(resources.GetObject("vectorPanel.Value")));
            this.vectorPanel.OnChange += new System.EventHandler(this.vectorPanel_OnChange);
            // 
            // rotationValue
            // 
            this.rotationValue.DecimalPlaces = 2;
            this.rotationValue.Location = new System.Drawing.Point(26, 96);
            this.rotationValue.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.rotationValue.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
            this.rotationValue.Name = "rotationValue";
            this.rotationValue.Size = new System.Drawing.Size(63, 20);
            this.rotationValue.TabIndex = 83;
            this.rotationValue.ValueChanged += new System.EventHandler(this.rotationValue_ValueChanged);
            // 
            // rotationSlider
            // 
            this.rotationSlider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rotationSlider.Location = new System.Drawing.Point(87, 96);
            this.rotationSlider.Maximum = 360;
            this.rotationSlider.Minimum = -360;
            this.rotationSlider.Name = "rotationSlider";
            this.rotationSlider.Size = new System.Drawing.Size(320, 42);
            this.rotationSlider.TabIndex = 84;
            this.rotationSlider.TickStyle = System.Windows.Forms.TickStyle.None;
            this.rotationSlider.Scroll += new System.EventHandler(this.rotationSlider_Scroll);
            // 
            // pitchValue
            // 
            this.pitchValue.DecimalPlaces = 2;
            this.pitchValue.Location = new System.Drawing.Point(26, 122);
            this.pitchValue.Maximum = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.pitchValue.Minimum = new decimal(new int[] {
            90,
            0,
            0,
            -2147483648});
            this.pitchValue.Name = "pitchValue";
            this.pitchValue.Size = new System.Drawing.Size(63, 20);
            this.pitchValue.TabIndex = 85;
            this.pitchValue.ValueChanged += new System.EventHandler(this.pitchValue_ValueChanged);
            // 
            // pitchSlider
            // 
            this.pitchSlider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pitchSlider.Location = new System.Drawing.Point(87, 122);
            this.pitchSlider.Maximum = 90;
            this.pitchSlider.Minimum = -90;
            this.pitchSlider.Name = "pitchSlider";
            this.pitchSlider.Size = new System.Drawing.Size(320, 42);
            this.pitchSlider.TabIndex = 86;
            this.pitchSlider.TickStyle = System.Windows.Forms.TickStyle.None;
            this.pitchSlider.Scroll += new System.EventHandler(this.pitchSlider_Scroll);
            // 
            // yawLabel
            // 
            this.yawLabel.AutoSize = true;
            this.yawLabel.Location = new System.Drawing.Point(6, 98);
            this.yawLabel.Name = "yawLabel";
            this.yawLabel.Size = new System.Drawing.Size(14, 13);
            this.yawLabel.TabIndex = 87;
            this.yawLabel.Text = "Y";
            // 
            // pitchLabel
            // 
            this.pitchLabel.AutoSize = true;
            this.pitchLabel.Location = new System.Drawing.Point(6, 124);
            this.pitchLabel.Name = "pitchLabel";
            this.pitchLabel.Size = new System.Drawing.Size(14, 13);
            this.pitchLabel.TabIndex = 88;
            this.pitchLabel.Text = "P";
            // 
            // RotationPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pitchLabel);
            this.Controls.Add(this.rotationValue);
            this.Controls.Add(this.yawLabel);
            this.Controls.Add(this.pitchValue);
            this.Controls.Add(this.pitchSlider);
            this.Controls.Add(this.rotationSlider);
            this.Controls.Add(this.vectorPanel);
            this.Name = "RotationPanel";
            this.Size = new System.Drawing.Size(410, 147);
            ((System.ComponentModel.ISupportInitialize)(this.rotationValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rotationSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pitchValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pitchSlider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private VectorPanel vectorPanel;
        private System.Windows.Forms.NumericUpDown rotationValue;
        private System.Windows.Forms.TrackBar rotationSlider;
        private System.Windows.Forms.NumericUpDown pitchValue;
        private System.Windows.Forms.TrackBar pitchSlider;
        private System.Windows.Forms.Label yawLabel;
        private System.Windows.Forms.Label pitchLabel;
    }
}

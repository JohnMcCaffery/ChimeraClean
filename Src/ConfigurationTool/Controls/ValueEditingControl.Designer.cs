namespace Chimera.ConfigurationTool.Controls {
    partial class ValueEditingControl {
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
            this.boolInput = new System.Windows.Forms.CheckBox();
            this.textInput = new System.Windows.Forms.ComboBox();
            this.numberInput = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numberInput)).BeginInit();
            this.SuspendLayout();
            // 
            // boolInput
            // 
            this.boolInput.AutoSize = true;
            this.boolInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.boolInput.Location = new System.Drawing.Point(0, 0);
            this.boolInput.Name = "boolInput";
            this.boolInput.Size = new System.Drawing.Size(259, 28);
            this.boolInput.TabIndex = 0;
            this.boolInput.UseVisualStyleBackColor = true;
            this.boolInput.Visible = false;
            // 
            // textInput
            // 
            this.textInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textInput.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.textInput.FormattingEnabled = true;
            this.textInput.Location = new System.Drawing.Point(0, 0);
            this.textInput.Name = "textInput";
            this.textInput.Size = new System.Drawing.Size(259, 21);
            this.textInput.TabIndex = 1;
            this.textInput.Visible = false;
            // 
            // numberInput
            // 
            this.numberInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numberInput.Location = new System.Drawing.Point(0, 0);
            this.numberInput.Name = "numberInput";
            this.numberInput.Size = new System.Drawing.Size(259, 20);
            this.numberInput.TabIndex = 2;
            // 
            // ValueEditingControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.boolInput);
            this.Controls.Add(this.textInput);
            this.Controls.Add(this.numberInput);
            this.Name = "ValueEditingControl";
            this.Size = new System.Drawing.Size(259, 28);
            ((System.ComponentModel.ISupportInitialize)(this.numberInput)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox boolInput;
        private System.Windows.Forms.ComboBox textInput;
        private System.Windows.Forms.NumericUpDown numberInput;
    }
}

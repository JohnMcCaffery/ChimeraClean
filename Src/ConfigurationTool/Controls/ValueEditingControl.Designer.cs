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
            this.dialogButton = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // boolInput
            // 
            this.boolInput.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.boolInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.boolInput.Location = new System.Drawing.Point(0, 0);
            this.boolInput.Name = "boolInput";
            this.boolInput.Size = new System.Drawing.Size(259, 28);
            this.boolInput.TabIndex = 0;
            this.boolInput.UseVisualStyleBackColor = true;
            this.boolInput.Visible = false;
            this.boolInput.CheckedChanged += new System.EventHandler(this.boolInput_CheckedChanged);
            // 
            // textInput
            // 
            this.textInput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textInput.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.textInput.FormattingEnabled = true;
            this.textInput.Location = new System.Drawing.Point(0, 0);
            this.textInput.Name = "textInput";
            this.textInput.Size = new System.Drawing.Size(259, 21);
            this.textInput.TabIndex = 1;
            this.textInput.Visible = false;
            this.textInput.TextChanged += new System.EventHandler(this.textInput_TextChanged);
            this.textInput.Validating += new System.ComponentModel.CancelEventHandler(this.textInput_Validating);
            // 
            // dialogButton
            // 
            this.dialogButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dialogButton.Location = new System.Drawing.Point(234, 0);
            this.dialogButton.Name = "dialogButton";
            this.dialogButton.Size = new System.Drawing.Size(25, 28);
            this.dialogButton.TabIndex = 2;
            this.dialogButton.Text = "...";
            this.dialogButton.UseVisualStyleBackColor = true;
            this.dialogButton.Click += new System.EventHandler(this.dialogButton_Click);
            // 
            // folderBrowserDialog
            // 
            this.folderBrowserDialog.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // ValueEditingControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dialogButton);
            this.Controls.Add(this.textInput);
            this.Controls.Add(this.boolInput);
            this.Name = "ValueEditingControl";
            this.Size = new System.Drawing.Size(259, 28);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox boolInput;
        private System.Windows.Forms.ComboBox textInput;
        private System.Windows.Forms.Button dialogButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
    }
}

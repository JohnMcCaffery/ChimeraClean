namespace Chimera.ConfigurationTool.Controls {
    partial class ConfigurationObjectControlPanel {
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
            this.parametersList = new System.Windows.Forms.DataGridView();
            this.Key = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ValueCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DefaultCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DescriptionCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.parametersList)).BeginInit();
            this.SuspendLayout();
            // 
            // parametersList
            // 
            this.parametersList.AllowUserToAddRows = false;
            this.parametersList.AllowUserToDeleteRows = false;
            this.parametersList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.parametersList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.parametersList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Key,
            this.ValueCol,
            this.DefaultCol,
            this.DescriptionCol});
            this.parametersList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.parametersList.Location = new System.Drawing.Point(0, 0);
            this.parametersList.Name = "parametersList";
            this.parametersList.RowHeadersWidth = 4;
            this.parametersList.Size = new System.Drawing.Size(664, 435);
            this.parametersList.TabIndex = 0;
            // 
            // Key
            // 
            this.Key.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Key.HeaderText = "Key";
            this.Key.Name = "Key";
            this.Key.ReadOnly = true;
            this.Key.Width = 50;
            // 
            // ValueCol
            // 
            this.ValueCol.HeaderText = "Value";
            this.ValueCol.Name = "ValueCol";
            this.ValueCol.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // DefaultCol
            // 
            this.DefaultCol.HeaderText = "Default";
            this.DefaultCol.Name = "DefaultCol";
            this.DefaultCol.ReadOnly = true;
            // 
            // DescriptionCol
            // 
            this.DescriptionCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.DescriptionCol.HeaderText = "Description";
            this.DescriptionCol.Name = "DescriptionCol";
            this.DescriptionCol.ReadOnly = true;
            // 
            // ConfigurationObjectControlPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.parametersList);
            this.Name = "ConfigurationObjectControlPanel";
            this.Size = new System.Drawing.Size(664, 435);
            ((System.ComponentModel.ISupportInitialize)(this.parametersList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView parametersList;
        private System.Windows.Forms.DataGridViewTextBoxColumn Key;
        private System.Windows.Forms.DataGridViewTextBoxColumn ValueCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn DefaultCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn DescriptionCol;

    }
}

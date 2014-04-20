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
            this.valueColumn1 = new Chimera.ConfigurationTool.Controls.ValueColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Key = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ValueCol = new Chimera.ConfigurationTool.Controls.ValueColumn();
            this.DefaultCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SectionCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.SectionCol,
            this.DescriptionCol});
            this.parametersList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.parametersList.Location = new System.Drawing.Point(0, 0);
            this.parametersList.Name = "parametersList";
            this.parametersList.RowHeadersWidth = 4;
            this.parametersList.Size = new System.Drawing.Size(664, 435);
            this.parametersList.TabIndex = 0;
            // 
            // valueColumn1
            // 
            this.valueColumn1.DataPropertyName = "Value";
            this.valueColumn1.HeaderText = "Value";
            this.valueColumn1.Name = "valueColumn1";
            this.valueColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Default";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn2.HeaderText = "Section";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn3.HeaderText = "Description";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // Key
            // 
            this.Key.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Key.HeaderText = "Key";
            this.Key.Name = "Key";
            this.Key.ReadOnly = true;
            this.Key.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Key.Width = 50;
            // 
            // ValueCol
            // 
            this.ValueCol.DataPropertyName = "Value";
            this.ValueCol.HeaderText = "Value";
            this.ValueCol.Name = "ValueCol";
            this.ValueCol.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ValueCol.Width = 450;
            // 
            // DefaultCol
            // 
            this.DefaultCol.HeaderText = "Default";
            this.DefaultCol.Name = "DefaultCol";
            this.DefaultCol.ReadOnly = true;
            // 
            // SectionCol
            // 
            this.SectionCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.SectionCol.HeaderText = "Section";
            this.SectionCol.Name = "SectionCol";
            this.SectionCol.ReadOnly = true;
            this.SectionCol.Width = 68;
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
        private ValueColumn valueColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Key;
        private ValueColumn ValueCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn DefaultCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn SectionCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn DescriptionCol;
        //private System.Windows.Forms.DataGridViewTextBoxColumn ValueCol;

    }
}

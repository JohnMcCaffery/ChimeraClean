namespace ConfigurationTool {
    partial class BindingsControlPanel {
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
            this.BindingsList = new System.Windows.Forms.ListView();
            this.BoundCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.InterfaceCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.NameCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DiscriptionCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.loadButton = new System.Windows.Forms.Button();
            this.loadFileButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BindingsList
            // 
            this.BindingsList.CheckBoxes = true;
            this.BindingsList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.BoundCol,
            this.NameCol,
            this.InterfaceCol,
            this.DiscriptionCol});
            this.BindingsList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BindingsList.FullRowSelect = true;
            this.BindingsList.GridLines = true;
            this.BindingsList.Location = new System.Drawing.Point(0, 0);
            this.BindingsList.Name = "BindingsList";
            this.BindingsList.Size = new System.Drawing.Size(695, 341);
            this.BindingsList.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.BindingsList.TabIndex = 0;
            this.BindingsList.UseCompatibleStateImageBehavior = false;
            this.BindingsList.View = System.Windows.Forms.View.Details;
            this.BindingsList.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.BindingsList_ItemChecked);
            // 
            // BoundCol
            // 
            this.BoundCol.Text = "Bind";
            this.BoundCol.Width = 33;
            // 
            // InterfaceCol
            // 
            this.InterfaceCol.Text = "Interface";
            this.InterfaceCol.Width = 156;
            // 
            // NameCol
            // 
            this.NameCol.Text = "Name";
            this.NameCol.Width = 182;
            // 
            // DiscriptionCol
            // 
            this.DiscriptionCol.Text = "Description";
            this.DiscriptionCol.Width = 396;
            // 
            // loadButton
            // 
            this.loadButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.loadButton.Location = new System.Drawing.Point(617, 3);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(75, 23);
            this.loadButton.TabIndex = 1;
            this.loadButton.Text = "Load";
            this.loadButton.UseVisualStyleBackColor = true;
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // loadFileButton
            // 
            this.loadFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.loadFileButton.Location = new System.Drawing.Point(617, 32);
            this.loadFileButton.Name = "loadFileButton";
            this.loadFileButton.Size = new System.Drawing.Size(75, 23);
            this.loadFileButton.TabIndex = 2;
            this.loadFileButton.Text = "Load File";
            this.loadFileButton.UseVisualStyleBackColor = true;
            this.loadFileButton.Click += new System.EventHandler(this.loadFileButton_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(617, 61);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Save File";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // BindingsControlPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.loadFileButton);
            this.Controls.Add(this.loadButton);
            this.Controls.Add(this.BindingsList);
            this.Name = "BindingsControlPanel";
            this.Size = new System.Drawing.Size(695, 341);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView BindingsList;
        private System.Windows.Forms.ColumnHeader BoundCol;
        private System.Windows.Forms.ColumnHeader InterfaceCol;
        private System.Windows.Forms.ColumnHeader NameCol;
        private System.Windows.Forms.ColumnHeader DiscriptionCol;
        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.Button loadFileButton;
        private System.Windows.Forms.Button button1;
    }
}

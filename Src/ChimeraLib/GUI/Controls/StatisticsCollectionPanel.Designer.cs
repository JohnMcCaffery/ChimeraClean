namespace Chimera.GUI.Controls {
    partial class StatisticsCollectionPanel {
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.mainTab = new System.Windows.Forms.TabControl();
            this.graphsTab = new System.Windows.Forms.TabPage();
            this.sharedGraph = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.valuesTab = new System.Windows.Forms.TabPage();
            this.statsList = new System.Windows.Forms.ListView();
            this.nameCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.meanCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.currentCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.mainTab.SuspendLayout();
            this.graphsTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sharedGraph)).BeginInit();
            this.valuesTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainTab
            // 
            this.mainTab.Controls.Add(this.graphsTab);
            this.mainTab.Controls.Add(this.valuesTab);
            this.mainTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTab.Location = new System.Drawing.Point(0, 0);
            this.mainTab.Name = "mainTab";
            this.mainTab.SelectedIndex = 0;
            this.mainTab.Size = new System.Drawing.Size(720, 479);
            this.mainTab.TabIndex = 0;
            this.mainTab.SelectedIndexChanged += new System.EventHandler(this.mainTab_SelectedIndexChanged);
            // 
            // graphsTab
            // 
            this.graphsTab.Controls.Add(this.sharedGraph);
            this.graphsTab.Location = new System.Drawing.Point(4, 22);
            this.graphsTab.Name = "graphsTab";
            this.graphsTab.Padding = new System.Windows.Forms.Padding(3);
            this.graphsTab.Size = new System.Drawing.Size(712, 453);
            this.graphsTab.TabIndex = 0;
            this.graphsTab.Text = "Graphs";
            this.graphsTab.UseVisualStyleBackColor = true;
            // 
            // sharedGraph
            // 
            chartArea2.Name = "ChartArea1";
            this.sharedGraph.ChartAreas.Add(chartArea2);
            this.sharedGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            legend2.Name = "Legend1";
            this.sharedGraph.Legends.Add(legend2);
            this.sharedGraph.Location = new System.Drawing.Point(3, 3);
            this.sharedGraph.Name = "sharedGraph";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.sharedGraph.Series.Add(series2);
            this.sharedGraph.Size = new System.Drawing.Size(706, 447);
            this.sharedGraph.TabIndex = 0;
            this.sharedGraph.Text = "sharedGraph";
            // 
            // valuesTab
            // 
            this.valuesTab.Controls.Add(this.statsList);
            this.valuesTab.Location = new System.Drawing.Point(4, 22);
            this.valuesTab.Name = "valuesTab";
            this.valuesTab.Padding = new System.Windows.Forms.Padding(3);
            this.valuesTab.Size = new System.Drawing.Size(712, 453);
            this.valuesTab.TabIndex = 1;
            this.valuesTab.Text = "Values";
            this.valuesTab.UseVisualStyleBackColor = true;
            // 
            // statsList
            // 
            this.statsList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nameCol,
            this.meanCol,
            this.currentCol});
            this.statsList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statsList.FullRowSelect = true;
            this.statsList.GridLines = true;
            this.statsList.Location = new System.Drawing.Point(3, 3);
            this.statsList.Name = "statsList";
            this.statsList.Size = new System.Drawing.Size(706, 447);
            this.statsList.TabIndex = 0;
            this.statsList.UseCompatibleStateImageBehavior = false;
            this.statsList.View = System.Windows.Forms.View.Details;
            // 
            // nameCol
            // 
            this.nameCol.Text = "Name";
            // 
            // meanCol
            // 
            this.meanCol.Text = "Mean";
            // 
            // currentCol
            // 
            this.currentCol.Text = "Current";
            // 
            // StatisticsCollectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(720, 479);
            this.Controls.Add(this.mainTab);
            this.Name = "StatisticsCollectionForm";
            this.Text = "Statistics Collection";
            this.mainTab.ResumeLayout(false);
            this.graphsTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sharedGraph)).EndInit();
            this.valuesTab.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl mainTab;
        private System.Windows.Forms.TabPage graphsTab;
        private System.Windows.Forms.TabPage valuesTab;
        private System.Windows.Forms.ListView statsList;
        private System.Windows.Forms.DataVisualization.Charting.Chart sharedGraph;
        private System.Windows.Forms.ColumnHeader nameCol;
        private System.Windows.Forms.ColumnHeader meanCol;
        private System.Windows.Forms.ColumnHeader currentCol;
    }
}
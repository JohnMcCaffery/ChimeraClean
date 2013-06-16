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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.mainTab = new System.Windows.Forms.TabControl();
            this.graphsTab = new System.Windows.Forms.TabPage();
            this.sharedGraph = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.valuesTab = new System.Windows.Forms.TabPage();
            this.statsList = new System.Windows.Forms.ListView();
            this.nameCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.meanCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.currentCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.individualTab = new System.Windows.Forms.TabPage();
            this.individualSplit = new System.Windows.Forms.SplitContainer();
            this.namesList = new System.Windows.Forms.ListBox();
            this.updateTimer = new System.Windows.Forms.Timer(this.components);
            this.tickCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ticksPerSecond = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.mainTab.SuspendLayout();
            this.graphsTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sharedGraph)).BeginInit();
            this.valuesTab.SuspendLayout();
            this.individualTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.individualSplit)).BeginInit();
            this.individualSplit.Panel1.SuspendLayout();
            this.individualSplit.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainTab
            // 
            this.mainTab.Controls.Add(this.graphsTab);
            this.mainTab.Controls.Add(this.valuesTab);
            this.mainTab.Controls.Add(this.individualTab);
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
            chartArea1.Name = "ChartArea1";
            this.sharedGraph.ChartAreas.Add(chartArea1);
            this.sharedGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.sharedGraph.Legends.Add(legend1);
            this.sharedGraph.Location = new System.Drawing.Point(3, 3);
            this.sharedGraph.Name = "sharedGraph";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.sharedGraph.Series.Add(series1);
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
            this.currentCol,
            this.tickCount,
            this.ticksPerSecond});
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
            this.nameCol.Width = 168;
            // 
            // meanCol
            // 
            this.meanCol.Text = "Mean";
            this.meanCol.Width = 40;
            // 
            // currentCol
            // 
            this.currentCol.Text = "Current";
            this.currentCol.Width = 46;
            // 
            // individualTab
            // 
            this.individualTab.Controls.Add(this.individualSplit);
            this.individualTab.Location = new System.Drawing.Point(4, 22);
            this.individualTab.Name = "individualTab";
            this.individualTab.Padding = new System.Windows.Forms.Padding(3);
            this.individualTab.Size = new System.Drawing.Size(712, 453);
            this.individualTab.TabIndex = 2;
            this.individualTab.Text = "Individual Breakdowns";
            this.individualTab.UseVisualStyleBackColor = true;
            // 
            // individualSplit
            // 
            this.individualSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.individualSplit.Location = new System.Drawing.Point(3, 3);
            this.individualSplit.Name = "individualSplit";
            // 
            // individualSplit.Panel1
            // 
            this.individualSplit.Panel1.Controls.Add(this.namesList);
            this.individualSplit.Size = new System.Drawing.Size(706, 447);
            this.individualSplit.SplitterDistance = 235;
            this.individualSplit.TabIndex = 2;
            // 
            // namesList
            // 
            this.namesList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.namesList.FormattingEnabled = true;
            this.namesList.Location = new System.Drawing.Point(0, 0);
            this.namesList.Name = "namesList";
            this.namesList.Size = new System.Drawing.Size(235, 447);
            this.namesList.TabIndex = 0;
            this.namesList.SelectedIndexChanged += new System.EventHandler(this.namesList_SelectedIndexChanged);
            // 
            // updateTimer
            // 
            this.updateTimer.Tick += new System.EventHandler(this.updateTimer_Tick);
            // 
            // tickCount
            // 
            this.tickCount.Text = "Tick Count";
            this.tickCount.Width = 68;
            // 
            // ticksPerSecond
            // 
            this.ticksPerSecond.Text = "Ticks/s";
            // 
            // StatisticsCollectionPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainTab);
            this.Name = "StatisticsCollectionPanel";
            this.Size = new System.Drawing.Size(720, 479);
            this.mainTab.ResumeLayout(false);
            this.graphsTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sharedGraph)).EndInit();
            this.valuesTab.ResumeLayout(false);
            this.individualTab.ResumeLayout(false);
            this.individualSplit.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.individualSplit)).EndInit();
            this.individualSplit.ResumeLayout(false);
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
        private System.Windows.Forms.TabPage individualTab;
        private System.Windows.Forms.ListBox namesList;
        private System.Windows.Forms.SplitContainer individualSplit;
        private System.Windows.Forms.Timer updateTimer;
        private System.Windows.Forms.ColumnHeader tickCount;
        private System.Windows.Forms.ColumnHeader ticksPerSecond;
    }
}

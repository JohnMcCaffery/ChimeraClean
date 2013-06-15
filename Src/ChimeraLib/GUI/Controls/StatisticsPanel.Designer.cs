namespace Chimera.GUI.Controls {
    partial class StatisticsPanel {
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.tickCountLabel = new System.Windows.Forms.Label();
            this.shortestWorkLabel = new System.Windows.Forms.Label();
            this.longestWorkLabel = new System.Windows.Forms.Label();
            this.meanWorkLabel = new System.Windows.Forms.Label();
            this.longestTickLabel = new System.Windows.Forms.Label();
            this.shortestTickLabel = new System.Windows.Forms.Label();
            this.meanTickLabel = new System.Windows.Forms.Label();
            this.tpsLabel = new System.Windows.Forms.Label();
            this.tickDeviation = new System.Windows.Forms.Label();
            this.workDeviation = new System.Windows.Forms.Label();
            this.statsTabs = new System.Windows.Forms.TabControl();
            this.valuesTab = new System.Windows.Forms.TabPage();
            this.graphTab = new System.Windows.Forms.TabPage();
            this.statsChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.statsTabs.SuspendLayout();
            this.valuesTab.SuspendLayout();
            this.graphTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.statsChart)).BeginInit();
            this.SuspendLayout();
            // 
            // tickCountLabel
            // 
            this.tickCountLabel.AutoSize = true;
            this.tickCountLabel.Location = new System.Drawing.Point(3, 16);
            this.tickCountLabel.Name = "tickCountLabel";
            this.tickCountLabel.Size = new System.Drawing.Size(59, 13);
            this.tickCountLabel.TabIndex = 15;
            this.tickCountLabel.Text = "Tick Count";
            // 
            // shortestWorkLabel
            // 
            this.shortestWorkLabel.AutoSize = true;
            this.shortestWorkLabel.Location = new System.Drawing.Point(3, 71);
            this.shortestWorkLabel.Name = "shortestWorkLabel";
            this.shortestWorkLabel.Size = new System.Drawing.Size(75, 13);
            this.shortestWorkLabel.TabIndex = 14;
            this.shortestWorkLabel.Text = "Shortest Work";
            // 
            // longestWorkLabel
            // 
            this.longestWorkLabel.AutoSize = true;
            this.longestWorkLabel.Location = new System.Drawing.Point(3, 58);
            this.longestWorkLabel.Name = "longestWorkLabel";
            this.longestWorkLabel.Size = new System.Drawing.Size(74, 13);
            this.longestWorkLabel.TabIndex = 13;
            this.longestWorkLabel.Text = "Longest Work";
            // 
            // meanWorkLabel
            // 
            this.meanWorkLabel.AutoSize = true;
            this.meanWorkLabel.Location = new System.Drawing.Point(3, 45);
            this.meanWorkLabel.Name = "meanWorkLabel";
            this.meanWorkLabel.Size = new System.Drawing.Size(99, 13);
            this.meanWorkLabel.TabIndex = 12;
            this.meanWorkLabel.Text = "Mean Work Length";
            // 
            // longestTickLabel
            // 
            this.longestTickLabel.AutoSize = true;
            this.longestTickLabel.Location = new System.Drawing.Point(3, 128);
            this.longestTickLabel.Name = "longestTickLabel";
            this.longestTickLabel.Size = new System.Drawing.Size(69, 13);
            this.longestTickLabel.TabIndex = 11;
            this.longestTickLabel.Text = "Longest Tick";
            // 
            // shortestTickLabel
            // 
            this.shortestTickLabel.AutoSize = true;
            this.shortestTickLabel.Location = new System.Drawing.Point(3, 141);
            this.shortestTickLabel.Name = "shortestTickLabel";
            this.shortestTickLabel.Size = new System.Drawing.Size(70, 13);
            this.shortestTickLabel.TabIndex = 10;
            this.shortestTickLabel.Text = "Shortest Tick";
            // 
            // meanTickLabel
            // 
            this.meanTickLabel.AutoSize = true;
            this.meanTickLabel.Location = new System.Drawing.Point(3, 115);
            this.meanTickLabel.Name = "meanTickLabel";
            this.meanTickLabel.Size = new System.Drawing.Size(94, 13);
            this.meanTickLabel.TabIndex = 9;
            this.meanTickLabel.Text = "Mean Tick Length";
            // 
            // tpsLabel
            // 
            this.tpsLabel.AutoSize = true;
            this.tpsLabel.Location = new System.Drawing.Point(3, 3);
            this.tpsLabel.Name = "tpsLabel";
            this.tpsLabel.Size = new System.Drawing.Size(81, 13);
            this.tpsLabel.TabIndex = 8;
            this.tpsLabel.Text = "Ticks / Second";
            // 
            // tickDeviation
            // 
            this.tickDeviation.AutoSize = true;
            this.tickDeviation.Location = new System.Drawing.Point(3, 154);
            this.tickDeviation.Name = "tickDeviation";
            this.tickDeviation.Size = new System.Drawing.Size(95, 13);
            this.tickDeviation.TabIndex = 16;
            this.tickDeviation.Text = "Tick Std Deviation";
            // 
            // workDeviation
            // 
            this.workDeviation.AutoSize = true;
            this.workDeviation.Location = new System.Drawing.Point(3, 84);
            this.workDeviation.Name = "workDeviation";
            this.workDeviation.Size = new System.Drawing.Size(100, 13);
            this.workDeviation.TabIndex = 17;
            this.workDeviation.Text = "Work Std Deviation";
            // 
            // statsTabs
            // 
            this.statsTabs.Controls.Add(this.valuesTab);
            this.statsTabs.Controls.Add(this.graphTab);
            this.statsTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statsTabs.Location = new System.Drawing.Point(0, 0);
            this.statsTabs.Name = "statsTabs";
            this.statsTabs.SelectedIndex = 0;
            this.statsTabs.Size = new System.Drawing.Size(667, 269);
            this.statsTabs.TabIndex = 18;
            // 
            // valuesTab
            // 
            this.valuesTab.Controls.Add(this.tpsLabel);
            this.valuesTab.Controls.Add(this.workDeviation);
            this.valuesTab.Controls.Add(this.meanTickLabel);
            this.valuesTab.Controls.Add(this.tickDeviation);
            this.valuesTab.Controls.Add(this.shortestTickLabel);
            this.valuesTab.Controls.Add(this.tickCountLabel);
            this.valuesTab.Controls.Add(this.longestTickLabel);
            this.valuesTab.Controls.Add(this.shortestWorkLabel);
            this.valuesTab.Controls.Add(this.meanWorkLabel);
            this.valuesTab.Controls.Add(this.longestWorkLabel);
            this.valuesTab.Location = new System.Drawing.Point(4, 22);
            this.valuesTab.Name = "valuesTab";
            this.valuesTab.Padding = new System.Windows.Forms.Padding(3);
            this.valuesTab.Size = new System.Drawing.Size(659, 243);
            this.valuesTab.TabIndex = 0;
            this.valuesTab.Text = "Values";
            this.valuesTab.UseVisualStyleBackColor = true;
            // 
            // graphTab
            // 
            this.graphTab.Controls.Add(this.statsChart);
            this.graphTab.Location = new System.Drawing.Point(4, 22);
            this.graphTab.Name = "graphTab";
            this.graphTab.Padding = new System.Windows.Forms.Padding(3);
            this.graphTab.Size = new System.Drawing.Size(659, 243);
            this.graphTab.TabIndex = 1;
            this.graphTab.Text = "Graph";
            this.graphTab.UseVisualStyleBackColor = true;
            // 
            // statsChart
            // 
            chartArea1.Name = "ChartArea1";
            this.statsChart.ChartAreas.Add(chartArea1);
            this.statsChart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.statsChart.Legends.Add(legend1);
            this.statsChart.Location = new System.Drawing.Point(3, 3);
            this.statsChart.Name = "statsChart";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.statsChart.Series.Add(series1);
            this.statsChart.Size = new System.Drawing.Size(653, 237);
            this.statsChart.TabIndex = 0;
            this.statsChart.Text = "Statistics Chart";
            // 
            // StatisticsPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.statsTabs);
            this.Name = "StatisticsPanel";
            this.Size = new System.Drawing.Size(667, 269);
            this.statsTabs.ResumeLayout(false);
            this.valuesTab.ResumeLayout(false);
            this.valuesTab.PerformLayout();
            this.graphTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.statsChart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label tickCountLabel;
        private System.Windows.Forms.Label shortestWorkLabel;
        private System.Windows.Forms.Label longestWorkLabel;
        private System.Windows.Forms.Label meanWorkLabel;
        private System.Windows.Forms.Label longestTickLabel;
        private System.Windows.Forms.Label shortestTickLabel;
        private System.Windows.Forms.Label meanTickLabel;
        private System.Windows.Forms.Label tpsLabel;
        private System.Windows.Forms.Label tickDeviation;
        private System.Windows.Forms.Label workDeviation;
        private System.Windows.Forms.TabControl statsTabs;
        private System.Windows.Forms.TabPage valuesTab;
        private System.Windows.Forms.TabPage graphTab;
        private System.Windows.Forms.DataVisualization.Charting.Chart statsChart;
    }
}

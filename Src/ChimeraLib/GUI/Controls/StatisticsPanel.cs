using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chimera.Util;
using System.Windows.Forms.DataVisualization.Charting;

namespace Chimera.GUI.Controls {
    public partial class StatisticsPanel : UserControl {
        private bool mActive;
        private bool mShowTick = true;
        private TickStatistics mStats;
        private Action mTickListener;
        private Core mCore;

        private Series mValues = new Series("Values", 100);
        private Series mMins = new Series("Min", 100);
        private Series mMaxs = new Series("Max", 100);
        private Series mMeans = new Series("Mean", 100);

        private DateTime mStart;

        public bool Active {
            get { return mActive; }
            set {
                if (mActive != value) {
                    mActive = value;
                    if (mCore != null) {
                        if (value)
                            Start();
                        else
                            mCore.Tick -= mTickListener;
                    }
                }
            }
        }

        public bool ShowTick {
            get { return mShowTick; }
            set {
                mShowTick = value;
                Invoke(() => {
                    meanTickLabel.Visible = value;
                    longestTickLabel.Visible = value;
                    shortestTickLabel.Visible = value;
                    tickDeviation.Visible = value;
                });
            }
        }

        public StatisticsPanel() {
            InitializeComponent();
            mTickListener = new Action(mCore_Tick);
        }

        public StatisticsPanel(TickStatistics stats, Core core) {
            Init(stats, core);
        }

        public void Init(TickStatistics stats, Core core) {
            mStats = stats;
            mCore = core;
            if (mActive)
                mCore.Tick += mTickListener;
        }

        private void mCore_Tick() {
            Invoke(() => {
                tpsLabel.Text = "Ticks / Second: " + mStats.TicksPerSecond;
                tickCountLabel.Text = "Tick Count: " + mStats.TickCount;

                meanTickLabel.Text = "Mean Tick Length: " + mStats.MeanTickLength;
                longestTickLabel.Text = "Longest Tick: " + mStats.LongestTick;
                shortestTickLabel.Text = "Shortest Tick: " + (mStats.ShortestTick == double.MaxValue ? -1.0 : mStats.ShortestTick);
                tickDeviation.Text = "Tick Std Deviation: " + mStats.TickStandardDeviation;

                meanWorkLabel.Text = "Mean Work Length: " + mStats.MeanWorkLength;
                longestWorkLabel.Text = "Longest Work: " + mStats.LongestWork;
                shortestWorkLabel.Text = "Shortest Work: " + (mStats.ShortestTick == double.MaxValue ? -1.0 : mStats.ShortestTick);
                workDeviation.Text = "Work Std Deviation: " + mStats.WorkStandardDeviation;

                double t = Math.Round(DateTime.Now.Subtract(mStart).TotalSeconds, 2);
                mValues.Points.Add(new DataPoint(t, mStats.LastWork));
                mMeans.Points.Add(new DataPoint(t, mStats.MeanWorkLength));
                mMins.Points.Add(new DataPoint(t, mStats.ShortestWork));
                if (mStats.LongestWork < mStats.MeanWorkLength * 5)
                    mMaxs.Points.Add(new DataPoint(t, mStats.LongestWork));
            });
        }

        private void Invoke(Action a) {
            if (!InvokeRequired)
                a();
            else if (!IsDisposed && !Disposing && Created)
                base.BeginInvoke(a);
        }

        private void Start() {
            mCore.Tick += mTickListener;
            mStart = DateTime.Now;

            mValues = new Series("Values", 100);
            mMins = new Series("Min", 100);
            mMaxs = new Series("Max", 100);
            mMeans = new Series("Mean", 100);

            mValues.ChartType = SeriesChartType.Line;
            mMins.ChartType = SeriesChartType.Line;
            mMaxs.ChartType = SeriesChartType.Line;
            mMeans.ChartType = SeriesChartType.Line;

            statsChart.Series.Clear();
            statsChart.Series.Add(mValues);
            statsChart.Series.Add(mMins);
            statsChart.Series.Add(mMeans);
            statsChart.Series.Add(mMaxs);
        }
    }
}

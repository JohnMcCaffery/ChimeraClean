using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chimera.Util;
using System.Windows.Forms.DataVisualization.Charting;
using Chimera.GUI.Controls;

namespace Chimera.GUI.Controls {
    public partial class StatisticsCollectionPanel : UserControl {
        private readonly Dictionary<string, Series> mCurrentSeries = new Dictionary<string, Series>();
        private readonly Dictionary<string, Series> mMeanSeries = new Dictionary<string, Series>();
        private readonly Dictionary<string, ListViewItem> mRows = new Dictionary<string, ListViewItem>();
        private readonly Dictionary<string, StatisticsPanel> mPanels = new Dictionary<string, StatisticsPanel>();
        private readonly Action mTickListener;

        private StatisticsCollection mCollection;
        private Core mCore;

        private StatisticsPanel mCurrentPanel;
        private DateTime mStart = DateTime.UtcNow;
        private bool mActive;

        public bool Active {
            get { return mActive; }
            set {
                if (mActive != value) {
                    mActive = value;
                    if (value) {
                        updateTimer.Enabled = true;
                        //mCore.Tick += mTickListener;
                        if (mCurrentPanel != null)
                            mCurrentPanel.Active = true;
                    }  else {
                        //mCore.Tick -= mTickListener;
                        updateTimer.Enabled = false;
                        if (mCurrentPanel != null)
                            mCurrentPanel.Active = false;
                    }
                }
            }
        }

        public StatisticsCollectionPanel() {
            InitializeComponent();
            mTickListener = new Action(core_Tick);
        }

        public StatisticsCollectionPanel(StatisticsCollection collection, Core core)
            : this() {

            Init(collection, core);
        }

        public void Init(StatisticsCollection collection, Core core) {
            mCore = core; 
            mCollection = collection;

            foreach (var name in collection.StatisticsNames) {
                Series current = new Series(name, 100);
                Series mean = new Series(name + " Mean", 100);

                current.ChartType = SeriesChartType.Line;
                mean.ChartType = SeriesChartType.Line;

                sharedGraph.Series.Add(current);
                sharedGraph.Series.Add(mean);

                namesList.Items.Add(name);
                
                ListViewItem row = new ListViewItem(name);
                //row.SubItems.Add(name);
                row.SubItems.Add("0");
                row.SubItems.Add("0");
                row.SubItems.Add("0");
                row.SubItems.Add("0");
                statsList.Items.Add(row);

                mCurrentSeries.Add(name, current);
                mMeanSeries.Add(name, mean);
                mRows.Add(name, row);
            }
        }

        void core_Tick() {
            if (mainTab.SelectedTab == graphsTab) {
                foreach (var name in mCollection.StatisticsNames) {
                    TickStatistics stats = mCollection[name];
                    double s = Math.Round(DateTime.UtcNow.Subtract(mStart).TotalSeconds, 2);
                    mCurrentSeries[name].Points.Add(new DataPoint(s, stats.LastWork));
                    mMeanSeries[name].Points.Add(new DataPoint(s, stats.MeanWorkLength));
                }
            } else if (mainTab.SelectedTab == valuesTab) {
                foreach (var name in mCollection.StatisticsNames) {
                    TickStatistics stats = mCollection[name];
                    double s = Math.Round(DateTime.UtcNow.Subtract(mStart).TotalSeconds, 2);
                    mRows[name].SubItems[1].Text = stats.MeanWorkLength.ToString(".##");
                    mRows[name].SubItems[2].Text = stats.LastWork.ToString(".##");
                    mRows[name].SubItems[3].Text = stats.TickCount.ToString();
                    mRows[name].SubItems[4].Text = stats.TicksPerSecond.ToString();
                }
            }
        }

        private void mainTab_SelectedIndexChanged(object sender, EventArgs e) {
            if (mCurrentPanel != null) {
                mCurrentPanel.Active = false;
                mCurrentPanel = null;
            }

            if (mainTab.SelectedTab == graphsTab) {
                //mCore.Tick -= mTickListener;
                updateTimer.Enabled = false;

                foreach (var series in mCurrentSeries.Values)
                    series.Points.Clear();
                foreach (var series in mMeanSeries.Values)
                    series.Points.Clear();

                mStart = DateTime.UtcNow;

                //mCore.Tick += mTickListener;
                updateTimer.Enabled = true;
            } else if (mainTab.SelectedTab == individualTab && mCurrentPanel != null)
                mCurrentPanel.Active = true;
        }

        private void namesList_SelectedIndexChanged(object sender, EventArgs e) {
            if (mCurrentPanel != null) {
                mCurrentPanel.Visible = false;
                mCurrentPanel.Active = false;
                mCurrentPanel = null;
            }
            if (namesList.SelectedItem != null) {
                string name = namesList.SelectedItem as String;
                if (!mPanels.ContainsKey(name)) {
                    StatisticsPanel panel = new StatisticsPanel(mCollection[name], mCore);
                    mPanels.Add(name, panel);
                    panel.Dock = DockStyle.Fill;
                    individualSplit.Panel2.Controls.Add(panel);
                }

                mCurrentPanel = mPanels[name];
                mCurrentPanel.Active = true;
                mCurrentPanel.Visible = true;
            }
        }

        private void updateTimer_Tick(object sender, EventArgs e) {
            core_Tick();
        }
    }
}

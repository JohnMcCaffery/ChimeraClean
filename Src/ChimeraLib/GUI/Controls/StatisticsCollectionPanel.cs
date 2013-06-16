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
        private DateTime mStart = DateTime.Now;
        private bool mActive;

        public bool Active {
            get { return mActive; }
            set {
                if (mActive != value) {
                    mActive = value;
                    if (value) {
                        mCore.Tick += mTickListener;
                        if (mCurrentPanel != null)
                            mCurrentPanel.Active = true;
                    }  else {
                        mCore.Tick -= mTickListener;
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

                sharedGraph.Series.Add(current);
                sharedGraph.Series.Add(mean);

                TabPage p = new TabPage();
                p.Name = name;
                StatisticsPanel panel = new StatisticsPanel(collection[name], mCore);
                p.Controls.Add(panel);
                mainTab.Controls.Add(p);
                
                ListViewItem row = new ListViewItem();
                row.SubItems.Add(name);
                row.SubItems.Add("0");
                row.SubItems.Add("0");
                statsList.Items.Add(row);

                mCurrentSeries.Add(name, current);
                mMeanSeries.Add(name, mean);
                mRows.Add(name, row);
                mPanels.Add(name, panel);
            }
        }

        void core_Tick() {
            if (mainTab.SelectedTab == graphsTab) {
                foreach (var name in mCollection.StatisticsNames) {
                    TickStatistics stats = mCollection[name];
                    double s = Math.Round(DateTime.Now.Subtract(mStart).TotalSeconds, 2);
                    mCurrentSeries[name].Points.Add(new DataPoint(s, stats.LastWork));
                    mMeanSeries[name].Points.Add(new DataPoint(s, stats.MeanWorkLength));
                }
            } else if (mainTab.SelectedTab == valuesTab) {
                foreach (var name in mCollection.StatisticsNames) {
                    TickStatistics stats = mCollection[name];
                    double s = Math.Round(DateTime.Now.Subtract(mStart).TotalSeconds, 2);
                    mRows[name].SubItems[1].Text = stats.MeanWorkLength.ToString(".##");
                    mRows[name].SubItems[2].Text = stats.LastWork.ToString(".##");
                }
            }
        }

        private void mainTab_SelectedIndexChanged(object sender, EventArgs e) {
            if (mCurrentPanel != null) {
                mCurrentPanel.Active = false;
                mCurrentPanel = null;
            }

            if (mainTab.SelectedTab == graphsTab) {
                mStart = DateTime.Now;
            } else if (mPanels.ContainsKey(mainTab.SelectedTab.Name)) {
                StatisticsPanel panel = mPanels[mainTab.SelectedTab.Name];
                panel.Active = true;
                mCurrentPanel = panel;
            }
        }
    }
}

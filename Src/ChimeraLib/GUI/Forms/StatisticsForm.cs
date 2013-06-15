using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chimera.Util;

namespace Chimera.GUI.Forms {
    public partial class StatisticsForm : Form {
        public bool ShowTick {
            get { return statisticsPanel.ShowTick; }
            set { statisticsPanel.ShowTick = value; }
        }

        public StatisticsForm() {
            InitializeComponent();
        }

        public StatisticsForm(Core core, TickStatistics stats) : this() {
            statisticsPanel.Init(stats, core);
        }

        private void StatisticsForm_FormClosing(object sender, FormClosingEventArgs e) {
            statisticsPanel.Active = false;
        }

        private void StatisticsForm_Load(object sender, EventArgs e) {
            statisticsPanel.Active = true;
        }
    }
}

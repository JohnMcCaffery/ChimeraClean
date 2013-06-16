using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chimera.Util {
    public class StatisticsCollection {
        #region Static 

        private static StatisticsCollection sCol = new StatisticsCollection();

        public static StatisticsCollection Collection {
            get { return sCol; }
        }

        public static string[] Names {
            get { return sCol.mStatistics.Keys.ToArray(); }
        }

        public static TickStatistics GetStats(string name) {
            return sCol[name];
        }

        public static void AddStatistics(TickStatistics statistics, string name) {
            sCol.AddStatisticsl(statistics, name);
        }

        #endregion

        #region

        private readonly Dictionary<string, TickStatistics> mStatistics = new Dictionary<string, TickStatistics>();

        public string[] StatisticsNames {
            get { return mStatistics.Keys.ToArray(); }
        }

        public TickStatistics this[string name] {
            get { return mStatistics[name]; }
        }

        private void AddStatisticsl(TickStatistics statistics, string name) {
            mStatistics.Add(name, statistics);
        }

        #endregion
    }
}

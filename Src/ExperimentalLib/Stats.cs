using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Config;
using System.Globalization;

namespace Chimera.Experimental {
    public class Stats {
        public readonly DateTime TimeStamp;
        protected readonly ExperimentalConfig mConfig;

        public Stats(DateTime timestamp, ExperimentalConfig config) {
            mConfig = config;

            TimeStamp = timestamp;
        }

        public Stats(string line, ExperimentalConfig config) {
            string[] s = line.Split(',');
            int frames = new CoreConfig().Frames.Length;

            mConfig = config;
            TimeStamp = DateTime.ParseExact(line.Split(',')[0], mConfig.TimestampFormat, new DateTimeFormatInfo());
        }

        public string ToString(int count) {
            return TimeStamp.ToString(mConfig.TimestampFormat) + "," + (TimeStamp - mConfig.Timestamp).TotalMilliseconds + "," + count + ",";
        }
    }
}

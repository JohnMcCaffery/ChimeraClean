using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chimera.Util {
    public class TickStatistics {
        private Queue<DateTime> mTickTimes = new Queue<DateTime>();
        private bool mStarted;
        private DateTime mLastTick;
        private DateTime mTickStart;
        private long mWorkTotal;
        private long mTickTotal;
        private int mTickCount;
        private double mShortestTick = double.MaxValue;
        private double mShortestWork = double.MaxValue;
        private double mLongestTick = double.MinValue;
        private double mLongestWork = double.MinValue;

        public int TickCount {
            get { lock (mTickTimes) return mTickCount; }
        }

        public long MeanTickLength {
            get { lock (mTickTimes) return mTickCount > 1 ? mTickTotal / (mTickCount -1) : 0; }
        }

        public double LongestTick {
            get { return mLongestTick; }
        }

        public double ShortestTick {
            get { return mShortestTick; }
        }

        public long MeanWorkLength {
            get { lock (mTickTimes) return mTickCount > 0 ? mWorkTotal / mTickCount : 0; }
        }

        public double LongestWork {
            get { return mLongestWork; }
        }

        public double ShortestWork {
            get { return mShortestWork; }
        }

        public int TicksPerSecond {
            get { lock (mTickTimes) return mTickTimes.Count; }
        }

        public void Begin() {
            mTickStart = DateTime.Now;
        }
        public void Tick() {
            lock (mTickTimes) {
                if (mStarted) {
                    double tickLength = DateTime.Now.Subtract(mLastTick).TotalMilliseconds;
                    if (mWorkTotal > long.MaxValue - tickLength) {
                        mWorkTotal = MeanWorkLength;
                        mTickTotal = MeanTickLength;
                        mTickCount = 1;
                    }
                    mTickTotal += (long)Math.Round(tickLength);
                    mShortestTick = Math.Min(tickLength, mShortestTick);
                    mLongestTick = Math.Max(tickLength, mLongestTick);
                }

                double workLength = DateTime.Now.Subtract(mTickStart).TotalMilliseconds;
                mWorkTotal += (long)Math.Round(workLength);
                mShortestWork = Math.Min(workLength, mShortestWork);
                mLongestWork = Math.Max(workLength, mLongestWork);

                mTickCount++;
                mStarted = true;
                while (mTickTimes.Count > 0 && DateTime.Now.Subtract(mTickTimes.Peek()).TotalSeconds > 1.0)
                    mTickTimes.Dequeue();
                mLastTick = DateTime.Now;
                mTickTimes.Enqueue(DateTime.Now);
            }
        }
    }
}

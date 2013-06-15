/*************************************************************************
Copyright (c) 2012 John McCaffery 

This file is part of Chimera.

Chimera is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Chimera is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Chimera.  If not, see <http://www.gnu.org/licenses/>.

**************************************************************************/
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
        private long mWorkDeviationTotal;
        private long mTickDeviationTotal;
        private int mTickCount;
        private double mShortestTick = double.MaxValue;
        private double mShortestWork = double.MaxValue;
        private double mLongestTick = -1.0;
        private double mLongestWork = -1.0;
        private double mLastTickLength;
        private double mLastWorkLength;

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

        public long TickTotal {
            get { return mTickTotal; }
        }

        public double LastTick {
            get { return mLastTickLength; }
        }

        public long TickStandardDeviation {
            get { lock (mTickTimes) return mTickCount > 0 ? mTickDeviationTotal / mTickCount : 0; }
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

        public long WorkTotal {
            get { return mWorkTotal; }
        }

        public double LastWork {
            get { return mLastWorkLength; }
        }

        public long WorkStandardDeviation {
            get { lock (mTickTimes) return mTickCount > 0 ? mWorkDeviationTotal / mTickCount : 0; }
        }

        public int TicksPerSecond {
            get { lock (mTickTimes) return mTickTimes.Count; }
        }

        public void Begin() {
            mTickStart = DateTime.Now;
        }
        public void End() {
            lock (mTickTimes) {
                if (mStarted) {
                    mLastTickLength = DateTime.Now.Subtract(mLastTick).TotalMilliseconds;
                    if (mWorkTotal > long.MaxValue - mLastTickLength) {
                        mWorkTotal = MeanWorkLength;
                        mTickTotal = MeanTickLength;
                        mTickCount = 1;
                    }
                    long roundedTick = (long)Math.Round(mLastTickLength);
                    mTickTotal += roundedTick;
                    mTickDeviationTotal += Math.Abs(MeanTickLength - roundedTick);
                    mShortestTick = Math.Min(mLastTickLength, mShortestTick);
                    mLongestTick = Math.Max(mLastTickLength, mLongestTick);
                }

                mLastWorkLength = DateTime.Now.Subtract(mTickStart).TotalMilliseconds;
                long roundedWork = (long)Math.Round(mLastWorkLength);
                mWorkTotal += roundedWork;
                mWorkDeviationTotal += Math.Abs(MeanWorkLength - roundedWork);
                mShortestWork = Math.Min(mLastWorkLength, mShortestWork);
                mLongestWork = Math.Max(mLastWorkLength, mLongestWork);

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

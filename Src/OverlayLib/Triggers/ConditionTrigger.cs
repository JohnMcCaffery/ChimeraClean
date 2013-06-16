using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using Chimera.Util;

namespace Chimera.Overlay.Triggers {
    public abstract class ConditionTrigger : XmlLoader, ITrigger {
        private Core mCore;
        private Action mTickListener;
        private double mWaitMS = 0.0;
        private bool mActive;

        private bool mCondition;
        private bool mHasTriggered;
        private DateTime mStart;

        private readonly TickStatistics mStatistics = new TickStatistics();

        private event Action mTriggered;

        public abstract bool Condition {
            get;
        }

        protected double WaitMS {
            get { return mWaitMS; }
        }

        public ConditionTrigger(Core core, string name) {
            mCore = core;
            mTickListener = new Action(mCoordinator_Tick);

            StatisticsCollection.AddStatistics(mStatistics, name + " Trigger");
        }

        public ConditionTrigger(Core core, string name, double waitMS)
            : this(core, name) {

            mWaitMS = waitMS;
        }

        void mCoordinator_Tick() {
            mStatistics.Begin();
            if (Condition) {
                if (!mCondition) {
                    mCondition = true;
                    if (mWaitMS <= 0.0)
                        mTriggered();
                    else
                        mStart = DateTime.Now;
                } else if (!mHasTriggered && mWaitMS > 0.0 && DateTime.Now.Subtract(mStart).TotalMilliseconds > mWaitMS) {
                    mHasTriggered = true;
                    mTriggered();
                }
            } else if (mCondition) {
                mCondition = false;
                mHasTriggered = false;
            }
            mStatistics.End();
        }

        #region ITrigger Members

        public event Action Triggered {
            add {
                if (mActive && mTriggered == null)
                    mCore.Tick += mTickListener;
                mTriggered += value;
            }
            remove {
                mTriggered -= value;
                if (mActive && mTriggered == null)
                    mCore.Tick -= mTickListener;
            }

        }

        public virtual bool Active {
            get { return mActive; }
            set {
                if (mActive != value) {
                    mActive = value;
                    if (value)
                        mCore.Tick += mTickListener;
                    else
                        mCore.Tick -= mTickListener;
                }
            }
        }

        #endregion
    }
}

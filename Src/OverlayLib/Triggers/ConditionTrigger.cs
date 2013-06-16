using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;

namespace Chimera.Overlay.Triggers {
    public abstract class ConditionTrigger : XmlLoader, ITrigger {
        private Core mCore;
        private Action mTickListener;
        private double mWaitMS = 0.0;
        private bool mActive;

        private bool mCondition;
        private bool mHasTriggered;
        private DateTime mStart;

        private event Action mTriggered;

        public abstract bool Condition {
            get;
        }

        protected double WaitMS {
            get { return mWaitMS; }
        }

        public ConditionTrigger(Core core) {
            mCore = core;
            mTickListener = new Action(mCoordinator_Tick);
        }

        public ConditionTrigger(Core core, double waitMS)
            : this(core) {

            mWaitMS = waitMS;
        }

        void mCoordinator_Tick() {
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

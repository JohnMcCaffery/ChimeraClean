using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;

namespace Chimera.Overlay.Triggers {
    public abstract class ConditionTrigger : XmlLoader, ITrigger {
        private static readonly double TIMEOUT = 10000.0;
        private Coordinator mCoordinator;
        private Action mTickListener;
        private bool mCondition;
        private bool mActive;

        private event Action mTriggered;

        public abstract bool Condition {
            get;
        }

        public ConditionTrigger(Coordinator coordinator) {
            mCoordinator = coordinator;
            mTickListener = new Action(mCoordinator_Tick);
        }

        void mCoordinator_Tick() {
            if (Condition) {
                if (!mCondition) {
                    mCondition = true;
                    mTriggered();
                }
            } else if (mCondition) {
                mCondition = false;
            }
        }

        #region ITrigger Members

        public event Action Triggered {
            add {
                if (mActive && mTriggered == null)
                    mCoordinator.Tick += mTickListener;
                mTriggered += value;
            }
            remove {
                mTriggered -= value;
                if (mActive && mTriggered == null)
                    mCoordinator.Tick -= mTickListener;
            }

        }

        public bool Active {
            get { return mActive; }
            set {
                if (mActive != value) {
                    mActive = value;
                    if (value)
                        mCoordinator.Tick += mTickListener;
                    else
                        mCoordinator.Tick -= mTickListener;
                }
            }
        }

        #endregion
    }
}

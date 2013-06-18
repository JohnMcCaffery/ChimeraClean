using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using Chimera.Overlay;
using System.Xml;

namespace Chimera.Features.Triggers {
    public class TimerTrigger : XmlLoader, ITrigger {
        private bool mActive;
        private double mLengthMS = 5000;
        private Action mTickListener;
        private DateTime mStart;
        private Core mCore;

        public TimerTrigger(OverlayPlugin plugin, XmlNode node)
            : base(node) {

            mTickListener = new Action(mCore_Tick);
            mLengthMS = GetDouble(node, mLengthMS, "LengthMS");
        }

        void mCore_Tick() {
            if (DateTime.Now.Subtract(mStart).TotalMilliseconds > mLengthMS) {
                mCore.Tick -= mTickListener;
                if (Triggered != null)
                    Triggered();
            }
        }

        #region ITrigger Members

        public event Action Triggered;

        public bool Active {
            get { return mActive; }
            set {
                if (mActive != value) {
                    mActive = value;
                    if (value) {
                        mStart = DateTime.Now;
                        mCore.Tick += mTickListener;
                    } else
                        mCore.Tick -= mTickListener;
                }
            }
        }

        #endregion
    }
}

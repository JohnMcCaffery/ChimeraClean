using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using Chimera.Overlay;
using System.Xml;

namespace Chimera.Overlay.Triggers {
    public class TimerTriggerFactory : ITriggerFactory {
        public SpecialTrigger Special {
            get { return SpecialTrigger.None; }
        }

        public string Mode {
            get { return "None"; }
        }

        public ITrigger Create(OverlayPlugin manager, XmlNode node) {
            return new TimerTrigger(manager, node);
        }

        public ITrigger Create(OverlayPlugin manager, XmlNode node, System.Drawing.Rectangle clip) {
            return Create(manager, node);
        }

        public string Name {
            get { return "Timer"; }
        }
    }
 
    public class TimerTrigger : TriggerBase, ITrigger {
        private bool mActive;
        private double mLengthMS = 5000;
        private Action mTickListener;
        private DateTime mStart;
        private Core mCore;

        public TimerTrigger(OverlayPlugin plugin, XmlNode node)
            : base(node) {

            mCore = plugin.Core;
            mTickListener = new Action(mCore_Tick);
            mLengthMS = GetDouble(node, mLengthMS, "LengthMS");
        }

        void mCore_Tick() {
            if (DateTime.Now.Subtract(mStart).TotalMilliseconds > mLengthMS) {
                mCore.Tick -= mTickListener;
                Trigger();
            }
        }

        #region ITrigger Members

        public override bool Active {
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

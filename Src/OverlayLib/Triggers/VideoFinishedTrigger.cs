using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;

namespace Chimera.Overlay.Triggers {
    public class VideoFinishedTriggerFactory : ITriggerFactory {

        public SpecialTrigger Special {
            get { return SpecialTrigger.None; }
        }

        public string Mode {
            get { return "All"; }
        }

        public ITrigger Create(OverlayPlugin manager, System.Xml.XmlNode node) {
            return new VideoFinishedTrigger();
        }

        public ITrigger Create(OverlayPlugin manager, System.Xml.XmlNode node, System.Drawing.Rectangle clip) {
            return new VideoFinishedTrigger();
        }

        public string Name {
            get { return "VideoFinished"; }
        }
    }
    public class VideoFinishedTrigger : TriggerBase {
        private static HashSet<VideoFinishedTrigger> sTriggers = new HashSet<VideoFinishedTrigger>();
        private bool mActive = false;
        private static int sCount = 0;
        private int mHash = sCount++;

        internal static void TriggerAll() {
            foreach (var trigger in sTriggers.Where(t => t.Active))
                trigger.Trigger();
        }

        public override int GetHashCode() {
            return mHash;
        }

        public override bool Active {
            get { return mActive; }
            set {
                if (mActive != value) {
                    if (value)
                        sTriggers.Add(this);
                    else
                        sTriggers.Remove(this);
                    mActive = value;
                }
            }
        }
    }
}

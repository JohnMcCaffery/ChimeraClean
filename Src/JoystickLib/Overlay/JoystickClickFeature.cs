using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using Chimera.Overlay;
using System.Xml;
using Chimera.Util;

namespace Joystick.Overlay {
    public class JoystickClickFeature : OverlayXmlLoader, IFeature {
        private bool mActive;
        private bool mRight;
        private string mFrame;
        private ITrigger[] mTriggers;
        private Action mTriggerListener;

        public JoystickClickFeature(OverlayPlugin plugin, XmlNode node) {
            mTriggerListener = new Action(TriggerListener);

            mRight = GetBool(node, false, "RightClick");

            List<ITrigger> triggers = new List<ITrigger>();
            foreach (XmlNode trigger in GetChildrenOfChild(node, "Triggers")) {
                ITrigger t = plugin.GetTrigger(trigger, "JoystickClick trigger", null);
                if (t != null)
                    triggers.Add(t);
            }
            mTriggers = triggers.ToArray();
        }

        public void TriggerListener() {
            ProcessWrangler.Click();
        }

        public System.Drawing.Rectangle Clip { get { return new System.Drawing.Rectangle(); } set { } }

        public bool Active {
            get { return mActive; }
            set {
                if (mActive != value) {
                    mActive = value;
                    foreach (var trigger in mTriggers)
                        if (value)
                            trigger.Triggered += mTriggerListener;
                        else
                            trigger.Triggered -= mTriggerListener;
                }
            }
        }

        public bool NeedsRedrawn {
            get { return false; }
        }

        public string Frame {
            get { return mFrame; }
        }

        public string Name {
            get { return "JoystickClick"; }
            set { }
        }

        public void DrawStatic(System.Drawing.Graphics graphics) { }

        public void DrawDynamic(System.Drawing.Graphics graphics) { }

    }
}

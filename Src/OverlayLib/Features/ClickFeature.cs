using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using Chimera.Overlay;
using System.Xml;
using Chimera.Util;

namespace Chimera.Overlay.Features {
    public class ClickFeatureFactory : IFeatureFactory {
        public IFeature Create(OverlayPlugin manager, XmlNode node) {
            return new ClickFeature(manager, node);
        }

        public IFeature Create(OverlayPlugin manager, XmlNode node, System.Drawing.Rectangle clip) {
            return Create(manager, node);
        }

        public string Name {
            get { return "JoystickClick"; }
        }
    }

    public class ClickFeature : OverlayXmlLoader, IFeature {
        private bool mActive;
        private bool mLeft;
        private string mFrame;
        private ITrigger[] mTriggers;
        private Action mTriggerListener;

        public ClickFeature(OverlayPlugin plugin, XmlNode node) {
            mTriggerListener = new Action(TriggerListener);

            mLeft = GetBool(node, false, "LeftClick");
            mFrame = GetManager(plugin, node, "Click Feature").Name;

            List<ITrigger> triggers = new List<ITrigger>();
            foreach (XmlNode trigger in GetChildrenOfChild(node, "Triggers")) {
                ITrigger t = plugin.GetTrigger(trigger, "JoystickClick trigger", null);
                if (t != null)
                    triggers.Add(t);
            }
            mTriggers = triggers.ToArray();
        }

        public void TriggerListener() {
            ProcessWrangler.Click(mLeft);
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
            get { return "Joystick" + (mLeft ? "Left" : "Right") + "Click"; }
            set { }
        }

        public void DrawStatic(System.Drawing.Graphics graphics) { }

        public void DrawDynamic(System.Drawing.Graphics graphics) { }

    }
}

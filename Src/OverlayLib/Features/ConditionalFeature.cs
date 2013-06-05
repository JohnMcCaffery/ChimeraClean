using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using System.Drawing;
using System.Xml;

namespace Chimera.Overlay.Drawables {
    public class ConditionalFeature : XmlLoader, IFeature {
        private OverlayPlugin mPlugin;
        private IFeature mFeature;
        private ITrigger mActiveTrigger;
        private ITrigger mInactiveTrigger;

        public ConditionalFeature(OverlayPlugin plugin, XmlNode node) {
            mPlugin = plugin;
            XmlNode activeTriggerNode = node.SelectSingleNode("child::ActiveTrigger");
            XmlNode inActiveTriggerNode = node.SelectSingleNode("child::InactiveTrigger");
            XmlNode featureNode = node.SelectSingleNode("child::Feature");
            if (activeTriggerNode == null)
                throw new ArgumentException("Unable to load Conditional Feature. No 'ActiveTrigger' attribute specified.");
            if (inActiveTriggerNode == null)
                throw new ArgumentException("Unable to load Conditional Feature. No 'InactiveTrigger' attribute specified.");
            if (featureNode == null)
                throw new ArgumentException("Unable to load Conditional Feature. No 'Feature' attribute specified.");

            mActiveTrigger = mPlugin.GetTrigger(node, "conditional feature", null);
            mInactiveTrigger = mPlugin.GetTrigger(node, "conditional feature", null);
            mFeature = mPlugin.GetFeature(featureNode, "conditional feature", null);

            if (activeTriggerNode == null)
                throw new ArgumentException("Unable to load Conditional Feature. Unable to parse active trigger.");
            if (inActiveTriggerNode == null)
                throw new ArgumentException("Unable to load Conditional Feature. Unable to parse inactive trigger.");
            if (featureNode == null)
                throw new ArgumentException("Unable to load Conditional Feature. Unable to parse feature.");

            mActiveTrigger.Triggered += new Action(mActiveTrigger_Triggered);
            mInactiveTrigger.Triggered += new Action(mInactiveTrigger_Triggered);
        }

        void mActiveTrigger_Triggered() {
            mFeature.Active = true;
        }

        void mInactiveTrigger_Triggered() {
            mFeature.Active = false;
        }

        #region IFeature Members

        public Rectangle Clip {
            get { return mFeature.Clip; }
            set { mFeature.Clip = value; }
        }

        public bool Active {
            get { return mActiveTrigger.Active; }
            set {
                mActiveTrigger.Active = value;
                mInactiveTrigger.Active = value;
                if (!value)
                    mFeature.Active = false;
            }
        }

        public bool NeedsRedrawn {
            get { return mFeature.NeedsRedrawn; }
        }

        public string Window {
            get { return mFeature.Window; }
        }

        public void DrawStatic(Graphics graphics) {
            mFeature.DrawStatic(graphics);
        }

        public void DrawDynamic(Graphics graphics) {
            mFeature.DrawDynamic(graphics);
        }

        #endregion
    }
}

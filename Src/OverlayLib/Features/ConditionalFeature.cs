using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using System.Drawing;
using System.Xml;
using log4net;

namespace Chimera.Overlay.Features {
    public class ConditionalFeatureFactory : IFeatureFactory {
        public IFeature Create(OverlayPlugin manager, XmlNode node) {
            return new ConditionalFeature(manager, node);
        }

        public IFeature Create(OverlayPlugin manager, XmlNode node, Rectangle clip) {
            return Create(manager, node);
        }

        public string Name {
            get { return "Conditional"; }
        }
    }
    public class ConditionalFeature : OverlayXmlLoader, IFeature {
        private static ILog Logger = LogManager.GetLogger("Overlay");
        private OverlayPlugin mPlugin;
        private IFeature mFeature;
        private List<ITrigger> mActiveTriggers = new List<ITrigger>();
        private List<ITrigger> mInactiveTriggers = new List<ITrigger>();
        private bool mActive = false;
        private bool mStartActive = false;
        private bool mMultiActivate = false;

        public ConditionalFeature(OverlayPlugin plugin, XmlNode node) {
            mPlugin = plugin;
            XmlNode featureNode = node.SelectSingleNode("child::Feature");

            mStartActive = GetBool(node, false, "StartActive");
            mMultiActivate = GetBool(node, false, "MultiActivate");

            if (featureNode == null)
                throw new ArgumentException("Unable to load Conditional Feature. Unable to parse feature.");

            foreach (var child in GetChildrenOfChild(node, "ActiveTriggers")) {
                var trigger = plugin.GetTrigger(child, "conditional feature", null);
                mActiveTriggers.Add(trigger);
                trigger.Triggered += mActiveTrigger_Triggered;
            } foreach (var child in GetChildrenOfChild(node, "InactiveTriggers")) {
                var trigger = plugin.GetTrigger(child, "conditional feature", null);
                mInactiveTriggers.Add(trigger);
                trigger.Triggered += mInactiveTrigger_Triggered;
            }

            mFeature = mPlugin.GetFeature(featureNode, "conditional feature", null);

            if (node.SelectSingleNode("child::ActiveTriggers") == null)
                Logger.Info("No active trigger node found in conditional feature " + Name + ".");
            if (node.SelectSingleNode("child::InactiveTriggers") == null)
                Logger.Info("No inactive trigger node found in conditional feature.");

            if (mActiveTriggers.Count == 0)
                Logger.Info("No active triggers loaded in conditional feature " + Name + ".");
            if (mInactiveTriggers.Count == 0)
                Logger.Info("No inactive triggers loaded in conditional feature " + Name + ".");
        }

        void mActiveTrigger_Triggered(ITrigger source) {
            mFeature.Active = true;
        }

        void mInactiveTrigger_Triggered(ITrigger source) {
            mFeature.Active = false;
        }

        #region IFeature Members

        public Rectangle Clip {
            get { return mFeature.Clip; }
            set { mFeature.Clip = value; }
        }

        public bool Active {
            get { return mActive; }
            set {
                if (mActive != value || mMultiActivate) {
                    mActive = value;
                    mActiveTriggers.ForEach((testc) => testc.Active = value);
                    mInactiveTriggers.ForEach((testc) => testc.Active = value);
                    if (!value)
                        mFeature.Active = false;
                    else if (mStartActive)
                        mFeature.Active = true;
                }
            }
        }

        public bool NeedsRedrawn {
            get { return mFeature.NeedsRedrawn; }
        }

        public string Frame {
            get { return mFeature.Frame; }
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

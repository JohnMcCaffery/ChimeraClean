using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Xml;
using Chimera.Interfaces.Overlay;
using Chimera.Overlay;
using OpenMetaverse;
using log4net;

namespace Chimera.OpenSim.Overlay
{
    public class CloseBrowserFeatureFactory : IFeatureFactory
    {
        private static ILog log = LogManager.GetLogger("CloseBrowser");

        public IFeature Create(OverlayPlugin manager, XmlNode node)
        {
            log.Warn("Creating CloseBrowserFeature");
            return new CloseBrowserFeature(manager, node);
        }

        public IFeature Create(OverlayPlugin manager, XmlNode node, Rectangle clip)
        {
            return Create(manager, node);
        }

        public string Name
        {
            get { return "CloseBrowser"; }
        }
    }

    public class CloseBrowserFeature : OverlayXmlLoader, IFeature
    {
        private bool mActive;
        private string mFrame;
        private ITrigger[] mTriggers;
        private Action<ITrigger> mTriggerListener;
        protected OpenSimController mController;
        protected OverlayPlugin mPlugin;

        private static ILog log = LogManager.GetLogger("CloseBrowser");

        public CloseBrowserFeature(OverlayPlugin plugin, XmlNode node)
        {
            mTriggerListener = new Action<ITrigger>(TriggerListener);

            mFrame = GetManager(plugin, node, "CloseBrowser Feature").Name;

            List<ITrigger> triggers = new List<ITrigger>();
            foreach (XmlNode trigger in GetChildrenOfChild(node, "Triggers"))
            {
                ITrigger t = plugin.GetTrigger(trigger, "CloseBrowser trigger", null);
                if (t != null)
                {
                    triggers.Add(t);
                }
            }
            mTriggers = triggers.ToArray();
            if (plugin.Core[mFrame].Output is OpenSimController)
                mController = plugin.Core[mFrame].Output as OpenSimController;
            mPlugin = plugin;
        }

        public void TriggerListener(ITrigger source)
        {
            log.Warn("Sending CloseBrowserPacket");
            mController.ProxyController.CloseBrowser();
        }

        public System.Drawing.Rectangle Clip { get { return new System.Drawing.Rectangle(); } set { } }

        public bool Active
        {
            get { return mActive; }
            set
            {
                if (mActive != value)
                {
                    mActive = value;
                    foreach (var trigger in mTriggers)
                        if (value)
                        {
                            trigger.Active = true;
                            trigger.Triggered += mTriggerListener;
                        }
                        else
                        {
                            trigger.Triggered -= mTriggerListener;
                            trigger.Active = false;
                        }
                }
            }
        }

        public bool NeedsRedrawn
        {
            get { return false; }
        }

        public string Frame
        {
            get { return mFrame; }
        }

        public string Name
        {
            get { return "CloseBrowser"; }
            set { }
        }

        public void DrawStatic(System.Drawing.Graphics graphics) { }

        public void DrawDynamic(System.Drawing.Graphics graphics) { }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Xml;
using Chimera.Interfaces.Overlay;
using Chimera.Overlay;

namespace Chimera.OpenSim.Overlay
{
    public class SayFeatureFactory : IFeatureFactory
    {
        public IFeature Create(OverlayPlugin manager, XmlNode node)
        {
            return new SayFeature(manager, node);
        }

        public IFeature Create(OverlayPlugin manager, XmlNode node, Rectangle clip)
        {
            return Create(manager, node);
        }

        public string Name
        {
            get { return "Say"; }
        }
    }

    public class SayFeature : OverlayXmlLoader, IFeature
    {
        private bool mActive;
        private string mFrame;
        private string mMessageString;
        protected int mChannel;
        private ITrigger[] mTriggers;
        private Action<ITrigger> mTriggerListener;
        protected OpenSimController mController;
        protected OverlayPlugin mPlugin;

        public SayFeature(OverlayPlugin plugin, XmlNode node)
        {
            mTriggerListener = new Action<ITrigger>(TriggerListener);

            mFrame = GetManager(plugin, node, "Say Feature").Name;

            List<ITrigger> triggers = new List<ITrigger>();
            foreach (XmlNode trigger in GetChildrenOfChild(node, "Triggers"))
            {
                ITrigger t = plugin.GetTrigger(trigger, "Say trigger", null);
                if (t != null)
                {
                    triggers.Add(t);
                }
            }
            mTriggers = triggers.ToArray();
            if (plugin.Core[mFrame].Output is OpenSimController)
                mController = plugin.Core[mFrame].Output as OpenSimController;
            mPlugin = plugin;
            mMessageString = GetString(node, "click", "Message");
            mChannel = GetInt(node, 1, "Channel");
        }

        public void TriggerListener(ITrigger source)
        {
            Chat(mMessageString);
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
            get { return "Say"; }
            set { }
        }

        public void DrawStatic(System.Drawing.Graphics graphics) { }

        public void DrawDynamic(System.Drawing.Graphics graphics) { }

        public virtual void Chat(string msg) {
            mController.ProxyController.Chat(msg, mChannel);
        }

       
    }
}

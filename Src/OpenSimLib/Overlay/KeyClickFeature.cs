using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using Chimera.Overlay;
using System.Xml;
using log4net;
using Chimera.Util;
using Chimera.OpenSim;

namespace Chimera.OpenSim.Overlay
{
    public class KeyClickFeatureFactory : IFeatureFactory
    {
        public IFeature Create(OverlayPlugin manager, XmlNode node)
        {
            return new KeyClickFeature(manager, node);
        }

        public IFeature Create(OverlayPlugin manager, XmlNode node, System.Drawing.Rectangle clip)
        {
            return Create(manager, node);
        }

        public string Name
        {
            get { return "KeyClick"; }
        }
    }

    public class KeyClickFeature : OverlayXmlLoader, IFeature
    {
        private bool mActive;
        private string keys;
        private string mFrame;
        private ITrigger[] mTriggers;
        private Action mTriggerListener;
        private OpenSimController mController;

        public KeyClickFeature(OverlayPlugin plugin, XmlNode node)
        {
            mTriggerListener = new Action(TriggerListener);

            keys = node.Attributes["Keys"].Value;
            mFrame = GetManager(plugin, node, "Click Feature").Name;

            List<ITrigger> triggers = new List<ITrigger>();
            foreach (XmlNode trigger in GetChildrenOfChild(node, "Triggers"))
            {
                ITrigger t = plugin.GetTrigger(trigger, "KeyClick trigger", null);
                if (t != null)
                {
                    triggers.Add(t);
                }
            }
            mTriggers = triggers.ToArray();
            if (plugin.Core.Frames.First().Output is OpenSimController)
                mController = plugin.Core.Frames.First().Output as OpenSimController;
        }

        public void TriggerListener()
        {
            foreach (var key in keys.Split(','))
            {
                mController.ViewerController.PressKey(key);
            }
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
            get { return "KeyClick"; }
            set { }
        }

        public void DrawStatic(System.Drawing.Graphics graphics) { }

        public void DrawDynamic(System.Drawing.Graphics graphics) { }

    }
}

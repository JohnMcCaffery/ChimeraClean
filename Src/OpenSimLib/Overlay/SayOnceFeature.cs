using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Xml;
using Chimera.Interfaces.Overlay;
using Chimera.Overlay;

namespace Chimera.OpenSim.Overlay
{
    public class SayOnceFeatureFactory : IFeatureFactory
    {
        public IFeature Create(OverlayPlugin manager, XmlNode node)
        {
            return new SayOnceFeature(manager, node);
        }

        public IFeature Create(OverlayPlugin manager, XmlNode node, Rectangle clip)
        {
            return Create(manager, node);
        }

        public string Name
        {
            get { return "SayOnce"; }
        }
    }

    public class SayOnceFeature : OverlayXmlLoader, IFeature
    {
        private bool mActive;
        private string mFrame;
        private string mMessageString;
        private int mChannel;
        private OpenSimController mController;
        private OverlayPlugin mPlugin;

        public SayOnceFeature(OverlayPlugin plugin, XmlNode node)
        {
            mFrame = GetManager(plugin, node, "SayOnce Feature").Name;

            if (plugin.Core[mFrame].Output is OpenSimController)
                mController = plugin.Core[mFrame].Output as OpenSimController;
            mPlugin = plugin;
            mMessageString = GetString(node, "click", "Message");
            mChannel = GetInt(node, 1, "Channel");
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
                    if(value)
                        Chat(mMessageString);
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
            get { return "SayOnce"; }
            set { }
        }

        public void DrawStatic(System.Drawing.Graphics graphics) { }

        public void DrawDynamic(System.Drawing.Graphics graphics) { }

        private void Chat(string msg) {
            mController.ProxyController.Chat(msg, mChannel);
        }
    }
}

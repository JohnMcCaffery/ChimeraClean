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
    public class MuteAudioFeatureFactory : IFeatureFactory
    {
        private static ILog log = LogManager.GetLogger("MuteAudio");

        public IFeature Create(OverlayPlugin manager, XmlNode node)
        {
            return new MuteAudioFeature(manager, node);
        }

        public IFeature Create(OverlayPlugin manager, XmlNode node, Rectangle clip)
        {
            return Create(manager, node);
        }

        public string Name
        {
            get { return "MuteAudio"; }
        }
    }

    public class MuteAudioFeature : OverlayXmlLoader, IFeature
    {
        private bool mActive;
        private string mFrame;
        private bool mMute;
        protected OpenSimController mController;
        protected OverlayPlugin mPlugin;

        private static ILog log = LogManager.GetLogger("CloseBrowser");

        public MuteAudioFeature(OverlayPlugin plugin, XmlNode node)
        {

            mFrame = GetManager(plugin, node, "MuteAudio Feature").Name;
            mMute = GetBool(node, false, "Mute");
            
            if (plugin.Core[mFrame].Output is OpenSimController)
                mController = plugin.Core[mFrame].Output as OpenSimController;
            mPlugin = plugin;
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
                    if (value)
                    {
                        log.Warn("Sending MuteAudioPacket");
                        mController.ProxyController.MuteAudio(mMute);
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

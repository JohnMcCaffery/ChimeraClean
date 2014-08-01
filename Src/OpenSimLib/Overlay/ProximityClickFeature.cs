using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Xml;
using Chimera.Interfaces.Overlay;
using Chimera.Overlay;
using OpenMetaverse;

namespace Chimera.OpenSim.Overlay
{
    public class ProximityClickFeatureFactory : IFeatureFactory
    {
        public IFeature Create(OverlayPlugin manager, XmlNode node)
        {
            return new ProximityClickFeature(manager, node);
        }

        public IFeature Create(OverlayPlugin manager, XmlNode node, Rectangle clip)
        {
            return Create(manager, node);
        }

        public string Name
        {
            get { return "ProximityClick"; }
        }
    }

    public class ProximityClickFeature : SayFeature
    {
        private string camera = "MainWindow";

        public ProximityClickFeature(OverlayPlugin plugin, XmlNode node) : base(plugin, node)
        {
            string camera = GetString(node, "MainWindow", "Camera");
            
            
        }

        public override void Chat(string msg)
        {
            OpenSimController cameraController;
            UUID cameraID = UUID.Zero;
            if (mPlugin.Core[camera].Output is OpenSimController)
            {
                cameraController = mPlugin.Core[camera].Output as OpenSimController;
                cameraID = cameraController.ProxyController.AgentID;
            }
            if(cameraID != UUID.Zero)
                msg = String.Format("{0} {1}", msg, cameraID);
            mController.ProxyController.Chat(msg, mChannel);
        }


    }
}

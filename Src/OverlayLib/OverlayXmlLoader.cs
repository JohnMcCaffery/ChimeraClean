using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Chimera.Overlay.Features;
using System.Drawing;
using System.IO;
using Chimera.Interfaces.Overlay;
using Chimera.Overlay.Interfaces;
using System.Windows.Forms;
using log4net;

namespace Chimera.Overlay {
    public abstract class OverlayXmlLoader : XmlLoader, IControllable {
        private static readonly ILog Logger = LogManager.GetLogger("Overlay");

        private Panel mPanel = new Panel();
        private string mName = null;

        public virtual Control ControlPanel {
            get { return mPanel; }
        }

        public virtual string Name {
            get { return mName == null ? GetType().Name : mName; }
            set { mName = value; }
        }

        protected OverlayXmlLoader() { }
        protected OverlayXmlLoader(string name) { mName = name; }
        protected OverlayXmlLoader(XmlNode node) { mName = GetName(node, "XmlLoader"); }

        public static FrameOverlayManager GetManager(OverlayPlugin manager, XmlNode node, string request) {
            FrameOverlayManager mManager;
            if (node == null) {
                mManager = manager[0];
                Logger.Debug("No node specified when looking up frame for " + request + ". Using " + mManager.Frame.Name + " as default.");
                return mManager;
            }
            XmlAttribute frameAttr = node.Attributes["Frame"];
            if (frameAttr == null) {
                mManager = manager[0];
                Logger.Debug("No window specified whilst resolving " + node.Name + " from " + node.Name + ". Using " + mManager.Frame.Name + " as default.");
            } else {
                if (!manager.IsKnownWindow(frameAttr.Value)) {
                    mManager = manager[0];
                    Logger.Debug(frameAttr.Value + " is not a known frame. Using " + mManager.Frame.Name + " as default.");
                } else
                    mManager = manager[frameAttr.Value];
            }
            return mManager;
        }  
    }
}

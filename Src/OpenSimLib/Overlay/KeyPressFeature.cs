using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using System.Xml;
using System.Drawing;
using Chimera.Overlay.Triggers;
using Chimera.OpenSim;
using log4net;
using Chimera.Overlay;

namespace Chimera.OpenSim.Overlay
{
    public class KeyPressFeatureFactory : IFeatureFactory
    {
        #region IFactory<IFeature> Members

        public IFeature Create(OverlayPlugin manager, XmlNode node)
        {
            return new KeyPressFeature(manager, node);
        }

        public IFeature Create(OverlayPlugin manager, XmlNode node, Rectangle clip)
        {
            return Create(manager, node);
        }

        #endregion

        #region IFactory Members

        public string Name
        {
            get { return "KeyPress"; }
        }

        #endregion
    }

    public class KeyPressFeature : OverlayXmlLoader, IFeature
    {
        private OverlayPlugin mPlugin;
        private OpenSimController mController;
        private bool mActive = false;
        private string keys;
        private readonly ILog ThisLogger = LogManager.GetLogger("KeyPress");
        private string mFrame;

        public KeyPressFeature(OverlayPlugin plugin, XmlNode node)
        {
            mPlugin = plugin;
            keys = node.Attributes["Keys"].Value;
            ThisLogger.WarnFormat("Creating KeyPress with: {0}", keys);
            mFrame = GetManager(plugin, node, "Click Feature").Name;
            if (plugin.Core[mFrame].Output is OpenSimController)
                mController = plugin.Core[mFrame].Output as OpenSimController;
        }



        #region IFeature Members

        public Rectangle Clip
        {
            get { return Rectangle.Empty; }
            set {  }
        }

        public bool Active
        {
            get { return mActive; }
            set
            {
                if (value && !mActive)
                {
                    foreach (var key in keys.Split(','))
                    {
                        ThisLogger.Info(mController.ViewerController.Name + " viewer pressing " + key);
                        mController.ViewerController.PressKey(key);
                    }   
                }
                mActive = value;
            }
        }

        public bool NeedsRedrawn
        {
            get { return false; }
        }

        public string Frame
        {
            get { return mPlugin.Core.Frames.First().Name; }
        }

        public void DrawStatic(Graphics graphics)
        {
        }

        public void DrawDynamic(Graphics graphics)
        {
        }

        #endregion
    }
}

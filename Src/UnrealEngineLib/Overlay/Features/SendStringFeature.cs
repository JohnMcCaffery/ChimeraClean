using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using Chimera.Overlay.Features;
using Chimera.Overlay;
using System.Xml;

namespace UnrealEngineLib.Overlay.Features {

    public class SendStringFeatureFactory : IFeatureFactory {

        public IFeature Create(OverlayPlugin manager, XmlNode node) {
            return new SendStringFeature(manager, node);
        }

        public IFeature Create(OverlayPlugin manager, XmlNode node, System.Drawing.Rectangle clip) {
            return Create(manager, node);
        }

        public string Name {
            get { return "SendUnrealString"; }
        }
    }

    public class SendStringFeature : FeatureBase {
        private UnrealController mController;
        private string mString;

        public override bool Active {
            get { return false; }
            set {
                mController.SendString(mString);
            }
        }

        public SendStringFeature(OverlayPlugin manager, XmlNode node)
            : base(manager, node) {

                mString = GetString(node, "Blah", "String");

                if (!(manager.Core.Frames[0].Output is UnrealController))
                    throw new Exception("Unable to load unreal send string feature. No unreal controller found.");
                mController = manager.Core.Frames[0].Output as UnrealController;
        }
    }
}

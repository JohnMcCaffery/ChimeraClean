using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Overlay;
using Chimera.Interfaces.Overlay;
using Chimera.Overlay.Features;
using System.Xml;
using Chimera;
using log4net;

namespace UnrealEngineLib.Overlay {
    public class FillFeatureFactory : IFeatureFactory {

        public IFeature Create(OverlayPlugin manager, XmlNode node) {
            return new FillFeature(manager, node);
        }

        public IFeature Create(OverlayPlugin manager, XmlNode node, System.Drawing.Rectangle clip) {
            return Create(manager, node);
        }

        public string Name {
            get { return "UnrealFill"; }
        }
    }
    public class FillFeature : FeatureBase, IFeature {
        private Fill mFill;
        private UnrealController mController;

	public FillFeature(OverlayPlugin plugin, XmlNode node) : base (plugin, node) {
            mFill = GetEnum<Fill>(node, Fill.Full, LogManager.GetLogger("UnrealOverlay"), "Fill");

            mController = Manager.Frame.Output as UnrealController;
            if (mController == null)
                throw new Exception("Cannot use FillFeature, UnrealController is not the controller.");
        }

        public override bool Active {
            get { return base.Active; }
            set {
                if (value && base.Active != value) {
                    mController.Fill = mFill;
                }
                base.Active = value;
            }
        }
    }
}

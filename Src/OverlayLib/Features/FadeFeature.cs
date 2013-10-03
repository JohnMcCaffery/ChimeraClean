using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Overlay.Transitions;
using Chimera.Interfaces.Overlay;
using System.Xml;
using System.Drawing;

namespace Chimera.Overlay.Features {
    public class FadeFeatureFactory : OverlayXmlLoader, IFeatureFactory {
        #region IImageTransitionFactory Members

        public string Name { get { return "Fade"; } }

        public IFeature Create(OverlayPlugin manager, XmlNode node) {
            return new FadeFeature(manager, node);
        }

        public IFeature Create(OverlayPlugin manager, XmlNode node, Rectangle clip) {
            return Create(manager, node);
        }

        #endregion
    }

    public class FadeFeature : FadeTransition {
        public override bool Active {
            get { return base.Active; }
            set {
                base.Active = value;
                Begin();
            }
        }

        public FadeFeature(OverlayPlugin manager, XmlNode node)
            : base(manager, node, false) {
        }
    }
}
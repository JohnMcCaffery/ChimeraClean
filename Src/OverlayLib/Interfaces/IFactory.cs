using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Chimera.Overlay;
using System.Drawing;
using System.Windows.Forms;

namespace Chimera.Interfaces.Overlay {
    public interface IFactory {
        string Name { get; }
    }
    public interface IFactory<T> : IFactory {
        T Create(OverlayPlugin manager, XmlNode node);
        T Create(OverlayPlugin manager, XmlNode node, Rectangle clip);
    }

    public enum SpecialTrigger { Invisible, Text, Image, None }

    public interface ITriggerFactory : IFactory<ITrigger> {
        SpecialTrigger Special { get; }
        string Mode { get; }
    }

    public interface ITransitionStyleFactory : IFactory<ITransitionStyle> { }

    public interface ISelectionRendererFactory : IFactory<ISelectionRenderer> { }

    public interface IStateFactory : IFactory<State> { }

    public interface IFeatureFactory : IFactory<IFeature> { }

    public interface IFeatureTransitionFactory : IFactory<IFeatureTransition> {
        IFeatureTransition Create(double length);
    }
}

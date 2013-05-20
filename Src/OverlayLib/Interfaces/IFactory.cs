using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Chimera.Overlay;
using System.Drawing;

namespace Chimera.Interfaces.Overlay {
    public interface IFactory<T> {
        string Name { get; }
        T Create(OverlayPlugin manager, XmlNode node);
        T Create(OverlayPlugin manager, XmlNode node, Rectangle clip);
    }

    public enum SpecialTrigger { Invisible, Text, Image, None }

    public interface ITriggerFactory : IFactory<ITrigger> {
        SpecialTrigger Special { get; }
        string Mode { get; }
    }

    public interface ITransitionStyleFactory : IFactory<IWindowTransitionFactory> { }

    public interface ISelectionRendererFactory : IFactory<IHoverSelectorRenderer> { }

    public interface IStateFactory : IFactory<State> { }

    public interface IDrawableFactory : IFactory<IDrawable> { }

    public interface IImageTransitionFactory : IFactory<IImageTransition> {
        IImageTransition Create(double length);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Chimera.Overlay;

namespace Chimera.Interfaces.Overlay {
    public interface IFactory<T> {
        string Name { get; }
        T Create(XmlNode node, Coordinator coordinator);
    }

    public enum SpecialTrigger { Invisible, Text, Image, None }

    public interface ITriggerFactory : IFactory<ITrigger> {
        SpecialTrigger Special { get; }
        string Mode { get; }
    }

    public interface ITransitionStyleFactory : IFactory<IWindowTransitionFactory> { }

    public interface ISelectionRendererFactory : IFactory<IHoverSelectorRenderer> { }

    public interface IStateFactory : IFactory<State> { }
}

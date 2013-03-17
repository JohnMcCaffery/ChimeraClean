using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chimera.Interfaces {
    public interface IUpdater<T> {
        T Value { get; set; }
        event Action<T> Changed;
        string Name { get; }
    }
}

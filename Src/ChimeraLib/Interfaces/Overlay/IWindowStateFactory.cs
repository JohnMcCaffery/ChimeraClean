using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chimera.Interfaces.Overlay {
    public interface IWindowStateFactory {
        /// <param name="window">The window the new window state is linked to.</param>
        void Create(Window window);
    }
}

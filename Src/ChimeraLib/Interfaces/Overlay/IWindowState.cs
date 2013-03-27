using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chimera.Interfaces.Overlay {
    public interface IWindowState : IDrawable {
        Chimera.Overlay.WindowOverlayManager WindowOverlayManager {
            get;
            set;
        }
    }
}

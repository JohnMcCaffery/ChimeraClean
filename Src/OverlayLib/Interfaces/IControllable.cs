using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Chimera.Overlay.Interfaces {
    public interface IControllable {
        Control ControlPanel { get; }
        string Name { get; set;  }
    }
}

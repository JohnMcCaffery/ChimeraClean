using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Chimera.Kinect.Interfaces {
    public interface IHelpTrigger {
        event Action<IHelpTrigger> Triggered;

        UserControl ControlPanel { get; }
        bool Enabled { get; set; }
        string Name { get; }

        void Init(Coordinator controller);
    }
}

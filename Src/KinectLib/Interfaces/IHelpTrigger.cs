using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Chimera.Kinect.Interfaces {
    public interface IHelpTrigger {
        event Action HelpTriggered;

        UserControl ControlPanel { get; }
        bool Active { get; }

        void Init(IKinectController controller);
    }
}

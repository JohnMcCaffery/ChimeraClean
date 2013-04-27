using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Chimera.Util;

namespace Chimera {
    public interface ISystemPlugin : IPlugin {

        /// <summary>
        /// The coordinator this plugin is to control.
        /// </summary>
        Coordinator Coordinator {
            get;
        }

        /// <summary>
        /// Initialise the plugin, giving it a reference to the plugin it is to control.
        /// </summary>
        /// <param name="coordinator">The coordinator object the plugin can control.</param>
        void Init(Coordinator coordinator);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chimera.Interfaces.Overlay {
    public interface ITrigger {
        /// <summary>
        /// The trigger has been activated.
        /// </summary>
        event Action Triggered;

        /// <summary>
        /// Whether the trigger should activate.
        /// </summary>
        bool Active {
            get;
            set;
        }
    }
}

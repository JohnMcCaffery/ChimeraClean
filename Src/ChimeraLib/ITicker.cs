using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chimera {
    public interface IInputSource {
        /// <summary>
        /// Triggered every tick. Listen for this to keep time across the system.
        /// </summary>
        event System.Action Tick;

        /// <summary>
        /// Selected whenever a key is pressed or released on the keyboard.
        /// </summary>
        event System.Action<Chimera.Coordinator, System.Windows.Forms.KeyEventArgs> KeyUp;

        /// <summary>
        /// Selected whenever a key is pressed or released on the keyboard.
        /// </summary>
        event System.Action<Chimera.Coordinator, System.Windows.Forms.KeyEventArgs> KeyDown;
    }
}

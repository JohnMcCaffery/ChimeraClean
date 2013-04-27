using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Chimera {
    public interface ITickSource {
        /// <summary>
        /// Triggered every tick. Listen for this to keep time across the system.
        /// </summary>
        event Action Tick;

        /// <summary>
        /// Selected whenever a key is pressed or released on the keyboard.
        /// </summary>
        event Action<Coordinator, KeyEventArgs> KeyUp;

        /// <summary>
        /// Selected whenever a key is pressed or released on the keyboard.
        /// </summary>
        event Action<Coordinator, KeyEventArgs> KeyDown;

        /// <summary>
        /// CustomTrigger a key event.
        /// </summary>
        /// <param name="down">Whether the key is being pressed or released.</param>
        /// <param name="args">The argument for the key press.</param>
        void TriggerKeyboard(bool down, KeyEventArgs args);
    }
}

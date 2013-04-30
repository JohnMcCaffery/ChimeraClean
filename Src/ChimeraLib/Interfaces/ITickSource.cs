/*************************************************************************
Copyright (c) 2012 John McCaffery 

This file is part of Chimera.

Chimera is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Chimera is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Chimera.  If not, see <http://www.gnu.org/licenses/>.

**************************************************************************/
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

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
using System.Diagnostics;
using Chimera.Util;

namespace Chimera {
    public enum Fill {
        /// <summary>
        /// Fill the left half of the screen.
        /// </summary>
        Left,
        /// <summary>
        /// Fill the right half of the screen.
        /// </summary>
        Right,
        /// <summary>
        /// Run windowed.
        /// </summary>
        Windowed,
        /// <summary>
        /// Fill the whole screen.
        /// </summary>
        Full
    }

    public interface IOutput {

        /// <summary>
        /// The input which this output is rendering the view through.
        /// </summary>
        Frame Frame { get; }

        /// <summary>
        /// The panel which can be added to a form to configure this output.
        /// </summary>
        Control ControlPanel { get; }

        /// <summary>
        /// Set this to true if you want the output to be restarted whenever it exits. Useful if the process needs to run for long periods of time and to recover transition any errors.
        /// </summary>
        bool AutoRestart {
            get;
            set;
        }

        /// <summary>
        /// A multi line string that can be printed to file to store a record of window in the event of a crash.
        /// </summary>
        string State {
            get;
        }

        /// <summary>
        /// The type of output this is.
        /// </summary>
        string Type {
            get;
        }

	/// <summary>
	/// How the output should fill the screen.
	/// </summary>
        Fill Fill { get; set; }

        /// <summary>
        /// Whether the output has started.
        /// </summary>
        bool Active { get; }

        /// <summary>
        /// The process which is running this output.
        /// </summary>
        Process Process { get; }

        /// <summary>
        /// Initialise the input, giving it a reference to the input it is to render.
        /// </summary>
        /// <param name="input">The input which this output is supposed to render the view through.</param>
        void Init(Frame frame);

        /// <summary>
        /// CustomTrigger the output application to launch.
        /// </summary>
        bool Launch();

        /// <summary>
        /// Close down the output.
        /// </summary>
        void Close();

        /// <summary>
        /// Restart the output.
        /// </summary>
        void Restart(string reason);
    }
}

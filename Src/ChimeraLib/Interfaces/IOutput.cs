﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Chimera {
    public interface IOutput {

        /// <summary>
        /// The window which this output is rendering the view through.
        /// </summary>
        Window Window { get; }

        /// <summary>
        /// The panel which can be added to a form to configure this output.
        /// </summary>
        UserControl ConfigPanel { get; }

        /// <summary>
        /// Set this to true if you want the output to be restarted whenever it exits. Useful if the process needs to run for long periods of time and to recover from any errors.
        /// </summary>
        bool AutoRestart {
            get;
            set;
        }

        /// <summary>
        /// A multi line string that can be printed to file to store a record of state in the event of a crash.
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
        /// Initialise the input, giving it a reference to the window it is to render.
        /// </summary>
        /// <param name="window">The window which this output is supposed to render the view through.</param>
        void Init(Window window);

        /// <summary>
        /// Trigger the output application to launch.
        /// </summary>
        bool Launch();

        void Close();
    }
}
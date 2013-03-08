using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Chimera {
    public interface IOutput {

        Window Window { get; }

        /// <summary>
        /// The panel which can be added to a form to configure this output.
        /// </summary>
        UserControl ConfigPanel { get; }

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

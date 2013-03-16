using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Chimera.Util;

namespace Chimera {
    public interface IInput {
        /// <summary>
        /// The panel which can be added to a form to configure this input.
        /// </summary>
        UserControl ControlPanel {
            get;
        }

        /// <summary>
        /// Whether this input should control the virtual camera.
        /// </summary>
        bool Enabled {
            get;
            set;
        }

        /// <summary>
        /// The unique name by which this input can be identified.
        /// </summary>
        string Name {
            get;
        }

        /// <summary>
        /// A multi line string that can be printed to file to store a record of window in the event of a crash.
        /// </summary>
        string State {
            get;
        }

        /// <summary>
        /// The configuration object used by this input.
        /// </summary>
        ConfigBase Config {
            get;
        }

        /// <summary>
        /// Called when the input is to be disposed of.
        /// </summary>
        void Close();

        /// <summary>
        /// DrawSelected any relevant information about this input onto a diagram.
        /// </summary>
        /// <param name="perspective">The perspective to render along.</param>
        /// <param name="graphics">The graphics object to draw with.</param>
        void Draw(Perspective perspective, Graphics graphics);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Chimera {
    public interface IInput {
        /// <summary>
        /// The panel which can be added to a form to configure this input.
        /// </summary>
        UserControl ControlPanel {
            get;
        }

        /// <summary>
        /// The unique name by which this input can be identified.
        /// </summary>
        string Name {
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
        /// Strings which can be output as help information about how this input can be configured.
        /// </summary>
        string[] ConfigSwitches {
            get;
        }

        /// <summary>
        /// A multi line string that can be printed to file to store a record of state in the event of a crash.
        /// </summary>
        string State {
            get;
        }

        /// <summary>
        /// The coordinator this input is to control.
        /// </summary>
        Coordinator Coordinator {
            get;
        }

        /// <summary>
        /// Initialise the input, giving it a reference to the coordinator it is to control.
        /// </summary>
        /// <param name="coordinator">The coordinator object the input can control.</param>
        void Init(Coordinator coordinator);

        /// <summary>
        /// Called when the input is to be disposed of.
        /// </summary>
        void Close();

        /// <summary>
        /// Draw any relevant information about this input onto a diagram.
        /// </summary>
        /// <param name="perspective">The perspective to render along.</param>
        /// <param name="graphics">The graphics object to draw with.</param>
        void Draw(Chimera.Perspective perspective, System.Drawing.Graphics graphics);
    }
}

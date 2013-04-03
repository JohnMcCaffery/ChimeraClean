using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Chimera.Util;
using OpenMetaverse;

namespace Chimera {
    public interface IInput {
        /// <summary>
        /// Triggered whenever this input is enabled or disabled;
        /// </summary>
        event Action<IInput, bool> EnabledChanged;

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
        /// <param name=" to2D">Lambda to convert a 3D coordinate into a 2D coordinate that can be rendered on the output.</param>
        /// <param name="graphics">The graphics object to draw with.</param>
        void Draw(Func<Vector3, Point> to2D, Graphics graphics, Action redraw);
    }
}

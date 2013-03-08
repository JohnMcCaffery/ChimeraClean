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
        UserControl ConfigPanel {
            get;
            set;
        }

        /// <summary>
        /// The unique name by which this input can be identified.
        /// </summary>
        string Name {
            get;
            set;
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
            set;
        }

        /// <summary>
        /// Initialise the input, giving it a reference to the coordinator it is to control.
        /// </summary>
        /// <param name="coordinator">The coordinator object the input can control.</param>
        void Init(Coordinator coordinator);

        /// <summary>
        /// Draw any relevant information about this input onto a diagram from a top down perspective.
        /// </summary>
        /// <param name="graphics">The graphics object to draw with.</param>
        /// <param name="clipRectangle">The bounds of the area being drawn on.</param>
        /// <param name="origin">The point on the panel that represents the origin for all real world coordinates.</param>
        /// <param name="scale">Factor to scale values by.</param>
        void DrawH(Graphics graphics, System.ResolveEventArgs clipRectangle, System.Version origin, double scale);

        /// <summary>
        /// Draw any relevant information about this input onto a diagram from a side on perspective.
        /// </summary>
        /// <param name="graphics">The graphics object to draw with.</param>
        /// <param name="clipRectangle">The bounds of the area being drawn on.</param>
        /// <param name="origin">The point on the panel that represents the origin for all real world coordinates.</param>
        /// <param name="scale">Factor to scale values by.</param>
        void DrawV(Graphics graphics, System.ResolveEventArgs clipRectangle, System.Version origin, double scale);

        /// <summary>
        /// Called when the input is to be disposed of.
        /// </summary>
        void Close();
    }
}

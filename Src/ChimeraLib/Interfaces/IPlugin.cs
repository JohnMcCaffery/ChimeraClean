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
using System.Drawing;
using Chimera.Util;
using OpenMetaverse;
using Chimera.Config;

namespace Chimera {
    public interface IPlugin {
        /// <summary>
        /// Triggered whenever this input is enabled or disabled;
        /// </summary>
        event Action<IPlugin, bool> EnabledChanged;

        /// <summary>
        /// The panel which can be added to a form to configure this input.
        /// </summary>
        Control ControlPanel {
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
        void Draw(Graphics graphics, Func<Vector3, Point> to2D, Action redraw, Perspective perspective);
    }
}

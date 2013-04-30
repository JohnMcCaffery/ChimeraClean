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
using OpenMetaverse;
using System.Windows.Forms;
using Chimera.Util;

namespace Chimera {
    public interface IDeltaInput : IPlugin {
        /// <summary>
        /// Triggered whenever the values are updated.
        /// </summary>
        event Action<IDeltaInput> Change;

        /// <summary>
        /// The changes to position.
        /// </summary>
        Vector3 PositionDelta {
            get;
        }

        /// <summary>
        /// How much the orientation of the camera should change.
        /// </summary>
        Rotation OrientationDelta {
            get;
        }

        /// <summary>
        /// Whether this input should control x position.
        /// </summary>
        bool WalkEnabled {
            get;
            set;
        }
        /// <summary>
        /// Whether this input should control y position.
        /// </summary>
        bool StrafeEnabled {
            get;
            set;
        }
        /// <summary>
        /// Whether this input should control z position.
        /// </summary>
        bool FlyEnabled {
            get;
            set;
        }
        /// <summary>
        /// Whether this input should control yaw.
        /// </summary>
        bool YawEnabled {
            get;
            set;
        }
        /// <summary>
        /// Whether this input should control pitch.
        /// </summary>
        bool PitchEnabled {
            get;
            set;
        }

        /// <summary>
        /// Initialise the input. Linking it to an object that can provide information about keyboard input and ticks.
        /// </summary>
        /// <param name="input">The source of tick and keyboard events.</param>
        void Init(ITickSource input);
    }
}

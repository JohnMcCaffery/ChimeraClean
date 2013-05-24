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

namespace Chimera.Interfaces {
    /// <summary>
    /// The different options for binding an input axis to a camera axis.
    /// </summary>
    public enum AxisBinding {
        /// <summary>
        /// The input axis has not had a value assigned. These will get bound to None in startup.
        /// </summary>
        NotSet,
        /// <summary>
        /// The input axis is not bound to any camera axis.
        /// </summary>
        None,
        /// <summary>
        /// The input axis moves the camera forward or backward.
        /// </summary>
        X,
        /// <summary>
        /// The input axis strafes the camera left or right.
        /// </summary>
        Y,
        /// <summary>
        /// The input axis flies the camera up or down.
        /// </summary>
        Z,
        /// <summary>
        /// The input axis pitches the camera up or down.
        /// </summary>
        Pitch,
        /// <summary>
        /// The input axis yaws the camera left or right.
        /// </summary>
        Yaw
    }

    /// <summary>
    /// Interface for an input axis that can control the camera.
    /// </summary>
    public interface IAxis {
        /// <summary>
        /// The control panel which can be used to configure this axis.
        /// </summary>
        UserControl ControlPanel { get; }
        /// <summary>
        /// The value for the delta on this axis.
        /// Can be positive or negative.
        /// </summary>
        float Delta { get; }
        /// <summary>
        /// The camera axis this input axis is bound to.
        /// </summary>
        AxisBinding Binding { get; set;  }

        /// <summary>
        /// The name by which this axis can be identified. Is not necessarily unique.
        /// </summary>
        string Name { get; }
    }
}

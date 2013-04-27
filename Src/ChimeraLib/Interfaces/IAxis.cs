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
        /// Triggered whenever the axis updates.
        /// </summary>
        event Action Changed;

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
    }
}

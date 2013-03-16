using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using OpenMetaverse;
using Chimera.Util;

namespace Chimera.Kinect {
    public interface IKinectCursorWindowFactory {
        IKinectCursorWindow Make();
    }
    public interface IKinectCursorWindow {
        /// <summary>
        /// Triggered whenever the cursor enters the window controlled by this object.
        /// </summary>
        event Action CursorEnter;
        /// <summary>
        /// Triggered whenever the cursor leaves the window controlled by this object.
        /// </summary>
        event Action CursorLeave;
        /// <summary>
        /// Triggered whenever the cursor is on the screen and moves.
        /// </summary>
        event Action<int, int> CursorMove;
        /// <summary>
        /// Triggered whenever this controller is enabled or disabled.
        /// </summary>
        event Action<bool> EnabledChanged;

        /// <summary>
        /// The location of the cursor on screen.
        /// </summary>
        Point Location { get; }
        /// <summary>
        /// A panel which can be used to control the cursor.
        /// </summary>
        UserControl ControlPanel { get; }
        /// <summary>
        /// The state of the cursor control. Will be used to compile crash reports in the event of a crash.
        /// </summary>
        string State { get; }
        /// <summary>
        /// Whether the cursor is on the screen.
        /// </summary>
        bool OnScreen { get; }
        /// <summary>
        /// Whether this instance is allowed to control the cursor.
        /// </summary>
        bool Enabled { get; }

        /// <summary>
        /// Initialise the instance, linking it with a window it is to control.
        /// </summary>
        /// <param name="window">The window this cursor appears on.</param>
        /// <param name="position">The position of the device.</param>
        /// <param name="orientation">The position of the device.</param>
        void Init(Window window, Vector3 position, Rotation orientation);
        /// <summary>
        /// Set where the physical device is in the real world (mm).
        /// </summary>
        /// <param name="position">The position of the device.</param>
        void SetPosition(Vector3 position);
        /// <summary>
        /// Set where the physical device is pointed in the real world (mm).
        /// </summary>
        /// <param name="orientation">The position of the device.</param>
        void SetOrientation(Rotation orientation);
    }
}

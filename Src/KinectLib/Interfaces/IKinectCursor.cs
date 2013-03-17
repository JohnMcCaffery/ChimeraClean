using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using OpenMetaverse;
using Chimera.Util;

namespace Chimera.Kinect.Interfaces {
    public interface IKinectCursorFactory {
        IKinectCursor Make();
        /// <summary>
        /// The name of this type of cursor.
        /// </summary>
        string Name { get; }
    }
    public interface IKinectCursor {
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
        event Action<float, float> CursorMove;
        /// <summary>
        /// Triggered whenever this controller is enabled or disabled.
        /// </summary>
        event Action<bool> EnabledChanged;

        /// <summary>
        /// The location of the cursor on screen.
        /// Specified as percentages. 0,0 = top left, 1,1 = top right.
        /// </summary>
        PointF Location { get; }
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
        bool Enabled {
            get;
            set;
        }

        /// <summary>
        /// Initialise the instance, linking it with a window it is to control.
        /// </summary>
        /// <param name="controller">The kinect controller which this cursor works from.</param>
        /// <param name="window">The window this cursor appears on.</param>
        void Init(IKinectController controller, Window window);
    }
}

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
        event Action<IKinectCursor> CursorEnter;
        /// <summary>
        /// Triggered whenever the cursor leaves the window controlled by this object.
        /// </summary>
        event Action<IKinectCursor> CursorLeave;
        /// <summary>
        /// Triggered whenever the cursor is on the screen and moves.
        /// </summary>
        event Action<IKinectCursor, float, float> CursorMove;
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
        /// The window this cursor updates.
        /// </summary>
        Window Window { get; }
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
        /// <param name="controller">The kinect controller which this cursor works transition.</param>
        /// <param name="window">The window this cursor appears on.</param>
        void Init(IKinectController controller, Window window);
    }
}

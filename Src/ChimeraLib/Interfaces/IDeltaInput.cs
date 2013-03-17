using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;
using System.Windows.Forms;
using Chimera.Util;

namespace Chimera {
    public interface IDeltaInput : IInput {
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
        void Init(IInputSource input);
    }
}

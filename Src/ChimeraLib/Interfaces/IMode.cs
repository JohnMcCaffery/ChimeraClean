using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chimera {
    public interface IMode {
        /// <summary>
        /// The overlay area that this mode is triggered by.
        /// </summary>
        IOverlayArea OverlayArea {
            get;
            set;
        }

        /// <summary>
        /// A multi line string that can be printed to file to store a record of state in the event of a crash.
        /// </summary>
        string State {
            get;
            set;
        }

        /// <summary>
        /// The unique name by which the mode is known.
        /// </summary>
        string Name {
            get;
            set;
        }

        /// <summary>
        /// The type of mode this is.
        /// </summary>
        string Type {
            get;
            set;
        }

        /// <summary>
        /// Called when the mode should be engaged.
        /// </summary>
        void Select();

        /// <summary>
        /// Forces to mode to exit.
        /// </summary>
        void Disable();

        /// <summary>
        /// Initialise the mode, linking it with an overlay area which can trigger it.
        /// </summary>
        /// <param name="mainMenuArea">The overlay area this mode is tied to.</param>
        void Init(IOverlayArea mainMenuArea);

        /// <summary>
        /// Called when the coordinator is to be disposed of.
        /// </summary>
        void Close();
    }
}

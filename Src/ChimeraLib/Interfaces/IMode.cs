using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chimera {
    public interface IMode {
        IOverlayArea OverlayArea {
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

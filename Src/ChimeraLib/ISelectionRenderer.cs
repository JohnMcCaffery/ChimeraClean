using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chimera {
    public interface ISelectionRenderer {
        /// <summary>
        /// The selectable area this renderer is render selections for.
        /// </summary>
        ISelectable Selectable {
            get;
            set;
        }

        /// <summary>
        /// Draw on the overlay to indicate the selectable area is being hovered over.
        /// </summary>
        void DrawHover(string graphics);

        /// <summary>
        /// Draw on the overlay to indicate the selectable area is selected.
        /// </summary>
        void DrawSelected();

        /// <summary>
        /// Initialise this render, linking it to a selectable area.
        /// </summary>
        /// <param name="area">The area this renderer should highlight as hovering or selected.</param>
        void Init(ISelectable area);
    }
}

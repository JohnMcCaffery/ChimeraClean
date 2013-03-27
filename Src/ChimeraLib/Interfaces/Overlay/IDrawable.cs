using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chimera.Interfaces.Overlay {
    public interface IDrawable {
        /// <summary>
        /// Draw the elements of the drawable that only change when the overlay resizes.
        /// </summary>
        void DrawStatic();

        /// <summary>
        /// Draw the elements of the drawable that change dynamically.
        /// </summary>
        void DrawDynamic();
    }
}

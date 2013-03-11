using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.FlythroughLib;
using Chimera.Util;

namespace FlythroughLib {
    public interface IOrientationListener {
        /// <summary>
        /// Create a link to the sequence of orientations this listener wishes to track.
        /// </summary>
        /// <param name="positions">The orientations this listener will query.</param>
        void Init(EventSequence<Rotation> positions);
    }
}

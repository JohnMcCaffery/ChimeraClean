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
using Chimera.Flythrough;
using Chimera.Util;

namespace Chimera.Flythrough {
    public interface IOrientationListener {
        /// <summary>
        /// CreateWindowState a link to the sequence of orientations this listener wishes to track.
        /// </summary>
        /// <param name="positions">The orientations this listener will query.</param>
        void Init(EventSequence<Rotation> positions);
    }
}

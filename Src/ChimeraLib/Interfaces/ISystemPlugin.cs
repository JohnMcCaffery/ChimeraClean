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
using System.Windows.Forms;
using System.Drawing;
using Chimera.Util;

namespace Chimera {
    public interface ISystemPlugin : IPlugin {
        /// <summary>
        /// Initialise the plugin, giving it a reference to the plugin it is to control.
        /// </summary>
        /// <param name="core">The coordinator object the plugin can control.</param>
        void Init(Core core);

        /// <summary>
        /// Set the form which is controlling the coordinator. May never be called if the coordinator has been initialised without a form.
        /// </summary>
        /// <param name="form">The form which is the GUI for controlling the coordinator.</param>
        void SetForm(Form form);
    }
}

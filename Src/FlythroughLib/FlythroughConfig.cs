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
using Chimera.Util;
using Chimera.Config;

namespace Chimera.Flythrough {
    class FlythroughConfig : ConfigFolderBase {
        public bool SynchLengths;
        public bool Loop;
        public bool Autostart;
        public double Speed;
        public string StartFile;
        public string VideosphereFlythrough;
        public int VideosphereTimeIncrement;
        public string VideosphereFolder;
        public string VideosphereName;

        public override string Group {
            get { return "Flythrough"; }
        }

        public FlythroughConfig()
            : base("Flythrough") {
        }

        protected override void InitConfig() {
            SynchLengths = Get("SynchLengths", true, "Whether updating a position event's length will change the corresponding orientation event's length and vice versa.");
            Loop = Get("Loop", false, "Whether to loop playback by default.");
            StartFile = GetFile("DefaultFile", null, "Default file to load at startup.");
            Autostart = Get("Autostart", false, "If a default file is specified whether to start playing on system startup.");
            Speed = Get("Speed", 1.0, "How fast the playback should be. 1 is normal speed, < 1 is slower, > 1 is faster.");

            VideosphereFlythrough = GetFileSection("Videosphere", "Flythrough", null, "The flythrough file to use when recording the videosphere.");
            VideosphereFolder = GetFolderSection("Videosphere", "Folder", "Videospheres", "The folder where videospheres will be stored.");
            VideosphereName = GetSection("Videosphere", "Name", "Videosphere", "The folder where videospheres will be stored.");
            VideosphereTimeIncrement = Get("Videosphere", "TimeIncrement", 5, "The name of the videosphere being recorded.");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chimera.OpenSim.Interfaces {
    public interface IOpensimBotConfig {
        string FirstName { get; set; }

        string LastName { get; set; }

        string Password { get; set; }

        OpenMetaverse.Vector3 StartLocation { get; set; }

        string StartIsland { get; set; }

        bool AutoLogin { get; set; }
    }
}

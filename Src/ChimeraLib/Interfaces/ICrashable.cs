using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chimera.Interfaces {
    public interface ICrashable {
        bool AutoRestart { get; }
        void OnCrash(Exception e);
    }
}

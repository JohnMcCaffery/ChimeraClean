using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chimera.Interfaces {
    public interface ICrashable {
        void OnCrash(Exception e);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chimera.Interfaces {
    public interface ITickListener {
        void Init(ITickSource source);
    }
}

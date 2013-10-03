using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Chimera.Util {
    public static class InvokeExtension {
        public static void Invoke(this UserControl c, Action a) {
            if (!c.InvokeRequired)
                a();
            else if (!c.IsDisposed && !c.Disposing && c.Created)
                c.BeginInvoke(a);
        }
    }
}

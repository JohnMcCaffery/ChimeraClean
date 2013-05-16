using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Util;

namespace Chimera.OpenSim {
    class ViewerController : ProcessController {
        public ViewerController(string exe, string workingDir, string args)
            : base(exe, workingDir, args) {
        }

        public ViewerController() { }

        public void Close() {
            bool closed = false;
            object closeLock = new object();
            Action closeListener = () => {
                closed = true;
                lock (closeLock)
                    System.Threading.Monitor.PulseAll(closeLock);
            };
            Exited += closeListener;

            for (int i = 0; !closed && i < 5; i++) {
                PressKey("Q", true, false, false);
                lock (closeLock)
                    System.Threading.Monitor.Wait(closeLock, (i + 1) * 5000);
            }

            Exited -= closeListener;
        }
    }
}

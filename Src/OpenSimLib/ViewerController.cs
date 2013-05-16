using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Util;
using System.Threading;

namespace Chimera.OpenSim {
    class ViewerController : ProcessController {
        public ViewerController(string exe, string workingDir, string args)
            : base(exe, workingDir, args) {
        }

        public ViewerController() { }

        public void Close(bool blocking) {
            
           ThreadStart close = () => {
                bool closed = false;
                object closeLock = new object();
                Action closeListener = () => {
                    closed = true;
                    lock (closeLock)
                        System.Threading.Monitor.PulseAll(closeLock);
                };
                Exited += closeListener;

                for (int i = 0; !closed && i < 5; i++) {
                    PressKey("q", true, false, false);
                    lock (closeLock)
                        System.Threading.Monitor.Wait(closeLock, (i + 1) * 5000);
                }

                Exited -= closeListener;
            };

           if (blocking)
               close();
            else
               new Thread(close).Start();
        }

        internal void ToggleHUD() {
            PressKey("{F1}", true, false, true);
        }
    }
}

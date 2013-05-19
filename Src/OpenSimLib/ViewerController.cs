using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Util;
using System.Threading;

namespace Chimera.OpenSim {
    class ViewerController : ProcessController {
        private string mToggleHudKey = "^%{F1}";

        public ViewerController(string exe, string workingDir, string args)
            : base(exe, workingDir, args) {
        }

        public ViewerController(string toggleHUDKey) {
            mToggleHudKey = toggleHUDKey;
        }

        public void Close(bool blocking) {
            if (!Started)
                return;
            
           ThreadStart close = () => {
                bool closed = false;
                object closeLock = new object();
                Action closeListener = () => {
                    closed = true;
                    lock (closeLock)
                        System.Threading.Monitor.PulseAll(closeLock);
                };
                Exited += closeListener;

                for (int i = 0; !closed && Started && i < 5; i++) {
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
            PressKey(mToggleHudKey);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Util;
using System.Threading;
using log4net;
using System.Runtime.InteropServices;

namespace Chimera.OpenSim {
    public class ViewerController : ProcessController {
        private ILog Logger;
        private string mToggleHudKey = "^%{F1}";
        private string mName;

        public string Name { get { return mName; } }

        public ViewerController(string exe, string workingDir, string args, string name)
            : base(exe, workingDir, args) {

            mName = name;
            Logger = LogManager.GetLogger("OpenSim." + name + "Viewer");
        }

        public ViewerController(string toggleHUDKey, string name) {
            mToggleHudKey = toggleHUDKey;
            mName = name;

            Logger = LogManager.GetLogger("OpenSim." + name + "Viewer");
        }

        public void Close(bool blocking) {
            if (!Started)
                return;

            ThreadStart close = () => {
                Logger.Debug("Closing");
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

                Logger.Info("Closed");
                Exited -= closeListener;
            };

            if (blocking)
                close();
            else
                new Thread(close).Start();
        }

        public void ToggleHUD() {
            PressKey(mToggleHudKey);
        }

        internal void Split(bool left) {
            if (!Started || Monitor == null)
                return;

            System.Diagnostics.Process foreground = System.Diagnostics.Process.GetCurrentProcess();
            Int32 lStyle = GetWindowLong(Process.MainWindowHandle, GWL_STYLE);
            lStyle &= ~(WS_CAPTION | WS_THICKFRAME | WS_MINIMIZE | WS_MAXIMIZE | WS_SYSMENU);


            Int32 lExStyle = GetWindowLong(Process.MainWindowHandle, GWL_EXSTYLE);
            lExStyle &= ~(WS_EX_DLGMODALFRAME | WS_EX_CLIENTEDGE | WS_EX_STATICEDGE);

            SetWindowLong(Process.MainWindowHandle, GWL_STYLE, lStyle);
            SetWindowLong(Process.MainWindowHandle, GWL_EXSTYLE, lExStyle);

            //RECT bounds;
            //GetWindowRect(new HandleRef(Process, Process.MainWindowHandle), out bounds);
            //SetWindowPos(mProcess.MainWindowHandle, IntPtr.Zero, 0, 0, 0, 0, SWP_FRAMECHANGED | SWP_NOSIZE | SWP_NOREPOSITION | SWP_NOMOVE | SWP_NOZORDER | SWP_NOOWNERZORDER);

            int w = Monitor.Bounds.Width / 2;
            int x = Monitor.Bounds.X + (left ? 0 : w);

            SetWindowPos(Process.MainWindowHandle, IntPtr.Zero, x + 1, Monitor.Bounds.Y + 1, Monitor.Bounds.Width - 1, Monitor.Bounds.Height - 1, SWP_FRAMECHANGED | SWP_NOZORDER);
            SetWindowPos(Process.MainWindowHandle, IntPtr.Zero, x, Monitor.Bounds.Y, w, Monitor.Bounds.Height, SWP_FRAMECHANGED | SWP_NOZORDER);

            /*
            if (mMonitor != null) {
                SetWindowPos(mProcess.MainWindowHandle, IntPtr.Zero, mMonitor.Bounds.X + 50, mMonitor.Bounds.Y + 50, mMonitor.Bounds.Width - 50, mMonitor.Bounds.Height - 50, SWP_NOZORDER);
                SetWindowPos(mProcess.MainWindowHandle, IntPtr.Zero, mMonitor.Bounds.X, mMonitor.Bounds.Y, mMonitor.Bounds.Width, mMonitor.Bounds.Height, SWP_NOZORDER);
            }
            */
            BringToFront();
        }
    }
}

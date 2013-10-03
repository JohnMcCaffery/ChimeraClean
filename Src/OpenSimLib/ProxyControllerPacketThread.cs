using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Threading;
using Chimera.Util;

namespace Chimera.OpenSim {
    internal class ProxyControllerPacketThread {
#if DEBUG
        private readonly TickStatistics mStatistics = new TickStatistics();
#endif
        private readonly List<ProxyControllerBase> mControllers = new List<ProxyControllerBase>();
        private readonly Core mCore;
        private bool mCont = true;
        private ILog Logger = LogManager.GetLogger("OpenSim");

#if DEBUG
        public TickStatistics Statistics {
            get { return mStatistics; }
        }
#endif

        public ProxyControllerPacketThread(Core core, ProxyControllerBase controller) {
            mControllers.Add(controller);
            mCore = core;

            Thread t = new Thread(UpdateThread);
            t.Name = "CameraUpdate";
            t.Start();
        }

        public void AddController(ProxyControllerBase controller) {
                mControllers.Add(controller);
        }

        public void Stop() {
            mCont = false;
        }

        private void UpdateThread() {
            Thread.Sleep(500);
            while (mCont) {
#if DEBUG
                mStatistics.Begin();
#endif
                DateTime mStart = DateTime.Now;
                foreach (var controller in mControllers)
                    controller.UpdateCamera();
                double t = DateTime.Now.Subtract(mStart).TotalMilliseconds - mCore.TickLength;
#if DEBUG
                mStatistics.End();
#endif
                if (mCont && t > 0.0)
                    Thread.Sleep((int)t);

            }
            Logger.Info("Proxy Controller thread shut down.");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces;

namespace Chimera.OpenSim {
    public class OpenSimViewerFactory : IOutputFactory {
        private bool mPluginAssigned;
        private OpenSimController mPluginController;

        public OpenSimViewerFactory(IEnumerable<ISystemPlugin> plugins) {
            mPluginController = plugins.FirstOrDefault(p => p is OpenSimController) as OpenSimController;
        }

        public IOutput Create() {
            if (mPluginController != null && !mPluginAssigned) {
                mPluginAssigned = true;
                return mPluginController;
            }

            return new OpenSimController();
        }
    }
}

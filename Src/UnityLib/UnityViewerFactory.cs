using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces;

namespace Chimera.OpenSim
{
    public class UnityViewerFactory : IOutputFactory
    {
        private bool mPluginAssigned;

        public UnityViewerFactory(IEnumerable<ISystemPlugin> plugins)
        {
            
        }

        public IOutput Create()
        {
            return null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chimera.Interfaces;

namespace Chimera.Plugins
{
    public class StubOutputPluginFactory : IOutputFactory {
        public IOutput Create() {
            return new StubOutputPlugin();
        }
    }

    public class StubOutputPlugin : IOutput {
        private Frame mFrame;

        public Frame Frame {
            get { return mFrame; }
        }

        public Control ControlPanel {
            get { return new Control(); }
        }

        public bool AutoRestart {
            get { return false; }
            set { }
        }

        public string State {
            get { throw new NotImplementedException(); }
        }

        public string Type {
            get { return "Stub"; }
        }

        public Fill Fill {
            get {
                return Chimera.Fill.Windowed;
            }
            set { }
        }

        public bool Active {
            get { return false; }
        }

        public System.Diagnostics.Process Process {
            get { return null; }
        }

        public void Init(Frame frame) {
        }

        public bool Launch() {
            return false;
        }

        public void Close() {
        }

        public void Restart(string reason) {
        }
    }
}

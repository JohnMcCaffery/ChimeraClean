using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera;
using System.Windows.Forms;

namespace Chimera.RemoteControl {
    public class RemoteControlPlugin : ISystemPlugin {
        public const string SHUTDOWN = "Exit";
        private Core mCore;
        private Form mForm;

        public void Init(Core coordinator) {
            mCore = coordinator;
        }

        public void SetForm(System.Windows.Forms.Form form) {
            mForm = form;
        }

        public event Action<IPlugin, bool> EnabledChanged;

        public System.Windows.Forms.Control ControlPanel {
            get { return new Panel(); }
        }

        public bool Enabled {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }

        public string Name {
            get { throw new NotImplementedException(); }
        }

        public string State {
            get { throw new NotImplementedException(); }
        }

        public Config.ConfigBase Config {
            get { throw new NotImplementedException(); }
        }

        public void Close() {
        }

        public void Draw(System.Drawing.Graphics graphics, Func<OpenMetaverse.Vector3, System.Drawing.Point> to2D, Action redraw, Perspective perspective) { }
    }
}

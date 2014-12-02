using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Chimera.Plugins {
    public abstract class PluginBase<TControlPanel> : ISystemPlugin where TControlPanel : UserControl {
        protected Core mCore;
        protected Form mForm;
        protected TControlPanel mPanel;
        protected Func<PluginBase<TControlPanel>, TControlPanel> mCreatePanel;
        protected bool mEnabled;
        private string mName;

	protected PluginBase(string name, Func<PluginBase<TControlPanel>, TControlPanel> createPanel) {
		mCreatePanel = createPanel;
	}

        public virtual void Init(Core core) {
            mCore = core;
        }

        public void SetForm(Form form) {
            mForm = form;
        }

        public string State {
            get { throw new NotImplementedException(); }
        }

        public event Action<IPlugin, bool> EnabledChanged;

        public Control ControlPanel {
            get {
                if (mPanel == null)
                    mPanel = mCreatePanel(this);
                return mPanel;
            }
        }

        public bool Enabled {
            get { return mEnabled; }
            set {
                if (mEnabled != value) {
                    mEnabled = value;
                    if (EnabledChanged != null)
                        EnabledChanged(this, value);
                }
            }
        }

        public string Name {
            get { return mName; }
        }



        public abstract Config.ConfigBase Config { get; }

        public virtual void Close() { }

        public virtual void Draw(System.Drawing.Graphics graphics, Func<OpenMetaverse.Vector3, System.Drawing.Point> to2D, Action redraw, Perspective perspective) { }
    }
}

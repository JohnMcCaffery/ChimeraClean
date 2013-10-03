using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Overlay;
using Chimera.Util;

namespace Chimera.Features.Plugin {
    public class OverlayStatsPlugin : OverlayXmlLoader, ISystemPlugin {
        private readonly Dictionary<string, TickStatistics> mStatistics = new Dictionary<string,TickStatistics>();
        private TickStatistics mCurrentStats;
        private bool mEnabled;
        private OverlayPlugin mOverlay;

        public TickStatistics this[string state] {
            get { return mStatistics[state]; }
        }

        public TickStatistics this[State state] {
            get { return mStatistics[state.Name]; }
        }

        public OverlayPlugin Overlay {
            get { return mOverlay; }
        }

        public void Init(Core coordinator) {
            if (!coordinator.HasPlugin<OverlayPlugin>())
                return;

            mOverlay = coordinator.GetPlugin<OverlayPlugin>();

            foreach (var state in mOverlay.States)
                mStatistics.Add(state.Name, new TickStatistics());

            if (mOverlay.CurrentState != null)
                StateActivated(mOverlay.CurrentState);

            mOverlay.StateChanged += new Action<Overlay.State>(StateActivated);
        }

        private void StateActivated(State state) {
            if (mCurrentStats != null)
                mCurrentStats.End();

            mCurrentStats = mStatistics[state.Name];
            mCurrentStats.Begin();
        }

        #region ISystemPlugin members

        public void SetForm(System.Windows.Forms.Form form) { }

        public event Action<IPlugin, bool> EnabledChanged;

        public System.Windows.Forms.UserControl ControlPanel {
            get { throw new NotImplementedException(); }
        }

        public bool Enabled {
            get { return mEnabled; }
            set {
                if (value != mEnabled) {
                    mEnabled = value;
                    if (EnabledChanged != null)
                        EnabledChanged(this, value);
                }
            }
        }

        public string Name {
            get { return "StateStatistics"; }
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

        #endregion
    }
}

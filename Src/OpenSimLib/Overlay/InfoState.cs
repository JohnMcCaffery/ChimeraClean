/*************************************************************************
Copyright (c) 2012 John McCaffery 

This file is part of Chimera.

Chimera is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Chimera is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Chimera.  If not, see <http://www.gnu.org/licenses/>.

**************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using Chimera.Overlay;
using Chimera.Overlay.Drawables;
using System.Drawing;
using Chimera.Overlay.Triggers;
using Chimera.OpenSim;
using OpenMetaverse;
using System.Xml;

namespace Chimera.OpenSim.Overlay {
    public class InfoStateFactory : IStateFactory {
        public string Name {
            get { return "Info"; }
        }

        #region IFactory<IFeature> Members

        public State Create(OverlayPlugin manager, XmlNode node) {
            return new InfoState(manager, node);
        }

        public State Create(OverlayPlugin manager, XmlNode node, Rectangle clip) {
            return Create(manager, node);
        }

        #endregion
    }

    public class InfoState : State {
        private List<OpenSimController> mControllers = new List<OpenSimController>();
        private WindowOverlayManager mMainWindow;
        private OverlayPlugin mPlugin;
        private string mGlowString;
        private string mNoGlowString;
        private int mGlowChannel;

        public override IWindowState CreateWindowState(WindowOverlayManager manager) {
            return new WindowState(manager);
        }

        public InfoState(string name, OverlayPlugin manager, string mainWindow, string whereWindow)
            : base(name, manager) {

            mPlugin = manager;
            mMainWindow = manager[mainWindow];
        }

        public InfoState(OverlayPlugin manager, XmlNode node)
            : base(GetName(node, "information state"), manager) {

            mPlugin = manager;
            mGlowString = GetString(node, "Glow", "GlowMessage");
            mNoGlowString = GetString(node, "NoGlow", "NoGlowMessage");
            mGlowChannel = GetInt(node, -40, "GlowChannel");
        }

        protected override void TransitionToStart() {
            TransitionToFinish();
        }
        protected override void TransitionToFinish() {
            Manager.Coordinator.EnableUpdates = false;
            foreach (var manager in mPlugin.OverlayManagers)
                manager.ControlPointer = true;
            Chat(mGlowString);
        }

        protected override void TransitionFromStart() {
            Chat(mNoGlowString);
        }

        protected override void TransitionFromFinish() {
            Chat(mNoGlowString);
        }

        private void Chat(string msg) {
            foreach (var input in mControllers)
                input.ProxyController.Chat(msg, mGlowChannel);
        }
    }
}

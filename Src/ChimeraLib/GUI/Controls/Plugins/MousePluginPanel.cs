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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chimera.Plugins;
using Chimera.Util;

namespace Chimera.GUI.Controls.Plugins {
    public partial class MousePluginPanel : UserControl {
        private MousePlugin mPlugin;
        private Action<int, int> mMouseMovedListener;

        public MousePluginPanel() {
            InitializeComponent();
            mMouseMovedListener = new Action<int, int>(mPlugin_MouseMoved);
            HandleCreated += new EventHandler(MousePluginPanel_HandleCreated);
            Disposed += new EventHandler(MousePluginPanel_Disposed);
        }

        void MousePluginPanel_HandleCreated(object sender, EventArgs e) {
            if (mPlugin != null)
                mPlugin.MouseMoved += mMouseMovedListener;
        }

        void MousePluginPanel_Disposed(object sender, EventArgs e) {
            if (mPlugin != null)
                mPlugin.MouseMoved -= mMouseMovedListener;
        }

        public MousePluginPanel(MousePlugin input)
            : this() {
            Init(input);
        }

        public void Init(MousePlugin input) {
            mPlugin = input;
        }

        private void mPlugin_MouseMoved(int x, int y) {
            if (Created && !IsDisposed && !Disposing)
                Invoke(new Action(() => {
                    positionLabel.Text = string.Format("{0,-4},{1,-4}", x, y);
                    cursorHandleLabel.Text = ProcessWrangler.GetGlobalCursor().ToString();
                }));
        }
    }
}

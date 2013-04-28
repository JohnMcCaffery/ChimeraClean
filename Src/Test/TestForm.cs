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
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NuiLibDotNet;
using Chimera.Kinect.GUI;

namespace Test {
    public partial class TestForm : Form {
        public TestForm() {
            InitializeComponent();
        }

        public TestForm(Vector topLeft, Vector normal, Vector top, Vector side, Vector pointStart, Vector pointDir, Vector intersection, Scalar w, Scalar h, Scalar x, Scalar y)
            : this() {

            topLeftPanel.Vector = new VectorUpdater(topLeft);
            normalPanel.Vector = new VectorUpdater(normal);
            topPanel.Vector = new VectorUpdater(top);
            sidePanel.Vector = new VectorUpdater(side);
            pointStartPanel.Vector = new VectorUpdater(pointStart);
            pointDirPanel.Vector = new VectorUpdater(pointDir);
            intersectionPanel.Vector = new VectorUpdater(intersection);
            wPanel.Scalar = new ScalarUpdater(w);
            hPanel.Scalar = new ScalarUpdater(h);
            xPanel.Scalar = new ScalarUpdater(x);
            yPanel.Scalar = new ScalarUpdater(y);
        }
    }
}

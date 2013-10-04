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
using OpenMetaverse;

namespace Chimera.GUI {
    public partial class VectorPanel : UserControl {
        public event EventHandler ValueChanged;
        private bool externalSet = false;
        private bool guiSet = false;
        private Vector3 vector;

        public Vector3 Value {
            get { return vector; }
            set {
                if (vector.X == value.X && vector.Y == value.Y && vector.Z == value.Z)
                    return;
                Action change = new Action(() => {
                    vector = value;

                    if (!guiSet) {
                        externalSet = true;
                        xPanel.Value = vector.X;
                        yPanel.Value = vector.Y;
                        zPanel.Value = vector.Z;
                        externalSet = false;
                    }

                    if (ValueChanged != null)
                        ValueChanged(this, null);
                });
                if (!IsDisposed && !Disposing) {
                    if (InvokeRequired && Created)
                        BeginInvoke(change);
                    else if (!InvokeRequired)
                        change();
                }
            }
        }

        public float X {
            get { return (float)xPanel.Value; }
            set { xPanel.Value = value; }
        }
        public float Y {
            get { return (float)yPanel.Value; }
            set { yPanel.Value = value; }
        }
        public float Z {
            get { return (float)zPanel.Value; }
            set { zPanel.Value = value; }
        }

        public override string Text {
            get { return nameLabel.Text; }
            set { nameLabel.Text = value; }
        }
        public float Min {
            get { return xPanel.Min; }
            set {
                xPanel.Min = value;
                yPanel.Min = value;
                zPanel.Min = value;
            }
        }
        public Vector3 MinV {
            get { return new Vector3( xPanel.Min, yPanel.Min, zPanel.Min); }
            set {
                xPanel.Min = value.X;
                yPanel.Min = value.Y;
                zPanel.Min = value.Z;
            }
        }
        public float Max {
            get { return xPanel.Max; }
            set {
                xPanel.Max = value;
                yPanel.Max = value;
                zPanel.Max = value;
            }
        }
        public Vector3 MaxV {
            get { return new Vector3( xPanel.Max, yPanel.Max, zPanel.Max); }
            set {
                xPanel.Max = value.X;
                yPanel.Max = value.Y;
                zPanel.Max = value.Z;
            }
        }


        public VectorPanel() {
            InitializeComponent();
        }

        private void VectorPanel_EnabledChanged(object sender, EventArgs e) {
            /*
            xValue.Active = Active;
            yValue.Active = Active;
            zValue.Active = Active;
            xSlider.Active = Active;
            ySlider.Active = Active;
            zSlider.Active = Active;
            */
        }

        private void panel_Changed(float obj) {
            if (!externalSet) {
                guiSet = true;
                Value = new Vector3(xPanel.Value, yPanel.Value, zPanel.Value);
                guiSet = false;
            }

        }
    }
}

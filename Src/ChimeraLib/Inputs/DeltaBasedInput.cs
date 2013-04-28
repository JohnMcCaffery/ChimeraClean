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
using Chimera.Util;
using System.Windows.Forms;
using OpenMetaverse;
using System.Drawing;

namespace Chimera.Plugins {
    public class DeltaBasedInput : ISystemPlugin {
        private IDeltaInput mInput;
        private Coordinator mCoordinator;
        private bool mEnabled;
        private bool mDeltaActive;

        public bool WalkEnabled {
            get { return mInput.WalkEnabled; }
            set { mInput.WalkEnabled = value; }
        }
        public bool StrafeEnabled {
            get { return mInput.StrafeEnabled; }
            set { mInput.StrafeEnabled = value; }
        }
        public bool FlyEnabled {
            get { return mInput.FlyEnabled; }
            set { mInput.FlyEnabled = value; }
        }
        public bool PitchEnabled {
            get { return mInput.PitchEnabled; }
            set { }
        }
        public bool YawEnabled {
            get { return mInput.YawEnabled; }
            set { mInput.YawEnabled = value; }
        }

        public DeltaBasedInput(IDeltaInput input) {
            mInput = input;

            mInput.Change += new Action<IDeltaInput>(mInput_Change);
        }

        void mInput_Change(IDeltaInput input) {
            Vector3 move = mInput.PositionDelta;

            bool wasActive = mDeltaActive;
            mDeltaActive = move != Vector3.Zero || mInput.OrientationDelta.Pitch != 0.0 || mInput.OrientationDelta.Yaw != 0.0;

            //TODO - handle keyboard rotation
            if (mInput.Enabled && (mDeltaActive || wasActive)) {
                if (mCoordinator.ControlMode == ControlMode.Absolute) {
                    float fly = move.Z;
                    move.Z = 0f;
                    move *= mCoordinator.Orientation.Quaternion;
                    move.Z += fly;

                    Vector3 pos = mCoordinator.Position + move;
                    Rotation orientation = mCoordinator.Orientation + mInput.OrientationDelta;
                    mCoordinator.Update(pos, move, orientation, mInput.OrientationDelta);
                } else
                    mCoordinator.Update(Vector3.Zero, input.PositionDelta, Rotation.Zero, mInput.OrientationDelta);
            }
        }

        #region ISystemPlugin Members

        public event Action<IPlugin, bool> EnabledChanged;

        public virtual UserControl ControlPanel {
            get { return mInput.ControlPanel; }
        }

        public virtual string Name {
            get { return mInput.Name; }
        }

        public virtual bool Enabled {
            get { return mInput.Enabled; }
            set { 
                mInput.Enabled = value;
                if (EnabledChanged != null)
                    EnabledChanged(this, value);
            }
        }

        public virtual ConfigBase Config {
            get { return mInput.Config; }
        }

        public virtual string State {
            get { return mInput.State; }
        }

        public void Init(Coordinator coordinator) {
            mCoordinator = coordinator;
            mInput.Init(coordinator);
        }

        public void Close() {
            mInput.Close();
        }

        public void Draw(Func<Vector3, Point> to2D, Graphics graphics, Action redraw) {
            mInput.Draw(to2D, graphics, redraw);
        }

        #endregion
    }
}

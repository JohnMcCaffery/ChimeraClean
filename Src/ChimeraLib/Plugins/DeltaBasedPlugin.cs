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
using Chimera.Config;

namespace Chimera.Plugins {
    public abstract class DeltaBasedPlugin : ISystemPlugin {
        private Core mCoordinator;
        private bool mDeltaActive;

        private bool mEnableX = true;
        private bool mEnableY = true;
        private bool mEnableZ = true;
        private bool mEnablePitch = true;
        private bool mEnableYaw = true;
        private bool mEnabled = true;

        public Action<DeltaBasedPlugin> Change;


        public bool WalkEnabled {
            get { return mEnableX; }
            set {
                if (value != mEnableX) {
                    mEnableX = value;
                    if (EnabledChanged != null)
                        EnabledChanged(this, value);
                }
            }
        }

        public bool StrafeEnabled {
            get { return mEnableY; }
            set {
                if (value != mEnableY) {
                    mEnableY = value;
                    if (EnabledChanged != null)
                        EnabledChanged(this, value);
                }
            }
        }

        public bool FlyEnabled {
            get { return mEnableZ; }
            set {
                if (value != mEnableZ) {
                    mEnableZ = value;
                    if (EnabledChanged != null)
                        EnabledChanged(this, value);
                }
            }
        }

        public bool YawEnabled {
            get { return mEnableYaw; }
            set {
                if (value != mEnableYaw) {
                    mEnableYaw = value;
                    if (EnabledChanged != null)
                        EnabledChanged(this, value);
                }
            }
        }

        public bool PitchEnabled {
            get { return mEnablePitch; }
            set {
                if (value != mEnablePitch) {
                    mEnablePitch = value;
                    if (EnabledChanged != null)
                        EnabledChanged(this, value);
                }
            }
        }

        protected void TriggerChange(DeltaBasedPlugin input) {
            Vector3 moveDelta = PositionDelta;
            moveDelta.X = mEnableX ? moveDelta.X : 0f;
            moveDelta.Y = mEnableY ? moveDelta.Y : 0f;
            moveDelta.Z = mEnableZ ? moveDelta.Z : 0f;

            Rotation orientationDelta = OrientationDelta;
            orientationDelta.Pitch = mEnablePitch ? orientationDelta.Pitch : 0.0;
            orientationDelta.Yaw = mEnableYaw ? orientationDelta.Yaw : 0.0;

            bool wasActive = mDeltaActive;
            mDeltaActive = moveDelta != Vector3.Zero || orientationDelta.Pitch != 0.0 || orientationDelta.Yaw != 0.0;

            //TODO - handle keyboard rotation
            if (Enabled && (mDeltaActive || wasActive)) {
                if (mCoordinator.ControlMode == ControlMode.Absolute) {
                    float fly = moveDelta.Z;
                    moveDelta.Z = 0f;
                    moveDelta *= mCoordinator.Orientation.Quaternion;
                    moveDelta.Z += fly;

                    Vector3 pos = mCoordinator.Position + moveDelta;
                    Rotation orientation = mCoordinator.Orientation + orientationDelta;
                    mCoordinator.Update(pos, moveDelta, orientation, orientationDelta);
                } else
                    mCoordinator.Update(Vector3.Zero, moveDelta, Rotation.Zero, orientationDelta);
                if (Change != null)
                    Change(this);
            }
        }

        #region ISystemPlugin Members

        public event Action<IPlugin, bool> EnabledChanged;

        public abstract Control ControlPanel {
            get;
        }

        public abstract string Name {
            get;
        }

        public virtual bool Enabled {
            get { return mEnabled; }
            set { 
                mEnabled = value;
                if (!value && !mEnabled && mCoordinator != null && mCoordinator.ControlMode == ControlMode.Delta) {
                    bool enable = mCoordinator.EnableUpdates;
                    mCoordinator.EnableUpdates = true;
                    mCoordinator.Update(mCoordinator.Position, Vector3.Zero, mCoordinator.Orientation, Rotation.Zero);
                    mCoordinator.EnableUpdates = enable;
                }
                if (EnabledChanged != null)
                    EnabledChanged(this, value);
            }
        }

        public abstract ConfigBase Config {
            get;
        }

        public abstract string State {
            get;
        }

        public abstract void Close();

        public abstract void Draw(Graphics graphics, Func<Vector3, Point> to2D, Action redraw, Perspective perspective);

        #endregion



        /// <summary>
        /// The changes to position.
        /// </summary>
        public abstract Vector3 PositionDelta {
            get;
        }

        /// <summary>
        /// How much the orientation of the camera should change.
        /// </summary>
        public abstract Rotation OrientationDelta {
            get;
        }


        /// <summary>
        /// Initialise the input. Linking it to an object that can provide information about keyboard input and ticks.
        /// </summary>
        /// <param name="input">The source of tick and keyboard events.</param>
        public virtual void Init(Core input) {
            mCoordinator = input;
        }

        #region ISystemPlugin Members


        public virtual void SetForm(Form form) {
        }

        #endregion
    }
}

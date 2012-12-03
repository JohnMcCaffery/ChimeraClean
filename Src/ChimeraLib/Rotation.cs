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
using OpenMetaverse;

namespace UtilLib {
    public class Rotation {
        public static readonly double RAD2DEG = 180.0 / Math.PI;
        public static readonly double DEG2RAD = Math.PI / 180.0;

        /// <summary>
        /// Triggered whenever any of the values change.
        /// </summary>
        public event EventHandler OnChange;

        private Quaternion rotation = Quaternion.Identity;
        private Vector3 lookAtVector = Vector3.UnitX;
        private float yaw, pitch;

        private bool mRotationChanging = false;
        private bool mVectorChanging = false;
        private bool mYawChanging = false;
        private bool mPitchChanging = false;
        /// <summary>
        /// MasterRotation of the camera around the y axis (vertical).
        /// </summary>
        public float Pitch {
            get { return pitch; }
            set {
                value = constrain(value);
                if (pitch == value)
                    return;
                pitch = value;
                mPitchChanging = true;
                if (!mRotationChanging)
                    Quaternion = CalculateRotation();
                mPitchChanging = false;
            }
        }

        /// <summary>
        /// MasterRotation of the camera around the z axis (horizontal).
        /// </summary>
        public float Yaw {
            get { return yaw; }
            set {
                value = constrain(value);
                if (yaw == value)
                    return;
                yaw = value;
                mYawChanging = true;
                if (!mRotationChanging)
                    Quaternion = CalculateRotation();
                mYawChanging = false;
            }
        }

        /// <summary>
        /// MasterRotation of the Camera.
        /// </summary>
        public Quaternion Quaternion {
            get { return rotation; }
            set {
                if (rotation.Equals(value))
                    return;
                rotation = value;

                mRotationChanging = true;
                if (!mPitchChanging && !mYawChanging) {
                    float roll, pitch, yaw;
                    value.GetEulerAngles(out roll, out pitch, out yaw);
                    Pitch = (float) (pitch * RAD2DEG);
                    Yaw = (float) (yaw * RAD2DEG);
                }
                if (!mVectorChanging) 
                    lookAtVector = Vector3.UnitX * value;

                if (OnChange != null)
                    OnChange(this, null);
                mRotationChanging = false;
            }
        }

        /// <summary>
        /// Direction the camera is looking.
        /// </summary>
        public Vector3 LookAtVector {
            get { return lookAtVector; }
            set {
                if (value.Equals(lookAtVector))
                    return;
                lookAtVector = value;

                mVectorChanging = true;
                if (!mRotationChanging) {
                    Vector3 cross = Vector3.Cross(Vector3.UnitX, value);
                    float dot = (float)Math.Acos(Vector3.Dot(Vector3.UnitX, Vector3.Normalize(value)));
                    Quaternion = Quaternion.CreateFromAxisAngle(cross, dot);
                }
                mVectorChanging = false;
            }
        }

        /// <summary>
        /// Initialise the rotation with 0 yaw and pitch.
        /// </summary>
        public Rotation() { }

        /// <summary>
        /// Initialise the VirtualRotationOffset looking along the specified vector.
        /// </summary>
        /// <param name="lookAtVector">A vector representing the direction along which this VirtualRotationOffset starts looking.</param>
        public Rotation(Vector3 lookAtVector) {
            LookAtVector = lookAtVector;
        }

        /// <summary>
        /// Initiliase the VirtualRotationOffset with the specified yaw and pitch.
        /// </summary>
        /// <param name="pitch">The pitch value for this VirtualRotationOffset to start with.</param>
        /// <param name="yaw">The yaw value for this VirtualRotationOffset to start with.</param>
        public Rotation(float pitch, float yaw) {
            Pitch = pitch;
            Yaw = yaw;
        }

        /// <summary>
        /// Initialise the VirtualRotationOffset with the specified VirtualRotationOffset.
        /// </summary>
        /// <param name="rotation">The quaternion value representing this Rotations starting rotation.</param>
        public Rotation(Quaternion rotation) {
            Quaternion = rotation;
        }

        private Quaternion CalculateRotation() {
            Quaternion yaw = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, (float) (this.yaw * DEG2RAD));
            Vector3 normal = Vector3.UnitY * yaw;
            Quaternion pitch = Quaternion.CreateFromAxisAngle(normal, (float) (this.pitch * DEG2RAD));

            return pitch * yaw;
        }

        private float constrain(float x) {
            x += 180;
            x %= 360;
            x += x > 0 ? -180 : 180;
            return x;
        }
    }
}

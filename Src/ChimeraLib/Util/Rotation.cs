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

namespace Chimera.Util {
    public class Rotation {
        /*public static bool operator!=(Rotation rot1, Rotation rot2) {
            return  !(rot1.Equals(null) && rot2.Equals(null)) &&
                   (rot1.Equals(null) ||
                    rot2.Equals(null) || 
                    rot1.Yaw != rot2.Yaw || 
                    rot1.Pitch != rot2.Pitch);
        }
        public static bool operator==(Rotation rot1, Rotation rot2) {
            return !rot1.Equals(null) && !rot2.Equals(null) && rot1.Yaw == rot2.Yaw && rot1.Pitch == rot2.Pitch;
        }*/
        public static Rotation operator +(Rotation r1, Rotation r2) {
            return new Rotation(r1.Pitch + r2.Pitch, r1.Yaw + r2.Yaw);
        }

        public static readonly double RAD2DEG = 180.0 / Math.PI;
        public static readonly double DEG2RAD = Math.PI / 180.0;

        /// <summary>
        /// Triggered whenever any of the values change.
        /// </summary>
        public event EventHandler OnChange;

        private Quaternion mRotation = Quaternion.Identity;
        private Vector3 mLookAtVector = Vector3.UnitX;
        private double mYaw, mPitch;

        private bool mRotationChanging = false;
        private bool mVectorChanging = false;
        private bool mYawChanging = false;
        private bool mPitchChanging = false;
        /// <summary>
        /// MasterRotation of the camera around the y axis (vertical).
        /// </summary>
        public double Pitch {
            get { return mPitch; }
            set {
                if (mReadOnlyLock != null)
                    throw new Exception("Unable to update readonly rotation.");
                Set(value, mYaw);
            }
        }

        /// <summary>
        /// MasterRotation of the camera around the z axis (horizontal).
        /// </summary>
        public double Yaw {
            get { return mYaw; }
            set {
                if (mReadOnlyLock != null)
                    throw new Exception("Unable to update readonly rotation.");
                Set(mPitch, value);
            }
        }

        /// <summary>
        /// MasterRotation of the Camera.
        /// </summary>
        public Quaternion Quaternion {
            get { return mRotation; }
            set {
                if (mReadOnlyLock != null)
                    throw new Exception("Unable to update readonly rotation.");
                Set(value);
            }
        }

        /// <summary>
        /// Direction the camera is looking.
        /// </summary>
        public Vector3 LookAtVector {
            get { return mLookAtVector; }
            set {
                if (mReadOnlyLock != null)
                    throw new Exception("Unable to update readonly rotation.");
                Set(value);
            }
        }

        /// <summary>
        /// Initialise the lookAt with 0 mYaw and mPitch.
        /// </summary>
        public Rotation() { }

        /// <summary>
        /// Initialise the Rotation from the specified lookAt.
        /// </summary>
        /// <param name="mLookAtVector">Another lookAt to copy the values from.</param>
        public Rotation(Rotation rotation) {
            LookAtVector = rotation.LookAtVector;
        }

        /// <summary>
        /// Initialise the Rotation looking along the specified vector.
        /// </summary>
        /// <param name="mLookAtVector">A vector representing the direction along which this VirtualRotationOffset starts looking.</param>
        public Rotation(Vector3 lookAtVector) {
            LookAtVector = lookAtVector;
        }

        /// <summary>
        /// Initiliase the Rotation with the specified mYaw and mPitch.
        /// </summary>
        /// <param name="mPitch">The mPitch value for this VirtualRotationOffset to start with.</param>
        /// <param name="mYaw">The mYaw value for this VirtualRotationOffset to start with.</param>
        public Rotation(double pitch, double yaw) {
            Set(pitch, yaw);
        }

        /// <summary>
        /// Initialise the VirtualRotationOffset with the specified VirtualRotationOffset.
        /// </summary>
        /// <param name="lookAt">The quaternion value representing this Rotations starting lookAt.</param>
        public Rotation(Quaternion rotation) {
            Quaternion = rotation;
        }

        /// <summary>
        /// Initialise the rotation as read only. Yaw and Pitch are 0.
        /// </summary>
        /// <param name="readonlyLock">Initialise the rotation with a lock object. The only way to update this rotation will be to use an Update method and pass in the same lock object.</param>
        public Rotation(object readonlyLock) {
            mReadOnlyLock = readonlyLock;
        }

        /// <summary>
        /// Initialise the Rotation as read only looking along the specified vector.
        /// </summary>
        /// <param name="mLookAtVector">A vector representing the direction along which this VirtualRotationOffset starts looking.</param>
        /// <param name="readonlyLock">Initialise the rotation with a lock object. The only way to update this rotation will be to use an Update method and pass in the same lock object.</param>
        public Rotation(object readonlyLock, Vector3 lookAtVector)
            : this(readonlyLock) {
            LookAtVector = lookAtVector;
        }

        /// <summary>
        /// Initialise the Rotation as read only with the specified VirtualRotationOffset.
        /// </summary>
        /// <param name="lookAt">The quaternion value representing this Rotations starting lookAt.</param>
        /// <param name="readonlyLock">Initialise the rotation with a lock object. The only way to update this rotation will be to use an Update method and pass in the same lock object.</param>
        public Rotation(object readonlyLock, Quaternion rotation)
            : this(readonlyLock) {
            Quaternion = rotation;
        }

        /// <summary>
        /// Initialise the Rotation as read only with values from the specified lookAt.
        /// </summary>
        /// <param name="mLookAtVector">Another lookAt to copy the values from.</param>
        /// <param name="readonlyLock">Initialise the rotation with a lock object. The only way to update this rotation will be to use an Update method and pass in the same lock object.</param>
        public Rotation(object readonlyLock, Rotation rotation)
            : this(readonlyLock) {
            LookAtVector = rotation.LookAtVector;
        }

        /// <summary>
        /// Initiliase the Rotation as read only with the specified mYaw and mPitch.
        /// </summary>
        /// <param name="mPitch">The mPitch value for this VirtualRotationOffset to start with.</param>
        /// <param name="mYaw">The mYaw value for this VirtualRotationOffset to start with.</param>
        /// <param name="readonlyLock">Initialise the rotation with a lock object. The only way to update this rotation will be to use an Update method and pass in the same lock object.</param>
        public Rotation(object readonlyLock, float pitch, float yaw)
            : this(readonlyLock) {
            Pitch = pitch;
            Yaw = yaw;
        }

        private Quaternion CalculateRotation() {
            Quaternion yaw = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, (float) (this.mYaw * DEG2RAD));
            Vector3 normal = Vector3.UnitY * yaw;
            Quaternion pitch = Quaternion.CreateFromAxisAngle(normal, (float) (this.mPitch * DEG2RAD));

            return pitch * yaw;
        }

        private double constrain(double x) {
            x += 180.0;
            x %= 360.0;
            x += x > 0 ? -180.0 : 180.0;
            return x;
        }

        private object mReadOnlyLock;

        /// <summary>
        /// Update a readonly rotation.
        /// </summary>
        /// <param name="readonlyLock">Lock object to update readonly rotation.</param>
        /// <param name="lookAt">Pitch to set to.</param>
        /// <param name="yaw">Yaw to set to.</param>
        public void Update(object readonlyLock, double pitch, double yaw) {
            if (readonlyLock == mReadOnlyLock)
                Set(pitch, yaw);
            else 
                throw new Exception("Unable to update readonly rotation, wrong authentication object.");
        }

        /// <summary>
        /// Update a readonly rotation.
        /// </summary>
        /// <param name="readonlyLock">Lock object to update readonly rotation.</param>
        /// <param name="lookAt">Look at vector to set to.</param>
        public void Update(object readonlyLock, Vector3 lookAt) {
            if (readonlyLock == mReadOnlyLock)
                Set(lookAt);
            else 
                throw new Exception("Unable to update readonly rotation, wrong authentication object.");
        }

        /// <summary>
        /// Update a readonly rotation.
        /// </summary>
        /// <param name="readonlyLock">Lock object to update readonly rotation.</param>
        /// <param name="rotation">The quaternion to set the rotation to.</param>
        /// <param name="lookAt">Pitch to set to.</param>
        public void Update(object readonlyLock, Quaternion rotation) {
            if (readonlyLock == mReadOnlyLock)
                Set(rotation);
            else 
                throw new Exception("Unable to update readonly rotation, wrong authentication object.");
        }

        /// <summary>
        /// Update a readonly rotation.
        /// </summary>
        /// <param name="readonlyLock">Lock object to update readonly rotation.</param>
        /// <param name="rotation">The quaternion to set the rotation to.</param>
        /// <param name="lookAt">Pitch to set to.</param>
        public void Update(object readonlyLock, Rotation rotation) {
            if (readonlyLock == mReadOnlyLock)
                Set(rotation.Quaternion);
            else 
                throw new Exception("Unable to update readonly rotation, wrong authentication object.");
        }

        private void Set(double pitch, double yaw) {
            pitch = constrain(pitch);
            yaw = constrain(yaw);
            if (mPitch == pitch && mYaw == yaw)
                return;
            mPitch = pitch;
            mYaw = yaw;
            mPitchChanging = true;
            mYawChanging = true;
            if (!mRotationChanging)
                Quaternion = CalculateRotation();
            mPitchChanging = false;
            mYawChanging = false;
        }

        private void Set(Quaternion value) {
            if (mRotation.Equals(value))
                return;
            mRotation = value;

            mRotationChanging = true;
            if (!mPitchChanging && !mYawChanging) {
                float roll, pitch, yaw;
                value.GetEulerAngles(out roll, out pitch, out yaw);
                Set(pitch * RAD2DEG, yaw * RAD2DEG);
            }
            if (!mVectorChanging)
                mLookAtVector = Vector3.UnitX * value;

                if (OnChange != null)
                    OnChange(this, null);
                mRotationChanging = false;
        }

        private void Set(Vector3 value) {
            if (value.Equals(mLookAtVector))
                return;
            mLookAtVector = value;

            mVectorChanging = true;
            if (!mRotationChanging) {
                Vector3 cross = Vector3.Cross(Vector3.UnitX, value);
                float dot = (float)Math.Acos(Vector3.Dot(Vector3.UnitX, Vector3.Normalize(value)));
                Quaternion = Quaternion.CreateFromAxisAngle(cross, dot);
            }
            mVectorChanging = false;
        }
    }
}

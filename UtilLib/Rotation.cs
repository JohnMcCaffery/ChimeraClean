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
                    Rot = CalculateRotation();
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
                    Rot = CalculateRotation();
                mYawChanging = false;
            }
        }

        /// <summary>
        /// MasterRotation of the Camera.
        /// </summary>
        public Quaternion Rot {
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

                mVectorChanging = true;
                if (!mRotationChanging) {
                    Vector3 cross = Vector3.Cross(Vector3.UnitX, value);
                    float dot = (float)Math.Acos(Vector3.Dot(Vector3.UnitX, Vector3.Normalize(value)));
                    Rot = Quaternion.CreateFromAxisAngle(cross, dot);
                }
                mVectorChanging = false;
            }
        }

        private Quaternion CalculateRotation() {
            Quaternion yaw = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, (float) (this.yaw * DEG2RAD));
            Vector3 newDir = Vector3.UnitX * yaw;
            Vector3 normal = Vector3.Cross(Vector3.UnitZ, newDir);
            Quaternion pitch = Quaternion.CreateFromAxisAngle(normal, this.pitch);

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

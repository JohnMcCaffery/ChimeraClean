/*************************************************************************
Copyright (c) 2012 John McCaffery 

This file is part of Armadillo SlaveProxy.

Routing Project is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Routing Project is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Routing Project.  If not, see <http://www.gnu.org/licenses/>.

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
using UtilLib;

namespace ProxyTestGUI {
    public partial class RotationPanel : UserControl {
        private readonly Rotation rotation = new Rotation();
        public event EventHandler OnChange;

        public Quaternion Rotation {
            get { return rotation.Rot; }
            set { rotation.Rot = value; }
        }
        public Vector3 Vector {
            get { return rotation.LookAtVector; }
            set { rotation.LookAtVector = value; }
        }
        public float Yaw {
            get { return rotation.Yaw; }
            set { rotation.Yaw = value; }
        }
        public float Pitch {
            get { return rotation.Pitch; }
            set { rotation.Pitch = value; }
        }

        public RotationPanel() {
            InitializeComponent();

            rotation.OnChange += (src, args) => {
                vectorPanel.Value = rotation.LookAtVector;
                pitchValue.Value = new decimal (rotation.Pitch);
                pitchSlider.Value = (int)rotation.Pitch;
                yawValue.Value = new decimal (rotation.Yaw);
                yawSlider.Value = (int)rotation.Yaw;
                if (OnChange != null)
                    OnChange(this, null);
            };
        }


        public string DisplayName {
            get { return vectorPanel.DisplayName; }
            set { vectorPanel.DisplayName = value; }
        }

        private void yawSlider_Scroll(object sender, EventArgs e) {
            yawValue.Value = yawSlider.Value;
        }

        private void pitchSlider_Scroll(object sender, EventArgs e) {
            pitchValue.Value = pitchSlider.Value;
        }

        private void yawValue_ValueChanged(object sender, EventArgs e) {
            rotation.Yaw = (float) decimal.ToDouble(yawValue.Value);
        }

        private void pitchValue_ValueChanged(object sender, EventArgs e) {
            rotation.Pitch = (float) decimal.ToDouble(pitchValue.Value);
        }

        private void vectorPanel_Load(object sender, EventArgs e) {
            rotation.LookAtVector = vectorPanel.Value;
        }

        /*
        private Quaternion pitch = Quaternion.Identity, yaw = Quaternion.Identity, rotation = Quaternion.Identity;
        private float oldYaw, oldPitch;
        public Quaternion MasterRotation {
            get { return rotation; }
            set {
                Action change = () => {
                    mRotationChanging = true;
                    rotation = value;
                    Vector3 newVec = Vector3.UnitX * value;
                    float roll, pitch, yaw;
                    value.GetEulerAngles(out roll, out pitch, out yaw);

                    pitch *= (float)MasterRotation.RAD2DEG;
                    yaw *= (float)MasterRotation.RAD2DEG;

                    pitch = pitch > 180 ? -180 + (180-pitch) : pitch;
                    pitch = pitch < -180 ? 180-(180+pitch) : pitch;

                    yaw = yaw > 180 ? -180 + (180-yaw) : yaw;
                    yaw = yaw < -180 ? 180-(180+yaw) : yaw;

                    if (oldPitch != pitch || oldYaw != yaw) {
                        if (!mPitchChanging && !mYawChanging) {
                            pitchValue.Value = new decimal(pitch);
                            yawValue.Value = new decimal(yaw);
                        }
                        if (!mVectorChanging)
                            vectorPanel.Value = Vector3.UnitX * value;
                        if (OnChange != null)
                            OnChange(this, null);
                    }
                    mRotationChanging = false;
                };
                if (InvokeRequired)
                    Invoke(change);
                else
                    change();
            }
        }
        public Vector3 Vector {
            get { return vectorPanel.Value; }
            set { vectorPanel.Value = value; }
        }
        public float Yaw {
            get { return (float)decimal.ToDouble(yawValue.Value); }
            set { yawValue.Value = new decimal(constrain(value)); }
        }
        public float Pitch {
            get { return (float)decimal.ToDouble(pitchValue.Value); }
            set { pitchValue.Value = new decimal(constrain(value)); }
        }

        private float constrain(float x) {
            x += 180;
            x %= 360;
            x += x > 0 ? -180 : 180;
            return x;
        }

        private bool mRotationChanging = false;
        private bool mVectorChanging = false;
        private bool mYawChanging = false;
        private bool mYawSliderChanging = false;
        private bool mPitchChanging = false;
        private bool mPitchSliderChanging = false;

        private void vectorPanel_OnChange(object sender, EventArgs e) {
            mVectorChanging = true;

            if (!mRotationChanging) {
                Vector3 cross = Vector3.Cross(Vector3.UnitX, vectorPanel.Value);
                float dot = (float) Math.Acos(Vector3.Dot(Vector3.UnitX, Vector3.Normalize(vectorPanel.Value)));

                MasterRotation = Quaternion.CreateFromAxisAngle(cross, dot);
            }
            mVectorChanging = false;
        }

        private void yawValue_ValueChanged(object sender, EventArgs e) {
            float yaw = (float)decimal.ToDouble(yawValue.Value);
            if (oldYaw == yaw)
                return;
            mYawChanging = true;
            if (!mRotationChanging)
                MasterRotation = CalculateRotation();

            if (!mYawSliderChanging) {
                int val = decimal.ToInt32(yawValue.Value);
                int modifiedVal = val > 180 ? (val - 360) : (val < -180 ? 360 + val : val);
                if (modifiedVal != val) {
                    yawValue.Value = new decimal(modifiedVal);
                }
                yawSlider.Value = modifiedVal;
            }
            oldYaw = yaw;
            mYawChanging = false;
        }

        private void pitchValue_ValueChanged(object sender, EventArgs e) {
            float pitch = (float)decimal.ToDouble(pitchValue.Value);
            if (oldPitch == pitch)
                return;
            mPitchChanging = true;
            if (!mRotationChanging)
                MasterRotation = CalculateRotation();

            if (!mPitchSliderChanging) {
                int val = decimal.ToInt32(pitchValue.Value);
                int modifiedVal = val > 180 ? (val - 360) : (val < -180 ? 360 + val : val);
                if (modifiedVal != val)
                    pitchValue.Value = new decimal(modifiedVal);
                pitchSlider.Value = modifiedVal > 90 ? 90 : (modifiedVal < -90 ? -90 : modifiedVal);
            }
            oldPitch = pitch;
            mPitchChanging = false;
        }

        private Quaternion CalculateRotation() {
            //Quaternion yaw = Quaternion.CreateFromEulers(0f, 0f, (float) (decimal.ToDouble(yawValue.Value) * DEG2RAD));
            Quaternion yaw = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, (float) (decimal.ToDouble(yawValue.Value) * DEG2RAD));
            Vector3 newDir = Vector3.UnitX * yaw;
            Vector3 normal = Vector3.Cross(Vector3.UnitZ, newDir);
            Quaternion pitch = Quaternion.CreateFromAxisAngle(normal, (float)(decimal.ToDouble(pitchValue.Value)  * DEG2RAD));

            return pitch * yaw;
        }

        private void yawSlider_Scroll(object sender, EventArgs e) {
            if (!mYawChanging) {
                mYawSliderChanging = true;
                yawValue.Value = yawSlider.Value;
                mYawSliderChanging = false;
            }
        }

        private void pitchSlider_Scroll(object sender, EventArgs e) {
            if (!mPitchChanging) {
                mPitchSliderChanging = true;
                pitchValue.Value = pitchSlider.Value;
                mPitchSliderChanging = false;
            }
        }

        private void RotationPanel_EnabledChanged(object sender, EventArgs e) {
            vectorPanel.Enabled = Enabled;
            yawValue.Enabled = Enabled;
            yawSlider.Enabled = Enabled;
            pitchValue.Enabled = Enabled;
            pitchSlider.Enabled = Enabled;
        }
        */
    }
}

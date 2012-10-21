using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenMetaverse;

namespace ProxyTestGUI {
    public partial class RotationPanel : UserControl {
        private Quaternion pitch = Quaternion.Identity, yaw = Quaternion.Identity, rotation = Quaternion.Identity;

        public string DisplayName {
            get { return vectorPanel.DisplayName; }
            set { vectorPanel.DisplayName = value; }
        }
        public Quaternion Rotation {
            get { return rotation; }
            set {
                Action change = () => {
                    mRotationChanging = true;
                    rotation = value;
                    Vector3 newVec = Vector3.UnitX * value;
                    float roll, pitch, yaw;
                    value.GetEulerAngles(out roll, out pitch, out yaw);

                    pitch *= (float)RAD2DEG;
                    yaw *= (float)RAD2DEG;

                    pitch = pitch > 180f ? 180f : pitch;
                    pitch = pitch < -180f ? -180f : pitch;

                    yaw = yaw > 360f ? 360f : yaw;
                    yaw = yaw < -360f ? 360f : yaw;

                    if (!mPitchChanging)
                        pitchValue.Value = new decimal(pitch);
                    if (!mYawChanging)
                        yawValue.Value = new decimal(yaw);
                    if (!mVectorChanging)
                        vectorPanel.Value = Vector3.UnitX * value;
                    if (OnChange != null)
                        OnChange(this, null);
                    mRotationChanging = false;
                };
                if (InvokeRequired)
                    Invoke(change);
                else
                    change();
            }
        }        public Vector3 Vector {
            get { return vectorPanel.Value; }
            set { vectorPanel.Value = value; }
        }
        public float Yaw {
            get { return (float)decimal.ToDouble(yawValue.Value); }
            set { yawValue.Value = new decimal(value); }
        }
        public float Pitch {
            get { return (float)decimal.ToDouble(pitchValue.Value); }
            set { pitchValue.Value = new decimal(value); }
        }

        public event EventHandler OnChange;

        public RotationPanel() {
            InitializeComponent();
            vectorPanel.X = 1f;
        }

        public static readonly double RAD2DEG = 180.0 / Math.PI;
        public static readonly double DEG2RAD = Math.PI / 180.0;

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

                Rotation = Quaternion.CreateFromAxisAngle(cross, dot);
            }
            mVectorChanging = false;
        }

        private void yawValue_ValueChanged(object sender, EventArgs e) {
            mYawChanging = true;
            if (!mRotationChanging)
                Rotation = CalculateRotation();

            if (!mYawSliderChanging) {
                int val = decimal.ToInt32(yawValue.Value);
                yawSlider.Value = val > 180 ? 180 : (val < -180 ? -180 : val);
            }
            mYawChanging = false;
        }

        private void pitchValue_ValueChanged(object sender, EventArgs e) {
            mPitchChanging = true;
            CalculateRotation();
            if (!mRotationChanging)
                Rotation = CalculateRotation();

            if (!mPitchSliderChanging) {
                int val = decimal.ToInt32(yawValue.Value);
                pitchSlider.Value = val > 90 ? 90 : (val < -90 ? -90 : val);
            }
            mPitchChanging = false;
        }

        private Quaternion CalculateRotation() {
            Quaternion yaw = Quaternion.CreateFromEulers(0f, 0f, (float) (decimal.ToDouble(yawValue.Value) * DEG2RAD));
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
    }
}

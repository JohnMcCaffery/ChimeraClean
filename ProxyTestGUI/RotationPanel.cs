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
        public Vector3 Value {
            get { return vectorPanel.Value; }
            set { vectorPanel.Value = value; }
        }
        public string DisplayName {
            get { return vectorPanel.DisplayName; }
            set { vectorPanel.DisplayName = value; }
        }

        public RotationPanel() {
            InitializeComponent();
        }


        private static readonly double RAD2DEG = 180.0 / Math.PI;
        private static readonly double DEG2RAD = Math.PI / 180.0;

        private bool mVectorChanging = false;
        private bool mRotationChanging = false;
        private bool mRotationSliderChanging = false;
        private bool mPitchChanging = false;
        private bool mPitchSliderChanging = false;

        private Vector2 h;
        private Vector2 v;

        private void vectorPanel_OnChange(object sender, EventArgs e) {
            mVectorChanging = true;
            if (!mRotationChanging && !mPitchChanging) {
                Vector3 vector = vectorPanel.Value;
                h = Vector2.Normalize(new Vector2(vector.X, vector.Y));
                v = Vector2.Normalize(new Vector2(h.Length(), vector.Z));
            }
            if (!mRotationChanging) {
                float dot = Vector2.Dot(Vector2.UnitX, h);
                Vector3 cross = Vector3.Cross(Vector3.UnitX, new Vector3(h, 0f));
                double angle = Math.Acos(dot) * RAD2DEG * (cross.Z >= 0f ? 1D : -1D);
                rotationValue.Value = new decimal(angle);
            }
            if (!mPitchChanging) {
                pitchValue.Value = new decimal(Math.Asin(v.Y) * RAD2DEG);
            }
            mVectorChanging = false;
        }

        private void rotationValue_ValueChanged(object sender, EventArgs e) {
            mRotationChanging = true;
            if (!mVectorChanging) {
                double angle = DEG2RAD * decimal.ToDouble(rotationValue.Value);
                h = new Vector2((float) Math.Cos(angle), (float) Math.Sin(angle)) * v.X;
                vectorPanel.Value = new Vector3(h, v.Y);
            }
            if (!mRotationSliderChanging) {
                rotationSlider.Value = decimal.ToInt32(rotationValue.Value);
            }
            mRotationChanging = false;
        }

        private void rotationSlider_Scroll(object sender, EventArgs e) {
            if (!mRotationChanging) {
                mRotationSliderChanging = true;
                rotationValue.Value = rotationSlider.Value;
                mRotationSliderChanging = false;
            }
        }

        private void pitchValue_ValueChanged(object sender, EventArgs e) {
            mPitchChanging = true;
            if (!mVectorChanging) {
                double angle = DEG2RAD * decimal.ToDouble(pitchValue.Value);
                v = new Vector2((float) Math.Cos(angle), (float) Math.Sin(angle));
                vectorPanel.Value = Vector3.Normalize(new Vector3(h * v.X, v.Y));
            }
            if (!mPitchSliderChanging) {
                pitchSlider.Value = decimal.ToInt32(pitchValue.Value);
            }
            mPitchChanging = false;
        }

        private void pitchSlider_Scroll(object sender, EventArgs e) {
            if (!mPitchChanging) {
                mPitchSliderChanging = true;
                pitchValue.Value = pitchSlider.Value;
                mPitchSliderChanging = false;
            }
        }
    }
}

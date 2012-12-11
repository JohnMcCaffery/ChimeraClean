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
using UtilLib;
using OpenMetaverse;

namespace ChimeraLib.Controls {
    public partial class WindowPanel : UserControl {
        private static readonly decimal ASPECT_RATIO_TOLERANCE = new decimal(0.0001);
        private static double INCH2MM = 25.4;
        private Window window;

        public WindowPanel() : this(new Window()) {
        }
        public WindowPanel(Window window) {
            InitializeComponent();
            aspectRatioValue.Value = aspectRatioHValue.Value / aspectRatioWValue.Value;
            Window = window;
        }

        public Window Window {
            get { return window;  }
            set {
                if (value == null)
                    return;
                if (window != null)
                    window.OnChange -= WindowChanged;
                window = value;
                window.OnChange += WindowChanged;
                WindowChanged(value, null);
            }
        }
        private bool init;
        private void WindowChanged(object source, EventArgs args) {
            if (window == null || IsDisposed)
                return;
            Action a = () => {
                double max = Math.Max(window.Height, window.Width);
                if (max > heightSlider.Maximum) {
                    return;
                    //heightSlider.Maximum = (int) max;
                    //heightValue.Maximum = new decimal(max / 10.0);
                    //widthSlider.Maximum = (int) max;
                    //widthValue.Maximum = new decimal(max / 10.0);
                }

                double diagonalInch = Window.Diagonal / INCH2MM;
                if (diagonalInch * 10.0 > diagonalSlider.Maximum) {
                    return;
                    //diagonalSlider.Maximum = (int) (diagonalInch * 10.0);
                    //diagonalValue.Maximum = new decimal(diagonalInch);
                }

                init = true;
                decimal aspectRatio = aspectRatioValue.Value;
                lockScreenCheck.Checked = window.LockScreenPosition;
                widthSlider.Value =  (int) Math.Round(window.Width);
                widthValue.Value = new decimal(window.Width / 10.0);
                heightSlider.Value = (int) Math.Round(window.Height);
                heightValue.Value = new decimal(window.Height / 10.0);
                diagonalSlider.Value = (int) Math.Round(diagonalInch * 10);
                diagonalValue.Value = new decimal(diagonalInch);
                if (window.FieldOfView < Math.PI) {
                    fovSlider.Value = (int)Math.Round(window.FieldOfView * Rotation.RAD2DEG * 100);
                    fovValue.Value = new decimal(window.FieldOfView * Rotation.RAD2DEG);
                }
                aspectRatioValue.Value = new decimal(window.AspectRatio);
                if (Math.Abs(aspectRatio - aspectRatioValue.Value) > ASPECT_RATIO_TOLERANCE) {
                    aspectRatioWValue.Value = new decimal(window.Width);
                    aspectRatioHValue.Value = new decimal(window.Height);
                }
                screenPositionPanel.Value = window.ScreenPosition / 10f;
                eyeOffsetPanel.Value = window.EyePosition / 10f;
                rotationOffsetPanel.Rotation = window.RotationOffset.Quaternion;
                widthLabel.Text = "2 * Offset H / Width: " + Math.Round((2 * window.FrustumOffsetH) / window.Width, 5);
                heightLabel.Text = "2 * Offset V / Height: " + Math.Round((2 * window.FrustumOffsetV) / window.Height, 5);

                SetCameraPropertiesPacket p = window.CreateCameraPacket();
                double fovDeg = Math.Round(p.CameraProperty.CameraAngle * Rotation.RAD2DEG, 5);
                double fovRad = Math.Round(p.CameraProperty.CameraAngle, 5);
                fovLabel.Text = "FoV (rad/deg): " + fovRad  + " / " + fovDeg;
                hOffsetLabel.Text = "Offset H: " + Math.Round(window.FrustumOffsetH / 10.0, 5);
                vOffsetLabel.Text = "Offset V: " + Math.Round(window.FrustumOffsetV / 10.0, 5);
                init = false;
            };

            if (InvokeRequired && !IsDisposed)
                Invoke(a);
            else
                a();

        }
        private void widthSlider_Scroll(object sender, EventArgs e) {
            if (window != null && !init)
                window.Width = widthSlider.Value;
        }

        private void widthValue_ValueChanged(object sender, EventArgs e) {
            if (window != null && !init)
                window.Width = decimal.ToDouble(widthValue.Value) * 10.0;
        }

        private void heightSlider_Scroll(object sender, EventArgs e) {
            if (window != null && !init)
                window.Height = heightSlider.Value;
        }

        private void heightValue_ValueChanged(object sender, EventArgs e) {
            if (window != null && !init)
                window.Height = decimal.ToDouble(heightValue.Value) * 10.0;
        }

        private void diagonalSlider_Scroll(object sender, EventArgs e) {
            if (window != null && !init)
                window.Diagonal = (diagonalSlider.Value / 10) * INCH2MM;
        }

        private void diagonalValue_ValueChanged(object sender, EventArgs e) {
            if (window != null && !init)
                window.Diagonal = decimal.ToDouble(diagonalValue.Value) * INCH2MM;
        }

        private void fovSlider_Scroll(object sender, EventArgs e) {
            if (window != null && !init)
                window.FieldOfView = (fovSlider.Value / 100) * Rotation.DEG2RAD;
        }

        private void fovValue_ValueChanged(object sender, EventArgs e) {
            if (window != null && !init)
                window.FieldOfView = decimal.ToDouble(fovValue.Value) * Rotation.DEG2RAD;
        }

        private void aspectComponent_ValueChanged(object sender, EventArgs e) {
            if (window != null && aspectRatioHValue != null && aspectRatioWValue != null && !init)
                aspectRatioValue.Value = aspectRatioHValue.Value / aspectRatioWValue.Value;
        }

        private void aspectRatioValue_ValueChanged(object sender, EventArgs e) {
            if (window != null && aspectRatioHValue != null && aspectRatioWValue != null && !init)
                window.AspectRatio = decimal.ToDouble(aspectRatioValue.Value);
        }

        private void screenPositionPanel_OnChange(object sender, EventArgs e) {
            if (window != null && !init)
                window.ScreenPosition = screenPositionPanel.Value * 10f;
        }

        private void eyeOffsetPanel_OnChange(object sender, EventArgs e) {
            if (window != null && !init)
                window.EyePosition = eyeOffsetPanel.Value * 10f;
        }

        private void rotationOffsetPanel_OnChange(object sender, EventArgs e) {
            if (window != null && !init) {
                window.RotationOffset.Yaw = rotationOffsetPanel.Yaw;
                window.RotationOffset.Pitch = rotationOffsetPanel.Pitch;
            }
        }

        private void lockScreenCheck_CheckedChanged(object sender, EventArgs e) {
            if (window != null && !init) 
                window.LockScreenPosition = lockScreenCheck.Checked;
        }
    }
}

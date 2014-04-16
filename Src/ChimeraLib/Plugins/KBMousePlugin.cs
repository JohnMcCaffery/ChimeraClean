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
using System.Windows.Forms;
using OpenMetaverse;
using Chimera.Util;
using System.Drawing;
using Chimera.Config;

namespace Chimera.Plugins {
    public class KBMousePlugin : DeltaBasedPlugin {
        internal int X, Y, CurrentX, CurrentY;
        internal bool MouseDown;
        private Rotation mOrientation = Rotation.Zero;
        private Vector3 mDeltas;
        private bool mIgnorePitch;
        private bool mWalkEnabled = true;
        private bool mStrafeEnabled = true;
        private bool mFlyEnabled = true;
        private bool mYawEnabled = true;
        private bool mPitchEnabled = true;
        private bool mActive = false;

        private Action mTickListener;

        /// <summary>
        /// Triggered whenever the keyboard scale changes.
        /// </summary>
        public event Action<int> KBScaleChange;
        /// <summary>
        /// Triggered whenever the mouse scale changes.
        /// </summary>
        public event Action<int> MouseScaleChange;

        /// <summary>
        /// Amount to scale keyboard values by.
        /// </summary>
        private double mKBShift = 1.0;
        /// <summary>
        /// Amount to scale mouse values by.
        /// </summary>
        private double mMouseScale = 1.0;
        /// <summary>
        /// The output panel that controls the input.
        /// </summary>
        private KBMousePanel mControlPanel;
        /// <summary>
        /// Whether this input is enabled.
        /// </summary>
        private bool mEnabled = true;
        /// <summary>
        /// The source which this input is getting key events transition.
        /// </summary>
        ITickSource mInputSource;

        private Core mCore;
#if DEBUG
        private TickStatistics mStatistics = new TickStatistics();
#endif

        public override bool Enabled {
            get { return base.Enabled; }
            set {
                base.Enabled = value;
                if (value)
                    mCore.Tick += mTickListener;
                else
                    mCore.Tick -= mTickListener;
            }
        }

        public KBMousePlugin() {
            PluginConfig cfg = new PluginConfig();
            mEnabled = cfg.KeyboardEnabled;

            mTickListener = new Action(mCoordinator_Tick);

#if DEBUG
            StatisticsCollection.AddStatistics(mStatistics, Name);
#endif
        }

        /// <summary>
        /// How much to scale Keyboard inputs by.
        /// Should be a value between 1 and 1000.
        /// </summary>
        public int KBScale {
            get { return (int) (mKBShift / .002); }
            set { 
                mKBShift = .002 * value;
                if (KBScaleChange != null)
                    KBScaleChange(KBScale);
            }
        }

        /// <summary>
        /// How much to scale mouse inputs by.
        /// Should be a value between 1 and 1000.
        /// </summary>
        public int MouseScale {
            get { return (int) (mMouseScale / .01) * 5; }
            set { 
                mMouseScale = ((double) value / 5.0) * .01;
                if (MouseScaleChange != null)
                    MouseScaleChange(KBScale);
            }
        }

        internal ITickSource Source {
            get { return mInputSource; }
        }

        /// <summary>
        /// If set to true pitch will always be left at zero.
        /// </summary>
        public bool IgnorePitch {
            get { return mIgnorePitch; }
            set { mIgnorePitch = value; }
        }

        #region ISystemPlugin Members

        public override Control ControlPanel {
            get {
                if (mControlPanel == null)
                    mControlPanel = new KBMousePanel(this);
                return mControlPanel;
            }
        }

        public override string Name {
            get { return "KB+Mouse"; }
        }

        public override ConfigBase Config {
            get { throw new NotImplementedException(); }
        }

        public override string State {
            get { 
                string dump = "----------KB/Mouse Input----------" + Environment.NewLine;
                dump += "Mouse Scale: " + mMouseScale + Environment.NewLine;
                dump += "Keyboard Scale: " + mKBShift + Environment.NewLine;
                return dump;
            }
        }

        public override void Close() {
            if (mControlPanel != null)
                mControlPanel.Stop();
        }

        public override void Draw(System.Drawing.Graphics graphics, Func<Vector3, Point> to2D, Action redraw, Perspective perspective) {
            //Do nothing
        }

        #endregion

        #region IDeltaInput

        public override Vector3 PositionDelta {
            get { return mDeltas; }
        }

        public override Rotation OrientationDelta {
            get { return mOrientation; }
        }

        public override Vector2 MouseDelta
        {
            get { return Vector2.Zero; }
        }

        public override void Init(Core core) {
            mCore = core;
            base.Init(core);
            mInputSource = core;
            core.KeyDown += new Action<Core,KeyEventArgs>(mCoordinator_KeyDown);
            core.KeyUp += new Action<Core,KeyEventArgs>(mCoordinator_KeyUp);

            //if (mEnabled) {
                //core.Tick += mTickListener;
            //}
        }

        #endregion

        internal void panel_MouseDown(object sender, MouseEventArgs e) {
            CurrentX = e.X;
            CurrentY = e.Y;
            X = CurrentX;
            Y = CurrentY;
            MouseDown = true;
        }

        internal void panel_MouseUp(object sender, MouseEventArgs e) {
            MouseDown = false;
            mOrientation.Pitch = 0.0;
            mOrientation.Yaw = 0.0;
        }

        internal void panel_MouseMove(object sender, MouseEventArgs e) {
            if (MouseDown) {
                int xDiff = e.X - CurrentX;
                int yDiff = e.Y - CurrentY;
                mOrientation.Pitch = mIgnorePitch ? 0.0 : (yDiff * mMouseScale);
                mOrientation.Yaw = xDiff * -mMouseScale;
                //if (Change != null && (mDeltas != Vector3.Zero || mOrientation.Pitch != 0.0 || mOrientation.Yaw != 0.0))
                    //Change(this);
                //mOrientation.Pitch = 0;
                //mOrientation.Yaw = 0;
                CurrentX = e.X;
                CurrentY = e.Y;
            }
        }

        private void mCoordinator_KeyDown(Core coord, KeyEventArgs args) {
            switch (args.KeyData) {
                case Keys.W: mDeltas.X = mWalkEnabled ? (float) mKBShift : 0f; break;
                case Keys.S: mDeltas.X = mWalkEnabled ? (float) -mKBShift : 0f; break;
                case Keys.D: mDeltas.Y = mStrafeEnabled ? (float) -mKBShift : 0f; break;
                case Keys.A: mDeltas.Y = mStrafeEnabled ? (float) mKBShift : 0f; break;
                case Keys.E: mDeltas.Z = mFlyEnabled ? (float) mKBShift : 0f; break;
                case Keys.C: mDeltas.Z = mFlyEnabled ? (float) -mKBShift : 0f; break;
                case Keys.Left: mOrientation.Yaw = mYawEnabled ? mKBShift * 4.0 : 0.0; break;
                case Keys.Right: mOrientation.Yaw = mYawEnabled ? -mKBShift * 4.0 : 0.0; break;
                case Keys.Up: mOrientation.Pitch = mPitchEnabled ? -mKBShift * 4.0 : 0.0; break;
                case Keys.Down: mOrientation.Pitch = mPitchEnabled ? mKBShift * 4.0 : 0.0; break;
            }
        }

        private void mCoordinator_KeyUp(Core coord, KeyEventArgs args) {
            switch (args.KeyData) {
                case Keys.W: mDeltas.X = 0f; break;
                case Keys.S: mDeltas.X = 0f; break;
                case Keys.D: mDeltas.Y = 0f; break;
                case Keys.A: mDeltas.Y = 0f; break;
                case Keys.E: mDeltas.Z = 0f; break;
                case Keys.C: mDeltas.Z = 0f; break;
                case Keys.Left: mOrientation.Yaw = 0.0; break;
                case Keys.Right: mOrientation.Yaw = 0.0; break;
                case Keys.Up: mOrientation.Pitch = 0.0; break;
                case Keys.Down: mOrientation.Pitch = 0.0; break;
            }
        }

        private void mCoordinator_Tick() {
#if DEBUG
            mStatistics.Begin();
#endif
            if (mDeltas != Vector3.Zero || mOrientation.Pitch != 0.0 || mOrientation.Yaw != 0.0) {
                mActive = true;
                TriggerChange(this);
            } else if (mActive) {
                mActive = false;
                TriggerChange(this);
            }
            if (MouseDown)
                mOrientation = Rotation.Zero;
#if DEBUG
            mStatistics.End();
#endif
        }
    }
}

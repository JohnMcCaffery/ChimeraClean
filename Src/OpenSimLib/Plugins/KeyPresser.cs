﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Config;
using System.Windows.Forms;
using Chimera.OpenSim;
using Chimera.OpenSim.GUI.Controls.Plugins;

namespace Chimera.OpenSim.Plugins {
    public class KeyPresser : ISystemPlugin {
        private ViewerConfig mConfig = new ViewerConfig();
        private Core mCore;
        private KeyPresserPanel mPanel;
        private Form mForm = null;
        private bool mEnabled;
        private bool mRunning;
        private DateTime mStarted;
        private DateTime mLastPress;
        private Action mTickListener;

        public event Action Started;
        public event Action Stopped;

        public bool Running {
            get { return mRunning; }
            set {
                if (mRunning != value) {
                    mRunning = value;
                    if (value) {
                        mStarted = DateTime.Now;
                        mCore.Tick += mTickListener;
                        if (Started != null)
                            Started();
                    } else {
                        mCore.Tick -= mTickListener;
                        if (Started != null)
                            Stopped();
                    }
                }
            }
        }

        public void Init(Core core) {
            mCore = core;
            mTickListener = new Action(TickListener);
        }

        public void SetForm(Form form) {
            mForm = form;
        }

        public event Action<IPlugin, bool> EnabledChanged;

        public Control ControlPanel {
            get {
                if (mPanel == null)
                    mPanel = new KeyPresserPanel(this);
                return mPanel;
            }
        }

        public bool Enabled {
            get { return mEnabled; }
            set {
                if (mEnabled != value) {
                    mEnabled = value;
                    if (EnabledChanged != null)
                        EnabledChanged(this, value);
                }
            }
        }

        public string Name {
            get { return "Key Presser"; }
        }

        public string State {
            get { throw new NotImplementedException(); }
        }

        public ConfigBase Config {
            get { return mConfig; }
        }

        public void Close() { 
        }

        public void Draw(System.Drawing.Graphics graphics, Func<OpenMetaverse.Vector3, System.Drawing.Point> to2D, Action redraw, Perspective perspective) { }

        private void TickListener() {
            if (mConfig.StopM < DateTime.Now.Subtract(mStarted).TotalMinutes) {
                Running = false;
		if (mConfig.AutoShutdown)
			mForm.Close();
            } else if (mConfig.IntervalMS < DateTime.Now.Subtract(mLastPress).TotalMilliseconds) {
                PressKey();
                mLastPress = DateTime.Now;
            }
        }

        private void PressKey() {
            foreach (var controller in mCore.Frames.Select(f => f.Output as OpenSimController).Where(c => c.ViewerController.Started))
                controller.ViewerController.PressKey(mConfig.Key);
        }
    }
}

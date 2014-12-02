using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Config;
using System.Windows.Forms;
using Chimera.GUI.Controls.Plugins;
using System.Drawing;
using System.IO;
using System.Threading;

namespace Chimera.Plugins {
    public class ScreenshotSequencePlugin : ISystemPlugin {
        private CoreConfig mConfig = new CoreConfig();
        private Core mCore;
        private ScreenshotSequencePanel mPanel;
        private Form mForm = null;
        private bool mEnabled = true;
        private bool mRunning;
        private DateTime mStarted;
        private DateTime mLastPress;
        private Action mTickListener;
	private Frame mFrame;

        private Queue<Bitmap> mScreenshots = new Queue<Bitmap>();

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
                        Thread t = new Thread(ScreenshotProcessor);
                        t.Name = "Screenshot Processor";
                        t.Start();
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
	    mFrame = mCore.Frames[0];
            mTickListener = new Action(TickListener);
        }

        public void SetForm(Form form) {
            mForm = form;
        }

        public event Action<IPlugin, bool> EnabledChanged;

        public Control ControlPanel {
            get {
                if (mPanel == null)
                    mPanel = new ScreenshotSequencePanel(this);
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
            get { return "ScreenshotSequence"; }
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
            } else if (mConfig.IntervalMS < DateTime.Now.Subtract(mLastPress).TotalMilliseconds) {
                TakeScreenshot();
                mLastPress = DateTime.Now;
            }
        }

        private void ScreenshotProcessor() {
	    int image = 1;
            while (mRunning || mScreenshots.Count > 0) {
                if (mScreenshots.Count > 0) {
                    Bitmap screenshot = mScreenshots.Dequeue();
                    screenshot.Save(Path.Combine(mConfig.ScreenshotFolder, mConfig.ScreenshotFile) + "_" + (image++) + ".png");
                    screenshot.Dispose();
                }
                Thread.Sleep(5);
            }
            if (mConfig.AutoShutdown)
                mForm.Close();
        }

        private void TakeScreenshot() {
            Bitmap screenshot = new Bitmap(mFrame.Monitor.Bounds.Width, mFrame.Monitor.Bounds.Height);
            using (Graphics g = Graphics.FromImage(screenshot)) {
                g.CopyFromScreen(mFrame.Monitor.Bounds.Location, Point.Empty, mFrame.Monitor.Bounds.Size);
            }
            mScreenshots.Enqueue(screenshot);
        }
    }
}



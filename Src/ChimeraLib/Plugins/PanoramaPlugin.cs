using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Config;
using Chimera.GUI.Controls.Plugins;
using OpenMetaverse;
using Chimera.Util;
using System.Drawing;
using System.Threading;
using System.IO;

namespace Chimera.Plugins {
    public class PanoramaPlugin : PluginBase<PanoramaPanel> {

        private CoreConfig mConfig = new CoreConfig();
	private Frame mFrame;

	    private bool mRunning;
        private Queue<Bitmap> mScreenshots = new Queue<Bitmap>();

	public PanoramaPlugin() : base ("Panorama", plugin => new PanoramaPanel(plugin as PanoramaPlugin)) {
	}

        public override void Init(Core core) {
            base.Init(core);
            mCore.Frames.First();
        }

        public override Config.ConfigBase Config {
            get { return mConfig; }
        }

        public void TakePanorama() {
            mCore.Update(mCore.Position, Vector3.Zero, new Rotation(0.0, 0.0), new Rotation());
            Thread.Sleep(500);
            mCore.Update(mCore.Position, Vector3.Zero, new Rotation(90, 0.0), new Rotation());
            Thread.Sleep(500);
            mCore.Update(mCore.Position, Vector3.Zero, new Rotation(180, 0.0), new Rotation());
            Thread.Sleep(500);
            mCore.Update(mCore.Position, Vector3.Zero, new Rotation(-90, 0.0), new Rotation());
            Thread.Sleep(500);
            mCore.Update(mCore.Position, Vector3.Zero, new Rotation(0.0, 90.0), new Rotation());
            Thread.Sleep(500);
            mCore.Update(mCore.Position, Vector3.Zero, new Rotation(0.0, -90.0), new Rotation());
            Thread.Sleep(500);

            Thread t = new Thread(ScreenshotProcessor);
            t.Name = "Panorama Image Processor";
            t.Start();

            mCore.Update(mCore.Position, Vector3.Zero, new Rotation(0.0, 0.0), new Rotation());
            TakeScreenshot();
            mCore.Update(mCore.Position, Vector3.Zero, new Rotation(90, 0.0), new Rotation());
            TakeScreenshot();
            mCore.Update(mCore.Position, Vector3.Zero, new Rotation(180, 0.0), new Rotation());
            TakeScreenshot();
            mCore.Update(mCore.Position, Vector3.Zero, new Rotation(-90, 0.0), new Rotation());
            TakeScreenshot();
            mCore.Update(mCore.Position, Vector3.Zero, new Rotation(0.0, -90.0), new Rotation());
            TakeScreenshot();
            mCore.Update(mCore.Position, Vector3.Zero, new Rotation(0.0, 90.0), new Rotation());
            TakeScreenshot();
        }

        private void TakeScreenshot() {
            Bitmap screenshot = new Bitmap(mFrame.Monitor.Bounds.Width, mFrame.Monitor.Bounds.Height);
            using (Graphics g = Graphics.FromImage(screenshot)) {
                g.CopyFromScreen(mFrame.Monitor.Bounds.Location, Point.Empty, mFrame.Monitor.Bounds.Size);
            }
            mScreenshots.Enqueue(screenshot);
        }

        private void ScreenshotProcessor() {
            int image = 1;
            while (mRunning || mScreenshots.Count > 0) {
                if (mScreenshots.Count > 0) {
                    Bitmap screenshot = mScreenshots.Dequeue();
                    screenshot.Save(Path.Combine(mConfig.ScreenshotFolder, GetImageName(image), ".png"));
                    screenshot.Dispose();
                }
                Thread.Sleep(5);
            }
            if (mConfig.AutoShutdown)
                mForm.Close();
        }

        private string GetImageName(int image) {
            switch (image) {
                case 1: return "North";
                case 2: return "West";
                case 3: return "South";
                case 4: return "East";
                case 5: return "Up";
                case 6: return "Down";
                default: return "Unknown";
            }
        }
    }
}

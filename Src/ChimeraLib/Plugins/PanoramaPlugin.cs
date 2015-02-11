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
using log4net;

namespace Chimera.Plugins {
    public class PanoramaPlugin : PluginBase<PanoramaPanel> {
        private ILog Logger = LogManager.GetLogger("Panorama");

        private CoreConfig mConfig = new CoreConfig();
        private Frame mFrame;
        private bool mRunning;

        private static float sOffset = 6.3f;
        private Vector3 mCentre;
        private Vector3 mLeftOffset = new Vector3(0f, sOffset / 2f, 0f);
        private Vector3 mRightOffset = new Vector3(0f, -sOffset / 2f, 0f);

	private int mCurrentImage = 0;

        private Queue<Bitmap> mScreenshots = new Queue<Bitmap>();

        public PanoramaPlugin()
            : base("Panorama", plugin => new PanoramaPanel(plugin as PanoramaPlugin)) {
        }

        public override void Init(Core core) {
            base.Init(core);
            mFrame = mCore.Frames.First();
            mCentre = mCore.Position;
        }

        public override Config.ConfigBase Config {
            get { return mConfig; }
        }

        public void TakePanorama() {
            Rotation r = mCore.Orientation;

            for (int i = 1; i < 7; i++) {
                mCore.Update(mCore.Position, Vector3.Zero, GetRotation(i), Rotation.Zero);
                Thread.Sleep(500);
            }

            mRunning = true;

            if (!Directory.Exists(mConfig.ScreenshotFolder))
                Directory.CreateDirectory(mConfig.ScreenshotFolder);

            Thread t = new Thread(ScreenshotProcessor);
            t.Name = "Panorama Image Processor";
            t.Start();

            mCentre = mCore.Position;
            mCurrentImage = 0;

            for (int image = 0; image < 18; image++) {
                ShowNextImage();
                Thread.Sleep(mConfig.CaptureDelayMS);
                TakeScreenshot();
            }

            mRunning = false;

            mCore.Update(mCore.Position, Vector3.Zero, r, Rotation.Zero);
        }

        public void SetCentre() {
            mCentre = mCore.Position;
        }

        public void ShowNextImage() {
            Rotation rotation = GetRotation(mCurrentImage);
            Vector3 position = mCentre;
            if (mCurrentImage % 3 == 1)
                position += mLeftOffset * rotation.Quaternion;
            if (mCurrentImage % 3 == 2)
                position += mRightOffset * rotation.Quaternion;

            mCore.Update(position, Vector3.Zero, rotation, Rotation.Zero);

            mCurrentImage++;
            if (mCurrentImage == 18)
                mCurrentImage = 0;
        }

        public void TakeScreenshot() {
            Bitmap screenshot = new Bitmap(mFrame.Monitor.Bounds.Width, mFrame.Monitor.Bounds.Height);
            using (Graphics g = Graphics.FromImage(screenshot)) {
                g.CopyFromScreen(mFrame.Monitor.Bounds.Location, Point.Empty, mFrame.Monitor.Bounds.Size);
            }
            mScreenshots.Enqueue(screenshot);
        }

        private void ScreenshotProcessor() {
            int image = 0;
            while (mRunning || mScreenshots.Count > 0) {
                if (mScreenshots.Count > 0) {
                    Bitmap screenshot = mScreenshots.Dequeue();
                    using (Bitmap resized = new Bitmap(screenshot, new Size(screenshot.Height, screenshot.Height))) {
                        string file = Path.Combine(mConfig.ScreenshotFolder, GetImageName(image++) + ".png");
                        Logger.Info("Writing Panorama image to: " + file + ".");
                        resized.Save(file);
                    }
                    screenshot.Dispose();
                }
                Thread.Sleep(5);
            }
            if (mConfig.AutoShutdown)
                mForm.Close();
        }

        private Rotation GetRotation(int image) {
            switch (image / 3) {
                case 0: return new Rotation(0.0, 0.0);
                case 1: return new Rotation(0.0, 90);
                case 2: return new Rotation(0.0, 180.0);
                case 3: return new Rotation(0.0, -90);
                case 4: return new Rotation(-90.0, 0.0);
                case 5: return new Rotation(90.0, 0.0);
                default: return new Rotation(0.0, 0.0);
            }
        }

        private string GetImageName(int image) {
            string side = "North";
            switch (image / 3) {
                case 0: side = "North"; break;
                case 1: side = "West"; break;
                case 2: side = "South"; break;
                case 3: side = "East"; break;
                case 4: side = "Up"; break;
                case 5: side = "Down"; break;
            }

            string offset = "";
            switch (image % 3) {
                case 0: offset = ""; break;
                case 1: offset = "Left"; break;
                case 2: offset = "Right"; break;
            }

            return side + offset;
        }
    }
}

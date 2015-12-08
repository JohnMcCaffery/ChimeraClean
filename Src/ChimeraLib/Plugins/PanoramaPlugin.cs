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

        public bool Capture3D {
            get { return mConfig.Capture3D; }
            set { mConfig.Capture3D = value; }
        }

        public void TakePanorama() {
            Rotation r = mCore.Orientation;

            /*
            for (int i = 1; i < 7; i++) {
                mCore.Update(mCore.Position, Vector3.Zero, GetRotation(i), Rotation.Zero);
                Thread.Sleep(500);
            }
            */

            mRunning = true;

            if (!Directory.Exists(mConfig.ScreenshotFolder))
                Directory.CreateDirectory(mConfig.ScreenshotFolder);

            Thread t = new Thread(ScreenshotProcessor);
            t.Name = "Panorama Image Processor";
            t.Start();

            mCentre = mCore.Position;
            mCurrentImage = 0;

            for (int image = 0; mCurrentImage != 0 || image == 0; image++) {
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

            mCurrentImage = mConfig.Capture3D ? mCurrentImage + 1 : mCurrentImage += 3;
            if (!mConfig.CaptureOverlaps && mCurrentImage == 18)
                mCurrentImage = 0;
            else if (mConfig.CaptureOverlaps && mCurrentImage == 42)
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
                        string file = Path.Combine(mConfig.ScreenshotFolder, GetImageName(image) + ".png");
                        image = mConfig.Capture3D ? image + 1 : image += 3;
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
                case 0: return new Rotation(-90.0, 0.0);
                case 1: return new Rotation(90.0, 0.0);
                case 2: return new Rotation((mConfig.CaptureOverlaps ? -35 : 0.0), (!mConfig.CaptureOverlaps ? 0 : 0));
                case 3: return new Rotation((mConfig.CaptureOverlaps ? -35 : 0.0), (!mConfig.CaptureOverlaps ? 270 : 72));
                case 4: return new Rotation((mConfig.CaptureOverlaps ? -35 : 0.0), (!mConfig.CaptureOverlaps ? 180 : 144));
                case 5: return new Rotation((mConfig.CaptureOverlaps ? -35 : 0.0), (!mConfig.CaptureOverlaps ? 90 : 216));
                case 6: return new Rotation(-35, 288);
                case 7: return new Rotation(35, 0);
                case 8: return new Rotation(35, 72);
                case 9: return new Rotation(35, 144);
                case 10: return new Rotation(35, 216);
                case 11: return new Rotation(35, 288);
                default: return new Rotation(0.0, 0.0);
            }
        }

        private string GetImageName(int image) {
            string side = "North";
            switch (image / 3) {
                case 0: side = (mConfig.CaptureOverlaps ? "0,90" : "Up"); break;
                case 1: side = (mConfig.CaptureOverlaps ? "0,-90" : "Down"); break;
                case 2: side = (mConfig.CaptureOverlaps ? "0,35" : "North"); break;
                case 3: side = (mConfig.CaptureOverlaps ? "72,35" : "West"); break;
                case 4: side = (mConfig.CaptureOverlaps ? "144,35" : "South"); break;
                case 5: side = (mConfig.CaptureOverlaps ? "216,35" : "East"); break;
                case 6: side = "288,35"; break;
                case 7: side = "0,-35"; break;
                case 8: side = "72,-35"; break;
                case 9: side = "144,-35"; break;
                case 10: side = "216,-35"; break;
                case 11: side = "288,-35"; break;
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

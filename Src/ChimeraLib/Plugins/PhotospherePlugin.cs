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
    public class PhotospherePlugin : PluginBase<PhotospherePanel> {
        private ILog Logger = LogManager.GetLogger("Photosphere");

        private struct ImageToSave {
            public Bitmap Image;
            public int ImageNumber;

            public ImageToSave(Bitmap image, int imageNumber, PhotospherePlugin plugin) {
                Image = image;
                ImageNumber = imageNumber == 0 ? plugin.TotalImages * 3 : imageNumber;
                ImageNumber--;
            }
        }

        private CoreConfig mConfig = new CoreConfig();
        private Frame mFrame;
        private bool mRunning;

        private static float sOffset = 6.3f;
        private Vector3 mCentre;
        private Vector3 mLeftOffset = new Vector3(0f, sOffset / 2f, 0f);
        private Vector3 mRightOffset = new Vector3(0f, -sOffset / 2f, 0f);

        private Rotation mOriginalRotation;
        private double mOriginalWidth;
        private double mOriginalHeight;

        private double mFoV;

        private int mCols;
        private int mRows;

        private double mYawIncrement;
        private double mPitchIncrement;

        private int mTotalImages;

        private int mCurrentImage = 0;

        private Queue<ImageToSave> mScreenshots = new Queue<ImageToSave>();

        public PhotospherePlugin()
            : base("Photosphere", plugin => new PhotospherePanel(plugin as PhotospherePlugin)) {
        }

        public override void Init(Core core) {
            base.Init(core);
            mFrame = mCore.Frames.First();
            mCentre = mCore.Position;
            OutputWidth = mConfig.PhotosphereOutputWidth;
        }

        public override Config.ConfigBase Config {
            get { return mConfig; }
        }

        public bool Capture3D {
            get { return mConfig.PhotosphereCapture3D; }
            set { mConfig.PhotosphereCapture3D = value; }
        }

        public int Rows {
            get { return mRows; }
        }

        public int Cols {
            get { return mCols; }
        }

        public double FoV {
            get { return mFoV; }
        }

        public double YawIncrement {
            get { return mYawIncrement; }
        }

        public double PitchIncrement {
            get { return mPitchIncrement; }
        }

        public int TotalImages {
            get { return mTotalImages; }
        }

        public int OutputWidth {
            get { return mConfig.PhotosphereOutputWidth; }
            set {
                if (value < 1) {
                    Logger.Warn("Unable to set photosphere output width to " + value + ". OutputWidth must be > 1");
                    return;
                }

                //The height of the screen, used for calculating how much of the final output image can be captured in each screenshot.
                //Images are compressed to squares so the smallest of width/height is used.
                int screenHeight = Math.Min(mFrame.Monitor.Bounds.Width, mFrame.Monitor.Bounds.Height);
                //How many screenshots would be necessary to create the output resolution
                double screenshotsH = ((double)mConfig.PhotosphereOutputWidth) / (double)screenHeight;
                //The number of positions to stop at whilst rotating laterally. 
                //Calculated as the number of shots necessary for the output width, rounded up, + 1 to allow for overlaps
                mCols = (int)screenshotsH + (screenshotsH % 1.0 == 0.0 ? 1 : 2);
                //The field of view each screenshot should have to to produce a final image ofr OutputWidth or greater.
                mFoV = 360.0 / (mCols - 1.0);
                //If the calculated filed of view is less than 90 there will be problems with hugin so manually set to 90.
                if (mFoV > 90.0) {
                    Logger.Info("Calculated panosphere field of view was greater than 90 degrees. This causes problems with Hugin so has been changed to 90.");
                    mFoV = 90.0;
                    mCols = 5;
                }
                //How much to rotate the view by for each screenshot. Has to be slightly less that the FoV to allow for overlaps.
                //The extra screenshot which allows for overlaps is divided by the number of columns to calculate the overlap per image.
                mYawIncrement = mFoV - (mFoV / mCols);

                //The number of rows round 180 over the FoV + 1 to give an overlap
                mRows = (int) (180.0 / mFoV) + 1;
                //How much to pitch the view by between each row of screenshots. Is less than FoV to allow for overlaps.
                //The extra screenshot is split by the number of rows.
                mPitchIncrement = mFoV - (mFoV / mRows);

                //Total number of images is rows * columns + 2 (for up and down)
                mTotalImages = (mRows * mCols) + 2;
            }
        }

        public string PTOFile {
            get { return Path.Combine(SaveFolder, mConfig.PhotosphereName + ".pto"); }
        }

        public String SaveFolder {
            get { return Path.Combine(mConfig.ScreenshotFolder, mConfig.PhotosphereName); }
        }

        public void TakePhotosphere() {
            Rotation r = mCore.Orientation;

            /*
            for (int i = 1; i < 7; i++) {
                mCore.Update(mCore.Position, Vector3.Zero, GetRotation(i), Rotation.Zero);
                Thread.Sleep(500);
            }
            */

            mRunning = true;

            mOriginalRotation = mCore.Orientation;
            mOriginalWidth = mFrame.Width;
            mOriginalHeight = mFrame.Height;

            mFrame.LinkFoVs = false;
            mFrame.HFieldOfView = (Math.PI / 180) * mFoV;
            mFrame.VFieldOfView = mFrame.HFieldOfView;

            if (mFrame.Output.Process != null)
                ProcessWrangler.BringToFront(mFrame.Output.Process);

            if (File.Exists(PTOFile))
                File.Delete(PTOFile);

            if (!Directory.Exists(SaveFolder))
                Directory.CreateDirectory(SaveFolder);

            Thread t = new Thread(ScreenshotProcessor);
            t.Name = "Photosphere Image Processor";
            t.Start();

            mCentre = mCore.Position;
            mCurrentImage = 0;

            for (int image = 0; mCurrentImage != 0 || image == 0; image++) {
                ShowNextImage();
                Thread.Sleep(mConfig.PhotosphereCaptureDelayMS);
                TakeScreenshot();
            }
            mCore.Update(mCore.Position, Vector3.Zero, mOriginalRotation, Rotation.Zero);
            mFrame.Width = mOriginalWidth;
            mFrame.Height = mOriginalHeight;
            mFrame.LinkFoVs = true;

            mRunning = false;

            mCore.Update(mCore.Position, Vector3.Zero, r, mOriginalRotation);
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

            mCurrentImage = mConfig.PhotosphereCapture3D ? mCurrentImage + 1 : mCurrentImage += 3;
            if (mCurrentImage == mTotalImages * 3)
                mCurrentImage = 0;
            /*
            if (!mConfig.CaptureOverlaps && mCurrentImage == mTotalImages)
                mCurrentImage = 0;
            else if (mConfig.CaptureOverlaps && mCurrentImage == 42)
                mCurrentImage = 0;
            */
        }

        public void TakeScreenshot() {
            Bitmap screenshot = new Bitmap(mFrame.Monitor.Bounds.Width, mFrame.Monitor.Bounds.Height);
            using (Graphics g = Graphics.FromImage(screenshot)) {
                g.CopyFromScreen(mFrame.Monitor.Bounds.Location, Point.Empty, mFrame.Monitor.Bounds.Size);
            }
            mScreenshots.Enqueue(new ImageToSave(screenshot, mCurrentImage, this));
        }

        private void ScreenshotProcessor() {
            while (mRunning || mScreenshots.Count > 0) {
                if (mScreenshots.Count > 0) {
                    ImageToSave toSave = mScreenshots.Dequeue();
                    Bitmap screenshot = toSave.Image;
                    using (Bitmap resized = new Bitmap(screenshot, new Size(screenshot.Height, screenshot.Height))) {
                        int image = toSave.ImageNumber;
                        Rotation rot = GetRotation(image);
                        string name = GetImageName(image);
                        string file = Path.Combine(SaveFolder, name);
                        Logger.Info("Writing Photosphere image to: " + file + ".");
                        resized.Save(file);

                        if ((image / 3) == 0)
                            StartPTOFile();

                        File.AppendAllLines(PTOFile, new string[] {
                                "#-hugin  cropFactor=1",
                                string.Format("i w{0} h{0} f0 v{1} Ra0 Rb0 Rc0 Rd0 Re0 Eev0 Er1 Eb1 r0 p{2} y{3} TrX0 TrY0 TrZ0 Tpy0 Tpp0 j0 a0 b0 c0 d0 e0 g0 t0 Va1 Vb0 Vc0 Vd0 Vx0 Vy0  Vm5 n\"{4}\"",
                                resized.Height,
                                mFoV,
                                rot.Pitch * -1.0,
                                rot.Yaw * -1.0,
                                name)
                        });

                        if ((image / 3) == TotalImages - 1) {
                            FinishPTOFile();
                            if (mConfig.PhotosphereAddBatch) {
                                Logger.Info("Adding " + mConfig.PhotosphereName + " as batch processing job" + (mConfig.PhotosphereAutoStartBatch ? " and starting batcher" : "") + ".");
                                string args = String.Format("{0} {1}.pto {1}.jpg",
                                    (mConfig.PhotosphereAutoStartBatch ? " --batch" : ""),
                                    mConfig.PhotosphereName);
                                Logger.Debug(mConfig.PhotosphereAutoStartBatch + args);
                                ProcessWrangler.InitProcess(mConfig.PhotosphereBatcherExe, SaveFolder, args).Start();
                            }
                        }
                    }
                    screenshot.Dispose();
                }
                Thread.Sleep(5);
            }
            if (mConfig.AutoShutdown)
                mForm.Close();
        }

        private Rotation GetRotation(int image) {
            image /= 3;
            //Final 2 images are up and down
            if (image == mTotalImages - 2)
                return new Rotation(-90, 0);
            else if (image == mTotalImages - 1)
                return new Rotation(90, 0);

            //Pitch starts negative. As horizon is zero pitch has to start negative, work to zero and then past
            double startPitch = ((mRows - 1) * mPitchIncrement) / -2.0;
            int row = image / mCols;
            int column = image % mCols;
            return new Rotation(startPitch + (row * mPitchIncrement), column * mYawIncrement);

            /*
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
            */
        }

        private string GetImageName(int image) {
            /*
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
            */

            string offset = "";
            switch ((image + 1) % 3) {
                case 0: offset = ""; break;
                case 1: offset = "Left"; break;
                case 2: offset = "Right"; break;
            }

            Rotation rot = GetRotation(image);
            return string.Format("{0} - {1}y {2}p{3}.png", (image / 3), rot.Yaw, rot.Pitch, offset);
        }

        private void StartPTOFile() {
            File.WriteAllLines(PTOFile, new string[] {
                "# hugin project file",
                "#hugin_ptoversion 2",
                String.Format("p f2 w{0} h{1} v360  E0 R0 n\"TIFF_m c:LZW r:CROP\"", OutputWidth, OutputWidth / 2),
                "m g1 i0 f0 m2 p0.00784314",
                "",
                "# image lines",
            });
        }

        private void FinishPTOFile () {
            File.AppendAllLines(PTOFile, new string[] {
                "",
                "",
                "# specify variables that should be optimized",
                "v Ra0",
                "v Rb0",
                "v Rc0",
                "v Rd0",
                "v Re0",
                "v Vb0",
                "v Vc0",
                "v Vd0",
                "v Eev1",
                "v r1",
                "v p1",
                "v y1",
                "v Eev2",
                "v r2",
                "v p2",
                "v y2",
                "v Eev3",
                "v r3",
                "v p3",
                "v y3",
                "v Eev4",
                "v r4",
                "v p4",
                "v y4",
                "v Eev5",
                "v r5",
                "v p5",
                "v y5",
                "v Eev6",
                "v r6",
                "v p6",
                "v y6",
                "v Eev7",
                "v r7",
                "v p7",
                "v y7",
                "v Eev8",
                "v r8",
                "v p8",
                "v y8",
                "v Eev9",
                "v r9",
                "v p9",
                "v y9",
                "v Eev10",
                "v r10",
                "v p10",
                "v y10",
                "v Eev11",
                "v r11",
                "v p11",
                "v y11",
                "v",
                "",
                "",
                "# control points",
                "",
                "#hugin_optimizeReferenceImage 0",
                "#hugin_blender enblend",
                "#hugin_remapper nona",
                "#hugin_enblendOptions ",
                "#hugin_enfuseOptions ",
                "#hugin_hdrmergeOptions -m avg -c",
                "#hugin_outputLDRBlended true",
                "#hugin_outputLDRLayers false",
                "#hugin_outputLDRExposureRemapped false",
                "#hugin_outputLDRExposureLayers false",
                "#hugin_outputLDRExposureBlended false",
                "#hugin_outputLDRStacks false",
                "#hugin_outputLDRExposureLayersFused false",
                "#hugin_outputHDRBlended false",
                "#hugin_outputHDRLayers false",
                "#hugin_outputHDRStacks false",
                "#hugin_outputLayersCompression LZW",
                "#hugin_outputImageType jpg",
                "#hugin_outputImageTypeCompression LZW",
                "#hugin_outputJPEGQuality 100",
                "#hugin_outputImageTypeHDR exr",
                "#hugin_outputImageTypeHDRCompression LZW",
                "#hugin_outputStacksMinOverlap 0.7",
                "#hugin_outputLayersExposureDiff 0.5",
                "#hugin_optimizerMasterSwitch 1",
                "#hugin_optimizerPhotoMasterSwitch 21"
            });
        }
    }
}

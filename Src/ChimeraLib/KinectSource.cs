using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse.Skeletal.Kinect;
using OpenMetaverse.Skeletal;
using System.Drawing;
using OpenMetaverse;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using UtilLib;
using Nini.Config;

namespace ChimeraLib {
    public class KinectSource {
        private readonly Dictionary<int, DateTime> _lastUpdated = new Dictionary<int, DateTime>();
        private readonly List<KeyValuePair<int, DateTime>> _users = new List<KeyValuePair<int, DateTime>>();
        private readonly KinectSkeletonFrameSupplier supplier;
        private readonly object _imageLock = new object();
        private readonly Rotation rotation = new Rotation(0, 180);
        private readonly Vector3 scale = new Vector3 (1000f);
        private Vector3 position = new Vector3(400f, 0f, 0f);
        private Vector3 rawValue = Vector3.Zero;

        private int _locked = -1;
        private Bitmap bmp;


        public bool Started {
            get { return supplier != null; }
        }
        public Bitmap ImageFrame {
            get { return bmp; }
        }

        public Vector3 RawValue {
            get { return rawValue; }
            set { 
                rawValue = value;
                if (OnChange != null)
                    OnChange(HeadPosition);
            }
        }
        public Vector3 HeadPosition {
            get { return ((rawValue * scale) * rotation.Quaternion) + position; }
        }
        /// <summary>
        /// Where the kinect is positioned in real space (mm).
        /// </summary>
        public Vector3 Position {
            get { return position; }
            set { position = value; }
        }
        public Rotation Rotation {
            get { return rotation; }
        }

        public bool ProcessImageFrames {
            get { return supplier.ImageEnabled; }
            set { supplier.ImageEnabled = value; }
        }

        public int ImageWidth {
            get { return supplier.ImageWidth; }
        }

        public int ImageHeight {
            get { return supplier.ImageHeight; }
        }

        public event Action<Vector3> OnChange;

        public event Action<Bitmap> OnImage;

        public KinectSource() {
            if (AppDomain.CurrentDomain.SetupInformation.ConfigurationFile != null) {
                DotNetConfigSource configSource = new DotNetConfigSource(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                IConfig config = configSource.Configs["Kinect"];
                if (config != null) {
                    float posX = config.GetFloat("PositionX", 0f);
                    float posY = config.GetFloat("PositionY", 0f);
                    float posZ = config.GetFloat("PositionZ", 0f);

                    float pitch = config.GetFloat("Pitch", 0f);
                    float yaw = config.GetFloat("Yaw", 0f);

                    position = new Vector3(posX, posY, posZ);
                    rotation = new Rotation(pitch, yaw);
                }
            }
            supplier = new KinectSkeletonFrameSupplier();
            try {
                supplier.Start();
                supplier.OnSkeletonFrame += new EventHandler<SkeletonFrameEventArgs>(supplier_OnSkeletonFrame);
            } catch (Exception e) {
                Console.WriteLine("Problem starting Kinect: " + e.Message);
                supplier = null;
            }
        }

        void supplier_OnSkeletonFrame(object sender, SkeletonFrameEventArgs e) {
            if (supplier.ImageEnabled) {
                bmp = CopyDataToBitmap(e.Image.ToArray());
                if (OnImage != null)
                    OnImage(bmp);
            }

            int id = (int)sender;
            if (!_lastUpdated.ContainsKey(id) && id != 0) {
                Console.WriteLine("New ID found: " + id);
                _lastUpdated.Add(id, DateTime.Now);
                if (_locked == -1) {
                    Console.WriteLine("Locked to: " + id);
                    _locked = id;
                }
            }

            List<int> toRemove = new List<int>();
            foreach (var pair in _lastUpdated) {
                int checkID = pair.Key;
                if (DateTime.Now.Subtract(pair.Value).TotalSeconds > 1) {
                    Console.WriteLine("ID timed out: " + checkID);
                    toRemove.Add(checkID);
                    _users.Remove(pair);
                    if (_locked == checkID) {
                        Console.WriteLine("Unlocking");
                        _locked = -1;
                    } 
                } else if (!_users.Contains(pair))
                    _users.Add(pair);
            }

            if (id != 0) 
                _lastUpdated[id] = DateTime.Now;
            foreach (var removeID in toRemove)
                _lastUpdated.Remove(removeID);


            if (id != _locked || id == 0)
                return;

            rawValue = e.Skeleton.GetJoint("Head").Position;
            if (OnChange != null)
                OnChange(HeadPosition);
        }
        
        /// <summary>
        /// http://www.tek-tips.com/viewthread.cfm?qid=1264492
        /// function CopyDataToBitmap
        /// Purpose: Given the pixel data return a bitmap of size [352,288],PixelFormat=24RGB 
        /// </summary>
        /// <param name="data">Byte array with pixel data</param>
        public Bitmap CopyDataToBitmap(byte[] data) {
            lock (_imageLock) {
                //Here create the Bitmap to the know height, width and format
                Bitmap bmp = new Bitmap(supplier.ImageWidth, supplier.ImageHeight, System.Drawing.Imaging.PixelFormat.Format32bppRgb);

                //Create a BitmapData and Lock all pixels to be written 
                BitmapData bmpData = bmp.LockBits(
                                     new Rectangle(0, 0, bmp.Width, bmp.Height),
                                     ImageLockMode.WriteOnly, bmp.PixelFormat);

                //Copy the data from the byte array into BitmapData.Scan0
                Marshal.Copy(data.ToArray(), 0, bmpData.Scan0, bmpData.Stride * bmp.Height);


                //Unlock the pixels
                bmp.UnlockBits(bmpData);


                //Return the bitmap 
                return bmp;
            }
        }
        public void Stop() {
            if (Started)
                supplier.Stop();
        }
    }
}

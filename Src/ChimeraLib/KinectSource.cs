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
        private readonly object _imageLock = new object();
        private readonly Rotation rotation = new Rotation(0, 180);
        private readonly Vector3 scale = new Vector3 (1000f, -1000f, 1000f);
        private KinectSkeletonFrameSupplier supplier;
        private Vector3 position = new Vector3(1000f, 0f, 0f);
        private Vector3 rawValue = Vector3.UnitX * 2f;
        private Vector3 headPosition = new Vector3(-1000f, 0f, 0f);
        private bool enabled = false;
        private Vector3 startPosition = new Vector3(0f, -1000f, 0f);
        private bool init = false;

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
                headPosition = Head(value);
                Change();
            }
        }
        public Vector3 HeadPosition {
            get { return headPosition; }
        }
        /// <summary>
        /// Where the kinect is positioned in real space (mm).
        /// </summary>
        public Vector3 Position {
            get { return position; }
            set { 
                position = value;
                Change();
            }
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

        private void Change() {
            if (enabled && OnChange != null)
                OnChange(HeadPosition);
        }


        public event Action<Vector3> OnChange;

        public event Action<Bitmap> OnImage;

        public KinectSource() {
            if (AppDomain.CurrentDomain.SetupInformation.ConfigurationFile != null) {
                DotNetConfigSource configSource = new DotNetConfigSource(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                IConfig config = configSource.Configs["Kinect"];
                if (config != null) {
                    float pitch = config.GetFloat("Pitch", rotation.Yaw);
                    float yaw = config.GetFloat("Yaw", rotation.Pitch);

                    position = Vector3.Parse(config.Get("Position", position.ToString()));
                    rotation = new Rotation(pitch, yaw);
                }
            }
            rotation.OnChange += (source, args) => Change();
        }

        /// <summary>
        /// Enable kinect input.
        /// Stores the original head position so when the Kinect is disable it can be reset.
        /// </summary>
        /// <param name="eye">The original head position to be reset to</param>
        public void Enable(Vector3 eye) {
            if (enabled)
                return;
            startPosition = eye;
            headPosition = Head(rawValue);
            enabled = true;

            try {
                supplier = new KinectSkeletonFrameSupplier();
                supplier.OnSkeletonFrame += new EventHandler<SkeletonFrameEventArgs>(supplier_OnSkeletonFrame);
                supplier.Start();
            } catch (Exception e) {
                Console.WriteLine("Problem starting Kinect: " + e.Message);
                supplier = null;
            }
            if (OnChange != null)
                OnChange(HeadPosition);
        }

        public void Disable() {
            headPosition = startPosition;
            enabled = false;
            if (Started) {
                supplier.Stop();
                supplier = null;
            }
            if (OnChange != null)
                OnChange(startPosition);
        }
        private Vector3 Head(Vector3 raw) {
            return((rawValue * scale) * rotation.Quaternion) + position;
        }

        public void supplier_OnSkeletonFrame(object sender, SkeletonFrameEventArgs e) {
            if (supplier.ImageEnabled) {
                bmp = CopyDataToBitmap(e.Image.ToArray());
                if (OnImage != null)
                    OnImage(bmp);
            }

            if (!enabled)
                return;

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
            headPosition = Head(rawValue);
            if (OnChange != null)
                OnChange(HeadPosition);
        }
        
        /// <summary>
        /// http://www.tek-tips.com/viewthread.cfm?qid=1264492
        /// function CopyDataToBitmap
        /// Purpose: Given the pixel data return a bitmap of size [352,288],PixelFormat=24RGB 
        /// </summary>
        /// <param name="data">Byte array with pixel data</param>
        private Bitmap CopyDataToBitmap(byte[] data) {
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

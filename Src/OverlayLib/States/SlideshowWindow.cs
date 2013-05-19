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
using Chimera.Interfaces.Overlay;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;

namespace Chimera.Overlay.States {
    public class SlideshowWindow : WindowState {
        private readonly IImageTransition mTransition;
        private readonly Bitmap[] mRawImages;
        private readonly string mFolder;
        private Bitmap[] mImages;
        private int mCurrentImage = -1;

        public SlideshowWindow(WindowOverlayManager manager, string folder, IImageTransition transition)
            : base(manager) {

            mFolder = folder;
            mTransition = transition;

            List<Bitmap> images = new List<Bitmap>();
            foreach (var file in Directory.GetFiles(Path.Combine(folder, manager.Window.Name))) {
                if (Regex.IsMatch(Path.GetExtension(file), @"png$|jpe?g$|bmp$", RegexOptions.IgnoreCase)) {
                    images.Add(new Bitmap(file));
                }
            }

            mRawImages = images.ToArray();

            AddFeature(transition);
        }

        public override bool Active {
            get { return base.Active; }
            set {
                base.Active = value;
                mTransition.Active = value;
            }
        }

        public override bool NeedsRedrawn {
            get {
                return base.NeedsRedrawn || mTransition.NeedsRedrawn;
            }
        }

        public override Rectangle Clip {
            get { return base.Clip; }
            set {
                base.Clip = value;
                if (Clip.Width == 0 || Clip.Height == 0)
                    return;
                mImages = new Bitmap[mRawImages.Length];
                for (int i = 0; i < mRawImages.Length; i++) {
                    lock (mRawImages) {
                        Bitmap img = mRawImages[i];
                        Bitmap n = new Bitmap(Clip.Width, Clip.Height);
                        int x = (Clip.Width - img.Width) / 2;
                        int y = (Clip.Height - img.Height) / 2;
                        using (Graphics g = Graphics.FromImage(n)) {
                            g.FillRectangle(Brushes.Black, Clip);
                            g.DrawImage(img, x, y, img.Width, img.Height);
                        }
                        mImages[i] = n;
                    }
                }
            }
        }

        public override void DrawStatic(Graphics graphics) {
            if (mCurrentImage == -1) {
                mCurrentImage = 0;
                mTransition.Init(mImages[mCurrentImage], mImages[mCurrentImage]);
            }
            base.DrawStatic(graphics);
        }

        public void Prev() {
            int next = (mCurrentImage - 1) % mImages.Length;
            Transition(next >= 0 ? next : mImages.Length - 1);
        }

        public void Next() {
            Transition((mCurrentImage + 1) % mImages.Length);
        }

        private void Transition(int next) {
            mTransition.Init(mImages[mCurrentImage], mImages[next]);
            mTransition.Begin();
            mCurrentImage = next;
            Manager.OverlayWindow.RedrawStatic();
        }
    }
}

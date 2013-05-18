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

namespace Chimera.Overlay.States {
    public class ImageBGWindow : WindowState {
        private Bitmap mBG;
        private Bitmap mDefaultBG;

        public Bitmap BackgroundImage {
            get {
                if (mBG == null) {
                    mDefaultBG = new Bitmap(50, 50);
                    using (Graphics g = Graphics.FromImage(mDefaultBG))
                        g.FillEllipse(Brushes.Black, 0, 0, 50, 50);
                    mBG = mDefaultBG;
                }
                return mBG;
            }
            set {
                if (value != null) {
                    mBG = value;
                    Manager.ForceRedrawStatic();
                    if (mDefaultBG != null) {
                        mDefaultBG.Dispose();
                        mDefaultBG = null;
                    }
                }
            }
        }

        public ImageBGWindow(WindowOverlayManager manager, Bitmap BG)
            : base(manager) {
            mBG = BG;
        }

        public override void DrawStatic(Graphics graphics) {
            graphics.DrawImage(BackgroundImage, Clip);
            base.DrawStatic(graphics);
        }

        protected override void OnActivated() {
            Manager.ControlPointer = true;
            Manager.Opacity = 1.0; ;
        }
    }
}

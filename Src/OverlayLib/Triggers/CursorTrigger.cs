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
using Chimera.Util;
using Chimera.GUI.Forms;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using Chimera.Interfaces.Overlay;
using System.Xml;

namespace Chimera.Overlay.Triggers {
    public class CursorTrigger : ConditionTrigger, IFeature {
        private DateTime mEnter;
        private static float mSelectMS = 1000f;
        private bool mHovering = false;
        private bool mClicked = false;
        private Cursor mSelectCursor;
        private ISelectionRenderer mRenderer;
        private bool mActive = true;
        private WindowOverlayManager mManager;
        private int mR;
        /// <summary>
        /// The clip rectangle bounding the area this item will be drawn to.
        /// </summary>
        private Rectangle mClip;

        public CursorTrigger(ISelectionRenderer renderer, WindowOverlayManager manager)
            : base(manager.Frame.Core, mSelectMS) {
            mManager = manager;
            mRenderer = renderer;
            //mSelectCursor = new Cursor(new IntPtr(65571));
            mSelectCursor = new Cursor(new IntPtr(65567));
        }

        public CursorTrigger(OverlayPlugin plugin, XmlNode node)
            : base(plugin.Core) {
            //TODO add logic for initialisation
        }

        public double HoverTime {
            get { return DateTime.Now.Subtract(mEnter).TotalMilliseconds; }
        }

        public override bool Condition {
            get { 
                bool ret = ProcessWrangler.GetGlobalCursor().Equals(mSelectCursor.Handle);
                if (ret) {
                    if (!mHovering) {
                        mHovering = true;
                        mEnter = DateTime.Now;
                    }
                } else if (mHovering) {
                    mManager.ForceRedraw();
                    mRenderer.Clear();
                    mHovering = false;
                    mClicked = false;
                }
                return ret;
            }
        }

        #region IDrawable Members

        public virtual Rectangle Clip {
            get { return mClip; }
            set { mClip = value; }
        }


        public bool NeedsRedrawn {
            get { return mHovering && !mClicked; }
        }

        public string Frame {
            get { return mManager.Name; }
        }

        public void DrawStatic(Graphics graphics) { }

        public void DrawDynamic(Graphics graphics) {
            if (mHovering && !mClicked) {
                int x = (int)(mClip.Width * mManager.CursorX);
                int y = (int)(mClip.Height * mManager.CursorY);
                Rectangle r = new Rectangle(x - mR, y - mR, mR * 2, mR * 2);
                mRenderer.DrawHover(graphics, r, HoverTime / mSelectMS );
            }
        }

        #endregion
    }
}

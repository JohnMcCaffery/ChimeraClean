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
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using Chimera.Util;
using System.Xml;
using Chimera.Interfaces.Overlay;

namespace Chimera.Overlay.SelectionRenderers {
    public class CursorRendererFactory : ISelectionRendererFactory {
        #region IFactory<ISelectionRenderer> Members

        public ISelectionRenderer Create(OverlayPlugin manager, XmlNode node) {
            return new CursorRenderer(manager, node);
        }

        public ISelectionRenderer Create(OverlayPlugin manager, XmlNode node, Rectangle clip) {
            return Create(manager, node);
        }

        #endregion

        #region IFactory Members

        public string Name {
            get { return "Cursor"; }
        }

        #endregion
    }


    public class CursorRenderer : OverlayXmlLoader, ISelectionRenderer {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetIconInfo(IntPtr hIcon, ref IconInfo pIconInfo);

        [DllImport("user32.dll")]
        public static extern IntPtr CreateIconIndirect(ref IconInfo icon);

        public struct IconInfo {
            public bool fIcon;
            public int xHotspot;
            public int yHotspot;
            public IntPtr hbmMask;
            public IntPtr hbmColor;
        }

        private static readonly HashSet<IntPtr> sCursors = new HashSet<IntPtr>();
        private static int sSteps = 100;
        private Cursor[] mCursors = new Cursor[sSteps];
        private Cursor mCompletedCursor;
        private FrameOverlayManager mOverlayManager;

        public static bool GlobalCursorIsHover() {
            return sCursors.Contains(ProcessWrangler.GetGlobalCursor());
        }

        public static Cursor CreateCursor(Bitmap bmp, int xHotSpot, int yHotSpot) {
            IntPtr ptr = bmp.GetHicon();
            IconInfo tmp = new IconInfo();
            GetIconInfo(ptr, ref tmp);
            tmp.xHotspot = xHotSpot;
            tmp.yHotspot = yHotSpot;
            tmp.fIcon = false;
            ptr = CreateIconIndirect(ref tmp);
            return new Cursor(ptr);
        }

        private void DrawCursor(Cursor cursor) {
            if (mOverlayManager == null) {
                ProcessWrangler.SetGlobalCursor(cursor);
            } else {
                mOverlayManager.Cursor = cursor;
            }
        }

        public CursorRenderer (Action<Graphics, Rectangle, double> drawStep, Size size, FrameOverlayManager manager) {
            mOverlayManager = manager;

            Init(size, drawStep);
        }
        public CursorRenderer (ISelectionRenderer renderer, Size size, FrameOverlayManager manager) {
            mOverlayManager = manager;

            Init(size, renderer.DrawHover);
        }

        public CursorRenderer(OverlayPlugin manager, XmlNode node) {
            ISelectionRenderer renderer = manager.GetRenderer(node, "cursor renderer", null, "InnerRenderer");
            mOverlayManager = GetManager(manager, node, "cursor renderer");

            Size size = new Size(GetInt(node, 80, "W"), GetInt(node, 80, "H"));
            Init(size, renderer.DrawHover);
        }

        private void Init(Size size, Action<Graphics, Rectangle, double> drawStep) {
            if (!GlobalCursorIsHover())
                mCompletedCursor = new Cursor(ProcessWrangler.GetGlobalCursor());

            for (double i = 0.0; i < sSteps; i++) {
                //TODO - using means the bmp is disposed which could cause issues.
                using (Bitmap b = new Bitmap(size.Width, size.Height)) {
                    using (Graphics g = Graphics.FromImage(b)) {
                        drawStep(g, new Rectangle(new Point(0, 0), size), i / sSteps);
                    }
                    Cursor c = CreateCursor(b, size.Width / 2, size.Height / 2);
                    sCursors.Add(c.Handle);
                    mCursors[(int)i] = c;
                }
            }
        }

        #region IHoverSelectorRenderer Members

        public void DrawHover(Graphics graphics, Rectangle bounds, double hoverDone) {
            int i = (int) (hoverDone * mCursors.Length);
            if (i < mCursors.Length)
                DrawCursor(mCursors[i]);
        }

        public void DrawSelected(Graphics graphics, Rectangle bounds) { 
            ResetCursor();
        }

        public void Clear() {
            ResetCursor();
        }

        #endregion

        private void ResetCursor() {
            if (mOverlayManager == null) {
                ProcessWrangler.SetGlobalCursor(mCompletedCursor);
            } else {
                mOverlayManager.ResetCursor();
            }
        }
    }
}

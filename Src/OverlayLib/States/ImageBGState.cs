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
using Chimera.Overlay;
using System.Drawing;
using Chimera.Interfaces.Overlay;
using System.Xml;

namespace Chimera.Overlay.States {
    public class ImageBGStateFactory : IStateFactory {
        #region IFactory<State> Members

        public string Name {
            get { return "ImageBG"; }
        }

        public State Create(OverlayPlugin manager, XmlNode node) {
            return new ImageBGState(manager, node);
        }

        public State Create(OverlayPlugin manager, XmlNode node, Rectangle clip) {
            return Create(manager, node);
        }

        #endregion
    }

    public class ImageBGState : State {
        private readonly Dictionary<string, Bitmap> mWindowBGs = new Dictionary<string, Bitmap>();
        private readonly Dictionary<string, ImageBGWindow> mWindows = new Dictionary<string, ImageBGWindow>();
        private Bitmap mDefaultBG;

        public ImageBGState(string name, OverlayPlugin manager, Bitmap defaultBG)
            : base(name, manager) {
            mDefaultBG = defaultBG;
            foreach (var window in mWindows.Values)
                window.BackgroundImage = defaultBG;
        }

        public ImageBGState(string name, OverlayPlugin manager, string defaultBG)
            : this(name, manager, new Bitmap(defaultBG)) {
        }

        public ImageBGState(OverlayPlugin manager, XmlNode node)
            : base(GetName(node, "image background state"), manager) {

            mDefaultBG = new Bitmap(manager.Clip.Width, manager.Clip.Height);
            using (Graphics g = Graphics.FromImage(mDefaultBG))
                g.FillRectangle(Brushes.Black, manager.Clip);

            foreach (XmlNode child in node.ChildNodes) {
                if (!(child is XmlElement))
                    continue;
                Bitmap img = GetImage(child, "image background state");
                if (img != null) {
                    string window = GetManager(manager, child, "image background state").Window.Name;
                    mWindowBGs.Add(window, img);
                    if (mWindows.ContainsKey(window))
                        mWindows[window].BackgroundImage = img;
                }
            }
        }

        /// <summary>
        /// CreateWindowState a window state for drawing this state to the specified window.
        /// </summary>
        /// <param name="window">The window the new window state is to draw on.</param>
        public override IWindowState CreateWindowState(WindowOverlayManager manager) {
            ImageBGWindow win = new ImageBGWindow(manager, mWindowBGs.ContainsKey(manager.Name) ?  mWindowBGs[manager.Name] : mDefaultBG);
            mWindows.Add(manager.Name, win);
            return win;
        }

        /// <summary>
        /// Map a window name to a background image.
        /// </summary>
        /// <param name="window">The name of the window to map the image to.</param>
        /// <param name="image">The image to map.</param>
        public void MapWindowImage(string window, Bitmap image) {
            mWindowBGs.Add(window, image);
            if (mWindows.ContainsKey(window))
                mWindows[window].BackgroundImage = image;
        }

        protected override void TransitionToFinish() {
            Manager.Coordinator.EnableUpdates = false;
        }

        protected override void TransitionFromStart() { }

        public override void TransitionToStart() { }

        public override void TransitionFromFinish() { }
    }
}

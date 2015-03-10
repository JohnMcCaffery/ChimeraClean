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
using System.Drawing;
using System.Xml;
using Chimera.Interfaces.Overlay;
using CefSharp.WinForms;
using System.Windows.Forms;
using CefSharp.Example;

namespace Chimera.Overlay.Features
{
    public class BrowserFeatureFactory : IFeatureFactory
    {
        #region IFactory<IFeature> Members

        public IFeature Create(OverlayPlugin manager, XmlNode node)
        {
            return new BrowserFeature(manager, node);
        }

        public IFeature Create(OverlayPlugin manager, XmlNode node, Rectangle clip)
        {
            return new BrowserFeature(manager, node, clip);
        }

        #endregion

        #region IFactory Members

        public string Name
        {
            get { return "Browser"; }
        }

        #endregion
    }
    public class BrowserFeature : OverlayXmlLoader, IFeature
    {
        private FrameOverlayManager mManager;
        private bool mNeedsRedrawn;
        private ChromiumWebBrowser browser;
        private bool mActive;
        private Rectangle mClip;
        private RectangleF mBounds = new RectangleF(0f, 0f, 1f, 1f);
        private string mFrame;

        public  BrowserFeature(OverlayPlugin manager, XmlNode node)
        {
            mManager = GetManager(manager, node, "text");
        }

        public BrowserFeature(OverlayPlugin manager, XmlNode node, Rectangle clip)
        {
            mManager = GetManager(manager, node, "text");
            mClip = clip;
            mBounds = manager.GetBounds(node, "Browser Feature");

            CefExample.Init();

            browser = new ChromiumWebBrowser("http://get.webgl.org/")
            {
                Dock = DockStyle.Fill,
            };
            
            browser.RegisterJsObject("bound", new BoundObject());
        }

        public Rectangle Clip
        {
            get { return mClip; }
            set { mClip = value; }
        }

        public bool Active
        {
            get { return mActive; }
            set
            {
                if (mActive != value)
                {
                    if (value)
                    {
                        mManager.OverlayWindow.AddControl(browser, mBounds);
                    }
                    else
                    {
                        mManager.OverlayWindow.RemoveControl(browser);
                    }
                }
                mActive = value;
            }
        }


        public string Frame
        {
            get { return mFrame; }
        }

        public bool NeedsRedrawn
        {
            get { return mNeedsRedrawn; }
        }

        public string TextString
        {
            get { return null; }
            set
            {
                mNeedsRedrawn = true;
            }
        }

        public void DrawStatic(Graphics graphics) { }

        public void DrawDynamic(Graphics graphics)
        {
        }
    }
}

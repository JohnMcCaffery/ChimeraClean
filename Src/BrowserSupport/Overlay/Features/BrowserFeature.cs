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
using Chimera.Overlay.Features;
using Chimera.Overlay;
using CefSharp;
using System.Diagnostics;
using BrowserLib.Overlay.Triggers;

namespace Chimera.BrowserLib.Features
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
    public class BrowserFeature : ControlFeature<ChromiumWebBrowser>, IFeature
    {
        private const bool SINGLETON = false;
        private static bool sInitialised;

        private RectangleF mBounds = new RectangleF(0f, 0f, 1f, 1f);
        private string mUrl;
        private ChromiumWebBrowser mBrowser;

        private static HashSet<ChromiumWebBrowser> sActiveFeatures = new HashSet<ChromiumWebBrowser>();

	/// <summary>
	/// Checks whether the supplied browser is currently activated.
	/// </summary>
	/// <param name="browser">The browser to check.</param>
	/// <returns>True if the browser is active.</returns>
        public static bool IsActive(ChromiumWebBrowser browser) {
            return sActiveFeatures.Contains(browser);
        }

        private void Init() {
            if (!sInitialised) {

                var settings = new CefSettings();
                settings.RemoteDebuggingPort = 8088;
                //settings.CefCommandLineArgs.Add("renderer-process-limit", "1");
                //settings.CefCommandLineArgs.Add("renderer-startup-dialog", "renderer-startup-dialog");
                settings.LogSeverity = LogSeverity.Warning;

                if (Debugger.IsAttached) {
                    var architecture = Environment.Is64BitProcess ? "x64" : "x86";
                    settings.BrowserSubprocessPath = "BrowserSubprocess\\" + architecture + "\\Debug\\CefSharp.BrowserSubprocess.exe";
                }


                settings.RegisterScheme(new CefCustomScheme {
                    SchemeName = CefSharpSchemeHandlerFactory.SchemeName,
                    SchemeHandlerFactory = new CefSharpSchemeHandlerFactory()
                });
                settings.UserAgent = "ChromeCEF";

                if (!Cef.Initialize(settings)) {
                    if (Environment.GetCommandLineArgs().Contains("--type=renderer")) {
                        Environment.Exit(0);
                    } else {
                        return;
                    }
                }
                sInitialised = true;
            }

            mBrowser = new ChromiumWebBrowser(mUrl);
            PageLoadTrigger.RegisterBrowser(mBrowser);

            base.Active = true;
            base.Active = false;

            mBrowser.RegisterJsObject("bound", new BoundObject());

        }

        public BrowserFeature(OverlayPlugin manager, XmlNode node)
            : base(manager, node, SINGLETON) {
            mUrl = GetString(node, "http://get.webgl.org/", "URL");
            Init();
        }

        public BrowserFeature(OverlayPlugin manager, XmlNode node, Rectangle clip)
            : base(manager, node, SINGLETON, clip) {
            mUrl = GetString(node, "http://get.webgl.org/", "URL");
            Init();
        }

        protected override Func<ChromiumWebBrowser> MakeControl {
            get { return new Func<ChromiumWebBrowser>(() => mBrowser); }
        }

        protected override Func<Control> MakeControlPanel {
            get { return null; }
        }



        internal class CefSharpSchemeHandlerFactory : ISchemeHandlerFactory {
            public const string SchemeName = "custom";

            public ISchemeHandler Create() {
                return new CefSharpSchemeHandler();
            }
        }

        public override bool Active {
            get { return base.Active; }
            set {
                if (value) {
                    if (base.Active)
                        Control.Load(mUrl);
                    else
                        sActiveFeatures.Add(Control);
                } else {
                    sActiveFeatures.Remove(Control);
                    Control.Load(mUrl);
                }
                base.Active = value;
            }
        }

        public override string ToString() {
            return mUrl;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CefSharp.WinForms;
using Chimera.Overlay;
using Chimera.Interfaces.Overlay;
using Chimera.Overlay.Triggers;
using System.Xml;
using Chimera.BrowserLib.Features;

namespace BrowserLib.Overlay.Triggers {
    public class PageLoadTriggerFactory : ITriggerFactory {
        public SpecialTrigger Special {
            get { return SpecialTrigger.None; }
        }

        public string Mode {
            get { return "None"; }
        }

        public ITrigger Create(OverlayPlugin manager, XmlNode node) {
            return new PageLoadTrigger(manager, node);
        }

        public ITrigger Create(OverlayPlugin manager, XmlNode node, System.Drawing.Rectangle clip) {
            return Create(manager, node);
        }

        public string Name {
            get { return "PageLoad"; }
        }
    }
    public class PageLoadTrigger :TriggerBase {
        public static void RegisterBrowser(ChromiumWebBrowser browser) {
            browser.AddressChanged += (source, args) => {
                if (PageLoaded != null)
                    PageLoaded(args.Address, browser);
            };
        }

        private static event Action<string, ChromiumWebBrowser> PageLoaded;

        private bool mActive;
        private string mUrl;
        private Action<string, ChromiumWebBrowser> mPageLoadListener;

        public PageLoadTrigger(OverlayPlugin manager, XmlNode node)
            : base(node) {
            mUrl = GetString(node, "http://openvirtualworlds.org", "URL");

            mPageLoadListener = (addr, browser) => {
                                if (BrowserFeature.IsActive(browser) && mUrl == addr)
                                    Trigger();
                            };
        }

        public override bool Active {
            get { return mActive; }
            set {
                if (mActive != value) {
                    if (value)
                        PageLoaded += mPageLoadListener;
                    else
                        PageLoaded -= mPageLoadListener;
                    mActive = value;
                }
            }
        }
    }
}

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
using log4net;
using CefSharp;

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
        public const string ANY_PAGE = "ANY_PAGE";

        public static void RegisterBrowser(ChromiumWebBrowser browser) {
            browser.AddressChanged += (source, args) => {
                if (PageLoaded != null)
                    PageLoaded(args.Address, browser);
            };
        }


        private static event Action<string, IWebBrowser> PageLoaded;
        public static String TriggeredURL = "";

        private bool mActive;
        private string mUrl;
        private bool mNot;
        private bool mSaveURL;
        private Action<string, IWebBrowser> mPageLoadListener;
        private readonly ILog Logger;

        public PageLoadTrigger(OverlayPlugin manager, XmlNode node)
            : base(node) {
                Logger = LogManager.GetLogger("PageLoadTrigger");
            mUrl = GetString(node, "http://openvirtualworlds.org", "URL");
            mNot = GetBool(node, false, "Not");
            mSaveURL = GetBool(node, false, "SaveURL");

            mPageLoadListener = (addr, browser) => {
                if (BrowserFeature.IsActive(browser) && (mUrl == ANY_PAGE || (mUrl == addr && !mNot) || (mUrl != addr && mNot))) {
                    //Logger.WarnFormat("Triggering for : {0}", addr);
                    if(mSaveURL) TriggeredURL = addr;
                    Trigger();
                }
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

        public static void TriggerPageLoaded(string url, IWebBrowser browser) {
            if (PageLoaded != null)
                PageLoaded(url, browser);
        }
    }
}

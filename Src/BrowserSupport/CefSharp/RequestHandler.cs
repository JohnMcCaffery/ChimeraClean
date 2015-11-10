using System;
using log4net;
using CefSharp;
using System.Collections.Generic;
using BrowserLib.Overlay.Triggers;

namespace Chimera.BrowserLib
{
    public class RequestHandler : IRequestHandler
    {
        //private static readonly ILog Logger = LogManager.GetLogger("RequestHandler");
        private string mURL;
        private List<string> mBlackList = null;
        private List<string> mWhiteList = null;

        public RequestHandler() { }
        public RequestHandler(String url, String whiteList, String blackList) {
            mURL = url;
            if (whiteList != "")
                mWhiteList = new List<string>(whiteList.Split(new char[] {';'}));
            if (blackList != "")
                mBlackList = new List<string>(blackList.Split(new char[] { ';' }));
        }

        public static readonly string VersionNumberString = String.Format("Chromium: {0}, CEF: {1}, CefSharp: {2}",
            Cef.ChromiumVersion, Cef.CefVersion, Cef.CefSharpVersion);

        bool IRequestHandler.OnBeforeBrowse(IWebBrowser browser, IRequest request, bool isRedirect)
        {
            //Logger.WarnFormat("Before Loading page: {0} {1}", request.Url, request.TransitionType);
            if (request.Url == mURL || (mWhiteList != null && mWhiteList.Contains(request.Url))) {
                //Logger.WarnFormat("In whitelist");
                return false;
            } else if (mWhiteList == null) {
                //Logger.WarnFormat("No whitelist");
                if (mBlackList != null && mBlackList.Contains(request.Url)) {
                    PageLoadTrigger.TriggerPageLoaded(request.Url, browser);
                    //Logger.WarnFormat("In blacklistlist");
                    return true;
                } else {
                    //Logger.WarnFormat("Not in blacklist");
                    return false;
                }
            } else {
                PageLoadTrigger.TriggerPageLoaded(request.Url, browser);
                //Logger.WarnFormat("Not in whitelist");
                return true;
            }
        }

        bool IRequestHandler.OnCertificateError(IWebBrowser browser, CefErrorCode errorCode, string requestUrl)
        {
            return false;
        }

        void IRequestHandler.OnPluginCrashed(IWebBrowser browser, string pluginPath)
        {
            // TODO: Add your own code here for handling scenarios where a plugin crashed, for one reason or another.
        }

        bool IRequestHandler.OnBeforeResourceLoad(IWebBrowser browser, IRequest request, IResponse response) {
            return false;
        }

        bool IRequestHandler.GetAuthCredentials(IWebBrowser browser, bool isProxy, string host, int port, string realm, string scheme, ref string username, ref string password)
        {
            return false;
        }

        bool IRequestHandler.OnBeforePluginLoad(IWebBrowser browser, string url, string policyUrl, IWebPluginInfo info)
        {
            bool blockPluginLoad = false;

            // Enable next line to demo: Block any plugin with "flash" in its name
            // try it out with e.g. http://www.youtube.com/watch?v=0uBOtQOO70Y
            //blockPluginLoad = info.Name.ToLower().Contains("flash");
            return blockPluginLoad;
        }

        void IRequestHandler.OnRenderProcessTerminated(IWebBrowser browser, CefTerminationStatus status)
        {
            // TODO: Add your own code here for handling scenarios where the Render Process terminated for one reason or another.
        }
    }
}

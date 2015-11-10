using log4net;
using CefSharp;
namespace Chimera.BrowserLib
{
    public class JsDialogHandler : IJsDialogHandler
    {
        private static readonly ILog Logger = LogManager.GetLogger("JsDialogHandler");
        public bool OnJSAlert(IWebBrowser browser, string url, string message)
        {
            Logger.WarnFormat("Alert from URL: {0} Message: {1}", url, message);
            //browser.Reload();
            return true;
        }

        public bool OnJSConfirm(IWebBrowser browser, string url, string message, out bool retval)
        {
            retval = false;

            return true;
        }

        public bool OnJSPrompt(IWebBrowser browser, string url, string message, string defaultValue, out bool retval, out string result)
        {
            retval = false;
            result = null;

            return true;
        }

        public bool OnJSBeforeUnload(IWebBrowser browser, string message, bool isReload, out bool allowUnload)
        {
            //NOTE: Setting allowUnload to false will cancel the unload request
            allowUnload = false;

            //NOTE: Returning false will trigger the default behaviour, you need to return true to handle yourself.
            return false;
        }
    }
}

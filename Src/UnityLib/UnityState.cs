using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Interfaces.Overlay;
using Chimera.Overlay.Features;
using System.Drawing;
using System.Diagnostics;
using Chimera.Util;
using System.IO;
using System.Threading;
using Chimera.Overlay.Triggers;
using Chimera.Overlay;
using Chimera.Overlay.States;
using System.Xml;
using System.Windows.Forms;
using Chimera.Interfaces;
using Chimera.Overlay.Transitions;
using log4net;
using System.Runtime.InteropServices;

namespace Chimera.Overlay.States
{
    public class UnityStateFactory : IStateFactory
    {

        public UnityStateFactory()
        {
        }

        public string Name
        {
            get { return "Unity"; }
        }

        public State Create(OverlayPlugin manager, XmlNode node)
        {
            return new UnityState(manager, node);
        }

        public State Create(OverlayPlugin manager, XmlNode node, Rectangle clip)
        {
            return Create(manager, node);
        }
    }

    public class UnityState : State
    {
        [DllImport("User32.dll")]
        static extern bool MoveWindow(IntPtr handle, int x, int y, int width, int height, bool redraw);

        internal delegate int WindowEnumProc(IntPtr hwnd, IntPtr lparam);
        [DllImport("user32.dll")]
        internal static extern bool EnumChildWindows(IntPtr hwnd, WindowEnumProc func, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern int SendMessage(IntPtr hWnd, int msg, int wParam, uint lParam);

        private readonly ILog Logger = LogManager.GetLogger("Overlay.Unity");
        private string mUnity;
        private FrameOverlayManager mMainWindow;
        private SimpleTrigger mTrigger;
        private RectangleF mBounds = new RectangleF(0f, 0f, 1f, 1f);
        private OverlayPlugin mManager;

        private Process process;
        private IntPtr unityHWND = IntPtr.Zero;
        private const int WM_ACTIVATE = 0x0006;
        private const int WA_ACTIVE = 1;
        private System.EventHandler ResizedHandler;
        private bool Started = false;

        private static Bitmap mDefaultBG;

        private static Bitmap DefaultBG
        {
            get
            {
                if (mDefaultBG == null)
                {
                    mDefaultBG = new Bitmap(50, 50);
                    using (Graphics g = Graphics.FromImage(mDefaultBG))
                        g.FillEllipse(Brushes.Black, 0, 0, 50, 50);
                }
                return mDefaultBG;
            }
        }

        public UnityState(OverlayPlugin manager, XmlNode node)
            : base(GetName(node, "creating unity state"), manager, node, false)
        {
            mUnity = Path.GetFullPath(GetString(node, null, "File"));
            mManager = manager;
            mMainWindow = GetManager(manager, node, "unity state");
            mBounds = manager.GetBounds(node, "unity state");

            ResizedHandler = new System.EventHandler(this.Resized);
        }

        protected override void TransitionToStart()
        {
        }

        protected override void TransitionToFinish()
        {
            Start();
        }

        protected override void TransitionFromStart()
        {
            Stop();
        }

        protected override void TransitionFromFinish()
        {
            Stop();

        }

        private void Start()
        {
            if (Started) return;
            Logger.InfoFormat("Starting {0}", mUnity);
            mMainWindow.OverlayWindow.Resize += ResizedHandler;
            process = new Process();
            process.StartInfo.FileName = mUnity;
            process.StartInfo.Arguments = "-parentHWND " + mMainWindow.OverlayWindow.Handle.ToInt32();
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.CreateNoWindow = true;

            process.Start();

            process.WaitForInputIdle();
            // Doesn't work for some reason ?!
            //unityHWND = process.MainWindowHandle;
            EnumChildWindows(mMainWindow.OverlayWindow.Handle, WindowEnum, IntPtr.Zero);
            Started = true;
        }

        private int WindowEnum(IntPtr hwnd, IntPtr lparam)
        {
            unityHWND = hwnd;
            SendMessage(unityHWND, WM_ACTIVATE, WA_ACTIVE, 0);
            Logger.InfoFormat("WindowUnum {0}", unityHWND);
            return 0;
        }

        private void Stop()
        {
            if (!Started) return;
            mMainWindow.OverlayWindow.Resize -= ResizedHandler;
            try
            {
                process.CloseMainWindow();

                Thread.Sleep(1000);
                while (process.HasExited == false)
                    process.Kill();
            }
            catch (Exception)
            {

            }
            Started = false;
        }

        private void Resized(object sender, EventArgs e)
        {
            MoveWindow(unityHWND, 0, 0, mMainWindow.OverlayWindow.Width, mMainWindow.OverlayWindow.Height, true);
            SendMessage(unityHWND, WM_ACTIVATE, WA_ACTIVE, 0);
            Logger.Info("Window Resized");
        }
    }
}

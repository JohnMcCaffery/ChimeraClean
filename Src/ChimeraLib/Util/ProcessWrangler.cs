using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using Chimera.Interfaces;

namespace Chimera.Util {
    public static class ProcessWrangler {
        private static ICrashable sRoot;
        private static Form sForm;
        private static int sCurrentScreen = 0;

        //Send a keyboard event
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMEssage(int hWnd, int Msg, int wParam, int lParam);

        //Trigger keyboard event
        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

        // Get a handle to an application window.
        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        // Activate an application window.
        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private extern static bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);

        public static void SwitchWindowMonitor(Process proc) {
            Screen screen = Screen.AllScreens[sCurrentScreen++%Screen.AllScreens.Count()];
            SetWindowPos(proc.MainWindowHandle, new IntPtr(0), screen.Bounds.X, screen.Bounds.Y, screen.Bounds.Width, screen.Bounds.Height, 0x0004);
        }

        public static void PressKey(Process app, string key) {
            PressKey(app, key, false, false, false);
        }

        public static void PressKey(Process app, string key, bool ctrl, bool alt, bool shift) {
            if (!app.HasExited) {
                SetForegroundWindow(app.MainWindowHandle);
                PressKey(key, ctrl, alt, shift);
            }
        }

        public static void PressKey(string key) {
            PressKey(key, false, false, false);
        }

        public static void PressKey(string key, bool ctrl, bool alt, bool shift) {
            SendKeys.SendWait((ctrl ? "^" : "") + (alt ? "%" : "") + (shift ? "+" : "") + key);
        }

        public static Process InitProcess(string exe) {
            return InitProcess(exe, Path.GetDirectoryName(exe), "");
        }

        public static Process InitProcess(string exe, string workingDir, string args) {
            Process process = new Process();
            process.StartInfo.FileName = exe;
            process.StartInfo.WorkingDirectory = workingDir;
            process.StartInfo.Arguments =  args;
            return process;
        }

        public static void BlockingRunForm(Form form, ICrashable root, bool autoRestart) {
            Application.EnableVisualStyles();
            sForm = form;
            sRoot = root;
            if (autoRestart) {
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
                Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
                try {
                    Application.Run(form);
                } catch (Exception e) {
                    Console.WriteLine("Exception caught from GUI thread.");
                    HandleException(e);
                }
            } else
                Application.Run(form);
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e) {
            Console.WriteLine("Exception caught from ThreadException.");
            sForm.Close();
            HandleException(e.Exception);
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e) {
            Console.WriteLine("Exception caught from App Domain.");
            sForm.Close();
            HandleException((Exception)e.ExceptionObject);
        }

        static void HandleException(Exception e) {
            Console.WriteLine(e.Message);
            Console.WriteLine(e.StackTrace);
            Console.WriteLine("Program crashed starting, a new instance.");
            sRoot.OnCrash(e);
            ProcessWrangler.InitProcess(Assembly.GetEntryAssembly().Location).Start();
        }
    }
}

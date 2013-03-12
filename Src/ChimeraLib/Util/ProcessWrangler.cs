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
        // --- SetWindowLong -  nIndex ---
        /// <summary>
        /// Sets a new extended window style.
        /// </summary>
        private static readonly int GWL_EXSTYLE = -20;
        /// <summary>
        /// Sets a new application instance handle.
        /// </summary>
        private static readonly int GWL_HINSTANCE = -6;
        /// <summary>
        /// Sets a new identifier of the child window. The window cannot be a top-level window.
        /// </summary>
        private static readonly int GWL_ID = -12;
        /// <summary>
        /// Sets a new window style.
        /// </summary>
        private static readonly int GWL_STYLE = -16;
        /// <summary>
        /// Sets the user data associated with the window. This data is intended for use by the application that created the window. Its value is initially zero.
        /// </summary>
        private static readonly int GWL_USERDATA = -21;
        /// <summary>
        /// Sets a new address for the window procedure.  You cannot change this attribute if the window does not belong to the same process as the calling thread.
        /// </summary>
        private static readonly int GWL_WNDPROC = -4;


        // --- Window Styles  ---
        /// <summary>
        /// The window has a thin-line border.
        /// </summary>
        private static readonly long WS_BORDER = 0x00800000L;
        /// <summary>
        /// The window has a title bar (includes the WS_BORDER style).
        /// </summary>
        private static readonly long WS_CAPTION = 0x00C00000L;
        /// <summary>
        ///The window is a child window. A window with this style cannot have a menu bar. This style cannot be used with the WS_POPUP style.
        /// </summary>
        private static readonly long WS_CHILD = 0x40000000L;
        /// <summary>
        /// Same as the WS_CHILD style.
        /// </summary>
        private static readonly long WS_CHILDWINDOW = 0x40000000L;
        /// <summary>
        /// Excludes the area occupied by child windows when drawing occurs within the parent window. This style is used when creating the parent window.
        /// </summary>
        private static readonly long WS_CLIPCHILDREN = 0x02000000L;
        /// <summary>
        /// Clips child windows relative to each other; that is, when a particular child window receives a WM_PAINT message, the WS_CLIPSIBLINGS style clips all other overlapping child windows out of the region of the child window to be updated. If WS_CLIPSIBLINGS is not specified and child windows overlap, it is possible, when drawing within the client area of a child window, to draw within the client area of a neighboring child window.
        /// </summary>
        private static readonly long WS_CLIPSIBLINGS = 0x04000000L;
        /// <summary>
        /// The window is initially disabled. A disabled window cannot receive input from the user. To change this after a window has been created, use the EnableWindow function.
        /// </summary>
        private static readonly long WS_DISABLED = 0x08000000L;
        /// <summary>
        /// The window has a border of a style typically used with dialog boxes. A window with this style cannot have a title bar.
        /// </summary>
        private static readonly long WS_DLGFRAME = 0x00400000L;
        /// <summary>
        /// The window is the first control of a group of controls. The group consists of this first control and all controls defined after it, up to the next control with the WS_GROUP style. The first control in each group usually has the WS_TABSTOP style so that the user can move from group to group. The user can subsequently change the keyboard focus from one control in the group to the next control in the group by using the direction keys.
        /// You can turn this style on and off to change dialog box navigation. To change this style after a window has been created, use the SetWindowLong function.
        /// </summary>
        private static readonly long WS_GROUP = 0x00020000L;
        /// <summary>
        /// The window has a horizontal scroll bar.
        /// </summary>
        private static readonly long WS_HSCROLL = 0x00100000L;
        /// <summary>
        /// The window is initially minimized. Same as the WS_MINIMIZE style.
        /// </summary>
        private static readonly long WS_ICONIC = 0x20000000L;
        /// <summary>
        /// The window is initially maximized.
        /// </summary>
        private static readonly long WS_MAXIMIZE = 0x01000000L;
        /// <summary>
        /// The window has a maximize button. Cannot be combined with the WS_EX_CONTEXTHELP style. The WS_SYSMENU style must also be specified.
        /// </summary>
        private static readonly long WS_MAXIMIZEBOX = 0x00010000L;
        /// <summary>
        /// The window is initially minimized. Same as the WS_ICONIC style.
        /// </summary>
        private static readonly long WS_MINIMIZE = 0x20000000L;
        /// <summary>
        /// The window has a minimize button. Cannot be combined with the WS_EX_CONTEXTHELP style. The WS_SYSMENU style must also be specified.
        /// </summary>
        private static readonly long WS_MINIMIZEBOX = 0x00020000L;
        /// <summary>
        /// The window is an overlapped window. An overlapped window has a title bar and a border. Same as the WS_TILED style.
        /// </summary>
        private static readonly long WS_OVERLAPPED = 0x00000000L;
        /// <summary>
        /// The window is an overlapped window. Same as the WS_TILEDWINDOW style.
        /// </summary>
        private static readonly long WS_OVERLAPPEDWINDOW = (WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX);
        /// <summary>
        /// The windows is a pop-up window. This style cannot be used with the WS_CHILD style.
        /// </summary>
        private static readonly long WS_POPUP = 0x80000000L;
        /// <summary>
        /// The window is a pop-up window. The WS_CAPTION and WS_POPUPWINDOW styles must be combined to make the window menu visible.
        /// </summary>
        private static readonly long WS_POPUPWINDOW = (WS_POPUP | WS_BORDER | WS_SYSMENU);
        /// <summary>
        /// The window has a sizing border. Same as the WS_THICKFRAME style.
        /// </summary>
        private static readonly long WS_SIZEBOX = 0x00040000L;
        /// <summary>
        /// The window has a window menu on its title bar. The WS_CAPTION style must also be specified.
        /// </summary>
        private static readonly long WS_SYSMENU = 0x00080000L;
        /// <summary>
        /// The window is a control that can receive the keyboard focus when the user presses the TAB key. Pressing the TAB key changes the keyboard focus to the next control with the WS_TABSTOP style.
        /// You can turn this style on and off to change dialog box navigation. To change this style after a window has been created, use the SetWindowLong function. For user-created windows and modeless dialogs to work with tab stops, alter the message loop to call the IsDialogMessage function.
        /// </summary>
        private static readonly long WS_TABSTOP = 0x00010000L;
        /// <summary>
        /// The window has a sizing border. Same as the WS_SIZEBOX style.
        /// </summary>
        private static readonly long WS_THICKFRAME = 0x00040000L;
        /// <summary>
        /// The window is an overlapped window. An overlapped window has a title bar and a border. Same as the WS_OVERLAPPED style.
        /// </summary>
        private static readonly long WS_TILED = 0x00000000L;
        /// <summary>
        /// The window is an overlapped window. Same as the WS_OVERLAPPEDWINDOW style.
        /// </summary>
        private static readonly long WS_TILEDWINDOW = (WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX);

        // ----- Extended window styles -----
        /// <summary>
        /// The window accepts drag-drop files.
        /// </summary>
        private static readonly long WS_EX_ACCEPTFILES = 0x00000010L;
        /// <summary>
        /// Forces a top-level window onto the taskbar when the window is visible.
        /// </summary>
        private static readonly long WS_EX_APPWINDOW = 0x00040000L;
        /// <summary>
        /// The window has a border with a sunken edge.
        /// </summary>
        private static readonly long WS_EX_CLIENTEDGE = 0x00000200L;
        /// <summary>
        /// Paints all descendants of a window in bottom-to-top painting order using double-buffering. For more information, see Remarks. This cannot be used if the window has a class style of either CS_OWNDC or CS_CLASSDC.
        /// Windows 2000:  This style is not supported.
        /// </summary>
        private static readonly long WS_EX_COMPOSITED = 0x02000000L;
        /// <summary>
        /// The title bar of the window includes a question mark. When the user clicks the question mark, the cursor changes to a question mark with a pointer. If the user then clicks a child window, the child receives a WM_HELP message. The child window should pass the message to the parent window procedure, which should call the WinHelp function using the HELP_WM_HELP command. The Help application displays a pop-up window that typically contains help for the child window.
        /// WS_EX_CONTEXTHELP cannot be used with the WS_MAXIMIZEBOX or WS_MINIMIZEBOX styles.
        /// </summary>
        private static readonly long WS_EX_CONTEXTHELP = 0x00000400L;
        /// <summary>
        /// The window itself contains child windows that should take part in dialog box navigation. If this style is specified, the dialog manager recurses into children of this window when performing navigation operations such as handling the TAB key, an arrow key, or a keyboard mnemonic.
        /// </summary>
        private static readonly long WS_EX_CONTROLPARENT = 0x00010000L;
        /// <summary>
        /// The window has a double border; the window can, optionally, be created with a title bar by specifying the WS_CAPTION style in the dwStyle parameter.
        /// </summary>
        private static readonly long WS_EX_DLGMODALFRAME = 0x00000001L;
        /// <summary>
        /// The window is a layered window. This style cannot be used if the window has a class style of either CS_OWNDC or CS_CLASSDC.
        /// Windows 8:  The WS_EX_LAYERED style is supported for top-level windows and child windows. Previous Windows versions support WS_EX_LAYERED only for top-level windows.
        /// </summary>
        private static readonly long WS_EX_LAYERED = 0x00080000;
        /// <summary>
        /// If the shell language is Hebrew, Arabic, or another language that supports reading order alignment, the horizontal origin of the window is on the right edge. Increasing horizontal values advance to the left.
        /// </summary>
        private static readonly long WS_EX_LAYOUTRTL = 0x00400000L;
        /// <summary>
        /// The window has generic left-aligned properties. This is the default.
        /// </summary>
        private static readonly long WS_EX_LEFT = 0x00000000L;
        /// <summary>
        /// If the shell language is Hebrew, Arabic, or another language that supports reading order alignment, the vertical scroll bar (if present) is to the left of the client area. For other languages, the style is ignored.
        /// </summary>
        private static readonly long WS_EX_LEFTSCROLLBAR = 0x00004000L;
        /// <summary>
        /// The window text is displayed using left-to-right reading-order properties. This is the default.
        /// </summary>
        private static readonly long WS_EX_LTRREADING = 0x00000000L;
        /// <summary>
        /// The window is a MDI child window.
        /// </summary>
        private static readonly long WS_EX_MDICHILD = 0x00000040L;
        /// <summary>
        /// A top-level window created with this style does not become the foreground window when the user clicks it. The system does not bring this window to the foreground when the user minimizes or closes the foreground window.
        /// To activate the window, use the SetActiveWindow or SetForegroundWindow function.
        /// The window does not appear on the taskbar by default. To force the window to appear on the taskbar, use the WS_EX_APPWINDOW style.
        /// </summary>
        private static readonly long WS_EX_NOACTIVATE = 0x08000000L;
        /// <summary>
        /// The window does not pass its window layout to its child windows.
        /// </summary>
        private static readonly long WS_EX_NOINHERITLAYOUT = 0x00100000L;
        /// <summary>
        /// The child window created with this style does not send the WM_PARENTNOTIFY message to its parent window when it is created or destroyed.
        /// </summary>
        private static readonly long WS_EX_NOPARENTNOTIFY = 0x00000004L;
        /// <summary>
        /// The window does not render to a redirection surface. This is for windows that do not have visible content or that use mechanisms other than surfaces to provide their visual.
        /// </summary>
        private static readonly long WS_EX_NOREDIRECTIONBITMAP = 0x00200000L;
        /// <summary>
        /// The window is an overlapped window.
        /// </summary>
        private static readonly long WS_EX_OVERLAPPEDWINDOW = (WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE);
        /// <summary>
        /// The window is palette window, which is a modeless dialog box that presents an array of commands.
        /// </summary>
        private static readonly long WS_EX_PALETTEWINDOW = (WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST);
        /// <summary>
        /// The window has generic "right-aligned" properties. This depends on the window class. This style has an effect only if the shell language is Hebrew, Arabic, or another language that supports reading-order alignment; otherwise, the style is ignored.
        /// Using the WS_EX_RIGHT style for static or edit controls has the same effect as using the SS_RIGHT or ES_RIGHT style, respectively. Using this style with button controls has the same effect as using BS_RIGHT and BS_RIGHTBUTTON styles.
        /// </summary>
        private static readonly long WS_EX_RIGHT = 0x00001000L;
        /// <summary>
        /// The vertical scroll bar (if present) is to the right of the client area. This is the default.
        /// </summary>
        private static readonly long WS_EX_RIGHTSCROLLBAR = 0x00000000L;
        /// <summary>
        /// If the shell language is Hebrew, Arabic, or another language that supports reading-order alignment, the window text is displayed using right-to-left reading-order properties. For other languages, the style is ignored.
        /// </summary>
        private static readonly long WS_EX_RTLREADING = 0x00002000L;
        /// <summary>
        /// The window has a three-dimensional border style intended to be used for items that do not accept user input.
        /// </summary>
        private static readonly long WS_EX_STATICEDGE = 0x00020000L;
        /// <summary>
        /// The window is intended to be used as a floating toolbar. A tool window has a title bar that is shorter than a normal title bar, and the window title is drawn using a smaller font. A tool window does not appear in the taskbar or in the dialog that appears when the user presses ALT+TAB. If a tool window has a system menu, its icon is not displayed on the title bar. However, you can display the system menu by right-clicking or by typing ALT+SPACE.
        /// </summary>
        private static readonly long WS_EX_TOOLWINDOW = 0x00000080L;
        /// <summary>
        /// The window should be placed above all non-topmost windows and should stay above them, even when the window is deactivated. To add or remove this style, use the SetWindowPos function.
        /// </summary>
        private static readonly long WS_EX_TOPMOST = 0x00000008L;
        /// <summary>
        /// The window should not be painted until siblings beneath the window (that were created by the same thread) have been painted. The window appears transparent because the bits of underlying sibling windows have already been painted.
        /// To achieve transparency without these restrictions, use the SetWindowRgn function.
        /// </summary>
        private static readonly long WS_EX_TRANSPARENT = 0x00000020L;
        /// <summary>
        /// The window has a border with a raised edge.
        /// </summary>
        private static readonly long WS_EX_WINDOWEDGE = 0x00000100L;

        // ---- SetWindowPos - uFlags
        /// <summary>
        /// If the calling thread and the thread that owns the window are attached to different input queues, the system posts the request to the thread that owns the window. This prevents the calling thread from blocking its execution while other threads process the request.
        /// </summary>
        private static readonly uint SWP_ASYNCWINDOWPOS = 0x4000;
        /// <summary>
        /// Prevents generation of the WM_SYNCPAINT message.
        /// </summary>
        private static readonly uint SWP_DEFERERASE = 0x2000;
        /// <summary>
        /// Draws a frame (defined in the window's class description) around the window.
        /// </summary>
        private static readonly uint SWP_DRAWFRAME = 0x0020;
        /// <summary>
        /// Applies new frame styles set using the SetWindowLong function. Sends a WM_NCCALCSIZE message to the window, even if the window's size is not being changed. If this flag is not specified, WM_NCCALCSIZE is sent only when the window's size is being changed.
        /// </summary>
        private static readonly uint SWP_FRAMECHANGED = 0x0020;
        /// <summary>
        /// Hides the window.
        /// </summary>
        private static readonly uint SWP_HIDEWINDOW = 0x0080;
        /// <summary>
        /// Does not activate the window. If this flag is not set, the window is activated and moved to the top of either the topmost or non-topmost group (depending on the setting of the hWndInsertAfter parameter).
        /// </summary>
        private static readonly uint SWP_NOACTIVATE = 0x0010;
        /// <summary>
        /// Discards the entire contents of the client area. If this flag is not specified, the valid contents of the client area are saved and copied back into the client area after the window is sized or repositioned.
        /// </summary>
        private static readonly uint SWP_NOCOPYBITS = 0x0100;
        /// <summary>
        /// Retains the current position (ignores X and Y parameters).
        /// </summary>
        private static readonly uint SWP_NOMOVE = 0x0002;
        /// <summary>
        /// Does not change the owner window's position in the Z order.
        /// </summary>
        private static readonly uint SWP_NOOWNERZORDER = 0x0200;
        /// <summary>
        /// Does not redraw changes. If this flag is set, no repainting of any kind occurs. This applies to the client area, the nonclient area (including the title bar and scroll bars), and any part of the parent window uncovered as a result of the window being moved. When this flag is set, the application must explicitly invalidate or redraw any parts of the window and parent window that need redrawing.
        /// </summary>
        private static readonly uint SWP_NOREDRAW = 0x0008;
        /// <summary>
        /// Same as the SWP_NOOWNERZORDER flag.
        /// </summary>
        private static readonly uint SWP_NOREPOSITION = 0x0200;
        /// <summary>
        /// Prevents the window from receiving the WM_WINDOWPOSCHANGING message.
        /// </summary>
        private static readonly uint SWP_NOSENDCHANGING = 0x0400;
        /// <summary>
        /// Retains the current size (ignores the cx and cy parameters).
        /// </summary>
        private static readonly uint SWP_NOSIZE = 0x0001;
        /// <summary>
        /// Retains the current Z order (ignores the hWndInsertAfter parameter).
        /// </summary>
        private static readonly uint SWP_NOZORDER = 0x0004;
        /// <summary>
        /// Displays the window.
        /// </summary>
        private static readonly uint SWP_SHOWWINDOW = 0x0040;
        

        private static ICrashable sRoot;
        private static Form sForm;
        private static int sCurrentScreen = 0;
        private static bool sAutoRestart;

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
        private extern static bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        private extern static long GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private extern static bool SetWindowLong(IntPtr hWnd, int nIndex, long dwNewLong);

        public static void SetBorder(Process window, bool enableBorder) {
            Process foreground = Process.GetCurrentProcess();
            long lStyle = GetWindowLong(window.MainWindowHandle, GWL_STYLE);
            if (enableBorder) lStyle |= WS_CAPTION | WS_THICKFRAME | WS_SYSMENU;
            else lStyle &= ~(WS_CAPTION | WS_THICKFRAME | WS_MINIMIZE | WS_MAXIMIZE | WS_SYSMENU);


            long lExStyle = GetWindowLong(window.MainWindowHandle, GWL_EXSTYLE);
            if (enableBorder) lExStyle |= WS_EX_DLGMODALFRAME | WS_EX_CLIENTEDGE;
            else lExStyle &= ~(WS_EX_DLGMODALFRAME | WS_EX_CLIENTEDGE | WS_EX_STATICEDGE);

            SetWindowLong(window.MainWindowHandle, GWL_STYLE, lStyle);
            SetWindowLong(window.MainWindowHandle, GWL_EXSTYLE, lExStyle);
            SetWindowPos(window.MainWindowHandle, IntPtr.Zero, 0, 0, 0, 0, SWP_FRAMECHANGED | SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_NOOWNERZORDER);
            SetForegroundWindow(foreground.MainWindowHandle);
        }

        public static void SetMonitor (Process proc, Screen monitor) {
            SetWindowPos(
                proc.MainWindowHandle, 
                IntPtr.Zero, 
                monitor.Bounds.X, 
                monitor.Bounds.Y, 
                monitor.Bounds.Width, 
                monitor.Bounds.Height, 
                SWP_NOZORDER);
        }

        public static void SwitchWindowMonitor(Process proc) {
            Screen screen = Screen.AllScreens[sCurrentScreen++%Screen.AllScreens.Count()];
            SetWindowPos(
                proc.MainWindowHandle, 
                IntPtr.Zero, 
                screen.Bounds.X, 
                screen.Bounds.Y, 
                screen.Bounds.Width, 
                screen.Bounds.Height, SWP_NOZORDER);
        }

        public static void PressKey(Process app, string key) {
            PressKey(app, key, false, false, false);
        }

        public static void PressKey(Process app, string key, bool ctrl, bool alt, bool shift) {
            if (!app.HasExited) {
                Process foreground = Process.GetCurrentProcess();
                SetForegroundWindow(app.MainWindowHandle);
                PressKey(key, ctrl, alt, shift);
                SetForegroundWindow(foreground.MainWindowHandle);
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
            sAutoRestart = autoRestart;
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            try {
                Application.Run(form);
            } catch (Exception e) {
                Console.WriteLine("Exception caught from GUI thread.");
                HandleException(e);
            }
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
            sRoot.OnCrash(e);
            if (sAutoRestart) {
                Console.WriteLine("Program crashed starting, a new instance.");
                ProcessWrangler.InitProcess(Assembly.GetEntryAssembly().Location).Start();
            }
            throw e;
        }
    }
}

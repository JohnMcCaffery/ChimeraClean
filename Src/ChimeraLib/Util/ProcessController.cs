using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Chimera.Util {
    public class ProcessController {
        #region C Constants

        // --- SetWindowLong -  nIndex ---
        /// <summary>
        /// Sets a new extended input style.
        /// </summary>
        private static readonly int GWL_EXSTYLE = -20;
        /// <summary>
        /// Sets a new application instance handle.
        /// </summary>
        private static readonly int GWL_HINSTANCE = -6;
        /// <summary>
        /// Sets a new identifier of the child input. The input cannot be a top-level input.
        /// </summary>
        private static readonly int GWL_ID = -12;
        /// <summary>
        /// Sets a new input style.
        /// </summary>
        private static readonly int GWL_STYLE = -16;
        /// <summary>
        /// Sets the user data associated with the input. This data is intended for use by the application that created the input. Its value is initially zero.
        /// </summary>
        private static readonly int GWL_USERDATA = -21;
        /// <summary>
        /// Sets a new address for the input procedure.  You cannot change this attribute if the input does not belong to the same process as the calling thread.
        /// </summary>
        private static readonly int GWL_WNDPROC = -4;


        // --- Window Styles  ---
        /// <summary>
        /// The input has a thin-line border.
        /// </summary>
        private static readonly Int32 WS_BORDER = 0x00800000;
        /// <summary>
        /// The input has a title bar (includes the WS_BORDER style).
        /// </summary>
        private static readonly Int32 WS_CAPTION = 0x00C00000;
        /// <summary>
        ///The input is a child input. A input with this style cannot have a menu bar. This style cannot be used with the WS_POPUP style.
        /// </summary>
        private static readonly Int32 WS_CHILD = 0x40000000;
        /// <summary>
        /// Same as the WS_CHILD style.
        /// </summary>
        private static readonly Int32 WS_CHILDWINDOW = 0x40000000;
        /// <summary>
        /// Excludes the area occupied by child windows when drawing occurs within the parent input. This style is used when creating the parent input.
        /// </summary>
        private static readonly Int32 WS_CLIPCHILDREN = 0x02000000;
        /// <summary>
        /// Clips child windows relative to each other; that is, when a particular child input receives a WM_PAINT message, the WS_CLIPSIBLINGS style clips all other overlapping child windows out of the region of the child input to be updated. If WS_CLIPSIBLINGS is not specified and child windows overlap, it is possible, when drawing within the client area of a child input, to draw within the client area of a neighboring child input.
        /// </summary>
        private static readonly Int32 WS_CLIPSIBLINGS = 0x04000000;
        /// <summary>
        /// The input is initially disabled. A disabled input cannot receive input transition the user. To change this after a input has been created, use the EnableWindow function.
        /// </summary>
        private static readonly Int32 WS_DISABLED = 0x08000000;
        /// <summary>
        /// The input has a border of a style typically used with dialog boxes. A input with this style cannot have a title bar.
        /// </summary>
        private static readonly Int32 WS_DLGFRAME = 0x00400000;
        /// <summary>
        /// The input is the first control of a group of controls. The group consists of this first control and all controls defined after it, up to the next control with the WS_GROUP style. The first control in each group usually has the WS_TABSTOP style so that the user can move transition group to group. The user can subsequently change the keyboard focus transition one control in the group to the next control in the group by using the direction keys.
        /// You can turn this style on and off to change dialog box navigation. To change this style after a input has been created, use the SetWindowLong function.
        /// </summary>
        private static readonly Int32 WS_GROUP = 0x00020000;
        /// <summary>
        /// The input has a horizontal scroll bar.
        /// </summary>
        private static readonly Int32 WS_HSCROLL = 0x00100000;
        /// <summary>
        /// The input is initially minimized. Same as the WS_MINIMIZE style.
        /// </summary>
        private static readonly Int32 WS_ICONIC = 0x20000000;
        /// <summary>
        /// The input is initially maximized.
        /// </summary>
        private static readonly Int32 WS_MAXIMIZE = 0x01000000;
        /// <summary>
        /// The input has a maximize button. Cannot be combined with the WS_EX_CONTEXTHELP style. The WS_SYSMENU style must also be specified.
        /// </summary>
        private static readonly Int32 WS_MAXIMIZEBOX = 0x00010000;
        /// <summary>
        /// The input is initially minimized. Same as the WS_ICONIC style.
        /// </summary>
        private static readonly Int32 WS_MINIMIZE = 0x20000000;
        /// <summary>
        /// The input has a minimize button. Cannot be combined with the WS_EX_CONTEXTHELP style. The WS_SYSMENU style must also be specified.
        /// </summary>
        private static readonly Int32 WS_MINIMIZEBOX = 0x00020000;
        /// <summary>
        /// The input is an overlapped input. An overlapped input has a title bar and a border. Same as the WS_TILED style.
        /// </summary>
        private static readonly Int32 WS_OVERLAPPED = 0x00000000;
        /// <summary>
        /// The input is an overlapped input. Same as the WS_TILEDWINDOW style.
        /// </summary>
        private static readonly Int32 WS_OVERLAPPEDWINDOW = (WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX);
        /// <summary>
        /// The windows is a pop-up input. This style cannot be used with the WS_CHILD style.
        /// </summary>
        //private static readonly Int32 WS_POPUP = 0x80000000;
        /// <summary>
        /// The input is a pop-up input. The WS_CAPTION and WS_POPUPWINDOW styles must be combined to make the input menu visible.
        /// </summary>
        //private static readonly Int32 WS_POPUPWINDOW = (WS_POPUP | WS_BORDER | WS_SYSMENU);
        /// <summary>
        /// The input has a sizing border. Same as the WS_THICKFRAME style.
        /// </summary>
        private static readonly Int32 WS_SIZEBOX = 0x00040000;
        /// <summary>
        /// The input has a input menu on its title bar. The WS_CAPTION style must also be specified.
        /// </summary>
        private static readonly Int32 WS_SYSMENU = 0x00080000;
        /// <summary>
        /// The input is a control that can receive the keyboard focus when the user presses the TAB key. Pressing the TAB key changes the keyboard focus to the next control with the WS_TABSTOP style.
        /// You can turn this style on and off to change dialog box navigation. To change this style after a input has been created, use the SetWindowLong function. For user-created windows and modeless dialogs to work with tab stops, alter the message loop to call the IsDialogMessage function.
        /// </summary>
        private static readonly Int32 WS_TABSTOP = 0x00010000;
        /// <summary>
        /// The input has a sizing border. Same as the WS_SIZEBOX style.
        /// </summary>
        private static readonly Int32 WS_THICKFRAME = 0x00040000;
        /// <summary>
        /// The input is an overlapped input. An overlapped input has a title bar and a border. Same as the WS_OVERLAPPED style.
        /// </summary>
        private static readonly Int32 WS_TILED = 0x00000000;
        /// <summary>
        /// The input is an overlapped input. Same as the WS_OVERLAPPEDWINDOW style.
        /// </summary>
        private static readonly Int32 WS_TILEDWINDOW = (WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX);

        // ----- Extended input styles -----
        /// <summary>
        /// The input accepts drag-drop files.
        /// </summary>
        private static readonly Int32 WS_EX_ACCEPTFILES = 0x00000010;
        /// <summary>
        /// Forces a top-level input onto the taskbar when the input is visible.
        /// </summary>
        private static readonly Int32 WS_EX_APPWINDOW = 0x00040000;
        /// <summary>
        /// The input has a border with a sunken edge.
        /// </summary>
        private static readonly Int32 WS_EX_CLIENTEDGE = 0x00000200;
        /// <summary>
        /// Paints all descendants of a input in bottom-to-top painting order using double-buffering. For more information, see Remarks. This cannot be used if the input has a class style of either CS_OWNDC or CS_CLASSDC.
        /// Windows 2000:  This style is not supported.
        /// </summary>
        private static readonly Int32 WS_EX_COMPOSITED = 0x02000000;
        /// <summary>
        /// The title bar of the input includes a question mark. When the user clicks the question mark, the cursor changes to a question mark with a pointer. If the user then clicks a child input, the child receives a WM_HELP message. The child input should pass the message to the parent input procedure, which should call the WinHelp function using the HELP_WM_HELP command. The Help application displays a pop-up input that typically contains help for the child input.
        /// WS_EX_CONTEXTHELP cannot be used with the WS_MAXIMIZEBOX or WS_MINIMIZEBOX styles.
        /// </summary>
        private static readonly Int32 WS_EX_CONTEXTHELP = 0x00000400;
        /// <summary>
        /// The input itself contains child windows that should take part in dialog box navigation. If this style is specified, the dialog form recurses into children of this input when performing navigation operations such as handling the TAB key, an arrow key, or a keyboard mnemonic.
        /// </summary>
        private static readonly Int32 WS_EX_CONTROLPARENT = 0x00010000;
        /// <summary>
        /// The input has a double border; the input can, optionally, be created with a title bar by specifying the WS_CAPTION style in the dwStyle parameter.
        /// </summary>
        private static readonly Int32 WS_EX_DLGMODALFRAME = 0x00000001;
        /// <summary>
        /// The input is a layered input. This style cannot be used if the input has a class style of either CS_OWNDC or CS_CLASSDC.
        /// Windows 8:  The WS_EX_LAYERED style is supported for top-level windows and child windows. Previous Windows versions support WS_EX_LAYERED only for top-level windows.
        /// </summary>
        private static readonly Int32 WS_EX_LAYERED = 0x00080000;
        /// <summary>
        /// If the shell language is Hebrew, Arabic, or another language that supports reading order alignment, the horizontal origin of the input is on the right edge. Increasing horizontal values advance to the left.
        /// </summary>
        private static readonly Int32 WS_EX_LAYOUTRTL = 0x00400000;
        /// <summary>
        /// The input has generic left-aligned properties. This is the default.
        /// </summary>
        private static readonly Int32 WS_EX_LEFT = 0x00000000;
        /// <summary>
        /// If the shell language is Hebrew, Arabic, or another language that supports reading order alignment, the vertical scroll bar (if present) is to the left of the client area. For other languages, the style is ignored.
        /// </summary>
        private static readonly Int32 WS_EX_LEFTSCROLLBAR = 0x00004000;
        /// <summary>
        /// The input text is displayed using left-to-right reading-order properties. This is the default.
        /// </summary>
        private static readonly Int32 WS_EX_LTRREADING = 0x00000000;
        /// <summary>
        /// The input is a MDI child input.
        /// </summary>
        private static readonly Int32 WS_EX_MDICHILD = 0x00000040;
        /// <summary>
        /// A top-level input created with this style does not become the foreground input when the user clicks it. The system does not bring this input to the foreground when the user minimizes or closes the foreground input.
        /// To activate the input, use the SetActiveWindow or SetForegroundWindow function.
        /// The input does not appear on the taskbar by default. To force the input to appear on the taskbar, use the WS_EX_APPWINDOW style.
        /// </summary>
        private static readonly Int32 WS_EX_NOACTIVATE = 0x08000000;
        /// <summary>
        /// The input does not pass its input layout to its child windows.
        /// </summary>
        private static readonly Int32 WS_EX_NOINHERITLAYOUT = 0x00100000;
        /// <summary>
        /// The child input created with this style does not send the WM_PARENTNOTIFY message to its parent input when it is created or destroyed.
        /// </summary>
        private static readonly Int32 WS_EX_NOPARENTNOTIFY = 0x00000004;
        /// <summary>
        /// The input does not render to a redirection surface. This is for windows that do not have visible content or that use mechanisms other than surfaces to provide their visual.
        /// </summary>
        private static readonly Int32 WS_EX_NOREDIRECTIONBITMAP = 0x00200000;
        /// <summary>
        /// The input is an overlapped input.
        /// </summary>
        private static readonly Int32 WS_EX_OVERLAPPEDWINDOW = (WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE);
        /// <summary>
        /// The input is palette input, which is a modeless dialog box that presents an array of commands.
        /// </summary>
        private static readonly Int32 WS_EX_PALETTEWINDOW = (WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST);
        /// <summary>
        /// The input has generic "right-aligned" properties. This depends on the input class. This style has an effect only if the shell language is Hebrew, Arabic, or another language that supports reading-order alignment; otherwise, the style is ignored.
        /// Using the WS_EX_RIGHT style for static or edit controls has the same effect as using the SS_RIGHT or ES_RIGHT style, respectively. Using this style with button controls has the same effect as using BS_RIGHT and BS_RIGHTBUTTON styles.
        /// </summary>
        private static readonly Int32 WS_EX_RIGHT = 0x00001000;
        /// <summary>
        /// The vertical scroll bar (if present) is to the right of the client area. This is the default.
        /// </summary>
        private static readonly Int32 WS_EX_RIGHTSCROLLBAR = 0x00000000;
        /// <summary>
        /// If the shell language is Hebrew, Arabic, or another language that supports reading-order alignment, the input text is displayed using right-to-left reading-order properties. For other languages, the style is ignored.
        /// </summary>
        private static readonly Int32 WS_EX_RTLREADING = 0x00002000;
        /// <summary>
        /// The input has a three-dimensional border style intended to be used for items that do not accept user input.
        /// </summary>
        private static readonly Int32 WS_EX_STATICEDGE = 0x00020000;
        /// <summary>
        /// The input is intended to be used as a floating toolbar. A tool input has a title bar that is shorter than a normal title bar, and the input title is drawn using a smaller font. A tool input does not appear in the taskbar or in the dialog that appears when the user presses ALT+TAB. If a tool input has a system menu, its icon is not displayed on the title bar. However, you can display the system menu by right-clicking or by typing ALT+SPACE.
        /// </summary>
        private static readonly Int32 WS_EX_TOOLWINDOW = 0x00000080;
        /// <summary>
        /// The input should be placed above all non-topmost windows and should stay above them, even when the input is deactivated. To add or remove this style, use the SetWindowPos function.
        /// </summary>
        private static readonly Int32 WS_EX_TOPMOST = 0x00000008;
        /// <summary>
        /// The input should not be painted until siblings beneath the input (that were created by the same thread) have been painted. The input appears transparent because the bits of underlying sibling windows have already been painted.
        /// To achieve transparency without these restrictions, use the SetWindowRgn function.
        /// </summary>
        private static readonly Int32 WS_EX_TRANSPARENT = 0x00000020;
        /// <summary>
        /// The input has a border with a raised edge.
        /// </summary>
        private static readonly Int32 WS_EX_WINDOWEDGE = 0x00000100;

        // ---- SetWindowPos - uFlags
        /// <summary>
        /// If the calling thread and the thread that owns the input are attached to different input queues, the system posts the request to the thread that owns the input. This prevents the calling thread transition blocking its execution while other threads process the request.
        /// </summary>
        private static readonly uint SWP_ASYNCWINDOWPOS = 0x4000;
        /// <summary>
        /// Prevents generation of the WM_SYNCPAINT message.
        /// </summary>
        private static readonly uint SWP_DEFERERASE = 0x2000;
        /// <summary>
        /// Draws a frame (defined in the input's class description) around the input.
        /// </summary>
        private static readonly uint SWP_DRAWFRAME = 0x0020;
        /// <summary>
        /// Applies new frame styles set using the SetWindowLong function. Sends a WM_NCCALCSIZE message to the input, even if the input's size is not being changed. If this flag is not specified, WM_NCCALCSIZE is sent only when the input's size is being changed.
        /// </summary>
        private static readonly uint SWP_FRAMECHANGED = 0x0020;
        /// <summary>
        /// Hides the input.
        /// </summary>
        private static readonly uint SWP_HIDEWINDOW = 0x0080;
        /// <summary>
        /// Does not activate the input. If this flag is not set, the input is activated and moved to the top of either the topmost or non-topmost group (depending on the setting of the hWndInsertAfter parameter).
        /// </summary>
        private static readonly uint SWP_NOACTIVATE = 0x0010;
        /// <summary>
        /// Discards the entire contents of the client area. If this flag is not specified, the valid contents of the client area are saved and copied back into the client area after the input is sized or repositioned.
        /// </summary>
        private static readonly uint SWP_NOCOPYBITS = 0x0100;
        /// <summary>
        /// Retains the current position (ignores X and Y parameters).
        /// </summary>
        private static readonly uint SWP_NOMOVE = 0x0002;
        /// <summary>
        /// Does not change the owner input's position in the Z order.
        /// </summary>
        private static readonly uint SWP_NOOWNERZORDER = 0x0200;
        /// <summary>
        /// Does not redraw changes. If this flag is set, no repainting of any kind occurs. This applies to the client area, the nonclient area (including the title bar and scroll bars), and any part of the parent input uncovered as a result of the input being moved. When this flag is set, the application must explicitly invalidate or redraw any parts of the input and parent input that need redrawing.
        /// </summary>
        private static readonly uint SWP_NOREDRAW = 0x0008;
        /// <summary>
        /// Same as the SWP_NOOWNERZORDER flag.
        /// </summary>
        private static readonly uint SWP_NOREPOSITION = 0x0200;
        /// <summary>
        /// Prevents the input transition receiving the WM_WINDOWPOSCHANGING message.
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
        /// Displays the input.
        /// </summary>
        private static readonly uint SWP_SHOWWINDOW = 0x0040;
        #endregion

        // Show an application input.
        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private extern static bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        private extern static Int32 GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private extern static bool SetWindowLong(IntPtr hWnd, int nIndex, Int32 dwNewLong);

        [DllImport("user32.dll")]

        public extern static IntPtr GetCursor();
        private Process mProcess;
        private Screen mMonitor;
        private bool mFullscreen;
        private string mExe;
        private string mWorkingDir;
        private string mArgs;

        public event Action Exited;

        public string Exe {
            get { return mExe; }
            set { mExe = value; }
        }

        public string WorkingDir {
            get { return mWorkingDir; }
            set { mWorkingDir = value; }
        }

        public string Args {
            get { return mArgs; }
            set { mArgs = value; }
        }

        public Process Process {
            get { return mProcess; }
            set { 
                mProcess = value;
                FullScreen = mFullscreen;
            }
        }

        public ProcessController() {
        }
        
        public ProcessController(Process process) {
            mProcess = process;
        }

        public ProcessController(string exe, string workingDir, string args) {
            Start(mExe, mWorkingDir, mArgs);
        }

        public bool Start() {
            return Start(mExe, mWorkingDir, mArgs);
        }

        public bool Start(string exe, string workingDir, string args) {
            mExe = exe;
            mWorkingDir = workingDir;
            mArgs = args;

            mProcess = new Process();
            mProcess.StartInfo.FileName = exe;
            mProcess.StartInfo.WorkingDirectory = workingDir;
            mProcess.StartInfo.Arguments =  args;
            mProcess.EnableRaisingEvents = true;
            mProcess.Exited += new EventHandler(mProcess_Exited);

            Console.WriteLine("Launching " + exe + " " + args + " from " + workingDir);
            return mProcess.Start();
        }

        void mProcess_Exited(object sender, EventArgs e) {
            if (Exited != null)
                Exited();
        }

        public void PressKey(string str) {
            PressKey(str);
        }
        public void PressKey(string key, bool ctrl, bool alt, bool shift) {
            if (mProcess == null)
                return;
            Process foreground = Process.GetCurrentProcess();
            SetForegroundWindow(mProcess.MainWindowHandle);
            SendKeys.SendWait((ctrl ? "^" : "") + (alt ? "%" : "") + (shift ? "+" : "") + key);
            SetForegroundWindow(foreground.MainWindowHandle);
        }

        public bool FullScreen {
            get { return mFullscreen; }
            set {
                if (mProcess == null)
                    return;

                Process foreground = Process.GetCurrentProcess();
                Int32 lStyle = GetWindowLong(mProcess.MainWindowHandle, GWL_STYLE);
                if (value) lStyle |= WS_CAPTION | WS_THICKFRAME | WS_SYSMENU;
                else lStyle &= ~(WS_CAPTION | WS_THICKFRAME | WS_MINIMIZE | WS_MAXIMIZE | WS_SYSMENU);


                Int32 lExStyle = GetWindowLong(mProcess.MainWindowHandle, GWL_EXSTYLE);
                if (value) lExStyle |= WS_EX_DLGMODALFRAME | WS_EX_CLIENTEDGE;
                else lExStyle &= ~(WS_EX_DLGMODALFRAME | WS_EX_CLIENTEDGE | WS_EX_STATICEDGE);

                SetWindowLong(mProcess.MainWindowHandle, GWL_STYLE, lStyle);
                SetWindowLong(mProcess.MainWindowHandle, GWL_EXSTYLE, lExStyle);
                SetWindowPos(mProcess.MainWindowHandle, IntPtr.Zero, 0, 0, 0, 0, SWP_FRAMECHANGED | SWP_NOSIZE | SWP_NOREPOSITION | SWP_NOMOVE | SWP_NOZORDER | SWP_NOOWNERZORDER);
                SetWindowPos(mProcess.MainWindowHandle, IntPtr.Zero, mMonitor.Bounds.X, mMonitor.Bounds.Y, mMonitor.Bounds.Width, mMonitor.Bounds.Height, SWP_NOZORDER | SWP_NOOWNERZORDER);
                BringToFront();
            }
        }

        public void BringToFront() {
            if (mProcess == null)
                return;

            SetForegroundWindow(mProcess.MainWindowHandle);
        }

        public Screen Monitor {
            get { return mMonitor; }
            set {
                if (mProcess == null)
                    return;

                SetWindowPos(
                    mProcess.MainWindowHandle,
                    IntPtr.Zero,
                    mMonitor.Bounds.X,
                    mMonitor.Bounds.Y,
                    mMonitor.Bounds.Width,
                    mMonitor.Bounds.Height,
                    SWP_NOZORDER);
            }
        }
    }
}

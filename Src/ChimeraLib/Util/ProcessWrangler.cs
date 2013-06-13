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
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using Chimera.Interfaces;
using Chimera.Config;
using log4net;

namespace Chimera.Util {
    public static class ProcessWrangler {
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
        /// Frames 2000:  This style is not supported.
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
        /// Frames 8:  The WS_EX_LAYERED style is supported for top-level windows and child windows. Previous Frames versions support WS_EX_LAYERED only for top-level windows.
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



        [Flags]
        internal enum KEYEVENTF : uint {
            EXTENDEDKEY = 0x0001,
            KEYUP = 0x0002,
            SCANCODE = 0x0008,
            UNICODE = 0x0004
        }

        internal enum VirtualKeyShort : short {
            ///<summary>
            ///Left mouse button
            ///</summary>
            LBUTTON = 0x01,
            ///<summary>
            ///Right mouse button
            ///</summary>
            RBUTTON = 0x02,
            ///<summary>
            ///Control-break processing
            ///</summary>
            CANCEL = 0x03,
            ///<summary>
            ///Middle mouse button (three-button mouse)
            ///</summary>
            MBUTTON = 0x04,
            ///<summary>
            ///Frames 2000/XP: X1 mouse button
            ///</summary>
            XBUTTON1 = 0x05,
            ///<summary>
            ///Frames 2000/XP: X2 mouse button
            ///</summary>
            XBUTTON2 = 0x06,
            ///<summary>
            ///BACKSPACE key
            ///</summary>
            BACK = 0x08,
            ///<summary>
            ///TAB key
            ///</summary>
            TAB = 0x09,
            ///<summary>
            ///CLEAR key
            ///</summary>
            CLEAR = 0x0C,
            ///<summary>
            ///ENTER key
            ///</summary>
            RETURN = 0x0D,
            ///<summary>
            ///SHIFT key
            ///</summary>
            SHIFT = 0x10,
            ///<summary>
            ///CTRL key
            ///</summary>
            CONTROL = 0x11,
            ///<summary>
            ///ALT key
            ///</summary>
            MENU = 0x12,
            ///<summary>
            ///PAUSE key
            ///</summary>
            PAUSE = 0x13,
            ///<summary>
            ///CAPS LOCK key
            ///</summary>
            CAPITAL = 0x14,
            ///<summary>
            ///Input ForceRedrawStatic Editor (IME) Kana mode
            ///</summary>
            KANA = 0x15,
            ///<summary>
            ///IME Hangul mode
            ///</summary>
            HANGUL = 0x15,
            ///<summary>
            ///IME Junja mode
            ///</summary>
            JUNJA = 0x17,
            ///<summary>
            ///IME final mode
            ///</summary>
            FINAL = 0x18,
            ///<summary>
            ///IME Hanja mode
            ///</summary>
            HANJA = 0x19,
            ///<summary>
            ///IME Kanji mode
            ///</summary>
            KANJI = 0x19,
            ///<summary>
            ///ESC key
            ///</summary>
            ESCAPE = 0x1B,
            ///<summary>
            ///IME convert
            ///</summary>
            CONVERT = 0x1C,
            ///<summary>
            ///IME nonconvert
            ///</summary>
            NONCONVERT = 0x1D,
            ///<summary>
            ///IME accept
            ///</summary>
            ACCEPT = 0x1E,
            ///<summary>
            ///IME mode change request
            ///</summary>
            MODECHANGE = 0x1F,
            ///<summary>
            ///SPACEBAR
            ///</summary>
            SPACE = 0x20,
            ///<summary>
            ///PAGE UP key
            ///</summary>
            PRIOR = 0x21,
            ///<summary>
            ///PAGE DOWN key
            ///</summary>
            NEXT = 0x22,
            ///<summary>
            ///END key
            ///</summary>
            END = 0x23,
            ///<summary>
            ///HOME key
            ///</summary>
            HOME = 0x24,
            ///<summary>
            ///LEFT ARROW key
            ///</summary>
            LEFT = 0x25,
            ///<summary>
            ///UP ARROW key
            ///</summary>
            UP = 0x26,
            ///<summary>
            ///RIGHT ARROW key
            ///</summary>
            RIGHT = 0x27,
            ///<summary>
            ///DOWN ARROW key
            ///</summary>
            DOWN = 0x28,
            ///<summary>
            ///SELECT key
            ///</summary>
            SELECT = 0x29,
            ///<summary>
            ///PRINT key
            ///</summary>
            PRINT = 0x2A,
            ///<summary>
            ///EXECUTE key
            ///</summary>
            EXECUTE = 0x2B,
            ///<summary>
            ///PRINT SCREEN key
            ///</summary>
            SNAPSHOT = 0x2C,
            ///<summary>
            ///INS key
            ///</summary>
            INSERT = 0x2D,
            ///<summary>
            ///DEL key
            ///</summary>
            DELETE = 0x2E,
            ///<summary>
            ///HELP key
            ///</summary>
            HELP = 0x2F,
            ///<summary>
            ///0 key
            ///</summary>
            KEY_0 = 0x30,
            ///<summary>
            ///1 key
            ///</summary>
            KEY_1 = 0x31,
            ///<summary>
            ///2 key
            ///</summary>
            KEY_2 = 0x32,
            ///<summary>
            ///3 key
            ///</summary>
            KEY_3 = 0x33,
            ///<summary>
            ///4 key
            ///</summary>
            KEY_4 = 0x34,
            ///<summary>
            ///5 key
            ///</summary>
            KEY_5 = 0x35,
            ///<summary>
            ///6 key
            ///</summary>
            KEY_6 = 0x36,
            ///<summary>
            ///7 key
            ///</summary>
            KEY_7 = 0x37,
            ///<summary>
            ///8 key
            ///</summary>
            KEY_8 = 0x38,
            ///<summary>
            ///9 key
            ///</summary>
            KEY_9 = 0x39,
            ///<summary>
            ///A key
            ///</summary>
            KEY_A = 0x41,
            ///<summary>
            ///B key
            ///</summary>
            KEY_B = 0x42,
            ///<summary>
            ///C key
            ///</summary>
            KEY_C = 0x43,
            ///<summary>
            ///D key
            ///</summary>
            KEY_D = 0x44,
            ///<summary>
            ///E key
            ///</summary>
            KEY_E = 0x45,
            ///<summary>
            ///F key
            ///</summary>
            KEY_F = 0x46,
            ///<summary>
            ///G key
            ///</summary>
            KEY_G = 0x47,
            ///<summary>
            ///H key
            ///</summary>
            KEY_H = 0x48,
            ///<summary>
            ///I key
            ///</summary>
            KEY_I = 0x49,
            ///<summary>
            ///J key
            ///</summary>
            KEY_J = 0x4A,
            ///<summary>
            ///K key
            ///</summary>
            KEY_K = 0x4B,
            ///<summary>
            ///L key
            ///</summary>
            KEY_L = 0x4C,
            ///<summary>
            ///M key
            ///</summary>
            KEY_M = 0x4D,
            ///<summary>
            ///N key
            ///</summary>
            KEY_N = 0x4E,
            ///<summary>
            ///O key
            ///</summary>
            KEY_O = 0x4F,
            ///<summary>
            ///P key
            ///</summary>
            KEY_P = 0x50,
            ///<summary>
            ///Q key
            ///</summary>
            KEY_Q = 0x51,
            ///<summary>
            ///R key
            ///</summary>
            KEY_R = 0x52,
            ///<summary>
            ///S key
            ///</summary>
            KEY_S = 0x53,
            ///<summary>
            ///T key
            ///</summary>
            KEY_T = 0x54,
            ///<summary>
            ///U key
            ///</summary>
            KEY_U = 0x55,
            ///<summary>
            ///V key
            ///</summary>
            KEY_V = 0x56,
            ///<summary>
            ///W key
            ///</summary>
            KEY_W = 0x57,
            ///<summary>
            ///X key
            ///</summary>
            KEY_X = 0x58,
            ///<summary>
            ///Y key
            ///</summary>
            KEY_Y = 0x59,
            ///<summary>
            ///Z key
            ///</summary>
            KEY_Z = 0x5A,
            ///<summary>
            ///Left Frames key (Microsoft Natural keyboard) 
            ///</summary>
            LWIN = 0x5B,
            ///<summary>
            ///Right Frames key (Natural keyboard)
            ///</summary>
            RWIN = 0x5C,
            ///<summary>
            ///Applications key (Natural keyboard)
            ///</summary>
            APPS = 0x5D,
            ///<summary>
            ///Computer Sleep key
            ///</summary>
            SLEEP = 0x5F,
            ///<summary>
            ///Numeric keypad 0 key
            ///</summary>
            NUMPAD0 = 0x60,
            ///<summary>
            ///Numeric keypad 1 key
            ///</summary>
            NUMPAD1 = 0x61,
            ///<summary>
            ///Numeric keypad 2 key
            ///</summary>
            NUMPAD2 = 0x62,
            ///<summary>
            ///Numeric keypad 3 key
            ///</summary>
            NUMPAD3 = 0x63,
            ///<summary>
            ///Numeric keypad 4 key
            ///</summary>
            NUMPAD4 = 0x64,
            ///<summary>
            ///Numeric keypad 5 key
            ///</summary>
            NUMPAD5 = 0x65,
            ///<summary>
            ///Numeric keypad 6 key
            ///</summary>
            NUMPAD6 = 0x66,
            ///<summary>
            ///Numeric keypad 7 key
            ///</summary>
            NUMPAD7 = 0x67,
            ///<summary>
            ///Numeric keypad 8 key
            ///</summary>
            NUMPAD8 = 0x68,
            ///<summary>
            ///Numeric keypad 9 key
            ///</summary>
            NUMPAD9 = 0x69,
            ///<summary>
            ///Multiply key
            ///</summary>
            MULTIPLY = 0x6A,
            ///<summary>
            ///Add key
            ///</summary>
            ADD = 0x6B,
            ///<summary>
            ///Separator key
            ///</summary>
            SEPARATOR = 0x6C,
            ///<summary>
            ///Subtract key
            ///</summary>
            SUBTRACT = 0x6D,
            ///<summary>
            ///Decimal key
            ///</summary>
            DECIMAL = 0x6E,
            ///<summary>
            ///Divide key
            ///</summary>
            DIVIDE = 0x6F,
            ///<summary>
            ///F1 key
            ///</summary>
            F1 = 0x70,
            ///<summary>
            ///F2 key
            ///</summary>
            F2 = 0x71,
            ///<summary>
            ///F3 key
            ///</summary>
            F3 = 0x72,
            ///<summary>
            ///F4 key
            ///</summary>
            F4 = 0x73,
            ///<summary>
            ///F5 key
            ///</summary>
            F5 = 0x74,
            ///<summary>
            ///F6 key
            ///</summary>
            F6 = 0x75,
            ///<summary>
            ///F7 key
            ///</summary>
            F7 = 0x76,
            ///<summary>
            ///F8 key
            ///</summary>
            F8 = 0x77,
            ///<summary>
            ///F9 key
            ///</summary>
            F9 = 0x78,
            ///<summary>
            ///F10 key
            ///</summary>
            F10 = 0x79,
            ///<summary>
            ///F11 key
            ///</summary>
            F11 = 0x7A,
            ///<summary>
            ///F12 key
            ///</summary>
            F12 = 0x7B,
            ///<summary>
            ///F13 key
            ///</summary>
            F13 = 0x7C,
            ///<summary>
            ///F14 key
            ///</summary>
            F14 = 0x7D,
            ///<summary>
            ///F15 key
            ///</summary>
            F15 = 0x7E,
            ///<summary>
            ///F16 key
            ///</summary>
            F16 = 0x7F,
            ///<summary>
            ///F17 key  
            ///</summary>
            F17 = 0x80,
            ///<summary>
            ///F18 key  
            ///</summary>
            F18 = 0x81,
            ///<summary>
            ///F19 key  
            ///</summary>
            F19 = 0x82,
            ///<summary>
            ///F20 key  
            ///</summary>
            F20 = 0x83,
            ///<summary>
            ///F21 key  
            ///</summary>
            F21 = 0x84,
            ///<summary>
            ///F22 key, (PPC only) Key used to lock device.
            ///</summary>
            F22 = 0x85,
            ///<summary>
            ///F23 key  
            ///</summary>
            F23 = 0x86,
            ///<summary>
            ///F24 key  
            ///</summary>
            F24 = 0x87,
            ///<summary>
            ///NUM LOCK key
            ///</summary>
            NUMLOCK = 0x90,
            ///<summary>
            ///SCROLL LOCK key
            ///</summary>
            SCROLL = 0x91,
            ///<summary>
            ///Left SHIFT key
            ///</summary>
            LSHIFT = 0xA0,
            ///<summary>
            ///Right SHIFT key
            ///</summary>
            RSHIFT = 0xA1,
            ///<summary>
            ///Left CONTROL key
            ///</summary>
            LCONTROL = 0xA2,
            ///<summary>
            ///Right CONTROL key
            ///</summary>
            RCONTROL = 0xA3,
            ///<summary>
            ///Left MENU key
            ///</summary>
            LMENU = 0xA4,
            ///<summary>
            ///Right MENU key
            ///</summary>
            RMENU = 0xA5,
            ///<summary>
            ///Frames 2000/XP: Browser Back key
            ///</summary>
            BROWSER_BACK = 0xA6,
            ///<summary>
            ///Frames 2000/XP: Browser Forward key
            ///</summary>
            BROWSER_FORWARD = 0xA7,
            ///<summary>
            ///Frames 2000/XP: Browser Refresh key
            ///</summary>
            BROWSER_REFRESH = 0xA8,
            ///<summary>
            ///Frames 2000/XP: Browser Stop key
            ///</summary>
            BROWSER_STOP = 0xA9,
            ///<summary>
            ///Frames 2000/XP: Browser Search key 
            ///</summary>
            BROWSER_SEARCH = 0xAA,
            ///<summary>
            ///Frames 2000/XP: Browser Favorites key
            ///</summary>
            BROWSER_FAVORITES = 0xAB,
            ///<summary>
            ///Frames 2000/XP: Browser Begin and Home key
            ///</summary>
            BROWSER_HOME = 0xAC,
            ///<summary>
            ///Frames 2000/XP: Volume Mute key
            ///</summary>
            VOLUME_MUTE = 0xAD,
            ///<summary>
            ///Frames 2000/XP: Volume Down key
            ///</summary>
            VOLUME_DOWN = 0xAE,
            ///<summary>
            ///Frames 2000/XP: Volume Up key
            ///</summary>
            VOLUME_UP = 0xAF,
            ///<summary>
            ///Frames 2000/XP: Next Track key
            ///</summary>
            MEDIA_NEXT_TRACK = 0xB0,
            ///<summary>
            ///Frames 2000/XP: Previous Track key
            ///</summary>
            MEDIA_PREV_TRACK = 0xB1,
            ///<summary>
            ///Frames 2000/XP: Stop Media key
            ///</summary>
            MEDIA_STOP = 0xB2,
            ///<summary>
            ///Frames 2000/XP: Play/Pause Media key
            ///</summary>
            MEDIA_PLAY_PAUSE = 0xB3,
            ///<summary>
            ///Frames 2000/XP: Begin Mail key
            ///</summary>
            LAUNCH_MAIL = 0xB4,
            ///<summary>
            ///Frames 2000/XP: Select Media key
            ///</summary>
            LAUNCH_MEDIA_SELECT = 0xB5,
            ///<summary>
            ///Frames 2000/XP: Begin Application 1 key
            ///</summary>
            LAUNCH_APP1 = 0xB6,
            ///<summary>
            ///Frames 2000/XP: Begin Application 2 key
            ///</summary>
            LAUNCH_APP2 = 0xB7,
            ///<summary>
            ///Used for miscellaneous characters; it can vary by keyboard.
            ///</summary>
            OEM_1 = 0xBA,
            ///<summary>
            ///Frames 2000/XP: For any country/region, the '+' key
            ///</summary>
            OEM_PLUS = 0xBB,
            ///<summary>
            ///Frames 2000/XP: For any country/region, the ',' key
            ///</summary>
            OEM_COMMA = 0xBC,
            ///<summary>
            ///Frames 2000/XP: For any country/region, the '-' key
            ///</summary>
            OEM_MINUS = 0xBD,
            ///<summary>
            ///Frames 2000/XP: For any country/region, the '.' key
            ///</summary>
            OEM_PERIOD = 0xBE,
            ///<summary>
            ///Used for miscellaneous characters; it can vary by keyboard.
            ///</summary>
            OEM_2 = 0xBF,
            ///<summary>
            ///Used for miscellaneous characters; it can vary by keyboard. 
            ///</summary>
            OEM_3 = 0xC0,
            ///<summary>
            ///Used for miscellaneous characters; it can vary by keyboard. 
            ///</summary>
            OEM_4 = 0xDB,
            ///<summary>
            ///Used for miscellaneous characters; it can vary by keyboard. 
            ///</summary>
            OEM_5 = 0xDC,
            ///<summary>
            ///Used for miscellaneous characters; it can vary by keyboard. 
            ///</summary>
            OEM_6 = 0xDD,
            ///<summary>
            ///Used for miscellaneous characters; it can vary by keyboard. 
            ///</summary>
            OEM_7 = 0xDE,
            ///<summary>
            ///Used for miscellaneous characters; it can vary by keyboard.
            ///</summary>
            OEM_8 = 0xDF,
            ///<summary>
            ///Frames 2000/XP: Either the angle bracket key or the backslash key on the RT 102-key keyboard
            ///</summary>
            OEM_102 = 0xE2,
            ///<summary>
            ///Frames 95/98/Me, Frames NT 4.0, Frames 2000/XP: IME PROCESS key
            ///</summary>
            PROCESSKEY = 0xE5,
            ///<summary>
            ///Frames 2000/XP: Used to pass Unicode characters as if they were keystrokes.
            ///The VK_PACKET key is the low word of a 32-bit Virtual Key value used for non-keyboard input methods. For more information,
            ///see Remark in KEYBDINPUT, SendInput, WM_KEYDOWN, and WM_KEYUP
            ///</summary>
            PACKET = 0xE7,
            ///<summary>
            ///Attn key
            ///</summary>
            ATTN = 0xF6,
            ///<summary>
            ///CrSel key
            ///</summary>
            CRSEL = 0xF7,
            ///<summary>
            ///ExSel key
            ///</summary>
            EXSEL = 0xF8,
            ///<summary>
            ///Erase EOF key
            ///</summary>
            EREOF = 0xF9,
            ///<summary>
            ///Play key
            ///</summary>
            PLAY = 0xFA,
            ///<summary>
            ///Zoom key
            ///</summary>
            ZOOM = 0xFB,
            ///<summary>
            ///Reserved 
            ///</summary>
            NONAME = 0xFC,
            ///<summary>
            ///PA1 key
            ///</summary>
            PA1 = 0xFD,
            ///<summary>
            ///Clear key
            ///</summary>
            OEM_CLEAR = 0xFE
        }
        internal enum ScanCodeShort : short {
            LBUTTON = 0,
            RBUTTON = 0,
            CANCEL = 70,
            MBUTTON = 0,
            XBUTTON1 = 0,
            XBUTTON2 = 0,
            BACK = 14,
            TAB = 15,
            CLEAR = 76,
            RETURN = 28,
            SHIFT = 42,
            CONTROL = 29,
            MENU = 56,
            PAUSE = 0,
            CAPITAL = 58,
            KANA = 0,
            HANGUL = 0,
            JUNJA = 0,
            FINAL = 0,
            HANJA = 0,
            KANJI = 0,
            ESCAPE = 1,
            CONVERT = 0,
            NONCONVERT = 0,
            ACCEPT = 0,
            MODECHANGE = 0,
            SPACE = 57,
            PRIOR = 73,
            NEXT = 81,
            END = 79,
            HOME = 71,
            LEFT = 75,
            UP = 72,
            RIGHT = 77,
            DOWN = 80,
            SELECT = 0,
            PRINT = 0,
            EXECUTE = 0,
            SNAPSHOT = 84,
            INSERT = 82,
            DELETE = 83,
            HELP = 99,
            KEY_0 = 11,
            KEY_1 = 2,
            KEY_2 = 3,
            KEY_3 = 4,
            KEY_4 = 5,
            KEY_5 = 6,
            KEY_6 = 7,
            KEY_7 = 8,
            KEY_8 = 9,
            KEY_9 = 10,
            KEY_A = 30,
            KEY_B = 48,
            KEY_C = 46,
            KEY_D = 32,
            KEY_E = 18,
            KEY_F = 33,
            KEY_G = 34,
            KEY_H = 35,
            KEY_I = 23,
            KEY_J = 36,
            KEY_K = 37,
            KEY_L = 38,
            KEY_M = 50,
            KEY_N = 49,
            KEY_O = 24,
            KEY_P = 25,
            KEY_Q = 16,
            KEY_R = 19,
            KEY_S = 31,
            KEY_T = 20,
            KEY_U = 22,
            KEY_V = 47,
            KEY_W = 17,
            KEY_X = 45,
            KEY_Y = 21,
            KEY_Z = 44,
            LWIN = 91,
            RWIN = 92,
            APPS = 93,
            SLEEP = 95,
            NUMPAD0 = 82,
            NUMPAD1 = 79,
            NUMPAD2 = 80,
            NUMPAD3 = 81,
            NUMPAD4 = 75,
            NUMPAD5 = 76,
            NUMPAD6 = 77,
            NUMPAD7 = 71,
            NUMPAD8 = 72,
            NUMPAD9 = 73,
            MULTIPLY = 55,
            ADD = 78,
            SEPARATOR = 0,
            SUBTRACT = 74,
            DECIMAL = 83,
            DIVIDE = 53,
            F1 = 59,
            F2 = 60,
            F3 = 61,
            F4 = 62,
            F5 = 63,
            F6 = 64,
            F7 = 65,
            F8 = 66,
            F9 = 67,
            F10 = 68,
            F11 = 87,
            F12 = 88,
            F13 = 100,
            F14 = 101,
            F15 = 102,
            F16 = 103,
            F17 = 104,
            F18 = 105,
            F19 = 106,
            F20 = 107,
            F21 = 108,
            F22 = 109,
            F23 = 110,
            F24 = 118,
            NUMLOCK = 69,
            SCROLL = 70,
            LSHIFT = 42,
            RSHIFT = 54,
            LCONTROL = 29,
            RCONTROL = 29,
            LMENU = 56,
            RMENU = 56,
            BROWSER_BACK = 106,
            BROWSER_FORWARD = 105,
            BROWSER_REFRESH = 103,
            BROWSER_STOP = 104,
            BROWSER_SEARCH = 101,
            BROWSER_FAVORITES = 102,
            BROWSER_HOME = 50,
            VOLUME_MUTE = 32,
            VOLUME_DOWN = 46,
            VOLUME_UP = 48,
            MEDIA_NEXT_TRACK = 25,
            MEDIA_PREV_TRACK = 16,
            MEDIA_STOP = 36,
            MEDIA_PLAY_PAUSE = 34,
            LAUNCH_MAIL = 108,
            LAUNCH_MEDIA_SELECT = 109,
            LAUNCH_APP1 = 107,
            LAUNCH_APP2 = 33,
            OEM_1 = 39,
            OEM_PLUS = 13,
            OEM_COMMA = 51,
            OEM_MINUS = 12,
            OEM_PERIOD = 52,
            OEM_2 = 53,
            OEM_3 = 41,
            OEM_4 = 26,
            OEM_5 = 43,
            OEM_6 = 27,
            OEM_7 = 40,
            OEM_8 = 0,
            OEM_102 = 86,
            PROCESSKEY = 0,
            PACKET = 0,
            ATTN = 0,
            CRSEL = 0,
            EXSEL = 0,
            EREOF = 93,
            PLAY = 0,
            ZOOM = 98,
            NONAME = 0,
            PA1 = 0,
            OEM_CLEAR = 0,
        }
        

        private static ICrashable sRoot;
        private static Form sForm;
        private static int sCurrentScreen = 0;

        //Send a keyboard event
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMEssage(int hWnd, int Msg, int wParam, int lParam);

        //CustomTrigger keyboard event
        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

        // Get a handle to an application input.
        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

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

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT {
            public Int32 x;
            public Int32 y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct CURSORINFO {
            public Int32 cbSize;        // Specifies the size, in bytes, of the structure. 
            // The caller must set this to Marshal.SizeOf(typeof(CURSORINFO)).
            public Int32 flags;         // Specifies the cursor state. This parameter can be one of the following values:
            //    0             The cursor is hidden.
            //    CURSOR_SHOWING    The cursor is showing.
            public IntPtr hCursor;          // Handle to the cursor. 
            public POINT ptScreenPos;       // A POINT structure that receives the screen coordinates of the cursor. 
        }

        [DllImport("user32.dll")]
        private static extern bool GetCursorInfo(out CURSORINFO pci);

        [StructLayout(LayoutKind.Sequential)]
        public struct INPUT {
            internal uint type;
            internal InputUnion U;
            internal static int Size {
                get { return Marshal.SizeOf(typeof(INPUT)); }
            }
        }


        ///<summary>
        /// The event is a mouse event. Use the mi structure of the union.
        ///</summary>
        static uint INPUT_MOUSE = 0;
        ///<summary>
        /// The event is a keyboard event. Use the ki structure of the union.
        ///</summary>
        static uint INPUT_KEYBOARD = 1;
        ///<summary>
        /// The event is a hardware event. Use the hi structure of the union.
        ///</summary>
        static uint INPUT_HARDWARE = 2;

        [Flags]
        internal enum MouseEventDataXButtons : uint {
            Nothing = 0x00000000,
            XBUTTON1 = 0x00000001,
            XBUTTON2 = 0x00000002
        }

        [Flags]
        internal enum MOUSEEVENTF : uint {
            ABSOLUTE = 0x8000,
            HWHEEL = 0x01000,
            MOVE = 0x0001,
            MOVE_NOCOALESCE = 0x2000,
            LEFTDOWN = 0x0002,
            LEFTUP = 0x0004,
            RIGHTDOWN = 0x0008,
            RIGHTUP = 0x0010,
            MIDDLEDOWN = 0x0020,
            MIDDLEUP = 0x0040,
            VIRTUALDESK = 0x4000,
            WHEEL = 0x0800,
            XDOWN = 0x0080,
            XUP = 0x0100
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct HARDWAREINPUT {
            internal int uMsg;
            internal short wParamL;
            internal short wParamH;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct KEYBDINPUT {
            internal VirtualKeyShort wVk;
            internal ScanCodeShort wScan;
            internal KEYEVENTF dwFlags;
            internal int time;
            internal UIntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct MOUSEINPUT {
            internal int dx;
            internal int dy;
            internal MouseEventDataXButtons mouseData;
            internal MOUSEEVENTF dwFlags;
            internal uint time;
            internal UIntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct InputUnion {
            [FieldOffset(0)]
            internal MOUSEINPUT mi;
            [FieldOffset(0)]
            internal KEYBDINPUT ki;
            [FieldOffset(0)]
            internal HARDWAREINPUT hi;
        }
        
        /// <summary>
        /// Synthesizes keystrokes, mouse motions, and button clicks.
        /// </summary>
        [DllImport("user32.dll")]
        internal static extern uint SendInput( uint nInputs, [MarshalAs(UnmanagedType.LPArray), In] INPUT[] pInputs, int cbSize);

        [DllImport("user32.dll")]
        private static extern IntPtr SetCursor(IntPtr cursor);

        private const Int32 CURSOR_SHOWING = 0x00000001;

        public static void Click() {
            INPUT mouseInput = new INPUT();
            mouseInput.type = INPUT_MOUSE;
            mouseInput.U.mi.dx = 0;
            mouseInput.U.mi.dy = 0;
            mouseInput.U.mi.mouseData = MouseEventDataXButtons.Nothing;

            mouseInput.U.mi.dwFlags = MOUSEEVENTF.LEFTDOWN;
            SendInput(1, new INPUT[] { mouseInput }, INPUT.Size);

            mouseInput.U.mi.dwFlags = MOUSEEVENTF.LEFTUP;
            SendInput(1, new INPUT[] { mouseInput }, INPUT.Size);
        }

        public static void Click(Process p) {
        }

        public static IntPtr GetGlobalCursor() {
            CURSORINFO pci;
            pci.cbSize = Marshal.SizeOf(typeof(CURSORINFO));
            GetCursorInfo(out pci);
            return pci.hCursor;
        }

        public static void SetGlobalCursor(Cursor c) {
            SetCursor(c.Handle);
        }
        public static void SetBorder(Process window, Screen monitor, bool enableBorder) {
            Process foreground = Process.GetCurrentProcess();
            Int32 lStyle = GetWindowLong(window.MainWindowHandle, GWL_STYLE);
            if (enableBorder) lStyle |= WS_CAPTION | WS_THICKFRAME | WS_SYSMENU;
            else lStyle &= ~(WS_CAPTION | WS_THICKFRAME | WS_MINIMIZE | WS_MAXIMIZE | WS_SYSMENU);


            Int32 lExStyle = GetWindowLong(window.MainWindowHandle, GWL_EXSTYLE);
            if (enableBorder) lExStyle |= WS_EX_DLGMODALFRAME | WS_EX_CLIENTEDGE;
            else lExStyle &= ~(WS_EX_DLGMODALFRAME | WS_EX_CLIENTEDGE | WS_EX_STATICEDGE);

            SetWindowLong(window.MainWindowHandle, GWL_STYLE, lStyle);
            SetWindowLong(window.MainWindowHandle, GWL_EXSTYLE, lExStyle);
            SetWindowPos(window.MainWindowHandle, IntPtr.Zero, 0, 0, 0, 0, SWP_FRAMECHANGED | SWP_NOSIZE | SWP_NOREPOSITION | SWP_NOMOVE | SWP_NOZORDER | SWP_NOOWNERZORDER);
            SetWindowPos(window.MainWindowHandle, IntPtr.Zero, monitor.Bounds.X, monitor.Bounds.Y, monitor.Bounds.Width, monitor.Bounds.Height, SWP_NOZORDER | SWP_NOOWNERZORDER);
            BringToFront(foreground);
        }

        public static void BringToFront(Process proc) {
            SetForegroundWindow(proc.MainWindowHandle);
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
                BringToFront(foreground);
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

        private static ILog Logger = LogManager.GetLogger("ProcessManager");

        public static void BlockingRunForm(Form form, ICrashable root) {
            Application.EnableVisualStyles();
            sForm = form;
            sRoot = root;
            if (!Debugger.IsAttached) {
            //if (true) {
                Logger.Info("Listening for crashes.");
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
                Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
                try {
                    Application.Run(form);
                } catch (Exception e) {
                    Logger.Warn("Exception caught in GUI thread.");
                    HandleException(e);
                }
            } else
                Application.Run(form);
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e) {
            Logger.Warn("Exception caught in Thread.");
            sForm.Close();
            HandleException(e.Exception);
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e) {
            Logger.Warn("Exception caught in App Domain.");
            sForm.Close();
            HandleException((Exception)e.ExceptionObject);
        }

        static void HandleException(Exception e) {
            Logger.Warn(e.Message);
            Logger.Warn(e.StackTrace);
            sRoot.OnCrash(e);
            throw e;
            //Environment.Exit(42);
        }

        public static void Dump(string dump, string end) {
            DumpConfig cfg = new DumpConfig();

            if (cfg.Dump) {
                string t = DateTime.Now.ToString("dd-HH.mm");
                string y = DateTime.Now.ToString("yyyy");
                string m = DateTime.Now.ToString("MMM");

                string file = Path.GetFullPath(cfg.Folder + y + "/" + m + "/" + t + end);
                if (!Directory.Exists(Path.GetDirectoryName(file)))
                    Directory.CreateDirectory(Path.GetDirectoryName(file));
                Logger.Info("Dumping to " + file);

                File.WriteAllText(file, dump);
            }
        }

        private class DumpConfig : ConfigBase {
            public bool Dump;
            public string Folder;

            public DumpConfig()
                : base() {
            }

            public override string Group {
                get { return "Dump"; }
            }

            protected override void InitConfig() {
                Dump = Get(true, "DumpLogs", true, "Whether to write logs which are dumped via the ProcessWrangler.Dump method.");
                Folder = Get(true, "DumpFolder", "Logs/", "The folder to write log files to.");
            }
        }
    }
}

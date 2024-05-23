using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Coree.NETWindows.NativeMethods
{
    /// <summary>
    /// Provides a set of static methods for managing windows at the operating system level.
    /// </summary>
    public static partial class WindowManagement
    {
        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        public static void MoveWin(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint)
        {
            var nfo = GetWindowInformation(hWnd);
            var newx = X - (int)(nfo.cxWindowBorders);
            var newy = Y;
            var neww = nWidth + (int)(nfo.cxWindowBorders * 2);
            var newh = nHeight + (int)(nfo.cyWindowBorders * 1);
            MoveWindow(hWnd, newx, newy, neww, newh, true);
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            /// <summary>
            /// Gets the X-coordinate of the rectangle.
            /// </summary>
            public int X
            {
                get { return Left; }
            }

            /// <summary>
            /// Gets the Y-coordinate of the rectangle.
            /// </summary>
            public int Y
            {
                get { return Top; }
            }

            /// <summary>
            /// Gets the width of the rectangle.
            /// </summary>
            public int Width
            {
                get { return Right - Left; }
            }

            /// <summary>
            /// Gets the height of the rectangle.
            /// </summary>
            public int Height
            {
                get { return Bottom - Top; }
            }

            public Rectangle ToRectangle()
            {
                return new Rectangle(this.X, this.Y, this.Width, this.Height);
            }
        }

        /// <summary>
        /// Retrieves the dimensions of the client area for the specified window.
        /// </summary>
        /// <remarks>
        /// This method encapsulates the native GetClientRect function from the user32.dll.
        /// </remarks>
        /// <param name="windowHandle">A handle to the window.</param>
        /// <returns>
        /// A RECT structure that receives the client coordinates.
        /// </returns>
        /// <example>
        /// <code>
        /// IntPtr hWnd = this.Handle; // Handle to a form or control
        /// RECT clientRect = GetClientRectangle(hWnd);
        /// Console.WriteLine($"Left: {clientRect.Left}, Top: {clientRect.Top}, Right: {clientRect.Right}, Bottom: {clientRect.Bottom}");
        /// </code>
        /// </example>
        public static RECT GetClientRectangle(IntPtr windowHandle)
        {
            RECT rect;
            if (GetClientRect(windowHandle, out rect))
            {
                return rect;
            }
            else
            {
                throw new InvalidOperationException("Failed to get client rectangle");
            }
        }

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        /// <summary>
        /// Retrieves the dimensions of the bounding rectangle of the specified window.
        /// </summary>
        /// <remarks>
        /// This method encapsulates the native GetWindowRect function from the user32.dll.
        /// </remarks>
        /// <param name="windowHandle">A handle to the window.</param>
        /// <returns>
        /// A RECT structure that receives the screen coordinates of the upper-left and lower-right corners of the window.
        /// </returns>
        /// <example>
        /// <code>
        /// IntPtr hWnd = this.Handle; // Handle to a form or control
        /// RECT windowRect = GetWindowRectangle(hWnd);
        /// Console.WriteLine($"Left: {windowRect.Left}, Top: {windowRect.Top}, Right: {windowRect.Right}, Bottom: {windowRect.Bottom}");
        /// </code>
        /// </example>
        public static RECT GetWindowRectangle(IntPtr windowHandle)
        {
            RECT rect;
            if (GetWindowRect(windowHandle, out rect))
            {
                return rect;
            }
            else
            {
                throw new InvalidOperationException("Failed to get window rectangle");
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct WINDOWINFO
        {
            public uint cbSize;
            public RECT rcWindow;
            public RECT rcClient;
            public uint dwStyle;
            public uint dwExStyle;
            public uint dwWindowStatus;
            public uint cxWindowBorders;
            public uint cyWindowBorders;
            public ushort atomWindowType;
            public ushort wCreatorVersion;

            public WINDOWINFO(Boolean? filler)
            : this()   // Allows for parameterless constructor
            {
                cbSize = (uint)(Marshal.SizeOf(typeof(WINDOWINFO)));
            }
        }

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetWindowInfo(IntPtr hwnd, ref WINDOWINFO pwi);

        public static WINDOWINFO GetWindowInformation(IntPtr windowHandle)
        {
            WINDOWINFO info = new WINDOWINFO();
            info.cbSize = (uint)Marshal.SizeOf(info);
            if (!GetWindowInfo(windowHandle, ref info))
            {
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
            }
            return info;
        }
    }

    public static partial class DrawManagement
    {
        /// <summary>
        /// Draws a line on the desktop using GDI with specified parameters.
        /// </summary>
        /// <param name="startX">The starting X-coordinate of the line.</param>
        /// <param name="startY">The starting Y-coordinate of the line.</param>
        /// <param name="endX">The ending X-coordinate of the line.</param>
        /// <param name="endY">The ending Y-coordinate of the line.</param>
        /// <param name="color">The color of the line.</param>
        /// <param name="thickness">The thickness of the line in pixels.</param>
        public static void DrawLineOnDesktop(int startX, int startY, int endX, int endY, Color color, float thickness)
        {
            IntPtr desktopDC = WindowManagement.GetDC(IntPtr.Zero);
            try
            {
                using (Graphics g = Graphics.FromHdc(desktopDC))
                {
                    using (Pen pen = new Pen(color, thickness))
                    {
                        g.DrawLine(pen, startX, startY, endX, endY);
                    }
                }
            }
            finally
            {
                WindowManagement.ReleaseDC(IntPtr.Zero, desktopDC);
            }
        }

        public static void DrawRECTOnDesktop2(WindowManagement.RECT rECT, Color color)
        {
            DrawLineOnDesktop2(rECT.Left, rECT.Top, rECT.Right - rECT.Left, rECT.Bottom - rECT.Top, color, 1);
        }

        public static void DrawRECTOnDesktop2(int startX, int startY, int witdh, int height, Color color)
        {
            DrawLineOnDesktop2(startX, startY, witdh, height, color, 1);
        }

        public static void DrawLineOnDesktop2(int startX, int startY, int witdh, int height, Color color, float thickness)
        {
            IntPtr desktopDC = WindowManagement.GetDC(IntPtr.Zero);
            try
            {
                using (Graphics g = Graphics.FromHdc(desktopDC))
                {
                    using (Pen pen = new Pen(color, thickness))
                    {
                        g.DrawRectangle(pen, startX, startY, witdh, height);
                    }
                }
            }
            finally
            {
                WindowManagement.ReleaseDC(IntPtr.Zero, desktopDC);
            }
        }
    }

    public class MonitorManager3
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct MonitorInfoExA
        {
            public int cbSize;
            public RECT rcMonitor;
            public RECT rcWork;
            public int dwFlags;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string szDevice;

            public MonitorInfoExA()
            {
                cbSize = Marshal.SizeOf(typeof(MonitorInfoExA));
                rcMonitor = new RECT();
                rcWork = new RECT();
                dwFlags = 0;
                szDevice = string.Empty;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        private const int MONITORINFOF_PRIMARY = 1;

        public class MonitorDetails
        {
            public string DeviceName { get; set; }
            public RECT Coordinates { get; set; }
            public RECT WorkArea { get; set; }
            public int Flags { get; set; }
            public bool IsPrimary => (Flags & MONITORINFOF_PRIMARY) != 0;

            public override string ToString()
            {
                return $"Device: {DeviceName}, Primary: {IsPrimary}, Coordinates: ({Coordinates.left}, {Coordinates.top}, {Coordinates.right}, {Coordinates.bottom}), Work Area: ({WorkArea.left}, {WorkArea.top}, {WorkArea.right}, {WorkArea.bottom}), Flags: {Flags}";
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Ansi)]
        private static extern bool GetMonitorInfo(IntPtr hMonitor, ref MonitorInfoExA lpmi);

        [DllImport("user32.dll")]
        private static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, MonitorEnumDelegate lpfnEnum, IntPtr dwData);

        private delegate bool MonitorEnumDelegate(IntPtr hMonitor, IntPtr hdcMonitor, ref RECT lprcMonitor, IntPtr dwData);

        private static List<MonitorDetails> monitors = new List<MonitorDetails>();

        private static bool MonitorEnumProc(IntPtr hMonitor, IntPtr hdcMonitor, ref RECT lprcMonitor, IntPtr dwData)
        {
            MonitorInfoExA mi = new MonitorInfoExA();
            if (GetMonitorInfo(hMonitor, ref mi))
            {
                monitors.Add(new MonitorDetails
                {
                    DeviceName = mi.szDevice,
                    Coordinates = mi.rcMonitor,
                    WorkArea = mi.rcWork,
                    Flags = mi.dwFlags
                });
            }
            return true; // Continue enumeration
        }

        public static List<MonitorDetails> EnumerateMonitors()
        {
            monitors.Clear();
            EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, MonitorEnumProc, IntPtr.Zero);
            return monitors;
        }
    }

    public class PositionManager
    {

        public enum Posi
        {
            BottomLeft,
            BottomRight,
            TopLeft,
            TopRight,
        }

        public static void Pos(IntPtr Handle,Posi posi, int Width, int Height)
        {
            var monitorDetails = MonitorManager3.EnumerateMonitors().First(e => e.IsPrimary == true);
            switch (posi)
            {
                case Posi.BottomLeft:
                    WindowManagement.MoveWin(Handle, monitorDetails.WorkArea.left + 0, monitorDetails.WorkArea.bottom - Height, Width, Height, true);
                    break;
                case Posi.BottomRight:
                    WindowManagement.MoveWin(Handle, monitorDetails.WorkArea.right -Width, monitorDetails.WorkArea.bottom - Height, Width, Height, true);
                    break;
                case Posi.TopLeft:
                    WindowManagement.MoveWin(Handle, monitorDetails.WorkArea.left, monitorDetails.WorkArea.top, Width, Height, true);
                    break;
                case Posi.TopRight:
                    WindowManagement.MoveWin(Handle, monitorDetails.WorkArea.right - Width, monitorDetails.WorkArea.top, Width, Height, true);
                    break;
                default:
                    break;
            }
        }

        public static void ConsolePos(Posi posi, int Width, int Height)
        {

            Pos(ConsoleManagement.GetConsoleWindow(), posi, Width, Height);
        }
    }



    public class TopMostManager
    {
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        private const int SWP_NOSIZE = 0x0001;
        private const int SWP_NOMOVE = 0x0002;
        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_TOPMOST = 0x00000008;
        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);

        /// <summary>
        /// Sets the console window to be top-most using the WS_EX_TOPMOST style.
        /// </summary>
        /// <remarks>
        /// This method retrieves the handle of the console window and modifies its extended window styles to include WS_EX_TOPMOST.
        /// It then positions the console window above other standard windows. This change might affect user interaction with other applications.
        /// </remarks>
        public static void SetConsoleWindowTopMost()
        {
            IntPtr consoleWindow = GetConsoleWindow();
            int style = GetWindowLong(consoleWindow, GWL_EXSTYLE);
            SetWindowLong(consoleWindow, GWL_EXSTYLE, style | WS_EX_TOPMOST);
            SetWindowPos(consoleWindow, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);

            Console.WriteLine("Console window set to top-most.");
        }
    }

    public class ConsoleResizeHelper
    {
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private const int GWL_EXSTYLE = -20;
        private const int GWL_STYLE = -16;
        private const int WS_THICKFRAME = 0x00040000;

        /// <summary>
        /// Modifies the console window's extended style to hide its taskbar icon.
        /// </summary>
        /// <remarks>
        /// This method alters the extended window style to avoid the console window appearing in the taskbar by setting it as a tool window.
        /// </remarks>
        public static void UnResizeable()
        {
            IntPtr consoleWindow = GetConsoleWindow();
            int style = GetWindowLong(consoleWindow, GWL_STYLE);
            SetWindowLong(consoleWindow, GWL_STYLE, style & ~WS_THICKFRAME);
        }
    }

    public class RemoveFromTaskbarListManager
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetConsoleWindow();

        [DllImport("ole32.dll")]
        private static extern int CoCreateInstance(ref Guid clsid, IntPtr pUnkOuter, int dwClsContext, ref Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out object ppvObject);

        [DllImport("ole32.dll")]
        private static extern void CoInitialize(IntPtr pvReserved);

        [DllImport("ole32.dll")]
        private static extern void CoUninitialize();

        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("56FDF342-FD6D-11D0-958A-006097C9A090")]
        private interface ITaskbarList
        {
            void HrInit();

            void AddTab(IntPtr hwnd);

            void DeleteTab(IntPtr hwnd);

            void ActivateTab(IntPtr hwnd);

            void SetActiveAlt(IntPtr hwnd);
        }

        public static void ShowTaskbarIcon(IntPtr windowHandle)
        {
            Guid CLSID_TaskbarList = new Guid("56FDF344-FD6D-11D0-958A-006097C9A090");
            Guid IID_ITaskbarList = new Guid("56FDF342-FD6D-11D0-958A-006097C9A090");
            CoInitialize(IntPtr.Zero);
            try
            {
                object taskListObj;
                int result = CoCreateInstance(ref CLSID_TaskbarList, IntPtr.Zero, 1, ref IID_ITaskbarList, out taskListObj);
                if (result == 0)
                {
                    var taskList = (ITaskbarList)taskListObj;
                    taskList.AddTab(windowHandle);
                    Marshal.ReleaseComObject(taskList);
                }
            }
            finally
            {
                CoUninitialize();
            }
        }

        public static void HideTaskbarIcon(IntPtr windowHandle)
        {
            Guid CLSID_TaskbarList = new Guid("56FDF344-FD6D-11D0-958A-006097C9A090");
            Guid IID_ITaskbarList = new Guid("56FDF342-FD6D-11D0-958A-006097C9A090");
            CoInitialize(IntPtr.Zero);
            try
            {
                object taskListObj;
                int result = CoCreateInstance(ref CLSID_TaskbarList, IntPtr.Zero, 1, ref IID_ITaskbarList, out taskListObj);
                if (result == 0)
                {
                    var taskList = (ITaskbarList)taskListObj;
                    taskList.DeleteTab(windowHandle);
                    Marshal.ReleaseComObject(taskList);
                }
            }
            finally
            {
                CoUninitialize();
            }
        }
    }

    public class ConsoleRemoveTitleNoMoveWindowManager
    {
        private const int GWL_STYLE = -16;
        private const int WS_CAPTION = 0xC00000;

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        public static void FixConsoleWindow()
        {
            IntPtr consoleWindowHandle = GetConsoleWindow();
            int currentStyle = GetWindowLong(consoleWindowHandle, GWL_STYLE);
            SetWindowLong(consoleWindowHandle, GWL_STYLE, currentStyle & ~WS_CAPTION);
        }
    }

    public class ConsoleTransprentWindowManager
    {
        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_LAYERED = 0x80000;
        private const int LWA_ALPHA = 0x2;

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetLayeredWindowAttributes(IntPtr hWnd, uint crKey, byte bAlpha, uint dwFlags);

        public static void MakeWindowTransparent(byte transparency)
        {
            IntPtr consoleWindowHandle = GetConsoleWindow();

            // Get the current window style
            int currentStyle = GetWindowLong(consoleWindowHandle, GWL_EXSTYLE);

            // Set window to layered to change transparency
            SetWindowLong(consoleWindowHandle, GWL_EXSTYLE, currentStyle | WS_EX_LAYERED);

            // Set transparency level: 0 (fully transparent) to 255 (opaque)
            SetLayeredWindowAttributes(consoleWindowHandle, 0, transparency, LWA_ALPHA);
        }

    }
}
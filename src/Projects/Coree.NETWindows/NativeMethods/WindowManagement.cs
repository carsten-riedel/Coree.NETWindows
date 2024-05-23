using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
            DrawLineOnDesktop2( rECT.Left, rECT.Top, rECT.Right - rECT.Left, rECT.Bottom - rECT.Top, color, 1);
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

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Coree.NETWindows.NativeMethods
{
    /// <summary>
    /// Provides a set of static methods and properties for managing and interacting with the console.
    /// This includes operations such as setting console colors, writing messages, and configuring
    /// console settings.
    /// </summary>
    /// <remarks>
    /// This partial class can be extended to include various console-related functionalities.
    /// </remarks>
    public static partial class ConsoleManagement
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        internal static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll")]
        internal static extern bool RemoveMenu(IntPtr hMenu, uint uPosition, uint uFlags);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool SetCurrentConsoleFontEx(IntPtr consoleOutput, bool maximumWindow, ref CONSOLE_FONT_INFOEX consoleFontInfo);

        [DllImport("kernel32.dll", SetLastError = true)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool FreeConsole();

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern bool SetConsoleTitle(string lpConsoleTitle);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool SetConsoleScreenBufferInfoEx(IntPtr hConsoleOutput, ref CONSOLE_SCREEN_BUFFER_INFO_EX consoleScreenBufferInfoEx);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool GetConsoleScreenBufferInfoEx(IntPtr hConsoleOutput, ref CONSOLE_SCREEN_BUFFER_INFO_EX consoleScreenBufferInfoEx);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool SetConsoleTextAttribute(IntPtr hConsoleOutput, ushort wAttributes);

        [StructLayout(LayoutKind.Sequential)]
        internal struct COORD
        {
            public short X;
            public short Y;

            public COORD(short x, short y)
            {
                X = x;
                Y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct CONSOLE_FONT_INFOEX
        {
            public uint cbSize;
            public uint nFont;
            public COORD dwFontSize;
            public int FontFamily;
            public int FontWeight;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string FaceName;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct CONSOLE_SCREEN_BUFFER_INFO_EX
        {
            public uint cbSize;
            public COORD dwSize;
            public COORD dwCursorPosition;
            public ushort wAttributes;
            public SMALL_RECT srWindow;
            public COORD dwMaximumWindowSize;
            public ushort wPopupAttributes;
            public bool bFullscreenSupported;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public COLORREF[] ColorTable;

            public static CONSOLE_SCREEN_BUFFER_INFO_EX Create()
            {
                return new CONSOLE_SCREEN_BUFFER_INFO_EX
                {
                    cbSize = (uint)Marshal.SizeOf<CONSOLE_SCREEN_BUFFER_INFO_EX>(),
                    ColorTable = new COLORREF[16]
                };
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct SMALL_RECT
        {
            public short Left;
            public short Top;
            public short Right;
            public short Bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct COLORREF
        {
            public uint ColorDWORD;

            public COLORREF(System.Drawing.Color color)
            {
                ColorDWORD = color.R + ((uint)color.G << 8) + ((uint)color.B << 16);
            }

            public System.Drawing.Color ToColor()
            {
                return System.Drawing.Color.FromArgb((int)(0x000000FFU & ColorDWORD),
                                                     (int)(0x0000FF00U & ColorDWORD) >> 8,
                                                     (int)(0x00FF0000U & ColorDWORD) >> 16);
            }
        }

        /// <summary>
        /// Represents color codes for console output. These colors can be used to set foreground and background colors
        /// of console text, allowing for a variety of text colors in console applications.
        /// </summary>
        /// <remarks>
        /// The colors defined in this enumeration map directly to the console's color settings,
        /// providing a simple way to enhance the visual aspect of console output. Values are based on
        /// standard console color codes, allowing for both bright and dark variations of primary colors,
        /// as well as black, gray, and white.
        /// </remarks>
        public enum ConsoleColors : ushort
        {
            /// <summary>Black color.</summary>
            Black = 0,
            /// <summary>Dark blue color.</summary>
            DarkBlue = 1,
            /// <summary>Dark green color.</summary>
            DarkGreen = 2,
            /// <summary>Dark cyan color, a blend of dark blue and dark green.</summary>
            DarkCyan = 3,
            /// <summary>Dark red color.</summary>
            DarkRed = 4,
            /// <summary>Dark magenta color, a blend of dark red and dark blue.</summary>
            DarkMagenta = 5,
            /// <summary>Dark yellow color, a darker shade of yellow.</summary>
            DarkYellow = 6,
            /// <summary>Gray color, a light shade of black.</summary>
            Gray = 7,
            /// <summary>Dark gray color, a shade between black and gray.</summary>
            DarkGray = 8,
            /// <summary>Bright blue color.</summary>
            Blue = 9,
            /// <summary>Bright green color.</summary>
            Green = 10,
            /// <summary>Cyan color, a blend of blue and green.</summary>
            Cyan = 11,
            /// <summary>Red color.</summary>
            Red = 12,
            /// <summary>Magenta color, a blend of red and blue.</summary>
            Magenta = 13,
            /// <summary>Yellow color.</summary>
            Yellow = 14,
            /// <summary>White color, the combination of all color light.</summary>
            White = 15
        }


        internal const uint SC_CLOSE = 0xF060;
        internal const uint MF_BYCOMMAND = 0x00000000;
        internal const int STD_INPUT_HANDLE = -10;
        internal const int STD_OUTPUT_HANDLE = -11;

        internal const uint ENABLE_EXTENDED_FLAGS = 0x0080;
        internal const uint ENABLE_QUICK_EDIT_MODE = 0x0040;
    }
}
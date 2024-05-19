using System;
using System.Collections.Generic;
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
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
    }
}

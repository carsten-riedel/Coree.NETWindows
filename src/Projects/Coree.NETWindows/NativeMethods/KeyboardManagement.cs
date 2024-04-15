using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


using static System.Collections.Specialized.BitVector32;

namespace Coree.NETWindows.NativeMethods
{
    /// <summary>
    /// Provides functionality for keyboard-related operations.
    /// </summary>
    internal static partial class KeyboardManagement
    {
        [DllImport("user32.dll")]
        internal static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        internal static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        internal const int WM_HOTKEY = 0x0312;
    }



}

using System.Runtime.InteropServices;

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
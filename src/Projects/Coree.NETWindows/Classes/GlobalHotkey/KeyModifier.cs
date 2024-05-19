namespace Coree.NETWindows.Classes.GlobalHotkey
{
    /// <summary>
    /// Defines keyboard modifiers that can be combined using bitwise operations to represent multiple modifiers for a hotkey.
    /// </summary>
    [Flags]
    public enum KeyModifier : int
    {
        /// <summary>
        /// Indicates that no modifiers are pressed.
        /// </summary>
        None = 0x0000,

        /// <summary>
        /// Represents the Alt key modifier.
        /// </summary>
        Alt = 0x0001,

        /// <summary>
        /// Represents the Control (Ctrl) key modifier.
        /// </summary>
        Ctrl = 0x0002,

        /// <summary>
        /// Represents the Shift key modifier.
        /// </summary>
        Shift = 0x0004,

        /// <summary>
        /// Represents the Windows key modifier.
        /// </summary>
        WinKey = 0x0008
    }
}
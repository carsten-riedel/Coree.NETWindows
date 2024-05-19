using Coree.NETWindows.NativeMethods;

namespace Coree.NETWindows.Utilities
{
    /// <summary>
    /// Provides utility methods for managing the application's execution context, typically used within the Program.Main entry point.
    /// </summary>
    public static partial class MainContext
    {
        /// <summary>
        /// Allocates a new console to the calling process.
        /// </summary>
        /// <returns>A boolean value indicating whether the operation was successful.</returns>
        public static bool AllocConsole()
        {
            return ConsoleManagement.AllocConsole();
        }
    }
}
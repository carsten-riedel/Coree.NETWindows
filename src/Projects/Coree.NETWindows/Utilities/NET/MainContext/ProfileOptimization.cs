using System.Reflection;

namespace Coree.NETWindows.Utilities
{
    /// <summary>
    /// Provides utility methods for managing the application's execution context, typically used within the Program.Main entry point.
    /// </summary>
    public static partial class MainContext
    {
        /// <summary>
        /// Enhances startup performance by enabling profile optimization.
        /// </summary>
        /// <remarks>
        /// Call this method early in the Program.Main to optimize startup on subsequent runs.
        /// </remarks>
        public static void ProfileOptimization()
        {
            System.Runtime.ProfileOptimization.SetProfileRoot(AppDomain.CurrentDomain.BaseDirectory);
            System.Runtime.ProfileOptimization.StartProfile($@"{System.Reflection.Assembly.GetEntryAssembly().GetName().Name}.profile");
        }
    }
}

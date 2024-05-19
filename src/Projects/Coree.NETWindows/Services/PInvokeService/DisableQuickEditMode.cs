using System.Runtime.InteropServices;

using Coree.NETStandard.Abstractions.ServiceFactory;
using Coree.NETWindows.NativeMethods;

using Microsoft.Extensions.Logging;

namespace Coree.NETWindows.Services.PInvokeService
{
    public partial class PInvokeService : ServiceFactory<PInvokeService>, IPInvokeService
    {
        /// <summary>
        /// Disables the quick edit mode of the console.
        /// </summary>
        public void DisableQuickEditMode()
        {
            DisableQuickEditModeAsync(CancellationToken.None).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Disables the quick edit mode of the console.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DisableQuickEditModeAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await Task.Run(() =>
                {
                    IntPtr consoleHandle = ConsoleManagement.GetStdHandle(ConsoleManagement.STD_INPUT_HANDLE);
                    if (!ConsoleManagement.GetConsoleMode(consoleHandle, out uint consoleMode))
                    {
                        // Handle error if needed
                        throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
                    }

                    // Clear the quick edit bit in the mode flags
                    consoleMode &= ~ConsoleManagement.ENABLE_QUICK_EDIT_MODE;
                    consoleMode |= ConsoleManagement.ENABLE_EXTENDED_FLAGS; // Required to set the mode

                    if (!ConsoleManagement.SetConsoleMode(consoleHandle, consoleMode))
                    {
                        // Handle error if needed
                        throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
                    }
                }, cancellationToken);

                logger?.LogTrace("Successfully DisableQuickEditMode.");
            }
            catch (OperationCanceledException)
            {
                logger?.LogInformation("DisableQuickEditMode operation was canceled.");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Failed to DisableQuickEditMode.");
            }
        }
    }
}
using System.Reflection.Emit;
using System.Runtime.Intrinsics.X86;

using Coree.NETStandard.Abstractions.ServiceFactory;
using Coree.NETWindows.NativeMethods;

using Microsoft.Extensions.Logging;

namespace Coree.NETWindows.Services.PInvokeService
{

    public partial class PInvokeService : ServiceFactory<PInvokeService>, IPInvokeService
    {
        /// <summary>
        /// Synchronously disables the close button of the console window. This method internally
        /// calls the asynchronous version <see cref="DisableCloseButtonAsync"/> and waits for its completion.
        /// </summary>
        public void DisableCloseButton()
        {
            DisableCloseButtonAsync(CancellationToken.None).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Asynchronously disables the close button of the console window in a thread-safe manner.
        /// Utilizes <see cref="Task.Run(Action)"/> to execute the operation potentially on a background thread.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task DisableCloseButtonAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await Task.Run(() =>
                {
                    IntPtr consoleWindow = ConsoleManagement.GetConsoleWindow();
                    IntPtr systemMenu = ConsoleManagement.GetSystemMenu(consoleWindow, false);
                    ConsoleManagement.RemoveMenu(systemMenu, ConsoleManagement.SC_CLOSE, ConsoleManagement.MF_BYCOMMAND);
                }, cancellationToken);

                logger?.LogTrace("Successfully disabled close button.");
            }
            catch (OperationCanceledException)
            {
                logger?.LogInformation("DisableCloseButtonAsync operation was canceled.");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Failed to disable close button asynchronously.");
            }
        }
    }
}
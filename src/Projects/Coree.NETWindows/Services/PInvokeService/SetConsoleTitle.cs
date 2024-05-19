using Coree.NETStandard.Abstractions.ServiceFactory;
using Coree.NETWindows.NativeMethods;

using Microsoft.Extensions.Logging;

namespace Coree.NETWindows.Services.PInvokeService
{
    public partial class PInvokeService : ServiceFactory<PInvokeService>, IPInvokeService
    {
        /// <summary>
        /// Sets the title of the console window.
        /// </summary>
        /// <param name="consoleTitle">The new title for the console window.</param>
        public void SetConsoleTitle(string consoleTitle)
        {
            SetConsoleTitleAsync(consoleTitle, CancellationToken.None).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Sets the title of the console window.
        /// </summary>
        /// <param name="consoleTitle">The new title for the console window.</param>
        /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task SetConsoleTitleAsync(string consoleTitle, CancellationToken cancellationToken = default)
        {
            try
            {
                await Task.Run(() =>
                {
                    ConsoleManagement.SetConsoleTitle(consoleTitle);
                }, cancellationToken);

                logger?.LogTrace("Successfully set console title.");
            }
            catch (OperationCanceledException)
            {
                logger?.LogInformation("SetConsoleTitle operation was canceled.");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Failed to set console title.");
            }
        }
    }
}
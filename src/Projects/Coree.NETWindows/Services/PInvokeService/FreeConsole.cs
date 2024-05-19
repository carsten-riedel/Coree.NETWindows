using Coree.NETStandard.Abstractions.ServiceFactory;
using Coree.NETWindows.NativeMethods;

using Microsoft.Extensions.Logging;

namespace Coree.NETWindows.Services.PInvokeService
{
    public partial class PInvokeService : ServiceFactory<PInvokeService>, IPInvokeService
    {
        /// <summary>
        /// Releases the current console window associated with the calling process synchronously.
        /// </summary>
        public void FreeConsole()
        {
            FreeConsoleAsync(CancellationToken.None).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Asynchronously releases the current console window associated with the calling process.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task FreeConsoleAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await Task.Run(() =>
                {
                    ConsoleManagement.FreeConsole();
                }, cancellationToken);

                logger?.LogTrace("Successfully freed console.");
            }
            catch (OperationCanceledException)
            {
                logger?.LogInformation("FreeConsoleAsync operation was canceled.");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Failed to free console asynchronously.");
            }
        }
    }
}
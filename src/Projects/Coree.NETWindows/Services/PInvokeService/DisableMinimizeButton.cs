using System.Reflection.Emit;
using System.Runtime.Intrinsics.X86;

using Coree.NETStandard.Abstractions.ServiceFactory;
using Coree.NETWindows.NativeMethods;

using Microsoft.Extensions.Logging;

namespace Coree.NETWindows.Services.PInvokeService
{

    public partial class PInvokeService : ServiceFactory<PInvokeService>, IPInvokeService
    {

        public void DisableMinimizeButton()
        {
            DisableMinimizeButtonAsync(CancellationToken.None).GetAwaiter().GetResult();
        }

        public async Task DisableMinimizeButtonAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await Task.Run(() =>
                {
                    IntPtr consoleWindow = ConsoleManagement.GetConsoleWindow();
                    IntPtr systemMenu = ConsoleManagement.GetSystemMenu(consoleWindow, false);
                    ConsoleManagement.RemoveMenu(systemMenu, ConsoleManagement.SC_MINIMIZE, ConsoleManagement.MF_BYCOMMAND);
                }, cancellationToken);

                logger?.LogTrace("Successfully disabled minimize button.");
            }
            catch (OperationCanceledException)
            {
                logger?.LogInformation("DisableMinimizeButtonAsync operation was canceled.");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Failed to disable minimize button asynchronously.");
            }
        }
    }
}
using System.Runtime.InteropServices;

using Coree.NETStandard.Abstractions.ServiceFactory;
using Coree.NETWindows.NativeMethods;

using Microsoft.Extensions.Logging;

namespace Coree.NETWindows.Services.PInvokeService
{
    public partial class PInvokeService : ServiceFactory<PInvokeService>, IPInvokeService
    {
        /// <summary>
        /// Sets the font of the console window synchronously.
        /// </summary>
        /// <param name="fontSize">The size of the font to set (default is 14).</param>
        /// <param name="fontName">The name of the font to set (default is "Lucida Console").</param>
        public void SetConsoleFont(short fontSize = 14, string fontName = "Lucida Console")
        {
            SetConsoleFontAsync(fontSize, fontName, CancellationToken.None).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Sets the font of the console window asynchronously.
        /// </summary>
        /// <param name="fontSize">The size of the font to set (default is 14).</param>
        /// <param name="fontName">The name of the font to set (default is "Lucida Console").</param>
        /// <param name="cancellationToken">The cancellation token to cancel the operation (default is <see cref="CancellationToken.None"/>).</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task SetConsoleFontAsync(short fontSize = 14, string fontName = "Lucida Console", CancellationToken cancellationToken = default)
        {
            try
            {
                await Task.Run(() =>
                {
                    var consoleFontInfo = new ConsoleManagement.CONSOLE_FONT_INFOEX
                    {
                        cbSize = (uint)Marshal.SizeOf<ConsoleManagement.CONSOLE_FONT_INFOEX>(),
                        nFont = 0,
                        dwFontSize = new ConsoleManagement.COORD(0, fontSize),
                        FontFamily = 0, // Don't care
                        FontWeight = 400, // Normal weight
                        FaceName = fontName
                    };

                    IntPtr stdHandle = ConsoleManagement.GetStdHandle(ConsoleManagement.STD_OUTPUT_HANDLE);
                    if (!ConsoleManagement.SetCurrentConsoleFontEx(stdHandle, false, ref consoleFontInfo))
                    {
                        throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
                    }
                }, cancellationToken);

                logger?.LogTrace("Successfully set console font.");
            }
            catch (OperationCanceledException)
            {
                logger?.LogInformation("SetConsoleFontAsync operation was canceled.");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Failed to set console font asynchronously.");
            }
        }
    }
}
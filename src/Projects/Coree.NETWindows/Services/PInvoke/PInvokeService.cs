using System.Runtime.InteropServices;
using System.Threading;

using Coree.NETStandard;
using Coree.NETStandard.Abstractions;
using Coree.NETWindows.NativeMethods;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Coree.NETWindows.Services.PInvoke
{
    public partial class PInvokeService : DependencySingleton<PInvokeService>, IPInvokeService
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

                logger.LogTrace("Successfully disabled close button.");
            }
            catch (OperationCanceledException)
            {
                logger.LogInformation("DisableCloseButtonAsync operation was canceled.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to disable close button asynchronously.");
            }
        }
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

                logger.LogTrace("Successfully set console font.");
            }
            catch (OperationCanceledException)
            {
                logger.LogInformation("SetConsoleFontAsync operation was canceled.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to set console font asynchronously.");
            }
        }

        /// <summary>
        /// Allocates a new console window synchronously.
        /// </summary>
        public void AllocConsole()
        {
            AllocConsoleAsync(CancellationToken.None).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Asynchronously allocates a new console window if one does not already exist.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task AllocConsoleAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await Task.Run(() =>
                {
                    ConsoleManagement.AllocConsole();
                }, cancellationToken);

                logger.LogTrace("Successfully allocated console.");
            }
            catch (OperationCanceledException)
            {
                logger.LogInformation("AllocConsoleAsync operation was canceled.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to allocate console asynchronously.");
            }
        }

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

                logger.LogTrace("Successfully freed console.");
            }
            catch (OperationCanceledException)
            {
                logger.LogInformation("FreeConsoleAsync operation was canceled.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to free console asynchronously.");
            }
        }
    }
}
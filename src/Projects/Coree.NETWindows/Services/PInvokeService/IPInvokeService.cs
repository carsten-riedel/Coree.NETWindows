namespace Coree.NETWindows.Services.PInvokeService
{
    /// <summary>
    /// Represents the interface for invoking platform (P/Invoke) methods.
    /// </summary>
    public interface IPInvokeService
    {

        void DisableMinimizeButton();

        Task DisableMinimizeButtonAsync(CancellationToken cancellationToken = default);

        void DisableMaximizeButton();

        Task DisableMaximizeButtonAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Disables the close button of the current window using platform-specific methods.
        /// </summary>
        void DisableCloseButton();

        /// <summary>
        /// Asynchronously disables the close button of the current window using platform-specific methods.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DisableCloseButtonAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Disables the quick edit mode of the console.
        /// </summary>
        void DisableQuickEditMode();

        /// <summary>
        /// Disables the quick edit mode of the console.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DisableQuickEditModeAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Sets the font of the console window synchronously.
        /// </summary>
        /// <param name="fontSize">The size of the font to set (default is 14).</param>
        /// <param name="fontName">The name of the font to set (default is "Lucida Console").</param>
        void SetConsoleFont(short fontSize = 14, string fontName = "Lucida Console");

        /// <summary>
        /// Sets the font of the console window asynchronously.
        /// </summary>
        /// <param name="fontSize">The size of the font to set (default is 14).</param>
        /// <param name="fontName">The name of the font to set (default is "Lucida Console").</param>
        /// <param name="cancellationToken">The cancellation token to cancel the operation (default is <see cref="CancellationToken.None"/>).</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task SetConsoleFontAsync(short fontSize = 14, string fontName = "Lucida Console", CancellationToken cancellationToken = default);

        /// <summary>
        /// Releases the current console window associated with the calling process synchronously.
        /// </summary>
        void FreeConsole();

        /// <summary>
        /// Asynchronously releases the current console window associated with the calling process.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task FreeConsoleAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Sets the title of the console window.
        /// </summary>
        /// <param name="consoleTitle">The new title for the console window.</param>
        void SetConsoleTitle(string consoleTitle);

        /// <summary>
        /// Sets the title of the console window.
        /// </summary>
        /// <param name="consoleTitle">The new title for the console window.</param>
        /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task SetConsoleTitleAsync(string consoleTitle, CancellationToken cancellationToken = default);
    }
}
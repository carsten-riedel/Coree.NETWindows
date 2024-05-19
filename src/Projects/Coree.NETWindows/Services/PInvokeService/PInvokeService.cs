using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using Coree.NETStandard.Abstractions.ServiceFactory;
using Coree.NETStandard.Services.FileService;
using Coree.NETWindows.NativeMethods;

using Microsoft.Extensions.Logging;

namespace Coree.NETWindows.Services.PInvokeService
{
    /// <summary>
    /// Provides pinvoke service functionalities for system-level operations.
    /// </summary>
    /// <remarks>
    /// This service implements<see cref="IPInvokeService"/> and provides methods to manipulate
    /// system features, including disabling UI elements of system windows.
    /// </remarks>
    public partial class PInvokeService : ServiceFactory<PInvokeService>, IPInvokeService
    {
        private readonly ILogger<PInvokeService>? logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PInvokeService"/> class.
        /// </summary>
        /// <param name="logger">Optional logger instance for logging purposes.</param>
        /// <remarks>
        /// The logger provided here can be used with the field within the class.
        /// Be mindful that the logger may be null in scenarios where it's not explicitly provided.
        /// </remarks>
        public PInvokeService(ILogger<PInvokeService>? logger = null)
        {
            this.logger = logger;
        }
    }
}
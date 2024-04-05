using Coree.NETStandard.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Coree.NETWindows.Services.PInvoke
{
    /// <summary>
    /// Represents a service for invoking platform (P/Invoke) methods with optional dependency injection support.
    /// This service implements the IPInvokeService interface, providing methods for platform invocation.
    /// This service inherits from DependencySingleton&lt;PInvokeService&gt;, which supports both dependency injection (DI) and non-DI scenarios
    /// </summary>
    public partial class PInvokeService : DependencySingleton<PInvokeService>, IPInvokeService , IDependencySingleton
    {
        /// <summary>
        /// Initializes a new instance of the PInvokeService class with the specified logger and configuration.
        /// </summary>
        /// <param name="logger">The logger instance for logging messages.</param>
        /// <param name="configuration">The configuration instance for accessing application settings.</param>
        public PInvokeService(ILogger<PInvokeService> logger, IConfiguration configuration) : base(logger, configuration) { }
    }
}

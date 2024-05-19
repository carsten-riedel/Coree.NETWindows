using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Coree.NETStandard.Abstractions.ServiceFactory;
using Coree.NETWindows.NativeMethods;

namespace Coree.NETWindows.Services.PInvokeService
{
    public partial class PInvokeService : ServiceFactory<PInvokeService>, IPInvokeService
    {
        /// <summary>
        /// Allocates a new console window synchronously.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AllocConsole()
        {
            ConsoleManagement.AllocConsole();
        }


    }
}

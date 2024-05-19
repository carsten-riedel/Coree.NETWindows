using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Coree.NETStandard.Abstractions.ServiceFactory;
using Coree.NETWindows.NativeMethods;

namespace Coree.NETWindows.Services.PInvokeService
{
    public partial class PInvokeService : ServiceFactory<PInvokeService>, IPInvokeService
    {

        public static void SetConsoleColor(ConsoleManagement.ConsoleColors foregroundColor, ConsoleManagement.ConsoleColors backgroundColor)
        {
            IntPtr consoleHandle = ConsoleManagement.GetStdHandle(ConsoleManagement.STD_OUTPUT_HANDLE);
            ushort attributes = (ushort)((int)backgroundColor << 4 | (int)foregroundColor);

            if (!ConsoleManagement.SetConsoleTextAttribute(consoleHandle, attributes))
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
        }

        public static void SetConsoleColors(System.Drawing.Color screenTextColor, System.Drawing.Color screenBackgroundColor, System.Drawing.Color popupTextColor, System.Drawing.Color popupBackgroundColor)
        {
            IntPtr hConsoleOutput = ConsoleManagement.GetStdHandle(ConsoleManagement.STD_OUTPUT_HANDLE);
            ConsoleManagement.CONSOLE_SCREEN_BUFFER_INFO_EX csbe = ConsoleManagement.CONSOLE_SCREEN_BUFFER_INFO_EX.Create();
            ConsoleManagement.GetConsoleScreenBufferInfoEx(hConsoleOutput, ref csbe);

            // Set the screen text and background colors
            csbe.ColorTable[0] = new ConsoleManagement.COLORREF(screenBackgroundColor); // Background
            csbe.ColorTable[15] = new ConsoleManagement.COLORREF(screenTextColor); // Foreground

            // Set the popup text and background colors
            csbe.ColorTable[1] = new ConsoleManagement.COLORREF(popupBackgroundColor); // Popup Background
            csbe.ColorTable[14] = new ConsoleManagement.COLORREF(popupTextColor); // Popup Text

            // Apply the changes
            ConsoleManagement.SetConsoleScreenBufferInfoEx(hConsoleOutput, ref csbe);
        }
    }
}

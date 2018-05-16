using System;

namespace Shared
{
    public static class Desktop
    {
        private static IntPtr Handle => NativeMethods.FindWindow("Progman", "Program Manager");

        public static void Show()
        {
            NativeMethods.ShowWindow(Handle, NativeMethods.ShowWindowCommands.Normal);
        }

        public static void Hide()
        {
            NativeMethods.ShowWindow(Handle, NativeMethods.ShowWindowCommands.Hide);
        }
    }
}

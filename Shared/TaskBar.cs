﻿using System;

namespace Shared
{
    public static class Taskbar
    {
        private static IntPtr Handle => NativeMethods.FindWindow("Shell_TrayWnd", "");

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

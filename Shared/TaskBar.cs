namespace Shared
{
    public static class Taskbar
    {
        private const int SW_HIDE = 0;
        private const int SW_SHOW = 1;

        public static int Handle => NativeMethods.FindWindow("Shell_TrayWnd", "").ToInt32();

        public static void Show()
        {
            NativeMethods.ShowWindow(Handle, SW_SHOW);
        }

        public static void Hide()
        {
            NativeMethods.ShowWindow(Handle, SW_HIDE);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Interop;

namespace VirtualPC_WPF.Hook
{
    public class HookBase:IHook
    {
        protected virtual void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (sender is Window window)
            {
                var handle = (new WindowInteropHelper(window)).Handle;
                var source = HwndSource.FromHwnd(handle);
                source?.RemoveHook(WndProc);
            }
        }

        protected virtual void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (sender is Window window)
            {
                var handle = new WindowInteropHelper(window).Handle;
                var source = HwndSource.FromHwnd(handle);
                source?.AddHook(WndProc);
            }
        }

        protected virtual IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            return IntPtr.Zero;
        }

        public virtual void Enable(Window window)
        {
            window.Closing += OnClosing;
            window.Loaded += OnLoaded;
        }

        public virtual void Disable(Window window)
        {
            window.Closing -= OnClosing;
            window.Loaded -= OnLoaded;
        }
    }
}

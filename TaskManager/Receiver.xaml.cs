using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Shared;

namespace TaskManager
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class Receiver : Window
    {
        public Action<string> MessageReceived;

        public Receiver()
        {
            InitializeComponent();                
        }
     
        IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == NativeMethods.WM_COPYDATA)
            {
                // Extract the file name
                NativeMethods.COPYDATASTRUCT copyData = (NativeMethods.COPYDATASTRUCT)Marshal.PtrToStructure(lParam, typeof(NativeMethods.COPYDATASTRUCT));
                int dataType = (int)copyData.dwData;
                if (dataType == 2)
                {                    
                    string message = Marshal.PtrToStringAnsi(copyData.lpData);
                    MessageReceived?.Invoke(message);
                }
            }
            handled = true;             
            return IntPtr.Zero;
        }

        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            HwndSource source = PresentationSource.FromVisual(this) as HwndSource;
            source?.AddHook(WndProc);
        }
    }
}

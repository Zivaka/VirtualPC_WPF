using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;

using System.Threading.Tasks;
using System.Timers;
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
using System.Windows.Threading;
using Shared;

namespace VirtualPC_WPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Process _taskbar;
        public BitmapSource ImageSource => new Bitmap($@"{AppDomain.CurrentDomain.BaseDirectory}images\image.jpg").ToBitmapSource();
        public ObservableCollection<DesktopElement> DesktopElements { get; set; } = new ObservableCollection<DesktopElement>();

        public MainWindow()
        {
            InitializeComponent();    

            var pcIcon = new Bitmap($@"{AppDomain.CurrentDomain.BaseDirectory}images\pc.png").ToBitmapSource();
            var icon1 = System.Drawing.Icon.ExtractAssociatedIcon(@"C:\windows\system32\notepad.exe")?.ToBitmap().ToBitmapSource();
            var icon2 = System.Drawing.Icon.ExtractAssociatedIcon(@"C:\Program Files\Internet Explorer\iexplore.exe")?.ToBitmap().ToBitmapSource();
            var icon3 = System.Drawing.Icon.ExtractAssociatedIcon(@"E:\games\Blizzard App\Battle.net Launcher.exe")?.ToBitmap().ToBitmapSource();

            DesktopElements.Add(new DesktopElement("My PC", pcIcon, $"{AppDomain.CurrentDomain.BaseDirectory}FileExplorer.exe"));
            DesktopElements.Add(new DesktopElement("Нотатки", icon1, @"notepad.exe"));
            DesktopElements.Add(new DesktopElement("Internet Explorer", icon2, @"C:\Program Files\Internet Explorer\iexplore.exe"));
            DesktopElements.Add(new DesktopElement("Battle.net Launcher", icon3, @"E:\games\Blizzard App\Battle.net Launcher.exe"));

            var eventTimer = new Timer(100);
            eventTimer.Elapsed += (sender, e) => HandleTimer();
            eventTimer.Start();

            new WindowSinker(this).Sink();
        }

        private void HandleTimer()
        {
            if (_taskbar!=null && _taskbar.HasExited)
                Dispatcher.Invoke(DispatcherPriority.Normal, new Action(Close));
        }    

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(!_taskbar.HasExited) _taskbar.CloseMainWindow();
            
        }

        private void MainWindow_OnActivated(object sender, EventArgs e)
        {
            //var ptr = NativeMethods.FindWindow(null, Title);
            //var ptr2 = NativeMethods.FindWindow(null, "Program Manager");
            //if (ptr != IntPtr.Zero) NativeMethods.SetParent(ptr, ptr2);
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            _taskbar = Process.Start($"{AppDomain.CurrentDomain.BaseDirectory}TaskManager.exe");
            //Process.Start(@"E:\!Универ\Дипломная работа\VirtualPC\VirtualPC_WPF\TaskManager\bin\Debug\TaskManager.exe");
        }

    }
}

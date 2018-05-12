using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private readonly Process _taskbar;
        public BitmapSource ImageSource => new Bitmap($"{AppDomain.CurrentDomain.BaseDirectory}image.jpg").ToBitmapSource();
        public ObservableCollection<DesktopElement> DesktopElements { get; set; } = new ObservableCollection<DesktopElement>();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            _taskbar = Process.Start(@"E:\!Универ\Дипломная работа\VirtualPC\VirtualPC_WPF\TaskManager\bin\Debug\TaskManager.exe");

            var icon1 = System.Drawing.Icon.ExtractAssociatedIcon(@"C:\windows\system32\notepad.exe")?.ToBitmap().ToBitmapSource();
            var icon2 = System.Drawing.Icon.ExtractAssociatedIcon(@"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe")?.ToBitmap().ToBitmapSource();
            var icon3 = System.Drawing.Icon.ExtractAssociatedIcon(@"E:\games\Blizzard App\Battle.net Launcher.exe")?.ToBitmap().ToBitmapSource();

            DesktopElements.Add(new DesktopElement("Notepad", icon1, @"C:\windows\system32\notepad.exe"));
            DesktopElements.Add(new DesktopElement("Google Chrome", icon2, @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe"));
            DesktopElements.Add(new DesktopElement("Battle.net Launcher", icon3, @"E:\games\Blizzard App\Battle.net Launcher.exe"));

            var unused = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                var ptr = NativeMethods.FindWindow(null, "{DESKTOP 123-321}");
                if(ptr != IntPtr.Zero) NativeMethods.SendBack(ptr);
                if (_taskbar.HasExited) Close();
            }, Dispatcher);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(!_taskbar.HasExited) _taskbar.CloseMainWindow();
        }       
    }
}

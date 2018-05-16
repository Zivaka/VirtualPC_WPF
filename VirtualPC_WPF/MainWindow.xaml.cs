using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Timers;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Shared;
using VirtualPC_WPF.Hook;

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

        private HookManager _hookManager;

        public MainWindow()
        {
            InitializeComponent();    

            //var pcIcon = new Bitmap($@"{AppDomain.CurrentDomain.BaseDirectory}images\pc.png").ToBitmapSource();
            //var icon1 = System.Drawing.Icon.ExtractAssociatedIcon(@"C:\windows\system32\notepad.exe")?.ToBitmap().ToBitmapSource();
            //var icon2 = System.Drawing.Icon.ExtractAssociatedIcon(@"C:\Program Files\Internet Explorer\iexplore.exe")?.ToBitmap().ToBitmapSource();
            //var icon3 = System.Drawing.Icon.ExtractAssociatedIcon(@"E:\games\Blizzard App\Battle.net Launcher.exe")?.ToBitmap().ToBitmapSource();

            //DesktopElements.Add(new DesktopElement("My PC", pcIcon, $"{AppDomain.CurrentDomain.BaseDirectory}FileExplorer.exe"));
            //DesktopElements.Add(new DesktopElement("Нотатки", icon1, @"C:\windows\system32\notepad.exe"));
            //DesktopElements.Add(new DesktopElement("Internet Explorer", icon2, @"C:\Program Files\Internet Explorer\iexplore.exe"));
            //DesktopElements.Add(new DesktopElement("Battle.net Launcher", icon3, @"E:\games\Blizzard App\Battle.net Launcher.exe"));

            var eventTimer = new Timer(100);
            eventTimer.Elapsed += (sender, e) => HandleTimer();
            eventTimer.Start();

            InitHooks();
            InitDesktop();
        }

        private void InitHooks()
        {
            _hookManager = new HookManager(this);
            _hookManager.Attach(new WindowHook());
            _hookManager.Attach(new KeyBoardHook());
            _hookManager.Attach(new WindowsCombinationHook());
        }

        private void InitDesktop()
        {
            var desktopFileName = $"{AppDomain.CurrentDomain.BaseDirectory}{Configurations.DesktopInfoFileName}";
            if (!File.Exists(desktopFileName))
            {
                const string defaultDesktop = "~FileExplorer.exe | Цей комп'ютер";
                File.WriteAllText(desktopFileName, defaultDesktop);
            }
            var desktopElements = File.ReadLines(desktopFileName);
            foreach (var element in desktopElements)
            {
                //if comment
                if(element.StartsWith("#")) continue;
                var parsedLine = element.Split('|');
                // if wrong line
                if(parsedLine.Length == 0) continue;
                var path = parsedLine[0];      
                // if program dir path
                if (path.StartsWith("~")) path = path.Replace("~", AppDomain.CurrentDomain.BaseDirectory);
                // if not exe
                if (Path.GetExtension(path) != ".exe") continue;
                var name = parsedLine.Length > 1 ? parsedLine[1] : Path.GetFileNameWithoutExtension(path);
                var icon = System.Drawing.Icon.ExtractAssociatedIcon(path)?.ToBitmap().ToBitmapSource();
                DesktopElements.Add(new DesktopElement(name, icon, path));
            }
        }

        private void HandleTimer()
        {
            if (_taskbar!=null && _taskbar.HasExited)
                Dispatcher.Invoke(DispatcherPriority.Normal, new Action(Close));
        }    

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!_taskbar.HasExited) _taskbar.CloseMainWindow();           
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            _taskbar = Process.Start($"{AppDomain.CurrentDomain.BaseDirectory}TaskManager.exe");
        }

        private void MainWindow_OnClosed(object sender, EventArgs e)
        {
            _hookManager.DetachAll();
        }
    }
}

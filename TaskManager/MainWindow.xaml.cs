using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Drawing;
using System.IO;
using System.Timers;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

using Shared;

namespace TaskManager
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {
        public ObservableCollection<TaskBarElement> Elements { get; set; } = new ObservableCollection<TaskBarElement>();
        public BitmapSource ImageSource => new Bitmap($@"{AppDomain.CurrentDomain.BaseDirectory}images\start.png").ToBitmapSource();

        public MainWindow()
        {
            InitializeComponent();

            new Receiver {MessageReceived = MessageR}.Show();        
        }

              
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);            
            WindowStyle = WindowStyle.None;
            ResizeMode = ResizeMode.NoResize;
            Width = SystemParameters.PrimaryScreenWidth;
            Topmost = true;
            Left = 0;
            Top = SystemParameters.PrimaryScreenHeight - Height;
            var unused = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                DateTimeBox.Text = DateTime.Now.ToShortTimeString() + Environment.NewLine + DateTime.Now.ToShortDateString();
                InternetBox.Source = new Bitmap($@"{AppDomain.CurrentDomain.BaseDirectory}images\internet_{NativeMethods.IsConnectedToInternet().ToString()}.png").ToBitmapSource();      
            }, Dispatcher);

            var timer = new Timer(10);
            timer.Elapsed += (s, ars) =>
            {
                Dispatcher.Invoke(DispatcherPriority.Normal, new Action(UpdateLanguage));
                int index = 0;
                while (index < Elements.Count)
                {
                    var process = Elements[index];
                    if (process == null) continue;
                    if (process.HasExited)
                        Dispatcher.Invoke(DispatcherPriority.Normal, new Action<int>(RemoveAt), index);
                    else
                        index++;
                }
            };
            timer.Start();
            //Taskbar.Hide();
        }

        private void RemoveAt(int id)
        {
            Elements.RemoveAt(id);
        }

        private void UpdateLanguage()
        {
            LanguageBox.Text = new CultureInfo(CurrentCultureInfo.GetKeyboardLayoutIdAtTime()).ThreeLetterISOLanguageName.ToUpper();
        }

        private void MessageR(string path)
        {
            if (File.Exists(path))
            {
                var newTask = new TaskBarElement(path);
                Elements.Add(newTask);                
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            foreach (var taskBarElement in Elements)         
                taskBarElement.Close();
            Taskbar.Show();
            Environment.Exit(0);
        }

        private void WinButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

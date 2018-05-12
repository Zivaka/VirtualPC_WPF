using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Drawing;
using System.IO;
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

        public MainWindow()
        {
            InitializeComponent();
            Control.Items.Clear();
            DataContext = this;

            new Receiver {MessageReceived = MessageR}.Show();          
        }

        public BitmapSource ImageSource => new Bitmap($"{AppDomain.CurrentDomain.BaseDirectory}start.png").ToBitmapSource();
              
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
                LanguageBox.Text = new CultureInfo(CurrentCultureInfo.GetKeyboardLayoutIdAtTime()).ThreeLetterISOLanguageName.ToUpper();
                InternetBox.Source = new Bitmap($"{AppDomain.CurrentDomain.BaseDirectory}internet_{NativeMethods.IsConnectedToInternet().ToString()}.png").ToBitmapSource();
                int index = 0;
                while (index < Elements.Count)
                {
                    var process = Elements[index];
                    if(process == null) continue;
                    if (process.HasExited)
                        Elements.RemoveAt(index);
                    else
                        index++;
                }         
            }, Dispatcher);
            Taskbar.Hide();
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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

using Shared;
using TaskManager.Annotations;

namespace TaskManager
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private Receiver _receiver;

        public MainWindow()
        {
            InitializeComponent();

            _receiver = new Receiver {MessageReceived = MessageR};
            _receiver.Show();


            var a = new TaskBarElement(@"C:\WINDOWS\system32\notepad.exe");
            Control.Items.Clear();
            Control.Items.Add(a);
        }

        //(bitmap);
        public BitmapSource Image => new Bitmap(@"C:\Users\Alex-REG\Documents\Scanned Documents\Приветствие программы сканирования.jpg").ToBitmapSource();//System.Drawing.Icon.ExtractAssociatedIcon(@"C:\WINDOWS\system32\notepad.exe")?.ToBitmap().ToBitmapSource();

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            //Taskbar.Hide();
            WindowStyle = WindowStyle.None;
            ResizeMode = ResizeMode.NoResize;
            Width = SystemParameters.PrimaryScreenWidth;
            Topmost = true;
            Left = 0;
            Top = SystemParameters.PrimaryScreenHeight - Height;
            new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                DateTimeBox.Text = DateTime.Now.ToLongTimeString() + Environment.NewLine + DateTime.Now.ToShortDateString();
                //DateTimeBox.ToolTip = DateTime.Now.ToLongDateString();
                LanguageBox.Text = new CultureInfo(CurrentCultureInfo.GetKeyboardLayoutIdAtTime()).ThreeLetterISOLanguageName.ToUpper();
            }, this.Dispatcher);
        }

        private void MessageR(string s)
        {
            //
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        private void WinButton_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

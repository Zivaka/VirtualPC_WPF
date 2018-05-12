using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;
using TaskManager.Annotations;

namespace TaskManager
{
    public class TaskBarElement : INotifyPropertyChanged
    {
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string _fullPath;
        public string FullPath
        {
            get => _fullPath;
            set
            {
                _fullPath = value;
                OnPropertyChanged(nameof(FullPath));
            }
        }

        private BitmapSource _processIcon;
        public BitmapSource ProcessIcon
        {
            get => _processIcon;
            set
            {
                _processIcon = value;
                OnPropertyChanged(nameof(BitmapSource));
            }
        }


        public Process ProcessInfo;

        public TaskBarElement(string fullPath)
        {
            FullPath = fullPath;
            Name = Path.GetFileNameWithoutExtension(FullPath);
            if (Path.GetExtension(fullPath) == ".exe")            
                ProcessIcon = Icon.ExtractAssociatedIcon(fullPath)?.ToBitmap().ToBitmapSource();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public static class BitmapConversion
    {
        public static BitmapSource ToBitmapSource(this Bitmap source)
        {
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                source.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
        }
    }
}

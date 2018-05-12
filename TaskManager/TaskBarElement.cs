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
using System.Windows.Input;
using System.Windows.Media.Imaging;
using TaskManager.Annotations;
using Shared;

namespace TaskManager
{
    public class TaskBarElement : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string FullPath { get; set; }  
        public BitmapSource ProcessIcon { get; set; }
        public Process ProcessInfo { get; set; }

        public ICommand ActivateWindowCmnd { get; }
        public ICommand CloseWindowCmnd { get; }

        

        public bool HasExited => ProcessInfo.HasExited;

        public TaskBarElement(string fullPath)
        {
            FullPath = fullPath;
            Name = Path.GetFileNameWithoutExtension(FullPath);
            if (Path.GetExtension(fullPath) == ".exe")            
                ProcessIcon = Icon.ExtractAssociatedIcon(fullPath)?.ToBitmap().ToBitmapSource();
            ProcessInfo = Process.Start(fullPath);
            ActivateWindowCmnd = new ActivateWindowCommand();
            CloseWindowCmnd = new CloseWindowCommand();
        }

        public void Close()
        {
            ProcessInfo.Kill();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private class ActivateWindowCommand : ICommand
        {
            public bool CanExecute(object parameter) { return true; }
            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {
                var process = parameter as TaskBarElement;
                if (process == null || process.HasExited) return;
                var processHandle = process.ProcessInfo.MainWindowHandle;

                NativeMethods.ShowWindow(processHandle,
                    NativeMethods.IsMinimised(processHandle)
                        ? NativeMethods.ShowWindowCommands.Restore
                        : NativeMethods.ShowWindowCommands.Normal);
                NativeMethods.SetForegroundWindow(processHandle);
            }
        }

        private class CloseWindowCommand : ICommand
        {
            public bool CanExecute(object parameter) { return true; }
            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {
                var process = parameter as TaskBarElement;
                if (process == null || process.HasExited) return;
                process.ProcessInfo.CloseMainWindow();
            }
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

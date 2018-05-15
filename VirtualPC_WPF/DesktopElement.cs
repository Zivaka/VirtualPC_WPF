using System;
using System.Windows.Input;
using System.Windows.Media;
using Shared;

namespace VirtualPC_WPF
{
    public class DesktopElement
    {
        public string Name { get; set; }

        public ImageSource Image { get; set; }
        
        public string Command { get; set; }

        public ICommand Open { get; set; }

        public DesktopElement(string name, ImageSource image, string command)
        {
            Name = name;
            Image = image;
            Command = command;
            Open = new OpenCommand();
        }

        private class OpenCommand : ICommand
        {
            public bool CanExecute(object parameter) { return true; }
            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {
                if (parameter is DesktopElement element)
                    NativeMethods.SendMessage(Configurations.TaskManagerWindowTitle, element.Command);
            }
        }       
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
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
                var element = parameter as DesktopElement;
                if (element == null) return;
                SendMessage(element.Command);
            }
        }

        private static void SendMessage(string message)
        {
            string windowTitle = "{TASKMANAGER 123-321}";
            // Find the window with the name of the main form
            IntPtr ptrWnd = NativeMethods.FindWindow(null, windowTitle);
            if (ptrWnd == IntPtr.Zero)
            {
                
            }
            else
            {
                IntPtr ptrCopyData = IntPtr.Zero;
                try
                {
                    // Create the data structure and fill with data
                    NativeMethods.COPYDATASTRUCT copyData = new NativeMethods.COPYDATASTRUCT();
                    copyData.dwData = new IntPtr(2);    // Just a number to identify the data type
                    copyData.cbData = message.Length + 1;  // One extra byte for the \0 character
                    copyData.lpData = Marshal.StringToHGlobalAnsi(message);

                    // Allocate memory for the data and copy
                    ptrCopyData = Marshal.AllocCoTaskMem(Marshal.SizeOf(copyData));
                    Marshal.StructureToPtr(copyData, ptrCopyData, false);

                    // Send the message
                    NativeMethods.SendMessage(ptrWnd, NativeMethods.WM_COPYDATA, IntPtr.Zero, ptrCopyData);
                }
                catch (Exception ex)
                {
                    
                }
                finally
                {
                    // Free the allocated memory after the contol has been returned
                    if (ptrCopyData != IntPtr.Zero)
                        Marshal.FreeCoTaskMem(ptrCopyData);
                }
            }
        }
    }
}

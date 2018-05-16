using System.ComponentModel;
using System.Windows;
using Shared;

namespace VirtualPC_WPF.Hook
{
    public class WindowsCombinationHook: HookBase
    {
        protected override void OnClosing(object sender, CancelEventArgs e)
        {
            Taskbar.Show();
            Desktop.Show();
            Shared.TaskManager.Show();
        }

        protected override void OnLoaded(object sender, RoutedEventArgs e)
        {
            Taskbar.Hide();
            Desktop.Hide();
            Shared.TaskManager.Hide();
        }
    }
}

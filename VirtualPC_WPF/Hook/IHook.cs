using System.Windows;

namespace VirtualPC_WPF.Hook
{
    public interface IHook
    {
        void Enable(Window window);
        void Disable(Window window);
    }
}

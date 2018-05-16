using System.Collections.Generic;
using System.Windows;

namespace VirtualPC_WPF.Hook
{
    public class HookManager
    {
        private readonly Window _window;

        private List<IHook> _hooks = new List<IHook>();

        public HookManager(Window window)
        {
            _window = window;
        }

        public void Attach(IHook hook)
        {
            hook.Enable(_window);
            _hooks.Add(hook);
        }

        public void Detach(IHook hook)
        {
            hook.Disable(_window);
            _hooks.Remove(hook);
        }

        public static HookManager operator +(HookManager manager, IHook hook)
        {
            manager.Attach(hook);
            return manager;
        }

        public static HookManager operator -(HookManager manager, IHook hook)
        {
            manager.Detach(hook);
            return manager;
        }

        public void DetachAll()
        {
            while (_hooks.Count > 0)
            {
                _hooks[0].Disable(_window);
                _hooks.RemoveAt(0);
            }
        }

    }
}

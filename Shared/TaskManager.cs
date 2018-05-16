using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace Shared
{
    public static class TaskManager
    {
        private static string _registryPath = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System";

        public static void Show()
        {
            try
            {
                var regkey = Registry.CurrentUser.CreateSubKey(_registryPath);
                regkey?.SetValue("DisableTaskMgr", "0");
                regkey?.Close();
            }
            catch
            {
                // ignored
            }
        }

        public static void Hide()
        {
            try
            {
                var regkey = Registry.CurrentUser.CreateSubKey(_registryPath);
                regkey?.SetValue("DisableTaskMgr", "1");
                regkey?.Close();
            }
            catch 
            {
                // ignored
            }
        }
    }
}

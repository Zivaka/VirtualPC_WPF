using System.IO;
using Microsoft.Win32;

namespace Shared
{
    public static class RegistryHelper
    {
        public static bool TryGetRegisteredApplication(string extension, out string registeredApp)
        {
            string extensionId = GetClassesRootKeyDefaultValue(extension);

            if (extensionId == null)
            {
                registeredApp = null;
                return false;
            }

            string openCommand = GetClassesRootKeyDefaultValue(
                Path.Combine(new[] { extensionId, "shell", "open", "command" }));

            if (openCommand == null)
            {
                registeredApp = null;
                return false;
            }

            registeredApp = openCommand
                .Replace("%1", string.Empty)
                .Replace("\"", string.Empty)
                .Trim();

            return true;
        }

        private static string GetClassesRootKeyDefaultValue(string keyPath)
        {
            using (var key = Registry.ClassesRoot.OpenSubKey(keyPath))
            {
                var defaultValue = key?.GetValue(null);
                return defaultValue?.ToString();
            }
        }
    }
}

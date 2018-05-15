using System;
using System.IO;
using System.Linq;

namespace Shared
{
    public static class PermissionHelper
    {
        private static readonly string PermissionsFileName = $"{AppDomain.CurrentDomain.BaseDirectory}{Configurations.PermissionsFileName}";

        public static bool HavePermissionToOpen(string filename)
        {
            if (!File.Exists(PermissionsFileName)) return false;
            var allowedPermissions = File.ReadLines(PermissionsFileName).ToList();

            if (allowedPermissions.Any(permission => permission.Equals(Path.GetExtension(filename))))
                return true;

            if (!filename.EndsWith(".exe"))
            {
                if (!RegistryHelper.TryGetRegisteredApplication(Path.GetExtension(filename), out filename))
                    return false;
            }

            return allowedPermissions.Any(permission => permission == filename);
        }  
    }
}

using System;
using System.Text.RegularExpressions;

namespace VirtualFileSystem
{
    public static class VirtualUnixSupport
    {
        public static string Path(string path, Func<bool> isUnixF = null)
        {
            var isUnix = isUnixF ?? IsUnixPlatform;

            if (isUnix())
            {
                path = Regex.Replace(path, @"^[a-zA-Z]:(?<path>.*)$", "${path}");
                path = path.Replace(@"\", "/");
            }

            return path;
        }

        public static string Separator(Func<bool> isUnixF = null)
        {
            var isUnix = isUnixF ?? IsUnixPlatform;
            return isUnix() ? "/" : @"\";
        }

        public static bool IsUnixPlatform()
        {
            return System.IO.Path.DirectorySeparatorChar == '/';
        }
    }
}

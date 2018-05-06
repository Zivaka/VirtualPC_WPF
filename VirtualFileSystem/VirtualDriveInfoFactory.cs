using System;
using System.Collections.Generic;
using System.IO.Abstractions;

namespace VirtualFileSystem
{
    [Serializable]
    public class VirtualDriveInfoFactory : IDriveInfoFactory
    {
        private readonly IVirtualFileDataAccessor VirtualFileSystem;

        public VirtualDriveInfoFactory(IVirtualFileDataAccessor VirtualFileSystem)
        {
            if (VirtualFileSystem == null)
            {
                throw new ArgumentNullException("VirtualFileSystem");
            }

            this.VirtualFileSystem = VirtualFileSystem;
        }

        public DriveInfoBase[] GetDrives()
        {
            var driveLetters = new HashSet<string>(new DriveEqualityComparer());
            foreach (var path in VirtualFileSystem.AllPaths)
            {
                var pathRoot = VirtualFileSystem.Path.GetPathRoot(path);
                driveLetters.Add(pathRoot);
            }

            var result = new List<DriveInfoBase>();
            foreach (string driveLetter in driveLetters)
            {
                try
                {
                    var VirtualDriveInfo = new VirtualDriveInfo(VirtualFileSystem, driveLetter);                    
                    result.Add(VirtualDriveInfo);
                }
                catch (ArgumentException)
                {
                    // invalid drives should be ignored
                }
            }

            return result.ToArray();
        }

        private string NormalizeDriveName(string driveName)
        {
            if (driveName.Length == 3 && driveName.EndsWith(@":\", StringComparison.OrdinalIgnoreCase))
            {
                return char.ToUpperInvariant(driveName[0]) + @":\";
            }

            if (driveName.StartsWith(@"\\", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            return driveName;
        }

        private class DriveEqualityComparer : IEqualityComparer<string>
        {
            public bool Equals(string x, string y)
            {
                if (ReferenceEquals(x, y))
                {
                    return true;
                }

                if (ReferenceEquals(x, null))
                {
                    return false;
                }

                if (ReferenceEquals(y, null))
                {
                    return false;
                }

                if (x[1] == ':' && y[1] == ':')
                {
                    return char.ToUpperInvariant(x[0]) == char.ToUpperInvariant(y[0]);
                }

                return false;
            }

            public int GetHashCode(string obj)
            {
                return obj.ToUpperInvariant().GetHashCode();
            }
        }
    }
}

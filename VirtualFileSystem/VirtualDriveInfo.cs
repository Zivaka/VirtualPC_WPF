using System;
using System.IO;
using System.IO.Abstractions;

namespace VirtualFileSystem
{
    [Serializable]
    public class VirtualDriveInfo : DriveInfoBase
    {
        private readonly IVirtualFileDataAccessor _virtualFileDataAccessor;

        public VirtualDriveInfo(IVirtualFileDataAccessor virtualFileDataAccessor, string name)
        {
            if (virtualFileDataAccessor == null)
            {
                throw new ArgumentNullException(nameof(virtualFileDataAccessor));
            }

            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            const string DRIVE_SEPARATOR = @":\";

            if (name.Length == 1)
            {
                name = char.ToUpperInvariant(name[0]) + DRIVE_SEPARATOR;
            }
            else if (name.Length == 2 && name[1] == ':')
            {
                name = char.ToUpperInvariant(name[0]) + DRIVE_SEPARATOR;
            }
            else if (name.Length == 3 && name.EndsWith(DRIVE_SEPARATOR, StringComparison.Ordinal))
            {
                name = char.ToUpperInvariant(name[0]) + DRIVE_SEPARATOR;
            }
            else
            {
                VirtualPath.CheckInvalidPathChars(name);
                name = virtualFileDataAccessor.Path.GetPathRoot(name);

                if (string.IsNullOrEmpty(name) || name.StartsWith(@"\\", StringComparison.Ordinal))
                {
                    throw new ArgumentException(
                        @"Object must be a root directory (""C:\"") or a drive letter (""C"").");
                }
            }

            this._virtualFileDataAccessor = virtualFileDataAccessor;

            Name = name;
            IsReady = true;
        }

        public new long AvailableFreeSpace { get; set; }
        public new string DriveFormat { get; set; }
        public new DriveType DriveType { get; set; }
        public new bool IsReady { get; protected set; }
        public override string Name { get; protected set; }

        public override DirectoryInfoBase RootDirectory
        {
            get
            {
                var directory = _virtualFileDataAccessor.DirectoryInfo.FromDirectoryName(Name);
                return directory;
            }
        }

        public new long TotalFreeSpace { get; protected set; }
        public new long TotalSize { get; protected set; }
        public override string VolumeLabel { get; set; }
    }
}

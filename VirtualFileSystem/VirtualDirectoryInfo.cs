using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Security.AccessControl;

namespace VirtualFileSystem
{
    [Serializable]
    public class VirtualDirectoryInfo : DirectoryInfoBase
    {
        private readonly IVirtualFileDataAccessor VirtualFileDataAccessor;
        private readonly string directoryPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualDirectoryInfo"/> class.
        /// </summary>
        /// <param name="VirtualFileDataAccessor">The Virtual file data accessor.</param>
        /// <param name="directoryPath">The directory path.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="VirtualFileDataAccessor"/> or <paramref name="directoryPath"/> is <see langref="null"/>.</exception>
        public VirtualDirectoryInfo(IVirtualFileDataAccessor VirtualFileDataAccessor, string directoryPath)
        {
            this.VirtualFileDataAccessor = VirtualFileDataAccessor ?? throw new ArgumentNullException("VirtualFileDataAccessor");

            directoryPath = VirtualFileDataAccessor.Path.GetFullPath(directoryPath);

            this.directoryPath = EnsurePathEndsWithDirectorySeparator(directoryPath);
        }

        VirtualFileData VirtualFileData
        {
            get { return VirtualFileDataAccessor.GetFile(directoryPath); }
        }

        public override void Delete()
        {
            VirtualFileDataAccessor.Directory.Delete(directoryPath);
        }

        public override void Refresh()
        {
        }

        public override FileAttributes Attributes
        {
            get { return VirtualFileData.Attributes; }
            set { VirtualFileData.Attributes = value; }
        }

        public override DateTime CreationTime
        {
            get { throw new NotImplementedException(StringResources.Manager.GetString("NOT_IMPLEMENTED_EXCEPTION")); }
            set { throw new NotImplementedException(StringResources.Manager.GetString("NOT_IMPLEMENTED_EXCEPTION")); }
        }

        public override DateTime CreationTimeUtc
        {
            get { throw new NotImplementedException(StringResources.Manager.GetString("NOT_IMPLEMENTED_EXCEPTION")); }
            set { throw new NotImplementedException(StringResources.Manager.GetString("NOT_IMPLEMENTED_EXCEPTION")); }
        }

        public override bool Exists
        {
            get { return VirtualFileDataAccessor.Directory.Exists(FullName); }
        }

        public override string Extension
        {
            get
            {
                // System.IO.Path.GetExtension does only string manipulation,
                // so it's safe to delegate.
                return Path.GetExtension(directoryPath);
            }
        }

        public override string FullName
        {
            get
            {
                var root = VirtualFileDataAccessor.Path.GetPathRoot(directoryPath);
                if (string.Equals(directoryPath, root, StringComparison.OrdinalIgnoreCase))
                {
                    // drives have the trailing slash
                    return directoryPath;
                }

                // directories do not have a trailing slash
                return directoryPath.TrimEnd('\\').TrimEnd('/');
            }
        }

        public override DateTime LastAccessTime
        {
            get { throw new NotImplementedException(StringResources.Manager.GetString("NOT_IMPLEMENTED_EXCEPTION")); }
            set { throw new NotImplementedException(StringResources.Manager.GetString("NOT_IMPLEMENTED_EXCEPTION")); }
        }

        public override DateTime LastAccessTimeUtc
        {
            get {
                if (VirtualFileData == null) throw new FileNotFoundException("File not found", directoryPath);
                return VirtualFileData.LastAccessTime.UtcDateTime;
            }
            set { throw new NotImplementedException(StringResources.Manager.GetString("NOT_IMPLEMENTED_EXCEPTION")); }
        }

        public override DateTime LastWriteTime
        {
            get { throw new NotImplementedException(StringResources.Manager.GetString("NOT_IMPLEMENTED_EXCEPTION")); }
            set { throw new NotImplementedException(StringResources.Manager.GetString("NOT_IMPLEMENTED_EXCEPTION")); }
        }

        public override DateTime LastWriteTimeUtc
        {
            get { throw new NotImplementedException(StringResources.Manager.GetString("NOT_IMPLEMENTED_EXCEPTION")); }
            set { throw new NotImplementedException(StringResources.Manager.GetString("NOT_IMPLEMENTED_EXCEPTION")); }
        }

        public override string Name
        {
            get { return new VirtualPath(VirtualFileDataAccessor).GetFileName(directoryPath.TrimEnd('\\')); }
        }

        public override void Create()
        {
            VirtualFileDataAccessor.Directory.CreateDirectory(FullName);
        }

        public override void Create(DirectorySecurity directorySecurity)
        {
            VirtualFileDataAccessor.Directory.CreateDirectory(FullName, directorySecurity);
        }

        public override DirectoryInfoBase CreateSubdirectory(string path)
        {
            return VirtualFileDataAccessor.Directory.CreateDirectory(Path.Combine(FullName, path));
        }

        public override DirectoryInfoBase CreateSubdirectory(string path, DirectorySecurity directorySecurity)
        {
            return VirtualFileDataAccessor.Directory.CreateDirectory(Path.Combine(FullName, path), directorySecurity);
        }

        public override void Delete(bool recursive)
        {
            VirtualFileDataAccessor.Directory.Delete(directoryPath, recursive);
        }

        public override IEnumerable<DirectoryInfoBase> EnumerateDirectories()
        {
            return GetDirectories();
        }

        public override IEnumerable<DirectoryInfoBase> EnumerateDirectories(string searchPattern)
        {
            return GetDirectories(searchPattern);
        }

        public override IEnumerable<DirectoryInfoBase> EnumerateDirectories(string searchPattern, SearchOption searchOption)
        {
            return GetDirectories(searchPattern, searchOption);
        }

        public override IEnumerable<FileInfoBase> EnumerateFiles()
        {
            return GetFiles();
        }

        public override IEnumerable<FileInfoBase> EnumerateFiles(string searchPattern)
        {
            return GetFiles(searchPattern);
        }

        public override IEnumerable<FileInfoBase> EnumerateFiles(string searchPattern, SearchOption searchOption)
        {
            return GetFiles(searchPattern, searchOption);
        }

        public override IEnumerable<FileSystemInfoBase> EnumerateFileSystemInfos()
        {
            return GetFileSystemInfos();
        }

        public override IEnumerable<FileSystemInfoBase> EnumerateFileSystemInfos(string searchPattern)
        {
            return GetFileSystemInfos(searchPattern);
        }

        public override IEnumerable<FileSystemInfoBase> EnumerateFileSystemInfos(string searchPattern, SearchOption searchOption)
        {
            return GetFileSystemInfos(searchPattern, searchOption);
        }

        public override DirectorySecurity GetAccessControl()
        {
            return VirtualFileDataAccessor.Directory.GetAccessControl(directoryPath);
        }

        public override DirectorySecurity GetAccessControl(AccessControlSections includeSections)
        {
            return VirtualFileDataAccessor.Directory.GetAccessControl(directoryPath, includeSections);
        }

        public override DirectoryInfoBase[] GetDirectories()
        {
            return ConvertStringsToDirectories(VirtualFileDataAccessor.Directory.GetDirectories(directoryPath));
        }

        public override DirectoryInfoBase[] GetDirectories(string searchPattern)
        {
            return ConvertStringsToDirectories(VirtualFileDataAccessor.Directory.GetDirectories(directoryPath, searchPattern));
        }

        public override DirectoryInfoBase[] GetDirectories(string searchPattern, SearchOption searchOption)
        {
            return ConvertStringsToDirectories(VirtualFileDataAccessor.Directory.GetDirectories(directoryPath, searchPattern, searchOption));
        }

        private DirectoryInfoBase[] ConvertStringsToDirectories(IEnumerable<string> paths)
        {
            return paths
                .Select(path => new VirtualDirectoryInfo(VirtualFileDataAccessor, path))
                .Cast<DirectoryInfoBase>()
                .ToArray();
        }

        public override FileInfoBase[] GetFiles()
        {
            return ConvertStringsToFiles(VirtualFileDataAccessor.Directory.GetFiles(FullName));
        }

        public override FileInfoBase[] GetFiles(string searchPattern)
        {
            return ConvertStringsToFiles(VirtualFileDataAccessor.Directory.GetFiles(FullName, searchPattern));
        }

        public override FileInfoBase[] GetFiles(string searchPattern, SearchOption searchOption)
        {
            return ConvertStringsToFiles(VirtualFileDataAccessor.Directory.GetFiles(FullName, searchPattern, searchOption));
        }

        FileInfoBase[] ConvertStringsToFiles(IEnumerable<string> paths)
        {
            return paths
                  .Select(VirtualFileDataAccessor.FileInfo.FromFileName)
                  .ToArray();
        }

        public override FileSystemInfoBase[] GetFileSystemInfos()
        {
            return GetFileSystemInfos("*");
        }

        public override FileSystemInfoBase[] GetFileSystemInfos(string searchPattern)
        {
            return GetFileSystemInfos(searchPattern, SearchOption.TopDirectoryOnly);
        }

        public override FileSystemInfoBase[] GetFileSystemInfos(string searchPattern, SearchOption searchOption)
        {
            return GetDirectories(searchPattern, searchOption).OfType<FileSystemInfoBase>().Concat(GetFiles(searchPattern, searchOption)).ToArray();
        }

        public override void MoveTo(string destDirName)
        {
            VirtualFileDataAccessor.Directory.Move(directoryPath, destDirName);
        }

        public override void SetAccessControl(DirectorySecurity directorySecurity)
        {
            VirtualFileDataAccessor.Directory.SetAccessControl(directoryPath, directorySecurity);
        }

        public override DirectoryInfoBase Parent
        {
            get
            {
                return VirtualFileDataAccessor.Directory.GetParent(directoryPath);
            }
        }

        public override DirectoryInfoBase Root
        {
            get
            {
                return new VirtualDirectoryInfo(VirtualFileDataAccessor, VirtualFileDataAccessor.Directory.GetDirectoryRoot(FullName));
            }
        }

        private string EnsurePathEndsWithDirectorySeparator(string path)
        {
            if (!path.EndsWith(string.Format(CultureInfo.InvariantCulture, "{0}", VirtualFileDataAccessor.Path.DirectorySeparatorChar), StringComparison.OrdinalIgnoreCase))
            {
                path += VirtualFileDataAccessor.Path.DirectorySeparatorChar;
            }

            return path;
        }

        public override string ToString()
        {
            return FullName;
        }
    }
}

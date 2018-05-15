using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace VirtualFileSystem
{
    using XFS = VirtualUnixSupport;

    [Serializable]
    public class VirtualFileSystem : IFileSystem, IVirtualFileDataAccessor
    {
        private readonly IDictionary<string, VirtualFileData> _files;
        private readonly FileBase _file;
        private readonly DirectoryBase _directory;
        private readonly IFileInfoFactory _fileInfoFactory;
        private readonly PathBase _pathField;
        private readonly IDirectoryInfoFactory _directoryInfoFactory;
        private readonly IDriveInfoFactory _driveInfoFactory;
        private readonly PathVerifier _pathVerifier;

        public VirtualFileSystem() : this(null)
        {
        }

        public VirtualFileSystem(IDictionary<string, VirtualFileData> files, string currentDirectory = "")
        {
            if (string.IsNullOrEmpty(currentDirectory))
                currentDirectory = System.IO.Path.GetTempPath();

            _pathVerifier = new PathVerifier(this);

            this._files = new Dictionary<string, VirtualFileData>(StringComparer.OrdinalIgnoreCase);
            _pathField = new VirtualPath(this);
            _file = new VirtualFile(this);
            _directory = new VirtualDirectory(this, _file, currentDirectory);
            _fileInfoFactory = new VirtualFileInfoFactory(this);
            _directoryInfoFactory = new VirtualDirectoryInfoFactory(this);
            _driveInfoFactory = new VirtualDriveInfoFactory(this);
            
            if (files != null)
            {
                foreach (var entry in files)
                {
                    AddFile(entry.Key, entry.Value);
                }
            }
        }

        public static VirtualFileSystem LoadFromFile(string path)
        {
            if (!System.IO.File.Exists(path)) return null;
            IFormatter formatter = new BinaryFormatter();
            using (Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                 return (VirtualFileSystem)formatter.Deserialize(stream);
        }

        public void SaveToFile(string path)
        {
            IFormatter formatter = new BinaryFormatter();
            using (Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
                formatter.Serialize(stream, this);
        }

        public FileBase File => _file;

        public DirectoryBase Directory => _directory;

        public IFileInfoFactory FileInfo => _fileInfoFactory;

        public PathBase Path => _pathField;

        public IDirectoryInfoFactory DirectoryInfo => _directoryInfoFactory;

        public IDriveInfoFactory DriveInfo => _driveInfoFactory;

        public PathVerifier PathVerifier => _pathVerifier;

        private string FixPath(string path, bool checkCaps = false)
        {
            var pathSeparatorFixed = path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            var fullPath = _pathField.GetFullPath(pathSeparatorFixed);

            return checkCaps ? GetPathWithCorrectDirectoryCapitalization(fullPath) : fullPath;
        }

        private string GetPathWithCorrectDirectoryCapitalization(string fullPath)
        {
            string[] splitPath = fullPath.Split(Path.DirectorySeparatorChar);
            string leftHalf = fullPath;
            string rightHalf = "";
            for (int i = splitPath.Length - 1; i > 1; i--)
            {
                rightHalf = i == splitPath.Length - 1
                    ? splitPath[i]
                    : splitPath[i] + Path.DirectorySeparatorChar + rightHalf;
                int lastSeparator = leftHalf.LastIndexOf(Path.DirectorySeparatorChar);
                leftHalf = lastSeparator > 0 ? leftHalf.Substring(0, lastSeparator) : leftHalf;
                if (_directory.Exists(leftHalf))
                {
                    leftHalf += Path.DirectorySeparatorChar;
                    leftHalf = _pathField.GetFullPath(leftHalf);
                    string baseDirectory =
                        AllDirectories.First(dir => dir.Equals(leftHalf, StringComparison.OrdinalIgnoreCase));
                    return baseDirectory + rightHalf;
                }
            }

            return fullPath;
        }


        public VirtualFileData GetFile(string path)
        {
            path = FixPath(path);

            return GetFileWithoutFixingPath(path);
        }

        public void AddFile(string path, VirtualFileData virtualFile)
        {
            var fixedPath = FixPath(path, true);
            lock (_files)
            {
                if (FileExists(fixedPath))
                {
                    var isReadOnly = (_files[fixedPath].Attributes & FileAttributes.ReadOnly) ==
                                     FileAttributes.ReadOnly;
                    var isHidden = (_files[fixedPath].Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;

                    if (isReadOnly || isHidden)
                    {
                        throw new UnauthorizedAccessException(string.Format(CultureInfo.InvariantCulture,
                            StringResources.Manager.GetString("ACCESS_TO_THE_PATH_IS_DENIED") ?? throw new InvalidOperationException(), path));
                    }
                }

                var directoryPath = Path.GetDirectoryName(fixedPath);

                if (!_directory.Exists(directoryPath))
                {
                    AddDirectory(directoryPath);
                }

                _files[fixedPath] = virtualFile;
            }
        }

        public void AddDirectory(string path)
        {
            var fixedPath = FixPath(path, true);
            var separator = XFS.Separator();

            lock (_files)
            {
                if (FileExists(fixedPath) &&
                    (_files[fixedPath].Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                    throw new UnauthorizedAccessException(string.Format(CultureInfo.InvariantCulture,
                        StringResources.Manager.GetString("ACCESS_TO_THE_PATH_IS_DENIED") ?? throw new InvalidOperationException(), fixedPath));

                var lastIndex = 0;

                bool isUnc =
                    fixedPath.StartsWith(@"\\", StringComparison.OrdinalIgnoreCase) ||
                    fixedPath.StartsWith(@"//", StringComparison.OrdinalIgnoreCase);

                if (isUnc)
                {
                    //First, confirm they aren't trying to create '\\server\'
                    lastIndex = fixedPath.IndexOf(separator, 2, StringComparison.OrdinalIgnoreCase);
                    if (lastIndex < 0)
                        throw new ArgumentException(@"The UNC path should be of the form \\server\share.",
                            nameof(path));
                }

                while ((lastIndex = fixedPath.IndexOf(separator, lastIndex + 1, StringComparison.OrdinalIgnoreCase)) >
                       -1)
                {
                    var segment = fixedPath.Substring(0, lastIndex + 1);
                    if (!_directory.Exists(segment))
                    {
                        _files[segment] = new VirtualDirectoryData();
                    }
                }

                var s = fixedPath.EndsWith(separator, StringComparison.OrdinalIgnoreCase)
                    ? fixedPath
                    : fixedPath + separator;
                _files[s] = new VirtualDirectoryData();
            }
        }

        public void AddFileFromEmbeddedResource(string path, Assembly resourceAssembly, string embeddedResourcePath)
        {
            using (var embeddedResourceStream = resourceAssembly.GetManifestResourceStream(embeddedResourcePath))
            {
                if (embeddedResourceStream == null)
                {
                    throw new Exception("Resource not found in assembly");
                }

                using (var streamReader = new BinaryReader(embeddedResourceStream))
                {
                    var fileData = streamReader.ReadBytes((int) embeddedResourceStream.Length);
                    AddFile(path, new VirtualFileData(fileData));
                }
            }
        }

        public void AddFilesFromEmbeddedNamespace(string path, Assembly resourceAssembly, string embeddedResourcePath)
        {
            var matchingResources = resourceAssembly.GetManifestResourceNames()
                .Where(f => f.StartsWith(embeddedResourcePath));
            foreach (var resource in matchingResources)
            {
                using (var embeddedResourceStream = resourceAssembly.GetManifestResourceStream(resource))
                using (var streamReader =
                    new BinaryReader(embeddedResourceStream ?? throw new InvalidOperationException()))
                {
                    var fileName = resource.Substring(embeddedResourcePath.Length + 1);
                    var fileData = streamReader.ReadBytes((int) embeddedResourceStream.Length);
                    var filePath = Path.Combine(path, fileName);
                    AddFile(filePath, new VirtualFileData(fileData));
                }
            }
        }

        public void RemoveFile(string path)
        {
            path = FixPath(path);

            lock (_files)
                _files.Remove(path);
        }

        public bool FileExists(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;

            path = FixPath(path);

            lock (_files)
                return _files.ContainsKey(path);
        }

        public IEnumerable<string> AllPaths
        {
            get
            {
                lock (_files)
                    return _files.Keys.ToArray();
            }
        }

        public IEnumerable<string> AllFiles
        {
            get
            {
                lock (_files)
                    return _files.Where(f => !f.Value.IsDirectory).Select(f => f.Key).ToArray();
            }
        }

        public IEnumerable<string> AllDirectories
        {
            get
            {
                lock (_files)
                    return _files.Where(f => f.Value.IsDirectory).Select(f => f.Key).ToArray();
            }
        }

        private VirtualFileData GetFileWithoutFixingPath(string path)
        {
            lock (_files)
            {
                _files.TryGetValue(path, out var result);
                return result;
            }
        }
    }
}
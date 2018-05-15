using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace VirtualFileSystem
{
    public static class VirtualFileSystemGenerator
    {
        private const string DriveSeparator = @":\";

        public static string ToRealPath(this string virtualPath, string realRootPath,
            string driveSeparator = DriveSeparator)
        {
            var driveSeparatorIndex = virtualPath.IndexOf(DriveSeparator, StringComparison.Ordinal);
            if (driveSeparatorIndex < 0) throw new IndexOutOfRangeException(nameof(driveSeparatorIndex));
            if (!realRootPath.EndsWith("\\")) realRootPath += "\\";
            return realRootPath + virtualPath.Remove(driveSeparatorIndex, DriveSeparator.Length).Insert(driveSeparatorIndex, "\\");
        }

        public static string ToVirtualPath(this string realPath, string fullPath, string driveSeparator = DriveSeparator)
        {
            if(fullPath.Length <= realPath.Length)
                throw new ArgumentException($"{nameof(fullPath)} should be longer than {nameof(realPath)}");

            var clearPath = fullPath.Remove(0, realPath.Length);
            var driveSeparatorIndex = clearPath.IndexOf('\\');
            if (driveSeparatorIndex < 0)
            {
                return clearPath + DriveSeparator;
            }
            return clearPath.Remove(driveSeparatorIndex, 1).Insert(driveSeparatorIndex, DriveSeparator);
        }


        private static List<string> GetDirs(string path, int deep)
        {
            if (deep == 0) return new List<string>();
            try
            {
                var root = Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly).ToList();
                var rootCount = root.Count;
                for (int i = 0; i < rootCount; i++)
                    root.AddRange(GetDirs(root[i], deep - 1));
                return root;
            }
            catch (Exception)
            {
                // ignored
            }
            return new List<string>();
        }

        private static List<string> GetFiles(string path)
        {
            try
            {
                return Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly).ToList();
            }
            catch (Exception e)
            {
                Debug.WriteLine($"[{path}] {e.Message}");
            }
            return new List<string>();
        }

        private static VirtualFileData GetFileData(string path)
        {
            var file = new FileInfo(path);
            var data = new VirtualFileData(file.Length.ToString())
            {
                CreationTime = file.CreationTime,
                LastWriteTime = file.LastWriteTime
            };
            return data;
        }

        private static bool CanAddFile(string path)
        {
            var file = new FileInfo(path);
            if (file.Attributes.HasFlag(FileAttributes.Hidden) ||
                file.Attributes.HasFlag(FileAttributes.System) ||
                file.Attributes.HasFlag(FileAttributes.Temporary)) return false;
            return true;
        }

        private static bool CanAddDirectory(string path)
        {
            var directory = new DirectoryInfo(path);
            if (directory.Attributes.HasFlag(FileAttributes.Hidden) ||
                directory.Attributes.HasFlag(FileAttributes.System) ||
                directory.Attributes.HasFlag(FileAttributes.Temporary)) return false;
            return true;
        }

        public static VirtualFileSystem GenerateFileSystemFromRealFileSystem(int deep = 2)
        {
            var virtualFileSystem = new VirtualFileSystem();
            var drives = DriveInfo.GetDrives().Select(x => x.Name);

            foreach (var drive in drives)
            {
                var dirs = GetDirs(drive, deep);
                dirs.ForEach(d =>
                {
                    if (CanAddDirectory(d))
                    {
                        virtualFileSystem.AddDirectory(d);
                        foreach (var file in GetFiles(d))
                        {
                            if (!CanAddFile(file)) continue;
                            var info = GetFileData(file);
                            virtualFileSystem.AddFile(file, info);
                        }
                    }
                });
            }

            return virtualFileSystem;
        }


        public static VirtualFileSystem GenerateFileSystemFromDirectory1(string realPath)
        {
            if (!Directory.Exists(realPath)) throw new DirectoryNotFoundException($"Directory [{realPath}] not found.");
            if (!realPath.EndsWith("\\")) realPath += "\\";
            var allDirectories = Directory.GetDirectories(realPath, "*", SearchOption.AllDirectories);
            var allFiles = Directory.GetFiles(realPath, "*.*", SearchOption.AllDirectories);
            var virtualFileSystem = new VirtualFileSystem();

            foreach (var readDirectoryPath in allDirectories)
            {
                var virtualDirectoryPath = realPath.ToVirtualPath(readDirectoryPath);
                virtualFileSystem.AddDirectory(virtualDirectoryPath);
            }

            foreach (var realFilePath in allFiles)
            {
                var virtualFilePath = realPath.ToVirtualPath(realFilePath);
                var fileData = File.ReadAllBytes(realFilePath);
                virtualFileSystem.AddFile(virtualFilePath,new VirtualFileData(fileData));
            }

            return virtualFileSystem;
        }
    }
}

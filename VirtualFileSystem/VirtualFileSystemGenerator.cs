using System;
using System.Collections.Generic;
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

        public static VirtualFileSystem GenerateFileSystemFromDirectory(string realPath)
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

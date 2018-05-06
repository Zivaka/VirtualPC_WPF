using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Security.AccessControl;
using System.Text;

namespace VirtualFileSystem
{
    [Serializable]
    public class VirtualFile : FileBase
    {
        private readonly IVirtualFileDataAccessor VirtualFileDataAccessor;
        private readonly VirtualPath VirtualPath;

        public VirtualFile(IVirtualFileDataAccessor VirtualFileDataAccessor)
        {
            if (VirtualFileDataAccessor == null)
            {
                throw new ArgumentNullException("VirtualFileDataAccessor");
            }

            this.VirtualFileDataAccessor = VirtualFileDataAccessor;
            VirtualPath = new VirtualPath(VirtualFileDataAccessor);
        }

        public override void AppendAllLines(string path, IEnumerable<string> contents)
        {
            VirtualFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");
            VerifyValueIsNotNull(contents, "contents");

            AppendAllLines(path, contents, VirtualFileData.DefaultEncoding);
        }

        public override void AppendAllLines(string path, IEnumerable<string> contents, Encoding encoding)
        {
            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }

            VirtualFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            var concatContents = contents.Aggregate("", (a, b) => a + b + Environment.NewLine);
            AppendAllText(path, concatContents, encoding);
        }

        public override void AppendAllText(string path, string contents)
        {
            VirtualFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            AppendAllText(path, contents, VirtualFileData.DefaultEncoding);
        }

        public override void AppendAllText(string path, string contents, Encoding encoding)
        {
            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }

            VirtualFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            if (!VirtualFileDataAccessor.FileExists(path))
            {
                var dir = VirtualFileDataAccessor.Path.GetDirectoryName(path);
                if (!VirtualFileDataAccessor.Directory.Exists(dir))
                {
                    throw new DirectoryNotFoundException(string.Format(CultureInfo.InvariantCulture, StringResources.Manager.GetString("COULD_NOT_FIND_PART_OF_PATH_EXCEPTION"), path));
                }

                VirtualFileDataAccessor.AddFile(path, new VirtualFileData(contents, encoding));
            }
            else
            {
                var file = VirtualFileDataAccessor.GetFile(path);
                var bytesToAppend = encoding.GetBytes(contents);
                file.Contents = file.Contents.Concat(bytesToAppend).ToArray();
            }
        }

        public override StreamWriter AppendText(string path)
        {
            VirtualFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            if (VirtualFileDataAccessor.FileExists(path))
            {
                StreamWriter sw = new StreamWriter(OpenWrite(path));
                sw.BaseStream.Seek(0, SeekOrigin.End); //push the stream pointer at the end for append.
                return sw;
            }

            return new StreamWriter(Create(path));
        }

        public override void Copy(string sourceFileName, string destFileName)
        {
            Copy(sourceFileName, destFileName, false);
        }

        public override void Copy(string sourceFileName, string destFileName, bool overwrite)
        {
            if (sourceFileName == null)
            {
                throw new ArgumentNullException("sourceFileName", StringResources.Manager.GetString("FILENAME_CANNOT_BE_NULL"));
            }

            if (destFileName == null)
            {
                throw new ArgumentNullException("destFileName", StringResources.Manager.GetString("FILENAME_CANNOT_BE_NULL"));
            }

            VirtualFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(sourceFileName, "sourceFileName");
            VirtualFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(destFileName, "destFileName");

            var directoryNameOfDestination = VirtualPath.GetDirectoryName(destFileName);
            if (!VirtualFileDataAccessor.Directory.Exists(directoryNameOfDestination))
            {
                throw new DirectoryNotFoundException(string.Format(CultureInfo.InvariantCulture, StringResources.Manager.GetString("COULD_NOT_FIND_PART_OF_PATH_EXCEPTION"), destFileName));
            }

            var fileExists = VirtualFileDataAccessor.FileExists(destFileName);
            if (fileExists)
            {
                if (!overwrite)
                {
                    throw new IOException(string.Format(CultureInfo.InvariantCulture, "The file {0} already exists.", destFileName));
                }

                VirtualFileDataAccessor.RemoveFile(destFileName);
            }

            var sourceFile = VirtualFileDataAccessor.GetFile(sourceFileName);
            VirtualFileDataAccessor.AddFile(destFileName, sourceFile);
        }

        public override Stream Create(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path", "Path cannot be null.");
            }
            VirtualFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            VirtualFileDataAccessor.AddFile(path, new VirtualFileData(new byte[0]));
            var stream = OpenWrite(path);
            return stream;
        }

        public override Stream Create(string path, int bufferSize)
        {
            throw new NotImplementedException(StringResources.Manager.GetString("NOT_IMPLEMENTED_EXCEPTION"));
        }

        public override Stream Create(string path, int bufferSize, FileOptions options)
        {
            throw new NotImplementedException(StringResources.Manager.GetString("NOT_IMPLEMENTED_EXCEPTION"));
        }

        public override Stream Create(string path, int bufferSize, FileOptions options, FileSecurity fileSecurity)
        {
            throw new NotImplementedException(StringResources.Manager.GetString("NOT_IMPLEMENTED_EXCEPTION"));
        }

        public override StreamWriter CreateText(string path)
        {
            return new StreamWriter(Create(path));
        }

        public override void Decrypt(string path)
        {
            VirtualFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            new VirtualFileInfo(VirtualFileDataAccessor, path).Decrypt();
        }

        public override void Delete(string path)
        {
            VirtualFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            VirtualFileDataAccessor.RemoveFile(path);
        }

        public override void Encrypt(string path)
        {
            VirtualFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            new VirtualFileInfo(VirtualFileDataAccessor, path).Encrypt();
        }

        public override bool Exists(string path)
        {
            return VirtualFileDataAccessor.FileExists(path) && !VirtualFileDataAccessor.AllDirectories.Any(d => d.Equals(path, StringComparison.OrdinalIgnoreCase));
        }

        public override FileSecurity GetAccessControl(string path)
        {
            VirtualFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            if (!VirtualFileDataAccessor.FileExists(path))
            {
                throw new FileNotFoundException(string.Format(CultureInfo.InvariantCulture, "Can't find {0}", path), path);
            }

            var fileData = VirtualFileDataAccessor.GetFile(path);
            return fileData.AccessControl;
        }

        public override FileSecurity GetAccessControl(string path, AccessControlSections includeSections)
        {
            return GetAccessControl(path);
        }

        /// <summary>
        /// Gets the <see cref="FileAttributes"/> of the file on the path.
        /// </summary>
        /// <param name="path">The path to the file.</param>
        /// <returns>The <see cref="FileAttributes"/> of the file on the path.</returns>
        /// <exception cref="ArgumentException"><paramref name="path"/> is empty, contains only white spaces, or contains invalid characters.</exception>
        /// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.</exception>
        /// <exception cref="NotSupportedException"><paramref name="path"/> is in an invalid format.</exception>
        /// <exception cref="FileNotFoundException"><paramref name="path"/> represents a file and is invalid, such as being on an unmapped drive, or the file cannot be found.</exception>
        /// <exception cref="DirectoryNotFoundException"><paramref name="path"/> represents a directory and is invalid, such as being on an unmapped drive, or the directory cannot be found.</exception>
        /// <exception cref="IOException">This file is being used by another process.</exception>
        /// <exception cref="UnauthorizedAccessException">The caller does not have the required permission.</exception>
        public override FileAttributes GetAttributes(string path)
        {
            if (path != null)
            {
                if (path.Length == 0)
                {
                    throw new ArgumentException(StringResources.Manager.GetString("THE_PATH_IS_NOT_OF_A_LEGAL_FORM"), "path");
                }
            }

            VirtualFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            var possibleFileData = VirtualFileDataAccessor.GetFile(path);
            FileAttributes result;
            if (possibleFileData != null)
            {
                result = possibleFileData.Attributes;
            }
            else
            {
                var directoryInfo = VirtualFileDataAccessor.DirectoryInfo.FromDirectoryName(path);
                if (directoryInfo.Exists)
                {
                    result = directoryInfo.Attributes;
                }
                else
                {
                    VerifyDirectoryExists(path);

                    throw new FileNotFoundException(string.Format(CultureInfo.InvariantCulture, "Could not find file '{0}'.", path));
                }
            }

            return result;
        }

        public override DateTime GetCreationTime(string path)
        {
            VirtualFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            return GetTimeFromFile(path, data => data.CreationTime.LocalDateTime, () => VirtualFileData.DefaultDateTimeOffset.LocalDateTime);
        }

        public override DateTime GetCreationTimeUtc(string path)
        {
            VirtualFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            return GetTimeFromFile(path, data => data.CreationTime.UtcDateTime, () => VirtualFileData.DefaultDateTimeOffset.UtcDateTime);
        }

        public override DateTime GetLastAccessTime(string path)
        {
            VirtualFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            return GetTimeFromFile(path, data => data.LastAccessTime.LocalDateTime, () => VirtualFileData.DefaultDateTimeOffset.LocalDateTime);
        }

        public override DateTime GetLastAccessTimeUtc(string path)
        {
            VirtualFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            return GetTimeFromFile(path, data => data.LastAccessTime.UtcDateTime, () => VirtualFileData.DefaultDateTimeOffset.UtcDateTime);
        }

        public override DateTime GetLastWriteTime(string path) {
            VirtualFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            return GetTimeFromFile(path, data => data.LastWriteTime.LocalDateTime, () => VirtualFileData.DefaultDateTimeOffset.LocalDateTime);
        }

        public override DateTime GetLastWriteTimeUtc(string path)
        {
            VirtualFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            return GetTimeFromFile(path, data => data.LastWriteTime.UtcDateTime, () => VirtualFileData.DefaultDateTimeOffset.UtcDateTime);
        }

        private DateTime GetTimeFromFile(string path, Func<VirtualFileData, DateTime> existingFileFunction, Func<DateTime> nonExistingFileFunction)
        {
            DateTime result;
            VirtualFileData file = VirtualFileDataAccessor.GetFile(path);
            if (file != null)
            {
                result = existingFileFunction(file);
            }
            else
            {
                result = nonExistingFileFunction();
            }

            return result;
        }

        public override void Move(string sourceFileName, string destFileName)
        {
            if (sourceFileName == null)
            {
                throw new ArgumentNullException("sourceFileName", StringResources.Manager.GetString("FILENAME_CANNOT_BE_NULL"));
            }

            if (destFileName == null)
            {
                throw new ArgumentNullException("destFileName", StringResources.Manager.GetString("FILENAME_CANNOT_BE_NULL"));
            }

            VirtualFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(sourceFileName, "sourceFileName");
            VirtualFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(destFileName, "destFileName");

            if (VirtualFileDataAccessor.GetFile(destFileName) != null)
                throw new IOException("A file can not be created if it already exists.");

            var sourceFile = VirtualFileDataAccessor.GetFile(sourceFileName);

            if (sourceFile == null)
                throw new FileNotFoundException(string.Format(CultureInfo.InvariantCulture, "The file \"{0}\" could not be found.", sourceFileName), sourceFileName);

            VerifyDirectoryExists(destFileName);

            VirtualFileDataAccessor.AddFile(destFileName, new VirtualFileData(sourceFile.Contents));
            VirtualFileDataAccessor.RemoveFile(sourceFileName);
        }

        public override Stream Open(string path, FileMode mode)
        {
            VirtualFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            return Open(path, mode, (mode == FileMode.Append ? FileAccess.Write : FileAccess.ReadWrite), FileShare.None);
        }

        public override Stream Open(string path, FileMode mode, FileAccess access)
        {
            VirtualFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            return Open(path, mode, access, FileShare.None);
        }

        public override Stream Open(string path, FileMode mode, FileAccess access, FileShare share)
        {
            VirtualFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            bool exists = VirtualFileDataAccessor.FileExists(path);

            if (mode == FileMode.CreateNew && exists)
                throw new IOException(string.Format(CultureInfo.InvariantCulture, "The file '{0}' already exists.", path));

            if ((mode == FileMode.Open || mode == FileMode.Truncate) && !exists)
                throw new FileNotFoundException(path);

            if (!exists || mode == FileMode.CreateNew)
                return Create(path);

            if (mode == FileMode.Create || mode == FileMode.Truncate)
            {
                Delete(path);
                return Create(path);
            }

            var length = VirtualFileDataAccessor.GetFile(path).Contents.Length;
            var stream = OpenWrite(path);

            if (mode == FileMode.Append)
                stream.Seek(length, SeekOrigin.Begin);

            return stream;
        }

        public override Stream OpenRead(string path)
        {
            VirtualFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            return Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        public override StreamReader OpenText(string path)
        {
            VirtualFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            return new StreamReader(
                OpenRead(path));
        }

        public override Stream OpenWrite(string path)
        {
            VirtualFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            return new VirtualFileStream(VirtualFileDataAccessor, path, VirtualFileStream.StreamType.WRITE);
        }

        public override byte[] ReadAllBytes(string path)
        {
            VirtualFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            return VirtualFileDataAccessor.GetFile(path).Contents;
        }

        public override string[] ReadAllLines(string path)
        {
            VirtualFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            if (!VirtualFileDataAccessor.FileExists(path))
            {
                throw new FileNotFoundException(string.Format(CultureInfo.InvariantCulture, "Can't find {0}", path));
            }

            return VirtualFileDataAccessor
                .GetFile(path)
                .TextContents
                .SplitLines();
        }

        public override string[] ReadAllLines(string path, Encoding encoding)
        {
            VirtualFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }

            if (!VirtualFileDataAccessor.FileExists(path))
            {
                throw new FileNotFoundException(string.Format(CultureInfo.InvariantCulture, "Can't find {0}", path));
            }

            return encoding
                .GetString(VirtualFileDataAccessor.GetFile(path).Contents)
                .SplitLines();
        }

        public override string ReadAllText(string path)
        {
            VirtualFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            if (!VirtualFileDataAccessor.FileExists(path))
            {
                throw new FileNotFoundException(string.Format(CultureInfo.InvariantCulture, "Can't find {0}", path));
            }

            return ReadAllText(path, VirtualFileData.DefaultEncoding);
        }

        public override string ReadAllText(string path, Encoding encoding)
        {
            VirtualFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }

            return ReadAllTextInternal(path, encoding);
        }

        public override IEnumerable<string> ReadLines(string path)
        {
            VirtualFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            return ReadAllLines(path);
        }

        public override IEnumerable<string> ReadLines(string path, Encoding encoding)
        {
            VirtualFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");
            VerifyValueIsNotNull(encoding, "encoding");

            return ReadAllLines(path, encoding);
        }

        public override void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName)
        {
            throw new NotImplementedException(StringResources.Manager.GetString("NOT_IMPLEMENTED_EXCEPTION"));
        }

        public override void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors)
        {
            throw new NotImplementedException(StringResources.Manager.GetString("NOT_IMPLEMENTED_EXCEPTION"));
        }

        public override void SetAccessControl(string path, FileSecurity fileSecurity)
        {
            VirtualFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            if (!VirtualFileDataAccessor.FileExists(path))
            {
                throw new FileNotFoundException(string.Format(CultureInfo.InvariantCulture, "Can't find {0}", path), path);
            }

            var fileData = VirtualFileDataAccessor.GetFile(path);
            fileData.AccessControl = fileSecurity;
        }

        public override void SetAttributes(string path, FileAttributes fileAttributes)
        {
            VirtualFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            VirtualFileDataAccessor.GetFile(path).Attributes = fileAttributes;
        }

        public override void SetCreationTime(string path, DateTime creationTime)
        {
            VirtualFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            VirtualFileDataAccessor.GetFile(path).CreationTime = new DateTimeOffset(creationTime);
        }

        public override void SetCreationTimeUtc(string path, DateTime creationTimeUtc)
        {
            VirtualFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            VirtualFileDataAccessor.GetFile(path).CreationTime = new DateTimeOffset(creationTimeUtc, TimeSpan.Zero);
        }

        public override void SetLastAccessTime(string path, DateTime lastAccessTime)
        {
            VirtualFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            VirtualFileDataAccessor.GetFile(path).LastAccessTime = new DateTimeOffset(lastAccessTime);
        }

        public override void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
        {
            VirtualFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            VirtualFileDataAccessor.GetFile(path).LastAccessTime = new DateTimeOffset(lastAccessTimeUtc, TimeSpan.Zero);
        }

        public override void SetLastWriteTime(string path, DateTime lastWriteTime)
        {
            VirtualFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            VirtualFileDataAccessor.GetFile(path).LastWriteTime = new DateTimeOffset(lastWriteTime);
        }

        public override void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
        {
            VirtualFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            VirtualFileDataAccessor.GetFile(path).LastWriteTime = new DateTimeOffset(lastWriteTimeUtc, TimeSpan.Zero);
        }

        /// <summary>
        /// Creates a new file, writes the specified byte array to the file, and then closes the file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="path">The file to write to.</param>
        /// <param name="bytes">The bytes to write to the file. </param>
        /// <exception cref="ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="Path.GetInvalidPathChars"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is <see langword="null"/> or contents is empty.</exception>
        /// <exception cref="PathTooLongException">
        /// The specified path, file name, or both exceed the system-defined maximum length.
        /// For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
        /// <exception cref="IOException">An I/O error occurred while opening the file.</exception>
        /// <exception cref="UnauthorizedAccessException">
        /// path specified a file that is read-only.
        /// -or-
        /// This operation is not supported on the current platform.
        /// -or-
        /// path specified a directory.
        /// -or-
        /// The caller does not have the required permission.
        /// </exception>
        /// <exception cref="FileNotFoundException">The file specified in <paramref name="path"/> was not found.</exception>
        /// <exception cref="NotSupportedException"><paramref name="path"/> is in an invalid format.</exception>
        /// <exception cref="System.Security.SecurityException">The caller does not have the required permission.</exception>
        /// <remarks>
        /// Given a byte array and a file path, this method opens the specified file, writes the contents of the byte array to the file, and then closes the file.
        /// </remarks>
        public override void WriteAllBytes(string path, byte[] bytes)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path", "Path cannot be null.");
            }

            VerifyValueIsNotNull(bytes, "bytes");

            VerifyDirectoryExists(path);

            VirtualFileDataAccessor.AddFile(path, new VirtualFileData(bytes));
        }

       /// <summary>
        /// Creates a new file, writes a collection of strings to the file, and then closes the file.
        /// </summary>
        /// <param name="path">The file to write to.</param>
        /// <param name="contents">The lines to write to the file.</param>
        /// <exception cref="ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="Path.GetInvalidPathChars"/>.</exception>
        /// <exception cref="ArgumentNullException">Either <paramref name="path"/> or <paramref name="contents"/> is <see langword="null"/>.</exception>
        /// <exception cref="DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
        /// <exception cref="FileNotFoundException">The file specified in <paramref name="path"/> was not found.</exception>
        /// <exception cref="IOException">An I/O error occurred while opening the file.</exception>
        /// <exception cref="PathTooLongException">
        /// The specified path, file name, or both exceed the system-defined maximum length.
        /// For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.
        /// </exception>
        /// <exception cref="NotSupportedException"><paramref name="path"/> is in an invalid format.</exception>
        /// <exception cref="System.Security.SecurityException">The caller does not have the required permission.</exception>
        /// <exception cref="UnauthorizedAccessException">
        /// <paramref name="path"/> specified a file that is read-only.
        /// -or-
        /// This operation is not supported on the current platform.
        /// -or-
        /// <paramref name="path"/> specified a directory.
        /// -or-
        /// The caller does not have the required permission.
        /// </exception>
        /// <remarks>
        /// <para>
        ///     If the target file already exists, it is overwritten.
        /// </para>
        /// <para>
        ///     You can use this method to create the contents for a collection class that takes an <see cref="IEnumerable{T}"/> in its constructor, such as a <see cref="List{T}"/>, <see cref="HashSet{T}"/>, or a <see cref="SortedSet{T}"/> class.
        /// </para>
        /// </remarks>
        public override void WriteAllLines(string path, IEnumerable<string> contents)
        {
            VirtualFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");
            VerifyValueIsNotNull(contents, "contents");

            WriteAllLines(path, contents, VirtualFileData.DefaultEncoding);
        }

        /// <summary>
        /// Creates a new file by using the specified encoding, writes a collection of strings to the file, and then closes the file.
        /// </summary>
        /// <param name="path">The file to write to.</param>
        /// <param name="contents">The lines to write to the file.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <exception cref="ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="Path.GetInvalidPathChars"/>.</exception>
        /// <exception cref="ArgumentNullException">Either <paramref name="path"/>, <paramref name="contents"/>, or <paramref name="encoding"/> is <see langword="null"/>.</exception>
        /// <exception cref="DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
        /// <exception cref="FileNotFoundException">The file specified in <paramref name="path"/> was not found.</exception>
        /// <exception cref="IOException">An I/O error occurred while opening the file.</exception>
        /// <exception cref="PathTooLongException">
        /// The specified path, file name, or both exceed the system-defined maximum length.
        /// For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.
        /// </exception>
        /// <exception cref="NotSupportedException"><paramref name="path"/> is in an invalid format.</exception>
        /// <exception cref="System.Security.SecurityException">The caller does not have the required permission.</exception>
        /// <exception cref="UnauthorizedAccessException">
        /// <paramref name="path"/> specified a file that is read-only.
        /// -or-
        /// This operation is not supported on the current platform.
        /// -or-
        /// <paramref name="path"/> specified a directory.
        /// -or-
        /// The caller does not have the required permission.
        /// </exception>
        /// <remarks>
        /// <para>
        ///     If the target file already exists, it is overwritten.
        /// </para>
        /// <para>
        ///     You can use this method to create a file that contains the following:
        /// <list type="bullet">
        /// <item>
        /// <description>The results of a LINQ to Objects query on the lines of a file, as obtained by using the ReadLines method.</description>
        /// </item>
        /// <item>
        /// <description>The contents of a collection that implements an <see cref="IEnumerable{T}"/> of strings.</description>
        /// </item>
        /// </list>
        /// </para>
        /// </remarks>
        public override void WriteAllLines(string path, IEnumerable<string> contents, Encoding encoding)
        {
            VirtualFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");
            VerifyValueIsNotNull(contents, "contents");
            VerifyValueIsNotNull(encoding, "encoding");

            var sb = new StringBuilder();
            foreach (var line in contents)
            {
                sb.AppendLine(line);
            }

            WriteAllText(path, sb.ToString(), encoding);
        }

        /// <summary>
        /// Creates a new file, writes the specified string array to the file by using the specified encoding, and then closes the file.
        /// </summary>
        /// <param name="path">The file to write to.</param>
        /// <param name="contents">The string array to write to the file.</param>
        /// <exception cref="ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="Path.GetInvalidPathChars"/>.</exception>
        /// <exception cref="ArgumentNullException">Either <paramref name="path"/> or <paramref name="contents"/> is <see langword="null"/>.</exception>
        /// <exception cref="PathTooLongException">
        /// The specified path, file name, or both exceed the system-defined maximum length.
        /// For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
        /// <exception cref="IOException">An I/O error occurred while opening the file.</exception>
        /// <exception cref="UnauthorizedAccessException">
        /// <paramref name="path"/> specified a file that is read-only.
        /// -or-
        /// This operation is not supported on the current platform.
        /// -or-
        /// <paramref name="path"/> specified a directory.
        /// -or-
        /// The caller does not have the required permission.
        /// </exception>
        /// <exception cref="FileNotFoundException">The file specified in <paramref name="path"/> was not found.</exception>
        /// <exception cref="NotSupportedException"><paramref name="path"/> is in an invalid format.</exception>
        /// <exception cref="System.Security.SecurityException">The caller does not have the required permission.</exception>
        /// <remarks>
        /// <para>
        ///     If the target file already exists, it is overwritten.
        /// </para>
        /// <para>
        ///     The default behavior of the WriteAllLines method is to write out data using UTF-8 encoding without a byte order mark (BOM). If it is necessary to include a UTF-8 identifier, such as a byte order mark, at the beginning of a file, use the <see cref="FileBase.WriteAllLines(string,string[],System.Text.Encoding)"/> method overload with <see cref="UTF8Encoding"/> encoding.
        /// </para>
        /// <para>
        ///     Given a string array and a file path, this method opens the specified file, writes the string array to the file using the specified encoding,
        ///     and then closes the file.
        /// </para>
        /// </remarks>
        public override void WriteAllLines(string path, string[] contents)
        {
            VirtualFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");
            VerifyValueIsNotNull(contents, "contents");

            WriteAllLines(path, contents, VirtualFileData.DefaultEncoding);
        }

        /// <summary>
        /// Creates a new file, writes the specified string array to the file by using the specified encoding, and then closes the file.
        /// </summary>
        /// <param name="path">The file to write to.</param>
        /// <param name="contents">The string array to write to the file.</param>
        /// <param name="encoding">An <see cref="Encoding"/> object that represents the character encoding applied to the string array.</param>
        /// <exception cref="ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="Path.GetInvalidPathChars"/>.</exception>
        /// <exception cref="ArgumentNullException">Either <paramref name="path"/> or <paramref name="contents"/> is <see langword="null"/>.</exception>
        /// <exception cref="PathTooLongException">
        /// The specified path, file name, or both exceed the system-defined maximum length.
        /// For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
        /// <exception cref="IOException">An I/O error occurred while opening the file.</exception>
        /// <exception cref="UnauthorizedAccessException">
        /// <paramref name="path"/> specified a file that is read-only.
        /// -or-
        /// This operation is not supported on the current platform.
        /// -or-
        /// <paramref name="path"/> specified a directory.
        /// -or-
        /// The caller does not have the required permission.
        /// </exception>
        /// <exception cref="FileNotFoundException">The file specified in <paramref name="path"/> was not found.</exception>
        /// <exception cref="NotSupportedException"><paramref name="path"/> is in an invalid format.</exception>
        /// <exception cref="System.Security.SecurityException">The caller does not have the required permission.</exception>
        /// <remarks>
        /// <para>
        ///     If the target file already exists, it is overwritten.
        /// </para>
        /// <para>
        ///     Given a string array and a file path, this method opens the specified file, writes the string array to the file using the specified encoding,
        ///     and then closes the file.
        /// </para>
        /// </remarks>
        public override void WriteAllLines(string path, string[] contents, Encoding encoding)
        {
            VirtualFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");
            VerifyValueIsNotNull(contents, "contents");
            VerifyValueIsNotNull(encoding, "encoding");

            WriteAllLines(path, new List<string>(contents), encoding);
        }

        /// <summary>
        /// Creates a new file, writes the specified string to the file using the specified encoding, and then closes the file. If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="path">The file to write to. </param>
        /// <param name="contents">The string to write to the file. </param>
        /// <exception cref="ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="Path.GetInvalidPathChars"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is <see langword="null"/> or contents is empty.</exception>
        /// <exception cref="PathTooLongException">
        /// The specified path, file name, or both exceed the system-defined maximum length.
        /// For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
        /// <exception cref="IOException">An I/O error occurred while opening the file.</exception>
        /// <exception cref="UnauthorizedAccessException">
        /// path specified a file that is read-only.
        /// -or-
        /// This operation is not supported on the current platform.
        /// -or-
        /// path specified a directory.
        /// -or-
        /// The caller does not have the required permission.
        /// </exception>
        /// <exception cref="FileNotFoundException">The file specified in <paramref name="path"/> was not found.</exception>
        /// <exception cref="NotSupportedException"><paramref name="path"/> is in an invalid format.</exception>
        /// <exception cref="System.Security.SecurityException">The caller does not have the required permission.</exception>
        /// <remarks>
        /// This method uses UTF-8 encoding without a Byte-Order Mark (BOM), so using the <see cref="M:Encoding.GetPreamble"/> method will return an empty byte array.
        /// If it is necessary to include a UTF-8 identifier, such as a byte order mark, at the beginning of a file, use the <see cref="FileBase.WriteAllText(string,string,System.Text.Encoding)"/> method overload with <see cref="UTF8Encoding"/> encoding.
        /// <para>
        /// Given a string and a file path, this method opens the specified file, writes the string to the file, and then closes the file.
        /// </para>
        /// </remarks>
        public override void WriteAllText(string path, string contents)
        {
            WriteAllText(path, contents, VirtualFileData.DefaultEncoding);
        }

        /// <summary>
        /// Creates a new file, writes the specified string to the file using the specified encoding, and then closes the file. If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="path">The file to write to. </param>
        /// <param name="contents">The string to write to the file. </param>
        /// <param name="encoding">The encoding to apply to the string.</param>
        /// <exception cref="ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="Path.GetInvalidPathChars"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is <see langword="null"/> or contents is empty.</exception>
        /// <exception cref="PathTooLongException">
        /// The specified path, file name, or both exceed the system-defined maximum length.
        /// For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
        /// <exception cref="IOException">An I/O error occurred while opening the file.</exception>
        /// <exception cref="UnauthorizedAccessException">
        /// path specified a file that is read-only.
        /// -or-
        /// This operation is not supported on the current platform.
        /// -or-
        /// path specified a directory.
        /// -or-
        /// The caller does not have the required permission.
        /// </exception>
        /// <exception cref="FileNotFoundException">The file specified in <paramref name="path"/> was not found.</exception>
        /// <exception cref="NotSupportedException"><paramref name="path"/> is in an invalid format.</exception>
        /// <exception cref="System.Security.SecurityException">The caller does not have the required permission.</exception>
        /// <remarks>
        /// Given a string and a file path, this method opens the specified file, writes the string to the file using the specified encoding, and then closes the file.
        /// The file handle is guaranteed to be closed by this method, even if exceptions are raised.
        /// </remarks>
        public override void WriteAllText(string path, string contents, Encoding encoding)
        {
            VirtualFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");
            VerifyValueIsNotNull(path, "path");

            if (VirtualFileDataAccessor.Directory.Exists(path))
            {
                throw new UnauthorizedAccessException(string.Format(CultureInfo.InvariantCulture, StringResources.Manager.GetString("ACCESS_TO_THE_PATH_IS_DENIED"), path));
            }

            VerifyDirectoryExists(path);
     
            VirtualFileData data = contents == null ? new VirtualFileData(new byte[0]) : new VirtualFileData(contents, encoding);
            VirtualFileDataAccessor.AddFile(path, data);
        }

        internal static string ReadAllBytes(byte[] contents, Encoding encoding)
        {
            using (var ms = new MemoryStream(contents))
            using (var sr = new StreamReader(ms, encoding))
            {
                return sr.ReadToEnd();
            }
        }

        private string ReadAllTextInternal(string path, Encoding encoding)
        {
            var VirtualFileData = VirtualFileDataAccessor.GetFile(path);
            return ReadAllBytes(VirtualFileData.Contents, encoding);
        }

        private void VerifyValueIsNotNull(object value, string parameterName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName, StringResources.Manager.GetString("VALUE_CANNOT_BE_NULL"));
            }
        }

        private void VerifyDirectoryExists(string path)
        {
            DirectoryInfoBase dir = VirtualFileDataAccessor.Directory.GetParent(path);
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(string.Format(CultureInfo.InvariantCulture, StringResources.Manager.GetString("COULD_NOT_FIND_PART_OF_PATH_EXCEPTION"), dir));
            }
        }
    }
}
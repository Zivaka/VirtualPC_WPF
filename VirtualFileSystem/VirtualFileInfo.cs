using System;
using System.IO;
using System.IO.Abstractions;
using System.Security.AccessControl;

namespace VirtualFileSystem
{
    [Serializable]
    public class VirtualFileInfo : FileInfoBase
    {
        private readonly IVirtualFileDataAccessor VirtualFileSystem;
        private string path;

        public VirtualFileInfo(IVirtualFileDataAccessor VirtualFileSystem, string path)
        {
            if (VirtualFileSystem == null)
            {
                throw new ArgumentNullException("VirtualFileSystem");
            }

            this.VirtualFileSystem = VirtualFileSystem;
            this.path = path;
        }

        VirtualFileData VirtualFileData
        {
            get { return VirtualFileSystem.GetFile(path); }
        }

        public override void Delete()
        {
            VirtualFileSystem.RemoveFile(path);
        }

        public override void Refresh()
        {
        }

        public override FileAttributes Attributes
        {
            get
            {
                if (VirtualFileData == null)
                    throw new FileNotFoundException("File not found", path);
                return VirtualFileData.Attributes;
            }
            set { VirtualFileData.Attributes = value; }
        }

        public override DateTime CreationTime
        {
            get
            {
                if (VirtualFileData == null) throw new FileNotFoundException("File not found", path);
                return VirtualFileData.CreationTime.DateTime;
            }
            set
            {
                if (VirtualFileData == null) throw new FileNotFoundException("File not found", path);
                VirtualFileData.CreationTime = value;
            }
        }

        public override DateTime CreationTimeUtc
        {
            get
            {
                if (VirtualFileData == null) throw new FileNotFoundException("File not found", path);
                return VirtualFileData.CreationTime.UtcDateTime;
            }
            set
            {
                if (VirtualFileData == null) throw new FileNotFoundException("File not found", path);
                VirtualFileData.CreationTime = value.ToLocalTime();
            }
        }

        public override bool Exists
        {
            get { return VirtualFileData != null; }
        }

        public override string Extension
        {
            get
            {
                // System.IO.Path.GetExtension does only string manipulation,
                // so it's safe to delegate.
                return Path.GetExtension(path);
            }
        }

        public override string FullName
        {
            get { return path; }
        }

        public override DateTime LastAccessTime
        {
            get
            {
                if (VirtualFileData == null) throw new FileNotFoundException("File not found", path);
                return VirtualFileData.LastAccessTime.DateTime;
            }
            set
            {
                if (VirtualFileData == null) throw new FileNotFoundException("File not found", path);
                VirtualFileData.LastAccessTime = value;
            }
        }

        public override DateTime LastAccessTimeUtc
        {
            get
            {
                if (VirtualFileData == null) throw new FileNotFoundException("File not found", path);
                return VirtualFileData.LastAccessTime.UtcDateTime;
            }
            set
            {
                if (VirtualFileData == null) throw new FileNotFoundException("File not found", path);
                VirtualFileData.LastAccessTime = value;
            }
        }

        public override DateTime LastWriteTime
        {
            get
            {
                if (VirtualFileData == null) throw new FileNotFoundException("File not found", path);
                return VirtualFileData.LastWriteTime.DateTime;
            }
            set
            {
                if (VirtualFileData == null) throw new FileNotFoundException("File not found", path);
                VirtualFileData.LastWriteTime = value;
            }
        }

        public override DateTime LastWriteTimeUtc
        {
            get
            {
                if (VirtualFileData == null) throw new FileNotFoundException("File not found", path);
                return VirtualFileData.LastWriteTime.UtcDateTime;
            }
            set
            {
                if (VirtualFileData == null) throw new FileNotFoundException("File not found", path);
                VirtualFileData.LastWriteTime = value.ToLocalTime();
            }
        }

        public override string Name {
            get { return new VirtualPath(VirtualFileSystem).GetFileName(path); }
        }

        public override StreamWriter AppendText()
        {
            if (VirtualFileData == null) throw new FileNotFoundException("File not found", path);
            return new StreamWriter(new VirtualFileStream(VirtualFileSystem, FullName, VirtualFileStream.StreamType.APPEND));
            //return ((VirtualFileDataModifier) VirtualFileData).AppendText();
        }

        public override FileInfoBase CopyTo(string destFileName)
        {
            new VirtualFile(VirtualFileSystem).Copy(FullName, destFileName);
            return VirtualFileSystem.FileInfo.FromFileName(destFileName);
        }

        public override FileInfoBase CopyTo(string destFileName, bool overwrite)
        {
            new VirtualFile(VirtualFileSystem).Copy(FullName, destFileName, overwrite);
            return VirtualFileSystem.FileInfo.FromFileName(destFileName);
        }

        public override Stream Create()
        {
            return new VirtualFile(VirtualFileSystem).Create(FullName);
        }

        public override StreamWriter CreateText()
        {
            return new VirtualFile(VirtualFileSystem).CreateText(FullName);
        }

        public override void Decrypt()
        {
            if (VirtualFileData == null) throw new FileNotFoundException("File not found", path);
            var contents = VirtualFileData.Contents;
            for (var i = 0; i < contents.Length; i++)
                contents[i] ^= (byte)(i % 256);
        }

        public override void Encrypt()
        {
            if (VirtualFileData == null) throw new FileNotFoundException("File not found", path);
            var contents = VirtualFileData.Contents;
            for(var i = 0; i < contents.Length; i++)
                contents[i] ^= (byte) (i % 256);
        }

        public override FileSecurity GetAccessControl()
        {
            throw new NotImplementedException(StringResources.Manager.GetString("NOT_IMPLEMENTED_EXCEPTION"));
        }

        public override FileSecurity GetAccessControl(AccessControlSections includeSections)
        {
            throw new NotImplementedException(StringResources.Manager.GetString("NOT_IMPLEMENTED_EXCEPTION"));
        }

        public override void MoveTo(string destFileName)
        {
            var movedFileInfo = CopyTo(destFileName);
            Delete();
            path = movedFileInfo.FullName;
        }

        public override Stream Open(FileMode mode)
        {
            return new VirtualFile(VirtualFileSystem).Open(FullName, mode);
        }

        public override Stream Open(FileMode mode, FileAccess access)
        {
            return new VirtualFile(VirtualFileSystem).Open(FullName, mode, access);
        }

        public override Stream Open(FileMode mode, FileAccess access, FileShare share)
        {
            return new VirtualFile(VirtualFileSystem).Open(FullName, mode, access, share);
        }

        public override Stream OpenRead()
        {
            if (VirtualFileData == null) throw new FileNotFoundException("File not found", path);
            return new VirtualFileStream(VirtualFileSystem, path, VirtualFileStream.StreamType.READ);
        }

        public override StreamReader OpenText()
        {
          return new StreamReader(OpenRead());
        }

        public override Stream OpenWrite()
        {
            return new VirtualFileStream(VirtualFileSystem, path, VirtualFileStream.StreamType.WRITE);
        }

        public override FileInfoBase Replace(string destinationFileName, string destinationBackupFileName)
        {
            throw new NotImplementedException(StringResources.Manager.GetString("NOT_IMPLEMENTED_EXCEPTION"));
        }

        public override FileInfoBase Replace(string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors)
        {
            throw new NotImplementedException(StringResources.Manager.GetString("NOT_IMPLEMENTED_EXCEPTION"));
        }

        public override void SetAccessControl(FileSecurity fileSecurity)
        {
            throw new NotImplementedException(StringResources.Manager.GetString("NOT_IMPLEMENTED_EXCEPTION"));
        }

        public override DirectoryInfoBase Directory
        {
            get
            {
                return VirtualFileSystem.DirectoryInfo.FromDirectoryName(DirectoryName);
            }
        }

        public override string DirectoryName
        {
            get
            {
                // System.IO.Path.GetDirectoryName does only string manipulation,
                // so it's safe to delegate.
                return Path.GetDirectoryName(path);
            }
        }

        public override bool IsReadOnly
        {
            get
            {
                if (VirtualFileData == null) throw new FileNotFoundException("File not found", path);
                return (VirtualFileData.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly;
            }
            set
            {
                if (VirtualFileData == null) throw new FileNotFoundException("File not found", path);
                if(value)
                    VirtualFileData.Attributes |= FileAttributes.ReadOnly;
                else
                    VirtualFileData.Attributes &= ~FileAttributes.ReadOnly;
            }
        }

        public override long Length
        {
            get
            {
                if (VirtualFileData == null) throw new FileNotFoundException("File not found", path);
                return VirtualFileData.Contents.Length;
            }
        }
    }
}
using System;
using System.IO.Abstractions;

namespace VirtualFileSystem
{
    [Serializable]
    public class VirtualDirectoryInfoFactory : IDirectoryInfoFactory
    {
        readonly IVirtualFileDataAccessor VirtualFileSystem;

        public VirtualDirectoryInfoFactory(IVirtualFileDataAccessor VirtualFileSystem)
        {
            this.VirtualFileSystem = VirtualFileSystem;
        }

        public DirectoryInfoBase FromDirectoryName(string directoryName)
        {
            return new VirtualDirectoryInfo(VirtualFileSystem, directoryName);
        }
    }
}
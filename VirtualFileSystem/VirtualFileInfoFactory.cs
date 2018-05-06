using System;
using System.IO.Abstractions;

namespace VirtualFileSystem
{
    [Serializable]
    public class VirtualFileInfoFactory : IFileInfoFactory
    {
        private readonly IVirtualFileDataAccessor VirtualFileSystem;

        public VirtualFileInfoFactory(IVirtualFileDataAccessor VirtualFileSystem)
        {
            if (VirtualFileSystem == null)
            {
                throw new ArgumentNullException("VirtualFileSystem");
            }

            this.VirtualFileSystem = VirtualFileSystem;
        }

        public FileInfoBase FromFileName(string fileName)
        {
            return new VirtualFileInfo(VirtualFileSystem, fileName);
        }
    }
}
using System;
using System.IO;

namespace VirtualFileSystem
{
    [Serializable]
    public class VirtualFileStream : MemoryStream
    {
        private readonly IVirtualFileDataAccessor VirtualFileDataAccessor;
        private readonly string path;

        public enum StreamType
        {
            READ,
            WRITE,
            APPEND
        }

        public VirtualFileStream(IVirtualFileDataAccessor VirtualFileDataAccessor, string path, StreamType streamType)
        {
            if (VirtualFileDataAccessor == null)
            {
                throw new ArgumentNullException("VirtualFileDataAccessor");
            }

            this.VirtualFileDataAccessor = VirtualFileDataAccessor;
            this.path = path;

            if (VirtualFileDataAccessor.FileExists(path))
            {
                /* only way to make an expandable MemoryStream that starts with a particular content */
                var data = VirtualFileDataAccessor.GetFile(path).Contents;
                if (data != null && data.Length > 0)
                {
                    Write(data, 0, data.Length);
                    Seek(0, StreamType.APPEND.Equals(streamType)
                        ? SeekOrigin.End
                        : SeekOrigin.Begin);
                }
            }
            else
            {
                if (StreamType.READ.Equals(streamType))
                {
                    throw new FileNotFoundException("File not found.", path);
                }
                VirtualFileDataAccessor.AddFile(path, new VirtualFileData(new byte[] { }));
            }
        }

#if NET40
        public override void Close()
        {
            InternalFlush();
        }
#else
        protected override void Dispose(bool disposing)
        {
            InternalFlush();
            base.Dispose(disposing);
        }
#endif

        public override void Flush()
        {
            InternalFlush();
        }

        private void InternalFlush()
        {
            if (VirtualFileDataAccessor.FileExists(path))
            {
                var VirtualFileData = VirtualFileDataAccessor.GetFile(path);
                /* reset back to the beginning .. */
                Seek(0, SeekOrigin.Begin);
                /* .. read everything out */
                var data = new byte[Length];
                Read(data, 0, (int)Length);
                /* .. put it in the Virtual system */
                VirtualFileData.Contents = data;
            }
        }
    }
}
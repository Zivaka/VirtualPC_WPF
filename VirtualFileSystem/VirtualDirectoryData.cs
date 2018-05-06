using System;
using System.IO;
using System.Security.AccessControl;

namespace VirtualFileSystem
{
    [Serializable]
    public class VirtualDirectoryData : VirtualFileData
    {
        [NonSerialized]
        private DirectorySecurity accessControl = new DirectorySecurity();
        
        public override bool IsDirectory { get { return true; } }

        public VirtualDirectoryData() : base(string.Empty)
        {
            Attributes = FileAttributes.Directory;
        }
        
        public new DirectorySecurity AccessControl
        {
            get { return accessControl; }
            set { accessControl = value; }
        }
    }
}
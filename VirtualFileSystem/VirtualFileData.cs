﻿using System;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Security.AccessControl;
using System.Text;

namespace VirtualFileSystem
{
    /// <summary>
    /// The class represents the associated data of a file.
    /// </summary>
    [Serializable]
    public class VirtualFileData
    {
        /// <summary>
        /// The default encoding.
        /// </summary>
        public static readonly Encoding DefaultEncoding = new UTF8Encoding(false, true);

        /// <summary>
        /// The null object.
        /// </summary>
        public static readonly VirtualFileData NullObject = new VirtualFileData(string.Empty)
        {
          LastWriteTime = new DateTime(1601, 01, 01, 00, 00, 00, DateTimeKind.Utc),
          LastAccessTime = new DateTime(1601, 01, 01, 00, 00, 00, DateTimeKind.Utc),
          CreationTime = new DateTime(1601, 01, 01, 00, 00, 00, DateTimeKind.Utc),
        };

        /// <summary>
        /// Gets the default date time offset.
        /// E.g. for not existing files.
        /// </summary>
        public static readonly DateTimeOffset DefaultDateTimeOffset = new DateTime(1601, 01, 01, 00, 00, 00, DateTimeKind.Utc);

        /// <summary>
        /// The actual contents of the file.
        /// </summary>
        private byte[] contents;

        /// <summary>
        /// The date and time the <see cref="VirtualFileData"/> was created.
        /// </summary>
        private DateTimeOffset creationTime = new DateTimeOffset(2010, 01, 02, 00, 00, 00, TimeSpan.FromHours(4));

        /// <summary>
        /// The date and time of the <see cref="VirtualFileData"/> was last accessed to.
        /// </summary>
        private DateTimeOffset lastAccessTime = new DateTimeOffset(2010, 02, 04, 00, 00, 00, TimeSpan.FromHours(4));

        /// <summary>
        /// The date and time of the <see cref="VirtualFileData"/> was last written to.
        /// </summary>
        private DateTimeOffset lastWriteTime = new DateTimeOffset(2010, 01, 04, 00, 00, 00, TimeSpan.FromHours(4));

        /// <summary>
        /// The attributes of the <see cref="VirtualFileData"/>.
        /// </summary>
        private FileAttributes attributes = FileAttributes.Normal;

        /// <summary>
        /// The access control of the <see cref="VirtualFileData"/>.
        /// </summary>
        [NonSerialized]
        private FileSecurity accessControl = new FileSecurity();

        /// <summary>
        /// Gets a value indicating whether the <see cref="VirtualFileData"/> is a directory or not.
        /// </summary>
        public virtual bool IsDirectory { get { return false; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualFileData"/> class with an empty content.
        /// </summary>
        private VirtualFileData()
        {
            // empty
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualFileData"/> class with the content of <paramref name="textContents"/> using the encoding of <see cref="DefaultEncoding"/>.
        /// </summary>
        /// <param name="textContents">The textual content encoded into bytes with <see cref="DefaultEncoding"/>.</param>
        public VirtualFileData(string textContents)
            : this(DefaultEncoding.GetBytes(textContents))
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualFileData"/> class with the content of <paramref name="textContents"/> using the encoding of <paramref name="encoding"/>.
        /// </summary>
        /// <param name="textContents">The textual content.</param>
        /// <param name="encoding">The specific encoding used the encode the text.</param>
        /// <remarks>The constructor respect the BOM of <paramref name="encoding"/>.</remarks>
        public VirtualFileData(string textContents, Encoding encoding)
            : this()
        {
            contents = encoding.GetPreamble().Concat(encoding.GetBytes(textContents)).ToArray();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualFileData"/> class with the content of <paramref name="contents"/>.
        /// </summary>
        /// <param name="contents">The actual content.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="contents"/> is <see langword="null" />.</exception>
        public VirtualFileData(byte[] contents)
        {
            if (contents == null)
            {
                throw new ArgumentNullException("contents");
            }

            this.contents = contents;
        }

        /// <summary>
        /// Gets or sets the byte contents of the <see cref="VirtualFileData"/>.
        /// </summary>
        public byte[] Contents
        {
            get { return contents; }
            set { contents = value; }
        }

        /// <summary>
        /// Gets or sets the string contents of the <see cref="VirtualFileData"/>.
        /// </summary>
        /// <remarks>
        /// The setter uses the <see cref="DefaultEncoding"/> using this can scramble the actual contents.
        /// </remarks>
        public string TextContents
        {
            get { return VirtualFile.ReadAllBytes(contents, DefaultEncoding); }
            set { contents = DefaultEncoding.GetBytes(value); }
        }

        /// <summary>
        /// Gets or sets the date and time the <see cref="VirtualFileData"/> was created.
        /// </summary>
        public DateTimeOffset CreationTime
        {
            get { return creationTime; }
            set { creationTime = value; }
        }

        /// <summary>
        /// Gets or sets the date and time of the <see cref="VirtualFileData"/> was last accessed to.
        /// </summary>
        public DateTimeOffset LastAccessTime
        {
            get { return lastAccessTime; }
            set { lastAccessTime = value; }
        }

        /// <summary>
        /// Gets or sets the date and time of the <see cref="VirtualFileData"/> was last written to.
        /// </summary>
        public DateTimeOffset LastWriteTime
        {
            get { return lastWriteTime; }
            set { lastWriteTime = value; }
        }

        /// <summary>
        /// Casts a string into <see cref="VirtualFileData"/>.
        /// </summary>
        /// <param name="s">The path of the <see cref="VirtualFileData"/> to be created.</param>
        public static implicit operator VirtualFileData(string s)
        {
            return new VirtualFileData(s);
        }

        /// <summary>
        /// Gets or sets the specified <see cref="FileAttributes"/> of the <see cref="VirtualFileData"/>.
        /// </summary>
        public FileAttributes Attributes
        {
            get { return attributes; }
            set { attributes = value; }
        }

        /// <summary>
        /// Gets or sets <see cref="FileSecurity"/> of the <see cref="VirtualFileData"/>. This is the object that is returned for this <see cref="VirtualFileData"/> when calling <see cref="FileBase.GetAccessControl(string)"/>.
        /// </summary>
        public FileSecurity AccessControl
        {
            get { return accessControl; }
            set { accessControl = value; }
        }
    }
}
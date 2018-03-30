using RpmReaderNetLib.Extension;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace RpmReaderNet.Section
{
    /// <summary>
    /// Reader a header section
    /// </summary>
    internal class RpmHeaderSection
        : AbstractHeaderSection
    {
        /// <summary>
        ///  The version of the package
        /// </summary>
        public string Version
        {
            get { return _version.Value; }
        }

        /// <summary>
        /// The name of the package
        /// </summary>
        public string Name
        {
            get { return _name.Value; }
        }

        /// <summary>
        /// The release number of the package
        /// </summary>
        public string Release
        {
            get { return _release.Value; }
        }

        /// <summary>
        /// The Serial number of the package
        /// </summary>
        public string Serial
        {
            get { return _serial.Value; }
        }

        /// <summary>
        /// The summary information about the package
        /// </summary>
        public string Summary
        {
            get { return _summary.Value; }
        }

        /// <summary>
        /// The description of the package
        /// </summary>
        public string Description
        {
            get { return _description.Value; }
        }

        /// <summary>
        /// A build time of the package
        /// </summary>
        public DateTime? BuildDateTime
        {
            get { return _buildTime.Value; }
        }

        /// <summary>
        /// a host hame where was build package
        /// </summary>
        public string BuildHost
        {
            get { return _buildHost.Value; }
        }

        /// <summary>
        /// A group name of packages, of which this package is a part
        /// </summary>
        public string Distribution
        {
            get { return _distribution.Value; }
        }

        /// <summary>
        /// a name of the entity that is responsible for a package
        /// </summary>
        public string Vendor
        {
            get { return _vendor.Value; }
        }

        /// <summary>
        /// A license of the package
        /// </summary>
        public string License
        {
            get { return _license.Value; }
        }

        /// <summary>
        /// a name and contact information for the person or persons who built the package
        /// </summary>
        public string Packager
        {
            get { return _packager.Value; }
        }

        /// <summary>
        /// a log or record of all or all notable changes made to a package
        /// </summary>
        public string Changelog
        {
            get { return _changeLog.Value; }
        }

        /// <summary>
        /// the time of log or record of all or all notable changes made to a package
        /// </summary>
        public DateTime[] ChangelogTime
        {
            get { return _changeLogTime.Value; }
        }

        ///// <summary>
        /// a log or record of all or all notable changes made to a package
        /// </summary>
        public string[] ChangelogArray
        {
            get { return _changeLogArray.Value; }
        }

        /// <summary>
        /// A name of a log or record of all or all notable changes made to a package
        /// </summary>
        public string[] ChanelogNameArray
        {
            get { return _changelogNameArray.Value; }
        }

        /// <summary>
        /// A original source files name
        /// </summary>
        public string[] Source
        {
            get { return _source.Value; }
        }

        /// <summary>
        /// a name of the source package from which this binary package was built
        /// </summary>
        public string SourceRpm
        {
            get { return _sourceRpm.Value; }
        }

        /// <summary>
        /// the architecture for which the package was built
        /// </summary>
        public string Arch
        {
            get { return _arch.Value; }
        }

        /// <summary>
        /// get content of the package's pre-install script
        /// </summary>
        public string PreinScript
        {
            get { return _preinScript.Value; }
        }

        /// <summary>
        /// get content of the package's post-install script
        /// </summary>
        public string PostinScript
        {
            get { return _postinScript.Value; }
        }

        /// <summary>
        /// get content of the package's pre-uninstall script
        /// </summary>
        public string PreunScript
        {
            get { return _preunScript.Value; }
        }

        /// <summary>
        /// get content of the package's post-uninstall script
        /// </summary>
        public string PostunScript
        {
            get { return _postunScript.Value; }
        }

        /// <summary>
        /// a list of all base names files in package
        /// </summary>
        public string[] BaseFilenames
        {
            get { return _baseFileNames.Value; }
        }

        /// <summary>
        /// a list of all dirs in package
        /// </summary>
        public string[] DirNames
        {
            get { return _dirNames.Value; }
        }

        /// <summary>
        /// Indexes of dir for link between the file name and directory
        /// </summary>
        public uint[] DirIndexes
        {
            get { return _dirIndexes.Value; }
        }

        /// <summary>
        /// a owner, in alphanumeric form, of each of the files that comprise the package
        /// </summary>
        public string[] OwnerUserOfFiles
        {
            get { return _fileUserNames.Value; }
        }

        /// <summary>
        /// a group, in alphanumeric form, of each of the files that comprise the package
        /// </summary>
        public string[] OwnerGroupOfFiles
        {
            get { return _fileGroupNames.Value; }
        }

        /// <summary>
        ///  a MD5 checksum for each of the files that comprise the package
        /// </summary>
        public string[] MD5Files
        {
            get { return _md5Files.Value; }
        }

        /// <summary>
        /// a size, in bytes, of each of the files that comprise the package
        /// </summary>
        public uint FileSize
        {
            get { return _fileSizes.Value; }
        }

        /// <summary>
        /// a list of capabilities the package requires.
        /// </summary>
        public string[] RequiresNames
        {
            get { return _requiresNames.Value; }
        }

        /// <summary>
        /// a total size, in bytes, of every file installed by a package
        /// </summary>
        public uint Size
        {
            get { return _size.Value; }
        }

        private Lazy<string> _name;
        private Lazy<string> _version;
        private Lazy<string> _release;
        private Lazy<string> _serial;
        private Lazy<string> _summary;
        private Lazy<string> _description;
        private Lazy<DateTime?> _buildTime;
        private Lazy<string> _buildHost;
        private Lazy<string> _distribution;
        private Lazy<string> _vendor;
        private Lazy<string> _license;
        private Lazy<string> _packager;
        private Lazy<string> _changeLog;
        private Lazy<DateTime[]> _changeLogTime;
        private Lazy<string[]> _changeLogArray;
        private Lazy<string[]> _changelogNameArray;
        private Lazy<string[]> _source;
        private Lazy<string> _sourceRpm;
        private Lazy<string> _arch;
        private Lazy<string> _preinScript;
        private Lazy<string> _postinScript;
        private Lazy<string> _preunScript;
        private Lazy<string> _postunScript;
        private Lazy<string[]> _baseFileNames;
        private Lazy<string[]> _dirNames;
        private Lazy<string[]> _fileUserNames;
        private Lazy<string[]> _fileGroupNames;
        private Lazy<string[]> _md5Files;
        private Lazy<uint> _fileSizes;
        private Lazy<string[]> _requiresNames;
        private Lazy<uint> _size;
        private Lazy<uint[]> _dirIndexes;

        /// <summary>
        /// Структура заголовка
        /// </summary>
        public RpmStruct.RPMHeader Header = new RpmStruct.RPMHeader();

        public RpmHeaderSection(FileStream file)
            : base(file)
        {
            _version = new Lazy<string>(() => GetStringFromStringTypeTag((int)RpmConstants.rpmTag.RPMTAG_VERSION));
            _name = new Lazy<string>(() => GetStringFromStringTypeTag((int)RpmConstants.rpmTag.RPMTAG_NAME));
            _release = new Lazy<string>(() => GetStringFromStringTypeTag((int)RpmConstants.rpmTag.RPMTAG_RELEASE));
            _serial = new Lazy<string>(() => GetStringFromStringTypeTag((int)RpmConstants.rpmTag.RPMTAG_EPOCH));
            _summary = new Lazy<string>(() => GetI18StringFromTag((int)RpmConstants.rpmTag.RPMTAG_SUMMARY));
            _description = new Lazy<string>(() => GetI18StringFromTag((int)RpmConstants.rpmTag.RPMTAG_DESCRIPTION));
            _buildTime = new Lazy<DateTime?>(GetBuildDateTime);
            _buildHost = new Lazy<string>(() => GetStringFromStringTypeTag((int)RpmConstants.rpmTag.RPMTAG_BUILDHOST));
            _distribution = new Lazy<string>(() => GetStringFromStringTypeTag((int)RpmConstants.rpmTag.RPMTAG_DISTRIBUTION));
            _vendor = new Lazy<string>(() => GetStringFromStringTypeTag((int)RpmConstants.rpmTag.RPMTAG_VENDOR));
            _license = new Lazy<string>(() => GetStringFromStringTypeTag((int)RpmConstants.rpmTag.RPMTAG_LICENSE));
            _packager = new Lazy<string>(() => GetStringFromStringTypeTag((int)RpmConstants.rpmTag.RPMTAG_PACKAGER));
            _changeLog = new Lazy<string>(() => GetStringFromStringTypeTag((int)RpmConstants.rpmTag.RPMTAG_CHANGELOG));
            _changeLogTime = new Lazy<DateTime[]>(() => GetDateTimeArrayFromTag((int)RpmConstants.rpmTag.RPMTAG_CHANGELOGTIME));
            _changeLogArray = new Lazy<string[]>(() => GetStringArrayFromTag((int)RpmConstants.rpmTag.RPMTAG_CHANGELOGTEXT));
            _changelogNameArray = new Lazy<string[]>(() => GetStringArrayFromTag((int)RpmConstants.rpmTag.RPMTAG_CHANGELOGNAME));
            _source = new Lazy<string[]>(() => GetStringArrayFromTag((int)RpmConstants.rpmTag.RPMTAG_SOURCE));
            _sourceRpm = new Lazy<string>(() => GetStringFromStringTypeTag((int)RpmConstants.rpmTag.RPMTAG_SOURCERPM));
            _arch = new Lazy<string>(() => GetStringFromStringTypeTag((int)RpmConstants.rpmTag.RPMTAG_ARCH));
            _preinScript = new Lazy<string>(() => GetStringFromStringTypeTag((int)RpmConstants.rpmTag.RPMTAG_PREIN));
            _postinScript = new Lazy<string>(() => GetStringFromStringTypeTag((int)RpmConstants.rpmTag.RPMTAG_POSTIN));
            _preunScript = new Lazy<string>(() => GetStringFromStringTypeTag((int)RpmConstants.rpmTag.RPMTAG_PREUN));
            _postunScript = new Lazy<string>(() => GetStringFromStringTypeTag((int)RpmConstants.rpmTag.RPMTAG_POSTUN));
            _baseFileNames = new Lazy<string[]>(() => GetStringArrayFromTag((int)RpmConstants.rpmTag.RPMTAG_BASENAMES));
            _dirNames = new Lazy<string[]>(() => GetStringArrayFromTag((int)RpmConstants.rpmTag.RPMTAG_DIRNAMES));
            _fileUserNames = new Lazy<string[]>(() => GetStringArrayFromTag((int)RpmConstants.rpmTag.RPMTAG_FILEUSERNAME));
            _fileGroupNames = new Lazy<string[]>(() => GetStringArrayFromTag((int)RpmConstants.rpmTag.RPMTAG_FILEGROUPNAME));
            _fileSizes = new Lazy<uint>(() => GetInt32FromTag((int)RpmConstants.rpmTag.RPMTAG_FILESIZES));
            _md5Files = new Lazy<string[]>(() => GetStringArrayFromTag((int)RpmConstants.rpmTag.RPMTAG_FILEMD5S));
            _requiresNames = new Lazy<string[]>(() => GetStringArrayFromTag((int)RpmConstants.rpmTag.RPMTAG_REQUIRENAME));
            _size = new Lazy<uint>(() => GetInt32FromTag((int)RpmConstants.rpmTag.RPMTAG_SIZE));
            _dirIndexes = new Lazy<uint[]>(() => GetInt32ArrayFromTag((int)RpmConstants.rpmTag.RPMTAG_DIRINDEXES));
        }

        /// <summary>
        /// Получение позиции первого раздела
        /// </summary>
        /// <returns></returns>
        public override long GetStartPositionFirstEntry()
        {
            return StartPosition + 16 + Header.entryCount * SIZE_ONE_ENTRY;
        }

        public bool FillHeaderData(byte[] data)
        {
            int len = Marshal.SizeOf(Header.GetType());
            IntPtr @in = Marshal.AllocHGlobal(len);
            Marshal.Copy(data, 0, @in, len);
            Header = (RpmStruct.RPMHeader)Marshal.PtrToStructure(@in, Header.GetType());
            Marshal.FreeHGlobal(@in);
            unchecked
            { 
                Header.bytesDataCount = (int)((uint)Header.bytesDataCount).ReverseBytes();
                Header.entryCount = (int)((uint)Header.entryCount).ReverseBytes();
            }
            byte[] buffer = new byte[RpmStruct.RPM_MAGIC_HEADER_NUMBER.Length];
            unsafe
            {
                fixed (byte* ptr = Header.magic)
                {
                    int i = 0;
                    for (byte* d = ptr; i < RpmStruct.RPM_MAGIC_HEADER_NUMBER.Length; ++i, ++d)
                    {
                        buffer[i] = *d;
                    }
                }
            }
            return ByteArrayCompare(buffer, RpmStruct.RPM_MAGIC_HEADER_NUMBER);
        }

        public bool FillHeaderEntry(byte[] data, int countEntry)
        {
            int len = Marshal.SizeOf(typeof(RpmStruct.RPMEntry));
            _entries = new RpmStruct.RPMEntry[countEntry];
            for (int i = 0; i < countEntry; ++i)
            {
                RpmStruct.RPMEntry entry = new RpmStruct.RPMEntry();
                IntPtr @in = Marshal.AllocHGlobal(len);
                Marshal.Copy(data, len * i, @in, len);
                entry = (RpmStruct.RPMEntry)Marshal.PtrToStructure(@in, typeof(RpmStruct.RPMEntry));
                Marshal.FreeHGlobal(@in);
                unchecked
                {
                    entry.Count = entry.Count.ReverseBytes();
                    entry.Offset = entry.Offset.ReverseBytes();
                    entry.Tag = entry.Tag.ReverseBytes();
                    entry.Type = entry.Type.ReverseBytes();
                }
                _entries[i] = entry;
            }
            return true;
        }

        public override string ToString()
        {
            return base.ToString();
        }

        private DateTime? GetBuildDateTime()
        {
            uint dateTimeStr = GetInt32FromTag((int)RpmConstants.rpmTag.RPMTAG_BUILDTIME);
            return dateTimeStr.ToDateTime();
        }        
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace RpmReaderNet.Section
{
    /// <summary>
    /// header-секция
    /// </summary>
    internal class RpmHeaderSection
        : AbstractRpmSection
    {
        /// <summary>
        /// Версия пакета
        /// </summary>
        public string Version
        {
            get { return _version.Value; }
        }

        /// <summary>
        /// Имя пакета
        /// </summary>
        public string Name
        {
            get { return _name.Value; }
        }

        public string Release
        {
            get { return _release.Value; }
        }

        public string Serial
        {
            get { return _serial.Value; }
        }

        public string Summary
        {
            get { return _summary.Value; }
        }

        public string Description
        {
            get { return _description.Value; }
        }

        public DateTime? BuildDateTime
        {
            get { return _buildTime.Value; }
        }

        public string BuildHost
        {
            get { return _buildHost.Value; }
        }

        public string Distribution
        {
            get { return _distribution.Value; }
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

        /// <summary>
        /// Структура заголовка
        /// </summary>
        public RpmStruct.RPMHeader Header = new RpmStruct.RPMHeader();

        // разделы заголовка
        private RpmStruct.RPMEntry[] _entries;

        /// <summary>
        /// размер одного раздела в секции
        /// </summary>
        public readonly long SIZE_ONE_ENTRY = Marshal.SizeOf(typeof(RpmStruct.RPMEntry));

        public RpmHeaderSection(FileStream file)
            : base(file)
        {
            _version = new Lazy<string>(GetVersion);
            _name = new Lazy<string>(GetName);
            _release = new Lazy<string>(GetRelease);
            _serial = new Lazy<string>(GetSerial);
            _summary = new Lazy<string>(GetSummary);
            _description = new Lazy<string>(GetDescription);
            _buildTime = new Lazy<DateTime?>(GetBuildDateTime);
            _buildHost = new Lazy<string>(() => GetStringFromTag(RpmConstants.rpmTag.RPMTAG_BUILDHOST));
            _distribution = new Lazy<string>(() => GetStringFromTag(RpmConstants.rpmTag.RPMTAG_DISTRIBUTION));
        }

        /// <summary>
        /// Получение позиции первого раздела
        /// </summary>
        /// <returns></returns>
        public long GetStartPositionFirstEntry()
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
            Header.bytesDataCount = (int)ReverseBytes((uint)Header.bytesDataCount);
            Header.entryCount = (int)ReverseBytes((uint)Header.entryCount);
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
            for(int i = 0; i < countEntry; ++i)
            {
                RpmStruct.RPMEntry entry = new RpmStruct.RPMEntry();
                IntPtr @in = Marshal.AllocHGlobal(len);
                Marshal.Copy(data, len * i, @in, len);
                entry = (RpmStruct.RPMEntry)Marshal.PtrToStructure(@in, typeof(RpmStruct.RPMEntry));
                Marshal.FreeHGlobal(@in);
                entry.Count = ReverseBytes(entry.Count);
                entry.Offset = ReverseBytes(entry.Offset);
                entry.Tag = ReverseBytes(entry.Tag);
                entry.Type = ReverseBytes(entry.Type);
                _entries[i] = entry;
            }
            return true;
        }

        private string GetVersion()
        {
            return GetStringFromTag(RpmConstants.rpmTag.RPMTAG_VERSION);
        }

        private string GetName()
        {
            return GetStringFromTag(RpmConstants.rpmTag.RPMTAG_NAME);
        }

        private string GetRelease()
        {
            return GetStringFromTag(RpmConstants.rpmTag.RPMTAG_RELEASE);
        }

        private string GetSerial()
        {
            return GetStringFromTag(RpmConstants.rpmTag.RPMTAG_EPOCH);
        }

        private string GetSummary()
        {
            return GetI18StringFromTag(RpmConstants.rpmTag.RPMTAG_SUMMARY);
        }

        private string GetDescription()
        {
            return GetI18StringFromTag(RpmConstants.rpmTag.RPMTAG_DESCRIPTION);
        }

        private DateTime? GetBuildDateTime()
        {
            int[] dateTimeStr = GetInt32FromTag(RpmConstants.rpmTag.RPMTAG_BUILDTIME);
            if(dateTimeStr != null)
            {
                long second = dateTimeStr.First();
                return second.FromUnixTime();
            }

            return null;
        }

        /// <summary>
        /// Получение одной строки с данными из тега
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        private string GetI18StringFromTag(RpmConstants.rpmTag tag)
        {
            var entry = _entries.Where(e => e.Tag == (int)tag)
                        .Cast<RpmStruct.RPMEntry?>()
                        .FirstOrDefault();
            if (entry != null)
            {
                if ((uint)RpmConstants.rpmTagType.RPM_I18NSTRING_TYPE != entry.Value.Type)
                {
                    throw new InvalidDataException("Тип тега у раздела не равен типу тега RPM_I18NSTRING_TYPE");
                }

                long startPosition = GetStartPositionFirstEntry();
                byte[][] data = ReadDataEntry(startPosition, entry.Value);
                if (data.Length > 0)
                {
                    return System.Text.Encoding.UTF8.GetString(data.ElementAt(0));
                }
            }
            return null;

        }

        /// <summary>
        /// Получение одной строки с данными из тега
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        private string GetStringFromTag(RpmConstants.rpmTag tag)
        {
            var entry = _entries.Where(e => e.Tag == (int)tag)
                        .Cast<RpmStruct.RPMEntry?>()
                        .FirstOrDefault();
            if (entry != null)
            {
                if ((uint)RpmConstants.rpmTagType.RPM_STRING_TYPE != entry.Value.Type)
                {
                    throw new InvalidDataException("Тип тега у раздела не равен типу тега RpmConstants.rpmTagType.RPM_STRING_TYPE");
                }

                long startPosition = GetStartPositionFirstEntry();
                byte[][] data = ReadDataEntry(startPosition, entry.Value);
                if (data.Length > 0)
                {
                    return System.Text.Encoding.UTF8.GetString(data.ElementAt(0));
                }
            }
            return null;
        }

        private int[] GetInt32FromTag(RpmConstants.rpmTag tag)
        {
            var entry = _entries.Where(e => e.Tag == (int)tag)
                        .Cast<RpmStruct.RPMEntry?>()
                        .FirstOrDefault();
            if (entry != null)
            {
                if ((uint)RpmConstants.rpmTagType.RPM_INT32_TYPE != entry.Value.Type)
                {
                    throw new InvalidDataException("Тип тега у раздела не равен типу тега RpmConstants.rpmTagType.RPM_INT32_TYPE");
                }

                long startPosition = GetStartPositionFirstEntry();
                byte[][] data = ReadDataEntry(startPosition, entry.Value);
                return data.Select(g => (int)BitConverter.ToUInt32(g, 0)).ToArray();
            }
            return null;
        }

    }
}
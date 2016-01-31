using System;
using System.Collections.Generic;
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
            get
            {
                return _version.Value;
            }
        }

        /// <summary>
        /// Версия
        /// </summary>
        private Lazy<string> _version;

        /// <summary>
        /// Структура заголовка
        /// </summary>
        public RpmStruct.RPMHeader Header = new RpmStruct.RPMHeader();

        // разделы заголовка
        private RpmStruct.RPMEntry[] _entries;

        /// <summary>
        /// размер одного раздела в секции
        /// </summary>
        public readonly long SIZE_ONE_ENTRY = Marshal.SizeOf<RpmStruct.RPMEntry>();

        public RpmHeaderSection(FileStream file)
            : base(file)
        {
            _version = new Lazy<string>(GetVersion);
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
            var entry = _entries.Where(e => e.Tag == (int)RpmConstants.rpmTag.RPMTAG_VERSION)
                .Cast<RpmStruct.RPMEntry?>()
                .FirstOrDefault();
            if (entry != null)
            {
                long startPosition = GetStartPositionFirstEntry();
                byte[][] data = ReadDataEntry(startPosition, entry.Value);
                if (data.Length > 0)
                {
                    return System.Text.Encoding.UTF8.GetString(data.ElementAt(0));
                }
            }
            return string.Empty;
        }

        private byte[][] ReadDataEntry(long startFirstEntryPosition, RpmStruct.RPMEntry entry)
        {
            Func<long, byte[]> func;
            if (_dataEntryReaders.TryGetValue((RpmConstants.rpmTagType)entry.Type, out func))
            {
                List<byte[]> data = new List<byte[]>();
                for (int i = 0; i < entry.Count; ++i)
                {
                    data.Add(func(startFirstEntryPosition + entry.Offset));
                }
                return data.ToArray();
            }
            else
            {
                throw new Exception(string.Format("Для тега типа {0} не реализована функция чтения данных", entry.Type));
            }
        }
    }
}
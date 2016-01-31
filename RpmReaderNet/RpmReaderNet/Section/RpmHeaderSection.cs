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
        /// Структура заголовка
        /// </summary>
        public RpmStruct.RPMHeader Header = new RpmStruct.RPMHeader();

        // разделы заголовка
        public RpmStruct.RPMEntry[] entries;

        /// <summary>
        /// размер одного раздела в секции
        /// </summary>
        public readonly long SIZE_ONE_ENTRY = Marshal.SizeOf<RpmStruct.RPMEntry>();

        public RpmHeaderSection(FileStream file)
            : base(file)
        {
            
        }

        /// <summary>
        /// Получение позиции первого раздела
        /// </summary>
        /// <returns></returns>
        public long GetStartPositionFirstEntry()
        {
            return StartPosition + 16 + Header.entryCount * SIZE_ONE_ENTRY;
        }

        public string GetVersion()
        {
            var entry = entries.Where(e => e.Tag == (int)RpmConstants.rpmTag.RPMTAG_VERSION)
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
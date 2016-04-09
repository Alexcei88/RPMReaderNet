using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace RpmReaderNet.Section
{
    internal abstract class AbstractHeaderSection
        : RpmSection
    {
        /// <summary>
        /// array of entry
        /// </summary>
        protected RpmStruct.RPMEntry[] _entries;

        /// <summary>
        /// size of one entry
        /// </summary>
        public readonly long SIZE_ONE_ENTRY = Marshal.SizeOf(typeof(RpmStruct.RPMEntry));

        /// <summary>
        /// get position first entry on section
        /// </summary>
        /// <returns></returns>
        public abstract long GetStartPositionFirstEntry();

        public AbstractHeaderSection(FileStream stream)
            : base(stream)
        { }

        protected byte[][] ReadDataEntry(long startFirstEntryPosition, RpmStruct.RPMEntry entry)
        {
            Func<long, byte[]> func;
            if (_dataEntryReaders.TryGetValue((RpmConstants.rpmTagType)entry.Type, out func))
            {
                _fileStream.Seek(startFirstEntryPosition + entry.Offset, SeekOrigin.Begin);
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

        /// <summary>
        /// Получение одной строки с данными из тега
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        protected string GetI18StringFromTag(int tag)
        {
            var entry = GetEntry(tag);
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
        protected string GetStringFromStringTypeTag(int tag)
        {
            var entry = GetEntry(tag);
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
                    return Encoding.UTF8.GetString(data.First());
                }
            }
            return null;
        }

        protected uint GetInt32FromTag(int tag)
        {
            var entry = GetEntry(tag);
            if (entry != null)
            {
                if ((uint)RpmConstants.rpmTagType.RPM_INT32_TYPE != entry.Value.Type)
                {
                    throw new InvalidDataException("Тип тега у раздела не равен типу тега RpmConstants.rpmTagType.RPM_INT32_TYPE");
                }
                if (entry.Value.Count > 1)
                {
                    throw new InvalidDataException("В разделе содержиться количество, больше одного значения. Нужен массив");
                }

                long startPosition = GetStartPositionFirstEntry();
                byte[][] data = ReadDataEntry(startPosition, entry.Value);
                if (data.Length > 0)
                {
                    return BitConverter.ToUInt32(data.First().Reverse().ToArray(), 0);
                }
            }
            return default(int);
        }

        protected byte[] GetBinDataFromTag(int tag)
        {
            var entry = GetEntry(tag);
            if (entry != null)
            {
                if ((uint)RpmConstants.rpmTagType.RPM_BIN_TYPE != entry.Value.Type)
                {
                    throw new InvalidDataException("Тип тега у раздела не равен типу тега RpmConstants.rpmTagType.RPM_BIN_TYPE");
                }

                if (entry.Value.Count > 1)
                {
                    throw new InvalidDataException("В разделе содержиться количество, больше одного значения. Нужен массив");
                }
                long startPosition = GetStartPositionFirstEntry();
                byte[][] data = ReadDataEntry(startPosition, entry.Value);
                if (data.Length > 0)
                {
                    return data.First();
                }
            }
            return null;
        }

        protected string[] GetStringArrayFromTag(int tag)
        {
            var entry = GetEntry(tag);
            if (entry != null)
            {
                if ((uint)RpmConstants.rpmTagType.RPM_STRING_ARRAY_TYPE != entry.Value.Type)
                {
                    throw new InvalidDataException("Тип тега у раздела не равен типу тега RpmConstants.rpmTagType.RPM_STRING_ARRAY_TYPE");
                }

                long startPosition = GetStartPositionFirstEntry();
                byte[][] data = ReadDataEntry(startPosition, entry.Value);
                if (data.Length > 0)
                {
                    return data.Select(g => Encoding.UTF8.GetString(g)).ToArray();
                }
            }
            return null;
        }

        protected uint[] GetInt32ArrayFromTag(int tag)
        {
            var entry = GetEntry(tag);
            if (entry != null)
            {
                if ((uint)RpmConstants.rpmTagType.RPM_INT32_TYPE != entry.Value.Type)
                {
                    throw new InvalidDataException("Тип тега у раздела не равен типу тега RpmConstants.rpmTagType.RPM_INT32_TYPE");
                }

                long startPosition = GetStartPositionFirstEntry();
                byte[][] data = ReadDataEntry(startPosition, entry.Value);
                return data.Select(g => BitConverter.ToUInt32(g.Reverse().ToArray(), 0)).ToArray();
            }
            return null;
        }

        private RpmStruct.RPMEntry? GetEntry(int tag)
        {
            return _entries.Where(e => e.Tag == tag)
                        .Cast<RpmStruct.RPMEntry?>()
                        .FirstOrDefault();
        }
    }
}
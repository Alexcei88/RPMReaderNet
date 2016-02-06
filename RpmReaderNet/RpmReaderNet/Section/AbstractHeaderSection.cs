using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RpmReaderNet.Section
{
    abstract class AbstractHeaderSection
        : AbstractRpmSection
    {
        /// <summary>
        /// Функции читатели данных в зависимости от типа тега
        /// </summary>
        protected Dictionary<RpmConstants.rpmTagType, Func<long, byte[]>> _dataEntryReaders = new Dictionary<RpmConstants.rpmTagType, Func<long, byte[]>>();

        // разделы заголовка
        protected RpmStruct.RPMEntry[] _entries;

        /// <summary>
        /// размер одного раздела в секции
        /// </summary>
        public readonly long SIZE_ONE_ENTRY = Marshal.SizeOf(typeof(RpmStruct.RPMEntry));

        public abstract long GetStartPositionFirstEntry();

        public AbstractHeaderSection(FileStream stream)
            : base(stream)
        {
            _dataEntryReaders[RpmConstants.rpmTagType.RPM_STRING_TYPE] = ReadStringTagType;
            _dataEntryReaders[RpmConstants.rpmTagType.RPM_I18NSTRING_TYPE] = ReadI18StringTagType;
            _dataEntryReaders[RpmConstants.rpmTagType.RPM_INT32_TYPE] = ReadInt32;

        }

        protected byte[][] ReadDataEntry(long startFirstEntryPosition, RpmStruct.RPMEntry entry)
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

        /// <summary>
        /// Получение одной строки с данными из тега
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        protected string GetI18StringFromTag(RpmConstants.rpmTag tag)
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
        protected string GetStringFromStringTypeTag(RpmConstants.rpmTag tag)
        {
            var array = GetStringArrayFromStringTypeTag(tag);
            if (array != null)
            {
                return array.First();
            }
            return null;
        }

        protected string[] GetStringArrayFromStringTypeTag(RpmConstants.rpmTag tag)
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
                    return data.Select(g => System.Text.Encoding.UTF8.GetString(g)).ToArray();
                }
            }
            return null;
        }

        protected int[] GetInt32FromTag(RpmConstants.rpmTag tag)
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

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace RpmReaderNet.Section
{
    internal abstract class AbstractRpmSection
    {
        /// <summary>
        /// Файловый поток
        /// </summary>
        protected FileStream _fileStream;

        /// <summary>
        /// Начало секций относительно начала файла
        /// </summary>
        public long StartPosition { get; set; }

        /// <summary>
        /// Функции читатели данных в зависимости от типа тега
        /// </summary>
        protected Dictionary<RpmConstants.rpmTagType, Func<long, byte[]>> _dataEntryReaders = new Dictionary<RpmConstants.rpmTagType, Func<long, byte[]>>();

        public AbstractRpmSection(FileStream fileStream)
        {
            _fileStream = fileStream;
            _dataEntryReaders[RpmConstants.rpmTagType.RPM_STRING_TYPE] = ReadStringTagType;
        }

        static public bool ByteArrayCompare(byte[] a1, byte[] a2)
        {
            return StructuralComparisons.StructuralEqualityComparer.Equals(a1, a2);
        }

        public static uint ReverseBytes(uint value)
        {
            return (value & 0x000000FFU) << 24 | (value & 0x0000FF00U) << 8 |
                (value & 0x00FF0000U) >> 8 | (value & 0xFF000000U) >> 24;
        }

        public static ushort ReverseBytes(ref ushort value)
        {
            return (ushort)((value & 0x00FFU) << 8 | (value & 0xFF00U) >> 8);
        }

        /// <summary>
        /// Чтение данных типа 6
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        protected byte[] ReadStringTagType(long position)
        {
            _fileStream.Seek(position, SeekOrigin.Begin);
            byte sym;
            List<byte> data = new List<byte>();
            while (_fileStream.CanRead)
            {
                sym = (byte)_fileStream.ReadByte();
                if (sym == '\0')
                {
                    break;
                }
                data.Add(sym);
            }
            return data.ToArray();
        }
    }
}
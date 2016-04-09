using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace RpmReaderNet.Section
{
    internal abstract class RpmSection
    {
        /// <summary>
        /// File stream
        /// </summary>
        protected FileStream _fileStream;

        /// <summary>
        /// Позиция начала секций относительно начала файла
        /// </summary>
        public long StartPosition { get; set; }

        /// <summary>
        /// Функции читатели данных в зависимости от типа тега
        /// </summary>
        protected Dictionary<RpmConstants.rpmTagType, Func<long, byte[]>> _dataEntryReaders = new Dictionary<RpmConstants.rpmTagType, Func<long, byte[]>>();

        public RpmSection(FileStream fileStream)
        {
            _fileStream = fileStream;

            /// fill readers
            _dataEntryReaders[RpmConstants.rpmTagType.RPM_STRING_TYPE] = ReadStringTagType;
            _dataEntryReaders[RpmConstants.rpmTagType.RPM_I18NSTRING_TYPE] = ReadI18StringTagType;
            _dataEntryReaders[RpmConstants.rpmTagType.RPM_INT32_TYPE] = ReadInt32;
            _dataEntryReaders[RpmConstants.rpmTagType.RPM_STRING_ARRAY_TYPE] = ReadStringTagType;
            _dataEntryReaders[RpmConstants.rpmTagType.RPM_BIN_TYPE] = ReadBin;
        }

        /// <summary>
        /// Compare arrays
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <returns></returns>
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
        /// Read data for a type of tag equal 6(string)
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        protected byte[] ReadStringTagType(long position)
        {
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

        /// <summary>
        /// Read data for a type of tag equal 9(i18string)
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        protected byte[] ReadI18StringTagType(long position)
        {
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

        /// <summary>
        /// Read data for a type of tag equal 4(int32)
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        protected byte[] ReadInt32(long position)
        {
            const int size = sizeof(int);
            byte[] buffer = new byte[size];
            if (_fileStream.Read(buffer, 0, size) < size)
            {
                return null;
            }
            return buffer;
        }

        /// <summary>
        /// Read data for a type of tag equal 7(Bin)
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        protected byte[] ReadBin(long position)
        {
            const int size = 16;
            byte[] buffer = new byte[size];
            if (_fileStream.Read(buffer, 0, size) < size)
            {
                return null;
            }
            return buffer;
        }
    }
}
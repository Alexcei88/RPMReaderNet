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

        public AbstractRpmSection(FileStream fileStream)
        {
            _fileStream = fileStream;
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
        /// Чтение данных тега типа 6(string)
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
        /// Чтение данных тега типа 9(i18string)
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
        /// Чтение данных тега типа 4(int32)
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
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace RpmReaderNet.Section
{
    /// <summary>
    /// Reader of Rpm section 
    /// </summary>
    internal abstract class RpmSection
    {
        /// <summary>
        /// File stream
        /// </summary>
        protected FileStream _fileStream;

        /// <summary>
        /// the position where section starts in file
        /// </summary>
        public long StartPosition { get; set; }

        /// <summary>
        /// Readers entry depending on the type of tag
        /// </summary>
        protected Dictionary<RpmConstants.rpmTagType, Func<byte[]>> _dataEntryReaders = new Dictionary<RpmConstants.rpmTagType, Func<byte[]>>();

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
      
        /// <summary>
        /// Read data for tag with type equal 6(string)
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        protected byte[] ReadStringTagType()
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
        /// Read data for tag with type equal 9(i18string)
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        protected byte[] ReadI18StringTagType()
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
        /// Read data for tag with type equal 4(int32)
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        protected byte[] ReadInt32()
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
        /// Read data for tag with type equal 7(Bin)
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        protected byte[] ReadBin()
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
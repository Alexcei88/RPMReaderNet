using System;
using System.IO;
using System.Runtime.InteropServices;

namespace RpmReaderNet.Section
{
    /// <summary>
    /// Signature cекция
    /// </summary>
    internal class RpmSignatureSection
        : AbstractRpmSection
    {
        public RpmStruct.RPMSignature Signature = new RpmStruct.RPMSignature();

        // разделы сигнатуры
        public RpmStruct.RPMEntry[] _entries;

        /// <summary>
        /// размер одного раздела в секции
        /// </summary>
        public static readonly long SIZE_ONE_ENTRY = Marshal.SizeOf(typeof(RpmStruct.RPMEntry));

        public RpmSignatureSection(FileStream file)
            : base(file)
        {

        }

        public bool FillHeaderData(byte[] data)
        {
            int len = Marshal.SizeOf(Signature.GetType());
            IntPtr @in = Marshal.AllocHGlobal(len);
            Marshal.Copy(data, 0, @in, len);
            Signature = (RpmStruct.RPMSignature)Marshal.PtrToStructure(@in, Signature.GetType());
            Marshal.FreeHGlobal(@in);
            Signature.bytesDataCount = (int)ReverseBytes((uint)Signature.bytesDataCount);
            Signature.entryCount = (int)ReverseBytes((uint)Signature.entryCount);
            byte[] buffer = new byte[RpmStruct.RPM_MAGIC_SIGNATURE_NUMBER.Length];
            unsafe
            {
                fixed (byte* ptr = Signature.magic)
                {
                    int i = 0;
                    for (byte* d = ptr; i < RpmStruct.RPM_MAGIC_SIGNATURE_NUMBER.Length; ++i, ++d)
                    {
                        buffer[i] = *d;
                    }
                }
            }
            return ByteArrayCompare(buffer, RpmStruct.RPM_MAGIC_SIGNATURE_NUMBER);
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
                entry.Count = ReverseBytes(entry.Count);
                entry.Offset = ReverseBytes(entry.Offset);
                entry.Tag = ReverseBytes(entry.Tag);
                entry.Type = ReverseBytes(entry.Type);
                _entries[i] = entry;
            }
            return true;
        }

    }
}
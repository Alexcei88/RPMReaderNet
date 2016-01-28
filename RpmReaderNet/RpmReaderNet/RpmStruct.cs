using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RpmReaderNet
{
    static class RpmStruct
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct RPMEntry
        {
            // тип тега
            public int Tag;
            // тип тега
            public int Type;
            // смещение данных, относительно начала данных
            public int Offset;
            // количество данных
            public int Count;
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct rpmlead
        {
            public static readonly byte[] RPM_MAGIC_HEADER_NUMBER = { 0xed, 0xab, 0xee, 0xdb };

            // магическое число rpm файла
            public fixed byte magic[4];
            public byte major, minor;
            public short type;
            public short archnum;
            public fixed char name[66];
            public short osnum;
            public short signature_type;
            public fixed char reserved[16];
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct RPMSignature
        {
            // магическое число начала заголовка сигнатуры
            public static readonly byte[] RPM_MAGIC_HEADER_NUMBER = { 0x8e, 0xad, 0xe8 };
            // 1 бит - версия сигнатуры
            public byte headerVersion;
            // 4 бита - зарезервированы
            public int reserved;
            // количество разделов сигнатуры
            public int entryCount;
            // количество байт с данными сигнатуры
            public int bytesDataCount;

            // разделы заголовка
            public RPMEntry[] entry;

            static public RPMSignature Create(int countEntry)
            {
                RPMSignature signature = new RPMSignature();
                signature.entry = new RPMEntry[countEntry];
                return signature;
            }
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct RPMHeader
        {
            // магическое число начала заголовка
            public static readonly byte[] RPM_MAGIC_HEADER_NUMBER = { 0x8e, 0xad, 0xe8 };
            // 1 бит - версия
            public byte headerVersion;
            // 4 бита - зарезервированы
            public int reserved;
            // количество разделов
            public int entryCount;
            // количество байт с данными
            public int bytesDataCount;

            // разделы заголовка
            public RPMEntry[] entry;
            
            static public RPMHeader Create(int countEntry)
            {
                RPMHeader header = new RPMHeader();
                header.entry = new RPMEntry[countEntry];
                return header;
            }
        };
    }
}

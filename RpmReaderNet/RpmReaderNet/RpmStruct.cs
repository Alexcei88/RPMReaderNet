using System.Runtime.InteropServices;

namespace RpmReaderNet
{
    internal static class RpmStruct
    {
        public static readonly byte[] RPM_MAGIC_LEAD_NUMBER = { 0xed, 0xab, 0xee, 0xdb };

        // магическое число начала заголовка сигнатуры
        public static readonly byte[] RPM_MAGIC_SIGNATURE_NUMBER = { 0x8e, 0xad, 0xe8 };

        // магическое число начала заголовка
        public static readonly byte[] RPM_MAGIC_HEADER_NUMBER = { 0x8e, 0xad, 0xe8 };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct RPMEntry
        {
            // тип тега
            public uint Tag;
            // тип тега
            public uint Type;
            // смещение данных, относительно начала данных
            public uint Offset;
            // количество данных
            public uint Count;
        };

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public unsafe struct rpmlead
        {
            // магическое число rpm файла
            public fixed byte magic[4];

            public byte major;
            public byte minor;
            
            public short type;
            public short archnum;
            
            public fixed byte name[66];
            public short osnum;
            public short signature_type;
            public fixed byte reserved[16];
        };

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public unsafe struct RPMSignature
        {
            // магическое числов сигнатуры
            public fixed byte magic[3];

            // 1 бит - версия сигнатуры
            public byte headerVersion;

            // 4 бита - зарезервированы
            public int reserved;

            // количество разделов сигнатуры
            public int entryCount;

            // количество байт с данными сигнатуры
            public int bytesDataCount;
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct RPMHeader
        {
            // магическое числов секции
            public fixed byte magic[3];

            // 1 бит - версия
            public byte headerVersion;

            // 4 бита - зарезервированы
            public int reserved;

            // количество разделов
            public int entryCount;

            // количество байт с данными
            public int bytesDataCount;
        };
    }
}
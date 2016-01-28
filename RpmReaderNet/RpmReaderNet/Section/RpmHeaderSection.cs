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

        /// <summary>
        /// размер одного раздела в секции
        /// </summary>
        public readonly long SIZE_ONE_ENTRY = Marshal.SizeOf<RpmStruct.RPMEntry>();

        /// <summary>
        /// Получение позиции первого раздела
        /// </summary>
        /// <returns></returns>
        public long GetStartPositionFirstEntry()
        {
            return StartPosition + 16 + Header.entryCount * SIZE_ONE_ENTRY;
        }
    }
}
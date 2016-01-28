namespace RpmReaderNet.Section
{
    internal abstract class AbstractRpmSection
    {
        /// <summary>
        /// Начало секций относительно начала файла
        /// </summary>
        public long StartPosition { get; set; }
    }
}
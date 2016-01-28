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

        /// <summary>
        /// размер одного раздела в секции
        /// </summary>
        public static readonly long SIZE_ONE_ENTRY = Marshal.SizeOf<RpmStruct.RPMEntry>();
    }
}
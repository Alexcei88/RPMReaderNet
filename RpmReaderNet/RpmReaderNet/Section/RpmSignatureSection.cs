namespace RpmReaderNet.Section
{
    /// <summary>
    /// Signature cекция 
    /// </summary>
    internal class RpmSignatureSection
        : AbstractRpmSection
    {
        public RpmStruct.RPMSignature Signature { get; set; } = new RpmStruct.RPMSignature();
    }
}

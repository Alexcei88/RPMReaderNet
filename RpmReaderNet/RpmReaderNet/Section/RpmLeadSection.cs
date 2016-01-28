namespace RpmReaderNet.Section
{
    /// <summary>
    /// Lead секция
    /// </summary>
    internal class RpmLeadSection
        : AbstractRpmSection
    {
        public RpmStruct.rpmlead Lead = new RpmStruct.rpmlead();

        public RpmLeadSection()
        {
            StartPosition = 0;
        }
    }
}
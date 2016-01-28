using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpmReaderNet.Section
{
    /// <summary>
    /// Lead секция
    /// </summary>
    class RpmLeadSection
        : AbstractRpmSection
    {
        public RpmStruct.rpmlead Lead { get; set; }

        public RpmLeadSection()
        {
            StartPosition = 0;
            Lead = new RpmStruct.rpmlead();
        }
    }
}

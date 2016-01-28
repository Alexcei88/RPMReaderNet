using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpmReaderNet.Section
{
    /// <summary>
    /// header-секция
    /// </summary>
    internal class RpmHeaderSection
    {
        /// <summary>
        /// Структура заголовка
        /// </summary>
        public RpmStruct.RPMHeader Header { get; set; } = new RpmStruct.RPMHeader();
    }

}

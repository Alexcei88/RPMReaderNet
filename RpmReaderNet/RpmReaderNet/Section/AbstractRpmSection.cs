using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpmReaderNet.Section
{
    internal abstract class AbstractRpmSection
    {
        /// <summary>
        /// Начало секций относительно начала файла
        /// </summary>
        public int StartPosition { get; set; }
    }
}

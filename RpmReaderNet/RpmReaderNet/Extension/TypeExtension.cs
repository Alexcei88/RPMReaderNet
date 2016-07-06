using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpmReaderNetLib.Extension
{
    /// <summary>
    /// Extension method for base types
    /// </summary>
    public static class TypeExtension
    {
        /// <summary>
        /// DateTime from Time Epoch
        /// </summary>
        /// <param name="unixTime"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this uint unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpmReaderNet
{
    public static class Extensions
    {
        public static void Ignore(this FileStream fileStream, long count)
        {
            fileStream.Seek(fileStream.Position + count, SeekOrigin.Begin);
        }

        public static DateTime FromUnixTime(this long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }
    }
}

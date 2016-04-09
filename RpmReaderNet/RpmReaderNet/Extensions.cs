using System;
using System.IO;

namespace RpmReaderNet
{
    public static class Extensions
    {
        public static void Ignore(this FileStream fileStream, long count)
        {
            fileStream.Seek(fileStream.Position + count, SeekOrigin.Begin);
        }

        /// <summary>
        /// DateTime from Time Epoch
        /// </summary>
        /// <param name="unixTime"></param>
        /// <returns></returns>
        public static DateTime FromUnixTime(this long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }
    }
}
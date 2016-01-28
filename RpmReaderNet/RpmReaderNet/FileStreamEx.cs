using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpmReaderNet
{
    public static class FileStreamEx
    {
        public static void Ignore(this FileStream fileStream, long count)
        {
            fileStream.Seek(fileStream.Position + count, SeekOrigin.Begin);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Core;

namespace RpmReaderNet.Section
{
    class RpmArchiveSection
        : AbstractRpmSection
    {
        /// <summary>
        /// Магическое число начала секции архива(gzip архив)
        /// </summary>
        public static readonly byte[] RPM_MAGIC_GZIP_NUMBER = { 0x1f, 0x8b };

        /// <summary>
        /// Архив с данными
        /// </summary>
        public byte[] Data { get; set; }

        public RpmArchiveSection(FileStream stream)
            : base(stream)
        {

        }

        public void SaveCpioArchive(string fileName)
        {
            byte[] dataBuffer = new byte[4096];
            using (MemoryStream sr = new MemoryStream(Data, 0, Data.Length))
            {
                using (GZipInputStream gzipStream = new GZipInputStream(sr))
                {
                    //string fnOut = Path.Combine(targetDir, Path.GetFileNameWithoutExtension(gzipFileName));

                    using (FileStream fsOut = File.Create(fileName))
                    {
                        StreamUtils.Copy(gzipStream, fsOut, dataBuffer);
                    }
                }
            }
        }

        
    }
}

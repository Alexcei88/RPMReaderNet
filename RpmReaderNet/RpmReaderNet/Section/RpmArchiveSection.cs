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
        : RpmSection
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

        public void Extract(string destFolder)
        {
            string tempDirectory = GetTemporaryDirectory();
            string tempCpioFile = Path.Combine(tempDirectory, Path.GetRandomFileName());
            try
            {
                SaveGZipArchive(tempCpioFile);
                ExtractCpioArchive(tempCpioFile, destFolder);
            }
            finally
            {
                // удаляем временную папку
                Directory.Delete(tempDirectory, true);
            }
        }

        /// <summary>
        /// Saves binary data of gzip to file
        /// </summary>
        /// <param name="fileName"></param>
        private void SaveGZipArchive(string fileName)
        {
            byte[] dataBuffer = new byte[4096];
            using (MemoryStream sr = new MemoryStream(Data, 0, Data.Length))
            {
                using (GZipInputStream gzipStream = new GZipInputStream(sr))
                {
                    using (FileStream fsOut = File.Create(fileName))
                    {
                        StreamUtils.Copy(gzipStream, fsOut, dataBuffer);
                    }
                }
            }
        }

        /// <summary>
        /// Extracts cpio file to destinition folder
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="destFolder"></param>
        private void ExtractCpioArchive(string fileName, string destFolder)
        {
            using (CPIOLibSharp.CPIOFileStream fs = new CPIOLibSharp.CPIOFileStream(fileName))
            {
                fs.Extract(destFolder, new CPIOLibSharp.ExtractFlags[] 
                {
                    CPIOLibSharp.ExtractFlags.ARCHIVE_EXTRACT_TIME
                });
            }
        }

        /// <summary>
        /// Returns random temporary folder
        /// </summary>
        /// <returns></returns>
        private static string GetTemporaryDirectory()
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            return tempDirectory;
        }
    }
}

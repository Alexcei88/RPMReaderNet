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

        public void Extract(string destFolder)
        {
            string tempDirectory = GetTemporaryDirectory();
            string tempCpioFile = Path.Combine(tempDirectory, Path.GetRandomFileName());
            try
            {
                SaveGZipArchive(tempCpioFile);

                ExtractCpioArchive(tempCpioFile, destFolder);
                /*
                if (!ArchiveUtils.ExtractArArchive(tempCpioFile, destFolder))
                {
                    // удаляем временную папку
                    Directory.Delete(tempDirectory, true);
                    throw new Exception("Не удалось распаковать cpioArchive");
                }*/
            }
            finally
            {
                // удаляем временную папку
                Directory.Delete(tempDirectory, true);
            }
        }

        private void SaveGZipArchive(string fileName)
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

        private void ExtractCpioArchive(string fileName, string destFolder)
        {
            using (CPIOLibSharp.CPIOFileStream fs = new CPIOLibSharp.CPIOFileStream(fileName))
            {
                fs.Extract(destFolder, null);
            }
        }

        /// <summary>
        /// Возвращает временную папку
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

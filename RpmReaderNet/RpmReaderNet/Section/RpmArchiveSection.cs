using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.GZip;
using System.IO;

namespace RpmReaderNet.Section
{
    /// <summary>
    /// Reader a archive section
    /// </summary>
    internal class RpmArchiveSection
        : RpmSection
    {
        /// <summary>
        /// the magic number то which start gzip archive
        /// </summary>
        public static readonly byte[] RPM_MAGIC_GZIP_NUMBER = { 0x1f, 0x8b };

        /// <summary>
        /// the buffer for data from archive
        /// </summary>
        public byte[] Data { get; set; }

        public RpmArchiveSection(FileStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Extract data from archive section in package
        /// </summary>
        /// <param name="destFolder"></param>
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
                // remove temp directory
                Directory.Delete(tempDirectory, true);
            }
        }

        /// <summary>
        /// decompressing gzip archive to destition file
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
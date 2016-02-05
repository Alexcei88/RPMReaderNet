using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace RpmReaderNet
{
    public static class ArchiveUtils
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [ResourceExposure(ResourceScope.Process)]
        public static extern IntPtr LoadLibrary(string libFilename);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        internal delegate int ExtractArchiveMethod([MarshalAs(UnmanagedType.LPStr)]string fileName, [MarshalAs(UnmanagedType.LPStr)]string destFolder);
        internal delegate int IsArArchiveMethod([MarshalAs(UnmanagedType.LPStr)]string fileName);

        /// <summary>
        /// Указатель на библиотеку с нативным кодом 
        /// </summary>
        private static IntPtr _extractLib;

        static ArchiveUtils()
        {
            if (Environment.Is64BitProcess)
            {
                LoadLibrary(@"x64\archive.dll");
                _extractLib = LoadLibrary(@"x64\extractarchive.dll");
            }
            else
            {
                LoadLibrary(@"x86\archive.dll");
                _extractLib = LoadLibrary(@"x86\extractarchive.dll");
            }
            if (_extractLib == IntPtr.Zero)
            {
                throw new Exception("Не удалось загрузить библиотек extractarchive.dll");
            }
        }

        /// <summary>
        /// Извлечение ar-архива
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="dest"></param>
        /// <returns></returns>
        public static bool ExtractArArchive(string fileName, string dest)
        {
            if (_extractLib != IntPtr.Zero)
            {
                IntPtr extractArchive = GetProcAddress(_extractLib, "ExtractArchive");
                if (extractArchive != IntPtr.Zero)
                {
                    ExtractArchiveMethod extractMethod = Marshal.GetDelegateForFunctionPointer(extractArchive, typeof(ExtractArchiveMethod)) as ExtractArchiveMethod;
                    return extractMethod(fileName, dest) == 0;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        // Extracts the file contained within a GZip to the target dir.
        // A GZip can contain only one file, which by default is named the same as the GZip except
        // without the extension.
        //
        public static void ExtractGZip(string gzipFileName, string targetDir)
        {
            // Use a 4K buffer. Any larger is a waste.
            byte[] dataBuffer = new byte[4096];

            using (Stream fs = new FileStream(gzipFileName, FileMode.Open, FileAccess.Read))
            {
                using (GZipInputStream gzipStream = new GZipInputStream(fs))
                {
                    // Change this to your needs
                    string fnOut = Path.Combine(targetDir, Path.GetFileNameWithoutExtension(gzipFileName));

                    using (FileStream fsOut = File.Create(fnOut))
                    {
                        StreamUtils.Copy(gzipStream, fsOut, dataBuffer);
                    }
                }
            }
        }
    }
}
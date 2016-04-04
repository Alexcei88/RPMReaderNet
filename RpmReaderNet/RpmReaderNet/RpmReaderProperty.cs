using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpmReaderNet
{
    public partial class RpmReader
    {
        /// <summary>
        /// Версия пакета
        /// </summary>
        public string Version
        {
            get { return IsValidate ? _headerSection.Version : null; }
        }

        public string Release
        {
            get { return IsValidate ? _headerSection.Release : null; }
        }

        public string Serial
        {
            get { return IsValidate ? _headerSection.Serial : null; }
        }

        /// <summary>
        /// Имя пакета
        /// </summary>
        public string Name
        {
            get { return IsValidate ? _headerSection.Name : null; }
        }

        public string Summary
        {
            get { return IsValidate ? _headerSection.Summary : null; }
        }

        public string Description
        {
            get { return IsValidate ? _headerSection.Description : null; }
        }

        public DateTime? BuildTime
        {
            get { return IsValidate ? _headerSection.BuildDateTime : null; }
        }

        public string BuildHost
        {
            get { return IsValidate ? _headerSection.BuildHost : null; }
        }

        public string Distribution
        {
            get { return IsValidate ? _headerSection.Distribution : null; }
        }

        public string Vendor
        {
            get { return IsValidate ? _headerSection.Vendor : null; }
        }

        public string License
        {
            get { return IsValidate ? _headerSection.License : null; }
        }

        public string Packager
        {
            get { return IsValidate ? _headerSection.Packager : null; }
        }

        public string Changelog
        {
            get { return IsValidate ? _headerSection.Changelog : null; }
        }

        public string[] Source
        {
            get { return IsValidate ? _headerSection.Source : null; }
        }

        public string Arch
        {
            get { return IsValidate ? _headerSection.Arch : null; }
        }

        public string PreinScript
        {
            get { return IsValidate ? _headerSection.PreinScript : null; }
        }

        public string PostinScript
        {
            get { return IsValidate ? _headerSection.PostinScript : null; }
        }

        public string PreunScript
        {
            get { return IsValidate ? _headerSection.PreunScript : null; }
        }
        public string PostunScript
        {
            get { return IsValidate ? _headerSection.PostunScript : null; }
        }

        [Obsolete]
        public string[] OldFileNames
        {
            get { return IsValidate ? _headerSection.OldFilenames : null; }
        }

        public string[] FileUserNames
        {
            get { return IsValidate ? _headerSection.FileUserNames : null; }
        }

        public string[] FileGroupNames
        {
            get { return IsValidate ? _headerSection.FileGroupNames : null; }
        }

        public uint FileSizes
        {
            get { return IsValidate ? _headerSection.FileSizes : default(uint); }
        }

        public string[] MD5Files
        {
            get { return IsValidate ? _headerSection.MD5Files : null; }
        }

        public byte[] MD5Signature
        {
            get { return IsValidate ? _signatureSection.MD5 : null; }
        }

        public byte[] GPGSignature
        {
            get { return IsValidate ? _signatureSection.GPG : null; }
        }

        public byte[] PGPSignature
        {
            get { return IsValidate ? _signatureSection.PGP : null; }
        }

        public string SHA1Signature
        {
            get { return IsValidate ? _signatureSection.SHA1 : null; }
        }

        public byte[] DSASignature
        {
            get { return IsValidate ? _signatureSection.DSA : null; }
        }

        public byte[] RSASignature
        {
            get { return IsValidate ? _signatureSection.RSA : null; }
        }

        public uint Size
        {
            get { return IsValidate ? _signatureSection.Size : default(uint); }
        }

        public uint PayloadSize
        {
            get { return IsValidate ? _signatureSection.PayloadSize : default(uint); }
        }
    }
}

using System;
using System.Linq;

namespace RpmReaderNet
{
    /// <summary>
    /// List of properties for access to metadata from a rpm file
    /// </summary>
    public partial class RpmReader
    {
        /// <summary>
        /// The version of the package
        /// </summary>
        public string Version
        {
            get { return IsValidate ? _headerSection.Version : null; }
        }

        /// <summary>
        /// The release number of the package
        /// </summary>
        public string Release
        {
            get { return IsValidate ? _headerSection.Release : null; }
        }

        /// <summary>
        /// Serial number of the package
        /// </summary>
        public string Serial
        {
            get { return IsValidate ? _headerSection.Serial : null; }
        }

        /// <summary>
        /// The name of the package
        /// </summary>
        public string Name
        {
            get { return IsValidate ? _headerSection.Name : null; }
        }

        /// <summary>
        /// The summary information about the package
        /// </summary>
        public string Summary
        {
            get { return IsValidate ? _headerSection.Summary : null; }
        }

        /// <summary>
        /// Description of the package
        /// </summary>
        public string Description
        {
            get { return IsValidate ? _headerSection.Description : null; }
        }

        /// <summary>
        /// A build time of the package
        /// </summary>
        public DateTime? BuildTime
        {
            get { return IsValidate ? _headerSection.BuildDateTime : null; }
        }

        /// <summary>
        /// a host hame where was build package
        /// </summary>
        public string BuildHost
        {
            get { return IsValidate ? _headerSection.BuildHost : null; }
        }

        /// <summary>
        /// A group name of packages, of which this package is a part
        /// </summary>
        public string Distribution
        {
            get { return IsValidate ? _headerSection.Distribution : null; }
        }

        /// <summary>
        /// a name of the entity that is responsible for a package
        /// </summary>
        public string Vendor
        {
            get { return IsValidate ? _headerSection.Vendor : null; }
        }

        /// <summary>
        /// A license of the package
        /// </summary>
        public string License
        {
            get { return IsValidate ? _headerSection.License : null; }
        }

        /// <summary>
        /// a name and contact information for the person or persons who built the package
        /// </summary>
        public string Packager
        {
            get { return IsValidate ? _headerSection.Packager : null; }
        }

        /// <summary>
        /// a log or record of all or all notable changes made to a package
        /// </summary>
        public string Changelog
        {
            get
            {   if (IsValidate)
                {
                    string result = _headerSection.Changelog;
                    if(result == null)
                    {
                        DateTime[] arrayTime = _headerSection.ChangelogTime;
                        var arrayValue = _headerSection.ChangelogArray;
                        var arrayName = _headerSection.ChanelogNameArray;

                        // predicate for get record of changelog
                        Func<int, string> GetRecordChangeLog = (index) =>
                        {
                            return "* " + arrayTime[index].ToString() + " "
                            + arrayName[index].ToString() + "\n"
                            + arrayValue[index].ToString() + "\n";
                        };

                        for(int i = 0; i < arrayName.Count(); ++i)
                        {
                            result += GetRecordChangeLog(i);
                            result += "\n";
                        }
                    }
                    return result;
                }
                else
                    return null;
            }
        }


        /// <summary>
        /// A original source files name
        /// </summary>
        public string[] Source
        {
            get { return IsValidate ? _headerSection.Source : null; }
        }

        /// <summary>
        /// a name of the source package from which this binary package was built
        /// </summary>
        public string SourceRpm
        {
            get { return IsValidate ? _headerSection.SourceRpm : null; }
        }

        /// <summary>
        /// the architecture for which the package was built
        /// </summary>
        public string Arch
        {
            get { return IsValidate ? _headerSection.Arch : null; }
        }

        /// <summary>
        /// get content of the package's pre-install script
        /// </summary>
        public string PreinScript
        {
            get { return IsValidate ? _headerSection.PreinScript : null; }
        }

        /// <summary>
        /// get content of the package's post-install script
        /// </summary>
        public string PostinScript
        {
            get { return IsValidate ? _headerSection.PostinScript : null; }
        }

        /// <summary>
        /// get content of the package's pre-uninstall script
        /// </summary>
        public string PreunScript
        {
            get { return IsValidate ? _headerSection.PreunScript : null; }
        }

        /// <summary>
        /// get content of the package's post-uninstall script
        /// </summary>
        public string PostunScript
        {
            get { return IsValidate ? _headerSection.PostunScript : null; }
        }

        /// <summary>
        /// the total size occupied by the package(in bytes)
        /// </summary>
        public uint Size
        {
            get { return IsValidate ? _headerSection.Size : default(uint); }
        }

        /// <summary>
        /// a list of all base names files in package
        /// </summary>
        public string[] BaseFileNames
        {
            get { return IsValidate ? _headerSection.BaseFilenames : null; }
        }

        /// <summary>
        /// a list of all dirs in package
        /// </summary>
        public string[] DirNames
        {
            get { return IsValidate ? _headerSection.DirNames : null; }
        }

        /// <summary>
        /// Indexes of dir for link between the file name and directory
        /// </summary>
        public uint[] DirIndexes
        {
            get { return IsValidate ? _headerSection.DirIndexes : null; }
        }

        /// <summary>
        /// a owner, in alphanumeric form, of each of the files that comprise the package
        /// </summary>
        public string[] FileUserNames
        {
            get { return IsValidate ? _headerSection.FileUserNames : null; }
        }

        /// <summary>
        /// a group, in alphanumeric form, of each of the files that comprise the package
        /// </summary>
        public string[] FileGroupNames
        {
            get { return IsValidate ? _headerSection.FileGroupNames : null; }
        }

        /// <summary>
        /// a size, in bytes, of each of the files that comprise the package
        /// </summary>
        public uint FileSize
        {
            get { return IsValidate ? _headerSection.FileSize : default(uint); }
        }

        /// <summary>
        ///  a MD5 checksum for each of the files that comprise the package
        /// </summary>
        public string[] MD5Files
        {
            get { return IsValidate ? _headerSection.MD5Files : null; }
        }

        /// <summary>
        /// MD5 signatures of files from package
        /// </summary>
        public byte[][] MD5Signature
        {
            get { return IsValidate ? _signatureSection.MD5 : null; }
        }

        /// <summary>
        /// a GPG signature of package
        /// </summary>
        public byte[] GPGSignature
        {
            get { return IsValidate ? _signatureSection.GPG : null; }
        }

        /// <summary>
        /// a PGP signature of package
        /// </summary>
        public byte[] PGPSignature
        {
            get { return IsValidate ? _signatureSection.PGP : null; }
        }

        /// <summary>
        /// a SHA hash of package
        /// </summary>
        public string SHA1Hash
        {
            get { return IsValidate ? _signatureSection.SHA1 : null; }
        }

        /// <summary>
        /// a DSA signature of package
        /// </summary>
        public byte[] DSASignature
        {
            get { return IsValidate ? _signatureSection.DSA : null; }
        }

        /// <summary>
        /// a RSA signature of package
        /// </summary>
        public byte[] RSASignature
        {
            get { return IsValidate ? _signatureSection.RSA : null; }
        }

        /// <summary>
        /// Размер секции сигнатуры
        /// </summary>
        /*public uint SignatureSize
        {
            get { return IsValidate ? _signatureSection.Size : default(uint); }
        }*/

        /*public uint FullSize
        {
            get { return IsValidate ? _signatureSection.Size : default(uint); }
        }*/

        /*
        public uint PayloadSize
        {
            get { return IsValidate ? _signatureSection.PayloadSize : default(uint); }
        }*/

        /// <summary>
        /// a list of capabilities the package requires.
        /// </summary>
        public string[] RequiresNames
        {
            get
            {
                return IsValidate ? _headerSection.RequiresNames : null;
            }
        }
    }
}
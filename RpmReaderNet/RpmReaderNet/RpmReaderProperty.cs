using System;

namespace RpmReaderNet
{
    /// <summary>
    /// Reader(содержит все свойства класса-читателя
    /// </summary>
    public partial class RpmReader
    {
        /// <summary>
        /// Версия пакета
        /// </summary>
        public string Version
        {
            get { return IsValidate ? _headerSection.Version : null; }
        }

        /// <summary>
        /// Версия релиза
        /// </summary>
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

        /// <summary>
        /// Общая информация о пакете
        /// </summary>
        public string Summary
        {
            get { return IsValidate ? _headerSection.Summary : null; }
        }

        /// <summary>
        /// Описание пакета
        /// </summary>
        public string Description
        {
            get { return IsValidate ? _headerSection.Description : null; }
        }

        /// <summary>
        /// Время сборки пакета
        /// </summary>
        public DateTime? BuildTime
        {
            get { return IsValidate ? _headerSection.BuildDateTime : null; }
        }

        /// <summary>
        /// Имя хоста машины, на котором собран пакет
        /// </summary>
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

        /// <summary>
        /// Имя исходника rpm, откуда было собрано
        /// </summary>
        public string SourceRpm
        {
            get { return IsValidate ? _headerSection.SourceRpm : null; }
        }

        /// <summary>
        /// Архитектура
        /// </summary>
        public string Arch
        {
            get { return IsValidate ? _headerSection.Arch : null; }
        }

        /// <summary>
        /// Содержимое скрипта Prein
        /// </summary>
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

        /// <summary>
        /// Размер данных пакета(после распаковки)
        /// </summary>
        public uint Size
        {
            get { return IsValidate ? _headerSection.Size : default(uint); }
        }

        /// <summary>
        /// The list of all base names files
        /// </summary>
        public string[] BaseFileNames
        {
            get { return IsValidate ? _headerSection.BaseFilenames : null; }
        }

        /// <summary>
        /// The list of all dirs
        /// </summary>
        public string[] DirNames
        {
            get { return IsValidate ? _headerSection.DirNames : null; }
        }

        /// <summary>
        /// Indexes of dir
        /// </summary>
        public uint[] DirIndexes
        {
            get { return IsValidate ? _headerSection.DirIndexes : null; }
        }

        /// <summary>
        /// Имя пользователя владельца файла
        /// </summary>
        public string[] FileUserNames
        {
            get { return IsValidate ? _headerSection.FileUserNames : null; }
        }

        /// <summary>
        /// Имя группы владельца файла
        /// </summary>
        public string[] FileGroupNames
        {
            get { return IsValidate ? _headerSection.FileGroupNames : null; }
        }

        public uint FileSizes
        {
            get { return IsValidate ? _headerSection.FileSizes : default(uint); }
        }

        /// <summary>
        /// MD5 Files
        /// </summary>
        public string[] MD5Files
        {
            get { return IsValidate ? _headerSection.MD5Files : null; }
        }

        /// <summary>
        /// MD5 Signature of package
        /// </summary>
        public byte[] MD5Signature
        {
            get { return IsValidate ? _signatureSection.MD5 : null; }
        }

        /// <summary>
        /// GPG Signature of package
        /// </summary>
        public byte[] GPGSignature
        {
            get { return IsValidate ? _signatureSection.GPG : null; }
        }

        /// <summary>
        /// PGP Signature of package
        /// </summary>
        public byte[] PGPSignature
        {
            get { return IsValidate ? _signatureSection.PGP : null; }
        }

        /// <summary>
        /// SHA Signature of package
        /// </summary>
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

        /// <summary>
        /// Размер секции сигнатуры
        /// </summary>
        public uint SignatureSize
        {
            get { return IsValidate ? _signatureSection.Size : default(uint); }
        }

        public uint FullSize
        {
            get { return IsValidate ? _signatureSection.Size : default(uint); }
        }

        public uint PayloadSize
        {
            get { return IsValidate ? _signatureSection.PayloadSize : default(uint); }
        }

        /// <summary>
        /// Список зависимостей в пакете
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
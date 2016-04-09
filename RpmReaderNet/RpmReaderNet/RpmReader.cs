using RpmReaderNet.Section;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace RpmReaderNet
{
    public partial class RpmReader
        : IDisposable
    {
        public bool IsValidate
        {
            get
            {
                return CheckValidate();
            }
        }

        /// <summary>
        /// Lead секция
        /// </summary>
        private RpmLeadSection _leadSection;

        /// <summary>
        /// signature секция
        /// </summary>
        private RpmSignatureSection _signatureSection;

        /// <summary>
        /// header секция
        /// </summary>
        private RpmHeaderSection _headerSection;

        /// <summary>
        /// archive секция
        /// </summary>
        private RpmArchiveSection _archiveSection;

        /// <summary>
        /// Был ли разрушен объект
        /// </summary>
        private bool isDisposed;

        /// <summary>
        /// Поток данных файла
        /// </summary>
        private FileStream _fileStream;

        /// <summary>
        /// Состояние чтения файла
        /// </summary>
        private enum StateRead
        {
            /// <summary>
            /// Файл не найден
            /// </summary>
            RPMFILE_NOTFOUND = 0,

            /// <summary>
            /// Файл не валидирован
            /// </summary>
            RPMFILE_NOT_VALIDATE = 1,

            /// <summary>
            /// Файл успешно валидирован
            /// </summary>
            RPMFILE_VALIDATE_SUCCESS = 2,

            /// <summary>
            /// Файл не прошел валидацию
            /// </summary>
            RPMFILE_VALIDATE_ERROR = 3,
        }

        /// <summary>
        /// Текущее состояние файла
        /// </summary>
        private StateRead _state;

        public RpmReader(string rpmFile)
        {
            _state = StateRead.RPMFILE_NOTFOUND;
            if (File.Exists(rpmFile))
            {
                _fileStream = new FileStream(rpmFile, FileMode.Open, FileAccess.Read);
                _state = StateRead.RPMFILE_NOT_VALIDATE;

                _headerSection = new RpmHeaderSection(_fileStream);
                _leadSection = new RpmLeadSection(_fileStream);
                _signatureSection = new RpmSignatureSection(_fileStream);
                _archiveSection = new RpmArchiveSection(_fileStream);
            }
            else
            {
                _state = StateRead.RPMFILE_NOTFOUND;
                throw new FileNotFoundException($"File {rpmFile} not found");
            }
        }

        /// <summary>
        /// Функция валидации пакета
        /// </summary>
        /// <returns></returns>
        public bool Validate()
        {
            if (_state == StateRead.RPMFILE_NOTFOUND || _state == StateRead.RPMFILE_VALIDATE_ERROR)
                return false;

            if (_state == StateRead.RPMFILE_VALIDATE_SUCCESS)
                return true;

            List<Func<bool>> readers = new List<Func<bool>>();
            readers.Add(ReadLead);
            readers.Add(ReadSignature);
            readers.Add(ReadHeader);
            readers.Add(ReadArchive);

            bool validate = true;

            foreach (var reader in readers)
            {
                if (!reader())
                {
                    validate = false;
                    break;
                }
            }

            _state = validate ? StateRead.RPMFILE_VALIDATE_SUCCESS : StateRead.RPMFILE_VALIDATE_ERROR;
            return validate;
        }

        private bool CheckValidate()
        {
            if (_state == StateRead.RPMFILE_VALIDATE_SUCCESS)
            {
                return true;
            }
            else
            {
                if (_state == StateRead.RPMFILE_NOTFOUND || _state == StateRead.RPMFILE_VALIDATE_ERROR)
                {
                    return false;
                }
                else if (_state == StateRead.RPMFILE_NOT_VALIDATE)
                {
                    bool result = Validate();
                    if (!result)
                    {
                        throw new Exception("File is not rpm format");
                    }
                    return result;
                }
            }
            return false;
        }

        public void ExtractPackage(string destFolder)
        {
            _archiveSection.Extract(destFolder);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("------Common section------\n");
            builder.Append($"Name: {Name}\n");
            builder.Append($"Version: {Version}\n");
            builder.Append($"Release: {Release}\n");
            builder.Append($"Serial: {Serial}\n");
            builder.Append($"Source RPM: {SourceRpm}\n");
            builder.Append($"Summary: {Summary}\n");
            builder.Append($"Description: {Description}\n");
            builder.Append($"BuildTime: {BuildTime}\n");
            builder.Append($"BuildHost: {BuildHost}\n");
            builder.Append($"License: {License}\n");
            builder.Append($"Arch: {Arch}\n");
            builder.Append($"Size: {Size}\n");

            builder.Append("------Signature section------\n");
            builder.Append(_signatureSection.ToString());
            return builder.ToString();
        }

        private bool ReadLead()
        {
            int size = Marshal.SizeOf(typeof(RpmStruct.rpmlead));
            byte[] buffer = new byte[size];
            _fileStream.Read(buffer, 0, size);
            return _leadSection.FillSection(buffer);
        }

        private bool ReadSignature()
        {
            if (FindBytes(RpmStruct.RPM_MAGIC_SIGNATURE_NUMBER))
            {
                _signatureSection.StartPosition = _fileStream.Position - RpmStruct.RPM_MAGIC_SIGNATURE_NUMBER.Length * sizeof(byte);
                /// чтение данных залоговка раздела
                _fileStream.Seek(_signatureSection.StartPosition, SeekOrigin.Begin);
                int countData = Marshal.SizeOf(typeof(RpmStruct.RPMSignature));
                byte[] data = new byte[countData];
                if (_fileStream.Read(data, 0, countData) < countData)
                {
                    return false;
                }

                if (!_signatureSection.FillHeaderData(data))
                {
                    return false;
                }

                /// чтение данных о разделе
                countData = Marshal.SizeOf(typeof(RpmStruct.RPMEntry)) * _signatureSection.Signature.entryCount;
                data = new byte[countData];
                if (_fileStream.Read(data, 0, countData) < countData)
                {
                    return false;
                }

                if (!_signatureSection.FillHeaderEntry(data, _signatureSection.Signature.entryCount))
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        private bool ReadHeader()
        {
            if (FindBytes(RpmStruct.RPM_MAGIC_HEADER_NUMBER))
            {
                _headerSection.StartPosition = _fileStream.Position - RpmStruct.RPM_MAGIC_HEADER_NUMBER.Length * sizeof(byte);
                /// чтение данных залоговка раздела
                _fileStream.Seek(_headerSection.StartPosition, SeekOrigin.Begin);
                int countData = Marshal.SizeOf(typeof(RpmStruct.RPMHeader));
                byte[] data = new byte[countData];
                if (_fileStream.Read(data, 0, countData) < countData)
                {
                    return false;
                }

                if (!_headerSection.FillHeaderData(data))
                {
                    return false;
                }

                /// чтение данных о разделе
                countData = Marshal.SizeOf(typeof(RpmStruct.RPMEntry)) * _headerSection.Header.entryCount;
                data = new byte[countData];
                if (_fileStream.Read(data, 0, countData) < countData)
                {
                    return false;
                }

                if (!_headerSection.FillHeaderEntry(data, _headerSection.Header.entryCount))
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        private bool ReadArchive()
        {
            if (FindBytes(RpmArchiveSection.RPM_MAGIC_GZIP_NUMBER))
            {
                _archiveSection.StartPosition = _fileStream.Position - RpmArchiveSection.RPM_MAGIC_GZIP_NUMBER.Length * sizeof(byte);
                long size = _fileStream.Length - _archiveSection.StartPosition;
                _fileStream.Seek(_archiveSection.StartPosition, SeekOrigin.Begin);
                byte[] buffer = new byte[size];
                _fileStream.Read(buffer, 0, (int)size);
                _archiveSection.Data = buffer;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Find array bytes in file
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private bool FindBytes(byte[] bytes)
        {
            byte[] buffer = new byte[bytes.Length];
            if (_fileStream.Read(buffer, 0, bytes.Length) < bytes.Length)
            {
                return false;
            }
            while (!RpmSection.ByteArrayCompare(buffer, bytes))
            {
                Buffer.BlockCopy(buffer, 1, buffer, 0, bytes.Length - 1);
                if (!_fileStream.CanRead)
                {
                    return false;
                }
                buffer[bytes.Length - 1] = (byte)_fileStream.ReadByte();
            }
            return true;
        }

        public void Dispose()
        {
            if (isDisposed) return;

            _fileStream.Dispose();
            isDisposed = true;
        }
    }
}
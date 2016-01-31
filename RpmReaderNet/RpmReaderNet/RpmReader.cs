using RpmReaderNet.Section;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace RpmReaderNet
{
    public class RpmReader
        : IDisposable
    {
        /// <summary>
        /// Версия пакета
        /// </summary>
        public string Version
        {
            get
            {
                return _headerSection.Version;
            }
        }

        public bool IsValidate
        {
            get
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
                        return Validate();
                    }
                }
                return false;
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

            List<Func<bool>> BinaryReader = new List<Func<bool>>();
            BinaryReader.Add(ReadLead);
            BinaryReader.Add(ReadSignature);
            BinaryReader.Add(ReadHeader);

            bool validate = true;

            foreach (var reader in BinaryReader)
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

        private bool ReadLead()
        {
            int size = Marshal.SizeOf<RpmStruct.rpmlead>();
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
                _fileStream.Seek(_headerSection.StartPosition, SeekOrigin.Begin);
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
                if(_fileStream.Read(data, 0, countData) < countData)
                {
                    return false;
                }

                if(!_headerSection.FillHeaderData(data))
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

        private bool FindBytes(byte[] bytes)
        {
            byte[] buffer = new byte[bytes.Length];
            if(_fileStream.Read(buffer, 0, bytes.Length) < bytes.Length)
            {
                return false;
            }
            while(!AbstractRpmSection.ByteArrayCompare(buffer, bytes))
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
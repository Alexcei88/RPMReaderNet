using RpmReaderNet.Section;
using System;
using System.Collections.Generic;
using System.IO;

namespace RpmReaderNet
{
    public class RpmReader
        : IDisposable
    {
        /// <summary>
        /// Версия пакета
        /// </summary>
        public string Version { get; set; }

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


        private RpmLeadSection _leadSection;

        private RpmSignatureSection _signatureSection;

        private RpmHeaderSection _headerSection;

        private bool isDisposed;
        /// <summary>
        /// поток данных файла
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

        private StateRead _state;

        public RpmReader(string rpmFile)
        {
            _state = StateRead.RPMFILE_NOTFOUND;
            if (File.Exists(rpmFile))
            {
                _fileStream = new FileStream(rpmFile, FileMode.Open, FileAccess.Read);
                _state = StateRead.RPMFILE_NOT_VALIDATE;
            }
        }

        /// <summary>
        /// Функция валидации пакета
        /// </summary>
        /// <returns></returns>
        public bool Validate()
        {
            List<Func<bool>> BinaryReader = new List<Func<bool>>();
            BinaryReader.Add(ReadLead);
            BinaryReader.Add(ReadSignature);
            BinaryReader.Add(ReadHeader);

            bool validate = true;

            foreach(var reader in BinaryReader)
            {
                if(!reader())
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
            return false;
        }

        private bool ReadSignature()
        {
            return false;
        }

        private bool ReadHeader()
        {
            return false;
        }



        public void Dispose()
        {
            if (isDisposed) return;

            _fileStream.Dispose();
            isDisposed = true;
        }
    }
}

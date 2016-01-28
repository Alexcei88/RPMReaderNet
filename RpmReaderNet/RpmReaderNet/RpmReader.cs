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
                if (IsValidate)
                {
                    if (_version == null)
                    {
                        _version = GetVersion();
                    }
                }
                return _version;
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
        private RpmLeadSection _leadSection = new RpmLeadSection();

        /// <summary>
        /// signature секция
        /// </summary>
        private RpmSignatureSection _signatureSection = new RpmSignatureSection();

        /// <summary>
        /// header секция
        /// </summary>
        private RpmHeaderSection _headerSection = new RpmHeaderSection();

        /// <summary>
        /// Был ли разрушен объект
        /// </summary>
        private bool isDisposed;

        /// <summary>
        /// Поток данных файла
        /// </summary>
        private FileStream _fileStream;

        /// <summary>
        /// Версия файла
        /// </summary>
        private string _version;

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
        /// Функции читатели данных в зависимости от типа тега
        /// </summary>
        private Dictionary<RpmConstants.rpmTagType, Func<long, byte[]>> _dataEntryReaders = new Dictionary<RpmConstants.rpmTagType, Func<long, byte[]>>();

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

                _dataEntryReaders[RpmConstants.rpmTagType.RPM_STRING_TYPE] = ReadStringTagType;
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
            return true;
        }

        private bool ReadSignature()
        {
            FindBytes(RpmStruct.RPMSignature.RPM_MAGIC_HEADER_NUMBER);
            _signatureSection.StartPosition = _fileStream.Position - RpmStruct.RPMSignature.RPM_MAGIC_HEADER_NUMBER.Length * sizeof(byte);

            _signatureSection.Signature.headerVersion = (byte)_fileStream.ReadByte();
            ReadIntFromCurrentPosition(out _signatureSection.Signature.reserved);
            ReadIntFromCurrentPosition(out _signatureSection.Signature.entryCount);
            ReadIntFromCurrentPosition(out _signatureSection.Signature.bytesDataCount);

            // Сигнатуру пропускаем, данные из нее нам не нужны
            _fileStream.Ignore(RpmSignatureSection.SIZE_ONE_ENTRY * _signatureSection.Signature.entryCount);
            _fileStream.Ignore(_signatureSection.Signature.bytesDataCount);
            return true;
        }

        private bool ReadHeader()
        {
            FindBytes(RpmStruct.RPMHeader.RPM_MAGIC_HEADER_NUMBER);
            _headerSection.StartPosition = _fileStream.Position - RpmStruct.RPMSignature.RPM_MAGIC_HEADER_NUMBER.Length * sizeof(byte);

            _headerSection.Header.headerVersion = (byte)_fileStream.ReadByte();
            ReadIntFromCurrentPosition(out _headerSection.Header.reserved);
            ReadIntFromCurrentPosition(out _headerSection.Header.entryCount);
            ReadIntFromCurrentPosition(out _headerSection.Header.bytesDataCount);

            RpmStruct.RPMEntry[] entries = new RpmStruct.RPMEntry[_headerSection.Header.entryCount];
            for (int i = 0; i < _headerSection.Header.entryCount; ++i)
            {
                RpmStruct.RPMEntry entry;
                ReadIntFromCurrentPosition(out entry.Tag);
                ReadIntFromCurrentPosition(out entry.Type);
                ReadIntFromCurrentPosition(out entry.Offset);
                ReadIntFromCurrentPosition(out entry.Count);
                entries[i] = entry;
            }

            _headerSection.Header.entries = entries;
            return true;
        }

        private string GetVersion()
        {
            var entry = _headerSection.Header.entries.Where(e => e.Tag == (int)RpmConstants.rpmTag.RPMTAG_VERSION)
                .Cast<RpmStruct.RPMEntry?>()
                .FirstOrDefault();
            if(entry != null)
            {
                long startPosition = _headerSection.GetStartPositionFirstEntry();
                byte[][] data = ReadDataEntry(startPosition, entry.Value);
                if(data.Length > 0)
                {
                    return System.Text.Encoding.UTF8.GetString(data.ElementAt(0));
                }
            }
            return string.Empty;
        }

        private byte[][] ReadDataEntry(long startFirstEntryPosition, RpmStruct.RPMEntry entry)
        {
            Func<long, byte[]> func;
            if(_dataEntryReaders.TryGetValue((RpmConstants.rpmTagType)entry.Type, out func))
            {
                List<byte[]> data = new List<byte[]>();
                for(int i = 0; i < entry.Count; ++i)
                {
                    data.Add(func(startFirstEntryPosition + entry.Offset));
                }
                return data.ToArray();
            }
            else
            {
                throw new Exception(string.Format("Для тега типа {0} не реализована функция чтения данных", entry.Type));
            }
        }

        private bool FindBytes(byte[] bytes)
        {
            byte[] buffer = new byte[bytes.Length];
            if(_fileStream.Read(buffer, 0, bytes.Length) < bytes.Length)
            {
                return false;
            }
            while(!ByteArrayCompare(buffer, bytes))
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

        private bool ReadIntFromCurrentPosition(out int value)
        {
            const int size = sizeof(int);
            byte[] buffer = new byte[size];
            if (_fileStream.Read(buffer, 0, size) < size)
            {
                value = -1;
                return false;
            }
            Array.Reverse(buffer);
            value = BitConverter.ToInt32(buffer, 0);
            return true;
        }

        static bool ByteArrayCompare(byte[] a1, byte[] a2)
        {
            return StructuralComparisons.StructuralEqualityComparer.Equals(a1, a2);
        }

        /// <summary>
        /// Чтение данных типа 6
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        private byte[] ReadStringTagType(long position)
        {
            _fileStream.Seek(position, SeekOrigin.Begin);
            byte sym;
            List<byte> data = new List<byte>();
            while(_fileStream.CanRead)
            {
                sym = (byte)_fileStream.ReadByte();
                if(sym == '\0')
                {
                    break;
                }
                data.Add(sym);
            }
            return data.ToArray();
        }   

        public void Dispose()
        {
            if (isDisposed) return;

            _fileStream.Dispose();
            isDisposed = true;
        }
    }
}
﻿using RpmReaderNetLib.Extension;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace RpmReaderNet.Section
{
    /// <summary>
    /// Reader of signature section
    /// </summary>
    internal class RpmSignatureSection
        : AbstractHeaderSection
    {
        public RpmStruct.RPMSignature Signature = new RpmStruct.RPMSignature();

        public uint Size
        {
            get { return _size.Value; }
        }

        public uint PayloadSize
        {
            get { return _payloadSize.Value; }
        }

        /// <summary>
        /// MD5 signatures of files from package
        /// </summary>
        public byte[][] MD5
        {
            get { return _md5.Value; }
        }

        /// <summary>
        /// a GPG signature of package
        /// </summary>
        public byte[] GPG
        {
            get { return _gpg.Value; }
        }

        /// <summary>
        /// a PGP signature of package
        /// </summary>
        public byte[] PGP
        {
            get { return _gpg.Value; }
        }

        /// <summary>
        /// a SHA hash of package
        /// </summary>
        public string SHA1
        {
            get { return _sha1.Value; }
        }

        /// <summary>
        /// a DSA signature of package
        /// </summary>
        public byte[] DSA
        {
            get { return _dsa.Value; }
        }

        /// <summary>
        /// a RSA signature of package
        /// </summary>
        public byte[] RSA
        {
            get { return _rsa.Value; }
        }

        private Lazy<uint> _size;
        private Lazy<uint> _payloadSize;
        private Lazy<byte[][]> _md5;
        private Lazy<byte[]> _gpg;
        private Lazy<byte[]> _pgp;
        private Lazy<string> _sha1;
        private Lazy<byte[]> _dsa;
        private Lazy<byte[]> _rsa;

        public RpmSignatureSection(FileStream file)
            : base(file)
        {
            _size = new Lazy<uint>(() => (GetInt32FromTag((int)(RpmConstants.rpmSigTag.RPMSIGTAG_SIZE))).ReverseBytes());
            _payloadSize = new Lazy<uint>(() => (GetInt32FromTag((int)(RpmConstants.rpmSigTag.RPMSIGTAG_PAYLOADSIZE))).ReverseBytes());
            _md5 = new Lazy<byte[][]>(() => GetArrayBinDataFromTag((int)RpmConstants.rpmSigTag.RPMSIGTAG_MD5));
            _gpg = new Lazy<byte[]>(() => GetBinDataFromTag((int)RpmConstants.rpmSigTag.RPMSIGTAG_GPG));
            _pgp = new Lazy<byte[]>(() => GetBinDataFromTag((int)RpmConstants.rpmSigTag.RPMSIGTAG_PGP));
            _sha1 = new Lazy<string>(() => GetStringFromStringTypeTag((int)RpmConstants.rpmSigTag.RPMSIGTAG_SHA1));
            _dsa = new Lazy<byte[]>(() => GetBinDataFromTag((int)RpmConstants.rpmSigTag.RPMSIGTAG_DSA));
            _rsa = new Lazy<byte[]>(() => GetBinDataFromTag((int)RpmConstants.rpmSigTag.RPMSIGTAG_RSA));
        }

        public bool FillHeaderData(byte[] data)
        {
            int len = Marshal.SizeOf(Signature.GetType());
            IntPtr @in = Marshal.AllocHGlobal(len);
            Marshal.Copy(data, 0, @in, len);
            Signature = (RpmStruct.RPMSignature)Marshal.PtrToStructure(@in, Signature.GetType());
            Marshal.FreeHGlobal(@in);
            unchecked
            {
                Signature.bytesDataCount = (int)(((uint)Signature.bytesDataCount).ReverseBytes());
                Signature.entryCount = (int)(((uint)Signature.entryCount).ReverseBytes());
                byte[] buffer = new byte[RpmStruct.RPM_MAGIC_SIGNATURE_NUMBER.Length];
                unsafe
                {
                    fixed (byte* ptr = Signature.magic)
                    {
                        int i = 0;
                        for (byte* d = ptr; i < RpmStruct.RPM_MAGIC_SIGNATURE_NUMBER.Length; ++i, ++d)
                        {
                            buffer[i] = *d;
                        }
                    }
                }
                return ByteArrayCompare(buffer, RpmStruct.RPM_MAGIC_SIGNATURE_NUMBER);
            }
        }

        public bool FillHeaderEntry(byte[] data, int countEntry)
        {
            int len = Marshal.SizeOf(typeof(RpmStruct.RPMEntry));
            _entries = new RpmStruct.RPMEntry[countEntry];
            for (int i = 0; i < countEntry; ++i)
            {
                RpmStruct.RPMEntry entry = new RpmStruct.RPMEntry();
                IntPtr @in = Marshal.AllocHGlobal(len);
                Marshal.Copy(data, len * i, @in, len);
                entry = (RpmStruct.RPMEntry)Marshal.PtrToStructure(@in, typeof(RpmStruct.RPMEntry));
                Marshal.FreeHGlobal(@in);
                entry.Count = entry.Count.ReverseBytes();
                entry.Offset = entry.Offset.ReverseBytes();
                entry.Tag = entry.Tag.ReverseBytes();
                entry.Type = entry.Type.ReverseBytes();
                _entries[i] = entry;
            }
            return true;
        }

        public override long GetStartPositionFirstEntry()
        {
            return StartPosition + 16 + Signature.entryCount * SIZE_ONE_ENTRY;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(string.Format("Size: {0}\n", Size));
            builder.Append(string.Format("PayloadSize: {0}\n", PayloadSize));

            /*if (MD5 != null)
            {
                //builder.Append(string.Format("MD5 Signature: {0}\n", Encoding.UTF8.GetString(MD5)));
            }*/
            if (GPG != null)
                builder.Append(string.Format("GPG Signature: {0}\n", Encoding.UTF8.GetString(GPG)));
            if (PGP != null)
                builder.Append(string.Format("PGP Signature: {0}\n", Encoding.UTF8.GetString(PGP)));
            if (SHA1 != null)
                builder.Append(string.Format("SHA1 Signature: {0}\n", SHA1));
            if (RSA != null)
                builder.Append(string.Format("RSA Signature: {0}\n", Encoding.UTF8.GetString(RSA)));
            if (DSA != null)
                builder.Append(string.Format("DSA Signature: {0}\n", Encoding.UTF8.GetString(DSA)));

            return builder.ToString();
        }
    }
}
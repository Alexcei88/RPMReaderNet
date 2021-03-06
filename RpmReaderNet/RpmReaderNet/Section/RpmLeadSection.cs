﻿using System;
using System.IO;
using System.Runtime.InteropServices;

namespace RpmReaderNet.Section
{
    /// <summary>
    /// Reader of lead section 
    /// </summary>
    internal class RpmLeadSection
        : RpmSection
    {
        public RpmStruct.rpmlead Lead = new RpmStruct.rpmlead();

        public RpmLeadSection(FileStream file)
            : base(file)
        {
            StartPosition = 0;
        }

        public bool FillSection(byte[] data)
        {
            int len = Marshal.SizeOf(Lead);
            IntPtr @in = Marshal.AllocHGlobal(len);
            Marshal.Copy(data, 0, @in, len);
            Lead = (RpmStruct.rpmlead)Marshal.PtrToStructure(@in, Lead.GetType());
            Marshal.FreeHGlobal(@in);
            byte[] buffer = new byte[RpmStruct.RPM_MAGIC_LEAD_NUMBER.Length];
            unsafe
            {
                fixed (byte* ptr = Lead.magic)
                {
                    int i = 0;
                    for (byte* d = ptr; i < RpmStruct.RPM_MAGIC_LEAD_NUMBER.Length; ++i, ++d)
                    {
                        buffer[i] = *d;
                    }
                }
            }
            return ByteArrayCompare(buffer, RpmStruct.RPM_MAGIC_LEAD_NUMBER);
        }
    }
}
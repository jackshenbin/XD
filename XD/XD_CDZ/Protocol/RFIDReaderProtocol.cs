using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RFIDREAD
{

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct ISO14443A_UIDPARAM
    {
        public HFREADER_OPRESULT result;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
        public ISO14443A_UID[] uid;
        public uint remainNum;
        public uint num;
    }
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct ISO14443A_UID
    {
        public uint type;
        public uint len;
        public uint sak;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] uid;
    };

    [StructLayout(LayoutKind.Sequential,  Pack = 4)]
    public struct HFREADER_OPRESULT
    {
        public uint srcAddr;
        public uint targetAddr;
        public uint flag;
        public uint errType;
        public uint t;
    };


    [StructLayout(LayoutKind.Sequential,  Pack = 4)]
    public struct HFREADER_CONFIG
    {
        public HFREADER_OPRESULT result;
        public uint workMode;
        public uint readerAddr;
        public uint cmdMode;
        public uint afiCtrl;
        public uint uidSendMode;
        public uint tagStatus;
        public uint baudrate;
        public uint beepStatus;
        public uint afi;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct ISO14443A_BLOCKPARAM
    {
        public HFREADER_OPRESULT result;
        public ISO14443A_UID uid;
        public uint keyType;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] key;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0xd0)]
        public byte[] block;
        public uint addr;
        public uint num;
    }
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct ISO14443A_VALUEPARAM
    {
        public HFREADER_OPRESULT result;
        public ISO14443A_UID uid;
        public uint keyType;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] key;
        public uint opCode;
        public uint blockAddr;
        public uint transAddr;
        public int value;
    }


    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct ISO14443A_OPPARAM
    {
        public HFREADER_OPRESULT result;
        public ISO14443A_UID uid;
        public uint keyType;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] key;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x100)]
        public byte[] txFrame;
        public uint txLen;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x100)]
        public byte[] rxFrame;
        public uint rxLen;
    }




    public class protocol
    {
        public static uint HFREADER_CFG_AFI_DISABLE = 0;
        public static uint HFREADER_CFG_BAUDRATE38400 = 7;
        public static uint HFREADER_CFG_BUZZER_DISABLE = 0;
        public static uint HFREADER_CFG_BUZZER_ENABLE = 1;
        public static uint HFREADER_CFG_INVENTORY_TRIGGER = 1;
        public static uint HFREADER_CFG_QUERY_DISABLE = 1;
        public static uint HFREADER_CFG_UID_POSITIVE = 1;
        public static uint HFREADER_CFG_TYPE_ISO14443A = 1;
        public static uint HFREADER_CFG_WM_INVENTORY = 2;
        public static byte HFREADER_READ_UID_REQALL = 0x52;
        public static int USER_PASSWORD_LENGTH = 6;
        public const uint HFREADER_ISO14443A_LEN_M1_KEY = 6;
        public const uint HFREADER_ISO14443A_LEN_M1BLOCK = 16;

        public static byte HFREADER_ISO14443A_LEN_SIGNAL_UID = 4;
        const string DLLPath = @"HFReader.dll";

        //public static byte[] block1Key = new byte[] { 0x0E, 0x73, 0x1D, 0x87, 0xB5, 0x9E, 0x28, 0xC9, 0x77, 0x1D, 0x97, 0xFE, 0x34, 0xBB, 0x7A, 0xAF };
        public static byte[] block1Key = new byte[] { 0x88, 0x68, 0x3A, 0x58, 0xDC, 0x9B, 0x78, 0x77, 0x88, 0x69, 0x88, 0x68, 0x3A, 0x58, 0xDC, 0x9C };
        //public static byte[] block16Key = new byte[] { 0xF2, 0xCB, 0xE3, 0x0B, 0xF3, 0x88, 0x8E, 0x74, 0xC2, 0x51, 0xFF, 0x89, 0x66, 0x93, 0x36, 0xB3 };
        public static byte[] block16Key = new byte[] { 0x88, 0x68, 0xB7, 0x24, 0x38, 0x39, 0x7B, 0x47, 0x88, 0x69, 0x88, 0x68, 0xB7, 0x24, 0x38, 0x3A };
        //public static byte[] block32Key = new byte[] { 0x08, 0x39, 0x18, 0x60, 0xE6, 0xB3, 0x6D, 0xA9, 0x5A, 0x36, 0x95, 0x9B, 0xA0, 0xE1, 0xFD, 0x9F };
        public static byte[] block32Key = new byte[] { 0x88, 0x68, 0x9F, 0x54, 0xDA, 0x18, 0x6A, 0x57, 0x89, 0x69, 0x88, 0x68, 0x9F, 0x54, 0xDA, 0x19 };

        public static int tagMode = 1;

        [DllImport(DLLPath)]
        public static extern int iso14443AGetUID(int h, ushort srcAddr, ushort targetAddr,
        byte mode, ref ISO14443A_UIDPARAM pUid,
        byte[] pTxFrame, byte[] pRxFrame);

        [DllImport(DLLPath)]
        public static extern int hfReaderGetConfig(int h, ushort srcAddr, ushort targetAddr,
        ref HFREADER_CONFIG pConfig, byte[] pTxFrame, byte[] pRxFrame);

        [DllImport(DLLPath)]
        public static extern int hfReaderSetConfig(int h, ushort srcAddr, ushort targetAddr,
        ref HFREADER_CONFIG pConfig, byte[] pTxFrame, byte[] pRxFrame);

        [DllImport(DLLPath)]
        public static extern int hfReaderOpenUsb(ushort VID, ushort PID);

        [DllImport(DLLPath)]
        public static extern int hfReaderCloseUsb(int h);

        [DllImport(DLLPath)]
        public static extern int hfReaderClosePort(int h);

        [DllImport(DLLPath)]
        public static extern int hfReaderOpenPort(string pPortName, string pBaudrate);

        [DllImport(DLLPath)]
        public static extern int iso14443AAuthReadM1Block(int h, ushort srcAddr, ushort targetAddr, ref ISO14443A_BLOCKPARAM pBlock, IntPtr pTxFrame, IntPtr pRxFrame);

        [DllImport(DLLPath)]
        public static extern int iso14443AAuthReadM1Value(int h, ushort srcAddr, ushort targetAddr, ref ISO14443A_VALUEPARAM pValue, byte[] pTxFrame, byte[] pRxFrame);

        [DllImport(DLLPath)]
        public static extern int iso14443AAuthWriteM1Block(int h, ushort srcAddr, ushort targetAddr, ref ISO14443A_BLOCKPARAM pBlock, IntPtr pTxFrame, IntPtr pRxFrame);

        [DllImport(DLLPath)]
        public static extern int iso14443AAuthWriteM1Value(int h, ushort srcAddr, ushort targetAddr, ref ISO14443A_VALUEPARAM pValue, byte[] pTxFrame, byte[] pRxFrame);

        [DllImport(DLLPath)]
        public static extern int iso14443AHalt(int h, ushort srcAddr, ushort targetAddr, ref ISO14443A_OPPARAM pOpInfo, byte[] pTxFrame, byte[] pRxFrame);

        [DllImport(DLLPath)]
        public static extern int iso14443AAuthOpM1Value(int h, ushort srcAddr, ushort targetAddr, ref ISO14443A_VALUEPARAM pValue, byte[] pTxFrame, byte[] pRxFrame);
    }
}

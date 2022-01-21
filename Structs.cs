using System.Runtime.InteropServices;

namespace ScreenResolutionChange
{
    public class SystemMethods
    {
        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool EnumDisplaySettings([param: MarshalAs(UnmanagedType.LPWStr)] string lpszDeviceName, [param: MarshalAs(UnmanagedType.U4)] int iModeNum,
                [In, Out] ref DEVMODE lpDevMode);

        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.I4)]
        internal static extern int ChangeDisplaySettingsEx([param: MarshalAs(UnmanagedType.LPWStr)] string lpszDeviceName, [In, Out] ref DEVMODE lpDevMode, IntPtr hwnd,
            [param: MarshalAs(UnmanagedType.U4)] uint dwflags, IntPtr lParam);

        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool EnumDisplayDevices([param: MarshalAs(UnmanagedType.LPWStr)] string lpDevice, [param: MarshalAs(UnmanagedType.U4)] uint iDevNum,
           [In, Out] ref DISPLAY_DEVICE lpDisplayDevice, [param: MarshalAs(UnmanagedType.U4)] uint dwFlags);
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct DEVMODE
    {
        // You can define the following constant
        // but OUTSIDE the structure because you know
        // that size and layout of the structure
        // is very important
        // CCHDEVICENAME = 32 = 0x50
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string dmDeviceName;
        // In addition you can define the last character array
        // as following:
        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        //public Char[] dmDeviceName;

        // After the 32-bytes array
        [MarshalAs(UnmanagedType.U2)]
        public ushort dmSpecVersion;

        [MarshalAs(UnmanagedType.U2)]
        public ushort dmDriverVersion;

        [MarshalAs(UnmanagedType.U2)]
        public ushort dmSize;

        [MarshalAs(UnmanagedType.U2)]
        public ushort dmDriverExtra;

        [MarshalAs(UnmanagedType.U4)]
        public uint dmFields;

        public POINTL dmPosition;

        [MarshalAs(UnmanagedType.U4)]
        public uint dmDisplayOrientation;

        [MarshalAs(UnmanagedType.U4)]
        public uint dmDisplayFixedOutput;

        [MarshalAs(UnmanagedType.I2)]
        public short dmColor;

        [MarshalAs(UnmanagedType.I2)]
        public short dmDuplex;

        [MarshalAs(UnmanagedType.I2)]
        public short dmYResolution;

        [MarshalAs(UnmanagedType.I2)]
        public short dmTTOption;

        [MarshalAs(UnmanagedType.I2)]
        public short dmCollate;

        // CCHDEVICENAME = 32 = 0x50
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string dmFormName;
        // Also can be defined as
        //[MarshalAs(UnmanagedType.ByValArray,
        //    SizeConst = 32, ArraySubType = UnmanagedType.U1)]
        //public Byte[] dmFormName;

        [MarshalAs(UnmanagedType.U2)]
        public ushort dmLogPixels;

        [MarshalAs(UnmanagedType.U4)]
        public uint dmBitsPerPel;

        [MarshalAs(UnmanagedType.U4)]
        public uint dmPelsWidth;

        [MarshalAs(UnmanagedType.U4)]
        public uint dmPelsHeight;

        [MarshalAs(UnmanagedType.U4)]
        public uint dmDisplayFlags;

        [MarshalAs(UnmanagedType.U4)]
        public uint dmDisplayFrequency;

        [MarshalAs(UnmanagedType.U4)]
        public uint dmICMMethod;

        [MarshalAs(UnmanagedType.U4)]
        public uint dmICMIntent;

        [MarshalAs(UnmanagedType.U4)]
        public uint dmMediaType;

        [MarshalAs(UnmanagedType.U4)]
        public uint dmDitherType;

        [MarshalAs(UnmanagedType.U4)]
        public uint dmReserved1;

        [MarshalAs(UnmanagedType.U4)]
        public uint dmReserved2;

        [MarshalAs(UnmanagedType.U4)]
        public uint dmPanningWidth;

        [MarshalAs(UnmanagedType.U4)]
        public uint dmPanningHeight;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct POINTL
    {
        [MarshalAs(UnmanagedType.I4)]
        public int x;
        [MarshalAs(UnmanagedType.I4)]
        public int y;
    }


    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct DISPLAY_DEVICE
    {
        [MarshalAs(UnmanagedType.U4)]
        public int cb;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string DeviceName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string DeviceString;
        [MarshalAs(UnmanagedType.U4)]
        public DisplayDeviceStateFlags StateFlags;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string DeviceID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string DeviceKey;
    }

    [Flags()]
    public enum DisplayDeviceStateFlags : int
    {
        /// <summary>The device is part of the desktop.</summary>
        AttachedToDesktop = 0x1,
        MultiDriver = 0x2,
        /// <summary>The device is part of the desktop.</summary>
        PrimaryDevice = 0x4,
        /// <summary>Represents a pseudo device used to mirror application drawing for remoting or other purposes.</summary>
        MirroringDriver = 0x8,
        /// <summary>The device is VGA compatible.</summary>
        VGACompatible = 0x10,
        /// <summary>The device is removable; it cannot be the primary display.</summary>
        Removable = 0x20,
        /// <summary>The device has more display modes than its output devices support.</summary>
        ModesPruned = 0x8000000,
        Remote = 0x4000000,
        Disconnect = 0x2000000
    }
}

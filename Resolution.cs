using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace ScreenResolutionChange
{
    internal enum Flag
    {
        DISP_CHANGE_SUCCESSFUL = 0,
        DISP_CHANGE_BADMODE = -2,
        DISP_CHANGE_FAILED = -1,
        DISP_CHANGE_RESTART = 1
    }

    public enum ResolutionEnum
    {
        FullHD = 0,
        UHD = 1,
        None = 2
    }

    internal class Resolution
    {
        private static void GetCurrentSettings(string monitor, ref DEVMODE mode)
        {
            mode.dmSize = (ushort)Marshal.SizeOf(mode);
            var ENUM_CURRENT_SETTINGS = -1;
            SystemMethods.EnumDisplaySettings(monitor, ENUM_CURRENT_SETTINGS, ref mode);
        }

        private static bool IsUHD(string monitor)
        {
            DEVMODE mode = new();
            mode.dmSize = (ushort)Marshal.SizeOf(mode);

            for (int i = 0; SystemMethods.EnumDisplaySettings(monitor, i, ref mode); i++)
            {
                if (mode.dmPelsWidth == 2560) return true;
            }
            return false;
        }

        // Change the scale of the Monitor (0 = 100%, 1 = 125%)
        private static int ChangeDPI(int dpi)
        {
            int originalDpi;

            RegistryKey? key = Registry.CurrentUser.OpenSubKey("Control Panel", true);
            key = key?.OpenSubKey("Desktop", true);
            key = key?.OpenSubKey("PerMonitorSettings", true);
            key = key?.OpenSubKey("AOC270230894_11_07E5_17^0FF1FA49F6A10A4AFCD1330DB3BA05EE", true);
            originalDpi = (int)(key?.GetValue("DpiValue") ?? 0);
            key?.SetValue("DpiValue", dpi);

            key = Registry.CurrentUser.OpenSubKey("Control Panel", true);
            key = key?.OpenSubKey("Desktop", true);
            key = key?.OpenSubKey("PerMonitorSettings", true);
            key = key?.OpenSubKey("AOC270230723_11_07E5_C2^B3F5565BA4CDB080E2DC645481899D42", true);
            key?.SetValue("DpiValue", dpi);

            return originalDpi;
        }      
        
        public static ResolutionEnum GetCurrentResolution()
        {
            var device = "\\\\.\\DISPLAY1";
            DEVMODE mode = new();
            mode.dmSize = (ushort)Marshal.SizeOf(mode);
            GetCurrentSettings(device, ref mode);

            if (mode.dmPelsWidth == 2560)
                return ResolutionEnum.UHD;
            else if (mode.dmPelsWidth == 1920)
                return ResolutionEnum.FullHD;
            return ResolutionEnum.None;
        }

        public static void ChangeDisplaySettings(int width, int height, int frequency, int dpi)
        {
            var deviceNames = GetDisplayNames();

            var i = 0;
            foreach (var device in deviceNames)
            {
                DEVMODE originalMode = new();
                originalMode.dmSize = (ushort)Marshal.SizeOf(originalMode);

                // Retrieving current settings to edit them
                GetCurrentSettings(device, ref originalMode);

                // Making a copy of the current settings to allow resetting to the original mode
                DEVMODE newMode = originalMode;

                var originalDpi = 0;

                if (IsUHD(device))
                {
                    // Changing the settings
                    newMode.dmPelsWidth = (uint)width;
                    newMode.dmPelsHeight = (uint)height;
                    newMode.dmDisplayFrequency = (uint)frequency;
                    newMode.dmPosition.y = 0;
                    originalDpi = ChangeDPI(dpi);
                }
                else if (width == 1920)
                {
                    // The Full HD monitor is only moved
                    newMode.dmPosition.y = 0;
                }
                else if (width == 2560)
                {
                    newMode.dmPosition.y = 360;
                }

                // Capturing the operation result
                int result = SystemMethods.ChangeDisplaySettingsEx(device, ref newMode, IntPtr.Zero, 0, IntPtr.Zero);

                if (result != (int)Flag.DISP_CHANGE_SUCCESSFUL)
                {
                    _ = SystemMethods.ChangeDisplaySettingsEx(device, ref originalMode, IntPtr.Zero, 0, IntPtr.Zero);
                    ChangeDPI(originalDpi);
                    MessageBox.Show($"Failed. Error code = {Enum.GetName(typeof(Flag), result)}");
                }
                i++;
            }
        }

        // Get all attached displays
        public static List<string> GetDisplayNames()
        {
            DISPLAY_DEVICE lpDisplayDevice = new();
            lpDisplayDevice.cb = Marshal.SizeOf(lpDisplayDevice);
            DISPLAY_DEVICE displaySettings = new();
            displaySettings.cb = Marshal.SizeOf(displaySettings);

            uint devNum = 0;
            var deviceNames = new List<string>();
            while (SystemMethods.EnumDisplayDevices(null, devNum, ref lpDisplayDevice, 0))
            {               
                SystemMethods.EnumDisplayDevices(lpDisplayDevice.DeviceName, 0, ref displaySettings, 0);
                if(displaySettings.StateFlags.HasFlag(DisplayDeviceStateFlags.AttachedToDesktop))
                    deviceNames.Add(lpDisplayDevice.DeviceName);
                ++devNum;
            }

            return deviceNames;
        }

        // Dubug Method
        public static string GetSupportedModes()
        {
            DEVMODE mode = new();
            mode.dmSize = (ushort)Marshal.SizeOf(mode);

            int modeIndex = 0; // 0 = The first mode

            var msg = "";

            while (SystemMethods.EnumDisplaySettings("\\\\.\\DISPLAY3", modeIndex, ref mode) == true) // Mode found
            {
                msg += $"{mode.dmPelsWidth} x {mode.dmPelsHeight} @ {mode.dmDisplayFrequency}\n";
                modeIndex++; // The next mode
            }

            return msg;
        }
    }
}

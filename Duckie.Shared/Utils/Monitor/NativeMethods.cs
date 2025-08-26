using System.Runtime.InteropServices;
using System.Windows;

namespace Duckie.Utils.Monitor;

public static class NativeMethods
{
    /// <summary>
    /// 获取显示器句柄的 Win32 函数
    /// </summary>
    [DllImport("user32.dll")]
    public static extern IntPtr MonitorFromPoint(POINT pt, MonitorOptions dwFlags);

    /// <summary>
    /// 获取显示器信息的 Win32 函数
    /// </summary>
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFOEX lpmi);

    /// <summary>
    /// 获取显示器 DPI 的 Win32 函数
    /// </summary>
    [DllImport("Shcore.dll")]
    public static extern int GetDpiForMonitor(IntPtr hmonitor, MonitorDpiType dpiType, out uint dpiX, out uint dpiY);

    /// <summary>
    /// 定义 MonitorFromPoint 选项
    /// </summary>
    public enum MonitorOptions : uint
    {
        MONITOR_DEFAULTTONULL = 0,
        MONITOR_DEFAULTTOPRIMARY = 1,
        MONITOR_DEFAULTTONEAREST = 2
    }

    /// <summary>
    /// 定义 GetDpiForMonitor 选项
    /// </summary>
    public enum MonitorDpiType
    {
        MDT_EFFECTIVE_DPI = 0,
        MDT_ANGULAR_DPI = 1,
        MDT_RAW_DPI = 2,
    }

    /// <summary>
    /// 定义 POINT 结构体
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int x;
        public int y;
    }

    /// <summary>
    /// 定义 RECT 结构体
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;

        public Rect ToWindowRect()
        {
            return new Rect(new Point(left, top), new Size(right - left, bottom - top));
        }
    }

    /// <summary>
    /// 定义 MONITORINFOEX 结构体，用于接收显示器信息
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct MONITORINFOEX
    {
        public int cbSize;
        public RECT rcMonitor;
        public RECT rcWork;
        public uint dwFlags;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string szDevice;
    }
}

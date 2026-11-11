using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using static Duckie.Utils.Monitor.NativeMethods;

namespace Duckie.Utils.Monitor;

public static class MonitorUtils
{
    public static MonitorInfo GetPrimaryDisplayInfo()
    {
        // 1. 使用 (0,0) 点获取主显示器句柄
        var point = new POINT { x = 0, y = 0 };
        var hMonitor = MonitorFromPoint(point, MonitorOptions.MONITOR_DEFAULTTOPRIMARY);

        if (hMonitor == IntPtr.Zero)
        {
            // 如果失败，可以抛出异常或返回 null
            throw new InvalidOperationException("Could not get primary monitor handle.");
        }

        // 2. 获取显示器 DPI
        GetDpiForMonitor(hMonitor, MonitorDpiType.MDT_EFFECTIVE_DPI, out uint dpiX, out uint _);
        var scaleFactor = dpiX / 96.0; // 96 DPI 是 100% 缩放

        // 3. 获取显示器信息（包括工作区）
        var monitorInfo = new MONITORINFOEX();
        monitorInfo.cbSize = Marshal.SizeOf(monitorInfo); // 关键一步：必须设置大小
        if (!GetMonitorInfo(hMonitor, ref monitorInfo))
        {
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        // 4. 计算逻辑工作区
        var physicalWorkArea = monitorInfo.rcWork;
        var logicalWorkArea = new Rect(
            physicalWorkArea.left / scaleFactor,
            physicalWorkArea.top / scaleFactor,
            (physicalWorkArea.right - physicalWorkArea.left) / scaleFactor,
            (physicalWorkArea.bottom - physicalWorkArea.top) / scaleFactor
        );

        return new MonitorInfo
        {
            ScaleFactor = scaleFactor,
            Dpi = dpiX,
            PhysicalWorkArea = physicalWorkArea.ToWindowRect(),
            LogicalWorkArea = logicalWorkArea
        };
    }
}

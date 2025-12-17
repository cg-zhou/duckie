using Duckie.Shared.Utils.Ui;
using System.Windows;
using System.Windows.Controls;

namespace Duckie.Windows.Services.Terminal;

/// <summary>
/// 终端管理视图
/// </summary>
public partial class TerminalManagerView : UserControl
{
    public TerminalManagerView()
    {
        InitializeComponent();
    }

    private void TestTerminal_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var currentPath = WindowsUtils.GetCurrentExplorerPath();
            TerminalUtils.OpenAt(currentPath);
            UiUtils.Info($"终端已在路径打开: {currentPath}", "终端测试");
        }
        catch (Exception ex)
        {
            UiUtils.Warning($"终端测试失败: {ex.Message}", "终端测试");
        }
    }

    private void OpenDefaultPath_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            TerminalUtils.OpenAt(userProfile);
            UiUtils.Info($"终端已在用户主目录打开: {userProfile}", "终端测试");
        }
        catch (Exception ex)
        {
            UiUtils.Warning($"打开用户主目录失败: {ex.Message}", "终端测试");
        }
    }

    private void DebugPath_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var currentPath = WindowsUtils.GetCurrentExplorerPath();
            var isValid = WindowsUtils.IsValidPath(currentPath);
            
            // 获取前台窗口信息
            var foregroundWindow = GetForegroundWindow();
            var windowTitle = GetWindowTitle(foregroundWindow);
            var processName = GetProcessName(foregroundWindow);
            
            var message = $"=== 路径调试信息 ===\n" +
                         $"检测到的路径: {currentPath}\n" +
                         $"路径是否有效: {isValid}\n" +
                         $"路径是否存在: {System.IO.Directory.Exists(currentPath)}\n\n" +
                         $"=== 前台窗口信息 ===\n" +
                         $"窗口句柄: 0x{foregroundWindow.ToString("X")}\n" +
                         $"窗口标题: {windowTitle}\n" +
                         $"进程名称: {processName}\n\n" +
                         $"如果路径检测失败，请确保:\n" +
                         $"1. 当前活动窗口是文件资源管理器\n" +
                         $"2. 不是在特殊位置（如控制面板）\n" +
                         $"3. Windows 版本支持此功能";
                         
            UiUtils.Info(message, "路径调试信息");
        }
        catch (Exception ex)
        {
            UiUtils.Warning($"调试失败: {ex.Message}", "路径调试");
        }
    }

    [System.Runtime.InteropServices.DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [System.Runtime.InteropServices.DllImport("user32.dll")]
    private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

    [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
    private static extern int GetWindowText(IntPtr hWnd, System.Text.StringBuilder text, int count);

    [System.Runtime.InteropServices.DllImport("user32.dll")]
    private static extern int GetWindowTextLength(IntPtr hWnd);

    private string GetWindowTitle(IntPtr hwnd)
    {
        try
        {
            var length = GetWindowTextLength(hwnd);
            if (length == 0) return "无标题";

            var title = new System.Text.StringBuilder(length + 1);
            GetWindowText(hwnd, title, title.Capacity);
            return title.ToString();
        }
        catch
        {
            return "获取失败";
        }
    }

    private string GetProcessName(IntPtr hwnd)
    {
        try
        {
            GetWindowThreadProcessId(hwnd, out uint processId);
            var process = System.Diagnostics.Process.GetProcessById((int)processId);
            return process.ProcessName;
        }
        catch
        {
            return "未知进程";
        }
    }
}

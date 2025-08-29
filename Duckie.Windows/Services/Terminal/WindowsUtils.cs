using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Duckie.Windows.Services.Terminal;

public static class WindowsUtils
{
    #region Win32 API 声明

    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

    #endregion

    /// <summary>
    /// 获取当前活动的文件资源管理器路径
    /// </summary>
    /// <returns>当前文件夹路径，如果获取失败则返回用户主目录</returns>
    public static string GetActiveExplorerPath()
    {
        try
        {
            var shellPath = GetActiveShellPath();
            if (!string.IsNullOrEmpty(shellPath))
            {
                return shellPath;
            }
        }
        catch (Exception)
        {
            // 忽略错误，返回默认路径
        }

        return string.Empty;
    }

    /// <summary>
    /// 通过 Shell.Application 获取活动窗口路径
    /// </summary>
    private static string GetActiveShellPath()
    {
        try
        {
            var foregroundWindow = GetForegroundWindow();
            if (foregroundWindow == IntPtr.Zero) return null;

            // 检查是否是 Explorer 窗口
            if (!IsExplorerWindow(foregroundWindow)) return null;

            var shellType = Type.GetTypeFromProgID("Shell.Application");
            if (shellType == null) return null;

            dynamic shell = Activator.CreateInstance(shellType);
            if (shell == null) return null;

            try
            {
                dynamic windows = shell.Windows();
                if (windows == null) return null;

                int count = windows.Count;

                for (int i = 0; i < count; i++)
                {
                    try
                    {
                        dynamic window = windows.Item(i);
                        if (window == null) continue;

                        long hwnd = window.HWND;

                        if (hwnd == foregroundWindow.ToInt64())
                        {
                            var path = ExtractPathFromDynamicWindow(window);
                            if (!string.IsNullOrEmpty(path))
                            {
                                return path;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        // 忽略单个窗口的错误
                    }
                }
            }
            catch (Exception)
            {
                // 忽略 COM 调用错误
            }
        }
        catch (Exception)
        {
            // 忽略所有错误
        }

        return null;
    }

    /// <summary>
    /// 从动态窗口对象中提取路径
    /// </summary>
    private static string ExtractPathFromDynamicWindow(dynamic window)
    {
        try
        {
            // 方法1: 尝试获取 LocationURL
            try
            {
                string locationUrl = window.LocationURL;
                if (!string.IsNullOrEmpty(locationUrl))
                {
                    var path = ConvertUrlToPath(locationUrl);
                    if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
                    {
                        return path;
                    }
                }
            }
            catch (Exception)
            {
                // 忽略错误
            }

            // 方法2: 通过 Document.Folder.Self.Path
            try
            {
                dynamic document = window.Document;
                if (document != null)
                {
                    dynamic folder = document.Folder;
                    if (folder != null)
                    {
                        dynamic self = folder.Self;
                        if (self != null)
                        {
                            string path = self.Path;
                            if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
                            {
                                return path;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                // 忽略错误
            }
        }
        catch (Exception)
        {
            // 忽略所有错误
        }

        return null;
    }

    /// <summary>
    /// 检查给定窗口是否是 Explorer 窗口
    /// </summary>
    private static bool IsExplorerWindow(IntPtr hWnd)
    {
        try
        {
            // 获取窗口类名
            var className = new StringBuilder(256);
            GetClassName(hWnd, className, className.Capacity);
            var classNameStr = className.ToString();

            // 获取进程 ID
            GetWindowThreadProcessId(hWnd, out uint processId);
            var process = Process.GetProcessById((int)processId);

            // 检查是否是 Explorer 进程的正确窗口类
            return process.ProcessName.Equals("explorer", StringComparison.OrdinalIgnoreCase) &&
                   (classNameStr == "CabinetWClass" || classNameStr == "ExploreWClass");
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// 将文件 URL 转换为本地路径
    /// </summary>
    private static string ConvertUrlToPath(string url)
    {
        if (string.IsNullOrEmpty(url)) return null;

        try
        {
            // 处理 file:// 协议
            if (url.StartsWith("file:///"))
            {
                var path = Uri.UnescapeDataString(url.Substring(8)).Replace('/', '\\');
                return path;
            }

            // 处理直接的文件路径
            if (url.Length >= 3 && char.IsLetter(url[0]) && url[1] == ':' && url[2] == '\\')
            {
                return url;
            }
        }
        catch
        {
            // 忽略转换错误
        }

        return null;
    }
}

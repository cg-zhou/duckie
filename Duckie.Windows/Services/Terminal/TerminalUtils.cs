using System.Diagnostics;
using System.IO;

namespace Duckie.Windows.Services.Terminal;

/// <summary>
/// 终端启动工具类
/// </summary>
public static class TerminalUtils
{
    /// <summary>
    /// 在指定路径打开终端
    /// </summary>
    /// <param name="path">要打开的路径</param>
    public static void OpenAt(string path)
    {
        if (!WindowsUtils.IsValidPath(path))
        {
            path = WindowsUtils.GetCurrentExplorerPath();
        }

        try
        {
            // 尝试启动 Windows Terminal
            if (TryOpenWindowsTerminal(path))
            {
                return;
            }

            // 降级到 PowerShell
            if (TryOpenPowerShell(path))
            {
                return;
            }

            // 最后降级到 CMD
            TryOpenCmd(path);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"打开终端失败: {ex.Message}");
            
            // 最后的降级方案：打开文件夹
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "explorer.exe",
                    Arguments = $"\"{path}\"",
                    UseShellExecute = true
                });
            }
            catch (Exception explorerEx)
            {
                Debug.WriteLine($"打开文件夹也失败: {explorerEx.Message}");
            }
        }
    }

    /// <summary>
    /// 尝试启动 Windows Terminal
    /// </summary>
    private static bool TryOpenWindowsTerminal(string path)
    {
        try
        {
            // 检查 Windows Terminal 是否存在
            if (!IsWindowsTerminalAvailable())
            {
                return false;
            }

            var startInfo = new ProcessStartInfo
            {
                FileName = "wt.exe",
                Arguments = $"-d \"{path}\"",
                UseShellExecute = true,
                CreateNoWindow = true
            };

            Process.Start(startInfo);
            Debug.WriteLine($"Windows Terminal 已在路径打开: {path}");
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Windows Terminal 启动失败: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// 尝试启动 PowerShell
    /// </summary>
    private static bool TryOpenPowerShell(string path)
    {
        try
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = $"-NoExit -Command \"Set-Location '{path}'\"",
                UseShellExecute = true,
                CreateNoWindow = false
            };

            Process.Start(startInfo);
            Debug.WriteLine($"PowerShell 已在路径打开: {path}");
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"PowerShell 启动失败: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// 尝试启动 CMD
    /// </summary>
    private static bool TryOpenCmd(string path)
    {
        try
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/K cd /d \"{path}\"",
                UseShellExecute = true,
                CreateNoWindow = false
            };

            Process.Start(startInfo);
            Debug.WriteLine($"CMD 已在路径打开: {path}");
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"CMD 启动失败: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// 检查 Windows Terminal 是否可用
    /// </summary>
    private static bool IsWindowsTerminalAvailable()
    {
        try
        {
            // 方法1：检查是否能找到 wt.exe
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "where.exe",
                    Arguments = "wt.exe",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            
            process.Start();
            var output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            
            return process.ExitCode == 0 && !string.IsNullOrWhiteSpace(output);
        }
        catch
        {
            // 方法2：尝试通过注册表或其他方式检查
            try
            {
                // 检查常见的安装路径
                var commonPaths = new[]
                {
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), 
                        "Microsoft", "WindowsApps", "wt.exe"),
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), 
                        "WindowsApps", "Microsoft.WindowsTerminal_*", "wt.exe")
                };

                foreach (var path in commonPaths)
                {
                    if (path.Contains("*"))
                    {
                        // 处理通配符路径
                        var directory = Path.GetDirectoryName(path);
                        var pattern = Path.GetFileName(path);
                        
                        if (Directory.Exists(Path.GetDirectoryName(directory)))
                        {
                            var parentDir = Directory.GetParent(directory).FullName;
                            var dirs = Directory.GetDirectories(parentDir, "Microsoft.WindowsTerminal_*");
                            
                            foreach (var dir in dirs)
                            {
                                var wtPath = Path.Combine(dir, "wt.exe");
                                if (File.Exists(wtPath))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                    else if (File.Exists(path))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                // 忽略错误
            }
            
            return false;
        }
    }
}

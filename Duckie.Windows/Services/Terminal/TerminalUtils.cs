using System.Diagnostics;
using System.IO;

namespace Duckie.Windows.Services.Terminal;

public static class TerminalUtils
{
    public static void OpenAt(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return;
        }

        if (!Directory.Exists(path))
        {
            return;
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
        }
    }

    private static bool TryOpenWindowsTerminal(string path)
    {
        try
        {
            // 确保路径是有效的
            if (!Directory.Exists(path))
            {
                Debug.WriteLine($"路径不存在: {path}");
                return false;
            }

            path = path.Replace(@"\", "/");

            // 尝试通过 cmd 启动 wt
            var cmdStartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $@"/c wt --startingDirectory ""{path}""",
                UseShellExecute = true,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            using (var progress = Process.Start(cmdStartInfo))
            {
                return true;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Windows Terminal 启动失败: {ex.Message}");
            // 记录更详细的错误信息
            if (ex.InnerException != null)
            {
                Debug.WriteLine($"内部异常: {ex.InnerException.Message}");
            }
            return false;
        }
    }

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
}

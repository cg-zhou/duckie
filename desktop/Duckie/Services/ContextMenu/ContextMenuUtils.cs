using Microsoft.Win32;
using System.Diagnostics;

namespace Duckie.Services.ContextMenu;

public static class ContextMenuUtils
{
    private const string CLSID_KEY = @"SOFTWARE\Classes\CLSID\{86ca1aa0-34aa-4e8b-a509-50c905bae2a2}\InprocServer32";

    /// <summary>
    /// Enable Windows 10 style context menu
    /// </summary>
    public static void SetWindows10ContextMenu()
    {
        using var key = Registry.CurrentUser.CreateSubKey(CLSID_KEY);
        key?.SetValue("", "", RegistryValueKind.String);

        RestartExplorer();
    }

    /// <summary>
    /// Enable Windows 11 style context menu
    /// </summary>
    public static void SetWindows11ContextMenu()
    {
        Registry.CurrentUser.DeleteSubKeyTree(CLSID_KEY, false);

        RestartExplorer();
    }

    /// <summary>
    /// Check if Windows 10 style context menu is currently enabled
    /// </summary>
    public static bool IsWindows10ContextMenuEnabled()
    {
        using RegistryKey key = Registry.CurrentUser.OpenSubKey(CLSID_KEY);
        return key != null;
    }

    /// <summary>
    /// Toggle between Windows 10 and Windows 11 context menu styles
    /// </summary>
    public static void ToggleContextMenuStyle()
    {
        if (IsWindows10ContextMenuEnabled())
        {
            SetWindows11ContextMenu();
        }
        else
        {
            SetWindows10ContextMenu();
        }
    }

    /// <summary>
    /// Restart Windows Explorer to apply context menu changes
    /// </summary>
    private static void RestartExplorer()
    {
        RestartExplorerProcess();
    }

    /// <summary>
    /// Restart explorer.exe process (more aggressive method)
    /// </summary>
    private static void RestartExplorerProcess()
    {
        var explorerProcesses = Process.GetProcessesByName("explorer");
        foreach (var process in explorerProcesses)
        {
            try
            {
                process.Kill();
                process.WaitForExit(5000); // Wait up to 5 seconds
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to kill explorer process {process.Id}: {ex.Message}");
            }
            finally
            {
                process.Dispose();
            }
        }

        Thread.Sleep(1000);

        if (!Process.GetProcessesByName("explorer").Any())
        {
            Process.Start("explorer.exe");
        }
    }

    /// <summary>
    /// Check if the current OS supports context menu customization
    /// </summary>
    public static bool IsContextMenuCustomizationSupported()
    {
        try
        {
            // Check if running on Windows 11 or later (where this feature is relevant)
            var version = Environment.OSVersion.Version;
            return version.Major >= 10 && version.Build >= 22000; // Windows 11 build number
        }
        catch
        {
            return false;
        }
    }
}

using Duckie.Shared.Services.UserConfigs;
using Duckie.Shared.Utils.Ui;
using Duckie.Utils.HotKeys;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Duckie.Windows.Services.Terminal;

public class LauncherService : IHotKeyService
{
    private static readonly HotKeyAction Shortcut = new("启动配置的目标", KeyModifiers.None,
        Keys.LaunchApplication2, () =>
        {
            try { Launch(); }
            catch (Exception ex) { UiUtils.Warning(ex.Message, "启动失败"); }
        }, noRepeat: true);

    public IEnumerable<HotKeyAction> Register()
    {
        if (UserConfigService.Get().Launcher?.Enabled == true)
            yield return Shortcut;
    }

    public static void Validate(LauncherConfig config)
    {
        if (!config.Enabled) return;
        var target = Environment.ExpandEnvironmentVariables(config.Target ?? "");
        if (!Path.IsPathRooted(target) || !File.Exists(target))
            throw new InvalidOperationException("请选择存在的目标文件，使用完整路径。");
    }

    public static void Save(LauncherConfig config)
    {
        Validate(config);
        var userConfig = UserConfigService.Get();
        var previous = userConfig.Launcher;
        HotKeyManager.Unregister(Shortcut);
        try
        {
            if (config.Enabled) HotKeyManager.Register(Shortcut, Shortcut.Modifiers, Shortcut.Keys);
            userConfig.Launcher = config;
            UserConfigService.Set(userConfig);
        }
        catch
        {
            HotKeyManager.Unregister(Shortcut);
            if (previous?.Enabled == true)
            {
                try { HotKeyManager.Register(Shortcut, Shortcut.Modifiers, Shortcut.Keys); }
                catch (Exception ex) { UiUtils.Warning(ex.Message, "快捷键恢复失败"); }
            }
            throw;
        }
    }

    public static void Launch()
    {
        var config = UserConfigService.Get().Launcher ?? new LauncherConfig();
        if (!config.Enabled) throw new InvalidOperationException("请先在终端管理中启用并保存启动项配置。");
        Validate(config);
        using (var process = Process.Start(CreateStartInfo(config))) { }
    }

    public static ProcessStartInfo CreateStartInfo(LauncherConfig config)
    {
        Validate(config);
        var target = Environment.ExpandEnvironmentVariables(config.Target);
        var startInfo = new ProcessStartInfo
        {
            FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "cmd.exe"),
            // START 的第一个引号参数是窗口标题；关闭 AutoRun 和延迟变量展开。
            Arguments = "/d /v:off /c start \"\" \"%DUCKIE_LAUNCH_TARGET%\"",
            WorkingDirectory = Path.GetDirectoryName(target),
            UseShellExecute = false,
            CreateNoWindow = true
        };
        // 路径通过子进程环境变量传入，并在命令中加引号，避免拼接为命令语法。
        startInfo.EnvironmentVariables["DUCKIE_LAUNCH_TARGET"] = target;
        return startInfo;
    }
}

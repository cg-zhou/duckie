using Duckie.Shared.Services.UserConfigs;
using Duckie.Shared.Utils.Localization;
using Duckie.Shared.Utils.Ui;
using Duckie.Utils.HotKeys;
using System.Diagnostics;
using System.IO;

namespace Duckie.Windows.Services.Launcher;

public class LauncherService : IHotKeyService
{
    public IEnumerable<HotKeyAction> Register() => (UserConfigService.Get().Launchers ?? []).Select(CreateAction);
    public static HotKeyAction CreateAction(LauncherConfig item) => new(
        "launcher." + item.Id, LocKey.HotKey_LaunchTarget, new HotKeyBinding(),
        () => { try { Launch(item.Id); } catch (Exception ex) { UiUtils.Warning(ex.Message, LocKey.Launcher_LaunchFailed.Text()); } }, true);
    public static void Launch(string id)
    {
        var item = (UserConfigService.Get().Launchers ?? []).FirstOrDefault(x => x.Id == id)
            ?? throw new InvalidOperationException(LocKey.Launcher_NotFound.Text());
        var target = Environment.ExpandEnvironmentVariables(item.Target ?? "");
        if (!Path.IsPathRooted(target) || !File.Exists(target)) throw new InvalidOperationException(LocKey.Launcher_TargetMissing.Text());
        var info = new ProcessStartInfo
        {
            FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "cmd.exe"),
            Arguments = "/d /v:off /c start \"\" \"%DUCKIE_LAUNCH_TARGET%\"",
            WorkingDirectory = Path.GetDirectoryName(target), UseShellExecute = false, CreateNoWindow = true
        };
        info.EnvironmentVariables["DUCKIE_LAUNCH_TARGET"] = target;
        using var process = Process.Start(info);
    }
}

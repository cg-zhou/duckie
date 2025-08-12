using Duckie.Services.Clipboard;
using Duckie.Services.PacManager;
using Duckie.Shared.Services.UserConfigs;
using Duckie.Shared.Utils.Localization;
using Duckie.Shared.Utils.Ui;
using Duckie.Utils.HotKeys;
using System.Windows;
using System.Windows.Threading;

namespace Duckie;

public partial class App : Application
{
    private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        UiUtils.Warning($"Exception: {e.Exception.Message}{e.Exception.InnerException?.Message}", "Warning");
        e.Handled = true;
    }

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        LocUtils.Initialize();

        NotifyIconUtils.Initialize();
        NotifyIconUtils.RefreshContextMenu();

        HotKeyManager.RegisterServices();

        PacManagerService.RefreshIconBadge();

        var proxyConfig = UserConfigService.Get().Proxy;
        if (!string.IsNullOrWhiteSpace(proxyConfig?.ProxyUri))
        {
            HttpUtils.SetProxy(proxyConfig);
        }
    }

    private void Application_Exit(object sender, ExitEventArgs e)
    {
        NotifyIconUtils.Release();
    }
}

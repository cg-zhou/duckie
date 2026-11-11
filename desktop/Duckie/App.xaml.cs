using Duckie.Services.Network;
using Duckie.Services.PacManager;
using Duckie.Services;
using Duckie.Shared.Services.UserConfigs;
using Duckie.Shared.Utils.Localization;
using Duckie.Shared.Utils.Ui;
using Duckie.Utils.HotKeys;
using System.Windows;
using System.Windows.Threading;

namespace Duckie;

public partial class App : Application
{
    private SingleInstanceService singleInstance;
    internal static bool IsToolboxMode { get; private set; }

    private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        UiUtils.Warning($"Exception: {e.Exception.Message}{e.Exception.InnerException?.Message}", "Warning");
        e.Handled = true;
    }

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        IsToolboxMode = e.Args.Any(arg => string.Equals(arg, "--toolbox", StringComparison.OrdinalIgnoreCase));
        singleInstance = new SingleInstanceService(() =>
        {
            if (!Dispatcher.HasShutdownStarted)
                Dispatcher.BeginInvoke(new Action(() => (MainWindow as Duckie.MainWindow)?.ShowWindow()));
        });
        if (!singleInstance.IsPrimary)
        {
            Shutdown();
            return;
        }

        LocUtils.Initialize();

        // 创建主窗口
        var mainWindow = new MainWindow();
        MainWindow = mainWindow;

        if (!IsToolboxMode)
        {
            mainWindow.Show();
            return;
        }
        
        NotifyIconUtils.Initialize();
        NotifyIconUtils.RefreshContextMenu();

        HotKeyManager.RegisterServices();

        PacManagerService.RefreshIconBadge();

        var proxyConfig = UserConfigService.Get().Proxy;
        if (!string.IsNullOrWhiteSpace(proxyConfig?.ProxyUri))
        {
            HttpUtils.SetProxy(proxyConfig);
        }

        // 检查是否启动时最小化
        var appSettings = UserConfigService.GetAppSettings();
        if (appSettings.StartMinimized)
        {
            // 不显示主窗口，直接最小化到托盘
            mainWindow.WindowState = WindowState.Minimized;
            mainWindow.ShowInTaskbar = false;
        }
        else
        {
            // 正常显示主窗口
            mainWindow.Show();
        }
    }

    private void Application_Exit(object sender, ExitEventArgs e)
    {
        if (singleInstance?.IsPrimary == true && IsToolboxMode) NotifyIconUtils.Release();
        singleInstance?.Dispose();
    }
}

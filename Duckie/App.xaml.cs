using Duckie.Utils.Localization;
using Duckie.Utils.Ui;
using System.Windows;
using System.Windows.Threading;

namespace Duckie
{
    public partial class App : Application
    {
        public static new MainWindow MainWindow => Current.MainWindow as MainWindow;

        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            UiUtils.Warning($"Exception: {e.Exception.Message}{e.Exception.InnerException?.Message}", "Warning");
            e.Handled = true;
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // 初始化本地化管理器
            InitializeLocalization();

            NotifyIconUtils.Initialize();
        }

        private void InitializeLocalization()
        {
            try
            {
                // 从用户配置中加载保存的语言偏好
                EmbeddedLocalizationManager.Instance.LoadSavedLanguage();
            }
            catch
            {
                // 如果加载失败，使用系统默认语言
                // EmbeddedLocalizationManager 会自动使用默认语言
            }
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            NotifyIconUtils.Release();
        }
    }
}

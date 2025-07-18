using Duckie.Utils;
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
    }
}

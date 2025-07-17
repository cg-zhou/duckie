using System.Windows;

namespace Duckie
{
    public partial class App : Application
    {
        public static new MainWindow MainWindow => Current.MainWindow as MainWindow;
    }
}

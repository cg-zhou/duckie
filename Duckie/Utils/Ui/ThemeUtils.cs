using System.Windows;
using System.Windows.Media;

namespace Duckie.Utils.Ui
{
    internal static class ThemeUtils
    {
        public static SolidColorBrush PrimaryBrush => GetBrush(App.MainWindow, nameof(PrimaryBrush));
        public static SolidColorBrush SelectedBrush => GetBrush(App.MainWindow, nameof(SelectedBrush));
        public static SolidColorBrush HoverBrush => GetBrush(App.MainWindow, nameof(HoverBrush));
        public static SolidColorBrush PrimaryTextBrush => GetBrush(App.MainWindow, nameof(PrimaryTextBrush));
        public static SolidColorBrush IconBrush => GetBrush(App.MainWindow, nameof(IconBrush));
        public static SolidColorBrush NoneBrush => Brushes.Transparent;

        private static SolidColorBrush GetBrush(Window window, string brushName)
        {
            var resource = window?.TryFindResource(brushName);
            return resource as SolidColorBrush ?? new SolidColorBrush(Color.FromRgb(0, 0, 0));
        }
    }
}

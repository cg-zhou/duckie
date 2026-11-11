using System.Windows;
using System.Windows.Media;

namespace Duckie.Shared.Utils.Ui;

public static class ThemeUtils
{
    public static SolidColorBrush PrimaryBrush => GetBrush(AppEnv.MainWindow, nameof(PrimaryBrush));
    public static SolidColorBrush SelectedBrush => GetBrush(AppEnv.MainWindow, nameof(SelectedBrush));
    public static SolidColorBrush HoverBrush => GetBrush(AppEnv.MainWindow, nameof(HoverBrush));
    public static SolidColorBrush PrimaryTextBrush => GetBrush(AppEnv.MainWindow, nameof(PrimaryTextBrush));
    public static SolidColorBrush IconBrush => GetBrush(AppEnv.MainWindow, nameof(IconBrush));
    public static SolidColorBrush NoneBrush => Brushes.Transparent;

    private static SolidColorBrush GetBrush(Window window, string brushName)
    {
        var resource = window?.TryFindResource(brushName);
        return resource as SolidColorBrush ?? new SolidColorBrush(Color.FromRgb(0, 0, 0));
    }
}

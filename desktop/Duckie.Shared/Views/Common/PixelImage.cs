using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Duckie.Shared.Views.Common;

/// <summary>
/// Shows a bitmap at one source pixel per display pixel, ignoring file DPI metadata.
/// The layout size follows the monitor's DPI when the window moves between displays.
/// </summary>
public class PixelImage : Image
{
    public PixelImage()
    {
        UseLayoutRounding = true;
        SnapsToDevicePixels = true;
        // Fill the explicitly calculated pixel bounds rather than using bitmap DPI.
        Stretch = Stretch.Fill;
        RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.NearestNeighbor);
        Loaded += (_, _) => UpdatePixelSize(VisualTreeHelper.GetDpi(this));
    }

    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (e.Property == SourceProperty) UpdatePixelSize(VisualTreeHelper.GetDpi(this));
    }

    protected override void OnDpiChanged(DpiScale oldDpi, DpiScale newDpi)
    {
        base.OnDpiChanged(oldDpi, newDpi);
        UpdatePixelSize(newDpi);
    }

    private void UpdatePixelSize(DpiScale dpi)
    {
        if (Source is BitmapSource bitmap)
        {
            SetCurrentValue(WidthProperty, bitmap.PixelWidth / dpi.DpiScaleX);
            SetCurrentValue(HeightProperty, bitmap.PixelHeight / dpi.DpiScaleY);
        }
        else
        {
            SetCurrentValue(WidthProperty, double.NaN);
            SetCurrentValue(HeightProperty, double.NaN);
        }
    }
}

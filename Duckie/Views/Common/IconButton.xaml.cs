using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Duckie.Views.Common;

public partial class IconButton : Button
{
    public static readonly DependencyProperty IconSizeProperty =
        DependencyProperty.Register(nameof(IconSize), typeof(double), typeof(IconButton), new PropertyMetadata(14.0));

    public static readonly DependencyProperty IconTypeProperty =
        DependencyProperty.Register(nameof(IconType), typeof(IconType), typeof(IconButton), new PropertyMetadata(IconType.Search));

    public static readonly DependencyProperty IconColorProperty =
        DependencyProperty.Register(nameof(IconColor), typeof(SolidColorBrush), typeof(IconButton), new PropertyMetadata(null));

    public double IconSize
    {
        get { return (double)GetValue(IconSizeProperty); }
        set { SetValue(IconSizeProperty, value); }
    }

    public IconType IconType
    {
        get { return (IconType)GetValue(IconTypeProperty); }
        set { SetValue(IconTypeProperty, value); }
    }

    public SolidColorBrush IconColor
    {
        get { return (SolidColorBrush)GetValue(IconColorProperty); }
        set { SetValue(IconColorProperty, value); }
    }

    public IconButton()
    {
        InitializeComponent();
    }
}

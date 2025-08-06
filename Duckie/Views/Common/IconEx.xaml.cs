using Duckie.Utils.Ui;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Duckie.Views.Common;

public partial class IconEx : UserControl
{
    public static readonly DependencyProperty IconSizeProperty =
        DependencyProperty.Register(nameof(IconSize), typeof(double), typeof(IconEx), new PropertyMetadata(14.0));

    public static readonly DependencyProperty IconTypeProperty =
        DependencyProperty.Register(nameof(IconType), typeof(IconType), typeof(IconEx), new PropertyMetadata(IconType.Search, OnIconPropChanged));

    public static readonly DependencyProperty IconColorProperty =
        DependencyProperty.Register(nameof(IconColor), typeof(SolidColorBrush), typeof(IconEx), new PropertyMetadata(null, OnIconPropChanged));

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

    public IconEx()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        UpdateIcon();
    }

    private static void OnIconPropChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is IconEx iconEx)
        {
            iconEx.UpdateIcon();
        }
    }

    private void UpdateIcon()
    {
        if (Application.Current?.Resources[IconType.ToString()] is Geometry geometry)
        {
            IconPath.Data = geometry;
            var iconName = IconType.ToString().ToLower();
            if (iconName.Contains("zoom") || iconName.Contains("search"))
            {
                IconPath.Stroke = IconColor ?? ThemeUtils.IconBrush;
                IconPath.StrokeThickness = 1.2;
            }
            else
            {
                IconPath.Fill = IconColor ?? ThemeUtils.IconBrush;
            }
        }
    }
}

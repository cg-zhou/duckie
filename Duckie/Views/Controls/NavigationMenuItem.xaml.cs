using Duckie.Utils.Ui;
using Duckie.Views.Common;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Duckie.Views.Controls;

public partial class NavigationMenuItem : UserControl
{
    public static readonly DependencyProperty IconTypeProperty =
        DependencyProperty.Register(nameof(IconType), typeof(IconType), typeof(NavigationMenuItem),
            new PropertyMetadata(IconType.Search, OnIconTypeChanged));

    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register(nameof(Text), typeof(string), typeof(NavigationMenuItem),
            new PropertyMetadata(string.Empty, OnTextChanged));

    public static readonly DependencyProperty IsSelectedProperty =
        DependencyProperty.Register(nameof(IsSelected), typeof(bool), typeof(NavigationMenuItem),
            new PropertyMetadata(false, OnIsSelectedChanged));

    public static readonly DependencyProperty IsTextVisibleProperty =
        DependencyProperty.Register(nameof(IsTextVisible), typeof(bool), typeof(NavigationMenuItem),
            new PropertyMetadata(true, OnIsTextVisibleChanged));

    public event EventHandler<RoutedEventArgs> Click;

    public NavigationMenuItem()
    {
        InitializeComponent();
    }

    public IconType IconType
    {
        get { return (IconType)GetValue(IconTypeProperty); }
        set { SetValue(IconTypeProperty, value); }
    }

    public string Text
    {
        get { return (string)GetValue(TextProperty); }
        set { SetValue(TextProperty, value); }
    }

    public bool IsSelected
    {
        get { return (bool)GetValue(IsSelectedProperty); }
        set { SetValue(IsSelectedProperty, value); }
    }

    public bool IsTextVisible
    {
        get { return (bool)GetValue(IsTextVisibleProperty); }
        set { SetValue(IsTextVisibleProperty, value); }
    }

    private static void OnIconTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is NavigationMenuItem control)
        {
            control.UpdateIcon();
        }
    }

    private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is NavigationMenuItem control)
        {
            control.UpdateText();
        }
    }

    private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is NavigationMenuItem control)
        {
            control.UpdateSelectedState();
        }
    }

    private static void OnIsTextVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is NavigationMenuItem control)
        {
            control.UpdateTextVisibility();
        }
    }

    private void UpdateIcon()
    {
        Icon.IconType = IconType;
    }

    private void UpdateText()
    {
        TextBlock.Text = Text ?? string.Empty;
    }

    private void UpdateSelectedState()
    {
        var template = MenuButton.Template;
        if (template != null)
        {
            MenuButton.ApplyTemplate();
            var indicator = template.FindName("SelectionIndicator", MenuButton) as Rectangle;
            if (indicator != null)
            {
                indicator.Visibility = IsSelected ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        MenuButton.Background = IsSelected ? ThemeUtils.SelectedBrush : ThemeUtils.NoneBrush;
    }

    private void UpdateTextVisibility()
    {
        TextBlock.Visibility = IsTextVisible ? Visibility.Visible : Visibility.Collapsed;
    }

    private void MenuButton_Click(object sender, RoutedEventArgs e)
    {
        Click?.Invoke(this, e);
    }
}

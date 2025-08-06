using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Duckie.Views.Common;

public partial class LinkEx : UserControl
{
    public static readonly DependencyProperty IconTypeProperty =
        DependencyProperty.Register(nameof(IconType), typeof(IconType), typeof(LinkEx),
            new PropertyMetadata(IconType.Search, OnIconTypeChanged));

    public static readonly DependencyProperty NavigateUriProperty =
        DependencyProperty.Register(nameof(NavigateUri), typeof(string), typeof(LinkEx),
            new PropertyMetadata(string.Empty, OnNavigateUriChanged));

    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register(nameof(Text), typeof(string), typeof(LinkEx),
            new PropertyMetadata(string.Empty, OnTextChanged));

    public IconType IconType
    {
        get => (IconType)GetValue(IconTypeProperty);
        set => SetValue(IconTypeProperty, value);
    }

    public string NavigateUri
    {
        get => (string)GetValue(NavigateUriProperty);
        set => SetValue(NavigateUriProperty, value);
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public LinkEx()
    {
        InitializeComponent();
    }

    private static void OnIconTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is LinkEx linkEx && e.NewValue is IconType iconType)
        {
            linkEx.LinkIcon.IconType = iconType;
        }
    }

    private static void OnNavigateUriChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is LinkEx linkEx)
        {
            var uri = e.NewValue?.ToString();
            if (!string.IsNullOrEmpty(uri))
            {
                linkEx.LinkHyperlink.NavigateUri = new Uri(uri);
                linkEx.LinkHyperlink.ToolTip = uri;
            }
        }
    }

    private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is LinkEx linkEx)
        {
            linkEx.LinkText.Text = e.NewValue?.ToString() ?? string.Empty;
        }
    }

    private void LinkHyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
    {
        Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
        e.Handled = true;
    }
}

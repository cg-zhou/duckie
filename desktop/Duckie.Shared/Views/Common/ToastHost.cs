using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Duckie.Shared.Views.Common;

/// <summary>
/// Window overlay for transient messages. Place last in the root Grid.
/// New messages replace the current message and restart its lifetime.
/// </summary>
public class ToastHost : Border
{
    private static readonly DependencyProperty HostProperty = DependencyProperty.RegisterAttached(
        "Host", typeof(ToastHost), typeof(ToastHost));

    private readonly TextBlock messageText;
    private readonly Path statusIcon;
    private readonly DispatcherTimer timer;
    private Window owner;

    public ToastHost()
    {
        // Match web-booster's wb-message layout and design tokens.
        HorizontalAlignment = HorizontalAlignment.Right;
        VerticalAlignment = VerticalAlignment.Bottom;
        Margin = new Thickness(24);
        Padding = new Thickness(14, 10, 14, 10);
        CornerRadius = new CornerRadius(8);
        BorderThickness = new Thickness(0.5);
        Effect = new DropShadowEffect
        {
            Color = Color.FromRgb(15, 23, 42),
            Opacity = 0.1,
            BlurRadius = 24,
            ShadowDepth = 8,
            Direction = 270
        };
        RenderTransform = new TranslateTransform();
        MaxWidth = 360;
        IsHitTestVisible = false;
        Visibility = Visibility.Collapsed;
        Panel.SetZIndex(this, 100);

        messageText = new TextBlock
        {
            TextWrapping = TextWrapping.Wrap,
            FontSize = 13,
            LineHeight = 19,
            VerticalAlignment = VerticalAlignment.Center
        };
        statusIcon = new Path
        {
            Width = 16,
            Height = 16,
            Stretch = Stretch.Uniform,
            StrokeThickness = 1.7,
            StrokeStartLineCap = PenLineCap.Round,
            StrokeEndLineCap = PenLineCap.Round,
            StrokeLineJoin = PenLineJoin.Round,
            VerticalAlignment = VerticalAlignment.Center,
            Margin = new Thickness(0, 0, 10, 0)
        };
        var content = new Grid();
        content.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        content.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        Grid.SetColumn(messageText, 1);
        content.Children.Add(statusIcon);
        content.Children.Add(messageText);
        Child = content;
        timer = new DispatcherTimer(DispatcherPriority.Background, Dispatcher);
        timer.Tick += (_, _) => Dismiss();

        Loaded += (_, _) =>
        {
            owner = Window.GetWindow(this);
            owner?.SetValue(HostProperty, this);
        };
        Unloaded += (_, _) =>
        {
            Dismiss();
            if (owner?.GetValue(HostProperty) == this) owner.ClearValue(HostProperty);
            owner = null;
        };
        IsVisibleChanged += (_, _) => { if (!IsVisible) Dismiss(); };
    }

    public static void Show(Window window, string message, bool isError = false)
    {
        if (window == null) return;
        window.Dispatcher.Invoke(() =>
        {
            if (window.IsVisible && window.GetValue(HostProperty) is ToastHost host)
                host.ShowMessage(message, isError);
        });
    }

    public void ShowMessage(string message, bool isError = false)
    {
        Dispatcher.VerifyAccess();
        timer.Stop();
        messageText.Text = message;
        // CSS #RRGGBBAA tokens mapped to WPF #AARRGGBB.
        Background = Brush(isError ? "#FAFEF2F2" : "#FAF0FDF4");
        BorderBrush = Brush(isError ? "#47F87171" : "#474ADE80");
        messageText.Foreground = Brush(isError ? "#B91C1C" : "#166534");
        statusIcon.Stroke = messageText.Foreground;
        statusIcon.Data = Geometry.Parse(isError
            ? "M 15,8 A 7,7 0 1 1 1,8 A 7,7 0 1 1 15,8 M 5.5,5.5 L 10.5,10.5 M 10.5,5.5 L 5.5,10.5"
            : "M 2,8 L 6,12 L 14,4");
        Visibility = Visibility.Visible;
        var duration = TimeSpan.FromMilliseconds(220);
        BeginAnimation(OpacityProperty, new DoubleAnimation(0, 1, duration));
        RenderTransform.BeginAnimation(TranslateTransform.YProperty, new DoubleAnimation(16, 0, duration)
        {
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
        });
        timer.Interval = TimeSpan.FromSeconds(isError ? 6 : 3);
        timer.Start();
    }

    public void Dismiss()
    {
        timer.Stop();
        Visibility = Visibility.Collapsed;
        BeginAnimation(OpacityProperty, null);
        RenderTransform.BeginAnimation(TranslateTransform.YProperty, null);
        messageText.Text = string.Empty;
    }

    private static SolidColorBrush Brush(string color) => new((Color)ColorConverter.ConvertFromString(color));
}

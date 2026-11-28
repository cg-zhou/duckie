using System.Windows;
using System.Windows.Controls;

namespace Duckie.Shared.Views.Common;

/// <summary>A wrapping label and an action, stacked automatically in narrow pages.</summary>
public class SettingRow : ContentControl
{
    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
        nameof(Title), typeof(string), typeof(SettingRow), new PropertyMetadata(string.Empty));
    public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
        nameof(Description), typeof(string), typeof(SettingRow), new PropertyMetadata(string.Empty));
    public string Title { get => (string)GetValue(TitleProperty); set => SetValue(TitleProperty, value); }
    public string Description { get => (string)GetValue(DescriptionProperty); set => SetValue(DescriptionProperty, value); }
    public SettingRow()
    {
        Focusable = false;
        SetResourceReference(StyleProperty, "DesignSettingRow");
        SizeChanged += (_, _) => UpdateLayoutMode();
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        UpdateLayoutMode();
    }

    private void UpdateLayoutMode()
    {
        if (GetTemplateChild("PART_Action") is not ContentPresenter action ||
            GetTemplateChild("PART_Copy") is not FrameworkElement copy) return;

        var stacked = ActualWidth < 420;
        Grid.SetRow(action, stacked ? 1 : 0);
        Grid.SetColumn(action, stacked ? 0 : 1);
        Grid.SetColumnSpan(action, stacked ? 2 : 1);
        Grid.SetColumnSpan(copy, stacked ? 2 : 1);
        action.Margin = stacked ? new Thickness(0, 12, 0, 0) : new Thickness(20, 0, 0, 0);
        action.HorizontalAlignment = stacked ? HorizontalAlignment.Left : HorizontalAlignment.Stretch;
    }
}

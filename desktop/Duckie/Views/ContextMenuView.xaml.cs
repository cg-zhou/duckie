using Duckie.Services.ContextMenu;
using Duckie.Shared.Utils.Localization;
using Duckie.Shared.Utils.Ui;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Duckie.Views;

public partial class ContextMenuView : UserControl
{
    private bool busy;

    public ContextMenuView()
    {
        InitializeComponent();
        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
        IsVisibleChanged += (_, _) => { if (IsVisible) RefreshState(); };
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        ContextMenuUtils.SettingsChanged += OnSettingsChanged;
        EmbeddedLocalizationManager.Instance.LanguageChanged += OnSettingsChanged;
        RefreshState();
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        ContextMenuUtils.SettingsChanged -= OnSettingsChanged;
        EmbeddedLocalizationManager.Instance.LanguageChanged -= OnSettingsChanged;
    }

    private void OnSettingsChanged(object sender, EventArgs e) => Dispatcher.Invoke(RefreshState);

    private void RefreshState()
    {
        ClassicCard.ToolTip = LocKey.ContextMenu_Note.Text(LocKey.ContextMenu_ClassicState.Text());
        DefaultCard.ToolTip = LocKey.ContextMenu_Note.Text(LocKey.ContextMenu_DefaultPreview.Text());
        try
        {
            var supported = ContextMenuUtils.IsContextMenuCustomizationSupported();
            var classic = supported && ContextMenuUtils.IsWindows10ContextMenuEnabled();
            ErrorText.Text = supported ? string.Empty : LocKey.ContextMenu_Unsupported.Text();
            ErrorText.Visibility = supported ? Visibility.Collapsed : Visibility.Visible;
            ClassicCard.IsEnabled = DefaultCard.IsEnabled = supported && !busy;
            HighlightCard(ClassicCard, supported && classic);
            HighlightCard(DefaultCard, supported && !classic);
        }
        catch (Exception ex)
        {
            ClassicCard.IsEnabled = DefaultCard.IsEnabled = false;
            HighlightCard(ClassicCard, false);
            HighlightCard(DefaultCard, false);
            ErrorText.Text = LocKey.ContextMenu_Failed.Text(ex.Message);
            ErrorText.Visibility = Visibility.Visible;
        }
    }

    private static void HighlightCard(Button card, bool selected)
    {
        Selector.SetIsSelected(card, selected);
    }

    private async void Classic_Click(object sender, RoutedEventArgs e) => await ApplyAsync(true);
    private async void Default_Click(object sender, RoutedEventArgs e) => await ApplyAsync(false);

    private async Task ApplyAsync(bool classic)
    {
        if (busy) return;
        busy = true;
        var targetMenu = classic ? LocKey.ContextMenu_ClassicState : LocKey.ContextMenu_DefaultPreview;
        try
        {
            if (!ContextMenuUtils.IsContextMenuCustomizationSupported()) return;
            if (ContextMenuUtils.IsWindows10ContextMenuEnabled() == classic) return;
            if (!UiUtils.Confirm(LocKey.ContextMenu_Confirm.Text(targetMenu.Text()), LocKey.Nav_ContextMenu.Text())) return;
            RefreshState();
            await Task.Run(() =>
            {
                if (classic) ContextMenuUtils.SetWindows10ContextMenu();
                else ContextMenuUtils.SetWindows11ContextMenu();
            });
            UiUtils.ShowToast(LocKey.ContextMenu_Applied.Text(targetMenu.Text()));
        }
        catch (Exception ex)
        {
            UiUtils.ShowToast(LocKey.ContextMenu_Failed.Text(ex.Message), isError: true);
        }
        finally
        {
            busy = false;
            RefreshState();
        }
    }
}

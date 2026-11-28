using Duckie.Shared.Utils;
using Duckie.Shared.Utils.Localization;
using Duckie.Utils;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Duckie.Views;

public partial class AboutView : UserControl
{
    private string packageName;

    public AboutView()
    {
        InitializeComponent();
        RefreshText();
        Loaded += AboutView_Loaded;
        Unloaded += (_, _) => EmbeddedLocalizationManager.Instance.LanguageChanged -= OnLanguageChanged;
    }

    private void AboutView_Loaded(object sender, RoutedEventArgs e)
    {
        EmbeddedLocalizationManager.Instance.LanguageChanged += OnLanguageChanged;
        packageName = MsixPackageUtils.TryGetMsixPackageName(out var name) ? name : null;
        RefreshText();
    }

    private void OnLanguageChanged(object sender, EventArgs e) => RefreshText();

    private void RefreshText()
    {
        versionText.Text = "v" + AppUtils.GetAppVersion();
        RuntimeInfoBorder.Visibility = packageName == null ? Visibility.Collapsed : Visibility.Visible;
        if (packageName == null) return;
        RuntimeTitle.Text = LocKey.MSIXPackage.Text(packageName);
        RuntimeDescription.Text = LocKey.MSIXRestriction.Text();
    }

    private void OpenLink_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is string uri)
            Process.Start(new ProcessStartInfo(uri) { UseShellExecute = true });
    }
}

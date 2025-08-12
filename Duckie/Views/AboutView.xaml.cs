using Duckie.Shared.Utils;
using Duckie.Shared.Utils.Localization;
using Duckie.Utils;
using System.Windows;
using System.Windows.Controls;

namespace Duckie.Views;

public partial class AboutView : UserControl
{
    public AboutView()
    {
        InitializeComponent();
        Loaded += AboutView_Loaded;
    }

    private void AboutView_Loaded(object sender, RoutedEventArgs e)
    {
        versionText.Text = LocKey.Version.Text(AppUtils.GetAppVersion());

        if (MsixPackageUtils.TryGetMsixPackageName(out var packageName))
        {
            ShowMsixInfo(packageName);
        }
    }

    private void ShowMsixInfo(string packageName)
    {
        RuntimeTitle.Text = LocKey.MSIXPackage.Text(packageName);
        RuntimeDescription.Text = LocKey.MSIXRestriction.Text();
        RuntimeInfoBorder.Visibility = Visibility.Visible;
    }
}

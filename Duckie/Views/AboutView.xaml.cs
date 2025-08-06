using Duckie.Utils;
using Duckie.Utils.Localization;
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
        if (MsixPackageUtils.TryGetMsixPackageName(out var packageName))
        {
            ShowMsixInfo(packageName);
        }
    }

    private void ShowMsixInfo(string packageName)
    {
        RuntimeTitle.Text = LocUtils.GetString("MSIXPackage", packageName);
        RuntimeDescription.Text = LocUtils.GetString("MSIXRestriction");
        RuntimeInfoBorder.Visibility = Visibility.Visible;
    }
}

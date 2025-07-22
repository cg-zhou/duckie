using Duckie.Utils;
using System.Windows;
using System.Windows.Controls;

namespace Duckie.Views
{
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
            RuntimeTitle.Text = $"MSIX Package: {packageName}";
            RuntimeDescription.Text = "Some features may be limited due to Microsoft Store restrictions";
            RuntimeInfoBorder.Visibility = Visibility.Visible;
        }
    }
}

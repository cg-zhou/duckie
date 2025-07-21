using Duckie.Utils.Ui;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Duckie.Views
{
    public partial class AboutView : UserControl
    {
        public AboutView()
        {
            InitializeComponent();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = e.Uri.AbsoluteUri,
                    UseShellExecute = true
                });
                e.Handled = true;
            }
            catch
            {
                UiUtils.Warning($"Failed to open {e.Uri.AbsoluteUri}");
            }
        }
    }
}

using Duckie.Services.UserConfigs;
using Duckie.Utils.Ui;
using System.Windows;
using System.Windows.Controls;

namespace Duckie.Views;

public partial class PacEditContent : UserControl
{
    public PacConfig PacConfig { get; private set; }
    public bool IsEditMode { get; private set; }

    public PacEditContent(PacConfig pacConfig = null)
    {
        InitializeComponent();
        
        IsEditMode = pacConfig != null;
        
        if (IsEditMode)
        {
            NameTextBox.Text = pacConfig.Name;
            UriTextBox.Text = pacConfig.Uri;
        }

        NameTextBox.Focus();
    }

    /// <summary>
    /// Validate input and create PAC configuration
    /// </summary>
    /// <returns>Whether validation succeeded</returns>
    public bool ValidateAndCreateConfig()
    {
        var name = NameTextBox.Text?.Trim();
        var uri = UriTextBox.Text?.Trim();

        if (string.IsNullOrWhiteSpace(name))
        {
            UiUtils.Warning("Please enter configuration name", "Input Validation");
            NameTextBox.Focus();
            return false;
        }

        if (string.IsNullOrWhiteSpace(uri))
        {
            UiUtils.Warning("Please enter PAC URL", "Input Validation");
            UriTextBox.Focus();
            return false;
        }

        // Simple URI format validation
        if (!uri.StartsWith("http://") && !uri.StartsWith("https://") && !uri.StartsWith("file:///"))
        {
            var result = MessageBox.Show(
                "PAC URL format may be incorrect. Continue anyway?\n\n" +
                "Common formats:\n" +
                "• http://example.com/proxy.pac\n" +
                "• file:///C:/proxy.pac",
                "Format Warning",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
            {
                UriTextBox.Focus();
                return false;
            }
        }

        PacConfig = new PacConfig
        {
            Name = name,
            Uri = uri
        };

        return true;
    }
}

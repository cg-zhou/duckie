using Duckie.Services.Network;
using Duckie.Shared.Services.UserConfigs;
using Duckie.Shared.Utils.Localization;
using Duckie.Utils.HotKeys;
using Duckie.Utils.Registry;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Duckie.Views;

public partial class SettingsView : UserControl
{
    public SettingsView()
    {
        InitializeComponent();
        Loaded += (_, _) => InitializeHotKeyItems();
        IsVisibleChanged += (_, _) => { if (IsVisible) InitializeHotKeyItems(); };

        // 初始化语言下拉框
        InitializeLanguageComboBox();

        // 监听语言变化事件
        EmbeddedLocalizationManager.Instance.LanguageChanged += OnLanguageChanged;

        // 初始化热键设置项
        InitializeHotKeyItems();

        // 初始化应用设置
        InitializeAppSettings();

        // 初始化代理设置
        InitializeProxySettings();
    }

    private void InitializeLanguageComboBox()
    {
        // 设置当前选中的语言
        var currentCulture = LocUtils.CurrentCulture;
        var currentLanguageTag = currentCulture.Name;

        foreach (ComboBoxItem item in LanguageComboBox.Items)
        {
            if (item.Tag.ToString() == currentLanguageTag)
            {
                LanguageComboBox.SelectedItem = item;
                break;
            }
        }
    }

    private void InitializeHotKeyItems()
    {
        HotKeysItemsControl.ItemsSource = HotKeyManager.GetHotKeyServices()
            .SelectMany(service => service.Register()).ToArray();
    }

    private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (LanguageComboBox.SelectedItem is ComboBoxItem selectedItem)
        {
            var cultureName = selectedItem.Tag.ToString();
            // 切换语言（会自动保存到用户配置）
            if (LocUtils.CurrentCulture.Name != cultureName)
            {
                EmbeddedLocalizationManager.Instance.SwitchLanguage(cultureName);
            }
        }
    }

    private void OnLanguageChanged(object sender, EventArgs e)
    {
        InitializeLanguageComboBox();
        // 语言变化时更新下拉框显示的文本
        UpdateLanguageDisplay();
        InitializeHotKeyItems();
    }

    private void UpdateLanguageDisplay()
    {
        // 更新下拉框中显示的语言文本
        if (LanguageComboBox.Template?.FindName("ToggleButton", LanguageComboBox) is ToggleButton toggleButton)
        {
            if (toggleButton.Template?.FindName("LanguageText", toggleButton) is TextBlock languageText)
            {
                var currentCulture = LocUtils.CurrentCulture;
                languageText.Text = currentCulture.Name == "zh-CN" ? "简体中文" : "English";
            }
        }
    }

    private void InitializeAppSettings()
    {
        var appSettings = UserConfigService.GetAppSettings();
        
        // 开机启动直接从注册表读取
        StartWithWindowsCheckBox.IsChecked = RegistryUtils.StartUp.Get();
        
        // 其他设置从用户配置读取
        StartMinimizedCheckBox.IsChecked = appSettings.StartMinimized;
        MinimizeToTrayOnCloseCheckBox.IsChecked = appSettings.MinimizeToTrayOnClose;
    }

    private void StartWithWindowsCheckBox_Changed(object sender, RoutedEventArgs e)
    {
        var isChecked = StartWithWindowsCheckBox.IsChecked == true;
        
        try
        {
            // 直接更新注册表，注册表是唯一数据源
            RegistryUtils.StartUp.Set(isChecked);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed to update startup setting: {ex.Message}");
            // 如果失败，恢复复选框状态
            StartWithWindowsCheckBox.IsChecked = !isChecked;
        }
    }

    private void StartMinimizedCheckBox_Changed(object sender, RoutedEventArgs e)
    {
        var isChecked = StartMinimizedCheckBox.IsChecked == true;
        
        var appSettings = UserConfigService.GetAppSettings();
        appSettings.StartMinimized = isChecked;
        UserConfigService.SetAppSettings(appSettings);
    }

    private void MinimizeToTrayOnCloseCheckBox_Changed(object sender, RoutedEventArgs e)
    {
        var isChecked = MinimizeToTrayOnCloseCheckBox.IsChecked == true;
        
        var appSettings = UserConfigService.GetAppSettings();
        appSettings.MinimizeToTrayOnClose = isChecked;
        UserConfigService.SetAppSettings(appSettings);
    }

    private void InitializeProxySettings()
    {
        var userConfig = UserConfigService.Get();
        var proxyConfig = userConfig.Proxy;

        // 设置控件状态
        var isProxyEnabled = !string.IsNullOrWhiteSpace(proxyConfig?.ProxyUri);
        EnableProxyCheckBox.IsChecked = isProxyEnabled;
        
        ProxyServerTextBox.Text = proxyConfig?.ProxyUri ?? "";
        UsernameTextBox.Text = proxyConfig?.Username ?? "";
        PasswordBox.Password = proxyConfig?.Password ?? "";

        // 根据代理启用状态控制控件可用性
        UpdateProxyControlsState(isProxyEnabled);
    }

    private void UpdateProxyControlsState(bool isEnabled)
    {
        ProxyServerTextBox.IsEnabled = isEnabled;
        UsernameTextBox.IsEnabled = isEnabled;
        PasswordBox.IsEnabled = isEnabled;
        TestNetworkButton.IsEnabled = isEnabled;
    }

    private void EnableProxyCheckBox_Changed(object sender, RoutedEventArgs e)
    {
        var isChecked = EnableProxyCheckBox.IsChecked == true;
        UpdateProxyControlsState(isChecked);

        if (!isChecked)
        {
            // 禁用代理时清空配置
            var emptyConfig = new ProxyConfig();
            UserConfigService.Set(emptyConfig);
            HttpUtils.SetProxy(emptyConfig);
        }
        else
        {
            // 启用代理时保存当前配置
            SaveProxyConfig();
        }
    }

    private void ProxyServerTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (EnableProxyCheckBox.IsChecked == true)
        {
            SaveProxyConfig();
        }
    }

    private void UsernameTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (EnableProxyCheckBox.IsChecked == true)
        {
            SaveProxyConfig();
        }
    }

    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (EnableProxyCheckBox.IsChecked == true)
        {
            SaveProxyConfig();
        }
    }

    private void SaveProxyConfig()
    {
        var proxyConfig = new ProxyConfig
        {
            ProxyUri = ProxyServerTextBox.Text?.Trim(),
            Username = UsernameTextBox.Text?.Trim(),
            Password = PasswordBox.Password?.Trim()
        };

        UserConfigService.Set(proxyConfig);
        HttpUtils.SetProxy(proxyConfig);
    }

    private async void TestNetworkButton_Click(object sender, RoutedEventArgs e)
    {
        TestNetworkButton.IsEnabled = false;
        TestNetworkButton.Content = "Testing...";

        try
        {
            await HttpUtils.TestNetworkAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Network test failed: {ex.Message}");
        }
        finally
        {
            TestNetworkButton.IsEnabled = true;
            TestNetworkButton.Content = LocKey.Settings_TestNetwork.Text();
        }
    }
}

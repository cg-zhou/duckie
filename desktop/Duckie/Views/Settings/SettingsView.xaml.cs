using Duckie.Shared.Services.UserConfigs;
using Duckie.Shared.Utils.Localization;
using Duckie.Utils.HotKeys;
using Duckie.Utils.Registry;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

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
        InitializeHotKeyItems();
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

}

using Duckie.Shared.Utils.Ui;
using Duckie.Utils.HotKeys;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Duckie.Views;

public partial class HotkeyManagerView : UserControl
{
    private readonly ObservableCollection<HotkeyInfo> _hotkeys = new();

    public HotkeyManagerView()
    {
        InitializeComponent();
        HotkeysItemsControl.ItemsSource = _hotkeys;
        LoadHotkeys();
    }

    private void LoadHotkeys()
    {
        try
        {
            _hotkeys.Clear();
            
            var services = HotKeyManager.GetHotKeyServices();
            foreach (var service in services)
            {
                foreach (var hotkey in service.Register())
                {
                    _hotkeys.Add(new HotkeyInfo
                    {
                        Name = hotkey.Name,
                        Description = GetHotkeyDescription(hotkey.Name),
                        KeyCombination = FormatKeyCombination(hotkey.Modifiers, hotkey.Keys),
                        Action = hotkey.Action,
                        IsEnabled = true
                    });
                }
            }
            
            UpdateHotkeyStatus();
        }
        catch (Exception ex)
        {
            UiUtils.Warning($"加载热键列表失败: {ex.Message}", "热键管理");
        }
    }

    private string GetHotkeyDescription(string name)
    {
        return name switch
        {
            "Show/Hide Duckie" => "显示或隐藏主窗口",
            "Exit Duckie" => "退出应用程序",
            "调小音量" => "降低系统音量",
            "调大音量" => "提高系统音量",
            "静音/取消静音" => "切换系统静音状态",
            _ => "执行相关操作"
        };
    }

    private string FormatKeyCombination(KeyModifiers modifiers, System.Windows.Forms.Keys keys)
    {
        var parts = new List<string>();
        
        if (modifiers.HasFlag(KeyModifiers.Alt))
            parts.Add("Alt");
        if (modifiers.HasFlag(KeyModifiers.Control))
            parts.Add("Ctrl");
        if (modifiers.HasFlag(KeyModifiers.Shift))
            parts.Add("Shift");
        if (modifiers.HasFlag(KeyModifiers.Win))
            parts.Add("Win");
            
        parts.Add(FormatKey(keys));
        
        return string.Join(" + ", parts);
    }

    private string FormatKey(System.Windows.Forms.Keys key)
    {
        return key switch
        {
            System.Windows.Forms.Keys.D0 => "0",
            System.Windows.Forms.Keys.D1 => "1",
            System.Windows.Forms.Keys.D2 => "2",
            System.Windows.Forms.Keys.D3 => "3",
            System.Windows.Forms.Keys.D4 => "4",
            System.Windows.Forms.Keys.D5 => "5",
            System.Windows.Forms.Keys.D6 => "6",
            System.Windows.Forms.Keys.D7 => "7",
            System.Windows.Forms.Keys.D8 => "8",
            System.Windows.Forms.Keys.D9 => "9",
            _ => key.ToString()
        };
    }

    private void UpdateHotkeyStatus()
    {
        var enabledCount = _hotkeys.Count(h => h.IsEnabled);
        var totalCount = _hotkeys.Count;
        
        if (enabledCount == totalCount && totalCount > 0)
        {
            HotkeyStatusText.Text = $"所有热键正常工作 ({totalCount} 个)";
            HotkeyStatusText.Foreground = System.Windows.Media.Brushes.Green;
        }
        else if (enabledCount > 0)
        {
            HotkeyStatusText.Text = $"部分热键工作正常 ({enabledCount}/{totalCount})";
            HotkeyStatusText.Foreground = System.Windows.Media.Brushes.Orange;
        }
        else
        {
            HotkeyStatusText.Text = "没有可用的热键";
            HotkeyStatusText.Foreground = System.Windows.Media.Brushes.Red;
        }
    }

    private void RefreshHotkeys_Click(object sender, RoutedEventArgs e)
    {
        LoadHotkeys();
        UiUtils.Info("热键列表已刷新", "热键管理");
    }

    private void TestHotkey_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is HotkeyInfo hotkey)
        {
            try
            {
                hotkey.Action?.Invoke();
                UiUtils.Info($"热键 '{hotkey.Name}' 测试成功", "热键测试");
            }
            catch (Exception ex)
            {
                UiUtils.Warning($"热键测试失败: {ex.Message}", "热键测试");
            }
        }
    }
}

public class HotkeyInfo
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string KeyCombination { get; set; } = string.Empty;
    public Action Action { get; set; }
    public bool IsEnabled { get; set; } = true;
}

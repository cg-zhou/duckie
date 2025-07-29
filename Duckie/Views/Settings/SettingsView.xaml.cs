using Duckie.Utils.HotKeys;
using Duckie.Utils.Localization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Duckie.Views
{
    public partial class SettingsView : UserControl
    {
        public ObservableCollection<HotKeySettingsItem> HotKeyItems { get; set; }

        public SettingsView()
        {
            InitializeComponent();
            Loaded += SettingsView_Loaded;

            // 初始化语言下拉框
            InitializeLanguageComboBox();

            // 监听语言变化事件
            EmbeddedLocalizationManager.Instance.LanguageChanged += OnLanguageChanged;

            // 初始化热键设置项
            InitializeHotKeyItems();
        }

        private void SettingsView_Loaded(object sender, RoutedEventArgs e)
        {
            HotKeysItemsControl.ItemsSource = HotKeyItems;
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
            HotKeyItems = new ObservableCollection<HotKeySettingsItem>();

            var hotKeyServices = HotKeyManager.GetHotKeyServices();
            foreach (var service in hotKeyServices)
            {
                var item = new HotKeySettingsItem
                {
                    Service = service,
                    Name = service.Name,
                    Modifiers = service.Modifiers,
                    Keys = service.Keys
                };
                item.UpdateHotKeyDisplay();
                HotKeyItems.Add(item);
            }
        }

        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LanguageComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                var cultureName = selectedItem.Tag.ToString();
                // 切换语言（会自动保存到用户配置）
                EmbeddedLocalizationManager.Instance.SwitchLanguage(cultureName);
            }
        }

        private void OnLanguageChanged(object sender, EventArgs e)
        {
            // 语言变化时更新下拉框显示的文本
            UpdateLanguageDisplay();
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

        private void HotKeyTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;

            var textBox = sender as TextBox;
            var item = textBox?.Tag as HotKeySettingsItem;
            if (item == null)
            {
                return;
            }

            // 处理退格键，等同于清除快捷键
            if (e.Key == Key.Back || e.Key == Key.Delete)
            {
                ClearHotKey(item);
                return;
            }

            // 忽略修饰键单独按下
            if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl ||
                e.Key == Key.LeftAlt || e.Key == Key.RightAlt ||
                e.Key == Key.LeftShift || e.Key == Key.RightShift ||
                e.Key == Key.LWin || e.Key == Key.RWin)
            {
                return;
            }

            // 获取修饰键
            var modifiers = KeyModifiers.None;
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                modifiers |= KeyModifiers.Control;
            if (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt))
                modifiers |= KeyModifiers.Alt;
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                modifiers |= KeyModifiers.Shift;
            if (Keyboard.IsKeyDown(Key.LWin) || Keyboard.IsKeyDown(Key.RWin))
                modifiers |= KeyModifiers.Win;

            // 转换为Windows Forms的Keys
            var keys = ConvertToWindowsFormsKeys(e.Key);
            if (keys == System.Windows.Forms.Keys.None)
                return;

            // 更新热键设置
            UpdateHotKey(item, modifiers, keys);
        }

        private System.Windows.Forms.Keys ConvertToWindowsFormsKeys(Key wpfKey)
        {
            try
            {
                return (System.Windows.Forms.Keys)KeyInterop.VirtualKeyFromKey(wpfKey);
            }
            catch
            {
                return System.Windows.Forms.Keys.None;
            }
        }

        private void UpdateHotKey(HotKeySettingsItem item, KeyModifiers modifiers, System.Windows.Forms.Keys keys)
        {
            try
            {
                // 先取消之前的注册
                HotKeyManager.Unregister(item.Service);

                // 更新设置项
                item.Modifiers = modifiers;
                item.Keys = keys;
                item.UpdateHotKeyDisplay();

                // 注册新的快捷键
                HotKeyManager.Register(item.Service, modifiers, keys);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"设置快捷键失败：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ClearHotKey(HotKeySettingsItem item)
        {
            try
            {
                // 取消注册
                HotKeyManager.Unregister(item.Service);

                // 清除设置
                item.Modifiers = KeyModifiers.None;
                item.Keys = System.Windows.Forms.Keys.None;
                item.UpdateHotKeyDisplay();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"清除快捷键失败：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void HotKeyTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null)
            {
                textBox.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 200));
            }
        }

        private void HotKeyTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null)
            {
                textBox.Background = System.Windows.Media.Brushes.White;
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var item = button?.Tag as HotKeySettingsItem;
            if (item != null)
            {
                ClearHotKey(item);
            }
        }
    }

    public class HotKeySettingsItem : INotifyPropertyChanged
    {
        public IHotKeyService Service { get; set; }
        public string Name { get; set; }
        public KeyModifiers Modifiers { get; set; }
        public System.Windows.Forms.Keys Keys { get; set; }

        private string _hotKeyDisplay = "";
        public string HotKeyDisplay
        {
            get => _hotKeyDisplay;
            set
            {
                _hotKeyDisplay = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasHotKey));
            }
        }

        public bool HasHotKey => !string.IsNullOrEmpty(_hotKeyDisplay) && _hotKeyDisplay != "无";

        public void UpdateHotKeyDisplay()
        {
            if (Modifiers == KeyModifiers.None && Keys == System.Windows.Forms.Keys.None)
            {
                HotKeyDisplay = "无";
                return;
            }

            var parts = new List<string>();

            if (Modifiers.HasFlag(KeyModifiers.Control))
                parts.Add("Ctrl");
            if (Modifiers.HasFlag(KeyModifiers.Alt))
                parts.Add("Alt");
            if (Modifiers.HasFlag(KeyModifiers.Shift))
                parts.Add("Shift");
            if (Modifiers.HasFlag(KeyModifiers.Win))
                parts.Add("Windows");

            if (Keys != System.Windows.Forms.Keys.None)
                parts.Add(Keys.ToString());

            HotKeyDisplay = string.Join(" + ", parts);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

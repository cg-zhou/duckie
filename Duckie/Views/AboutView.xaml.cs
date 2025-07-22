using Duckie.Utils;
using Duckie.Utils.Localization;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Duckie.Views
{
    public partial class AboutView : UserControl
    {
        public AboutView()
        {
            InitializeComponent();
            Loaded += AboutView_Loaded;

            // 初始化语言下拉框
            InitializeLanguageComboBox();

            // 监听语言变化事件
            EmbeddedLocalizationManager.Instance.LanguageChanged += OnLanguageChanged;
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
            RuntimeTitle.Text = EmbeddedLocalizationManager.Instance.GetString("MSIXPackage", packageName);
            RuntimeDescription.Text = EmbeddedLocalizationManager.Instance.GetString("MSIXRestriction");
            RuntimeInfoBorder.Visibility = Visibility.Visible;
        }

        private void InitializeLanguageComboBox()
        {
            // 设置当前选中的语言
            var currentCulture = EmbeddedLocalizationManager.Instance.CurrentCulture;
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
                    var currentCulture = EmbeddedLocalizationManager.Instance.CurrentCulture;
                    languageText.Text = currentCulture.Name == "zh-CN" ? "简体中文" : "English";
                }
            }
        }
    }
}

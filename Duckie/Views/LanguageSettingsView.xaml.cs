using Duckie.Utils.Localization;
using Duckie.Services.UserConfigs;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Duckie.Views
{
    public partial class LanguageSettingsView : UserControl
    {
        public LanguageSettingsView()
        {
            InitializeComponent();
            InitializeLanguageSettings();
        }

        private void InitializeLanguageSettings()
        {
            // 设置当前选中的语言
            var currentCulture = EmbeddedLocalizationManager.Instance.CurrentCulture;
            var currentItem = LanguageComboBox.Items.Cast<ComboBoxItem>()
                .FirstOrDefault(item => item.Tag.ToString() == currentCulture.Name);

            if (currentItem != null)
            {
                LanguageComboBox.SelectedItem = currentItem;
            }
            else
            {
                // 默认选择英语
                LanguageComboBox.SelectedIndex = 0;
            }
        }

        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LanguageComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                var cultureName = selectedItem.Tag.ToString();
                EmbeddedLocalizationManager.Instance.SwitchLanguage(cultureName);

                // 保存语言设置到用户配置
                SaveLanguagePreference(cultureName);
            }
        }

        private void SaveLanguagePreference(string cultureName)
        {
            try
            {
                // 保存到用户配置文件
                UserConfigService.SetLanguage(cultureName);
            }
            catch
            {
                // 忽略保存错误
            }
        }
    }
}

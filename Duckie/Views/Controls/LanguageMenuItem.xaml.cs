using Duckie.Utils.Localization;
using Duckie.Views.Common;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Duckie.Views.Controls
{
    public partial class LanguageMenuItem : UserControl
    {
        public LanguageMenuItem()
        {
            InitializeComponent();
            InitializeLanguageDisplay();
            
            // 订阅语言变更事件
            EmbeddedLocalizationManager.Instance.LanguageChanged += OnLanguageChanged;
        }

        private void InitializeLanguageDisplay()
        {
            UpdateLanguageDisplay();
        }

        private void UpdateLanguageDisplay()
        {
            var currentCulture = EmbeddedLocalizationManager.Instance.CurrentCulture;

            // 更新显示文本
            LanguageText.Text = EmbeddedLocalizationManager.Instance.GetString("Nav_Language");

            // 显示当前语言
            switch (currentCulture.Name)
            {
                case "zh-CN":
                    CurrentLanguageText.Text = "简体中文";
                    break;
                case "en-US":
                default:
                    CurrentLanguageText.Text = "English";
                    break;
            }
        }

        private void OnLanguageChanged(object sender, System.EventArgs e)
        {
            UpdateLanguageDisplay();
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            ShowLanguageMenu();
        }

        private void ShowLanguageMenu()
        {
            var languageDialog = new LanguageSettingsView();
            var dialog = DialogEx.Create(
                EmbeddedLocalizationManager.Instance.GetString("Dialog_LanguageSettings"),
                languageDialog,
                DialogButtons.OK
            );

            dialog.ShowDialog();
        }
    }
}

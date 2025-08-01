using Duckie.Utils.Localization;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Duckie.Views.Common
{
    public enum DialogButtons
    {
        OK = 1,
        OKCancel = 2
    }

    public partial class DialogEx : Window
    {
        private DialogEx()
        {
            InitializeComponent();
        }

        private void SetContent(UIElement content)
        {
            ContentArea.Content = content;
        }

        private void SetButtons(DialogButtons buttons, Action<DialogEx> okHandler = null)
        {
            ButtonPanel.Children.Clear();

            switch (buttons)
            {
                case DialogButtons.OK:
                    AddButton(LocUtils.GetString("Btn_OK"), true, true, okHandler);
                    break;

                case DialogButtons.OKCancel:
                    AddButton(LocUtils.GetString("Btn_Cancel"), false, false);
                    AddButton(LocUtils.GetString("Btn_OK"), true, true, okHandler);
                    break;
            }
        }

        private void AddButton(string text, bool isPrimary, bool result, Action<DialogEx> clickHandler = null)
        {
            var button = new Button
            {
                Content = text,
                Width = 80,
                Height = 32,
                Margin = new Thickness(10, 0, 0, 0)
            };

            // 设置样式
            var styleKey = isPrimary ? "PrimaryButtonStyle" : "ModernButtonStyle";
            button.SetResourceReference(Button.StyleProperty, styleKey);

            // 设置点击事件
            button.Click += (s, e) =>
            {
                if (clickHandler != null)
                {
                    clickHandler(this);
                }
                else
                {
                    DialogResult = result;
                    Close();
                }
            };

            ButtonPanel.Children.Add(button);
        }

        public static DialogEx Create(string title, UIElement content, DialogButtons buttons = DialogButtons.OKCancel, Action<DialogEx> okHandler = null)
        {
            var dialog = new DialogEx
            {
                Title = title,
                Owner = App.MainWindow
            };

            dialog.SetContent(content);
            dialog.SetButtons(buttons, okHandler);

            return dialog;
        }
    }
}

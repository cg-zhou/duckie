using System;
using System.Windows;
using System.Windows.Controls;

namespace Duckie.Views.Common
{
    public enum DialogButtons
    {
        OK,
        OKCancel,
        YesNo
    }

    public partial class CommonDialog : Window
    {
        private CommonDialog()
        {
            InitializeComponent();
        }

        private void SetContent(UIElement content)
        {
            ContentArea.Content = content;
        }

        private void SetButtons(DialogButtons buttons, Action<CommonDialog> okHandler = null)
        {
            ButtonPanel.Children.Clear();

            switch (buttons)
            {
                case DialogButtons.OK:
                    AddButton("OK", true, true, okHandler);
                    break;

                case DialogButtons.OKCancel:
                    AddButton("Cancel", false, false);
                    AddButton("OK", true, true, okHandler);
                    break;
            }
        }

        private void AddButton(string text, bool isPrimary, bool result, Action<CommonDialog> clickHandler = null)
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

        public static CommonDialog Create(string title, UIElement content, DialogButtons buttons = DialogButtons.OKCancel, Action<CommonDialog> okHandler = null)
        {
            var dialog = new CommonDialog
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

using Duckie.Shared.Utils.Ui;
using System.Windows;
using System.Windows.Controls;

namespace Duckie.Shared.Views.Common;

public class TextEx : TextBox
{
    public TextEx()
    {
        IsReadOnly = true;
        BorderThickness = new Thickness(0);
        Background = ThemeUtils.NoneBrush;
    }
}
